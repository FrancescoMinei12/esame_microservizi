using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventario.Repository.Model;

public class Articolo
{
    public int Id { get; set; } 
    public string Nome { get; set; } 
    public string Descrizione { get; set; } 
    public decimal Prezzo { get; set; } 
    public int QuantitaDisponibile { get; set; }
    public string CodiceSKU { get; set; } 
    public string Categoria { get; set; } 
    public DateTime DataInserimento { get; set; } // Data di inserimento dell'articolo in magazzino
}
