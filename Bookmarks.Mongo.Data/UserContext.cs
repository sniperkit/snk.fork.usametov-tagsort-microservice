using Bookmarks.Common;
using MongoDB.Driver;
using AutoMapper;
using System.Security.Cryptography;

namespace Bookmarks.Mongo.Data
{
    public class MongoUserContext
    {
        public const string USERS_COLLECTION = "users";

        IMongoClient _client;
        IMongoDatabase _database;
        IMapper MapperObj { get; set; }

        public string ConnectionString
        {
            get;
            set;
        }

        public MongoUserContext(string connectionString, string dbName)
        {
            ConnectionString = connectionString;
            _client = new MongoClient(ConnectionString);
            _database = _client.GetDatabase(dbName);        
        }

        public User GetUserByUserName(string userName)
        {
            var users = _database.GetCollection<User>(USERS_COLLECTION);
            return users.Find(u => u.UserName == userName).First();
        }
        
        public bool ValidateUser(string userName, string password)
        {
            var users = _database.GetCollection<User>(USERS_COLLECTION);
            var passwordHash = Utils.ComputeHash(password, SHA256.Create());

            return users.Find(u => 
                u.UserName == userName && u.PasswordHash == passwordHash).Any();         
        }
    }
}