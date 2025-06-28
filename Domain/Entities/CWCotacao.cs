namespace Domain.Entities
{
    public class CWCotacao
    {
        public int nCdCotacao { get; set; }
        public string sDsCotacao { get; set; }
        public DateTime tDtCotacao { get; set; }
        public bool bFlFreteIncluso { get; set; }
        public decimal dVlTotal { get; set; }
        public List<CWCotacaoItem> lstCotacaoItem { get; set; } = new();
    }
}
