namespace Soat.AntiGaspi.Api.Contracts.Profiles;

using AutoMapper;
using Soat.AntiGaspi.Api.Models;

public class OfferAutoMapperProfile : Profile
{
    public OfferAutoMapperProfile()
    {
        CreateMap<CreateOfferRequest, Offer>();

        CreateMap<Offer, GetOfferResponse>();
    }
}
