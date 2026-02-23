using System.Xml;
using VontobelTest.src.models;

namespace VontobelTest.src.services
{
    public interface IPartnerService
    {
        public List<Partner<T>> GetPartners<T>();
    }
}