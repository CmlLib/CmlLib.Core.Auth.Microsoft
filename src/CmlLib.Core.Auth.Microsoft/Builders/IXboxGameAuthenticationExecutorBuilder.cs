using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public interface IXboxGameAuthenticationExecutorBuilder
    {
        Task<XboxGameSession> ExecuteAsync();
    }
}