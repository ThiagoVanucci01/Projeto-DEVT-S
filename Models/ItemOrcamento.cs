namespace Projeto_DEVT_S.Models
{
    public class ItemOrcamento
    {
        public Guid ItemOrcamentoId { get; set; }
        public Guid OrcamentoId { get; set; }
        public Guid ServicoId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal? Subtotal { get; set; }
        public Servico? Servico { get; set; }
        public Orcamento? Orcamento { get; set; }
    }
}
