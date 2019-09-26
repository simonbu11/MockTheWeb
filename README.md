# MockTheWeb

[![Build Status](https://sbull.visualstudio.com/MockTheWeb/_apis/build/status/simonbu11.MockTheWeb?branchName=master)](https://sbull.visualstudio.com/MockTheWeb/_build/latest?definitionId=1&branchName=master)

A Library for mocking HttpClient

# Basic usage

```csharp
// Get a mock
var httpClientMock = new HttpClientMock();

// Use built in helper to build responses (or not)
var expected = new Person {Id = 1, FirstName = "John"};
var response = ResponseBuilder.Json(expected);

// Setup client to response like the actual endpoint
httpClientMock
    .When((req) => req.RequestUri.LocalPath == "/api/people/1")
    .Then(response);

// Inject it into your class
var apiClient = new ApiClient(httpClientMock.AsHttpClient());

// Exercise you code as normal
var actual = await apiClient.GetById(1);

// Expect http client to work as normal
Assert.IsNotNull(actual);
Assert.AreEqual(expected.FirstName, actual.FirstName);

// Verify your code is calling the api as expected
httpClientMock.Verify(
    (req) => req.Method == HttpMethod.Get && req.RequestUri.LocalPath == "/api/people/1",
    Times.Once());
```
