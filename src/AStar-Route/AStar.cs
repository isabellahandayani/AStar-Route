using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStar_Route
{
    class AStar
    {
        private Graph graph;
        private Dictionary<string, bool> expanded;
        private Dictionary<string, double> visitedCost;
        private List<string> path;
        private bool found;

        public AStar(Graph graph, string source, string target)
        {
            this.expanded = new Dictionary<string, bool>();
            this.visitedCost = new Dictionary<string, double>();
            this.graph = graph;
            this.path = new List<string>();
            initVisit();

            bool found = false;
            bool end = false;
            string currNode = source;
            double currCost = 0;

            
            Dictionary<string, List<string>> currNodeAdj = graph.getAdjLst();
            while(!end)
            {
                this.path.Add(currNode);   
                foreach(var x in currNodeAdj[currNode])
                {
                    if(x.Equals(target))
                    {
                        found = true;
                        end = true;
                        this.path.Add(x);
                        break;
                    }
                    else
                    {
                        if (!visitedCost.ContainsKey(x))
                        {
                            visitedCost.Add(x, currCost + graph.haversine(currNode, x));
                        }
                        else
                        {
                            visitedCost[x] = currCost + graph.haversine(currNode, x);
                        }
                    }
                }

                if(!end)
                {
                    expanded[currNode] = true;
                    currNode = findLowestCost();

                    if (currNode == null)
                    {
                        end = true;
                        found = false;
                    }
                    else
                    {
                        currCost = visitedCost[currNode];   
                    }
                }
            }
            this.found = found;
        }
        
        public void initVisit()
        {
            expanded = new Dictionary<string, bool>();
            foreach(var x in this.graph.getNodeLst())
            {
                expanded.Add(x.Key, false);
            }
        }

        public string findLowestCost()
        {
            double min = 999999999;
            foreach(var x in visitedCost)
            {
                if(expanded[x.Key] != true && x.Value < min)
                {
                    min = x.Value;
                }
            }
            return getKeyFromValue(min);
        }

        public string getKeyFromValue(double value)
        {
            foreach(var x in visitedCost)
            {
                if(value == x.Value)
                {
                    return x.Key;
                }
            }

            return null;
        }

        public bool getStatus()
        {
            return this.found;
        }

        public List<string> getPath()
        {
            return this.path;
        }


    }
}
