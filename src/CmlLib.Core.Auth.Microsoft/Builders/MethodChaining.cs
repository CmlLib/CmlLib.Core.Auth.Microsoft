
namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public abstract class MethodChaining<T>
    {
        private readonly T _returning;

        public MethodChaining(T returning)
        {
            this._returning = returning;
        }

        protected T GetThis()
        {
            return _returning;
        }
    }
}