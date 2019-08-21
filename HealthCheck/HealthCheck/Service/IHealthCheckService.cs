using System.Threading.Tasks;

namespace FNS.HealthCheck.Service
{
    public interface IHealthCheckService
    {
        Task<bool> Check();
    }
}
