namespace Buddy.Common
{
    public abstract class BaseParser : IParser
    {
        public abstract ISotialGraph Parse(string filename);
    }
}