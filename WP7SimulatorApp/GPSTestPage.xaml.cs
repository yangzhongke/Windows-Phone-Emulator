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
using System.Device.Location;

namespace WP7SimulatorApp
{
    public partial class GPSTestPage : PhoneApplicationPage
    {
        private GeoCoordinateWatcher geoWatcher;
        private double totalDistance = 0;
        private GeoCoordinate lastPosition = null;

        public GPSTestPage()
        {
            InitializeComponent();

            geoWatcher = new GeoCoordinateWatcher();
            geoWatcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(geoWatcher_PositionChanged);
        }

        private static double deg2rad(double deg)
        {
            return ((deg * 3.1415926535897931) / 180.0);
        }

        private static double rad2deg(double rad)
        {
            return ((rad / 3.1415926535897931) * 180.0);
        }

        private static double distance(double lat1, double lon1, double lat2, double lon2)
        {
            double deg = lon1 - lon2;
            double d = (Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2))) + ((Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2))) * Math.Cos(deg2rad(deg)));
            d = Math.Acos(d);
            d = (rad2deg(d) * 60.0) * 1.1515;
            return d;
        }

        void geoWatcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            //listBox1.Items.Add(e.Position.Location.Speed);
            double course = e.Position.Location.Course;
            if (course < 20 || course > 340)
            {
                button1.Content = "一路向北";
            }
            else if (course > 70 && course < 110)
            {
                button1.Content = "大河向东流";
            }
            else if (course > 160 && course < 200)
            {
                button1.Content = "雁南飞";
            }
            else if (course > 250 && course < 290)
            {
                button1.Content = "西天取经";
            }
            if (lastPosition == null)
            {
                lastPosition = e.Position.Location;
                return;
            }
            double d = distance(lastPosition.Latitude,lastPosition.Longitude,
                e.Position.Location.Latitude, e.Position.Location.Longitude);
            totalDistance += d;
            textBlock1.Text = totalDistance.ToString();

            lastPosition = e.Position.Location;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            geoWatcher.Start();
        }
    }
}
