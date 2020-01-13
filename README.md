# ConfigCore
Secure, centralized configuration for NET.CORE applications

## Objectives
* Manage application configuration settings from a secure, centralized database or API.
* Remove sensitive configuration data from source code files.
* Eliminate the need to manage environment variable settings on multiple host servers.

## ConfigCore.ApiSource - API Configuration Source/Provider
Custom IConfigurationProvider uses an HTTP client to retrieve configuration data from a REST API.
Extension methods on IConfigurationBuilder are used to add the API Configuration Source to the IConfigurationBuilder prior to calling Build. 
When the configuration is built, the API Provider will use an HTTP Client to retrieve key/value settings pairs from the API and add them to the configuration like any other configuration provider.

## Config.Core.DbSource - Database Configuration Provider
This custom configuration provider sources configuration data directly from a SQL Server database. Table and Column names used convention-based defaults for ease of configuration, but may also be overriden for flexibility.

## Environment Features
ConfigCore provides support for three custom application environments in addition to the .NET CORE standard environments
* Local
* Test
* QA

## See the [Wiki](https://github.com/Tricklebyte/ConfigCore/wiki) for documentation. 

Sample APIs, clients, and complete quickstart solutions are provided for multiple authentication types.
