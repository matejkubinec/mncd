using MNCD.Core;

namespace MNCD.Writers
{
    public interface IWriter
    {
        string ToString(Network network);
    }
}