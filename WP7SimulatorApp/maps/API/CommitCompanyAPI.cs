using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using WP7SimulatorApp.maps.API.Models;

namespace WP7SimulatorApp.maps.API
{
    public class CommitCompanyAPI: BaseAPI
    {
        public CommitCompanyAPI(SessionState state)
            : base(state)
        {
            
        }

        public void Commit(CompanyInfo info)
        {
            APIWebClient wc = new APIWebClient(SessionState);
            wc.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    ReportUnkownResponse(e);
                    return;
                }
                string result = e.Result;
                if (result.StartsWith("OK"))
                {
                    string id = result.Substring(2);
                    var args = new CommitCompanyEventArgs();
                    args.CompanyId = Convert.ToInt32(id);
                    if (OK != null)
                    {
                        OK(this, args);
                    }
                }
                else
                {
                    ReportUnkownResponse(e);
                }
            };
            wc.DownloadStringAsync("Company/CommitCompany.ashx?"
                + "Name=" + info.Name + "&TypeId=" + info.TypeId
                + "&Latitude=" + info.Latitude + "&Longitude=" + info.Longitude
                + "&Telephone=" + info.Telephone + "&Description=" + info.Description);
        }

        public event EventHandler<CommitCompanyEventArgs> OK;
    }

    public class CommitCompanyEventArgs : EventArgs
    {
        public int CompanyId { get; set; }
    }
}
