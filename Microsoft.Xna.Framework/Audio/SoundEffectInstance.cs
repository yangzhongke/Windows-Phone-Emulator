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
using Microsoft.Phone.Internals;
using System.IO;
using WaveMSS;
using Microsoft.Xna.Framework.Framework;

namespace Microsoft.Xna.Framework.Audio
{
    public class SoundEffectInstance : IDisposable
    {
        private MediaElement mediaElement;
        private RepeatedMediaElement repeatedMedia;

        private MemoryStream memStream;
        internal SoundEffectInstance(System.IO.Stream stream)
        {
            mediaElement = new MediaElement();
            mediaElement.Visibility = Visibility.Collapsed;
            WinPhoneCtrl.Instance.RootPanel.Children.Add(mediaElement);
            repeatedMedia = new RepeatedMediaElement(mediaElement);

            long oldPosition = stream.Position;
            memStream = new MemoryStream();
            stream.CopyTo(memStream);
            stream.Position = oldPosition;

            memStream.Position = 0;            
            try
            {
                //MediaElement in silverlight not support wav files
                //so we use WaveMediaStreamSource(http://code.msdn.microsoft.com/wavmss)
                WavParser wp = new WavParser(memStream);//if not wav file,InvalidOperationException will be thrown here

                //if no exception thrown,it's a wav file,play through WaveMediaStreamSource
                memStream.Position = 0;
                WaveMediaStreamSource wavMss = new WaveMediaStreamSource(memStream);
                mediaElement.SetSource(wavMss);
            }
            catch (InvalidOperationException)
            {
                //if not wave file,play directly
                memStream.Position = 0;
                mediaElement.SetSource(memStream);
            }               
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            memStream.Dispose();
            repeatedMedia.Stop();
            WinPhoneCtrl.Instance.RootPanel.Children.Remove(mediaElement);
        }

        public void Pause()
        {
            repeatedMedia.Pause();
        }

        public virtual void Play()
        {
            FrameworkDispatcher.CheckIsUpdateCalled();
            repeatedMedia.Play();
        }

        public void Resume()
        {
            repeatedMedia.Play();
        }

        public void Stop()
        {
            repeatedMedia.Stop();
        }

        public void Stop(bool immediate)
        {
            repeatedMedia.Stop(immediate);
        }

        public virtual bool IsLooped
        {
            get
            {
                return repeatedMedia.IsLooped;
            }
            set
            {
                repeatedMedia.IsLooped = value;
            }
        }

        public float Pan
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public float Pitch
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public SoundState State
        {
            get
            {
                if (mediaElement.CurrentState == MediaElementState.Paused)
                {
                    return SoundState.Paused;
                }
                if (mediaElement.CurrentState == MediaElementState.Playing||
                    mediaElement.CurrentState == MediaElementState.Buffering ||
                    mediaElement.CurrentState == MediaElementState.Opening)
                {
                    return SoundState.Playing;
                }
                if (mediaElement.CurrentState == MediaElementState.Stopped ||
                    mediaElement.CurrentState == MediaElementState.Closed)
                {
                    return SoundState.Stopped;
                }
                throw new Exception("Unkown State" + mediaElement.CurrentState);
            }
        }

        public float Volume
        {
            get
            {
                return (float)mediaElement.Volume;
            }
            set
            {
                mediaElement.Volume = value;
            }
        }
    }
}
