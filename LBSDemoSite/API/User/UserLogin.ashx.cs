using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LBSDemoSite.DAL;
using System.Data.OleDb;
using System.Data;

namespace LBSDemoSite.API.User
{
    /// <summary>
    /// UserLogin 的摘要说明
    /// </summary>
    public class UserLogin : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string username = context.Request["username"];
            string password = context.Request["password"];
            var dt = SqlHelper.ExecuteDataTable("select * from T_users where username=?",
                new OleDbParameter("username",username));
            if (dt.Rows.Count <= 0)
            {
                context.Response.Write("UserNotFound");
            }
            else if (dt.Rows.Count == 1)
            {
                DataRow row = dt.Rows[0];
                if (password == Convert.ToString(row["password"]))
                {
                    Guid guid = Guid.NewGuid();
                    SqlHelper.ExecuteNonQuery("update T_Users set LoginId=? where UserName=?",
                        new OleDbParameter("?", guid.ToString()),
                        new OleDbParameter("?", username));
                    context.Response.Write("OK" + guid);
                }
                else
                {
                    context.Response.Write("WrongPassword");
                }
            }
            else
            {
                throw new Exception("发现多个同用户名用户");
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