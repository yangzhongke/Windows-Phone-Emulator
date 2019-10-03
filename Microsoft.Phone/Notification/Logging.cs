namespace Microsoft.Phone.Notification
{
    using System;
    using System.Diagnostics;

    internal class Logging
    {
        private Logging()
        {
        }

        [Conditional("DEBUG")]
        internal static void Log(string message)
        {
        }
    }
}

