using AutoMapper;
using Domain.Entities;
using Domain.ViewModel;

namespace Infra.Mapping
{
    public class CotacaoItemProfile : Profile
    {
        public CotacaoItemProfile()
        {
            CreateMap<DTOCotacaoItem, CWCotacaoItem>()
                .ForMember(dest => dest.nCdCotacaoItem, opt => opt.MapFrom(src => src.CodigoCotacaoItem))
                .ForMember(dest => dest.nSequencial, opt => opt.MapFrom(src => src.Sequencial))
                .ForMember(dest => dest.dVlProposto, opt => opt.MapFrom(src => src.ValorProposto))
                .ForMember(dest => dest.nPrazoEntrega, opt => opt.MapFrom(src => src.PrazoEntrega))
                .ForMember(dest => dest.Produto, opt => opt.MapFrom(src => src.Produto));

            CreateMap<CWCotacaoItem, DTOCotacaoItem>()
                .ForMember(dest => dest.CodigoCotacaoItem, opt => opt.MapFrom(src => src.nCdCotacaoItem))
                .ForMember(dest => dest.Sequencial, opt => opt.MapFrom(src => src.nSequencial))
                .ForMember(dest => dest.ValorProposto, opt => opt.MapFrom(src => src.dVlProposto))
                .ForMember(dest => dest.PrazoEntrega, opt => opt.MapFrom(src => src.nPrazoEntrega))
                .ForMember(dest => dest.Produto, opt => opt.MapFrom(src => src.Produto));
        }
    }
}