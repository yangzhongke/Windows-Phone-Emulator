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
using Microsoft.Phone.Shell;

namespace WP7SimulatorApp.maps
{
    public partial class RegUserPage : PhoneApplicationPage
    {
        public RegUserPage()
        {
            InitializeComponent();
        }

        private void btnCreateUser_Click(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Password != txtPassword2.Password)
            {
                Helpers.ShowMsgDelayClose("两次密码不一致！");
                return;
            }
            UserRegAPI regAPI = new UserRegAPI(SessionManager.SessionState);
            regAPI.OK += new EventHandler(regAPI_OK);
            regAPI.UserNameExist += new EventHandler(regAPI_UserNameExist);
            regAPI.UnkownResponse += new EventHandler<ResponseEventArgs>(regAPI_UnkownResponse);
            regAPI.Register(txtUserName.Text, txtPassword.Password);
        }

        void regAPI_UnkownResponse(object sender, ResponseEventArgs e)
        {
            this.HandleUnkownResponse(e);
        }

        void regAPI_UserNameExist(object sender, EventArgs e)
        {
            Helpers.ShowMsgDelayClose("用户名已经存在！");
        }

        void regAPI_OK(object sender, EventArgs e)
        {
            Helpers.ShowMsgDelayClose("注册成功");
            this.Navigate("/maps/LoginPage.xaml?UserName=" + txtUserName.Text + "&Password=" + txtPassword.Password);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            var state = PhoneApplicationService.Current.State;
            state["RegUserPage.UserName"] = txtUserName.Text;
            state["RegUserPage.Password"] = txtPassword.Password;
            state["RegUserPage.Password2"] = txtPassword2.Password;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var state = PhoneApplicationService.Current.State;
            if (state.ContainsKey("RegUserPage.UserName"))
            {
                txtUserName.Text = (string)state["RegUserPage.UserName"];
            }
            if (state.ContainsKey("RegUserPage.Password"))
            {
                txtPassword.Password = (string)state["RegUserPage.Password"];
            }
            if (state.ContainsKey("RegUserPage.Password2"))
            {
                txtPassword2.Password = (string)state["RegUserPage.Password2"];
            }
        }
    }
}
