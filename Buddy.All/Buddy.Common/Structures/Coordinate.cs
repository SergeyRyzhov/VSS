namespace Buddy.Common.Structures
{
    public class Coordinate
    {
        public Coordinate()
        {
            X = 0;
            Y = 0;
        }

        public Coordinate(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public float FloatX
        {
            get
            {
                return (float)X;
            }
        }

        public float FloatY
        {
            get
            {
                return (float)Y;
            }
        }
    }
}