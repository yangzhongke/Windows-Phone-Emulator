using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.IO;

namespace LBSDemoSite.API.Company
{
    /// <summary>
    /// UploadCompanyPhoto 的摘要说明
    /// </summary>
    public class UploadCompanyPhoto : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int companyId = Convert.ToInt32(context.Request["companyId"]);
            string jpgPath = HostingEnvironment.MapPath("~/Images/companies/"+companyId+".jpg");
            using(Stream fileStream = File.OpenWrite(jpgPath))
            {
                context.Request.InputStream.CopyTo(fileStream);
            }            
            context.Response.Write("OK");
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