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
            using var reader = new StreamReader(file);
            return (T)serializer.Deserialize(reader)!;
        }

    }
}
