using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Microsoft.Xna.Framework.Audio
{
    public sealed class NoMicrophoneConnectedException : Exception
    {
        public NoMicrophoneConnectedException()
        {
        }

        public NoMicrophoneConnectedException(string message)
        {
        }

        public NoMicrophoneConnectedException(string message, Exception inner)
        {
        }
    }
}
