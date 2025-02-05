using Pagamenti.Repository.Model;
using Pagamenti.Shared;

namespace Pagamenti.Business;
public static class MyMapper
{
    public static PagamentoDto MapToDto(this Pagamento pagamento) => new()
    {
        Id = pagamento.Id,
        Importo = pagamento.Importo,
        DataPagamento = pagamento.DataPagamento,
        Fk_Ordine = pagamento.Fk_Ordine,
        Fk_MetodoPagamento = pagamento.Fk_MetodoPagamento
    };
    public static MetodoPagamentoDto MapToDto(this MetodoPagamento metodoPagamento) => new()
    {
        Id = metodoPagamento.Id,
        Nome = metodoPagamento.Nome
    };
}
