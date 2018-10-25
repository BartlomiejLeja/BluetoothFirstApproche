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

        public async Task Create(LightBulbDbModel game)
        {
            await _context.LightBulbDBModel.InsertOneAsync(game);
        }
        public async Task<bool> Update(LightBulbDbModel game)
        {
            ReplaceOneResult updateResult =
                await _context
                    .LightBulbDBModel
                    .ReplaceOneAsync(
                        filter: g => g.Id == game.Id,
                        replacement: game);
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
