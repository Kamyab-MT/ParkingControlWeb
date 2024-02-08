using Microsoft.EntityFrameworkCore;
using ParkingControlWeb.Data;
using ParkingControlWeb.Data.Interface;
using ParkingControlWeb.Models;

namespace ParkingControlWeb.Repository
{
    public class TransactionRepository : ITransaction
    {

        readonly ApplicationDbContext _dbContext;

        public TransactionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Transaction>> GetAll() => await _dbContext.Transactions.ToListAsync();

        public async Task<IEnumerable<Transaction>> GetAllFromAParking(string parkingId) => await _dbContext.Transactions.Where(s=> s.ParkingId == parkingId).ToListAsync();

        public async Task<IEnumerable<Transaction>> GetAllFromAUser(string userId) => await _dbContext.Transactions.Where(s => s.UserId == userId).ToListAsync();

        public async Task<Transaction> GetById(string id) => await _dbContext.Transactions.FirstOrDefaultAsync(s => s.Id == id);

        public bool Add(Transaction transaction)
        {
            _dbContext.Add(transaction);
            return Save();
        }

        public bool Save() => _dbContext.SaveChanges() > 0;
    }
}
