using AutoMapper;
using Domain.Entities;
using Domain.ViewModel;

namespace Infra.Mapping
{
    public class ProdutoProfile : Profile
    {
        public ProdutoProfile()
        {
            CreateMap<DTOProduto, CWProduto>()
                .ForMember(dest => dest.nCdProduto, opt => opt.MapFrom(src => src.CodigoProduto))
                .ForMember(dest => dest.sNmProduto, opt => opt.MapFrom(src => src.NomeProduto))
                .ForMember(dest => dest.sCdProduto, opt => opt.MapFrom(src => src.CodigoSKU))
                .ForMember(dest => dest.dVlUnitario, opt => opt.MapFrom(src => src.ValorProduto));

            CreateMap<CWProduto, DTOProduto>()
                .ForMember(dest => dest.CodigoProduto, opt => opt.MapFrom(src => src.nCdProduto))
                .ForMember(dest => dest.NomeProduto, opt => opt.MapFrom(src => src.sNmProduto))
                .ForMember(dest => dest.CodigoSKU, opt => opt.MapFrom(src => src.sCdProduto))
                .ForMember(dest => dest.ValorProduto, opt => opt.MapFrom(src => src.dVlUnitario));
        }
    }
}
