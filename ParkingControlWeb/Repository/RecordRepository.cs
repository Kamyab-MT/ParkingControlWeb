using ParkingControlWeb.Data;
using ParkingControlWeb.Data.Interface;
using ParkingControlWeb.Models;

namespace ParkingControlWeb.Repository
{
    public class RecordRepository : IRecord
    {

        readonly ApplicationDbContext _dbContext;

        public RecordRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Record> AddToParking(Parking parking)
        {
            throw new NotImplementedException();
        }

        public Task<Record> GetAllFromParking(Parking parking)
        {
            throw new NotImplementedException();
        }

        public Task<Record> GetFromParking(Parking parking, string userId)
        {
            throw new NotImplementedException();
        }

        public bool Save() => _dbContext.SaveChanges() > 0 ? true : false;
    }
}
