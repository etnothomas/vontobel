using System.Xml;
using VontobelTest.src.models;

namespace VontobelTest.src.services
{
    public interface IUserService
    {
        public List<Partner<T>> GetPartners<T>();
    }
}