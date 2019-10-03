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

namespace WP7SimulatorApp.maps.API
{
    public class UserRegAPI : BaseAPI
    {
        public UserRegAPI(SessionState state)
            : base(state)
        {
            
        }

        public void Register(string username, string password)
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
                if (result == "OK")
                {
                    FireEvent(OK);
                }
                else if (result == "UserNameExist")
                {
                    FireEvent(UserNameExist);
                }
                else
                {
                    ReportUnkownResponse(e);
                }
            };
            wc.DownloadStringAsync("User/UserReg.ashx?"
                + "username=" + username + "&password=" + password);
        }

        public event EventHandler OK;
        public event EventHandler UserNameExist;    
    }
}
