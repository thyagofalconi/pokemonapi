# Pokemon API

Retrieve information about Pokemons, such as their Shakespearean description

### Prerequisites

* Visual Studio 2017
* .NET Core SDK 3.1

### Swagger

https://localhost:44338/swagger/index.html

### Curl Request

curl -X GET "https://localhost:44338/pokemon/charizard" -H "accept: */*"

### Tests

* Unit Tests
* Integration Tests

### Built With

* [.Net Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)

### Authors

* **Thyago Falconi**

### TODO

* Retry policy
* Add support for multiple languages
* Trigger logger in services
* Add tests for refit services
* Fix warnings

### Important

There's a limitation of 5 requests per hour per IP, tests will fail with HTTP 429 (Too Many Requests) if limit is reached.