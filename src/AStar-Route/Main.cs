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

            addNode(x);
        }

        private void visualizeGraph(Microsoft.Msagl.Drawing.Graph Graf)
        {
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
            button1.Visible = true;
            button2.Visible = false;
            button3.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            comboBox1.Visible = false;
            comboBox2.Visible = false;

            pictureBox1.Image = null;

            comboBox1.Items.Clear();
            comboBox2.Items.Clear();

        }

        private void addNode(Graph graph)
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();

            foreach (var x in graph.getNodeLst())
            {
                comboBox1.Items.Add(x.Key);
                comboBox2.Items.Add(x.Key);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Microsoft.Msagl.Drawing.Graph graf = currGraph.getMSAGLGraph();

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

            if (comboBox2.GetItemText(comboBox2.SelectedItem).Length > 0)
            {
                graf.FindNode(comboBox2.GetItemText(comboBox2.SelectedItem)).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Magenta;
            }
            graf.FindNode(comboBox1.GetItemText(comboBox1.SelectedItem)).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Green;

            visualizeGraph(graf);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(comboBox1.GetItemText(comboBox1.SelectedItem).Length > 0 && comboBox2.GetItemText(comboBox2.SelectedItem).Length > 0)
            {
                AStar search = new AStar(currGraph, comboBox1.SelectedItem.ToString(), comboBox2.SelectedItem.ToString());
                if (search.getStatus() == true)
                {
                    Microsoft.Msagl.Drawing.Graph graf = currGraph.getMSAGLGraph();
                    List<string> result = search.getPath();
                    String prev = result[0];
                    for (int i = 0; i < result.Count; i++)
                    {
 
                        if(!result[i].Equals(comboBox1.GetItemText(comboBox1.SelectedItem)) && !result[i].Equals(comboBox2.GetItemText(comboBox2.SelectedItem)))
                        {
                            graf.FindNode(result[i]).Attr.FillColor = Microsoft.Msagl.Drawing.Color.DodgerBlue;
                        }
                        prev = result[i];
                    }
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
    }
}
