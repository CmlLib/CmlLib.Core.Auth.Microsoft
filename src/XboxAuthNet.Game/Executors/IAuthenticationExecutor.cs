using System.Threading.Tasks;

namespace XboxAuthNet.Game.Executors
{
    public interface IAuthenticationExecutor
    {
        Task<ISession> ExecuteAsync();
    }
}