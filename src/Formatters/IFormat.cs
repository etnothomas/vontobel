using System.Reflection.Metadata.Ecma335;
using VontobelTest.src.models;

namespace VontobelTest.src.Formatters
{

    public interface IFormat<T>
    {
        T FormatMessage(Dictionary<string, string> message);
    }
}