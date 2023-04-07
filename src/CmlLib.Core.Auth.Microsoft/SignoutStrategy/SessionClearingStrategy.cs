using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;

namespace CmlLib.Core.Auth.Microsoft.SignoutStrategy
{
    public class SessionClearingStrategy<T> : ISignoutStrategy
    {
        private readonly ISessionSource<T> _sessionSource;

        public SessionClearingStrategy(ISessionSource<T> sessionSource)
        {
            this._sessionSource = sessionSource;
        }

        public ValueTask Signout()
        {
            return _sessionSource.Clear();
        }
    }
}