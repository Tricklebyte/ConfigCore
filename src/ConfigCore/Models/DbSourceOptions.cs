using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;

namespace ConfigCore.Models
{
    public class DbSourceOptions
    {
        
        string defAppIdVal = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
        public string ConnString { get; set; }
        public string  ConnStringKey { get; set; }
        public int SqlCmdTimeout { get; set; }
        public string TableName { get; set; }
        public string KeyCol { get; set; }
        public string ValCol { get; set; }
        public string AppIdCol { get; set; }
        public string AppIdVal { get; set; }
        /// <summary>
        /// Performs validation on DbSource Configuration input before creating dbConfigOptions object
        /// Sets default configuration keys, default column values for database operations
        /// Throws validation errors only when "optional" parameter is set to false
        /// </summary>
        /// <param name="dbConfig"></param>
        /// <param name="env"></param>
        /// <param name="optional"></param>
        
           
        public DbSourceOptions() {}
               
     
        // EARLY LOAD - Used in Program.cs, uses default application id for searching database (current assembly name)
        public DbSourceOptions(string connStringKey, bool optional, int sqlCmdTimeout)  {
            
            SetConnStringFromEnvVar(connStringKey,optional);
            SqlCmdTimeout = sqlCmdTimeout;
            AddDefaults();
        }
      
        
        /// <summary>
       /// EARLY LOAD with non-default AppId
       /// </summary>
       /// <param name="connStringKey"></param>
       /// <param name="appId"></param>
       /// <param name="optional"></param>
       /// <param name="sqlCommandTimeout"></param>
        //
        public DbSourceOptions(string connStringKey,  string appId, bool optional, int sqlCommandTimeout)
        {
            SetConnStringFromEnvVar(connStringKey, optional);
            SqlCmdTimeout = sqlCommandTimeout;
            AppIdVal = appId;
            AddDefaults();
        }

        
   // accepts configuration parameter for overriding default settings. Configuration must contain section ConfigOptions:DbSource
        public DbSourceOptions(IConfiguration config, bool optional)
        {
            // validate the configuraration and throw error only if optional = true
            IConfigurationSection dbSection = config.GetSection("ConfigOptions:DbSource");
            if (!dbSection.Exists())
            {
                throw new Exception("DbSource section not found in configuration");
            }

            // The Sql Command Timeout is not a required input and should default to 0
            int parseResult;
            Int32.TryParse(dbSection["SqlCommandTimeout"], out parseResult);
            if (parseResult == 0)
            {
                SqlCmdTimeout = 0;
            }
            else
                SqlCmdTimeout = Int32.Parse(dbSection["SqlCommandTimeout"]);

            // CONNECTION STRING
            // Use the connection string key value to get the Connection string value.
            // Seperating the Key and the Value into different settings allows the User to override the ConnString value only if they can use the default key for their connection string.
            // Alternatively, the User could only override the ConnStringKey value if they need to use a specific naming convention for their configuration settings.  
            string connKey;

            //Assign default Connection String if not supplied by user
            connKey = dbSection["ConnStringKey"] ?? "ConfigOptions:DbSource:ConnString";
            
          
            // try to assign the connectionstring located at the connection string key
            ConnString = config[connKey];
            // Throw error if the connection string is not found by looking up the ConnStringKey reference
            if (ConnString == null && optional == false)
                throw new Exception($"The configuration setting 'ConfigOptions:DbSource:ConnStringKey' does not reference a valid configuration setting key or the setting value is null.");

            // Connection string is set

            //// SQL Server Table and Column metadata 
            TableName = dbSection["TableName"];
            AppIdCol = dbSection["AppIdCol"];
            KeyCol = dbSection["KeyCol"];
            ValCol = dbSection["SettingValue"];

            //If null, use Environment App name  //TODO Change to read default name here and remove environment from method signature
            AppIdVal = dbSection["AppIdVal"];
            
            // Add default values for options that were not supplied
            AddDefaults();
        }
        /// <summary>
        /// Used with override for Environment Variable connection string 
        /// </summary>
        /// <param name="connStringKey"></param>
        /// <param name="optional"></param>
        private void SetConnStringFromEnvVar(string connStringKey, bool optional)
        {
            ConnString = Environment.GetEnvironmentVariable(connStringKey);
            if (ConnString == null && optional == false)
                throw new Exception($"Unable to create Db Source Options, Environment Variable: '{connStringKey}' not found.");
        }
       
        private void AddDefaults() {
            TableName = TableName ?? DbDefault.TableName;
            AppIdCol = AppIdCol ?? DbDefault.AppIdCol;
            KeyCol = KeyCol ?? DbDefault.KeyCol;
            ValCol = ValCol ?? DbDefault.ValCol;
            AppIdVal = AppIdVal ?? System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
        }
            
    
    
    }
}
