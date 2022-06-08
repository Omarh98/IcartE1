using System;
using System.Runtime.Serialization;

namespace IcartE1.Models
{
	public class DataPoint
	{
		public DataPoint(long x, double y)
		{
			this.x = x;
			this.y = y;
		}

        public long x { get; set; }
        public double y { get; set; }
    }
}
