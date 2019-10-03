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
using System.Text;

namespace Microsoft.Phone.Internals.ctrls
{
    public class BaseKeyboard:UserControl
    {
        protected void Close()
        {
            WinPhoneCtrl.Instance.CloseAllKeyBoards();
        }

        protected void InputString(string s)
        {
            if (CurrentInputCtrl is TextBox)
            {
                TextBox txtBox = (TextBox)CurrentInputCtrl;
                txtBox.SelectedText = s;//插入文本
            }
            else
            {
                PasswordBox pwdBox = (PasswordBox)CurrentInputCtrl;
                pwdBox.Password = pwdBox.Password + s;
            }
        }

        protected void BackSpace()
        {
            TextBox txtBox = CurrentInputCtrl as TextBox;
            if (txtBox != null)
            {
                if (txtBox.Text.Length <= 0)
                {
                    return;
                }
                txtBox.Text = CutLast(txtBox.Text);
            }
            PasswordBox pwdBox = CurrentInputCtrl as PasswordBox;
            if (pwdBox != null)
            {
                if (pwdBox.Password.Length <= 0)
                {
                    return;
                }
                pwdBox.Password = CutLast(pwdBox.Password);
            }
        }

        /// <summary>
        /// 把字符串s中的大小写切换
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        protected static string SwapCaps(string s)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in s)
            {
                if (char.IsLower(c))
                {
                    sb.Append(char.ToUpper(c));
                }
                else if (char.IsUpper(c))
                {
                    sb.Append(char.ToLower(c));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 切掉最后一个字符
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        protected static string CutLast(string s)
        {
            return s.Substring(0, s.Length - 1);
        }

        protected Control CurrentInputCtrl
        {
            get
            {
                return KeyboardContainer.CurrentInputCtrl;
            }
        }

        protected KeyboardContainer KeyboardContainer
        {
            get
            {
                return this.Parent as KeyboardContainer;
            }
        }
    }
}
