using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using SignalIRServer.Model;
using SignalIRServer.MongoDbContexts;

namespace SignalIRServer.Repository
{
    public class LightBulbRepository : ILightBulbRepository
    {
        private readonly ILightBulbContext _context;

        public LightBulbRepository(ILightBulbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LightBulbDbModel>> GetAllLightBulbs()
        {
            return await _context
                .LightBulbDBModel
                .Find(_ => true)
                .ToListAsync();
        }
        public Task<LightBulbDbModel> GetLightBulb(int ID)
        {
            FilterDefinition<LightBulbDbModel> filter = Builders<LightBulbDbModel>.Filter.Eq(m => m.ID, ID);
            return _context
                .LightBulbDBModel
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public async Task Create(LightBulbDbModel lightBulbDbModel)
        {
            await _context.LightBulbDBModel.InsertOneAsync(lightBulbDbModel);
        }
        public async Task<bool> Update(LightBulbDbModel lightBulbDbModel)
        {
            ReplaceOneResult updateResult =
                await _context
                    .LightBulbDBModel
                    .ReplaceOneAsync(
                        filter: g => g.Id == lightBulbDbModel.Id,
                        replacement: lightBulbDbModel);
            return updateResult.IsAcknowledged
                   && updateResult.ModifiedCount > 0;
        }
        public async Task<bool> Delete(int ID)
        {
            FilterDefinition<LightBulbDbModel> filter = Builders<LightBulbDbModel>.Filter.Eq(m => m.ID, ID);
            DeleteResult deleteResult = await _context
                .LightBulbDBModel
                .DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged
                   && deleteResult.DeletedCount > 0;
        }
    }
}
