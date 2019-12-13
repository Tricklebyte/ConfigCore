# ConfigCore
Secure, centralized configuration for NET.CORE applications

## Objectives
* Manage application configuration settings from a secure, centralized database or API.
* Remove sensitive configuration data from source code files.
* Eliminate the need to install and maintain environment variables on multiple hosts.


# ConfigCore.ApiSource - API Configuration Source/Provider
Custom configuration provider uses an HTTP client to retrieve configuration data from a REST API.
Extension methods on IConfigurationBuilder are used to add the API Configuration Source to the IConfigurationBuilder prior to calling Build. 
When the configuration is built, the API Provider will use an HTTP Client to retrieve key/value settings pairs from the API and add them to the configuration like any other configuration provider. Settings from previous providers in the chain will be overwritten by the API sourced settings if the keys are the same. Use the ConfigCore.ApiSource with your other configuration providers, in the correct order for your desired precedence.  

## IConfigurationBuilder.AddApiSource Extension Method
Adds the API Source to the configuration builder source chain prior to configuration build.
 Overloads
 
 ### AddApiSource 
 
  Simplest method, single required parameter for name of an Environment Variable containing the URL of the Configuration API. Defaults are used for application Id and Authentication.
  
 ### AddApiSource(string, string, bool)  
 One additional parameter for the application name to retreive settings for. Defaults to the application's assembly name if not specified.
 AddApiSource(IConfiguration, bool) 

#### AddApiSource
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
