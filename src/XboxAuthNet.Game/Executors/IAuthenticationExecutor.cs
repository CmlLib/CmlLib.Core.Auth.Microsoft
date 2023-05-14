using System.Threading.Tasks;

namespace XboxAuthNet.Game.Executors
{
    public interface IAuthenticationExecutor<T> where T : ISession
    {
        Task<T> ExecuteAsync();
    }
}