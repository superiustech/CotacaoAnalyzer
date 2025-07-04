using AutoMapper;
using Domain.Entities;
using Domain.ViewModel;
using Domain.ViewModel.Requests;

namespace Infra.Mapping
{
    public class CotacaoProfile : Profile
    {
        public CotacaoProfile()
        {
            CreateMap<DTOCotacao, CWCotacao>()
                .ForMember(dest => dest.nCdCotacao, opt => opt.MapFrom(src => src.CodigoCotacao))
                .ForMember(dest => dest.sDsCotacao, opt => opt.MapFrom(src => src.Descricao))
                .ForMember(dest => dest.tDtCotacao, opt => opt.MapFrom(src => src.Data))
                .ForMember(dest => dest.dVlTotal, opt => opt.MapFrom(src => src.ValorTotal))
                .ForMember(dest => dest.bFlFreteIncluso, opt => opt.MapFrom(src => src.FreteIncluso))
                .ForMember(dest => dest.lstCotacaoItem, opt => opt.MapFrom(src => src.Itens));

            CreateMap<CWCotacao, DTOCotacao>()
                .ForMember(dest => dest.CodigoCotacao, opt => opt.MapFrom(src => src.nCdCotacao))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.sDsCotacao))
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.tDtCotacao))
                .ForMember(dest => dest.ValorTotal, opt => opt.MapFrom(src => src.dVlTotal))
                .ForMember(dest => dest.FreteIncluso, opt => opt.MapFrom(src => src.bFlFreteIncluso))
                .ForMember(dest => dest.Itens, opt => opt.MapFrom(src => src.lstCotacaoItem));
        }
    }
}
