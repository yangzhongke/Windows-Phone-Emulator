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
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Framework;

namespace Microsoft.Xna.Framework.Input.Touch
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TouchLocation : IEquatable<TouchLocation>
    {
        //private TouchLocationState previousState = TouchLocationState.Invalid;
        //private Vector2 previousPosition = Vector2.Zero;

        //public TouchLocation(int id, TouchLocationState state, Vector2 position)
        //{
        //    Id = id;
        //    this.State = state;
        //    this.Position = position;
        //}

        //public TouchLocation(int id, TouchLocationState state, Vector2 position,
        //    TouchLocationState previousState, Vector2 previousPosition)
        //{
        //    this.Id = id;
        //    this.State = state;
        //    this.Position = position;
        //    this.previousState = previousState;
        //    this.previousPosition = previousPosition;
        //}
        

        public TouchLocationState State
        {
            get;
            private set;
        }
        public int Id
        {
            get;
            private set;
        }
        public Vector2 Position
        {
            get;
            private set;
        }
        //public bool TryGetPreviousLocation(out TouchLocation previousLocation)
        //{
        //    //previousLocation = this.previousPosition;
        //}

        public bool Equals(TouchLocation other)
        {
            if (other == null)
            {
                return false;
            }
            return other.Id == this.Id && other.Position == this.Position
                && other.State == this.State;
        }

        public override bool Equals(object obj)
        {
            TouchLocation other = (TouchLocation)obj;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return (this.Id.ToString() + this.Position + this.State).GetHashCode();
        }

        public static bool operator ==(TouchLocation first, TouchLocation second)
        {
            if ((object)first == null && (object)second == null)
            {
                return true;
            }
            else if ((object)first != null & (object)second != null)
            {
                return first.Id == second.Id && first.Position == second.Position
                && first.State == second.State;
            }
            else
            {
                return false;
            }
        }

        public static bool operator !=(TouchLocation first, TouchLocation second)
        {
            return !(first == second);
        }
    }
}
