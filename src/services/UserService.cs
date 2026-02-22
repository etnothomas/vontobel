using System.Text.Json.Nodes;
using VontobelTest.src.models;
using System.Xml;
using VontobelTest.src.Formatters;
using VontobelTest.src.filters;

namespace VontobelTest.src.services
{
    public class UserService : IUserService
    {
        private readonly string _partnerConfigDirectory;
        List<Partner<string>> stringPartners = [];
        List<Partner<XmlDocument>> xmlPartners = [];

        public UserService(string partnerConfigDirectory)
        {
            _partnerConfigDirectory = partnerConfigDirectory;
            InitializePartners();
        }
        public List<Partner<T>> GetPartners<T>()
        {
            if (typeof(T) == typeof(string))
            {
                return stringPartners.Cast<Partner<T>>().ToList();
            }
            else if (typeof(T) == typeof(XmlDocument))
            {
                return xmlPartners.Cast<Partner<T>>().ToList();
            }
            else
            {
                throw new ArgumentException($"Unsupported partner type requested: {typeof(T)}");
            }
        }
        private JsonObject[] ReadPartnerConfigs() {
            if (string.IsNullOrWhiteSpace(_partnerConfigDirectory) || !Directory.Exists(_partnerConfigDirectory))
            {
                return Array.Empty<JsonObject>();
            }

            var files = Directory.GetFiles(_partnerConfigDirectory, "*.json");
            var configs = new List<JsonObject>(files.Length);

            foreach (var file in files)
            {
                try
                {
                    var text = File.ReadAllText(file);
                    var node = JsonNode.Parse(text);
                    if (node is JsonObject obj)
                    {
                        configs.Add(obj);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to read partner config '{file}': {ex.Message}");
                }
            }

            return configs.ToArray();
        }

        private void InitializePartners() {
            var configs = ReadPartnerConfigs();

            foreach (var config in configs)
            {
                try
                {
                    var targetFormat = config["targetFormat"]?.GetValue<string>() ?? throw new ArgumentException("Missing targetFormat in config");
                    var targetType = config["targetType"]?.GetValue<string>() ?? throw new ArgumentException("Missing targetType in config");

                    if (targetFormat == "text")
                    {
                        stringPartners.Add(createMailPartner(config));
                    }
                    else if (targetFormat == "xml")
                    {
                        xmlPartners.Add(createXmlPartner(config));
                    }
                    else
                    {
                        Console.Error.WriteLine($"Unsupported target type in config: {targetType}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create partner from config: {ex.Message}");
                }
            }
        }



        private Partner<string> createMailPartner(JsonObject config) {
            var partnerId = config["partnerid"]?.GetValue<string>() ?? throw new ArgumentException("Missing partnerid in config");
            var targetType = config["targetType"]?.GetValue<string>() ?? throw new ArgumentException("Missing targetType in config");
            var targetFormat = config["targetFormat"]?.GetValue<string>() ?? throw new ArgumentException("Missing targetFormat in config");
            var targetDestination = config["targetDestination"]?.GetValue<string>() ?? throw new ArgumentException("Missing targetDestination in config");
            var outputFormat = config["outputFormat"]?.GetValue<string>() ?? throw new ArgumentException("Missing outputFormat in config");
            var filter = config["fieldsFilter"]?.GetValue<string>() ?? throw new ArgumentException("Missing fieldsFilter in config");
            var messageFilter = GetMessageFilter(config) ?? new EmptyMessageFilter();

            var target = GetTarget(targetType, targetDestination, targetFormat);
            var format = GetMailFormat(outputFormat);
            var filterImplementation = GetFieldsFilter(filter);

            return new Partner<string>(partnerId, target, format, filterImplementation, messageFilter);
        }

        private Partner<XmlDocument> createXmlPartner(JsonObject config) {
            var partnerId = config["partnerid"]?.GetValue<string>() ?? throw new ArgumentException("Missing partnerid in config");
            var targetType = config["targetType"]?.GetValue<string>() ?? throw new ArgumentException("Missing targetType in config");
            var targetFormat = config["targetFormat"]?.GetValue<string>() ?? throw new ArgumentException("Missing targetFormat in config");
            var targetDestination = config["targetDestination"]?.GetValue<string>() ?? throw new ArgumentException("Missing targetDestination in config");
            var outputFormat = config["outputFormat"]?.GetValue<string>() ?? throw new ArgumentException("Missing outputFormat in config");
            var fieldsFilter = config["fieldsFilter"]?.GetValue<string>() ?? throw new ArgumentException("Missing fieldsFilter in config");
            var messageFilter = GetMessageFilter(config) ?? new EmptyMessageFilter();

            var target = GetTarget(targetType, targetDestination, targetFormat);
            var format = GetXmlFormat(outputFormat);
            var filterImplementation = GetFieldsFilter(fieldsFilter);

            return new Partner<XmlDocument>(partnerId, target, format, filterImplementation, messageFilter);
        }

        private ITarget GetTarget(string targetType, string targetDestination, string targetFormat)
        {
            return targetType switch
            {
                "file" => new FileTarget(targetDestination, targetType, targetFormat),
                "email" => new EmailTarget(targetDestination, targetType, targetFormat),
                _ => throw new ArgumentException($"Unsupported target type given: {targetType}"),
            };
        }

        private static IFormat<XmlDocument> GetXmlFormat(string format)
        {
            return format switch
            {
                "InstrumentNotificationXmlFormat" => new InstrumentNotificationXmlFormat(),
                _ => throw new ArgumentException($"Unsupported output format given: {format}"),
            };
        }

        private static IFormat<string> GetMailFormat(string format)
        {
            return format switch
            {
                "MailFormat" => new MailFormat(),
                _ => throw new ArgumentException($"Unsupported output format given: {format}"),
            };
        }

        private static IFilter GetFieldsFilter(string filter)
        {
            switch (filter)
            {
                case "NotificationFilter": return new InstrumentNotificationFilter();
                case "MailFilter": return new MailFilter();
                default: 
                    throw new ArgumentException($"Unsupported filter given: {filter}");
            }
        }

        private static MessageFilter? GetMessageFilter(JsonObject config)
        {
            if (config["messageFilter"] is not JsonObject filterNode) return new EmptyMessageFilter();

            var fieldName = filterNode["property"]?.GetValue<string>();
            var fieldValue = filterNode["value"]?.GetValue<string>();
            var operatorType = filterNode["operator"]?.GetValue<string>();

            if (fieldName == null || fieldValue == null || operatorType == null)
                return new EmptyMessageFilter();

            return new MessageFilter(fieldName, fieldValue, operatorType);
        }
    }
}