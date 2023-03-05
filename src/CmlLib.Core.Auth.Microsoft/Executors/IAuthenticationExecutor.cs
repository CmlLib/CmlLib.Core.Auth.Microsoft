using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft.Executors
{
    public interface IAuthenticationExecutor
    {
        Task<ISession> ExecuteAsync();
    }
}