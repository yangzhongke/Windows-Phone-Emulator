namespace Microsoft.Phone.Notification
{
    using System;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;

    /// <summary>Contains the raw notification data that has been passed to the application from the Microsoft Push Notification Service.</summary>
    public class HttpNotification
    {
        /// <summary>The contents of the raw notification data.</summary>
        /// <returns>Returns 
        /// <see cref="T:System.IO.Stream" />
        /// .</returns>
        public Stream Body { get; internal set; }

        /// <summary>The INotificationChannel object that the raw notification is associated with.</summary>
        /// <returns>Returns 
        /// <see cref="T:Microsoft.Phone.Notification.INotificationChannel" />
        /// .</returns>
        public HttpNotificationChannel Channel { get; internal set; }

        /// <summary>The HTTP headers associated with the raw notification.</summary>
        /// <returns>Returns 
        /// <see cref="T:System.Net.WebHeaderCollection" />
        /// .</returns>
        public WebHeaderCollection Headers { get; internal set; }
    }
}

