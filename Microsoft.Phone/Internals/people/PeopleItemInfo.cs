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

namespace Microsoft.Phone.Internals.people
{
    public class PeopleItemInfo
    {
        public PeopleItemInfo()
        {
            
        }

        public string Name
        {
            get;
            set;
        }

        public string PhoneNumber
        {
            get;
            set;
        }

        public string EmailAddress
        {
            get;
            set;
        }
    }
}
