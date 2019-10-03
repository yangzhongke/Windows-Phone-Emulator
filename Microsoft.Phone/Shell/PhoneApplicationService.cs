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

namespace Microsoft.Phone.Shell
{
    public class PhoneApplicationService : IApplicationService
    {
        static PhoneApplicationService()
        {
            Current = new PhoneApplicationService();
        }

        PhoneApplicationService()
        {
            State = new Dictionary<string, object>();
        }

        public void StartService(ApplicationServiceContext context)
        {
        }

        public void StopService()
        {
        }

        public static PhoneApplicationService Current
        {
            get;
            private set;
        }

        public IDictionary<string, object> State
        {
            get;
            private set;
        }
    }
}
