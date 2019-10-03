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
using System.Globalization;

namespace System.Device.Location
{
    public class GeoCoordinate : IEquatable<GeoCoordinate>
    {
        // Fields
        private double altitude;//海拔
        private double course;//航向。北为正轴，顺时针方向0-360
        private double horizontalAccuracy;
        private double latitude;
        private double longitude;
        private double speed;
        public static readonly GeoCoordinate Unknown = new GeoCoordinate();
        private double verticalAccuracy;

        // Methods
        public GeoCoordinate()
        {
            this.latitude = double.NaN;
            this.longitude = double.NaN;
            this.altitude = double.NaN;
            this.verticalAccuracy = double.NaN;
            this.horizontalAccuracy = double.NaN;
            this.speed = double.NaN;
            this.course = double.NaN;
        }

        public GeoCoordinate(double latitude, double longitude)
            : this(latitude, longitude, double.NaN)
        {
        }

        public GeoCoordinate(double latitude, double longitude, double altitude)
            : this(latitude, longitude, altitude, double.NaN, double.NaN, double.NaN, double.NaN)
        {
        }

        public GeoCoordinate(double latitude, double longitude, double altitude, double horizontalAccuracy, double verticalAccuracy, double speed, double course)
        {
            this.latitude = double.NaN;
            this.longitude = double.NaN;
            this.altitude = double.NaN;
            this.verticalAccuracy = double.NaN;
            this.horizontalAccuracy = double.NaN;
            this.speed = double.NaN;
            this.course = double.NaN;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Altitude = altitude;
            this.HorizontalAccuracy = horizontalAccuracy;
            this.VerticalAccuracy = verticalAccuracy;
            this.Speed = speed;
            this.Course = course;
        }

        public bool Equals(GeoCoordinate other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }
            if (this.IsUnknown || other.IsUnknown)
            {
                return (this.IsUnknown && other.IsUnknown);
            }
            return (this.Latitude.Equals(other.Latitude) && this.Longitude.Equals(other.Longitude));
        }

        public override bool Equals(object obj)
        {
            if (obj is GeoCoordinate)
            {
                return this.Equals(obj as GeoCoordinate);
            }
            return base.Equals(obj);
        }

        public double GetDistanceTo(GeoCoordinate other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            if ((double.IsNaN(this.Latitude) || double.IsNaN(this.Longitude)) || (double.IsNaN(other.Latitude) || double.IsNaN(other.Longitude)))
            {
                throw new ArgumentException("Latitude or longitude is not a number NaN");
            }
            double d = this.Latitude * 0.017453292519943295;
            double num3 = this.Longitude * 0.017453292519943295;
            double num4 = other.Latitude * 0.017453292519943295;
            double num5 = other.Longitude * 0.017453292519943295;
            double num6 = num5 - num3;
            double num7 = num4 - d;
            double num8 = Math.Pow(Math.Sin(num7 / 2.0), 2.0) + ((Math.Cos(d) * Math.Cos(num4)) * Math.Pow(Math.Sin(num6 / 2.0), 2.0));
            double num9 = 2.0 * Math.Atan2(Math.Sqrt(num8), Math.Sqrt(1.0 - num8));
            return (6376500.0 * num9);
        }

        public override int GetHashCode()
        {
            return (this.Latitude.GetHashCode() ^ this.Longitude.GetHashCode());
        }

        public static bool operator ==(GeoCoordinate left, GeoCoordinate right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }
            return left.Equals(right);
        }

        public static bool operator !=(GeoCoordinate left, GeoCoordinate right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            if (this == Unknown)
            {
                return "Unknown";
            }
            return (this.Latitude.ToString("G", CultureInfo.InvariantCulture) + ", " + this.Longitude.ToString("G", CultureInfo.InvariantCulture));
        }

        // Properties
        public double Altitude
        {
            get
            {
                return this.altitude;
            }
            set
            {
                this.altitude = value;
            }
        }

        public double Course
        {
            get
            {
                return this.course;
            }
            set
            {
                if ((value < 0.0) || (value > 360.0))
                {
                    throw new ArgumentOutOfRangeException("Course", "The value of the parameter must be from 0.0 to 360.0.");
                }
                this.course = value;
            }
        }

        public double HorizontalAccuracy
        {
            get
            {
                return this.horizontalAccuracy;
            }
            set
            {
                if (value < 0.0)
                {
                    throw new ArgumentOutOfRangeException("HorizontalAccuracy", "The value of the parameter must be greater than or equal to zero.");
                }
                this.horizontalAccuracy = (value == 0.0) ? double.NaN : value;
            }
        }

        public bool IsUnknown
        {
            get
            {
                if (!double.IsNaN(this.Latitude))
                {
                    return double.IsNaN(this.Longitude);
                }
                return true;
            }
        }

        public double Latitude
        {
            get
            {
                return this.latitude;
            }
            set
            {
                if ((value > 90.0) || (value < -90.0))
                {
                    throw new ArgumentOutOfRangeException("Latitude", "The value of the parameter must be from -90.0 to 90.0.");
                }
                this.latitude = value;
            }
        }

        public double Longitude
        {
            get
            {
                return this.longitude;
            }
            set
            {
                if ((value > 180.0) || (value < -180.0))
                {
                    throw new ArgumentOutOfRangeException("Longitude", "The value of the parameter must be from -180.0 to 180.0.");
                }
                this.longitude = value;
            }
        }

        public double Speed
        {
            get
            {
                return this.speed;
            }
            set
            {
                if (value < 0.0)
                {
                    throw new ArgumentOutOfRangeException("Speed", "The value of the parameter must be greater than or equal to zero.");
                }
                this.speed = value;
            }
        }

        public double VerticalAccuracy
        {
            get
            {
                return this.verticalAccuracy;
            }
            set
            {
                if (value < 0.0)
                {
                    throw new ArgumentOutOfRangeException("VerticalAccuracy", "The value of the parameter must be greater than or equal to zero.");
                }
                this.verticalAccuracy = (value == 0.0) ? double.NaN : value;
            }
        }
    }
}
