using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;

namespace LBSDemoSite.DAL
{
    class SqlHelper
    {
        public static readonly string connstr = 
            ConfigurationManager.ConnectionStrings["dbconnstr"].ConnectionString;

        public static int ExecuteNonQuery(string cmdText,
            params OleDbParameter[] parameters)
        {
            using (OleDbConnection conn = new OleDbConnection(connstr))
            {
                conn.Open();
                using (OleDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = cmdText;
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static object ExecuteScalar(string cmdText,
            params OleDbParameter[] parameters)
        {
            using (OleDbConnection conn = new OleDbConnection(connstr))
            {
                conn.Open();
                using (OleDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = cmdText;
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteScalar();
                }
            }
        }

        public static int ExecuteIdentity(string cmdText,
            params OleDbParameter[] parameters)
        {
            using (OleDbConnection conn = new OleDbConnection(connstr))
            {
                conn.Open();
                using (OleDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = cmdText;
                    cmd.Parameters.AddRange(parameters);
                    cmd.ExecuteNonQuery();
                }
                using (OleDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "select @@identity";
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public static DataTable ExecuteDataTable(string cmdText,
            params OleDbParameter[] parameters)
        {
            using (OleDbConnection conn = new OleDbConnection(connstr))
            {
                conn.Open();
                using (OleDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = cmdText;
                    cmd.Parameters.AddRange(parameters);
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public static OleDbDataReader ExecuteDataReader(string cmdText,
            params OleDbParameter[] parameters)
        {
            OleDbConnection conn = new OleDbConnection(connstr);
            conn.Open();
            using (OleDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = cmdText;
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }
    }
}
