using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventario.Repository.Model;

public class Fornitore
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Indirizzo { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
    public List<Articolo> Articoli { get; set; }
}
