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

namespace BookReadingProject
{
    public class BookReadingRepository
    {
        private BookReadingDBManager dBManager;

        public BookReadingRepository(BookReadingDBManager dbManager)
        {
            this.dBManager = dbManager;
        }

        [FunctionName("CreateBookReading")]
        public async Task<IActionResult> CreateBookReading([HttpTrigger(AuthorizationLevel.Function, "post", Route = "bookreadings")] HttpRequest req, ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var newBookReading = JsonConvert.DeserializeObject<BookReading>(requestBody);

                await dBManager.AddBookReadingEntry(newBookReading);
                return new OkObjectResult(newBookReading);
            } catch (Exception e)
            {
                var objectResult = new ObjectResult(e.Message);
                objectResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                return objectResult;
            }
        }

        [FunctionName("GetBookReadings")]
        public async Task<IActionResult> GetBookReadings([HttpTrigger(AuthorizationLevel.Function, "get", Route = "bookreadings")] HttpRequest req, ILogger log)
        {
            try
            {
                var bookReadings = await dBManager.GetAllBookReadingEntries();
                return new OkObjectResult(bookReadings);
            } catch (Exception e)
            {
                var objectResult = new ObjectResult(e.Message);
                objectResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                return objectResult;
            }
        }

        [FunctionName("DeleteBookReading")]
        public async Task<IActionResult> DeleteBookReading([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "bookreadings/{id}")] HttpRequest req, string id, ILogger log)
        {
            try
            {
                await dBManager.DeleteBookReadingEntry(id);
                return new NoContentResult();
            } catch (Exception e)
            {
                var objectResult = new ObjectResult(e.Message);
                objectResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                return objectResult;
            }
        }

    }
}
