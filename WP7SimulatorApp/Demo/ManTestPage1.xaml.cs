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
using Microsoft.Phone.Internals.Manipulation;

namespace WP7SimulatorApp.Demo
{
    public partial class ManTestPage1 : PhoneApplicationPage
    {
        public ManTestPage1()
        {
            InitializeComponent();

            ManipulationWrapper man = new ManipulationWrapper(border1);
            man.ManipulationStarted += new EventHandler<System.Windows.Input.Manipulations.Manipulation2DStartedEventArgs>(man_ManipulationStarted);
            man.ManipulationDelta += new EventHandler<System.Windows.Input.Manipulations.Manipulation2DDeltaEventArgs>(man_ManipulationDelta);
            man.ManipulationCompleted += new EventHandler<System.Windows.Input.Manipulations.Manipulation2DCompletedEventArgs>(man_ManipulationCompleted);
        }

         

        void man_ManipulationCompleted(object sender, System.Windows.Input.Manipulations.Manipulation2DCompletedEventArgs e)
        {
            //PageTitle.Text = "Completed" + e;
           
        }

        void man_ManipulationDelta(object sender, System.Windows.Input.Manipulations.Manipulation2DDeltaEventArgs e)
        {
            //PageTitle.Text = "Delta" + e;
            var transform = ellipse1.TransformToVisual(border1);
            var point1 = transform.Transform(new Point(0,0));
            var size = new Size(ellipse1.ActualWidth,ellipse1.ActualHeight);
            Rect rect = new Rect(point1, size);
            Point curPoint = new Point(e.OriginX+e.Cumulative.TranslationX,e.OriginY+ e.Cumulative.ExpansionY);
            if (rect.Contains(curPoint))
            {
                ellipse1.Fill = new SolidColorBrush(Colors.Yellow);
            }
        }

        void man_ManipulationStarted(object sender, System.Windows.Input.Manipulations.Manipulation2DStartedEventArgs e)
        {
            //PageTitle.Text = "Started"+e;
            ellipse1.Fill = new SolidColorBrush(Colors.Red);
        }
    }
}
