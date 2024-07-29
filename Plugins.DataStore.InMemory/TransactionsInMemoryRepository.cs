using CoreBusiness;
using UseCases.DataStorePluginInterfaces;

namespace MVCCourse.Models;

public class TransactionsInMemoryRepository : ITransactionRepository
{
    private List<CoreBusiness.Transaction> _transactions = new List<CoreBusiness.Transaction>();

    public IEnumerable<CoreBusiness.Transaction> GetByDayAndCashier(DateTime date, string cashierName)
    {
        if (string.IsNullOrWhiteSpace(cashierName))
        {
            return _transactions.Where(x => x.TimeStamp.Date == date.Date);
        }

        return _transactions.Where(x =>
            x.CashierName.ToLower().Contains(cashierName.ToLower()) && x.TimeStamp.Date == date.Date);
    }

    public IEnumerable<CoreBusiness.Transaction> Search(string cashierName, DateTime startDate, DateTime endDate)
    {
        if (string.IsNullOrWhiteSpace(cashierName))
        {
            return _transactions.Where(x => x.TimeStamp >= startDate.Date && x.TimeStamp <= endDate.Date.AddDays(1));
        }

        return _transactions.Where(x =>
            x.CashierName.ToLower().Contains(cashierName.ToLower()) && x.TimeStamp >= startDate.Date &&
            x.TimeStamp <= endDate.Date.AddDays(1));
    }

    public IEnumerable<Transaction> GetByDayTransactions(DateTime date)
    {
        return _transactions.Where(x =>
            x.TimeStamp.Date == date.Date);
    }

    public void AddTransaction(string cashierName, int productId, string productName, double productPrice,
        int beforeQty,
        int soldQty)
    {
        var transaction = new CoreBusiness.Transaction
        {
            ProductId = productId,
            ProductName = productName,
            TimeStamp = DateTime.Now,
            Price = productPrice,
            BeforeQty = beforeQty,
            SoldQty = soldQty,
            CashierName = cashierName
        };

        if (_transactions != null && _transactions.Count > 0)
        {
            var maxId = _transactions.Max(x => x.TransactionId);
            transaction.TransactionId = maxId + 1;
        }
        else
        {
            transaction.TransactionId = 1;
        }

        _transactions.Add(transaction);
    }
}