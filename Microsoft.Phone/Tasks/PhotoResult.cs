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
using System.IO;

namespace Microsoft.Phone.Tasks
{
    public class PhotoResult : TaskEventArgs
    {
        public PhotoResult()
        {
        }

        public PhotoResult(TaskResult taskResult)
            : base(taskResult)
        {
        }

        // Properties
        public Stream ChosenPhoto
        {
            get;
            set;
        }

        public string OriginalFileName
        {
            get;
            set;
        }
    }
}
