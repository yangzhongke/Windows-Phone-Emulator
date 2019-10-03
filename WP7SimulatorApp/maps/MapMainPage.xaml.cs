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
using System.Windows.Threading;
using Microsoft.Phone.Controls.Maps;
using System.Device.Location;
using Microsoft.Maps.MapControl;
using WP7SimulatorApp.maps.API;
using System.Runtime.Serialization.Json;
using WP7SimulatorApp.maps.API.Models;

namespace WP7SimulatorApp.maps
{
    public partial class MapMainPage : PhoneApplicationPage
    {
        private double latitude= double.NaN;
        private double longitude = double.NaN;
        public MapMainPage()
        {
            InitializeComponent();
            map.Mode = new ChineseMode();
            //map.Mode = new Microsoft.Phone.Controls.Maps.AerialMode();
            Location locItcast = new Location(40.039,116.313);
            map.SetView(locItcast, 16);

            GeoCoordinateWatcher gw = new GeoCoordinateWatcher();            
            gw.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(gw_PositionChanged);
            gw.Start();            
        }

        void gw_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            latitude = e.Position.Location.Latitude;
            longitude = e.Position.Location.Longitude;
            map.SetView(new Microsoft.Maps.MapControl.Location(latitude, longitude), map.ZoomLevel);

            //设置箭头的显示坐标
            MapLayer.SetPosition(arrowDir, new Microsoft.Maps.MapControl.Location(latitude, longitude));
            MapLayer.SetPositionOrigin(arrowDir, PositionOrigin.Center);
            //设置箭头的旋转方向
            arrowDirTransform.Angle = e.Position.Location.Course;

            //移动覆盖圈
            MapLayer.SetPosition(arrowDirCircle, new Microsoft.Maps.MapControl.Location(latitude, longitude));
            MapLayer.SetPositionOrigin(arrowDirCircle, PositionOrigin.Center);

            ShowNearBy();
        }

        private void ShowNearBy()
        {
            if (double.IsNaN(longitude) || double.IsNaN(latitude))
            {
                return;
            }
            GetNearByCompaniesAPI api = new GetNearByCompaniesAPI(SessionManager.SessionState);
            api.OK += new EventHandler<GetNearByCompaniesEventArgs>(api_OK);
            api.UnkownResponse += new EventHandler<ResponseEventArgs>(api_UnkownResponse);
            api.Query(latitude, longitude);
        }

        private void btnGoCheckIn_Click(object sender, RoutedEventArgs e)
        {
            if (double.IsNaN(longitude) || double.IsNaN(latitude))
            {
                Helpers.ShowMsgDelayClose("无法定位你的位置，因此无法提交商家信息！");
            }
            else
            {
                this.Navigate("/maps/CommitCompanyPage.xaml?latitude=" + latitude + "&longitude=" + longitude);
            }            
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //todo:缓存数据在独立存储中，这样就不会每次都进入网络获取数据了，降低网络流量
            ShowNearBy();            
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
        }

        void api_UnkownResponse(object sender, ResponseEventArgs e)
        {
            this.HandleUnkownResponse(e);
        }

        void api_OK(object sender, GetNearByCompaniesEventArgs e)
        {
            foreach (var company in e.Data)
            {
                foreach (var oldPuspin in map.Children.OfType<Pushpin>())
                {
                    //如果商家已经显示在地图上了，则不再重复画
                    CompanyInfo oldCompanyInfo = oldPuspin.Tag as CompanyInfo;
                    if (oldCompanyInfo.Id == company.Id)
                    {
                        continue;
                    }
                }
                Pushpin pushpin = new Pushpin();
                string typeName = CompanyHelper.dict[company.TypeId];
                pushpin.Content = typeName[0];
                pushpin.Location = new Location(company.Latitude,company.Longitude);
                pushpin.Tag = company;
                map.Children.Add(pushpin);
                pushpin.MouseLeftButtonDown += (s, ev) => {
                    //todo:Navigate到一个查看商家信息的页面，能够拨打电话、添加评论等
                    CompanyInfo c = ((Pushpin)s).Tag as CompanyInfo;
                    CompanyDetailPage.CurrentCompanyInfo = c;
                    this.Navigate("/maps/CompanyDetailPage.xaml?id="+c.Id);
                    //MessageBox.Show(c.Name);
                };
            }            
        }
    }
}
