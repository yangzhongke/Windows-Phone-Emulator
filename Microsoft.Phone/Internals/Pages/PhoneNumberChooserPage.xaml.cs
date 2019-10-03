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

namespace Microsoft.Phone.Internals.Pages
{
    public partial class PhoneNumberChooserPage : PhoneApplicationPage
    {
        public static string PhoneNumber { get; private set; }

        public PhoneNumberChooserPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            listBoxPeoples.DataContext = PeopleMgr.LoadPeoples();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            PhoneNumber = null;
            this.NavigationService.GoBack();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            PeopleItemInfo item = (PeopleItemInfo)listBoxPeoples.SelectedValue;
            if (item != null)
            {
                PhoneNumber = item.PhoneNumber;
                this.NavigationService.GoBack();
            }
        }
    }
}
