using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace Microsoft.Phone.Internals.ctrls
{
    public partial class KeyboardContainer : UserControl
    {
        public KeyboardContainer()
        {
            InitializeComponent();            
        }

        private Control currentInputCtrl;
        internal Control CurrentInputCtrl 
        {
            get
            {
                return currentInputCtrl;
            }
            set
            {
                currentInputCtrl = value;

                //不能用InputScope属性，因为调用就报错，所以只能用附加属性模拟，需要告诉学生
                //InputScopeNameValue 可选值在InputScopeNameValue枚举中
                string inpuScope = TextBoxEx.GetInputScope(currentInputCtrl);
                InputScopeNameValue inputScopeNameValue;
                if (!Enum.TryParse<InputScopeNameValue>(inpuScope, out inputScopeNameValue))
                {
                    inputScopeNameValue = InputScopeNameValue.Default;
                }
                if (inputScopeNameValue == InputScopeNameValue.Number)
                {
                    NumberKeyboard keyboard = new NumberKeyboard();
                    this.Content = keyboard;
                }
                else if (inputScopeNameValue == InputScopeNameValue.TelephoneNumber)
                {
                    TelNumKeyboard keyboard = new TelNumKeyboard();
                    this.Content = keyboard;
                }
                else if (inputScopeNameValue == InputScopeNameValue.EmailNameOrAddress)
                {
                    NormalKeyboard normalKeyboard = new NormalKeyboard();
                    normalKeyboard.IsUrlMode = true;
                    this.Content = normalKeyboard;
                }
                else
                {
                    NormalKeyboard normalKeyboard = new NormalKeyboard();
                    this.Content = normalKeyboard;
                }                
            }
        }
    }
}
