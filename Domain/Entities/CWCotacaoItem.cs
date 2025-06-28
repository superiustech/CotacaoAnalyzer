using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class CWCotacaoItem
    {
        public int nCdCotacaoItem { get; set; }
        public int nSequencial { get; set; }
        public int nPrazoEntrega { get; set; }
        public decimal dVlProposto { get; set; }
        public int nCdProduto { get; set; }
        [ForeignKey("nCdProduto")]
        public CWProduto Produto { get; set; }
        public int nCdCotacao { get; set; }
        [ForeignKey("nCdCotacao")]
        public CWCotacao Cotacao { get; set; }
    }
}
