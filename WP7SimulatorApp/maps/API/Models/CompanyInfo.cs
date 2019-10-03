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

namespace WP7SimulatorApp.maps.API.Models
{
    public class CompanyInfo
    {
        public int Id { get; set; }
        public string Name{get;set;}
        public int TypeId{get;set;}
        public double Latitude{get;set;}
        public double Longitude { get; set; }
        public string Telephone{get;set;}
        public string Description { get; set; }
    }
}
