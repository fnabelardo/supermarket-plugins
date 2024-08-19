using CoreBusiness;
using Microsoft.EntityFrameworkCore;
using UseCases.DataStorePluginInterfaces;

namespace Plugins.DataStore.SQL;

public class TransactionSQLRepository : ITransactionRepository
{
    private readonly MarketContext _db;

    public TransactionSQLRepository(MarketContext db)
    {
        _db = db;
    }

    public IEnumerable<Transaction> GetByDayAndCashier(DateTime date, string cashierName)
    {
        if (string.IsNullOrWhiteSpace(cashierName))
        {
            return _db.Transactions.Where(x => x.TimeStamp.Date == date.Date);
        }

        return _db.Transactions.Where(x =>
            EF.Functions.Like(x.CashierName, $"%{cashierName}%") &&
            x.TimeStamp.Date == date.Date);
    }

    public IEnumerable<Transaction> Search(string cashierName, DateTime startDate, DateTime endDate)
    {
        if (string.IsNullOrWhiteSpace(cashierName))
        {
            return _db.Transactions.Where(x =>
                x.TimeStamp.Date >= startDate.Date &&
                x.TimeStamp.Date <= endDate.Date.AddDays(1));
        }

        return _db.Transactions.Where(x =>
            EF.Functions.Like(x.CashierName, $"%{cashierName}%") &&
            x.TimeStamp.Date >= startDate.Date &&
            x.TimeStamp.Date <= endDate.Date.AddDays(1));
    }

    public void AddTransaction(string cashierName, int productId, string productName, double productPrice,
        int beforeQty,
        int soldQty)
    {
        var transaction = new Transaction
        {
            ProductId = productId,
            ProductName = productName,
            TimeStamp = DateTime.Now,
            Price = productPrice,
            BeforeQty = beforeQty,
            SoldQty = soldQty,
            CashierName = cashierName
        };

        _db.Transactions.Add(transaction);
        _db.SaveChanges();
    }

    public IEnumerable<Transaction> GetByDayTransactions(DateTime date)
    {
        return _db.Transactions.Where(x => x.TimeStamp.Date == date.Date);
    }
}