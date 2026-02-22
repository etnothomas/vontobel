using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using VontobelTest.src.models;

namespace VontobelTest.src.Formatters
{
    public class InstrumentNotificationXmlFormat : IFormat<XmlDocument>
    {
        public XmlDocument FormatMessage(Dictionary<string, string> input)
        {
            var xmlDoc = new XmlDocument();
            var root = xmlDoc.CreateElement("InstrumentNotification");
            xmlDoc.AppendChild(root);

            foreach (var kv in input)
            {
                var key = kv.Key ?? string.Empty;
                var value = kv.Value ?? string.Empty;

                var element = xmlDoc.CreateElement(key);
                element.InnerText = value;
                root.AppendChild(element);
            }

            return xmlDoc;
        }
    }
}