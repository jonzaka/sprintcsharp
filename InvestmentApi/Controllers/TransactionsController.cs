using InvestmentApi.Data;
using InvestmentApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvestmentApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly AppDbContext _db;
    public TransactionsController(AppDbContext db) => _db = db;

    // ---------- CRUD básico ----------

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetAll()
        => await _db.Transactions.AsNoTracking().ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Transaction>> GetById(int id)
        => await _db.Transactions.FindAsync(id) is { } t ? t : NotFound();

    [HttpPost]
    public async Task<ActionResult<Transaction>> Create(Transaction t)
    {
        _db.Transactions.Add(t);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = t.Id }, t);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Transaction t)
    {
        if (id != t.Id) return BadRequest();
        _db.Entry(t).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var t = await _db.Transactions.FindAsync(id);
        if (t is null) return NotFound();
        _db.Remove(t);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // ---------- Endpoints LINQ ----------

    // /api/transactions/by-asset/PETR4
    [HttpGet("by-asset/{asset}")]
    public async Task<ActionResult<IEnumerable<Transaction>>> ByAsset(string asset)
        => await _db.Transactions
                    .Where(t => t.Asset == asset)
                    .OrderByDescending(t => t.ExecutedAt)
                    .ToListAsync();

    // /api/transactions/summary
    [HttpGet("summary")]
    public async Task<ActionResult<object>> Summary()
    {
        var total = await _db.Transactions.SumAsync(t => t.Amount);
        var count = await _db.Transactions.CountAsync();
        var byType = await _db.Transactions
            .GroupBy(t => t.Type)
            .Select(g => new { Type = g.Key.ToString(), Total = g.Sum(x => x.Amount) })
            .ToListAsync();

        return new { total, count, byType };
    }

    // /api/transactions/top/2   (top N por valor)
    [HttpGet("top/{n:int}")]
    public async Task<ActionResult<IEnumerable<object>>> TopByAmount(int n = 3)
        => await _db.Transactions
                    .OrderByDescending(t => t.Amount)
                    .Take(n)
                    .Select(t => new { t.Id, t.Asset, t.Amount })
                    .ToListAsync();
}
