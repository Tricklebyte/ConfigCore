# ConfigCore
Secure, centralized configuration for NET.CORE applications


## Objective
Manage configuration settings across multiple applications from a secure, centralized source.


## Configuration Sources/Providers
### [ConfigCore.ApiSource](http://www.google.com) - API Configuration Source/Provider
This custom configuration provider uses an HTTP client to fetch configuration data from a centralized configuration API. This provides secure, central management of connection strings, authentication secrets, and other sensitive or distributed information.  
### [Config.Core.DbSource](http://www.google.com)  -	Database Configuration Provider
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
