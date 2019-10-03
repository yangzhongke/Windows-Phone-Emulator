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
using System.Runtime.InteropServices.Automation;

namespace Microsoft.Xna.Framework.Framework.Media
{
    public class MediaPlayer
    {
        //因为MediaElement无法播放My Music中的文件，即使在OOB下，所以只能用NativeCode
        private static dynamic mciPlayer =
            AppHelper.TryCreatePhoneInteropServices("MCIPlayer");

        public static void Play(Song song)
        {
            FrameworkDispatcher.CheckIsUpdateCalled();
            Stop();
            mciPlayer.FileName = song.filepath;
            mciPlayer.Play();
        }

        public static void Stop()
        {
            mciPlayer.Stop();
        } 
    }
}
