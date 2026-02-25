namespace VontobelTest.src.services;

using VontobelTest.src.models;

public interface IPartnerService
{
    public List<Partner<T>> GetPartners<T>();
}
