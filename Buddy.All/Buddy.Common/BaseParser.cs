namespace Buddy.Common
{
    public abstract class BaseParser : IParser
    {
        public abstract ISocialGraph Parse(string filename);
    }
}