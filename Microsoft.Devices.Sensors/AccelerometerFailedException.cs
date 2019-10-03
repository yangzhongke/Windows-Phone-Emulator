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

namespace Microsoft.Devices.Sensors
{
    public class AccelerometerFailedException : Exception
    {
        // Fields
        private int m_errorId;

        // Methods
        internal AccelerometerFailedException()
        {
        }

        internal AccelerometerFailedException(string message)
            : base(message)
        {
        }

        internal AccelerometerFailedException(string message, int ErrorId)
            : base(message)
        {
            this.m_errorId = ErrorId;
        }

        // Properties
        public int ErrorId
        {
            get
            {
                return this.m_errorId;
            }
        }
    }


}
