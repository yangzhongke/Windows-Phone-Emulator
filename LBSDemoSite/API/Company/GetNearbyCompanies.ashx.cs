using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LBSDemoSite.DAL;
using System.Data.OleDb;
using System.Data;
using System.Runtime.Serialization.Json;

namespace LBSDemoSite.API.Company
{
    /// <summary>
    /// GetNearbyCompanies 的摘要说明
    /// </summary>
    public class GetNearbyCompanies : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string latitude = context.Request["Latitude"];
            string longitude = context.Request["Longitude"];
            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Companys where ABS(Latitude-?)<0.01 and ABS(Longitude-?)<0.01",
                new OleDbParameter("?", latitude),
                new OleDbParameter("?", longitude));
            CompanyInfo[] infos = new CompanyInfo[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count;i++ )
            {
                DataRow row = dt.Rows[i];
                CompanyInfo info = new CompanyInfo();
                info.Id = Convert.ToInt32(row["Id"]);
                info.Description = Convert.ToString(row["Description"]);
                info.Latitude = Convert.ToDouble(row["Latitude"]);
                info.Longitude = Convert.ToDouble(row["Longitude"]);
                info.Name = Convert.ToString(row["Name"]);
                info.Telephone = Convert.ToString(row["Telephone"]);
                info.TypeId = Convert.ToInt32(row["TypeId"]);

                infos[i] = info;
            }
            DataContractJsonSerializer json = new DataContractJsonSerializer(infos.GetType());
            json.WriteObject(context.Response.OutputStream, infos);
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