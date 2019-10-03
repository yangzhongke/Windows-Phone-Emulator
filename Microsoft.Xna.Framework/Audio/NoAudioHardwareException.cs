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
    public sealed class NoAudioHardwareException:Exception
    {
        public NoAudioHardwareException()
        {
        }

        public NoAudioHardwareException(string message):base(message)
        {
        }

        public NoAudioHardwareException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
