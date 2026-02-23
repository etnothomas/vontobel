using VontobelTest.src.models;
using System.Text.Json.Nodes;

namespace VontobelTest.src.filters
{
    public interface IFilter
    {
        public JsonObject FilterMessage(IBTTermSheet message);
    }
}