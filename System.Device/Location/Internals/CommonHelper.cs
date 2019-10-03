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
using System.Collections.Generic;

namespace System.Device.Location.Internals
{
    internal static class CommonHelper
    {
        /// <summary>
        /// 查找baseObj中类型为T的子节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseObj"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindDesendants<T>(this DependencyObject baseObj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(baseObj); i++)
            {
                var child = VisualTreeHelper.GetChild(baseObj, i);
                if (child is T)
                {
                    yield return (T)child;
                }
                foreach (var c in FindDesendants<T>(child))
                {
                    yield return (T)c;
                }
            }
        }

        private static double deg2rad(double deg)
        {
            return ((deg * 3.1415926535897931) / 180.0);
        }

        private static double rad2deg(double rad)
        {
            return ((rad / 3.1415926535897931) * 180.0);
        }

        /// <summary>
        /// Calculate the distance between tow points,from http://wp7gps.codeplex.com/
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <param name="lat2"></param>
        /// <param name="lon2"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static double distance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            double deg = lon1 - lon2;
            double d = (Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2))) + ((Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2))) * Math.Cos(deg2rad(deg)));
            d = Math.Acos(d);
            d = (rad2deg(d) * 60.0) * 1.1515;
            if (unit == 'K')
            {
                return (d * 1.609344);
            }
            if (unit == 'N')
            {
                return (d * 0.8684);
            }
            return d;
        }

        /// <summary>
        /// Calculate the Course（航向）,from http://wp7gps.codeplex.com/
        /// </summary>
        /// <param name="coordinate1"></param>
        /// <param name="coordinate2"></param>
        /// <returns></returns>
        public static double CalcCourse(Microsoft.Maps.MapControl.Location coordinate1, Microsoft.Maps.MapControl.Location coordinate2)
        {
            double d = coordinate1.Latitude * 0.017453;
            double num2 = coordinate2.Latitude * 0.017453;
            double a = (coordinate2.Longitude - coordinate1.Longitude) * 0.017453;
            double y = Math.Sin(a) * Math.Cos(num2);
            double x = (Math.Cos(d) * Math.Sin(num2)) - ((Math.Sin(d) * Math.Cos(num2)) * Math.Cos(a));
            return (((Math.Atan2(y, x) * 57.29578) + 360.0) % 360.0);
        }
    }
}
