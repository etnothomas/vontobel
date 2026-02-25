namespace VontobelTest.src.Parsers;

using System.IO;
using System.Threading;
using System.Threading.Tasks;


public interface IParser
{
    T Parse<T>(string input);
    Task<T> ParseAsync<T>(Stream stream, CancellationToken cancellationToken = default);
}

