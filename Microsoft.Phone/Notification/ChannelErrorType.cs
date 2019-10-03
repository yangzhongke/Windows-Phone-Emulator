namespace Microsoft.Phone.Notification
{
    using System;

    /// <summary>An enumeration that contains asynchronous errors sent from the Push Client to the application. These error values can occur regardless of if the application is running in the foreground or not.</summary>
    public enum ChannelErrorType
    {
        ChannelOpenFailed,
        PayloadFormatError,
        MessageBadContent,
        NotificationRateTooHigh,
        PowerLevelChanged
    }
}

