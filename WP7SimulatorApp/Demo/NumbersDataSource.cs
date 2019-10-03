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
using Microsoft.Phone.Controls.Primitives;

namespace WP7SimulatorApp.Demo
{
    public class NumbersDataSource : ILoopingSelectorDataSource
    {

        private int minimum = 1;

        private int maximum = 100;

        private int selectedItem = 1;



        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;



        protected virtual void OnSelectedChanged(SelectionChangedEventArgs e)
        {

            var selectionChanged = SelectionChanged;



            if (selectionChanged != null)

                selectionChanged(this, e);

        }



        public object GetNext(object relativeTo)
        {

            var nextValue = ((int)relativeTo) + 1;



            return nextValue <= Maximum ? nextValue : Minimum;

        }



        public object GetPrevious(object relativeTo)
        {

            var previousValue = ((int)relativeTo) - 1;



            return previousValue >= Minimum ? previousValue : Maximum;

        }



        public object SelectedItem
        {

            get
            {

                return selectedItem;

            }

            set
            {

                var oldValue = selectedItem;

                var newValue = (int)value;



                if (oldValue == newValue)

                    return;



                selectedItem = newValue;



                OnSelectedChanged(new SelectionChangedEventArgs(new[] { oldValue }, new[] { newValue }));

            }

        }



        public int Minimum
        {

            get
            {

                return minimum;

            }

            set
            {

                minimum = value;



                if (selectedItem < minimum)

                    SelectedItem = value;

            }

        }





        public int Maximum
        {

            get
            {

                return maximum;

            }

            set
            {

                maximum = value;



                if (selectedItem > maximum)

                    SelectedItem = value;

            }

        }

    }

}
