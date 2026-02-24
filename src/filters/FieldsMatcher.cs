/* 
Not original work, taken from stackoverflow, but forgot the link
*/


using VontobelTest.src.models;

namespace VontobelTest.src.filters
{
    public class FieldsMatcher{

        public static bool Match(IBTTermSheet message, string property, string operatorType, object value)
        {
            var propValue = GetPropertyValue(message, property);
            if (propValue == null) return false;

            return operatorType switch
            {
                "Equals" => propValue.Equals(value),
                "NotEquals" => !propValue.Equals(value),
                _ => throw new ArgumentException($"Unsupported operator type: {operatorType}"),
            };
        }
        private static object GetPropertyValue(object src, string propName)
        {
            if (src == null) throw new ArgumentException("Value cannot be null.", "src");
            if (propName == null) throw new ArgumentException("Value cannot be null.", "propName");

            if(propName.Contains("."))
            {
                var temp = propName.Split(new char[] { '.' }, 2);
                return GetPropertyValue(GetPropertyValue(src, temp[0]), temp[1]);
            }
            else
            {
                var prop = src.GetType().GetProperty(propName);
                return prop != null ? prop.GetValue(src, null) : null;
            }
        }
    }
}