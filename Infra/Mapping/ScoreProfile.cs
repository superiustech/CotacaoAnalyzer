using AutoMapper;
using Domain.Entities;
using Domain.ViewModel;

namespace Infra.Mapping
{
    public class ScoreProfile : Profile
    {
        public ScoreProfile()
        {
            CreateMap<DTOScore, CWScore>()
                .ForMember(dest => dest.nCdScore, opt => opt.MapFrom(src => src.CodigoScore))
                .ForMember(dest => dest.nPesoValor, opt => opt.MapFrom(src => src.PesoValor))
                .ForMember(dest => dest.nPesoFreteIncluso, opt => opt.MapFrom(src => src.PesoFreteIncluso))
                .ForMember(dest => dest.nPesoPrazoEntrega, opt => opt.MapFrom(src => src.PesoPrazoEntrega))
                .ForMember(dest => dest.tDtCriacaoPeso, opt => opt.MapFrom(src => src.DataCriacao));

            CreateMap<CWScore, DTOScore>()
                .ForMember(dest => dest.CodigoScore, opt => opt.MapFrom(src => src.nCdScore))
                .ForMember(dest => dest.PesoValor, opt => opt.MapFrom(src => src.nPesoValor))
                .ForMember(dest => dest.PesoFreteIncluso, opt => opt.MapFrom(src => src.nPesoFreteIncluso))
                .ForMember(dest => dest.PesoPrazoEntrega, opt => opt.MapFrom(src => src.nPesoPrazoEntrega))
                .ForMember(dest => dest.DataCriacao, opt => opt.MapFrom(src => src.tDtCriacaoPeso));
        }
    }
}
