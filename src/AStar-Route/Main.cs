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

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string filename = openFileDialog1.FileName;
            Graph x = new Graph(filename);
            currGraph = x;

            Microsoft.Msagl.Drawing.Graph graf = x.getMSAGLGraph();
            visualizeGraph(graf);
            splitContainer1.Panel1.ResetText();

            button1.Visible = false;
            button2.Visible = true;
            button3.Visible = true;
            label1.Visible = true;
            label2.Visible = true;
            comboBox1.Visible = true;
            comboBox2.Visible = true;

            addNode(x, 1);
            addNode(x, 2);

        }

        private void visualizeGraph(Microsoft.Msagl.Drawing.Graph Graf)
        {
            if(pictureBox1.Image != null) pictureBox1.Image = null;
            Microsoft.Msagl.GraphViewerGdi.GraphRenderer renderer = new Microsoft.Msagl.GraphViewerGdi.GraphRenderer(Graf);
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form Main = new Main();
            Main.Show();

        }

        private void addNode(Graph graph, int y)
        {

            if(y == 1)
            {
                comboBox1.Items.Clear();

            }
            else
            {
                comboBox2.Items.Clear();
            }

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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Microsoft.Msagl.Drawing.Graph graf = currGraph.getMSAGLGraph();

            clear(graf);

            if (comboBox1.GetItemText(comboBox1.SelectedItem).Length > 0)
            {
                graf.FindNode(comboBox1.GetItemText(comboBox1.SelectedItem)).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Green;
            }

            graf.FindNode(comboBox2.GetItemText(comboBox2.SelectedItem)).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Magenta;

            visualizeGraph(graf);


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Microsoft.Msagl.Drawing.Graph graf = currGraph.getMSAGLGraph();

            clear(graf);

            if (comboBox2.GetItemText(comboBox2.SelectedItem).Length > 0)
            {
                graf.FindNode(comboBox2.GetItemText(comboBox2.SelectedItem)).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Magenta;
            }

            graf.FindNode(comboBox1.GetItemText(comboBox1.SelectedItem)).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Green;

            visualizeGraph(graf);

            comboBox2.Items.Clear();
            addNode(currGraph, 2);
            comboBox2.Items.Remove(comboBox1.GetItemText(comboBox1.SelectedItem));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(comboBox1.GetItemText(comboBox1.SelectedItem).Length > 0 && comboBox2.GetItemText(comboBox2.SelectedItem).Length > 0)
            {

                string source = comboBox1.GetItemText(comboBox1.SelectedItem);
                string target = comboBox2.GetItemText(comboBox2.SelectedItem);

                AStar search = new AStar(currGraph, source.ToString(), target.ToString());
                if (search.getStatus() == true)
                {
                    Microsoft.Msagl.Drawing.Graph graf = currGraph.getMSAGLGraph();
                    List<string> result = search.getPath(source, target);
                    Dictionary<(string, string), Microsoft.Msagl.Drawing.Edge> edgeLst = currGraph.getGUIEdge();
                    string prev = source;
                    
                    for (int i = 0; i < result.Count; i++)
                    {
                        if(!result[i].Equals(comboBox1.GetItemText(comboBox1.SelectedItem)) && !result[i].Equals(comboBox2.GetItemText(comboBox2.SelectedItem)))
                        {
                            graf.FindNode(result[i]).Attr.FillColor = Microsoft.Msagl.Drawing.Color.DodgerBlue;
                        }

                        var edge = edgeLst[(prev, result[i])];
                        edge.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                        prev = result[i];
                    }

                    var end = edgeLst[(prev, target)];
                    end.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;


                    visualizeGraph(graf);
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

        private void clear(Microsoft.Msagl.Drawing.Graph Graf)
        {
            foreach (var x in currGraph.getNodeLst())
            {
                Graf.FindNode(x.Key).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Transparent;
            }

            foreach(var x in currGraph.getGUIEdge())
            {
                x.Value.Attr.Color = Microsoft.Msagl.Drawing.Color.Black;
            }
        }
    }
}
