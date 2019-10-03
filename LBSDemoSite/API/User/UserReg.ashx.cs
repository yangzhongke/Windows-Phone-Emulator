using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LBSDemoSite.DAL;
using System.Data.OleDb;

namespace LBSDemoSite.API.User
{
    /// <summary>
    /// UserReg 的摘要说明
    /// </summary>
    public class UserReg : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string username = context.Request["username"];
            string password = context.Request["password"];
            int count = Convert.ToInt32(SqlHelper.ExecuteScalar("select count(*) from T_users where UserName=?",
                new OleDbParameter("?", username)));
            if (count > 0)
            {
                context.Response.Write("UserNameExist");
            }
            else
            {
                SqlHelper.ExecuteNonQuery("insert into T_Users(UserName,[Password]) values(?,?)", new OleDbParameter("?", username), new OleDbParameter("?", password));
                context.Response.Write("OK");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}