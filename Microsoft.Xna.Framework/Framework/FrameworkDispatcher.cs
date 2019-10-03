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

namespace Microsoft.Xna.Framework.Framework
{
    public static class FrameworkDispatcher
    {
        static FrameworkDispatcher()
        {
            IsUpdateCalled = false;
        }

        internal static bool IsUpdateCalled
        {
            get;
            private set;
        }

        internal static void CheckIsUpdateCalled()
        {
            if (!IsUpdateCalled)
            {
                string msg = @"FrameworkDispatcher.Update has not been called. Regular FrameworkDispatcher.
                Update calls are necessary for fire and forget sound effects 
                        and framework events to function correctly. 
                        See http://go.microsoft.com/fwlink/?LinkId=193853 for details.";
                throw new InvalidOperationException(msg);
            }
        }

        public static void Update()
        {
            IsUpdateCalled = true;
        }
    }
}
