using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json.Nodes;
using System.Xml;
using VontobelTest.src.models;

namespace VontobelTest.src.formatters
{
    public class InstrumentNotificationXmlFormat : IFormat<XmlDocument>
    {
        public XmlDocument FormatMessage(JsonObject input)
        {
            var xmlDoc = new XmlDocument();
            var root = xmlDoc.CreateElement("InstrumentNotification");
            xmlDoc.AppendChild(root);

            foreach (var kv in input)
            {
                var key = kv.Key ?? string.Empty;
                var value = kv.Value?.ToString() ?? string.Empty;

                var element = xmlDoc.CreateElement(key);
                element.InnerText = value;
                root.AppendChild(element);
            }

            return xmlDoc;
        }
    }
}