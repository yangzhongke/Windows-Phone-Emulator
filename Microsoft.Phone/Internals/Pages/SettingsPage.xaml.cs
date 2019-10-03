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
using System.Device.Location.Internals;

namespace Microsoft.Phone.Internals.Pages
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        private bool isCancel = false;
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            isCancel = false;
            NavigationService.GoBack();
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            new MapSettings().TileServerUriFormat = txtTileServerUri.Text;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            txtTileServerUri.Text = new MapSettings().TileServerUriFormat;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            isCancel = true;
            NavigationService.GoBack();
        }

        private void miCNBingMap_Click(object sender, EventArgs e)
        {
            txtTileServerUri.Text = 
                "http://r3.tiles.ditu.live.com/tiles/r{QuadKey}.png?g=47";
        }

        private void miItcastTile_Click(object sender, EventArgs e)
        {
            txtTileServerUri.Text =
                "http://localhost:8080/{QuadKey}.png";
        }
    }
}
