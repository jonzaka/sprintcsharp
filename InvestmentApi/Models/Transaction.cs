namespace InvestmentApi.Models;

public enum TransactionType { Buy, Sell }

public class Transaction
{
    public int Id { get; set; }
    public string Asset { get; set; } = default!;
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }          // valor total R$
    public int Quantity { get; set; }
    public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;
}
