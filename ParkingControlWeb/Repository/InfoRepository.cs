using Microsoft.EntityFrameworkCore;
using ParkingControlWeb.Data;
using ParkingControlWeb.Data.Interface;
using ParkingControlWeb.Models;

namespace ParkingControlWeb.Repository
{
    public class InfoRepository : IInfo
    {

        ApplicationDbContext _dbContext;

        public InfoRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Add(Info info)
        {
            _dbContext.Info.Add(info);
            return Save();
        }

        public bool Delete(Info info)
        {
            _dbContext.Info.Remove(info);
            return Save();
        }

        public bool Update(Info info)
        {
            _dbContext.Info.Update(info);
            return Save();
        }

        public async Task<Info> GetById(string id) => await _dbContext.Info.FirstOrDefaultAsync(s => s.Id == id);

        public bool Save() => _dbContext.SaveChanges() > 0 ? true : false;

    }
}
