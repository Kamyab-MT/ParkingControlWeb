using ParkingControlWeb.Models;

namespace ParkingControlWeb.Data.Interface
{
    public interface IRecord
    {
        public Task<Record> GetAllFromParking(Parking parking);
        public Task<Record> AddToParking(Parking parking);
        public Task<Record> GetFromParking(Parking parking, string userId);
        public bool Save();
    }
}
