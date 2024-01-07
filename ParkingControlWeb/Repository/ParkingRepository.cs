using Microsoft.EntityFrameworkCore;
using ParkingControlWeb.Data;
using ParkingControlWeb.Data.Interface;
using ParkingControlWeb.Models;

namespace ParkingControlWeb.Repository
{
    public class ParkingRepository : IParking
    {
        ApplicationDbContext _dbContext;

        public ParkingRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Add(Parking parking)
        {
            _dbContext.Parkings.Add(parking);
            return Save();
        }

        public bool Delete(Parking parking)
        {
            _dbContext.Parkings.Remove(parking);
            return Save();
        }

        public bool Update(Parking parking)
        {
            _dbContext.Parkings.Update(parking);
            return Save();
        }

        public async Task<Parking> GetById(string id) => await _dbContext.Parkings.FirstOrDefaultAsync(s => s.Id == id);

        public bool Save() => _dbContext.SaveChanges() > 0 ? true : false;

    }
}
