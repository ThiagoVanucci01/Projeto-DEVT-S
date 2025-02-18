using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_DEVT_S.Data;
using Projeto_DEVT_S.Models;

namespace Projeto_DEVT_S.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItensOrcamentoController : ControllerBase
    {
        private readonly ProjetoDevContext _context;

        public ItensOrcamentoController(ProjetoDevContext context)
        {
            _context = context;
        }

        // GET: api/ItensOrcamento
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemOrcamento>>> GetItensOrcamento()
        {
            return await _context.ItensOrcamento.ToListAsync();
        }

        // GET: api/ItensOrcamento/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemOrcamento>> GetItemOrcamento(Guid id)
        {
            var itemOrcamento = await _context.ItensOrcamento.FindAsync(id);

            if (itemOrcamento == null)
            {
                return NotFound();
            }

            return itemOrcamento;
        }

        // POST: api/ItensOrcamento/CalculateSubtotal
        [HttpPost("CalculateSubtotal")]
        public ActionResult<decimal> CalculateSubtotal(ItemOrcamento itemOrcamento)
        {
            if (itemOrcamento == null)
            {
                return BadRequest("ItemOrcamento não pode ser nulo.");
            }

            decimal subtotal = itemOrcamento.Quantidade * itemOrcamento.PrecoUnitario;
            return Ok(subtotal);
        }

        // PUT: api/ItensOrcamento/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemOrcamento(Guid id, ItemOrcamento itemOrcamento)
        {
            if (id != itemOrcamento.ItemOrcamentoId)
            {
                return BadRequest();
            }

            _context.Entry(itemOrcamento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await CalculateAndUpdateOrcamentoTotal(itemOrcamento.OrcamentoId);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemOrcamentoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ItensOrcamento
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ItemOrcamento>> PostItemOrcamento(ItemOrcamento itemOrcamento)
        {
            _context.ItensOrcamento.Add(itemOrcamento);
            await _context.SaveChangesAsync();
            await CalculateAndUpdateOrcamentoTotal(itemOrcamento.OrcamentoId);

            return CreatedAtAction("GetItemOrcamento", new { id = itemOrcamento.ItemOrcamentoId }, itemOrcamento);
        }

        // DELETE: api/ItensOrcamento/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemOrcamento(Guid id)
        {
            var itemOrcamento = await _context.ItensOrcamento.FindAsync(id);
            if (itemOrcamento == null)
            {
                return NotFound();
            }

            _context.ItensOrcamento.Remove(itemOrcamento);
            await _context.SaveChangesAsync();
            await CalculateAndUpdateOrcamentoTotal(itemOrcamento.OrcamentoId);

            return NoContent();
        }

        private bool ItemOrcamentoExists(Guid id)
        {
            return _context.ItensOrcamento.Any(e => e.ItemOrcamentoId == id);
        }

        private async Task CalculateAndUpdateOrcamentoTotal(Guid orcamentoId)
        {
            var orcamento = await _context.Orcamentos
                .Include(o => o.Itens)
                .FirstOrDefaultAsync(o => o.OrcamentoId == orcamentoId);

            if (orcamento != null)
            {
                orcamento.Total = orcamento.Itens.Sum(i => i.Quantidade * i.PrecoUnitario);
                _context.Entry(orcamento).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
    }
}
