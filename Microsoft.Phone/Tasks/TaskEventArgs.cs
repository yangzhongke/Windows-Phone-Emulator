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
    public class TaskEventArgs : EventArgs
    {        
        public TaskEventArgs()
        {
        }

        public TaskEventArgs(TaskResult taskResult)
        {
            this.TaskResult = taskResult;
        }

        public virtual Exception Error
        {
            get;
            set;
        }

        public virtual TaskResult TaskResult
        {
            get;
            set;
        }
    }

}
