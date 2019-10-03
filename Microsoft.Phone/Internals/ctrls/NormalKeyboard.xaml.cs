using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Internals;
using Microsoft.Phone.Internals.ctrls;

namespace Microsoft.Phone.Internals.ctrls
{
    public partial class NormalKeyboard : BaseKeyboard
	{   
		public NormalKeyboard()
		{
			// Required to initialize variables
			InitializeComponent();

            IsUrlMode = false;

            var buttons = LayoutRoot.FindDesendants<Button>();
            foreach(var button in buttons)
            {
                button.Click += new RoutedEventHandler(btnKey_Click);               
            }
		}

        internal bool IsUrlMode 
        {
            set
            {
                if (value)
                {
                    urlStackPanel.Visibility = System.Windows.Visibility.Visible;
                    normalStackPanel.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    urlStackPanel.Visibility = System.Windows.Visibility.Collapsed;
                    normalStackPanel.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        void btnKey_Click(object sender, RoutedEventArgs e)
        {
            Button btnKey = sender as Button;
            string tag = Convert.ToString(btnKey.Tag);
            if (tag == "shift")//上档键
            {
                //切换大小写
                var buttons = LayoutRoot.FindDesendants<Button>();
                foreach (var button in buttons)
                {
                    if (button.Tag == null)//特殊键都有Tag，没有Tag的都是普通键
                    {
                        button.Content = SwapCaps(Convert.ToString(button.Content));
                    }
                }
            }
            else if (tag == "back")//后退
            {
                BackSpace();
            }
            else if (tag == "number")//切换到数字键
            {
                KeyboardContainer.Content = new NumberKeyboard();
            }
            else if (tag == "space")//空格
            {
                InputString(" ");
            }
            else if (tag == "ime")//切换输入法
            {
                //暂不实现
            }
            else if (tag == "return")//回车
            {
                InputString("\n");
            }
            else
            {
                InputString(Convert.ToString(btnKey.Content));
            }            
        }
	}
}