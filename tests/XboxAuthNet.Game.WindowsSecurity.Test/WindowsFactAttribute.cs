using Xunit;

namespace XboxAuthNet.Game.WindowsSecurity.Test;

public sealed class WindowsFactAttribute : FactAttribute
{
    public WindowsFactAttribute()
    {
        if (!OperatingSystem.IsWindows())
            Skip = "Windows DPAPI is required.";
    }
}
