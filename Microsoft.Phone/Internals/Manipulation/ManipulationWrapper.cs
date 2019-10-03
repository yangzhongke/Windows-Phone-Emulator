using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Input.Manipulations;
using System.Collections.Generic;
using System.Windows.Input;

namespace Microsoft.Phone.Internals.Manipulation
{
    public class ManipulationWrapper
    {
        private UIElement wrappedElement;
        private ManipulationProcessor2D _manipulationProcessor;
        private bool IsMousePress;

        public event EventHandler<Manipulation2DCompletedEventArgs> ManipulationCompleted;

        public event EventHandler<Manipulation2DDeltaEventArgs> ManipulationDelta;

        public event EventHandler<Manipulation2DStartedEventArgs> ManipulationStarted;

        public ManipulationWrapper(UIElement wrappedElement)
        {
            this.wrappedElement = wrappedElement;

            this.wrappedElement.MouseLeftButtonDown += new MouseButtonEventHandler(wrappedElement_MouseLeftButtonDown);
            this.wrappedElement.MouseMove += new MouseEventHandler(wrappedElement_MouseMove);
            this.wrappedElement.MouseLeftButtonUp += new MouseButtonEventHandler(wrappedElement_MouseLeftButtonUp);
            this.wrappedElement.LostMouseCapture += new MouseEventHandler(wrappedElement_LostMouseCapture);


            _manipulationProcessor = new ManipulationProcessor2D(Manipulations2D.All);
            _manipulationProcessor.Started += new EventHandler<Manipulation2DStartedEventArgs>(_manipulationProcessor_Started);
            _manipulationProcessor.Delta += new EventHandler<Manipulation2DDeltaEventArgs>(_manipulationProcessor_Delta);
            _manipulationProcessor.Completed += new EventHandler<Manipulation2DCompletedEventArgs>(_manipulationProcessor_Completed);
        }

        void wrappedElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.IsMousePress = false;
            this.wrappedElement.ReleaseMouseCapture();
        }

        void wrappedElement_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.IsMousePress)
            {
                this.ProcessMouse(e);
            }
        }

        void wrappedElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.IsMousePress = true;
            wrappedElement.CaptureMouse();
            this.ProcessMouse(e);
        }

        private void ProcessMouse(MouseEventArgs e)
        {
            UIElement uIElement = VisualTreeHelper.GetParent(this.wrappedElement) as UIElement;
            //if (uIElement == null)
            //{
            //    return;
            //}
            Point position = e.GetPosition(uIElement);
            List<Manipulator2D> list = new List<Manipulator2D>();
            list.Add(new Manipulator2D(0, (float)position.X, (float)position.Y));
            this._manipulationProcessor.ProcessManipulators(DateTime.Now.Ticks, list);
        }

        void _manipulationProcessor_Completed(object sender, Manipulation2DCompletedEventArgs e)
        {
            if (ManipulationCompleted != null)
            {
                ManipulationCompleted(sender, e);
            }
        }

        void _manipulationProcessor_Delta(object sender, Manipulation2DDeltaEventArgs e)
        {
            if (ManipulationDelta != null)
            {
                ManipulationDelta(sender, e);
            }
        }

        void _manipulationProcessor_Started(object sender, Manipulation2DStartedEventArgs e)
        {
            if (ManipulationStarted != null)
            {
                ManipulationStarted(sender, e);
            }
        }

        void wrappedElement_LostMouseCapture(object sender, MouseEventArgs e)
        {
            this._manipulationProcessor.ProcessManipulators(DateTime.Now.Ticks, null);
        }


    }
}
