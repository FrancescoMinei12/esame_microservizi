using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagamenti.Shared;

public class PagamentoDto
{
    public int Id { get; set; }
    public decimal Importo { get; set; }
    public DateTime DataPagamento { get; set; }
    public int Fk_Ordine { get; set; }
    public int Fk_MetodoPagamento { get; set; }
}
