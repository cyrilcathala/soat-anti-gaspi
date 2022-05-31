using AutoMapper;
using Soat.AntiGaspi.Api.Models;

namespace Soat.AntiGaspi.Api.Contracts.Profiles;

public class AnnonceAutoMapperProfile : Profile
{
    public AnnonceAutoMapperProfile()
    {
        CreateMap<CreateAnnonceRequest, Annonce>();

        CreateMap<Annonce, GetAnnonceResponse>();
    }
}
