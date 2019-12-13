# ConfigCore
Secure, centralized configuration for NET.CORE applications

## Objective
Manage application configuration settings from a centralized api or database.
Encrypt and Decrypt sensitive application data from all sources.
Allow removal of sensitive configuration data from source code files.
Eliminate need to configure individual host variables for production secrets.

## Configuration Sources/Providers
### ConfigCore.ApiSource - API Configuration Source/Provider
Custom configuration provider uses an HTTP client to fetch configuration data from a REST API.
Three overloads are available for adding the APISource and Provider to the configuration chain.
```
  public ApiConfigSource(IConfigurationBuilder builder, string configUrlVar, bool optional = false){}
```

### Config.Core.DbSource - Database Configuration Provider
This custom configuration provider sources configuration data directly from a centralized configuration database.

## Environment Features
ConfigCore provides support for three custom application environments in addition to the .NET CORE standard environments of Development, Staging, and Production. 
* Local
* Test
* QA

The following extension methods on IHostEnvironment are provided:
*	IsLocal(IHostEnvironment)
*	IsTest(IHostEnvironment)
*	IsQA(IHostEnvironment)
