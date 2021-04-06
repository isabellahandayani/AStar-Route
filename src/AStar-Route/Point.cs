using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStar_Route
{
    class Point
    {

        //Atribut
        private double lat;
        private double lon;

        //ctor
        public Point(double lat, double lon)
        {
            this.lat = lat;
            this.lon = lon;
        }

        
        //getter
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
