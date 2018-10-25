using MongoDB.Driver;
using SignalIRServer.Model;

namespace SignalIRServer.MongoDbContexts
{
    public interface ILightBulbContext
    {
        IMongoCollection<LightBulbDbModel> LightBulbDBModel { get; }
    }
}
