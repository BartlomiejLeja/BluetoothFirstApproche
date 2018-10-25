using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SignalIRServer.Model
{
    public class LightBulbDbModel : LightBulbModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
    }
}
