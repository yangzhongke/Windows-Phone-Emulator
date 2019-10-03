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
using Microsoft.Phone.Internals.people;
using Microsoft.Phone.Tasks;

namespace Microsoft.Phone.Internals.Pages
{
    public partial class SavePhoneNumberPage : PhoneApplicationPage
    {
        public static TaskResult TaskResult
        {
            get;
            private set;
        }

        public SavePhoneNumberPage()
        {
            InitializeComponent();
           
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string phoneNumber = NavigationContext.QueryString["phoneNumber"];
            txtNumber.Text = phoneNumber;
            TaskResult = Tasks.TaskResult.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var peoples = PeopleMgr.LoadPeoples();
            peoples.Add(new PeopleItemInfo() { Name=txtName.Text,PhoneNumber=txtNumber.Text});
            PeopleMgr.SavePeoples(peoples);
            TaskResult = TaskResult.OK;
            this.NavigationService.GoBack();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            TaskResult = TaskResult.Cancel;
            this.NavigationService.GoBack();            
        }
    }
}
