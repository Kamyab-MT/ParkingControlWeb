using Microsoft.EntityFrameworkCore;
using ParkingControlWeb.Data;
using ParkingControlWeb.Data.Interface;
using ParkingControlWeb.Models;

namespace ParkingControlWeb.Repository
{
    public class BallanceRepository : IBallance
    {
        
        readonly ApplicationDbContext _dbContext;

        public BallanceRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Ballance> Get(string id) => await _dbContext.Ballances.FirstOrDefaultAsync(b => b.Id == id);

        public async Task<Ballance> Get(string parkingId, string userId) => await _dbContext.Ballances.FirstOrDefaultAsync(b => b.ParkingId == parkingId && b.UserId == userId);

        public async Task<IEnumerable<Ballance>> GetAll() => await _dbContext.Ballances.ToListAsync();

        public async Task<IEnumerable<Ballance>> GetAllFromParking(string parkingId) =>  await _dbContext.Ballances.Where(s=> s.ParkingId == parkingId).ToListAsync();

        public async Task<IEnumerable<Ballance>> GetAllFromUser(string userId) => await _dbContext.Ballances.Where(s => s.UserId == userId).ToListAsync();

        public bool Add(Ballance ballance)
        {
            _dbContext.Ballances.Add(ballance);
            return Save();
        }

        public bool Save() => _dbContext.SaveChanges() > 0;

    }
}
