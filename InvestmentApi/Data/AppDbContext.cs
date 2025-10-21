using InvestmentApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InvestmentApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>().HasData(
            new Transaction { Id = 1, Asset = "PETR4", Type = TransactionType.Buy, Amount = 1000m, Quantity = 50 },
            new Transaction { Id = 2, Asset = "VALE3", Type = TransactionType.Sell, Amount = 750m, Quantity = 10 },
            new Transaction { Id = 3, Asset = "ITUB4", Type = TransactionType.Buy, Amount = 500m, Quantity = 20 }
        );
    }
}
