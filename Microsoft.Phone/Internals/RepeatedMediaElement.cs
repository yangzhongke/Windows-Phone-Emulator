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

namespace Microsoft.Phone.Internals
{
    public class RepeatedMediaElement
    {
        private MediaElement mediaElement;
        private int repeatCount = -1;
        private int currentRepeatCount = 0;
        public RepeatedMediaElement(MediaElement mediaElement)
        {            
            if (VisualTreeHelper.GetParent(mediaElement) == null)
            {
                throw new ArgumentException("MediaElement must have parent control,MediaEnded event of mediaElement will not be fired without parent control");
            }

            this.mediaElement = mediaElement;
            this.mediaElement.MediaEnded += new RoutedEventHandler(mediaElement_MediaEnded);
        }

        void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            //if (!isLooped || currentRepeatCount<0)
            //{
            //    return;
            //}
            if (repeatCount < 0||currentRepeatCount < repeatCount)
            {
                currentRepeatCount++;
                InternalPlay();
            }            
        }

        private void InternalPlay()
        {           
            mediaElement.Stop();
            mediaElement.Position = TimeSpan.Zero;
            mediaElement.Play();
        }

        internal MediaElement MediaElement
        {
            get
            {
                return mediaElement;
            }
        }

        private bool isLooped=false;

        public bool IsLooped
        {
            get
            {
                return isLooped;
            }
            set
            {
                if (isLooped == value)
                {
                    return;
                }
                isLooped = true;
                if (!isLooped)
                {
                    Stop(false);//停止循环
                }
            }
        }

        public void Play()
        {
            Play(1);
        }

        //-1:repeat forever
        public void Play(int repeatCount)
        {
            if (repeatCount < 0)
            {
                isLooped = true;
            }
            currentRepeatCount = 1;
            this.repeatCount = repeatCount;
            InternalPlay();     
        }

        public void Pause()
        {
            mediaElement.Pause();
        }

        /// <summary>
        /// 立即结束
        /// </summary>
        public void Stop()
        {
            Stop(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="immediate">为true立即停止，false当前循环播放完毕再停止</param>
        public void Stop(bool immediate)
        {
            if (immediate)
            {
                mediaElement.Stop();
            }
            else
            {
                isLooped = false;
            }            
        }
    }
}
