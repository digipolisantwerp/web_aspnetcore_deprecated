# Web Toolbox

## 8.0.0
 - Upgrade dependencies for .Net Core 2.0

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
