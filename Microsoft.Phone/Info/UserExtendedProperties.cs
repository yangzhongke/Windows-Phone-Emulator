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

namespace Microsoft.Phone.Info
{
    public static class UserExtendedProperties
    {
        private static Dictionary<string, object> data = new Dictionary<string, object>();

        static UserExtendedProperties()
        {
            data["ANID"] = null;
        }

        public static object GetValue(string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException();
            }
            if (data.ContainsKey(propertyName))
            {
                return data[propertyName];
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }
        public static bool TryGetValue(string propertyName, out object propertyValue)
        {
            return data.TryGetValue(propertyName, out propertyValue);
        }
    }
}
