using System.Threading.Tasks;

namespace HealthCheck.Service
{
    public interface IHealthCheckService
    {
        Task<bool> Check();
    }
}
