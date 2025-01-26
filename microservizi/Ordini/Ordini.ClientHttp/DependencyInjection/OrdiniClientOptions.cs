using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordini.ClientHttp.DependencyInjection;

public class OrdiniClientOptions
{
    public const string SectionName = "OrdiniClientHttp";
    public string BaseAddress { get; set; } = string.Empty;
}
