using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class DTOProduto
    {
        public int CodigoProduto { get; set; }
        public string CodigoSKU { get; set; }
        public string NomeProduto { get; set; }
        public decimal ValorProduto { get; set; }
    }
}
