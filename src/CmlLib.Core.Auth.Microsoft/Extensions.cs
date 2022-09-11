using System.IO;

namespace CmlLib.Core.Auth.Microsoft
{
    public static class Extensions
    {
        public static JavaEditionLoginHandlerBuilder ForJavaEdition(this LoginHandlerBuilder builder)
        {
            var context = builder.Build();
            if (string.IsNullOrEmpty(context.CachePath))
                context.CachePath = Path.Combine(MinecraftPath.GetOSDefaultPath(), "cml_xsession.json");
            return new JavaEditionLoginHandlerBuilder(context);
        }
    }
}
