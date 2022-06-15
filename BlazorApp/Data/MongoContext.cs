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

        public IMongoCollection<Notification> Notifications
        {
            get { return database.GetCollection<Notification>("Notifications"); }
        }

        //public async Task<IEnumerable<Notification>> GetNotifications(int? year, string name)
        //{
        //    var builder = new FilterDefinitionBuilder<Notification>();
        //    var filter = builder.Empty;
        //    if (!String.IsNullOrWhiteSpace(name))
        //    {
        //        filter = filter & builder.Regex("Name", new BsonRegularExpression(name));
        //    }
        //    if (year.HasValue)
        //    {
        //        filter = filter & builder.Eq("Year", year.Value);
        //    }

        //    return await Notifications.Find(filter).ToListAsync();
        //}

        public async Task<List<Notification>> GetNotifications()
        {
            return await Notifications.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<Notification> GetNotification(string id)
        {
            return await Notifications.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
        }

        public async Task Create(Notification notification)
        {
            await Notifications.InsertOneAsync(notification);
        }

        public async Task Update(Notification notification)
        {
            await Notifications.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(notification.Id)), notification);
        }

        public async Task Remove(string id)
        {
            await Notifications.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
        }
    }
}
