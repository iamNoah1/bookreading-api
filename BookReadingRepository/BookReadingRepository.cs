using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MongoDB.Driver;
using System.Net;
using System.Security.Authentication;

namespace BookReadingRepository
{
    public static class BookReadingRepository
    {
        [FunctionName("CreateBookReading")]
        public static async Task<IActionResult> CreateBookReading([HttpTrigger(AuthorizationLevel.Function, "post", Route = "bookreadings")] HttpRequest req, ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var newBookReading = JsonConvert.DeserializeObject<BookReading>(requestBody);

                var nextLastPriority = await LastPriority() + 1;
                newBookReading.priority = nextLastPriority;

                await getCollection().InsertOneAsync(newBookReading);
                return new OkObjectResult(newBookReading);
            } catch (Exception e)
            {
                var objectResult = new ObjectResult(e.Message);
                objectResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                return objectResult;
            }
        }

        private static async Task<int> LastPriority()
        {
            var options = new FindOptions<BookReading>
            {
                Limit = 1,
                Sort = Builders<BookReading>.Sort.Descending(bookReadingEntry => bookReadingEntry.priority)
            };

            BookReading lastPriorityBookReading = (await getCollection().FindAsync(FilterDefinition<BookReading>.Empty, options)).FirstOrDefault() ?? new BookReading();
            return lastPriorityBookReading.priority;
        }

        [FunctionName("GetBookReadings")]
        public static async Task<IActionResult> GetBookReadings([HttpTrigger(AuthorizationLevel.Function, "get", Route = "bookreadings")] HttpRequest req, ILogger log)
        {
            try
            {
                var bookReadings = await getCollection().Find(_ => true).ToListAsync();
                return new OkObjectResult(bookReadings);
            }catch (Exception e)
            {
                var objectResult = new ObjectResult(e.Message);
                objectResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                return objectResult;
            }
        }

        private static IMongoCollection<BookReading> getCollection()
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
