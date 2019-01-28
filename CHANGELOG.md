# Web Toolbox

## 9.0.3

- Removed unnecessary DeserializationPagedResult, JsonConvertor and ISchemaFilter: the same result can be achieved with simple inheritance.
- PagedResult<T> changed to use inheritance, kept for backwards compatibility.
- Added new PagedResult<T, EmbeddedT> and Embedded<T> for correct HAL property name.
- Added new action to ValuesController demonstrate the difference between the two PagedResults. 
- Added EmbeddeValueDto to demonstrate the use of EmbeddedT.
- Removed StringExtensions: ToCamelCase only used for camelcasing bodyparameter names (and contained private extension methods); 
	now all parameter names are corrected in consistent way in 1 pass.
- Updated EndPointPathsAndParamsToLower to reflect this.
- Added new ToPagedResult<T, EmbeddedT> to PageOptionsExtensions, with less parameters => values from ActionContextDescriptor.
- Updated GenerateLink to include all query parameters, not just the Page(Sort)Options.
- Updated unit tests.

## 9.0.2

- Bugfix: SwaggerResponseDefinitions - retrieve the controller attributes from declaring Type and not from  the method

## 9.0.1

- Bugfix: pageOptions second to last page now has next page reference
- Added default value property to page options so they show up on swagger

## 9.0.0

- upgrade to netstandard2.0

## 8.0.1

- ToPagedResult<T>: removed new() restriction

## 8.0.0

- json serialization: initialized/assigned properties with a value different from null will be serialized; properties (collection, string, ...) with value null won't be serialized

## 7.0.1

- PagedResult<T>: removed new() restriction

## 7.0.0

- Swagger reference update
- Fixed api versioning
- Removed version route

## 6.1.0

- error response content type changed to application/problem+json

## 6.0.0

- removed global exception filter.

## 5.0.0

- conversion to csproj and MSBuild.

## 4.0.3

- Bugfix : routevaluedictionary exception when executing concurrent calls

## 4.0.2

- Bugfix : deserialization of nullable types failed.

## 4.0.1

- Bugfix : HAL querystring "?" was encoded

## 4.0.0

- HAL Url generation is now based on Request host information.
- Added support for X-Forwarded-For, X-Forwarded-Host and X-Forwarded-Proto headers.
- Updated reference to Digipolis.Error toolbox (breaking change)

## 3.0.6

- Register CommaDelimitedModelBinder automatically
- Bugfix: CommaDelimitedModelBinder supports enumerations and array but fixed string perceived as array. preventing string from being sent to the api
- Added proper consumes/produces types in swagger from de consumes/produces attributes on actions
- Bugfix: Swagger configuration is passed again so additional options are again set during start up

## 3.0.5

- Changed JsonConverters from internal to public
- Added deserialization support to PagedResultConverter

## 3.0.4

- fix for CommaDelimitedModelBinder

## 3.0.3

- added ability to disable the global exception filter

## 3.0.2

- added support for application/hal+json MIME-type
- added ModelBuilder for comma serperated arrays
- changed exception logging to log exception ToString() by default, exception object logged optionally using settings

## 2.0.3

- fixed namespace in WebAppBuilderExtensions

## 2.0.2

- fixed version endpoint error

## 2.0.1

- renamed AddVersioning to AddVersionEndpoint

## 2.0.0

- Exception handling
- ValidateModelState
- Version endpoint

## 1.0.0

- added support for application/hal+json MIME-type
- added ModelBuilder for comma serperated arrays
