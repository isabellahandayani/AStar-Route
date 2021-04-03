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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string filename = openFileDialog1.FileName;
            Graph x = new Graph(filename);
            Microsoft.Msagl.Drawing.Graph graf = x.getMSAGLGraph();
            visualizeGraph(graf);


        }

        private void visualizeGraph(Microsoft.Msagl.Drawing.Graph Graf)
        {
            Microsoft.Msagl.GraphViewerGdi.GraphRenderer renderer = new Microsoft.Msagl.GraphViewerGdi.GraphRenderer(Graf);
            renderer.CalculateLayout();

            int width;
            int height;

            if (Graf.Width > Graf.Height)
            {
                width = 822;
                height = (int)(Graf.Height * (width / Graf.Width));
            }
            else
            {
                height = 506;
                width = (int)(Graf.Width * (height / Graf.Height));
            }
            Graf.Attr.BackgroundColor = Microsoft.Msagl.Drawing.Color.Transparent;
            // Add graph to picture box
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
            renderer.Render(bitmap);
            pictureBox1.Image = bitmap;
        }
    }
}
