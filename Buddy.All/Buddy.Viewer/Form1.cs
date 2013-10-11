using System;
using System.Windows.Forms;

namespace SimpleVisualization
{
    public partial class Form1 : Form
    {
        private Graph graph;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            graph = new Graph();

            var r1 = new Randomize();
            r1.CreateRandomCraph(graph, 30, 70, pictureBox1.Width, pictureBox1.Height);

            var b1 = new BitMapDrawer();
            b1.SampleDraw(pictureBox1.Width, pictureBox1.Height, pictureBox1, graph);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}