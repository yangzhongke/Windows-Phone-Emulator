namespace Microsoft.Phone.Notification
{
    using Microsoft.Phone;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Text;
    using System.Xml;

    internal class ShellObjectChannelInternals : IDisposable
    {
        private string channelData;
        private uint channelId;
        private bool disposed;
        private HttpNotificationChannel httpNotificationChannel;
        private SafeNativeMethods.NotificationType notificationType;

        public event EventHandler<NotificationEventArgs> NotificationReceived
        {
            [SecuritySafeCritical] add
            {
                bool flag = this.notificationReceivedEvent != null;
                this.notificationReceivedEvent = (EventHandler<NotificationEventArgs>) Delegate.Combine(this.notificationReceivedEvent, value);
                ShellObjectCheckAndAddCallback(this);
                bool fActivation = this.notificationReceivedEvent != null;
                if (fActivation != flag)
                {
                    SafeNativeMethods.ThrowExceptionFromHResult(SafeNativeMethods.NotificationAgentActivateChannel(this.channelId, fActivation), SafeNativeMethods.NotificationType.TokenNotificationType);
                }
            }
            [SecuritySafeCritical] remove
            {
                bool flag = this.notificationReceivedEvent != null;
                this.notificationReceivedEvent = (EventHandler<NotificationEventArgs>) Delegate.Remove(this.notificationReceivedEvent, value);
                ShellObjectCheckAndAddCallback(this);
                bool fActivation = this.notificationReceivedEvent != null;
                if (fActivation != flag)
                {
                    SafeNativeMethods.ThrowExceptionFromHResult(SafeNativeMethods.NotificationAgentActivateChannel(this.channelId, fActivation), SafeNativeMethods.NotificationType.TokenNotificationType);
                }
            }
        }

        private event EventHandler<NotificationEventArgs> notificationReceivedEvent;

        [SecuritySafeCritical]
        internal ShellObjectChannelInternals(HttpNotificationChannel channel, SafeNativeMethods.NotificationType type)
        {
            this.httpNotificationChannel = channel;
            this.notificationType = type;
        }

        [SecuritySafeCritical]
        internal ShellObjectChannelInternals(HttpNotificationChannel channel, SafeNativeMethods.NotificationType type, uint channelId)
        {
            this.httpNotificationChannel = channel;
            this.notificationType = type;
            this.channelId = channelId;
        }

        [SecuritySafeCritical]
        internal void Bind()
        {
            uint channelId = 0;
            bool fActivation = this.notificationReceivedEvent != null;
            fActivation = true;
            SafeNativeMethods.ThrowExceptionFromHResult(SafeNativeMethods.NotificationAgentBindChannel(this.notificationType, string.Empty, this.httpNotificationChannel.ChannelId, this.channelData, fActivation, out channelId), new InvalidOperationException(LocalizeString.PushNotificationChannelBindFailed), SafeNativeMethods.NotificationType.TokenNotificationType);
            if (channelId == 0)
            {
                throw new InvalidOperationException(LocalizeString.PushNotificationChannelBindFailed);
            }
            this.channelId = channelId;
            if (fActivation)
            {
                HttpNotificationChannel.AddCallback(channelId, new HttpNotificationChannel.NotificationChannelHandler(this.ChannelHandler));
            }
        }

        [SecurityCritical]
        internal void ChannelHandler(uint eventType, IntPtr blob1, uint blobSize1, IntPtr blob2, uint blobSize2)
        {
            if (eventType != 2)
            {
                throw new ArgumentException("", "eventType");
            }
            this.OnNotificationReceived(blob1, blobSize1);
        }

        [SecuritySafeCritical]
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        [SecurityCritical]
        internal void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if ((this.httpNotificationChannel != null) && disposing)
                {
                    HttpNotificationChannel.RemoveCallback(this.channelId);
                }
                this.channelId = 0;
                this.disposed = true;
            }
        }

        [SecuritySafeCritical]
        ~ShellObjectChannelInternals()
        {
            this.Dispose(false);
        }

        [SecuritySafeCritical]
        internal static ShellObjectChannelInternals Find(HttpNotificationChannel channel, string channelName, SafeNativeMethods.NotificationType type, uint associatedChannelId)
        {
            uint channelId = 0;
            int hr = SafeNativeMethods.NotificationAgentFindChannel(type, channelName, associatedChannelId, out channelId);
            SafeNativeMethods.ThrowExceptionFromHResult(hr, SafeNativeMethods.NotificationType.TokenNotificationType);
            if ((hr == 1) || (channelId == 0))
            {
                return null;
            }
            return new ShellObjectChannelInternals(channel, type, channelId);
        }

        [SecurityCritical]
        private void OnNotificationReceived(IntPtr blob, uint blobSize)
        {
            if (this.notificationReceivedEvent != null)
            {
                IDictionary<string, string> dictionary;
                byte[] destination = new byte[blobSize];
                Marshal.Copy(blob, destination, 0, (int) blobSize);
                ParseXMLToProperties(HttpNotificationChannel.TrimString(Encoding.UTF8.GetString(destination, 0, (int) blobSize)), out dictionary);
                this.notificationReceivedEvent(this.httpNotificationChannel, new NotificationEventArgs(dictionary));
            }
        }

        [SecurityCritical]
        internal static void ParseXMLToProperties(string xmlString, out IDictionary<string, string> propertyBag)
        {
            propertyBag = new Dictionary<string, string>();
            StringReader input = new StringReader(xmlString);
            XmlReaderSettings settings = new XmlReaderSettings {
                ConformanceLevel = ConformanceLevel.Document,
                IgnoreWhitespace = true,
                IgnoreComments = true
            };
            XmlReader reader2 = XmlReader.Create(input, settings);
            string key = string.Empty;
            while (reader2.Read())
            {
                switch (reader2.NodeType)
                {
                    case XmlNodeType.Element:
                    {
                        key = reader2.Name.ToString();
                        if (!reader2.IsEmptyElement)
                        {
                        }
                        continue;
                    }
                    case XmlNodeType.Text:
                    {
                        string str2 = reader2.Value.ToString();
                        propertyBag.Add(new KeyValuePair<string, string>(key, str2));
                        break;
                    }
                }
            }
        }

        [SecurityCritical]
        private static void ShellObjectCheckAndAddCallback(ShellObjectChannelInternals shellChannelInternals)
        {
            if (shellChannelInternals.notificationReceivedEvent != null)
            {
                HttpNotificationChannel.AddCallback(shellChannelInternals.ChannelId, new HttpNotificationChannel.NotificationChannelHandler(shellChannelInternals.ChannelHandler));
            }
            else
            {
                HttpNotificationChannel.RemoveCallback(shellChannelInternals.ChannelId);
                shellChannelInternals.channelId = 0;
            }
        }

        [SecuritySafeCritical]
        internal void Unbind()
        {
            SafeNativeMethods.ThrowExceptionFromHResult(SafeNativeMethods.NotificationAgentUnbindChannel(this.channelId), SafeNativeMethods.NotificationType.TokenNotificationType);
            HttpNotificationChannel.RemoveCallback(this.channelId);
            this.channelId = 0;
        }

        internal string ChannelData
        {
            set
            {
                this.channelData = value;
            }
        }

        internal uint ChannelId
        {
            get
            {
                return this.channelId;
            }
        }
    }
}

