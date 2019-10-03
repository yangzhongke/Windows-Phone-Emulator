namespace Microsoft.Phone.Notification
{
    using System;

    /// <summary>Contains the event data when a notification channel receives a push notification from the Microsoft Push Notification Service.</summary>
    public class HttpNotificationEventArgs : EventArgs
    {
        private HttpNotification notification;

        internal HttpNotificationEventArgs(HttpNotification notif)
        {
            this.notification = notif;
        }

        /// <summary>Contains the notification data sent by the web service. </summary>
        /// <returns>Returns 
        /// <see cref="T:Microsoft.Phone.Notification.HttpNotification" />
        /// .</returns>
        public HttpNotification Notification
        {
            get
            {
                return this.notification;
            }
        }
    }
}

