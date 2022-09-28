namespace Soat.AntiGaspi.Api.Contracts.Profiles;

using AutoMapper;
using Soat.AntiGaspi.Api.Models;

public class ContactOfferAutoMapperProfile : Profile
{
    public ContactOfferAutoMapperProfile()
    {
        CreateMap<ContactRequest, ContactOffer>();
    }
}
