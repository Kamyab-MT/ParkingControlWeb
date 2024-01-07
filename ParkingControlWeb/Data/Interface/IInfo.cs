using ParkingControlWeb.Models;

namespace ParkingControlWeb.Data.Interface
{
    public interface IInfo
    {

        public Task<Info> GetById(string id);
        public bool Update(Info info);
        public bool Add(Info info);
        public bool Delete(Info info);
        public bool Save();
    }
}
