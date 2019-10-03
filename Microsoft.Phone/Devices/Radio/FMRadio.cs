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
using System.Security;
using Microsoft.Phone.Internals;
using System.Reflection;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Phone.Devices.Radio
{
    [SecuritySafeCritical]
    public sealed class FMRadio
    {
        // Fields
        private static FMRadio _Instance = new FMRadio();
        private RadioPowerMode _powerMode = RadioPowerMode.Off;
        private RadioRegion _thisRegion = RadioRegion.UnitedStates;

        private double frequency = Min_Frequency;
        private WebBrowser browserPlayer;//播放器
        private RepeatedMediaElement radioStaticPlayer;//没有电台时的电流噪音
        private RepeatedMediaElement embresPlayer;//嵌入的模拟电台文件的播放器，方便在没有网络的时候也能测试
        private double signalStrength=0;//信号强度

        private const double Min_Frequency = 87;
        private const double Max_Frequency = 108;

        private FMRadio()
        {            
            //在线播放器
            browserPlayer = new WebBrowser();
            browserPlayer.Visibility = Visibility.Collapsed;//隐藏播放器
            WinPhoneCtrl.Instance.LayoutRoot.Children.Add(browserPlayer);//WebBrowser必须有parent才能播放mms

            //电流声播放器
            MediaElement elementRadioStatic = new MediaElement();
            elementRadioStatic.Visibility = Visibility.Collapsed;
            elementRadioStatic.AutoPlay = false;
            elementRadioStatic.Volume = 1;
            WinPhoneCtrl.Instance.LayoutRoot.Children.Add(elementRadioStatic);
            radioStaticPlayer = new RepeatedMediaElement(elementRadioStatic);            
            Stream stream = AppHelper.GetExecutingAssemblyResourceStream("Devices.Radio.Internals.RadioStatic.mp3");
            elementRadioStatic.SetSource(stream);

            //模拟电台文件的播放器，方便在没有网络的时候也能测试
            MediaElement elementEmbedRes = new MediaElement();
            elementEmbedRes.Visibility = Visibility.Collapsed;
            elementRadioStatic.AutoPlay = false;
            elementRadioStatic.Volume = 1;
            WinPhoneCtrl.Instance.LayoutRoot.Children.Add(elementEmbedRes);
            embresPlayer = new RepeatedMediaElement(elementEmbedRes);            
        }

        public RadioRegion CurrentRegion
        {
            get
            {
                return this._thisRegion;
            }
            set
            {
                this._thisRegion = value;
            }
        }

        public double Frequency
        {
            get
            {
                return frequency;
            }
            set
            {
                if (PowerMode == RadioPowerMode.Off)
                {
                    throw new ArgumentException("Radio is not power on");
                }
                //FM频率范围在87～108Mhz之间
                if (value < Min_Frequency || value > Max_Frequency)
                {
                    throw new ArgumentException("Frequency cannot value < 87 || value > 108", "Frequency");
                }
                value = Math.Round(value, 2);
                this.frequency = value;
                IEnumerable<XElement> radios;
                using (Stream stream = AppHelper.GetExecutingAssemblyResourceStream("Devices.Radio.Internals.RadioList.xml"))
                {
                    //从配置文件中加载模拟电台列表，找到频率和要听的频率一致的
                    XDocument document = XDocument.Load(stream);
                    radios = from item in document.Element("Radios").Elements("Radio")
                             where IsSameFrequency(Convert.ToDouble(item.Attribute("frequency").Value), value)
                             select item;
                }
                //如果没找到电台则播放噪音
                if (radios.Count() <= 0)
                {
                    browserPlayer.NavigateToString("");
                    embresPlayer.Stop();

                    radioStaticPlayer.Play(-1);
                    signalStrength = 0;
                    return;
                }
                var radio = radios.Single();
                string url = radio.Attribute("url").Value;
                if (url.StartsWith("emres://"))//如果是嵌入的音频文件模拟电台，则使用embresPlayer播放
                {
                    browserPlayer.NavigateToString("");
                    radioStaticPlayer.Stop();

                    string resourceName = Regex.Match(url, "emres://(.+)").Groups[1].Value;//提取emres://后的资源名
                    Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
                    embresPlayer.MediaElement.SetSource(stream);
                    embresPlayer.Play(-1);
                }
                else
                {
                    radioStaticPlayer.Stop();
                    embresPlayer.Stop();

                    string templateHTML = @"<html><body>
                        <OBJECT id='VIDEO'
	                        CLASSID='CLSID:6BF52A52-394A-11d3-B153-00C04F79FAA6'
	                        type='application/x-oleobject'>	
	                        <PARAM NAME='URL' VALUE='RadioURL'>
	                        <PARAM NAME='SendPlayStateChangeEvents' VALUE='True'>
	                        <PARAM NAME='AutoStart' VALUE='True'>
	                        <PARAM name='uiMode' value='none'>
	                        <PARAM name='PlayCount' value='9999'>
                        </OBJECT></body>";
                    templateHTML = templateHTML.Replace("RadioURL", url);
                    browserPlayer.NavigateToString(templateHTML);
                }

                signalStrength = 20;
            }
        }

        //判断f1、f2是否相同的频率
        private bool IsSameFrequency(double f1, double f2)
        {
            return Math.Abs(f1 - f2) <= 0.03;
        }

        public static FMRadio Instance
        {
            get
            {
                return _Instance;
            }
        }

        public RadioPowerMode PowerMode
        {
            get
            {
                return _powerMode;
            }
            set
            {
                if (_powerMode == value)
                {
                    return;
                }
                _powerMode = value;
                if (value == RadioPowerMode.On)
                {                    
                    Frequency = Min_Frequency;
                }
                else if (value == RadioPowerMode.Off)
                {
                    this.browserPlayer.NavigateToString("");//导航到空白页就相当于关掉收音机
                    this.radioStaticPlayer.Stop();
                    embresPlayer.Stop();
                }
                else
                {
                    throw new ArgumentException();
                }                
            }
        }

        public double SignalStrength
        {
            get
            {
                return signalStrength;
            }
        }
    }


}
