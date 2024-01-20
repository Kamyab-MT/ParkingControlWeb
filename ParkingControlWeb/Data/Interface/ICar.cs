using ParkingControlWeb.Models;

namespace ParkingControlWeb.Data.Interface
{
    public interface ICar
    {

        public List<Car> GetAll();
        public Task<Car> GetByPlateNumber(string plateNumber);
        public bool AddVisitCountToCar(Car car);
        public bool Add(Car car);
        public bool Save();

    }
}
