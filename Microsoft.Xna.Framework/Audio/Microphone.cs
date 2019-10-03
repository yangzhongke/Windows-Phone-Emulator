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
using Microsoft.Xna.Framework.Audio.Internals;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Framework;

namespace Microsoft.Xna.Framework.Audio
{
    public sealed class Microphone
    {
        public static ReadOnlyCollection<Microphone> All
        {
            get
            {
                List<Microphone> list = new List<Microphone>();
                if (CaptureDeviceConfiguration.GetAvailableAudioCaptureDevices().Count > 0)
                {
                    list.Add(Default);
                }
                return new ReadOnlyCollection<Microphone>(list);
            }
        }

        private static Microphone instance = new Microphone();
        public static Microphone Default
        {
            get
            {
                return instance;
            }
        }

        private CaptureSource _captureSource;
        private MemoryStreamAudioSink _audioSink;
        private MemoryStream memStream;

        public event EventHandler<EventArgs> BufferReady;

        public int GetData(byte[] buffer)
        {
            return memStream.Read(buffer, 0, buffer.Length);
        }

        public int GetData(byte[] buffer, int offset, int count)
        {
            return memStream.Read(buffer, offset, count);
        }

        public TimeSpan GetSampleDuration(int sizeInBytes)
        {
            //这个函数根据名称来看,大概功能是输入一段音频数据的总字节数(存储空间大小),
            //利用返回值输出音频的持续时间.而音频数据的持续时间 = 音频数据所占用的存储空间的总大小 / 每一秒音频数据占用空间的大小
            //根据开头的公式,可以得出:
            //持续时间 = 存储空间的总大小/((每秒的样本个数 * 每个样本占用的存储空间)*声道)

            AudioFormat audioFormat = _audioSink.AudioFormat;
            double result = sizeInBytes / (audioFormat.SamplesPerSecond * audioFormat.BitsPerSample * audioFormat.Channels);
            return TimeSpan.FromSeconds(result);
        }

        public int GetSampleSizeInBytes(TimeSpan duration)
        {
            //这个函数的字面意思是根据音频数据的持续时间,计算出数据的字节数
            //音频数据的字节数 = (每秒音频数据占用的存储空间(这里的存储空间单位是bit) * 持续时间的秒数))/8, 原因是每8bit是一个byte(字节).
            AudioFormat audioFormat = _audioSink.AudioFormat;
            double result = audioFormat.SamplesPerSecond * audioFormat.BitsPerSample * duration.TotalSeconds / 8; 
            return (int)result;
        }

        private Microphone()
        {
            if (CaptureDeviceConfiguration.GetAvailableAudioCaptureDevices().Count <= 0)
            {
                throw new NoMicrophoneConnectedException();
            }

            _captureSource = new CaptureSource();
            _captureSource.AudioCaptureDevice = CaptureDeviceConfiguration.GetDefaultAudioCaptureDevice();
            // AudioSink 用于处理捕获音频信息的相关逻辑
            _audioSink = new MemoryStreamAudioSink();
            _audioSink.CaptureSource = _captureSource;
        }

        public void Start()
        {
            FrameworkDispatcher.CheckIsUpdateCalled();

            if (memStream != null)
            {
                memStream.Dispose();//Memory leak here !but i'm simulator!
            }
            if (CaptureDeviceConfiguration.AllowedDeviceAccess || CaptureDeviceConfiguration.RequestDeviceAccess())
            {
                _captureSource.Start();
                State = MicrophoneState.Started;
            }                
        }

        public void Stop()
        {
            _captureSource.Stop();
            State = MicrophoneState.Stopped;
            memStream = new MemoryStream();
            // 写入音频头
            byte[] wavFileHeader = WavFileHelper.GetWavFileHeader(_audioSink.MemoryStream.Length, _audioSink.AudioFormat);
            memStream.Write(wavFileHeader, 0, wavFileHeader.Length);

            byte[] buffer = new byte[4096];
            int read = 0;

            _audioSink.MemoryStream.Seek(0, SeekOrigin.Begin);

            // 写入音频数据
            while ((read = _audioSink.MemoryStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                memStream.Write(buffer, 0, read);
            }
            memStream.Position = 0;

            if (BufferReady != null)
            {
                BufferReady(this, EventArgs.Empty);
            }            
        }
       

        public TimeSpan BufferDuration
        {
            get
            {
                throw new NotImplementedException("I'm not audio expert,don't know how to,please help me");
            }
            set
            {
                throw new NotImplementedException("I'm not audio expert,don't know how to,please help me");
            }
        }        

        public bool IsHeadset
        {
            get
            {
                return false;
            }
        }

        public int SampleRate
        {
            get
            {
                //这个成员变量就是采样频率,值就是 SamplesPerSecond,即每秒的样本个数
                AudioFormat audioFormat = _audioSink.AudioFormat;
                return audioFormat.SamplesPerSecond;
            }
        }

        public MicrophoneState State
        {
            get;
            private set;
        }

    }
}
