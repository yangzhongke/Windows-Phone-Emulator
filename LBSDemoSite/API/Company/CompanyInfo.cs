using System;
using System.Net;

namespace LBSDemoSite.API.Company
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
