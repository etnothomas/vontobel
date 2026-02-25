namespace VontobelTest.src.Parsers;

using System.Xml.Serialization;
using System.IO;


public static class XmlParser
{
    public static T ParseXml<T>(string file)
    {
        if (string.IsNullOrWhiteSpace(file))
            throw new ArgumentException("XML input is empty", nameof(file));

        var serializer = new XmlSerializer(typeof(T));
        using (StreamReader reader = new(file))
        {
            return (T)serializer.Deserialize(reader)!;
        }
        ;
    }

}
