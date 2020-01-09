# BookReading
A sample Azure Functions Visual Studio Solution. Allows to store a prioritized list of Books. Can be used as a reading list.  


# Included Projects

* BookReadingRepository
* BookReadingRepositoryIntegrationTest


# Endpoints

* POST <baseurl>:<port>/api/bookreadings
* GET <baseurl>:<port>/api/bookreadings
* DELETE <baseurl>:<port>/api/bookreadings/<id>


# Preparation

* Checkout repository 
* Copy `local.settings.json.example` to `local.settings.json` and maybe adjust the env variable `MONGO_DB_CONNECTION_STRING`


# Run locally

* Using Visual Studio UI to start Functions from BookReadingRepository
* Using [Azure Functions Core Tools](https://github.com/Azure/azure-functions-core-tools) to start function. Therefore change to BookReadingRepository directory and simply run `func start`.


# Run Integration Tests

* Start Functions either through Visual Studio or from CLI.
* Through Visual Studio, use the context menue of the BookReadingRepositoryIntegrationTest project to run test. 
* **Hint**: It is recommended to start the Functions using the CLI. Otherwise you will have to open 2 instances of Visual Studio. One to run the Functions, the other one to run the tests.
