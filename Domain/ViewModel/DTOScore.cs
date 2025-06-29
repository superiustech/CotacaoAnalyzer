using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class DTOScore
    {
        public int CodigoScore { get; set; }
        public int PesoValor { get; set; }
        public int PesoFreteIncluso { get; set; }
        public int PesoPrazoEntrega { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
