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

namespace Microsoft.Phone.Tasks
{
    public class ChooserInfo<TTaskEventArgs> where TTaskEventArgs : TaskEventArgs
    {
        //Chooser的类型
        public Type ChooserType { get; set; }

        //监听的类中监听方法的委托
        public EventHandler<TTaskEventArgs> EventHandler { get; set; }
    }
}
