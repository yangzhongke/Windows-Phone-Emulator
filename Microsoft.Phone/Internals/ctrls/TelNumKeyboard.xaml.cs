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
    public partial class TelNumKeyboard : BaseKeyboard
	{
        public TelNumKeyboard()
		{
			// Required to initialize variables
			InitializeComponent();

            var buttons = LayoutRoot.FindDesendants<Button>();
            foreach(var button in buttons)
            {
                button.Click += new RoutedEventHandler(btnKey_Click);               
            }
		}

        void btnKey_Click(object sender, RoutedEventArgs e)
        {
            Button btnKey = sender as Button;
            string tag = Convert.ToString(btnKey.Tag);
            if (tag == "back")//后退
            {
                BackSpace();
            }
            else if (tag == "space")//空格
            {
                InputString(" ");
            }
            else
            {
                InputString(Convert.ToString(btnKey.Content));
            }            
        }
	}
}