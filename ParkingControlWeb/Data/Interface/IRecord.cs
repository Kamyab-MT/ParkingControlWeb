using ParkingControlWeb.Models;

namespace ParkingControlWeb.Data.Interface
{
    public interface IRecord
    {
        public Task<Record> GetAsync(string id);
        public Task<IEnumerable<Record>> GetAll();
        public Task<IEnumerable<Record>> GetAllFromParking(Parking parking);
        public Task<IEnumerable<Record>> GetAllActiveFromParking(Parking parking);
        public Task<IEnumerable<Record>> GetAllCompletedFromParking(Parking parking);
        public Task<Record> GetFromParking(Parking parking, string userId);
        public bool Add(Record record);
        public bool Save();
    }
}
