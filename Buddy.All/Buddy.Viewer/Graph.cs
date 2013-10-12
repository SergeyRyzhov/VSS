using System.Collections.Generic;
using System.Drawing;

namespace SimpleVisualization
{
    public class Graph
    {
        public List<GraphNode> nodeList = new List<GraphNode>();
        public List<GraphEdge> edgeList = new List<GraphEdge>();

        public void DrawGraph(Graphics g)
        {
            foreach (var ge in edgeList)
            {
                ge.DrawEdge(g);
            }
            foreach (var gv in nodeList)
            {
                gv.DrawNode(g);
            }
        }

        public void AddNode(int x, int y, int name, int radius)
        {
            var c1 = new GraphNode(x, y, name, radius);
            nodeList.Add(c1);
        }

        public void AddEdge(GraphNode firstpoint, GraphNode secondpoint, int cost)
        {
            var c2 = new GraphEdge(firstpoint, secondpoint, cost);
            edgeList.Add(c2);
        }

        public int GetNodeX(int name)
        {
            for (var i = 0; i < nodeList.Count; i++)
            {
                if (nodeList[i].Name == name)
                    return nodeList[i].X;
            }

            return -1;
        }

        public GraphNode GetNode(int name)
        {
            for (var i = 0; i < nodeList.Count; i++)
            {
                if (nodeList[i].Name == name)
                    return nodeList[i];
            }

            return null;
        }
    }

    public class GraphNode
    {
        private int x, y;
        private int name;

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public int Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Radius { get; set; }

        public GraphNode(int x, int y, int name, int radius)
        {
            this.x = x;
            this.y = y;
            this.name = name;
            this.Radius = radius;
        }

        public void DrawNode(Graphics g)
        {
            var p = new Pen(Color.SandyBrown, 2);

            g.DrawEllipse(p, x, y, Radius, Radius);

            p.Dispose();
        }
    }

    public class GraphEdge
    {
        public GraphNode firstpoint;
        public GraphNode secondpoint;

        public int Cost { get; set; }

        public GraphEdge(GraphNode firstpoint, GraphNode secondpoint, int cost)
        {
            this.firstpoint = firstpoint;
            this.secondpoint = secondpoint;
            this.Cost = cost;
        }

        public void DrawEdge(Graphics g)
        {
            var p = new Pen(Color.SlateBlue, 2);

            //p.EndCap = LineCap.ArrowAnchor;

            g.DrawLine(p, firstpoint.X + firstpoint.Radius / 2, firstpoint.Y + firstpoint.Radius / 2,
                     secondpoint.X + secondpoint.Radius / 2, secondpoint.Y + secondpoint.Radius / 2);
            /*
            g.DrawLines(Pens.Black, );*/

            //g.SmoothingMode = SmoothingMode.AntiAlias;

            p.Dispose();
        }
    }
}