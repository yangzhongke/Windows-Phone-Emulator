using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LBSDemoSite.DAL;
using System.Data.OleDb;

namespace LBSDemoSite.API.Company
{
    /// <summary>
    /// CommitCompany 的摘要说明
    /// </summary>
    public class CommitCompany : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //string accessToken = context.Request.Headers["AccessToken"];
            string name = context.Request["Name"];
            string typeId = context.Request["TypeId"];
            string latitude = context.Request["Latitude"];
            string longitude = context.Request["Longitude"];
            string telephone = context.Request["Telephone"];
            string description = context.Request["Description"];
            int compayId = SqlHelper.ExecuteIdentity("Insert into T_Companys([Name],TypeId,Latitude,Longitude,Telephone,[Description]) values(?,?,?,?,?,?)", 
                new OleDbParameter("?", name), new OleDbParameter("?", typeId),
                new OleDbParameter("?", latitude), new OleDbParameter("?", longitude),
                new OleDbParameter("?", telephone), new OleDbParameter("?", description));
            context.Response.Write("OK" + compayId);
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