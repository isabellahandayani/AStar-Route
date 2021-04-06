using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AStar_Route
{
    class Graph
    {
        // Attribute

        // MSAGL graph
        private Microsoft.Msagl.Drawing.Graph MSAGLform;

        // Dictionary of nodes and its point
        private Dictionary<string, Point> nodeLst;

        // Adjacency Dictionary
        private Dictionary<string, List<string>> adjLst;

        // Edge of MSAGL graph
        private Dictionary<(string, string), Microsoft.Msagl.Drawing.Edge> GUIEdge;


        //ctor
        public Graph(String filepath)
        {
            /* Kamus */
            /*
            file, nodeInfo, edgeInfo : array of string
            numNodes : int
            source, target : string
            edge : var
            */

            /* Algoritma */

            // init Attribute
            nodeLst = new Dictionary<string, Point>();
            MSAGLform = new Microsoft.Msagl.Drawing.Graph();
            adjLst = new Dictionary<string, List<string>>();
            GUIEdge = new Dictionary<(string, string), Microsoft.Msagl.Drawing.Edge>();


            // Read file
            string [] file = System.IO.File.ReadAllLines(filepath);
            
            // get number of nodes
            int numNodes = int.Parse(file[0]);



            // saves all nodes and its point 
            for(int i = 1; i < (numNodes + 1); i++)
            {
                string[] nodeInfo = file[i].Split(' ');
                nodeLst.Add (nodeInfo[2], new Point(Double.Parse(nodeInfo[0]), Double.Parse(nodeInfo[1])));
                adjLst.Add(nodeInfo[2], new List<string>());
            }

            // Saves edge between node and create MSAGL Graph
            for(int j = nodeLst.Count + 1; j < file.Length; j++)
            {
                string[] edgeInfo = file[j].Split(' ');
                for(int k = 0; k < numNodes; k++)
                {
                    if(edgeInfo[k].Equals("1"))
                    {
                        string source = nodeLst.ElementAt(j - numNodes - 1).Key;
                        string target = nodeLst.ElementAt(k).Key;

                        adjLst[source].Add(target);

                        if(!adjLst[target].Contains(source))
                        { 
                            var edge = MSAGLform.AddEdge(source, Math.Round(haversine(source, target), 5).ToString(), target);
                            GUIEdge.Add((source, target), edge);
                            edge.Attr.ArrowheadAtTarget = Microsoft.Msagl.Drawing.ArrowStyle.None;
                        }
                    }
                }
            }
        }


        // Haversine formula
        public double haversine(string source, string target)
        {
            /* KAMUS */
            /*
               lon, lat, a, d : double
             */

            /* ALGORITMA */


            // Haversine formula taken from http://1.bp.blogspot.com/-eIVzIqcs_ik/U4xLXqpgBMI/AAAAAAAAQyw/vRrNAYU3U2E/s1600/Haversine+method+bakhtyiar.png
            double lon = (Math.PI/180) * (nodeLst[target].getLon() - nodeLst[source].getLon());
            double lat = (Math.PI/180) *(nodeLst[target].getLat() - nodeLst[source].getLat());
            double a = Math.Pow(Math.Sin(lat / 2), 2) + Math.Cos((Math.PI / 180) * nodeLst[target].getLat()) * Math.Cos((Math.PI / 180) * nodeLst[source].getLat()) * Math.Pow(Math.Sin(lon / 2), 2);
            double d = Math.Sqrt(a);
            return 2 * 6371 * Math.Asin(d);   
        }

        

        // Getter
        public Microsoft.Msagl.Drawing.Graph getMSAGLGraph()
        {
            return MSAGLform;
        }

        public Dictionary<string, Point> getNodeLst()
        {
            return this.nodeLst;
        }


        public Dictionary<string, List<string>> getAdjLst()
        {
            return this.adjLst;
        }

        public Dictionary<(string, string), Microsoft.Msagl.Drawing.Edge> getGUIEdge()
        {
            return this.GUIEdge;
        }

        // find edge between prev and next
        public Microsoft.Msagl.Drawing.Edge filterEdge(string prev, string next)
        {
            /*KAMUS*/
            /*
             edge = Microsoft.MSAGL.Drawing.Edge

            */

            /* ALGORITMA */

            Microsoft.Msagl.Drawing.Edge edge;

            if (GUIEdge.ContainsKey((prev,  next)))
            {
                edge = GUIEdge[(prev, next)];
            }
            else
            {
                edge = GUIEdge[(next, prev)];
            }

            return edge;
        }
    }
}
