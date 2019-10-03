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
using System.IO;

namespace Microsoft.Xna.Framework.Audio
{

    public sealed class SoundEffect : IDisposable
    {
        private byte[] buffer;

        public SoundEffect(byte[] buffer, int sampleRate, AudioChannels channels)
        {
            this.buffer = buffer;
        }

        public SoundEffect(byte[] buffer, int offset, int count, int sampleRate, AudioChannels channels, int loopStart, int loopLength)
        {
            throw new NotSupportedException();
        }

        public SoundEffectInstance CreateInstance()
        {
            return new SoundEffectInstance(new MemoryStream(buffer));
        }

        public void Dispose()
        {
            buffer = null;
        }

        public static SoundEffect FromStream(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return new SoundEffect(ms.ToArray(), 4800, AudioChannels.Stereo);
            }            
        }
    }
}
