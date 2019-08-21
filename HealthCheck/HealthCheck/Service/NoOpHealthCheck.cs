using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HealthCheck.Service
{
    public class NoOpHealthCheck : IHealthCheckService
    {
        public Task<bool> Check()
        {
            return Task.FromResult(true);
        }
    }
}
