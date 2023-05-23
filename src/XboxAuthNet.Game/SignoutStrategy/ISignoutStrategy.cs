using System.Threading.Tasks;

namespace XboxAuthNet.Game.SignoutStrategy
{
    public interface ISignoutStrategy
    {
        ValueTask Signout();
    }
}