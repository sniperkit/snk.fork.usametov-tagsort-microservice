using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bookmarks.Mongo.Data
{
    public class User 
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string[] Claims { get; set; }
    }
}
