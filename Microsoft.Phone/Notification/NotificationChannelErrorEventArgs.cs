namespace Microsoft.Phone.Notification
{
    using System;

    /// <summary>The argument returned with the <see cref="E:Microsoft.Phone.Notification.HttpNotificationChannel.ErrorOccurred" /> event.</summary>
    public class NotificationChannelErrorEventArgs : EventArgs
    {
        private int errorAdditionalData;
        private int errorCode;
        private ChannelErrorType errorType;
        private string message;

        internal NotificationChannelErrorEventArgs(ChannelErrorType errorType, int errorCode, int errorAdditionalData, string message)
        {
            this.errorType = errorType;
            this.errorCode = errorCode;
            this.errorAdditionalData = errorAdditionalData;
            this.message = message;
        }

        /// <summary>Additional information that is associated with the error type.</summary>
        /// <returns>Returns 
        /// <see cref="T:System.Int32" />
        /// .</returns>
        public int ErrorAdditionalData
        {
            get
            {
                return this.errorAdditionalData;
            }
        }

        /// <summary>Returns an HRESULT that corresponds with the error.</summary>
        /// <returns>Returns 
        /// <see cref="T:System.Int32" />
        /// .</returns>
        public int ErrorCode
        {
            get
            {
                return this.errorCode;
            }
        }

        /// <summary>Contains an enumeration that corresponds with the error. For more information see <see cref="T:Microsoft.Phone.Notification.ChannelErrorType" />.</summary>
        /// <returns>Returns 
        /// <see cref="T:Microsoft.Phone.Notification.ChannelErrorType" />
        /// .</returns>
        public ChannelErrorType ErrorType
        {
            get
            {
                return this.errorType;
            }
        }

        /// <summary>The plain text message of the error.</summary>
        /// <returns>Returns 
        /// <see cref="T:System.String" />
        /// .</returns>
        public string Message
        {
            get
            {
                return this.message;
            }
        }
    }
}

