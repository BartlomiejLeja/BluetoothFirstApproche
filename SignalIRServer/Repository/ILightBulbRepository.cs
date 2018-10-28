using System.Collections.Generic;
using System.Threading.Tasks;
using SignalIRServer.Model;

namespace SignalIRServer.Repository
{
    public interface ILightBulbRepository
    {
        Task<IEnumerable<LightBulbDbModel>> GetAllLightBulbs();
        Task<LightBulbDbModel> GetLightBulb(int ID);
        Task Create(LightBulbDbModel lightBulbDbModel);
        Task<bool> Delete(int ID);
        Task<bool> Update(LightBulbDbModel lightBulbDbModel);
    }
}
