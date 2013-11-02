namespace Buddy.Common.Structures
{
    public class ArrayCoordinate
    {
        private readonly double[] m_x;
        private readonly double[] m_y;

        public ArrayCoordinate(double[] x, double[] y)
        {
            m_x = x;
            m_y = y;
        }

        public Coordinate this[int index]
        {
            get
            {
                return new Coordinate(m_x[index], m_y[index]);
            }
            set
            {
                m_x[index] = value.X;
                m_y[index] = value.Y;
            }
        }
    }
}