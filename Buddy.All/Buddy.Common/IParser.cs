namespace Buddy.Common
{
    internal interface IParser
    {
        ISocialGraph Parse(string filename);
    }
}