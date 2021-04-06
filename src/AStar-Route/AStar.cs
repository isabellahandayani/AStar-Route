using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStar_Route
{
    class AStar
    {
        //Atribut

        //Graph of map
        private Graph graph;

        // Expanded status of a node
        private Dictionary<string, bool> expanded;

        // Cost of already visited node
        private Dictionary<string, double> visitedCost;
        
        // Path of search
        private List<string> path;
        
        // Status of search
        private bool found;

        // Parent of node
        private Dictionary<string, string> parentMap;


        //Ctor
        public AStar(Graph graph, string source, string target)
        {
            /* KAMUS */
            /*
            end : bool
            currNode : string
            currCost : double
            currNodeAdj : Dictionary<string, List<string>>
            */

            /* ALGORTIMA */

            // Init atribbute
            this.expanded = new Dictionary<string, bool>();
            this.visitedCost = new Dictionary<string, double>();
            this.graph = graph;
            this.path = new List<string>();
            this.parentMap = new Dictionary<string, string>();
            
            //Initialize expanded as false
            initVisit();


            bool end = false;
            string currNode = source;
            double currCost = 0;


            Dictionary<string, List<string>> currNodeAdj = graph.getAdjLst();

            // while target not found or there are no path found
            while (!end)
            {
                this.path.Add(currNode);

                // Check for all node adjacent with currNode
                foreach (var x in currNodeAdj[currNode])
                {
                    // Add parent
                    if (!parentMap.ContainsKey(x))
                    {
                        parentMap.Add(x, currNode);
                    }

                    // Check if it's target
                    if (x.Equals(target))
                    {
                        this.found = true;
                        end = true;
                    }

                    // Calculate Visited cost using haversine
                    if (!visitedCost.ContainsKey(x))
                    {
                        visitedCost.Add(x, currCost + graph.haversine(currNode, x));
                    }
                    else
                    {
                        visitedCost[x] = currCost + graph.haversine(currNode, x);
                    }
                }

                // Check if found
                if (!end)
                {
                    
                    expanded[currNode] = true;
                    //find lowest cost node to be expanded
                    currNode = findLowestCost();

                    // Check if there are no node left
                    if (currNode == null)
                    {
                        end = true;
                        found = false;
                    }
                }
                currCost = visitedCost[currNode];
            }

        }

        public void initVisit()
        {
            /* Assign expanded value false for all node */

            expanded = new Dictionary<string, bool>();
            foreach (var x in this.graph.getNodeLst())
            {
                expanded.Add(x.Key, false);
            }
        }

        public string findLowestCost()
        {
            /* Find nodes with lowest cost */

            /* KAMUS */
            /*
            minNode : string
            min : double
            */

            string minNode = null ;
            double min = 999999999;
            foreach (var x in visitedCost)
            {
                if (expanded[x.Key] != true && x.Value < min)
                {
                    minNode = x.Key;
                    min = x.Value;
                }
            }
            return minNode;
        }

        //getter
        public bool getStatus()
        {
            return this.found;
        }

       
        public List<string> getPath(string source, string target)
        {
            // get lowest path

            /* KAMUS */
            /*
             *cleaned : List<string>
             *curr : string
             */

            /* ALGORITMA */

            List<string> cleaned = new List<string>();
            
            // Reverse path
            path.Reverse();
            string curr = path[0];
            
            // While reconstruction have not reached path
            while(!curr.Equals(source))
            {
                cleaned.Add(curr);
                curr = parentMap[curr];
            }

            
            // reverse 
            cleaned.Reverse();
            return cleaned;
            }

        // get path
        public double getVisitedCost(string target)
        {
            return this.visitedCost[target];
        }
    }
}

