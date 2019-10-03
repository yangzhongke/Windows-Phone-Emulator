namespace Microsoft.Phone.Notification
{
    using System;
    using System.Collections.Generic;

    /// <summary>Contains the event data for when a notification channel receives a tile or toast notification.</summary>
    public class NotificationEventArgs : EventArgs
    {
        private IDictionary<string, string> collection;

        internal NotificationEventArgs(IDictionary<string, string> collection)
        {
            this.collection = collection;
        }

        /// <summary>A Dictionary type that contains the toast or tile data sent in the notification. This data is always presented as the name-value pair (&lt;string&gt;, &lt;string&gt;).</summary>
        /// <returns>Returns 
        /// <see cref="T:System.Collections.Generic.IDictionary`2" />
        /// .</returns>
        public IDictionary<string, string> Collection
        {
            get
            {
                return this.collection;
            }
        }
    }
}

