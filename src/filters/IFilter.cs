using VontobelTest.src.models;

namespace VontobelTest.src.filters
{
    public interface IFilter
    {
        public Dictionary<string, string> FilterMessage(IBTTermSheet message);
    }
}