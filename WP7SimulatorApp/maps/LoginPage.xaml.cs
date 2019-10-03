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
using WP7SimulatorApp.maps.ctrls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using System.IO;
using System.Media.Imaging;
using Microsoft.Xna.Framework.Framework.Media;
using System.Diagnostics;

namespace WP7SimulatorApp.maps
{
    public partial class LoginPage : PhoneApplicationPage
    {
        public LoginPage()
        {
            InitializeComponent();
            if (Debugger.IsAttached)
            {
                txtUserName.Text = "admin";
                txtPassword.Password = "123";
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            UserLoginAPI login = new UserLoginAPI(SessionManager.SessionState);
            login.OK += new EventHandler(login_OK);
            login.UserNotFound += new EventHandler(login_UserNotFound);
            login.WrongPassword += new EventHandler(login_WrongPassword);
            login.UnkownResponse += new EventHandler<ResponseEventArgs>(login_UnkownResponse);
            Helpers.ShowMsg("正在验证登录信息...");
            login.Login(txtUserName.Text.Trim(),
                txtPassword.Password.Trim());
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var queryStr = this.NavigationContext.QueryString;
            if (queryStr.ContainsKey("UserName"))
            {
                txtUserName.Text = queryStr["UserName"];
            }
            if (queryStr.ContainsKey("Password"))
            {
                txtPassword.Password = queryStr["Password"];
            }
        }

        void login_UserNotFound(object sender, EventArgs e)
        {
            Helpers.ShortVibrate();
            Helpers.ShowMsgDelayClose("用户名错误");
        }

        void login_UnkownResponse(object sender, ResponseEventArgs e)
        {
            this.HandleUnkownResponse(e);
        }

        void login_WrongPassword(object sender, EventArgs e)
        {
            Helpers.ShortVibrate();
            Helpers.ShowMsgDelayClose("密码错误");
        }

        void login_OK(object sender, EventArgs e)
        {
            this.Navigate("/maps/MapMainPage.xaml");
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Navigate("/maps/RegUserPage.xaml");
        }
    }
}
