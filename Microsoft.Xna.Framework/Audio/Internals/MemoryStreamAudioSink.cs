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

namespace Microsoft.Xna.Framework.Audio.Internals
{
    internal class MemoryStreamAudioSink : AudioSink
    {
        public AudioFormat AudioFormat { get; private set; }
        public MemoryStream MemoryStream { get; private set; }

        // 当音频设备开始捕获音频数据时所调用的方法
        protected override void OnCaptureStarted()
        {
            MemoryStream = new MemoryStream();
        }

        // 当音频设备停止捕获音频数据时所调用的方法
        protected override void OnCaptureStopped()
        {

        }

        /// <summary>
        /// 当音频设备报告音频格式更改时所调用的方法
        /// 当音频设备开始捕获音频数据时会调用一次 OnFormatChange，此时便可以确定当前的音频格式
        /// </summary>
        /// <param name="audioFormat">新的音频格式</param>
        protected override void OnFormatChange(AudioFormat audioFormat)
        {
            AudioFormat = audioFormat;
        }

        /// <summary>
        /// 当音频设备捕获了一个完整的音频采样时所调用的方法
        /// </summary>
        /// <param name="sampleTime">当前采样被捕获时的时间。单位：100纳秒</param>
        /// <param name="sampleDuration">当前采样的时长。单位：100纳秒</param>
        /// <param name="sampleData">当前采样的音频数据的字节流</param>
        protected override void OnSamples(long sampleTime, long sampleDuration, byte[] sampleData)
        {
            MemoryStream.Write(sampleData, 0, sampleData.Length);
        }
    }
}
