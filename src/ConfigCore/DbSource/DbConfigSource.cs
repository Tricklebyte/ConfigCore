
using ConfigCore.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;


namespace ConfigCore.DbSource
{
    public class DbConfigSource : IConfigurationSource
    {
        ISqlClientAdo _sqlClient;

        bool _optional;
        int _sqlCommandTimeout;

       
        // Uses assembly name as AppId search value
        public DbConfigSource(IConfigurationBuilder builder, string conKeyVar, bool optional, int sqlCommandTimeout)
        {
            _optional = optional;
 
        
            try
            {
                // Create new dboptions object
                DbSourceOptions dbOptions = new DbSourceOptions(conKeyVar,optional,sqlCommandTimeout);
                // Initialize Sql client with dbOptions object
                _sqlClient = new SqlClientAdo(dbOptions);

            }
            catch (Exception e)
            {
                if (!optional)
                    throw e;
                return;
            }
        }

        //
        public DbConfigSource(IConfigurationBuilder builder, string conKeyVar, string appId, bool optional,int sqlCommandTimeout)
        {
           
            _optional = optional;
       
            try
            {
                // Create new dboptions object
                DbSourceOptions dbOptions = new DbSourceOptions(conKeyVar,appId,optional,sqlCommandTimeout);
                // Initialize Sql client with dbOptions object
                _sqlClient = new SqlClientAdo(dbOptions);

            }
            catch (Exception e)
            {
                if (!optional)
                    throw e;
                return;
            }
        }
        
        // Accepts configuration parameter for non-default database options
        public DbConfigSource(IConfigurationBuilder builder, IConfiguration config, bool optional)
        {
            _optional = optional;
            try
            {
                // Create new dboptions object
                DbSourceOptions dbOptions = new DbSourceOptions(config, optional);
                // Initialize Sql client with dbOptions object
                _sqlClient = new SqlClientAdo(dbOptions);

            }
            catch (Exception e)
            {
                if (!optional)
                    throw e;
                return;
            }
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DbConfigProvider( _sqlClient, _optional);
        }

    }
}
