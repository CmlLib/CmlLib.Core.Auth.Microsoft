using System.Globalization;
using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game;

public class LastAccessSource : ISessionSource<DateTime>
{
    private static LastAccessSource? _default;
    public static LastAccessSource Default => _default ??= new();

    public static string KeyName { get; } = "lastAccess";

    public DateTime Get(ISessionStorage sessionStorage)
    {
        var dateTimeStr = sessionStorage.Get<string>(KeyName);
        if (DateTime.TryParse(dateTimeStr, out var dateTime))
            return dateTime;
        else
            return DateTime.MinValue;
    }

    public void Set(ISessionStorage sessionStorage, DateTime value)
    {
        var dateTimeStr = value.ToString("o", CultureInfo.InvariantCulture);
        sessionStorage.Set<string>(KeyName, dateTimeStr);
    }

    public void SetToNow(ISessionStorage sessionStorage) =>
        Set(sessionStorage, DateTime.UtcNow);

    public void Clear(ISessionStorage sessionStorage) => 
        Set(sessionStorage, DateTime.MinValue);
}