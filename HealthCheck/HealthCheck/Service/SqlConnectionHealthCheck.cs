using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FNS.HealthCheck.Service
{
    public class SqlConnectionHealthCheck : IHealthCheckService
    {
        private const string ConnectionStringsSection = "ConnectionStrings";
        private IConfiguration _configuration;
        private ILogger<SqlConnectionHealthCheck> _logger;

        public SqlConnectionHealthCheck(IConfiguration configuration, ILogger<SqlConnectionHealthCheck> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public Task<bool> Check()
        {
            var currentConnectionString = string.Empty;
            try
            {
                var section = _configuration.GetSection(ConnectionStringsSection);

                if (section != null)
                {
                    foreach (var stringConn in section.GetChildren())
                    {
                        currentConnectionString = stringConn.Key;

                        using (var conn = new SqlConnection(stringConn.Value))
                        {
                            if (conn == null)
                            {
                                throw new Exception("Problem creating SqlConnection");
                            }

                            conn.Open();

                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.CommandText = "select 1";

                                cmd.ExecuteScalar();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                var msg = string.IsNullOrEmpty(currentConnectionString) ? 
                    "Failed to read configuration" : 
                    string.Format("Error trying to connect to the database.", currentConnectionString);
                _logger.LogError(ex, msg);
                throw new Exception(msg, ex);
            }

            return Task.FromResult(true);
        }
    }
}
