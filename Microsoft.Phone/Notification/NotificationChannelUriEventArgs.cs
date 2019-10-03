namespace Microsoft.Phone.Notification
{
    using System;

    /// <summary>Contains the event data when a notification channel receives the channel’s URI.</summary>
    public class NotificationChannelUriEventArgs : EventArgs
    {
        private Uri uri;

        internal NotificationChannelUriEventArgs(Uri inputUri)
        {
            this.uri = inputUri;
        }

        /// <summary>Contains the URI associated with the notification channel.</summary>
        /// <returns>Returns 
        /// <see cref="T:System.Uri" />
        /// .</returns>
        public Uri ChannelUri
        {
            get
            {
                return this.uri;
            }
        }
    }
}

