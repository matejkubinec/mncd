using MNCD.Core;

namespace MNCD.Readers
{
    public interface IReader
    {
        Network FromString(string input);
    }
}