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
using System.Reflection;
using System.IO;
using System.Xml.Linq;
using System.Linq;

namespace Microsoft.Phone.Internals
{
    //读取WMappManifest.xml的类
    internal class WMAppConfiguration
    {
        public static WMAppConfiguration Instance = new WMAppConfiguration();

        private XDocument xmldoc;

        private WMAppConfiguration() 
        {
            Assembly appAsm = Application.Current.GetType().Assembly;//当前应用的Assembly
            using (Stream stream = AppHelper.GetAssemblyResourceStream(appAsm, "Properties.WMappManifest.xml"))
            {
                xmldoc = XDocument.Load(stream);
            }
        }
        
        //起始页
        public string NavigationPage
        {
            get
            {
                var defaultTask = xmldoc.Descendants("DefaultTask").Single();
                string navigationPageUrl = defaultTask.Attribute("NavigationPage").Value;
                return navigationPageUrl;
            }
        }

        //应用的标题
        public string AppTitle
        {
            get
            {
                var app = xmldoc.Descendants("App").Single();
                return app.Attribute("Title").Value;
            }
        }

        //图标路径
        public string AppIconPath
        {
            get
            {
                var iconPath = xmldoc.Descendants("IconPath").Single();
                return iconPath.Value;
            }
        }

        //Pin To Start的图标路径
        public string TemplateType5BackgroundImageURI
        {
            get
            {
                var imgURI = xmldoc.Descendants("TemplateType5").Descendants("BackgroundImageURI").Single().Value;
                return imgURI;
            }
        }

        //Pin To Start的图标Title
        public string TemplateType5Title
        {
            get
            {
                var title = xmldoc.Descendants("TemplateType5").Descendants("Title").Single().Value;
                return title;
            }
        }
    }
}
