namespace Buddy.Placer
{
    public interface IPersistable
    {
        void Persist(string fileName, double[] x, double[] y);

        void Load(string fileName, out double[] x, out double[] y);
    }
}