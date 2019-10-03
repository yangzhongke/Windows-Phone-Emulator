using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using WP7SimulatorApp.maps.API;
using WP7SimulatorApp.maps.API.Models;
using Microsoft.Phone.Shell;

namespace WP7SimulatorApp.maps
{
    public partial class CommitCompanyPage : PhoneApplicationPage
    {
        public CommitCompanyPage()
        {
            InitializeComponent();

            foreach (var kvp in CompanyHelper.dict)
            {
                //listPickerType.Items.Add(new ListBoxItem() { Content = kvp.Value, Tag = kvp.Key });
                listPickerType.Items.Add(kvp);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            CommitCompanyAPI api = new CommitCompanyAPI(SessionManager.SessionState);
            api.OK += new EventHandler<CommitCompanyEventArgs>(api_OK);
            api.UnkownResponse += new EventHandler<ResponseEventArgs>(api_UnkownResponse);

            var queryStr = this.NavigationContext.QueryString;
            double latitude = Convert.ToDouble(queryStr["latitude"]);
            double longitude = Convert.ToDouble(queryStr["longitude"]);

            CompanyInfo info = new CompanyInfo();
            info.Name = txtName.Text;
            KeyValuePair<int, string> lpiType = (KeyValuePair<int, string>)listPickerType.SelectedItem;
            info.TypeId = Convert.ToInt32(lpiType.Key);
            info.Telephone = txtTelPhone.Text;
            info.Description = txtDesc.Text;
            info.Latitude = latitude;
            info.Longitude = longitude;
            api.Commit(info);
        }

        void api_UnkownResponse(object sender, ResponseEventArgs e)
        {
            this.HandleUnkownResponse(e);
        }

        void api_OK(object sender, CommitCompanyEventArgs e)
        {
            Helpers.ShowMsgDelayClose("谢谢，提交成功");
            //this.NavigationService.GoBack();
            //todo:进入拍照页面
            this.Navigate("/maps/UploadCompanyPhotoPage.xaml?id="+e.CompanyId);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
