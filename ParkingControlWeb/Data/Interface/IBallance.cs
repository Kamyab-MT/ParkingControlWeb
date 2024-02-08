using ParkingControlWeb.Models;
using System.Collections;

namespace ParkingControlWeb.Data.Interface
{
    public interface IBallance
    {
        public Task<Ballance> Get(string id);
        public Task<Ballance> Get(string parkingId, string userId);
        public Task<IEnumerable<Ballance>> GetAll();
        public Task<IEnumerable<Ballance>> GetAllFromUser(string userId);
        public Task<IEnumerable<Ballance>> GetAllFromParking(string parkingId);
        public bool Add(Ballance ballance);
        public bool Save();
    }
}
