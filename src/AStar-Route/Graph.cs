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
        private Dictionary<(string, string), double> adjLst;
        

        public Graph(String filepath)
        {
            nodeLst = new Dictionary<string, Point>();
            adjLst = new Dictionary<(string, string), double>();
            MSAGLform = new Microsoft.Msagl.Drawing.Graph();
            
            string [] file = System.IO.File.ReadAllLines(filepath);
            int numNodes = int.Parse(file[0]);



            // saves all nodes 
            for(int i = 1; i < (numNodes + 1); i++)
            {
                string[] nodeInfo = file[i].Split(' ');
                nodeLst.Add (nodeInfo[2], new Point(Double.Parse(nodeInfo[0]), Double.Parse(nodeInfo[1])));
            }

            for(int j = nodeLst.Count + 1; j < file.Length; j++)
            {
                string[] edgeInfo = file[j].Split(' ');
                if(!edgeInfo[0].Equals(edgeInfo[1]) && !adjLst.ContainsKey((edgeInfo[1], edgeInfo[0])))
                {
                    adjLst[(edgeInfo[0], edgeInfo[1])] = haversine(edgeInfo[0],  edgeInfo[1]);
                    MSAGLform.AddEdge(edgeInfo[0], haversine(edgeInfo[0], edgeInfo[1]).ToString(), edgeInfo[1]);
                    System.Diagnostics.Debug.WriteLine(haversine(edgeInfo[0], edgeInfo[1]));
                }
            }

        }


        public double haversine(string source, string target)
        {

            // Haversine formula taken from https://user-images.githubusercontent.com/2789198/27240436-e9a459da-52d4-11e7-8f84-f96d0b312859.png
            double lon = nodeLst[source].getLon() - nodeLst[target].getLon();
            double lat = nodeLst[source].getLat() - nodeLst[target].getLat();
            double a = Math.Pow(Math.Sin(lat / 2), 2) + Math.Cos(nodeLst[target].getLat()) * Math.Cos(nodeLst[source].getLat()) * Math.Pow(Math.Sin(lon / 2), 2);
            double c = 2 * a * Math.Asin(Math.Sqrt(a));
            return 6367 * c;
        }


        // Getter
        public Microsoft.Msagl.Drawing.Graph getMSAGLGraph()
        {
            return MSAGLform;
        }
    }
}
