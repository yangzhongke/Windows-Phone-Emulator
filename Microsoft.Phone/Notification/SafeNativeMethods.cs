namespace Microsoft.Phone.Notification
{
    using Microsoft.Phone;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;

    [SecurityCritical]
    internal sealed class SafeNativeMethods
    {
        private const int E_HANDLE = -2147024890;
        private const int E_INVALIDARG = -2147024809;
        private const int E_NOTIMPL = -2147467263;
        private const int E_OUTOFMEMORY = -2147024882;
        internal const int MPM_E_BEHAVIOR_CHANGED_BY_POLICY = -2129589900;
        private const int MPM_E_EXCEEDED_MAX_ENDPOINTSINSUBSCRIPTION = -2129590014;
        private const int MPM_E_EXCEEDED_MAX_SUBSCRIPTIONS = -2129590015;
        private const int MPM_E_EXCEEDED_MAX_SUBSCRIPTIONTYPES = -2129590013;
        internal const int MPM_E_FATAL = -2129589903;
        private const int MPM_E_INSUFFICIENT_BUFFER = -2129590271;
        internal const int MPM_E_MESSAGE_BAD_CONTENT = -2129589899;
        private const int MPM_E_MESSAGE_INVALID_FORMAT = -2129590266;
        private const int MPM_E_NO_ENDPOINT_ASSOCIATED = -2129590265;
        internal const int MPM_E_QUEUE_FULL = -2129589902;
        internal const int MPM_E_REGISTER_ENDPOINT_FAILED = -2129589901;
        private const int MPM_E_SUBSCRIPTION_ALREAY_EXIST = -2129590270;
        private const int MPM_E_SUBSCRIPTION_ASSOCIATED_TYPE_NOT_EXIST = -2129590010;
        private const int MPM_E_SUBSCRIPTION_DOES_NOT_EXIST = -2129590263;
        private const int MPM_E_SUBSCRIPTION_TYPE_ALREAY_EXIST = -2129590011;
        private const int MPM_E_SUBSCRIPTION_TYPE_DOES_NOT_EXIST = -2129590267;
        private const int MPM_E_SUBSCRIPTION_TYPE_INVALID_DATA = -2129590268;
        internal const int MPM_E_SUBSCRIPTION_TYPE_NO_DESCRIPTOR = -2129590009;
        private const int MPM_E_SUBSCRIPTION_TYPE_NOT_SUPPORTED = -2129590269;
        private const int MPM_E_SUBSCRIPTION_TYPE_RESOURCE_CONFLICTED = -2129590012;
        private const int MPM_E_UNEXPECTED_ASYNC_CONGRESSION = -2129590264;
        private const int RPC_S_SERVER_TOO_BUSY = -2147023173;
        private const int RPC_S_SERVER_UNAVAILABLE = -2147023174;
        internal const int S_FALSE = 1;
        internal const int S_OK = 0;

        private SafeNativeMethods()
        {
        }

        internal static int NotificationAgentActivateChannel(uint channelId, [In, MarshalAs(UnmanagedType.Bool)] bool fActivation)
        {
            return 0;
        }

        internal static int NotificationAgentBindChannel(NotificationType notificationType, string channelName, uint associatedChannelId, string channelData, [In, MarshalAs(UnmanagedType.Bool)] bool fActivation, out uint channelId)
        {
            channelId = 0;
            return 0;
        }

        internal static int NotificationAgentCloseChannel(uint channelId)
        {
            return 0;
        }

        internal static int NotificationAgentFindChannel(NotificationType notificationType, string channelName, uint associatedChannelId, out uint channelId)
        {
            channelId = 0;
            return 0;
        }

        internal static void NotificationAgentFreeMemory(IntPtr memory)
        {
        }

        internal static int NotificationAgentGetDescriptor(uint channelId, out IntPtr descriptor, out uint descriptorSize)
        {
            descriptor = IntPtr.Zero;
            descriptorSize = 0;
            return 0;
        }

        internal static int NotificationAgentInitialize(ReverseHandler reverseHandler)
        {
            return 0;
        }

        internal static int NotificationAgentOpenChannel(NotificationType notificationType, string channelName, string serviceName, [In, MarshalAs(UnmanagedType.Bool)] bool fActivation, out uint channelId)
        {
            channelId = 0;
            return 0;
        }

        internal static int NotificationAgentUnbindChannel(uint channelId)
        {
            return 0;
        }

        internal static int NotificationAgentUninitialize()
        {
            return 0;
        }

        internal static void ThrowExceptionFromHResult(int hr)
        {
            ThrowExceptionFromHResult(hr, new InvalidOperationException(), NotificationType.HttpNotificationType);
        }

        internal static void ThrowExceptionFromHResult(int hr, NotificationType type)
        {
            ThrowExceptionFromHResult(hr, new InvalidOperationException(), type);
        }

        internal static void ThrowExceptionFromHResult(int hr, Exception defaultException, NotificationType type)
        {
            switch (hr)
            {
                case -2147467263:
                    throw new NotSupportedException();

                case -2147024890:
                case -2147024809:
                    throw new ArgumentException("E_INVALIDARG");

                case -2147024882:
                    throw new InvalidOperationException();

                case -2147023174:
                case -2147023173:
                    throw new InvalidOperationException(LocalizeString.PushNotificationChannelServerUnavailable);

                case -2129590015:
                case -2129590014:
                case -2129590013:
                    throw new InvalidOperationException(LocalizeString.PushNotificationChannelQuotaExceeded);

                case -2129590012:
                case -2129590011:
                    if (type == NotificationType.HttpNotificationType)
                    {
                        throw new InvalidOperationException(LocalizeString.PushNotificationChannelExists);
                    }
                    throw new InvalidOperationException(LocalizeString.PushNotificationChannelBindingExists);

                case -2129590010:
                    throw new InvalidOperationException(LocalizeString.PushNotificationChannelNotOpened);

                case 0:
                case 1:
                    return;
            }
            throw defaultException;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct AgentError
        {
            public uint dwVersion;
            public uint hrLastError;
            public uint dwLastError;
        }

        internal enum HttpNotificationActionType
        {
            ExceptionOccuredActionType,
            DescriptorUpdatedActionType,
            NotificationReceivedActionType
        }

        internal enum NotificationType
        {
            HttpNotificationType,
            TokenNotificationType,
            ToastNotificationType
        }

        internal delegate void ReverseHandler(uint channelId, uint eventType, IntPtr blob1, uint blobSize1, IntPtr blob2, uint blobSize2);
    }
}

