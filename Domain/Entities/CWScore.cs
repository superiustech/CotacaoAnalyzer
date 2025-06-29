using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CWScore
    {
        public int nCdScore { get; set; }
        public int nPesoValor { get; set; }
        public int nPesoFreteIncluso { get; set; }
        public int nPesoPrazoEntrega { get; set; }
        public DateTime tDtCriacaoPeso { get; set; }
    }
}
