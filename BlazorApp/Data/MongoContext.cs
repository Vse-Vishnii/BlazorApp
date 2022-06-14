using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace BlazorApp.Data
{
    public class MongoContext
    {
        private IMongoDatabase database; // база данных
        private IGridFSBucket gridFS;   // файловое хранилище

        public MongoContext(string connectionString)
        {
            var connection = new MongoUrlBuilder(connectionString);
            MongoClient client = new MongoClient(connectionString);
            database = client.GetDatabase(connection.DatabaseName);
            gridFS = new GridFSBucket(database);
        }

        public IMongoCollection<User> Users
        {
            get { return database.GetCollection<User>("Users"); }
        }

        public async Task<IEnumerable<User>> GetUsers(int? year, string name)
        {
            var builder = new FilterDefinitionBuilder<User>();
            var filter = builder.Empty;
            if (!String.IsNullOrWhiteSpace(name))
            {
                filter = filter & builder.Regex("Name", new BsonRegularExpression(name));
            }
            if (year.HasValue)
            {
                filter = filter & builder.Eq("Year", year.Value);
            }

            return await Users.Find(filter).ToListAsync();
        }
        
        public async Task<User> GetUser(string id)
        {
            return await Users.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
        }

        public async Task Create(User user)
        {
            await Users.InsertOneAsync(user);
        }

        public async Task Update(User user)
        {
            await Users.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(user.Id)), user);
        }

        public async Task Remove(string id)
        {
            await Users.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
        }
    }
}
