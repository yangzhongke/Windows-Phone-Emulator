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
    public class UserLoginAPI : BaseAPI
    {
        public UserLoginAPI(SessionState state):base(state)
        {
            
        }
        public void Login(string username, string password)
        {
            APIWebClient wc = new APIWebClient(SessionState);
            wc.DownloadStringCompleted += (sender, e) => {
                if (e.Error != null)
                {
                    ReportUnkownResponse(e);
                    return;
                }
                string result = e.Result;
                if (result.StartsWith("OK"))
                {
                    string accessToken = result.Substring(2);
                    SessionState.AccessToken = accessToken;
                    FireEvent(OK);
                }
                else if (result == "UserNotFound")
                {
                    FireEvent(UserNotFound);
                }
                else if (result == "WrongPassword")
                {
                    FireEvent(WrongPassword);
                }
                else
                {
                    ReportUnkownResponse(e);
                }
            };
            wc.DownloadStringAsync("User/UserLogin.ashx?"
                +"username="+username+"&password="+password);
        }

        public event EventHandler OK;
        public event EventHandler UserNotFound;
        public event EventHandler WrongPassword;        
    }
}
