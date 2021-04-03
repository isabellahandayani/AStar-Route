using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStar_Route
{
    class Point
    {
        private double lon;
        private double lat;

        public Point(double lon, double lat)
        {
            this.lon = lon;
            this.lat = lat;
        }

        public double getLon()
        {
            return this.lon;
        }

        public double getLat()
        {
            return this.lat;
        }
    }
}
