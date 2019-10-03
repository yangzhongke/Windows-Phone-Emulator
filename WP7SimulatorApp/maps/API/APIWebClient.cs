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
using System.IO;
using System.Windows.Threading;
using Microsoft.Phone.Internals;

namespace WP7SimulatorApp.maps.API
{
    public class APIWebClient
    {
        private WebClient webClient = new WebClient();

        private SessionState state;
        public APIWebClient(SessionState state)
        {
            this.state = state;
        }

        public void UploadDataAsync(string apiUrl,byte[] data)
        {
            string url = "http://127.0.0.1:45671/API/" + apiUrl + "&randid=" + Guid.NewGuid();
            webClient.WriteStreamClosed += (s2, e2) => {
                ResponseEventArgs args = new ResponseEventArgs();
                args.Error = e2.Error;
                //args.RequestUrl = url;//闭包，所以可能有错
                if (UploadDataAsyncCompleted != null)
                {
                    UploadDataAsyncCompleted(this, args);
                }
            };
            webClient.OpenWriteCompleted += (s2, e2) =>
            {
                using (e2.Result)
                {
                    byte[] dataToUpload = (byte[])e2.UserState;
                    e2.Result.Write(dataToUpload, 0, dataToUpload.Length);
                }
                
            };
            webClient.OpenWriteAsync(new Uri(url),"POST",data);
        }

        public event EventHandler<ResponseEventArgs> UploadDataAsyncCompleted;

        public void DownloadStringAsync(string apiUrl)
        {
            if (!string.IsNullOrWhiteSpace(state.AccessToken))
            {
                webClient.Headers["AccessToken"] = state.AccessToken;
            }
            DownloadStringAsync(apiUrl, null);
        }
        public void DownloadStringAsync(string apiUrl, object userToken)
        {
            string url = "http://127.0.0.1:45671/API/" + apiUrl + "&randid=" + Guid.NewGuid();
            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += (sender, e) =>
            {
                ResponseEventArgs args = new ResponseEventArgs();
                if (e.Error != null)
                {
                    args.Error = e.Error;
                }
                else
                {
                    args.Result = e.Result;
                }
                args.RequestUrl = (string)e.UserState;
                if (DownloadStringCompleted != null)
                {
                    DownloadStringCompleted(this, args);
                }
            };
            wc.DownloadStringAsync(new Uri(url), url);
        }

        public event EventHandler<ResponseEventArgs> DownloadStringCompleted;
    }

    public class ResponseEventArgs : EventArgs
    {
        public string Result { get; set; }
        public Exception Error { get; set; }
        public string RequestUrl { get; set; }
    }
}
