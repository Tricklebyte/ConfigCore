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
 
1.)	AddApiSource(string configUrlVar,  bool optional = false)
a.	Required Parameter configUrlVar is the name of the Environment Variable containing the URL of the configuration API.
b.	Non-required parameter optional indicates if this configuration source is optional, default is false.
c.	This method is the simplest and easiest to use because it only requires a single parameter for the Environment variable name containing the URL of the Configuration API. It may be used prior to any other configuration being built. 
d.	Because this method relies on a single input, only default settings are supported. Default AppId is the Assembly Name, Default Authentication type is Client Certificate.
e.	See sample project ApiConfigClient_ProgramDefault for usage example.

  
 ### AddApiSource(string, string, bool)  
 One additional parameter for the application name to retreive settings for.
 Used to supply a non-default application Id as a search parameter to the API.
 Used when the Application Id is not equal to the Assembly Name.
 Client Certificate authentication will be used by default.
 

### AddApiSource(IConfiguration,bool)
3.)	AddApiSource(IConfiguration config, bool optional = false)
a.	Required Parameter IConfiguration. This Configuration object must contain section ConfigOptions:ApiSource. Use this method to override all default settings without the use of any Environment Variables. Specify a non-default applicationId and select non-default authentication methods and settings for APIKey Authorization or Windows Integrated Authentication. 
b.	This method requires that an initial Configuration object be built prior to calling the AddApiSource extension method.
i.	When loading configuration in Program.cs use an initial configuration builder prior to calling AddDbSource. See sample project ApiConfigClient_ProgramCustom for usage example.
ii.	When building configuration in Startup.cs, use the default system configuration passed into the Startup class via Dependency Injection and build the final configuration in Startup.ConfigureServices. See sample project ApiConfigClient_Startup for usage example

#### Configuration Section ConfigOptions:ApiSource
``` 
"ConfigOptions": {
    "ApiSource": {
      "AuthType": null,
      "AuthClaimType": null,
      "AuthClaimValue": null,
      "ConfigApiUrlKey": null,
      "ConfigApiUrl": "https://localhost:44397/iapi/ConfigSettings/",
      "AppId": null
    }
}
```
### Api Source Options Table

| Setting | Description | Default Value |
| ------- | ----------- | ------------- |
| ConfigApiUrl | URL of the configuration API.  The URL must consist of the Base Address and method route path without parameter values.| NA |
| ConfigApiUrlKey | Name of configuration setting key where the URL of the Confiugration API is located. Override this setting if your URL value is already present in a different, non-default setting key.| ConfigOptions:ApiSource:ConfigApiUrl |


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
