namespace VontobelTest.src.models
{
    public record MessageFilter(string FieldName, string FieldValue, string Operator);
    public record EmptyMessageFilter() : MessageFilter("", "", "");
}