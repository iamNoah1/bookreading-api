using BookReadingProject;
using AzureFunctions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BookReadingRepositoryUnitTest
{
    public class BookReadingRepositoryTester
    {
        private readonly ILogger logger = TestHelper.CreateLogger();
        private BookReadingController unitUnderTest;

        [Fact]
        public async System.Threading.Tasks.Task CreateBookReadingShouldAddBookReadingEntryAndReturn200IfAddedSuccessfully()
        {
            var dbManagerMock = new Mock<BookReadingDBManager>();
            dbManagerMock.Setup(manager => manager.AddBookReadingEntry(It.IsAny<BookReading>()));

            unitUnderTest = new BookReadingController(dbManagerMock.Object);
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            var result = (OkObjectResult)await unitUnderTest.CreateBookReading(request, logger);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact (Skip = "not implemented")]
        public void CreateBookReadingShouldShouldReturn500IfThereWasAnErrorWhileAdding()
        {

        }
    }
}
