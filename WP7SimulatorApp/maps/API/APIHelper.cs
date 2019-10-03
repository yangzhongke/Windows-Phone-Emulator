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
using System.Runtime.Serialization.Json;
using System.Text;
using System.IO;

namespace WP7SimulatorApp.maps.API
{
    public class APIHelper
    {
        public static T ParserJson<T>(string content)
        {
            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(T));
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                object obj = json.ReadObject(stream);
                return (T)obj;
            }     
        }
    }
}
