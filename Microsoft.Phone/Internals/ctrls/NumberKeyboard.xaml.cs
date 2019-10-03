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
    public partial class NumberKeyboard : BaseKeyboard
	{
        public NumberKeyboard()
		{
			// Required to initialize variables
			InitializeComponent();

            var buttons = LayoutRoot.FindDesendants<Button>();
            foreach(var button in buttons)
            {
                button.Click += new RoutedEventHandler(btnKey_Click);               
            }
		}

        /// <summary>
        /// 切换按钮的文本，在a|b之间切换
        /// </summary>
        /// <param name="btn"></param>
        private static void SwapButtonContent(Button btn)
        {
            string tag = Convert.ToString(btn.Tag);
            if(!tag.Contains("|"))
            {
                throw new Exception("文本框的Tag中不包含|");
            }
            string[] strs = tag.Split('|');
            string content = Convert.ToString(btn.Content);
            if(strs[0]==content)
            {
                btn.Content = strs[1];
            }
            else if(strs[1]==content)
            {
                btn.Content = strs[0];
            }
            else
            {
                throw new Exception("错误的Content");
            }
        }

        void btnKey_Click(object sender, RoutedEventArgs e)
        {
            Button btnKey = sender as Button;
            string tag = Convert.ToString(btnKey.Tag);
            if (tag == "shift")//上档键，切换键盘区的字符
            {
                string content = Convert.ToString(btnKey.Content);
                if (content == "→")
                {
                    btnKey.Content = "←";                    
                }
                else
                {
                    btnKey.Content = "→";
                }
                var buttons = LayoutRoot.FindDesendants<Button>();
                foreach (var button in buttons)
                {
                    if (Convert.ToString(button.Tag).Contains("|"))//把所有Tag包含“|”的键（符号键）切换
                    {
                        SwapButtonContent(button);
                    }
                }
            }
            else if (tag == "back")//后退
            {
                BackSpace();
            }
            else if (tag == "abcd")//切换到字母键
            {
                KeyboardContainer.Content = new NormalKeyboard();
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