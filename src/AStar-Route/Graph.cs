using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AStar_Route
{
    class Graph
    {
        private Microsoft.Msagl.Drawing.Graph MSAGLform;
        private Dictionary<string, Point> nodeLst;
        private Dictionary<string, List<string>> adjLst;
        private Dictionary<(string, string), Microsoft.Msagl.Drawing.Edge> GUIEdge;

        public Graph(String filepath)
        {
            nodeLst = new Dictionary<string, Point>();
            MSAGLform = new Microsoft.Msagl.Drawing.Graph();
            adjLst = new Dictionary<string, List<string>>();
            GUIEdge = new Dictionary<(string, string), Microsoft.Msagl.Drawing.Edge>();


            string [] file = System.IO.File.ReadAllLines(filepath);
            int numNodes = int.Parse(file[0]);



            // saves all nodes 
            for(int i = 1; i < (numNodes + 1); i++)
            {
                string[] nodeInfo = file[i].Split(' ');
                nodeLst.Add (nodeInfo[2], new Point(Double.Parse(nodeInfo[0]), Double.Parse(nodeInfo[1])));
                adjLst.Add(nodeInfo[2], new List<string>());
            }


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


        public double haversine(string source, string target)
        {

            // Haversine formula taken from https://user-images.githubusercontent.com/2789198/27240436-e9a459da-52d4-11e7-8f84-f96d0b312859.png
            double lon = (Math.PI/180) * (nodeLst[source].getLon() - nodeLst[target].getLon());
            double lat = (Math.PI/180) *(nodeLst[source].getLat() - nodeLst[target].getLat());
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
    }
}
