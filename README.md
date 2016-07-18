# Web Toolbox  

The Web Toolbox offers functionality that can be used in ASP.NET Core 1.0 Web projects:
- version endpoint.
- base classes that encapsulate common functionality.
- action filters.
- swagger extensions.

## Table of Contents

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->


- [Installation](#installation)
- [ActionFilters](#actionfilters)
  - [ValidateModelState](#validatemodelstate)
- [Version Endpoint](#version-endpoint)
- [Swagger extensions](#swagger-extensions)
- [Exception handling](#exception-handling)
  - [Http status code mappings](#http-status-code-mappings)
  - [Usage](#usage)
  - [Logging](#logging)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Installation

To add the toolbox to a project, you add the package to the project.json :

``` json 
"dependencies": {
    "Digipolis.Web":  "2.0.1"
 }
``` 

ALWAYS check the latest version [here](https://github.com/digipolisantwerp/web_aspnetcore/blob/master/src/Digipolis.Web/project.json) before adding the above line !

In Visual Studio you can also use the NuGet Package Manager to do this.      
    
## ActionFilters  

### ValidateModelState  
When you write a lot of CUD operations one of the most recurring pieces of code is the validation of the ModelState in your controllers :  

``` csharp
if ( !ModelState.IsValid )
{
    // maybe do some logging
    return new BadRequestObjectResult(ModelState);
}
```  

By adding the ValidateModelState action filter attribute to your action, the validation is done automatically :

``` csharp 
[HttpPost]
[ValidateModelState]
public IActionResult Create(MyModel model)
{
    // no need to validate the ModelState here, it's already done before this code is executed
}
```

In case of an invalid ModelState a response with http status 400 (bad Request) is returned with an object of type **Error** containing the details of the validation failure.

``` json
{
  "Id": "c0fcec1c-07e0-4dd0-baf0-e6bd57da8fce",
  "Messages": [
    {
      "Key": "FirstName",
      "Message": "The field FirstName must be a string or array type with a minimum length of '2'."
    },
    {
      "Key": "LastName",
      "Message": "The LastName field is required."
    },
    {
      "Key": "Email",
      "Message": "The Email field is not a valid e-mail address."
    }
  ]
}
```
   
## Version Endpoint

This framework adds an additional endpoint to the web site where the version number of the application can be requested via a GET request.  
By default, this endpoint is provided at the url **_status/version_**, but this can be changed to another value during startup.

The versioning framework is added to the project in **ConfigureServices** method of the Startup  class :

``` csharp
  services.AddMvc().AddVersionEndpoint();           // default route = /status/version
  
  service.AddMvc().AddVersionEndpoint(options => options.Route = "myRoute");      // use custom route 
```

## Swagger extensions

When you use SwashBuckle to generate a Swagger UI for your API project, you might like to have the root url of your API point to that site. This can be done in the **Configure** of the Startup class :  

``` csharp

// ui on default url (swagger/ui)
app.UseSwaggerUi();                 // from SwashBuckle.SwaggerUi package
app.UseSwaggerUiRedirect();         

// custom url
app.UseSwaggerUi("myUrl");          // from SwashBuckle.SwaggerUi package
app.UseSwaggerUiRedirect("myUrl") 
``` 

**Don't** use this for a plain web project if you want to server your own HTML pages from the root uri.

## Exception handling

The toolbox provides a uniform way of exception handling.
The best way to use this feature is to have you code throw exceptions that derive from the **BaseException** type defined in the [error toolbox](https://github.com/digipolisantwerp/errors_aspnetcore).

If an exception is thrown in the application, the exception handler will create a response with the correct http status code and a meaningful error object that the 
api consumers can use to handle the error.

For exceptions that derive from **BaseException** the **Error** property is used as the response error object.
``` javascript
{
  "Id": "e4506b3e-1066-4f8e-bae2-336a0215e1a3",
  "Messages": [
    {
      "Key": "125",
      "Message": "VAT number invalid"
    },
    {
      "Key": "356",
      "Message": "Address invalid"
    },
    {
      "Key": "698",
      "Message": "Email invalid"
    }
  ]
}
```

For other exceptions a simple error object is created, only exposing the exception type.
``` javascript
{
  "Id": "e4506b3e-1066-4f8e-bae2-336a0215e1a3",
  "Messages": [
    {
      "Key": "",
      "Message": "Exception of type System.ArgumentNullException occurred. Check logs for more info."
    }
  ]
}
```

### Http status code mappings

It is possible to map exception types to specific http status codes. 
The default code is 500.  

Some exception types that are defined in the [error toolbox](https://github.com/digipolisantwerp/errors_aspnetcore) have default mappings predefined. For these exceptions it is not necessary to define the mappings in the configuration.

Exception type              | Http status code
------------------ | ----------------------------------------------------------- | --------------------------------------
NotFoundException              | 404 
ValidationException | 400
UnauthorizedException | 403  

Important note!
The default mappings will be overridden if you specify them in the mappings setup.

### Usage

To enable exception handling, call the **UseExceptionHandling** method on the **IApplicationBuilder** object in the **Configure** method of the **Startup** class.  

Since the exception handler is a middleware it should be positioned as high as possible in the request pipeline in order to catch all exceptions from other middleware.
So it is advised to place it as the first call on the **IApplicationBuilder** object in the **Configure** method.
Only when enabling **Cors** via the **app.UseCors** method, that method must be placed before the **UseExceptionHanlding** method.

``` csharp
    //Only cors placed above the ExceptionHandling
    app.UseCors();

    app.UseExceptionHandling(options => {
        // add your custom exception mappings here
    });
    
    // all other middleware
``` 

To specify the mappings of exception types to http status codes you can use the **HttpStatusCodeMappings** object that is passed to the setupAction of the **UseExceptionHandling** method.

To add a new mapping, call the Add method and pass the exception type the http status code.

``` csharp
    app.UseExceptionHandling(mappings =>
    {
        mappings.Add(typeof(NotFoundException), 404);
    });
``` 

You can also use a generic method to specify the exception type.

``` csharp
    mappings.Add<NotFoundException>(404);
``` 

If you have a lot of mappings to configure and you don't want to place the code in the **Startup** code file you can use the **AddRange** method that accepts a mappings collection that will be added.


Define a collection of mappings somewhere:
``` csharp
    var customMappings = new Dictionary<Type, int>()
    {
        { typeof(NotFoundException), 404 },
        { typeof(ValidationException), 500 },
        { typeof(UnauthorizedException), 401 }
    };
``` 

and add it in the **setupAction**
``` csharp
    app.UseExceptionHandling(mappings =>
    {
        mappings.AddRange(customMappings);
    });
``` 

### Logging

The exception handler will also log the exception.
If the http status code is in range of 4xx the exception will be logged with a **Debug** level.
Exceptions with a status code in range 5xx will be logged as **Error** level.

The logged message is a json with following structure:

``` javascript
{
	"HttpStatusCode" : 404,
	"Error" : {
		//The Error object serialized as Json
	},
	"Exception" : {
		//The exception object serialized as Json
	}
}
```

For exceptions that do not derive from **BaseException** the **Error** property will be empty. 
