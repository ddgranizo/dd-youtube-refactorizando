using System;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Refactorizando.Server.Services.Implementations;

namespace Refactorizando.Server.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetImagesToken(this IConfiguration configuration)
        {
            return TryGetSettingFromSoruces(configuration, Definitions.ImagesToken);
        }
        public static string GetImagesEndpoint(this IConfiguration configuration)
        {
            return TryGetSettingFromSoruces(configuration, Definitions.ImagesEndpoint);
        }
        public static string GetEmailEndpoint(this IConfiguration configuration)
        {
            return TryGetSettingFromSoruces(configuration, Definitions.EmailEndpoint);
        }

        public static string GetEmailToken(this IConfiguration configuration)
        {
            return TryGetSettingFromSoruces(configuration, Definitions.EmailToken);
        }

        public static Guid GetEmailServerId(this IConfiguration configuration)
        {
            return Guid.Parse( TryGetSettingFromSoruces(configuration, Definitions.EmailServerId));
        }
     
  

        public static string GetJwt(this IConfiguration configuration)
        {
            var jwt = Environment.GetEnvironmentVariable("JWT_KEY");
            if (string.IsNullOrEmpty(jwt))
            {
                var fromSecret = configuration["jwt"];
                 if (string.IsNullOrEmpty(fromSecret))
                {
                    throw new Exception($"Can't find configuration for jwt key. Please, use environment variable 'JWT_KEY' or 'secrets.json' file with 'jwt' node.");
                }
                return fromSecret;
            }
            return jwt;
        }
        public static string GetConnectionString(this IConfiguration configuration){

            var server = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING_SERVER");
            var database = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING_DATABASE");
            var user = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING_USERNAME");
            var password = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING_PASSWORD");

            if (string.IsNullOrEmpty(server)
                || string.IsNullOrEmpty(user)
                || string.IsNullOrEmpty(database)
                || string.IsNullOrEmpty(password))
            {
                var fromSecret = configuration["connectionString"];
                if (string.IsNullOrEmpty(fromSecret))
                {
                    throw new Exception($"Can't find configuration for string connection. Please, use environment variable 'DB_CONNECTION_STRING_SERVER', 'DB_CONNECTION_STRING_DATABASE', 'DB_CONNECTION_STRING_USERNAME' and 'DB_CONNECTION_STRING_PASSWORD', or 'secrets.json' file with 'connectionString' node.");
                }
                return fromSecret;
            }
            return $"Server={server};Database={database};Uid={user};Pwd={password};";
        }

        private static string TryGetSettingFromSoruces(IConfiguration configuration, string settingName)
        {
            var value = Environment.GetEnvironmentVariable(settingName);
            if (string.IsNullOrEmpty(value))
            {
                var fromSecret = configuration[settingName];
                 if (string.IsNullOrEmpty(fromSecret))
                {
                    throw new Exception($"Can't find configuration for {settingName} key. Please, use environment variable '{settingName}' or add {settingName} property to 'secrets.json'");
                }
                return fromSecret;
            }
            return value;
        }

    }
}