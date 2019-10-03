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
using System.Windows.Media.Imaging;
using WP7SimulatorApp.maps.API.Models;
using Microsoft.Phone.Tasks;

namespace WP7SimulatorApp.maps
{
    public partial class CompanyDetailPage : PhoneApplicationPage
    {
        public static CompanyInfo CurrentCompanyInfo;
        public CompanyDetailPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //int id = Convert.ToInt32(this.NavigationContext.QueryString["id"]);
            //imagePhoto.Source = new BitmapImage(new Uri("http://127.0.0.1:45671/Images/companies/" + id + ".jpg"));

            imagePhoto.Source = new BitmapImage(new Uri("http://127.0.0.1:45671/Images/companies/" + CurrentCompanyInfo.Id + ".jpg"));
            btnDial.Content = CurrentCompanyInfo.Telephone;
            PageTitle.Text = CurrentCompanyInfo.Name;
            txtDesc.Text = CurrentCompanyInfo.Description;
        }

        private void btnDial_Click(object sender, RoutedEventArgs e)
        {
            PhoneCallTask task = new PhoneCallTask();
            task.DisplayName = CurrentCompanyInfo.Name;
            task.PhoneNumber = CurrentCompanyInfo.Telephone;
            task.Show();
        }
    }
}
