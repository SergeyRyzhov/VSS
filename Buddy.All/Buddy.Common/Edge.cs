namespace Buddy.Common
{
    public class Edge
    {
        public int Id { get; set; }

        public Vertex U { get; set; }

        public Vertex V { get; set; }

        public double Weight { get; set; }
    }
}