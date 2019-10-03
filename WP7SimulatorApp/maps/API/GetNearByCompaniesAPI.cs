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
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

namespace WP7SimulatorApp.maps.API
{
    public class GetNearByCompaniesAPI: BaseAPI
    {
        public GetNearByCompaniesAPI(SessionState state)
            : base(state)
        {
            
        }

        public void Query(double latitude, double longitude)
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
                CompanyInfo[] data = APIHelper.ParserJson<CompanyInfo[]>(result);
                if (OK != null)
                {
                    var args = new GetNearByCompaniesEventArgs();
                    args.Data = data;
                    OK(this, args);
                }             
            };
            wc.DownloadStringAsync("Company/GetNearbyCompanies.ashx?Latitude=" + latitude + "&Longitude=" + longitude);
        }

        public event EventHandler<GetNearByCompaniesEventArgs> OK;
    }

    public class GetNearByCompaniesEventArgs:EventArgs
    {
        public CompanyInfo[] Data{get;set;}
    }
}
