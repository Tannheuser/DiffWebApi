# DiffWebApi

## Features
This API provides 2 http endpoints (`<host>/v1/diff/<ID>/left` and `<host>/v1/diff/<ID>/right`) that accept JSON containing base64 encoded binary data on both endpoints.
The provided data will be diff-ed and the results will be available on a third end point (`<host>/v1/diff/<ID>`). 
The results provide the following info in JSON format: 
```
- Return if both equal
- Return if size is not equal
- If of same size, provides insight in where the diff are
```

## Required technology
.NET Core 1.1

## Sample input/output
| Request | Response |
| ---  | --- |
| GET /v1/diff/1        | 404 Not Found      |
| POST /v1/diff/1/left {"data": "aGVsbG8="}	 | 201 Created  |  
| POST /v1/diff/1/right {"data": "aGVsbG8="}  | 201 Created  |
| GET /v1/diff/1 |200 OK {"diffResultType": "Equals"} |
|    |     |
| POST /v1/diff/1/left {"data": "aGVsbG9mZg=="}  | 201 Created  |
| GET /v1/diff/1 |200 OK {"diffResultType": "SizeDoNotMatch"} |
|    |   |
| POST /v1/diff/1/left {"data": "aGVycm8="}  | 201 Created  |
| GET /v1/diff/1 |200 OK {"diffResultType": "ContentDoNotMatch", "diffs": [{"offset": 2, "length": 2}} |

## Installation

Sulution could be started from you preferable IDE like Microsoft Visual Studio, Vissual Studio Code or Jetbrains Rider.
Also you could do it manually from console/terminal following this short instruction:

- Navigate to the root solution folder and call `dotnet restore` command
- Navigate to the Web project folder `DiffWebApi.Web` and call `dotnet run` command

## Tests

Tests are written using xUnit testing framework and could be started from you preferable IDE like Microsoft Visual Studio.
Also you could do it manually from console/terminal following this short instruction:

- Navigate to the root solution folder and call `dotnet build` command
- Navigate to the Tests project folder `DiffWebApi.Tests` and call `dotnet test` command

## License

MIT