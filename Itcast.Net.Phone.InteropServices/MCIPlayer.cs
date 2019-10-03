using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Itcast.Net.Phone.InteropServices
{
    [ComVisible(true)]
    [Guid("CCAF483C-3F54-402E-BD43-7E4F8F3A5FAC")]
    public class MCIPlayer
    {
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);

        public string FileName { get; set; }

        public void Play()
        {
            string _command = "open \"" + FileName + "\" type mpegvideo alias MediaFile";//加""避免文件名中的空格问题
            mciSendString(_command, null, 0, IntPtr.Zero);
            mciSendString("play MediaFile", null, 0, IntPtr.Zero);
        }

        public void Stop()
        {
            mciSendString("close MediaFile", null, 0, IntPtr.Zero);
        }
    }
}
