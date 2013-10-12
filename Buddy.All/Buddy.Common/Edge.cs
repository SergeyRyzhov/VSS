namespace Buddy.Common
{
    public struct Edge
    {
        public int Id { get; set; }

        public Vertex U { get; set; }

        public Vertex V { get; set; }

        public double Weight { get; set; }
    }
}