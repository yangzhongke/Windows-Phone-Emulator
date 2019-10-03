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
    public class UploadCompanyPhotoAPI: BaseAPI
    {
        public UploadCompanyPhotoAPI(SessionState state)
            : base(state)
        {            
        }

        public void Upload(int companyId,byte[] data)
        {
            APIWebClient wc = new APIWebClient(SessionState);
            wc.UploadDataAsync("Company/UploadCompanyPhoto.ashx?companyId=" + companyId, data);
            wc.UploadDataAsyncCompleted += new EventHandler<ResponseEventArgs>(wc_UploadDataAsyncCompleted);
        }

        void wc_UploadDataAsyncCompleted(object sender, ResponseEventArgs e)
        {
            if (e.Error != null)
            {
                ReportUnkownResponse(e);
            }
            else
            {
                FireEvent(OK);
            }            
        }

        public event EventHandler OK;
    }
}
