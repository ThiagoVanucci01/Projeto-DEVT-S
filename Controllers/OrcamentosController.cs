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
    public class OrcamentosController : ControllerBase
    {
        private readonly ProjetoDevContext _context;

        public OrcamentosController(ProjetoDevContext context)
        {
            _context = context;
        }

        // GET: api/Orcamentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orcamento>>> GetOrcamentos()
        {
            return await _context.Orcamentos.ToListAsync();
        }

        // GET: api/Orcamentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Orcamento>> GetOrcamento(Guid id)
        {
            var orcamento = await _context.Orcamentos.FindAsync(id);

            if (orcamento == null)
            {
                return NotFound();
            }

            return orcamento;
        }

        // GET: api/Orcamentos/data/{data}
        [HttpGet("data/{data}")]
        public async Task<ActionResult<IEnumerable<Orcamento>>> GetOrcamentosByDate(DateTime data)
        {
            var orcamentos = await _context.Orcamentos
                .Where(o => o.Data.Date == data.Date)
                .ToListAsync();

            if (orcamentos == null || !orcamentos.Any())
            {
                return NotFound();
            }

            return orcamentos;
        }

        // GET: api/Orcamentos/cliente/{nome}
        [HttpGet("cliente/{nome}")]
        public async Task<ActionResult<IEnumerable<Orcamento>>> GetOrcamentosByClienteNome(string nome)
        {
            var orcamentos = await _context.Orcamentos
                .Include(o => o.Cliente)
                .Where(o => o.Cliente.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

            if (orcamentos == null || !orcamentos.Any())
            {
                return NotFound();
            }

            return orcamentos;
        }

        // PUT: api/Orcamentos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrcamento(Guid id, Orcamento orcamento)
        {
            if (id != orcamento.OrcamentoId)
            {
                return BadRequest();
            }

            _context.Entry(orcamento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrcamentoExists(id))
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

        // POST: api/Orcamentos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Orcamento>> PostOrcamento(Orcamento orcamento)
        {
            _context.Orcamentos.Add(orcamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrcamento", new { id = orcamento.OrcamentoId }, orcamento);
        }

        // DELETE: api/Orcamentos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrcamento(Guid id)
        {
            var orcamento = await _context.Orcamentos.FindAsync(id);
            if (orcamento == null)
            {
                return NotFound();
            }

            _context.Orcamentos.Remove(orcamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrcamentoExists(Guid id)
        {
            return _context.Orcamentos.Any(e => e.OrcamentoId == id);
        }
    }
}
