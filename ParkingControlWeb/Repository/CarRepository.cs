using Microsoft.EntityFrameworkCore;
using ParkingControlWeb.Data;
using ParkingControlWeb.Data.Interface;
using ParkingControlWeb.Models;

namespace ParkingControlWeb.Repository
{
    public class CarRepository : ICar
    {

        ApplicationDbContext _dbContext;

        public CarRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Car> GetAll() => _dbContext.Cars.ToList();

        public async Task<Car> GetByPlateNumber(string plateNumber) => await _dbContext.Cars.FirstOrDefaultAsync(s => s.PlateNumber == plateNumber);

        public bool AddVisitCountToCar(Car car)
        {
            car.VisitCount++;
            return Save();
        }

        public bool Add(Car car)
        {
            _dbContext.Cars.Add(car);
            return Save();
        }

        public bool Save() => _dbContext.SaveChanges() > 0;

    }
}
