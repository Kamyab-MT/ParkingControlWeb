using ParkingControlWeb.Models;

namespace ParkingControlWeb.Data.Interface
{
    public interface IParking
    {

        public Task<Parking> GetById(string id);
        public bool Update(Parking parking);
        public bool Add(Parking parking);
        public bool Delete(Parking parking);
        public bool Save();
    }
}
