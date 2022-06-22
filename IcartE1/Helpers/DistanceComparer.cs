using IcartE1.Data;
using System;
using System.Collections.Generic;

namespace IcartE1.Helpers
{
    public class DistanceComparer : IComparer<Branch>
    {

        private readonly double latitude;
        private readonly double longitude;
        public DistanceComparer(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public int Compare(Branch x, Branch y)
        {
            double distance1= GetDistance(x.Longitude,x.Latitude,longitude,latitude);
            double distance2= GetDistance(y.Latitude,y.Longitude,longitude,latitude);

            if (distance1 > distance2)
                return 1;
            else if (distance1 == distance2)
                return 0;
            else
                return -1;
        }

        public double GetDistance(double longitude, double latitude, double otherLongitude, double otherLatitude)
        {
            var d1 = latitude * (Math.PI / 180.0);
            var num1 = longitude * (Math.PI / 180.0);
            var d2 = otherLatitude * (Math.PI / 180.0);
            var num2 = otherLongitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }
    }
}
