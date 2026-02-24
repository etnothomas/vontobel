using VontobelTest.src.formatters;
using VontobelTest.src.filters;

namespace VontobelTest.src.models
{
    public record Partner<T>(String Id, ITarget Target, IFormat<T> Format, IFilter Filter, MessageFilter MFilter)
    {
        public QueueMessage<T>? FormatMessage(IBTTermSheet message)
        {
            if (!ShouldReceiveMessage(message))
            {
                Console.WriteLine($"Message does not match filter criteria for partner {Id}");
                return null;
            }
            return new QueueMessage<T>(Format.FormatMessage(Filter.FilterMessage(message)), Target);
        }

        private bool ShouldReceiveMessage(IBTTermSheet message)
        {
            if (MFilter.GetType() == typeof(EmptyMessageFilter))
            {
                return true;
            }
            return FieldsMatcher.Match(message, MFilter.FieldName, MFilter.Operator, MFilter.FieldValue);
        }
    };
}