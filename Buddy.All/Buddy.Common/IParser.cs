namespace Buddy.Common
{
    internal interface IParser
    {
        ISotialGraph Parse(string filename);
    }
}