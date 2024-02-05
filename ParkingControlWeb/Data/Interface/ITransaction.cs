using ParkingControlWeb.Models;

namespace ParkingControlWeb.Data.Interface
{
    public interface ITransaction
    {
        public Task<IEnumerable<Transaction>> GetAll();
        public Task<Transaction> GetById(string id);
        public Task<IEnumerable<Transaction>> GetAllFromAUser(string userId);
        public Task<IEnumerable<Transaction>> GetAllFromAParking(string parkingId);
        public Task<IEnumerable<Transaction>> GetAllFromACar(string carId);
        public bool Add(Transaction transaction);
        public bool Save();
    }
}
