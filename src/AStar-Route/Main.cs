using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AStar_Route
{
    public partial class Main : Form
    {
        private Graph currGraph;

        public Main()
        {
            InitializeComponent();
        }

        // Browse Button
        private void button1_Click(object sender, EventArgs e)
        {
            /* KAMUS */
            /*
             * filename : string
             * x : Graph
             * graf :  Microsoft.Msagl.Drawing.Graph
             */

            // get filename
            openFileDialog1.ShowDialog();
            string filename = openFileDialog1.FileName;

            // Construct graph
            currGraph = new Graph(filename);

            // Print Graph
            Microsoft.Msagl.Drawing.Graph graf = currGraph.getMSAGLGraph();
            clear(graf);
            visualizeGraph(graf);

            // Hide first page show second page
            button1.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            button2.Visible = true;
            button3.Visible = true;
            label1.Visible = true;
            label2.Visible = true;
            comboBox1.Visible = true;
            comboBox2.Visible = true;

            

            // Add Nodes to Combobox
            addNode(currGraph, 1);
            addNode(currGraph, 2);
        }

        // Print Graph
        private void visualizeGraph(Microsoft.Msagl.Drawing.Graph Graf)
        {
            /* Kamus */
            /*
             * renderer : Microsoft.Msagl.GraphViewerGdi.GraphRenderer
             * width, height : int
             * bitmap : Bitmap
             */

            /* ALGORITMA */

            // clear picturebox if there are any image
            if (pictureBox1.Image != null) pictureBox1.Image = null;
            Microsoft.Msagl.GraphViewerGdi.GraphRenderer renderer = new Microsoft.Msagl.GraphViewerGdi.GraphRenderer(Graf);
            
            // Calculate layout dimension
            renderer.CalculateLayout();

            int width;
            int height;

            if (Graf.Width > Graf.Height)
            {
                width = 506;
                height = (int)(Graf.Height * (width / Graf.Width));
            }
            else
            {
                height = 600;
                width = (int)(Graf.Width * (height / Graf.Height));
            }
            Graf.Attr.BackgroundColor = Microsoft.Msagl.Drawing.Color.Transparent;
            // Add graph to picture box
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
            renderer.Render(bitmap);
            pictureBox1.Image = bitmap;
        }

        /* Reset button*/
        private void button2_Click(object sender, EventArgs e)
        {
            /* KAMUS */
            /*
             * Main : Form
            */

            /* Algoritma */

            this.Hide();
            Form Main = new Main();
            Main.Show();

        }

        // Add node to combobox
        private void addNode(Graph graph, int y)
        {
            /* ALGORITMA */

            // Check Which combobox to clear
            if(y == 1)
            {
                comboBox1.Items.Clear();

            }

            else
            {
                comboBox2.Items.Clear();
            }

            /* add all node to combobox */
            foreach (var x in graph.getNodeLst())
            {
                if(y == 1)
                {
                    comboBox1.Items.Add(x.Key);
                }
                else
                {
                    comboBox2.Items.Add(x.Key);
                }
            }
        }

        /* Change graph visualization based on node choice for combobox1 */
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* KAMUS */
            /*
             * graf : Microsoft.Msagl.Drawing.Graph
             */

            /* ALGORITMA */

            Microsoft.Msagl.Drawing.Graph graf = currGraph.getMSAGLGraph();
            clear(graf);

            // Check if combobox1 has content to color
            if (comboBox1.GetItemText(comboBox1.SelectedItem).Length > 0)
            {
                graf.FindNode(comboBox1.GetItemText(comboBox1.SelectedItem)).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Green;
            }

            // color node
            graf.FindNode(comboBox2.GetItemText(comboBox2.SelectedItem)).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Magenta;

            visualizeGraph(graf);


        }

        /* Change graph visualization based on node choice for combobox2 */
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* KAMUS */

            /*
             * graf : Microsoft.Msagl.Drawing.Graph
             * combo2Content : string
             */

            /* ALGORITMA */

            Microsoft.Msagl.Drawing.Graph graf = currGraph.getMSAGLGraph();
            string combo2Content = "";

            clear(graf);

            // Check if combobox2 has content to color
            if (comboBox2.GetItemText(comboBox2.SelectedItem).Length > 0)
            {
                graf.FindNode(comboBox2.GetItemText(comboBox2.SelectedItem)).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Magenta;
                combo2Content = comboBox2.GetItemText(comboBox2.SelectedItem);
            }

            graf.FindNode(comboBox1.GetItemText(comboBox1.SelectedItem)).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Green;

            // color node
            comboBox2.Items.Clear();
            addNode(currGraph, 2);
            comboBox2.Items.Remove(comboBox1.GetItemText(comboBox1.SelectedItem));

            // Remove selecteditem from combobox1 in combobox2;
            if(combo2Content.Length > 0)
            {
                comboBox2.SelectedItem = combo2Content;
            }
            visualizeGraph(graf);
        }

        /* Submit button */
        private void button3_Click(object sender, EventArgs e)
        {
            /* KAMUS */
            /*
             * source, target : string
             * search : AStar
             * graf : Microsoft.Msagl.Drawing.Graph
             * result : List <string>
             * prev : string
             * edge : Microsoft.Msagl.Drawing.Edge
             */

            /* Algoritma */


            // Check if source and target node has been chosen
            if(comboBox1.GetItemText(comboBox1.SelectedItem).Length > 0 && comboBox2.GetItemText(comboBox2.SelectedItem).Length > 0)
            {

                string source = comboBox1.GetItemText(comboBox1.SelectedItem);
                string target = comboBox2.GetItemText(comboBox2.SelectedItem);
                Microsoft.Msagl.Drawing.Edge edge;

                // Init search
                AStar search = new AStar(currGraph, source.ToString(), target.ToString());
                
                // Check if there are path
                if (search.getStatus() == true)
                {
                    Microsoft.Msagl.Drawing.Graph graf = currGraph.getMSAGLGraph();
                    // Get shortest path
                    List<string> result = search.cleanPath(source, target);
                    string prev = source;
                    
                    // Color node and edges
                    for (int i = 0; i < result.Count; i++)
                    {
                        if(!result[i].Equals(comboBox1.GetItemText(comboBox1.SelectedItem)) && !result[i].Equals(comboBox2.GetItemText(comboBox2.SelectedItem)))
                        {
                            graf.FindNode(result[i]).Attr.FillColor = Microsoft.Msagl.Drawing.Color.DodgerBlue;
                        }

                        edge = currGraph.filterEdge(prev, result[i]);
                        edge.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                        prev = result[i];
                    }


                    // Print results
                    visualizeGraph(graf);
                    label3.Visible = true;
                    label3.Text = "Cost : " + search.getVisitedCost(source, target).ToString();
                }
                else
                {
                    MessageBox.Show("No Path Found");
                }
            }
            else
            {
                MessageBox.Show("Complete the field first!");
            }
        }

        /* Clear Msagl Graf from customized attribute */
        private void clear(Microsoft.Msagl.Drawing.Graph Graf)
        {
            /* ALGORITMA */

            // Color all node to transparent
            foreach (var x in currGraph.getNodeLst())
            {
                Graf.FindNode(x.Key).Attr.FillColor = Microsoft.Msagl.Drawing.Color.White;
            }

            // Color all edge to black
            foreach(var x in currGraph.getGUIEdge())
            {
                x.Value.Attr.Color = Microsoft.Msagl.Drawing.Color.Black;
            }
        }
    }
}
