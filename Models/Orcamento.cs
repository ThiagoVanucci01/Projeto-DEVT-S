namespace Projeto_DEVT_S.Models
{
    public class Orcamento
    {
        public Guid OrcamentoId { get; set; }
        public Guid ClienteId { get; set; }
        public DateTime Data { get; set; }
        public decimal Total { get; set; }
        public Cliente? Cliente { get; set; }
        public List<ItemOrcamento> Itens { get; set; } = new List<ItemOrcamento>();
    }
}
