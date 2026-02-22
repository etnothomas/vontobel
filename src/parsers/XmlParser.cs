using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VontobelTest.src.Parsers
{
    public static class XmlParser
    {
        public static T ParseXml<T>(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
                throw new ArgumentException("XML input is empty", nameof(file));

            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StreamReader(file))
            {
                return (T)serializer.Deserialize(reader)!;
            }
        }

        public static T ParseXml<T>(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stream)!;
        }

        public static async Task<T> ParseXmlAsync<T>(Stream stream, CancellationToken cancellationToken = default)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms, cancellationToken).ConfigureAwait(false);
            ms.Position = 0;
            XmlSerializer serializer = new(typeof(T));
            return (T)serializer.Deserialize(ms)!;
        }
    }
}
