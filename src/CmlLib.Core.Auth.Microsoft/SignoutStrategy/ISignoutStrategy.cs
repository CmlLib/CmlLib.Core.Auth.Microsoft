using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft.SignoutStrategy
{
    public interface ISignoutStrategy
    {
        ValueTask Signout();
    }
}