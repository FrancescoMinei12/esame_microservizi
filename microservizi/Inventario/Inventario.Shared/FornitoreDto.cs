using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventario.Shared;

public class FornitoreDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Indirizzo { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
}
