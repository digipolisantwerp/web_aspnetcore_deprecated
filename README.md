# Web Toolbox  

The Web Toolbox offers functionality that can be used in ASP.NET Core Web projects:
- The enforcement of configurable API guideline.
- Versioning of endpoints.
- Paging with paging response object.
- Dynamic sorting.
- Global error handling with configuration of responses returning a standard error model.
- Base classes that encapsulate common functionality.
- Action filters.
- Swagger extensions.

## Table of Contents

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->


- [Installation](#installation)
- [ActionFilters](#actionfilters)
  - [ValidateModelState](#validatemodelstate)
  - [Endpoint versioning](#endpoint-versioning)
- [Modelbinders](#modelbinders)
- [MVC Extensions](#mvc-extensions)
- [Swagger extensions](#swagger-extensions)
  - [Formatting Swagger responses](#formatting-swagger-responses)
  - [Exclude certain responses from Swagger](#exclude-certain-responses-from-swagger)
- [Global exception handling](#global-exception-handling)
  - [Mapping exceptions to responses](#mapping-exceptions-to-responses)
  - [Usage](#usage)
  - [Logging](#logging)
- [Using the API extensions](#using-the-api-extensions)
- [Paging](#paging)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

**Attention:** We are still in the process of updating the documentation and a more detailed version
will follow in the days to come.

## Installation

To add the toolbox to a project, you add the package to the csproj project file:

```xml
  <ItemGroup>
    <PackageReference Include="Digipolis.Web" Version="9.2.0" />
  </ItemGroup>
``` 

or if your project still works with project.json :

``` json 
"dependencies": {
    "Digipolis.Web":  "9.2.0"
 }
``` 

ALWAYS check the latest version [here](https://github.com/digipolisantwerp/web_aspnetcore/blob/master/src/Digipolis.Web/Digipolis.Web.csproj) before adding the above line !

Make sure you have our Nuget feed configured (https://www.myget.org/F/digipolisantwerp/api/v3/index.json).

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
  "Identifier": "c0fcec1c-07e0-4dd0-baf0-e6bd57da8fce",
  "Title": "Validation failed",
  "Status": 400,
  "Code": "UNVAL001"
  "ExtraParameters": [
    { "FirstName": [ "The field FirstName must be a string or array type with a minimum length of '2'." ] },
    { "LastName": [ "The LastName field is required." ] },
    { "Email": [ "The Email field is not a valid e-mail address." ] }
  ]
}
```

### Endpoint versioning
You can restrict endpoints to specific versions by adding the **VersionsAttribute**. The toolbox will inject the version specified in the 
constructor parameters passed to this attribute into the route.

So if you add this attribute to an endpoint

``` csharp 
/// endpoint: POST api/Users
[Versions("v1", "v2")]
public IActionResult Post(MyModel model)
{
}
```

The framework will inject these versions and the endpoint will only be reachable at the following endpoints:
- POST v1/api/Users
- POST v2/api/Users

NOTE: When versioning is enabled it can be disabled by configuration. in the appsettings set following section:

``` json
{
  ...
  "ApiExtensions": {
    "DisableVersioning": true,
    "DisableGlobalErrorHandling": true,
    "PageSize": 10
  }
}
``` 

## Modelbinders
A modelbinder for comma seperated arrays or enumerables is made available.
Insert it at the start of the modelbinders collection to always try to deserialize arrays or IEnumerables of value types or string using this modelbinder.

``` csharp
 services.AddMvc(
                (o) => 
                {
                    o.ModelBinderProviders.Insert(0,new CommaDelimitedArrayModelBinderProvider());
                })
```

The modelbinder can also be applied to properties individually, but you have to repeat the binding source ([FromQuery] in this example).
The bindingsource from the parent class is not inherted.

``` csharp
        [FromQuery][ModelBinder(BinderType = typeof(CommaDelimitedArrayModelBinder))]
        public string[] Sort { get; set; } = new string[0];
```

## MVC Extensions
These extensions will configure some standards for MVC and the JsonSerializer. It allows you to specify the default pagesize used in paging.
You can pass in a configsection, a configuration lambda or both.
The extensions will
- set the mime-type to 'application/json'
- Set Time handling to UTC
- remove empty fields from the output
- serialize Timespans according to guidelines

``` csharp
services.AddMvc().AddApiExtensions(Configuration.GetSection("ApiExtensions"), x =>
{
    //Override settings made by the appsettings.json
    x.PageSize = 10;
});
```

the configsection accepts below markup:
``` json
{
  ...
  "ApiExtensions": {
    "DisableVersioning": true,
    "DisableGlobalErrorHandling": true,
    "PageSize": 10
  }
}
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

### Formatting Swagger responses
You can configure some default responses by specifying a class as a generic when registering AddSwaggerGen

``` csharp 
 // Add Swagger extensions
            services.AddSwaggerGen<ApiExtensionSwaggerSettings>(o =>
            {
                o.SwaggerDoc(Versions.V1, new Info
                {
                    //Add Inline version
                    Version = Versions.V1,
                    Title = "API V1",
                    Description = "Description for V1 of the API",
                    Contact = new Contact { Email = "info@digipolis.be", Name = "Digipolis", Url = "https://www.digipolis.be" },
                    TermsOfService = "https://www.digipolis.be/tos",
                    License = new License
                    {
                        Name = "My License",
                        Url = "https://www.digipolis.be/licensing"
                    },
                });

                o.SwaggerDoc("v2", new Version2());
            });
```

**ApiExtensionSwaggerSettings** is a class that incorperates all guidelines from Digipolis. But this can be
overriden by inheriting from **SwaggerSettings**

``` csharp 
//custom settings

public class CustomSettings : SwaggerSettings<SwaggerResponseDefinitions>
{}

//Also SwaggerResponseDefinitions can be overriden to override some defaults.

```
The defaults can be expanded by overriding the **ApiExtensionSwaggerSettings** class

``` csharp 

//additional settings

public class AdditionalApiExtensionSwaggerSettings : ApiExtensionSwaggerSettings 
{}

```

Then register Swagger with this class
``` csharp 
services.AddSwaggerGen<AdditionalApiExtensionSwaggerSettings>();
```

### Exclude certain responses from Swagger
If you want to exclude certain you can do this by specifying on the endpoint.

``` csharp 
/// endpoint: POST api/Users
[ExcludeSwaggerResponse((int)HttpStatusCode.NotFound)]
public IActionResult Get(int id)
{
}
```
        
## Global exception handling

The toolbox provides a uniform way of exception handling.
The best way to use this feature is to have you code throw exceptions that derive from the **BaseException** type defined in the [error toolbox](https://github.com/digipolisantwerp/errors_aspnetcore).
But any exception can be used if properly mapped to an error model

If an exception is thrown in the application, the exception handler will create a response with the correct http status code and a meaningful error object that the 
api consumers can use to handle the error.

### Mapping exceptions to responses

To configure error models to exception you need to implement a new class that inherits from **ExceptionMapper** type defined in the [error toolbox](https://github.com/digipolisantwerp/errors_aspnetcore).
How to configure the exceptions can be found in the documentation of the [error toolbox](https://github.com/digipolisantwerp/errors_aspnetcore).

### Usage

To enable exception handling, call the **UseExceptionHandling** method on the **IApplicationBuilder** object in the **Configure** method of the **Startup** class.  
``` csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    /// ApiExceptionMapper is the derived class from ExceptionMapper
    services.AddGlobalErrorHandling<ApiExceptionMapper>();
    ...
}
``` 
NOTE: When versioning is enabled it can be disabled by configuration. in the appsettings set following section:

``` json
{
  ...
  "ApiExtensions": {
    "DisableVersioning": false,
    "DisableGlobalErrorHandling": true,
    "PageSize": 10
  }
}
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
    "ExceptionInfo" : "Digipolis.Errors.Exceptions.NotFoundException: Not found.
        at Digipolis.Web.SampleApi.Controllers.ValuesController.ThrowException() ...",
	"Exception" : {
		//The exception object serialized as Json
	}
}
```

For exceptions that do not derive from **BaseException** the **Error** property will be empty. 
The **ExceptionInfo** contains the result of the exception ToString() method.
The **Exception** property of the logged message is not filled by default. If you also want to log the exception object you can set the **LogExceptionObject** property to true on the **ApiExtensionOptions**.

## Using the API extensions

To enable the api extensions defined in this toolbox, you need to enable them as high as possible in the pipeline. 
Only when enabling **Cors** via the **app.UseCors** method, that method must be placed before the **UseApiExtensions** method.

Call the **UseApiExtensions** method on the **IApplicationBuilder** object in the **Configure** method of the **Startup** class.  
``` csharp
    app.UseApiExtensions();
``` 

## Paging
Paging has been made easy by using following code example. 

PageOptions can apply two paging strategies: withCount or noCount. When "NoCount" has been chosen, the paged result in HAL format 
will not have a last link and TotalElements/TotalPages will be set to null because they are unknown.

On the controller endpoint:
``` csharp
[Produces("application/hal+json")]
public IActionResult Get([FromQuery]PageOptions queryOptions)
{
    try
    {
        int total;
        var values = _valueLogic.GetAll(queryOptions, out total);
        var result = queryOptions.ToPagedResult<Value, EmbeddedValue>(values, total);
        return Ok(result);

    }
    catch (Exception ex)
    {
        return StatusCode((int)HttpStatusCode.InternalServerError);
    }
}
``` 

Inherit Embedded<T> to get correct resourceList name:
``` csharp
public class EmbeddedValue : Embedded<Value>
{
    [JsonProperty("values")]
    public override IEnumerable<Value> ResourceList { get; set; }
}
``` 
