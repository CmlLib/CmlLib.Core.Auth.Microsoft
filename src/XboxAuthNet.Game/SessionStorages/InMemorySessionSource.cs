namespace XboxAuthNet.Game.SessionStorages
{
    public class InMemorySessionSource<T> : ISessionSource<T>
    {
        private T? session = default(T);

        public T? Get() => session;

        public void Set(T? obj)
        {
            session = obj;
        }

        public void Clear()
        {
            Set(default(T?));
        }
    }
}