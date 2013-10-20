namespace Buddy.Placer
{
    public class DefaultSettings : ISettings
    {
        public DefaultSettings()
        {
            Iterations = 5;
        }

        public int Iterations { get; set; }
    }
}