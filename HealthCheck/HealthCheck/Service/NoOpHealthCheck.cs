using System.Threading.Tasks;

namespace FNS.HealthCheck.Service
{
    public class NoOpHealthCheck : IHealthCheckService
    {
        public Task<bool> Check()
        {
            return Task.FromResult(true);
        }
    }
}
