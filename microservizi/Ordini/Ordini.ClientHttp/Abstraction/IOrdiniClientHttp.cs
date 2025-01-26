using Ordini.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordini.ClientHttp.Abstraction;

public interface IOrdiniClientHttp
{
    Task<string?> CreateOrdineAsync(OrdineDto ordineDto, CancellationToken cancellationToken = default);
    Task<OrdineDto?> GetOrdineByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<OrdineDto>> GetAllOrdiniAsync(CancellationToken cancellationToken = default);
}
