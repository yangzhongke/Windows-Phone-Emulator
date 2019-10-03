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
    internal class WavFileHelper
    {
        // 获取 PCM 格式的音频头的字节数组
        public static byte[] GetWavFileHeader(long audioByteLength, AudioFormat audioFormat)
        {
            MemoryStream stream = new MemoryStream(44);

            // “RIFF”
            stream.Write(new byte[] { 0x52, 0x49, 0x46, 0x46 }, 0, 4);

            // 文件长度 = 音频数据长度 + 44个字节的header长度 - 8个字节（上面的4个字节和这里的4个字节）
            stream.Write(BitConverter.GetBytes((UInt32)(audioByteLength + 44 - 8)), 0, 4);

            // “WAVE”
            stream.Write(new byte[] { 0x57, 0x41, 0x56, 0x45 }, 0, 4);

            // “fmt ”
            stream.Write(new byte[] { 0x66, 0x6D, 0x74, 0x20 }, 0, 4);

            // 对于 PCM 来说，这里的值为 16
            stream.Write(BitConverter.GetBytes((UInt32)16), 0, 4);

            // “1”代表未压缩
            stream.Write(BitConverter.GetBytes((UInt16)1), 0, 2);

            // 通道数：“1”代表单声道；“2”代表双声道
            stream.Write(BitConverter.GetBytes((UInt16)audioFormat.Channels), 0, 2);

            // 采样率（采样频率），即每秒样本数
            stream.Write(BitConverter.GetBytes((UInt32)audioFormat.SamplesPerSecond), 0, 4);

            // 比特率，即每秒字节数。其值为通道数×每秒数据位数×每样本的数据位数／8（SampleRate * ChannelCount * BitsPerSample / 8）
            stream.Write(BitConverter.GetBytes((UInt32)((audioFormat.SamplesPerSecond * audioFormat.Channels * audioFormat.BitsPerSample) / 8)), 0, 4);

            // 通道数×每样本的数据位数／8（ChannelCount * BitsPerSample / 8）
            stream.Write(BitConverter.GetBytes((UInt16)((audioFormat.Channels * audioFormat.BitsPerSample) / 8)), 0, 2);

            // 采样位数（采样分辨率），即每样本的数据位数
            stream.Write(BitConverter.GetBytes((UInt16)audioFormat.BitsPerSample), 0, 2);

            // “data”
            stream.Write(new byte[] { 0x64, 0x61, 0x74, 0x61 }, 0, 4);

            // 语音数据的长度
            stream.Write(BitConverter.GetBytes((UInt32)audioByteLength), 0, 4);

            return (stream.GetBuffer());
        }
    }
}
