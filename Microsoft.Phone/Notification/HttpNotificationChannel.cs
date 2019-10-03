namespace Microsoft.Phone.Notification
{
    using Microsoft.Phone;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Text;
    using System.Threading;

    /// <summary>Creates a notification channel between the Microsoft Push Notification Service and the Push Client and creates a new subscription for raw notifications.</summary>
    public class HttpNotificationChannel : IDisposable
    {
        private static Dictionary<uint, NotificationChannelHandler> callbackDictionary = new Dictionary<uint, NotificationChannelHandler>();
        private uint channelId;
        private bool disposed;
        internal static int instanceCount;
        private string name;
        private static SafeNativeMethods.ReverseHandler reverseHandler;
        private string serviceName;
        internal ShellObjectChannelInternals shellEntryPointDefaultObject;
        internal ShellObjectChannelInternals shellNotificationDefaultObject;
        private Uri uri;

        /// <summary>Returns the URI associated with the notification channel.</summary>
        public event EventHandler<NotificationChannelUriEventArgs> ChannelUriUpdated
        {
            [SecuritySafeCritical] add
            {
                this.channelUriUpdatedEvent = (EventHandler<NotificationChannelUriEventArgs>) Delegate.Combine(this.channelUriUpdatedEvent, value);
                CheckAndAddCallback(this);
            }
            [SecuritySafeCritical] remove
            {
                this.channelUriUpdatedEvent = (EventHandler<NotificationChannelUriEventArgs>) Delegate.Remove(this.channelUriUpdatedEvent, value);
                CheckAndAddCallback(this);
            }
        }

        private event EventHandler<NotificationChannelUriEventArgs> channelUriUpdatedEvent;

        /// <summary>This event is raised when something unexpected happens when using the <see cref="T:Microsoft.Phone.Notification.HttpNotificationChannel" /> class.</summary>
        public event EventHandler<NotificationChannelErrorEventArgs> ErrorOccurred
        {
            [SecuritySafeCritical] add
            {
                this.errorOccurredEvent = (EventHandler<NotificationChannelErrorEventArgs>) Delegate.Combine(this.errorOccurredEvent, value);
                CheckAndAddCallback(this);
            }
            [SecuritySafeCritical] remove
            {
                this.errorOccurredEvent = (EventHandler<NotificationChannelErrorEventArgs>) Delegate.Remove(this.errorOccurredEvent, value);
                CheckAndAddCallback(this);
            }
        }

        private event EventHandler<NotificationChannelErrorEventArgs> errorOccurredEvent;

        /// <summary>The event that is raised when the application receives a raw notification.</summary>
        public event EventHandler<HttpNotificationEventArgs> HttpNotificationReceived
        {
            [SecuritySafeCritical] add
            {
                bool flag = this.httpNotificationReceivedEvent != null;
                this.httpNotificationReceivedEvent = (EventHandler<HttpNotificationEventArgs>) Delegate.Combine(this.httpNotificationReceivedEvent, value);
                CheckAndAddCallback(this);
                bool fActivation = this.httpNotificationReceivedEvent != null;
                if (fActivation != flag)
                {
                    SafeNativeMethods.ThrowExceptionFromHResult(SafeNativeMethods.NotificationAgentActivateChannel(this.channelId, fActivation));
                }
            }
            [SecuritySafeCritical] remove
            {
                bool flag = this.httpNotificationReceivedEvent != null;
                this.httpNotificationReceivedEvent = (EventHandler<HttpNotificationEventArgs>) Delegate.Remove(this.httpNotificationReceivedEvent, value);
                CheckAndAddCallback(this);
                bool fActivation = this.httpNotificationReceivedEvent != null;
                if (fActivation != flag)
                {
                    SafeNativeMethods.ThrowExceptionFromHResult(SafeNativeMethods.NotificationAgentActivateChannel(this.channelId, fActivation));
                }
            }
        }

        private event EventHandler<HttpNotificationEventArgs> httpNotificationReceivedEvent;

        /// <summary>The event that is raised when the application receives a toast notification.</summary>
        public event EventHandler<NotificationEventArgs> ShellToastNotificationReceived
        {
            [SecuritySafeCritical] add
            {
                this.shellNotificationDefaultObject.NotificationReceived += value;
            }
            [SecuritySafeCritical] remove
            {
                this.shellNotificationDefaultObject.NotificationReceived -= value;
            }
        }

        /// <summary>Creates a notification channel. The application will identify the notification channel using the <paramref name="channelName" /> value.</summary>
        /// <param name="channelName">The name the application uses to identify the notification channel instance.</param>
        [SecuritySafeCritical]
        public HttpNotificationChannel(string channelName)
        {
            this.name = channelName;
            this.serviceName = string.Empty;
            this.shellEntryPointDefaultObject = new ShellObjectChannelInternals(this, SafeNativeMethods.NotificationType.TokenNotificationType);
            this.shellNotificationDefaultObject = new ShellObjectChannelInternals(this, SafeNativeMethods.NotificationType.ToastNotificationType);
            Initialize();
        }

        /// <summary>Creates a notification channel. Use this constructor with an authenticated web service. The application will identify the notification channel using the <paramref name="channelName" /> value. The <paramref name="serviceName" /> value identifies the Subject Name of the authenticated web service's certificate. </summary>
        /// <param name="channelName">The name the application uses to identify the notification channel instance.</param>
        /// <param name="serviceName">The name that the web service uses to associate itself with the Push Notification Service.</param>
        [SecuritySafeCritical]
        public HttpNotificationChannel(string channelName, string serviceName)
        {
            this.name = channelName;
            this.serviceName = serviceName;
            this.shellEntryPointDefaultObject = new ShellObjectChannelInternals(this, SafeNativeMethods.NotificationType.TokenNotificationType);
            this.shellNotificationDefaultObject = new ShellObjectChannelInternals(this, SafeNativeMethods.NotificationType.ToastNotificationType);
            Initialize();
        }

        [SecurityCritical]
        private HttpNotificationChannel(string channelName, uint channelId)
        {
            this.name = channelName;
            this.channelId = channelId;
            this.shellEntryPointDefaultObject = new ShellObjectChannelInternals(this, SafeNativeMethods.NotificationType.TokenNotificationType);
            this.shellNotificationDefaultObject = new ShellObjectChannelInternals(this, SafeNativeMethods.NotificationType.ToastNotificationType);
            Initialize();
        }

        [SecuritySafeCritical]
        internal static void AddCallback(uint channelId, NotificationChannelHandler channelHandler)
        {
            if (channelId != 0)
            {
                try
                {
                    callbackDictionary.Remove(channelId);
                    callbackDictionary.Add(channelId, channelHandler);
                }
                catch (ArgumentException)
                {
                }
            }
        }

        /// <summary>The method the application uses to bind its default tile with a notification subscription. The tile can only contain local references for resources.</summary>
        /// <exception cref="InvalidOperationException">Details about this exception can be found in the exception string.</exception>
        [SecuritySafeCritical]
        public void BindToShellTile()
        {
            this.shellEntryPointDefaultObject.ChannelData = string.Empty;
            this.shellEntryPointDefaultObject.Bind();
        }

        /// <summary>Binds the tile passed as the input parameter with a notification subscription. The tile can contain either a local or remote resource reference.</summary>
        /// <param name="baseUri"></param>
        /// <exception cref="InvalidOperationException">Details about this exception can be found in the exception string.</exception>
        /// <exception cref="ArgumentException">Details about this exception can be found in the exception string.</exception>
        [SecuritySafeCritical]
        public void BindToShellTile(Collection<Uri> baseUri)
        {
            StringBuilder builder = new StringBuilder();
            bool flag = true;
            foreach (Uri uri in baseUri)
            {
                if (!flag)
                {
                    builder.Append("\n");
                }
                builder.Append(uri.OriginalString);
                flag = false;
            }
            this.shellEntryPointDefaultObject.ChannelData = builder.ToString();
            this.shellEntryPointDefaultObject.Bind();
        }

        /// <summary>The method the application uses to bind a toast notification subscription to the <see cref="T:Microsoft.Phone.Notification.HttpNotificationChannel" /> class instance.</summary>
        /// <exception cref="InvalidOperationException">Details about this exception can be found in the exception string.</exception>
        [SecuritySafeCritical]
        public void BindToShellToast()
        {
            this.shellNotificationDefaultObject.Bind();
        }

        [SecurityCritical]
        internal void ChannelHandler(uint eventType, IntPtr blob1, uint blobSize1, IntPtr blob2, uint blobSize2)
        {
            if (eventType == 2)
            {
                this.OnNotificationReceived(blob1, blobSize1, blob2, blobSize2);
            }
            else if (eventType == 0)
            {
                this.OnExceptionOccurred(blob1, blobSize1, blob2, blobSize2);
            }
            else
            {
                if (eventType != 1)
                {
                    throw new ArgumentException("", "eventType");
                }
                this.OnDescriptorUpdated(blob1, blobSize1);
            }
        }

        [SecurityCritical]
        private static void CheckAndAddCallback(HttpNotificationChannel channel)
        {
            if (((channel.httpNotificationReceivedEvent != null) || (channel.errorOccurredEvent != null)) || (channel.channelUriUpdatedEvent != null))
            {
                AddCallback(channel.ChannelId, new NotificationChannelHandler(channel.ChannelHandler));
            }
            else
            {
                RemoveCallback(channel.ChannelId);
                channel.channelId = 0;
            }
        }

        /// <summary>Closes a notification channel and disassociates all of the subscriptions associated with this instance of the <see cref="T:Microsoft.Phone.Notification.HttpNotificationChannel" /> class.</summary>
        /// <exception cref="InvalidOperationException">Details about this exception can be found in the exception string.</exception>
        [SecuritySafeCritical]
        public void Close()
        {
            if (this.shellEntryPointDefaultObject.ChannelId != 0)
            {
                this.shellEntryPointDefaultObject.Unbind();
            }
            if (this.shellNotificationDefaultObject.ChannelId != 0)
            {
                this.shellNotificationDefaultObject.Unbind();
            }
            SafeNativeMethods.ThrowExceptionFromHResult(SafeNativeMethods.NotificationAgentCloseChannel(this.channelId));
            RemoveCallback(this.channelId);
            this.channelId = 0;
        }

        [SecuritySafeCritical]
        internal static void Dispatch(object threadContext)
        {
            DispatchObject obj2 = (DispatchObject) threadContext;
            try
            {
                NotificationChannelHandler handler = null;
                try
                {
                    handler = callbackDictionary[obj2.channelId];
                }
                catch (KeyNotFoundException)
                {
                }
                if (handler != null)
                {
                    handler(obj2.eventType, obj2.blob1, obj2.blobSize1, obj2.blob2, obj2.blobSize2);
                }
            }
            finally
            {
                if (obj2.blob1 != IntPtr.Zero)
                {
                    SafeNativeMethods.NotificationAgentFreeMemory(obj2.blob1);
                }
                if (obj2.blob2 != IntPtr.Zero)
                {
                    SafeNativeMethods.NotificationAgentFreeMemory(obj2.blob2);
                }
            }
        }

        [SecurityCritical, AllowReversePInvokeCalls]
        internal static void DispatchCallback(uint channelId, uint eventType, IntPtr blob1, uint blobSize1, IntPtr blob2, uint blobSize2)
        {
            try
            {
                DispatchObject obj2;
                obj2.channelId = channelId;
                obj2.eventType = eventType;
                obj2.blob1 = blob1;
                obj2.blobSize1 = blobSize1;
                obj2.blob2 = blob2;
                obj2.blobSize2 = blobSize2;
                ThreadPool.QueueUserWorkItem(new WaitCallback(HttpNotificationChannel.Dispatch), obj2);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>Disposes the class.</summary>
        [SecuritySafeCritical]
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        [SecurityCritical]
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                Uninitialize();
                this.shellEntryPointDefaultObject.Dispose(disposing);
                this.shellNotificationDefaultObject.Dispose(disposing);
                if (disposing)
                {
                    RemoveCallback(this.channelId);
                }
                this.channelId = 0;
                this.disposed = true;
            }
        }

        /// <summary>Frees resources and performs cleanup operations before the class instance is reclaimed by garbage collection.</summary>
        [SecuritySafeCritical]
        ~HttpNotificationChannel()
        {
            this.Dispose(false);
        }

        /// <summary>Used to find a previously created notification channel.</summary>
        /// <returns>Returns 
        /// <see cref="T:Microsoft.Phone.Notification.HttpNotificationChannel" />
        /// .</returns>
        /// <param name="channelName"></param>
        /// <exception cref="InvalidOperationException">Details about this exception can be found in the exception string.</exception>
        [SecuritySafeCritical]
        public static HttpNotificationChannel Find(string channelName)
        {
            uint channelId = 0;
            HttpNotificationChannel channel = null;
            int hr = 0;
            Initialize();
            try
            {
                hr = SafeNativeMethods.NotificationAgentFindChannel(SafeNativeMethods.NotificationType.HttpNotificationType, channelName, 0, out channelId);
                SafeNativeMethods.ThrowExceptionFromHResult(hr);
                if ((hr == 1) || (channelId == 0))
                {
                    return channel;
                }
                channel = new HttpNotificationChannel(channelName, channelId);
                ShellObjectChannelInternals internals = ShellObjectChannelInternals.Find(channel, string.Empty, SafeNativeMethods.NotificationType.TokenNotificationType, channelId);
                if (internals != null)
                {
                    channel.shellEntryPointDefaultObject = internals;
                }
                internals = ShellObjectChannelInternals.Find(channel, string.Empty, SafeNativeMethods.NotificationType.ToastNotificationType, channelId);
                if (internals != null)
                {
                    channel.shellNotificationDefaultObject = internals;
                }
            }
            finally
            {
                Uninitialize();
            }
            return channel;
        }

        [SecurityCritical]
        private static Uri GetDescriptorFromPtr(IntPtr descriptorPtr, uint descriptorSize)
        {
            if ((descriptorPtr == IntPtr.Zero) || (descriptorSize == 0))
            {
                return null;
            }
            byte[] destination = new byte[descriptorSize];
            Marshal.Copy(descriptorPtr, destination, 0, (int) descriptorSize);
            return new Uri(TrimString(Encoding.UTF8.GetString(destination, 0, (int) descriptorSize)));
        }

        [SecuritySafeCritical]
        internal static void Initialize()
        {
            if (instanceCount == 0)
            {
                reverseHandler = new SafeNativeMethods.ReverseHandler(HttpNotificationChannel.DispatchCallback);
                SafeNativeMethods.ThrowExceptionFromHResult(SafeNativeMethods.NotificationAgentInitialize(reverseHandler));
            }
            instanceCount++;
        }

        [SecurityCritical]
        private void OnDescriptorUpdated(IntPtr blob, uint blobSize)
        {
            IntPtr descriptorPtr = blob;
            uint descriptorSize = blobSize;
            if (descriptorPtr != IntPtr.Zero)
            {
                Uri descriptorFromPtr = GetDescriptorFromPtr(descriptorPtr, descriptorSize);
                if (!descriptorFromPtr.Equals(this.uri) && (this.channelUriUpdatedEvent != null))
                {
                    this.uri = descriptorFromPtr;
                    this.channelUriUpdatedEvent(this, new NotificationChannelUriEventArgs(this.uri));
                }
            }
        }

        [SecurityCritical]
        private void OnExceptionOccurred(IntPtr blob1, uint blobSize1, IntPtr blob2, uint blobSize2)
        {
            SafeNativeMethods.AgentError error = (SafeNativeMethods.AgentError) Marshal.PtrToStructure(blob1, typeof(SafeNativeMethods.AgentError));
            int hrLastError = (int) error.hrLastError;
            NotificationChannelErrorEventArgs e = null;
            uint num2 = 0;
            if (hrLastError == -2129589900)
            {
                num2 = (uint) Marshal.ReadInt32(blob2);
            }
            switch (hrLastError)
            {
                case -2129589903:
                    e = new NotificationChannelErrorEventArgs(ChannelErrorType.PayloadFormatError, hrLastError, 0, LocalizeString.PushErrorTypePayloadFormatInvalid);
                    RemoveAllCallback();
                    break;

                case -2129589902:
                    e = new NotificationChannelErrorEventArgs(ChannelErrorType.NotificationRateTooHigh, hrLastError, 0, LocalizeString.PushErrorTypeNotificationRateTooHigh);
                    break;

                case -2129589901:
                    e = new NotificationChannelErrorEventArgs(ChannelErrorType.ChannelOpenFailed, hrLastError, 0, LocalizeString.PushErrorTypeChannelOpenFailed);
                    break;

                case -2129589900:
                    e = new NotificationChannelErrorEventArgs(ChannelErrorType.PowerLevelChanged, hrLastError, (int) num2, LocalizeString.PushErrorTypePowerLevelChanged);
                    break;

                case -2129589899:
                    e = new NotificationChannelErrorEventArgs(ChannelErrorType.MessageBadContent, hrLastError, 0, LocalizeString.PushErrorTypeMessageBadContent);
                    break;
            }
            if ((this.errorOccurredEvent != null) && (e != null))
            {
                this.errorOccurredEvent(this, e);
            }
        }

        [SecurityCritical]
        private void OnNotificationReceived(IntPtr blob1, uint blobSize1, IntPtr blob2, uint blobSize2)
        {
            if (this.httpNotificationReceivedEvent != null)
            {
                HttpNotification notif = new HttpNotification {
                    Channel = this
                };
                if ((blob1 != IntPtr.Zero) && (blobSize1 != 0))
                {
                    byte[] destination = new byte[blobSize1];
                    Marshal.Copy(blob1, destination, 0, (int) blobSize1);
                    string[] strArray = TrimString(Encoding.UTF8.GetString(destination, 0, (int) blobSize1)).Replace("\r\n", "$").Split(new char[] { '$' });
                    WebHeaderCollection headers = new WebHeaderCollection();
                    foreach (string str2 in strArray)
                    {
                        string[] strArray2 = str2.Replace(": ", "$").Split(new char[] { '$' });
                        if (strArray2.Length == 2)
                        {
                            headers[strArray2[0]] = strArray2[1];
                        }
                    }
                    notif.Headers = headers;
                }
                else
                {
                    notif.Headers = null;
                }
                if ((blob2 != IntPtr.Zero) && (blobSize2 != 0))
                {
                    byte[] buffer2 = new byte[blobSize2];
                    Marshal.Copy(blob2, buffer2, 0, (int) blobSize2);
                    MemoryStream stream = new MemoryStream(buffer2);
                    notif.Body = stream;
                }
                else
                {
                    notif.Body = null;
                }
                this.httpNotificationReceivedEvent(this, new HttpNotificationEventArgs(notif));
            }
        }

        /// <summary>Opens a notification channel with the Microsoft Push Notification Service.</summary>
        /// <exception cref="InvalidOperationException">Details about this exception can be found in the exception string.</exception>
        /// <exception cref="ArgumentException">Details about this exception can be found in the exception string.</exception>
        [SecuritySafeCritical]
        public void Open()
        {
            uint channelId = 0;
            bool fActivation = this.httpNotificationReceivedEvent != null;
            SafeNativeMethods.ThrowExceptionFromHResult(SafeNativeMethods.NotificationAgentOpenChannel(SafeNativeMethods.NotificationType.HttpNotificationType, this.name, this.serviceName, fActivation, out channelId), new InvalidOperationException(LocalizeString.PushNotificationChannelOpenFailed), SafeNativeMethods.NotificationType.HttpNotificationType);
            if (channelId == 0)
            {
                throw new InvalidOperationException(LocalizeString.PushNotificationChannelOpenFailed);
            }
            this.channelId = channelId;
            CheckAndAddCallback(this);
        }

        [SecurityCritical]
        internal static void RemoveAllCallback()
        {
            callbackDictionary.Clear();
        }

        [SecurityCritical]
        internal static void RemoveCallback(uint channelId)
        {
            if (channelId != 0)
            {
                callbackDictionary.Remove(channelId);
            }
        }

        [SecurityCritical]
        internal static string TrimString(string input)
        {
            int index = input.IndexOf('\0');
            if (index < 0)
            {
                return input;
            }
            return input.Substring(0, index);
        }

        /// <summary>Unbinds the active tile notification subscription from the notification channel.</summary>
        /// <exception cref="InvalidOperationException">Details about this exception can be found in the exception string.</exception>
        [SecuritySafeCritical]
        public void UnbindToShellTile()
        {
            this.shellEntryPointDefaultObject.Unbind();
        }

        /// <summary>Unbinds the active toast notification subscription from the notification channel.</summary>
        /// <exception cref="InvalidOperationException">Details about this exception can be found in the exception string.</exception>
        [SecuritySafeCritical]
        public void UnbindToShellToast()
        {
            this.shellNotificationDefaultObject.Unbind();
        }

        [SecurityCritical]
        internal static void Uninitialize()
        {
            instanceCount--;
            if (instanceCount < 0)
            {
                instanceCount = 0;
            }
            if (instanceCount == 0)
            {
                SafeNativeMethods.ThrowExceptionFromHResult(SafeNativeMethods.NotificationAgentUninitialize());
            }
        }

        internal uint ChannelId
        {
            get
            {
                return this.channelId;
            }
        }

        /// <summary>The name of the notification channel.</summary>
        /// <returns>Returns 
        /// <see cref="T:System.String" />
        /// .</returns>
        public string ChannelName
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>The current active notification channel URI.</summary>
        /// <returns>Returns 
        /// <see cref="T:System.Uri" />
        /// .</returns>
        public Uri ChannelUri
        {
            [SecuritySafeCritical]
            get
            {
                IntPtr zero = IntPtr.Zero;
                uint descriptorSize = 0;
                try
                {
                    int hr = SafeNativeMethods.NotificationAgentGetDescriptor(this.channelId, out zero, out descriptorSize);
                    if (hr != -2129590009)
                    {
                        SafeNativeMethods.ThrowExceptionFromHResult(hr);
                        if (zero != IntPtr.Zero)
                        {
                            this.uri = GetDescriptorFromPtr(zero, descriptorSize);
                        }
                    }
                }
                finally
                {
                    if (zero != IntPtr.Zero)
                    {
                        SafeNativeMethods.NotificationAgentFreeMemory(zero);
                    }
                    zero = IntPtr.Zero;
                }
                return this.uri;
            }
        }

        /// <summary>Returns true if the notification channel is currently bound to a tile notification subscription and false if not.
        /// </summary>
        /// <returns>Returns 
        /// <see cref="T:System.Boolean" />
        /// .</returns>
        public bool IsShellTileBound
        {
            get
            {
                return (this.shellEntryPointDefaultObject.ChannelId != 0);
            }
        }

        /// <summary>Returns true if the notification channel is currently bound to a toast notification subscription.</summary>
        /// <returns>Returns 
        /// <see cref="T:System.Boolean" />
        /// .</returns>
        public bool IsShellToastBound
        {
            get
            {
                return (this.shellNotificationDefaultObject.ChannelId != 0);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct DispatchObject
        {
            public uint channelId;
            public uint eventType;
            public IntPtr blob1;
            public uint blobSize1;
            public IntPtr blob2;
            public uint blobSize2;
        }

        [SecurityCritical]
        internal delegate void NotificationChannelHandler(uint eventType, IntPtr blob1, uint blobSize1, IntPtr blob2, uint blobSize2);
    }
}

