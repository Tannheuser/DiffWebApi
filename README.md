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

##Required technology
- .NET Core 1.1


## Sample input/output

Show what the library does as concisely as possible, developers should be able to figure out **how** your project solves their problem by looking at the code example. Make sure the API you are showing off is obvious, and that your code is short and concise.

## Motivation

A short description of the motivation behind the creation and maintenance of the project. This should explain **why** the project exists.

## Installation

Provide code examples and explanations of how to get the project.

## Tests

Tests are written using xUnit testing framework and could be started from you prefered IDE like Microsoft Visual Studio or using console/terminal command `dotnet test`.

## License

MIT