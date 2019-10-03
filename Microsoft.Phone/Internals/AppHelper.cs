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
using System.ComponentModel;
using System.IO;
using System.Reflection;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.Automation;

namespace Microsoft.Phone.Internals
{
    //Simulator Internal Use only
    internal static class AppHelper
    {
        public static WinPhoneCtrl GetWinPhoneCtrl(UIElement uiElement)
        {
            return FindParent<WinPhoneCtrl>(uiElement);
        }

        public static T FindParent<T>(DependencyObject baseObj) where T : DependencyObject
        {
            DependencyObject obj = VisualTreeHelper.GetParent(baseObj);
            if (obj is T)
            {
                return (T)obj;
            }
            else if (obj == null)
            {
                return null;
            }
            else
            {
                return FindParent<T>(obj);
            }
        }

        /// <summary>
        /// 查找baseObj中类型为T的子节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseObj"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindDesendants<T>(this DependencyObject baseObj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(baseObj); i++)
            {
                var child = VisualTreeHelper.GetChild(baseObj, i);
                if (child is T)
                {
                    yield return (T)child;
                }
                foreach (var c in FindDesendants<T>(child))
                {
                    yield return (T)c;
                }
            }
        }

        //add by yangzhongke
        public static bool IsInDesignMode()
        {
            if ((Application.Current != null) && (Application.Current.GetType() != typeof(Application)))
            {
                return DesignerProperties.IsInDesignTool;
            }
            return true;
        }

        public static Visibility ToVisibility(this bool visible)
        {
            return visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public static PhoneApplicationPage GetCurrentPhoneAppPage()
        {
            return WinPhoneCtrl.Instance.frameScreen.Content as PhoneApplicationPage;
        }

        public static Stream GetExecutingAssemblyResourceStream(string relativeName)
        {
            string asmName = new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name;
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(asmName + "." + relativeName);
            return stream;
        }

        public static PhoneApplicationFrame RootPhoneApplicationFrame
        {
            get
            {
                return WinPhoneCtrl.Instance.frameScreen;
            }
        }

        public static Stream GetAssemblyResourceStream(Assembly asm,string relativeName)
        {
            string asmName = new AssemblyName(asm.FullName).Name;
            Stream stream = asm.GetManifestResourceStream(asmName + "." + relativeName);
            return stream;
        }

        public static byte[] ToBytes(this Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }            
        }

        public static bool IsImgFile(string filename)
        {
            return System.IO.Path.GetExtension(filename) == ".jpg" || System.IO.Path.GetExtension(filename) == ".bmp"
                || System.IO.Path.GetExtension(filename) == ".jpeg" || System.IO.Path.GetExtension(filename) == ".png";
        }

        /// <summary>
        /// 在浏览器页面打开地址url
        /// </summary>
        /// <param name="url"></param>
        public static void OpenUrl(string url)
        {
            //如果是运行在OOB中则用Shell.Application打开网址
            if (AutomationFactory.IsAvailable)
            {
                dynamic shell = AutomationFactory.CreateObject("Shell.Application");
                shell.ShellExecute(url);
            }
            else if (!System.Windows.Application.Current.IsRunningOutOfBrowser)
            {
                System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(url,UriKind.Absolute), "_blank");
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// 得到WriteableBitmap的二进制内容
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[] BitMapToByte(System.Windows.Media.Imaging.WriteableBitmap bitmap)
        {
            if (bitmap == null) return null;
            int width = bitmap.PixelWidth;
            int height = bitmap.PixelHeight;
            int bands = 3;
            byte[][,] raster = new byte[bands][,];

            for (int i = 0; i < bands; i++)
            {
                raster[i] = new byte[width, height];
            }

            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    int pixel = bitmap.Pixels[width * row + column];
                    byte a = ((byte)(pixel >> 24));

                    byte r = (byte)(pixel >> 16);//4 R
                    byte g = (byte)(pixel >> 8);//2 G
                    byte b = (byte)pixel;//0 B

                    if (a < 2)
                    {
                        raster[0][column, row] = (byte)(255 - r);
                        raster[1][column, row] = (byte)(255 - g);
                        raster[2][column, row] = (byte)(255 - b);
                    }
                    else
                    {
                        raster[0][column, row] = (byte)(r * 255.0 / a);
                        raster[1][column, row] = (byte)(g * 255.0 / a);
                        raster[2][column, row] = (byte)(b * 255.0 / a);
                    }
                }
            }

            FluxJpeg.Core.ColorModel model = new FluxJpeg.Core.ColorModel { colorspace = FluxJpeg.Core.ColorSpace.RGB };
            FluxJpeg.Core.Image img = new FluxJpeg.Core.Image(model, raster);


            //Encode the Image as a JPEG
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            FluxJpeg.Core.Encoder.JpegEncoder encoder = new FluxJpeg.Core.Encoder.JpegEncoder(img, 100, stream);
            encoder.Encode();

            //Back to the start
            stream.Seek(0, System.IO.SeekOrigin.Begin);

            //Get teh Bytes and write them to the stream
            byte[] binaryData = new byte[stream.Length];
            long bytesRead = stream.Read(binaryData, 0, (int)stream.Length);
            return binaryData;
        }

        /// <summary>
        /// 创建Itcast.Net.Phone.InteropServices组件，如果没有注册组件则报错
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static dynamic TryCreatePhoneInteropServices(string name)
        {
            try
            {
                return AutomationFactory.CreateObject("Itcast.Net.Phone.InteropServices." + name);
            }
            catch (Exception ex)
            {
                throw new Exception("创建Itcast.Net.Phone.InteropServices失败，可能是因为没有注册Itcast.Net.Phone.InteropServices.dll的原因，请将Itcast.Net.Phone.InteropServices.zip包解压到硬盘上，然后运行reg.bat即可。注意运行完毕不要删除或者移动解压后的文件。", ex);
            }
        }
    }
}
