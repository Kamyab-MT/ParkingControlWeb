using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Record>> GetAll() =>
            await _dbContext.Records.Where(s => s.Status == 1 ).ToListAsync();

        public async Task<IEnumerable<Record>> GetAllFromParking(Parking parking) => 
            await _dbContext.Records.Where(s => s.ParkingId == parking.Id).ToListAsync();

        public async Task<IEnumerable<Record>> GetAllActiveFromParking(Parking parking) => 
            await _dbContext.Records.Where(s => ( s.Status == 0 && s.ParkingId == parking.Id)).ToListAsync();
        public async Task<IEnumerable<Record>> GetAllPendingFromParking(Parking parking) =>
            await _dbContext.Records.Where(s => ( (s.Status == -1 || s.Status == 0) && s.ParkingId == parking.Id)).ToListAsync();

        public async Task<IEnumerable<Record>> GetAllCompletedFromParking(Parking parking) => 
            await _dbContext.Records.Where(s => (s.Status == 1 && s.ParkingId == parking.Id)).ToListAsync();

        public async Task<Record> GetFromParking(Parking parking, string userId) => 
            await _dbContext.Records.FirstOrDefaultAsync(s => (s.UserId == userId && s.ParkingId == parking.Id));

        public bool Add(Record record)
        {
            _dbContext.Records.Add(record);
            return Save();
        }

        public bool Save() => _dbContext.SaveChanges() > 0 ? true : false;

        public async Task<Record> GetAsync(string id) => await _dbContext.Records.FirstOrDefaultAsync(s => s.Id == id);

    }
}
