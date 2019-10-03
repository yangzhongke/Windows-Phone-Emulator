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

namespace WP7SimulatorApp.maps.API
{
    public abstract class BaseAPI
    {
        protected SessionState SessionState
        {
            get;
            set;
        }
        public BaseAPI(SessionState state)
        {
            this.SessionState = state;
        }

        public event EventHandler<ResponseEventArgs> UnkownResponse;
        protected void ReportUnkownResponse(ResponseEventArgs e)
        {            
            if (UnkownResponse != null)
            {
                UnkownResponse(this, e);
            }
            if (System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Break();
            }
        }
        protected void FireEvent(EventHandler eventHandler)
        {
            if (eventHandler != null)
            {
                eventHandler(this, EventArgs.Empty);
            }
        }
    }
}
