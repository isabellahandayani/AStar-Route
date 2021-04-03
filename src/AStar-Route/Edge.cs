using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStar_Route
{
    class Edge
    {
        private string source;
        private string target;
        private float value;


        // ctor
        Edge(string start, string end, float value)
        {
            this.source = start;
            this.target = end;
            this.value = value;
        }

        // getter
        public string getSource()
        {
            return this.source;
        }

        public string getTarget()
        {
            return this.target;
        }

        public float getValue()
        {
            return this.value;
        }

    }
}
