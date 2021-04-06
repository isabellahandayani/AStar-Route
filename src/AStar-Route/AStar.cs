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
        private Dictionary<string, string> parentMap;

        public AStar(Graph graph, string source, string target)
        {
            this.expanded = new Dictionary<string, bool>();
            this.visitedCost = new Dictionary<string, double>();
            this.graph = graph;
            this.path = new List<string>();
            this.parentMap = new Dictionary<string, string>();
            initVisit();


            bool end = false;
            string currNode = source;
            double currCost = 0;


            Dictionary<string, List<string>> currNodeAdj = graph.getAdjLst();
            while (!end)
            {
                this.path.Add(currNode);
                foreach (var x in currNodeAdj[currNode])
                {
                    if (!parentMap.ContainsKey(x))
                    {
                        parentMap.Add(x, currNode);
                    }

                    if (x.Equals(target))
                    {
                        this.found = true;
                        end = true;
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

                if (!end)
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

        }

        public void initVisit()
        {
            expanded = new Dictionary<string, bool>();
            foreach (var x in this.graph.getNodeLst())
            {
                expanded.Add(x.Key, false);
            }
        }

        public string findLowestCost()
        {
            double min = 999999999;
            foreach (var x in visitedCost)
            {
                if (expanded[x.Key] != true && x.Value < min)
                {
                    min = x.Value;
                }
            }
            return getKeyFromValue(min);
        }

        public string getKeyFromValue(double value)
        {
            foreach (var x in visitedCost)
            {
                if (value == x.Value)
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

        public List<string> getPath(string source, string target)
        {
            List<string> cleaned = new List<string>();
            path.Reverse();
            string curr = path[0];

            while(!curr.Equals(source))
            {
                cleaned.Add(curr);
                curr = parentMap[curr];
            }


                cleaned.Reverse();
                return cleaned;
            }

        }
    }

