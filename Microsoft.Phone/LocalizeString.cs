namespace Microsoft.Phone
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0"), CompilerGenerated, DebuggerNonUserCode]
    internal class LocalizeString
    {
        private static CultureInfo resourceCulture;
        private static System.Resources.ResourceManager resourceMan;

        internal LocalizeString()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        internal static string PushErrorTypeChannelOpenFailed
        {
            get
            {
                return "PushErrorTypeChannelOpenFailed";
            }
        }

        internal static string PushErrorTypeMessageBadContent
        {
            get
            {
                return "PushErrorTypeMessageBadContent";
            }
        }

        internal static string PushErrorTypeNotificationRateTooHigh
        {
            get
            {
                return "PushErrorTypeNotificationRateTooHigh";
            }
        }

        internal static string PushErrorTypePayloadFormatInvalid
        {
            get
            {
                return "PushErrorTypePayloadFormatInvalid";
            }
        }

        internal static string PushErrorTypePowerLevelChanged
        {
            get
            {
                return "PushErrorTypePowerLevelChanged";
            }
        }

        internal static string PushErrorTypeUnknown
        {
            get
            {
                return "PushErrorTypeUnknown";
            }
        }

        internal static string PushNotificationChannelBindFailed
        {
            get
            {
                return "PushNotificationChannelBindFailed";
            }
        }

        internal static string PushNotificationChannelBindingExists
        {
            get
            {
                return "PushNotificationChannelBindingExists";
            }
        }

        internal static string PushNotificationChannelExists
        {
            get
            {
                return "PushNotificationChannelExists";
            }
        }

        internal static string PushNotificationChannelNotOpened
        {
            get
            {
                return "PushNotificationChannelNotOpened";
            }
        }

        internal static string PushNotificationChannelOpenFailed
        {
            get
            {
                return "PushNotificationChannelOpenFailed";
            }
        }

        internal static string PushNotificationChannelQuotaExceeded
        {
            get
            {
                return "PushNotificationChannelQuotaExceeded";
            }
        }

        internal static string PushNotificationChannelServerUnavailable
        {
            get
            {
                return "PushNotificationChannelServerUnavailable";
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("Microsoft.Phone.LocalizeString", typeof(LocalizeString).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }

        internal static string ShellInvalidInterval
        {
            get
            {
                return "ShellInvalidInterval";
            }
        }

        internal static string ShellInvalidRemoteImageUri
        {
            get
            {
                return "ShellInvalidRemoteImageUri";
            }
        }

        internal static string ShellInvalidUri
        {
            get
            {
                return "ShellInvalidUri";
            }
        }
    }
}

