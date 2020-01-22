using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace BookReadingRepository
{
    public class BookReadingDBManager
    {
        public async Task AddBookReadingEntry(BookReading newBookReading)
        {
            var nextLastPriority = await LastPriority() + 1;
            newBookReading.priority = nextLastPriority;

            await Collection().InsertOneAsync(newBookReading);
        }

        private async Task<int> LastPriority()
        {
            var options = new FindOptions<BookReading>
            {
                Limit = 1,
                Sort = Builders<BookReading>.Sort.Descending(bookReadingEntry => bookReadingEntry.priority)
            };

            BookReading lastPriorityBookReading = (await Collection().FindAsync(FilterDefinition<BookReading>.Empty, options)).FirstOrDefault() ?? new BookReading();
            return lastPriorityBookReading.priority;
        }

        public async Task<List<BookReading>> GetAllBookReadingEntries()
        {
            return await Collection().Find(_ => true).ToListAsync();
        }

        public async Task DeleteBookReadingEntry(string id)
        {
           await Collection().DeleteOneAsync(bookReading => bookReading.id == id);
        }

        private IMongoCollection<BookReading> Collection()
        {
            string connectionString = Environment.GetEnvironmentVariable("MONGO_DB_CONNECTION_STRING");
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var mongoClient = new MongoClient(settings);

            var db = mongoClient.GetDatabase("bookreadingsdb");
            return db.GetCollection<BookReading>("bookreadings");
        }
    }
}
