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
using System.Windows.Controls.Primitives;
using System.Linq;

namespace Microsoft.Phone.Internals
{
    internal class PopupHellper
    {
        public static void ScanPopups()
        {
            //遍历所有的Popup，将没有Parent的Popup的Parent设置为Frame中的根元素
            //避免Popup默认显示在模拟器面板左上方的问题
            foreach (var popup in 
                VisualTreeHelper.GetOpenPopups())
            {
                if (popup.Parent == null)
                {
                    GetFrameRootPanel().Children.Add(popup);
                }
            }
        }

        /// <summary>
        /// 关闭所有Parent为屏幕根元素的Popup
        /// </summary>
        public static void CloseRootPopups()
        {
            foreach (var popup in
                VisualTreeHelper.GetOpenPopups())
            {
                var rootPanel = GetFrameRootPanel();
                if (popup.Parent == rootPanel)
                {
                    popup.IsOpen = false;
                    rootPanel.Children.Remove(popup);
                }
            }
        }

        /// <summary>
        /// 获得名字为gridFrameRoot的Frame中的根元素
        /// </summary>
        /// <returns></returns>
        private static Panel GetFrameRootPanel()
        {
            foreach (var element in WinPhoneCtrl.Instance.frameScreen.FindDesendants<FrameworkElement>())
            {
                if (element.Name == "panelFrameRoot")
                {
                    return (Panel)element;
                }
            }
            throw new Exception("panelFrameRoot not found");
        }
    }
}
