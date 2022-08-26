using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft.XboxLive
{
    public class XboxSisuAuthParameters
    {
        public static XboxSisuAuthParameters CreateWin32()
            => CreateWin32(XboxGameTitles.MinecraftJava);

        public static XboxSisuAuthParameters CreateWin32(string cid)
        {
            return new XboxSisuAuthParameters(
                cid,
                XboxDeviceTypes.Win32,
                "0.0.0");
        }

        public static XboxSisuAuthParameters CreateNintendo()
            => CreateNintendo(XboxGameTitles.MinecraftNintendoSwitch);

        public static XboxSisuAuthParameters CreateNintendo(string cid)
        {
            return new XboxSisuAuthParameters(
                cid,
                XboxDeviceTypes.Nintendo,
                "0.0.0");
        }

        public static XboxSisuAuthParameters CreateiOS()
            => CreateiOS(XboxGameTitles.XboxAppIOS);

        public static XboxSisuAuthParameters CreateiOS(string cid)
        {
            return new XboxSisuAuthParameters(
                cid,
                XboxDeviceTypes.iOS,
                "0.0.0");
        }

        public XboxSisuAuthParameters(string clientId, string deviceType, string deviceVersion)
        {
            ClientId = clientId;
            DeviceType = deviceType;
            DeviceVersion = deviceVersion;
        }

        public string ClientId { get; }
        public string DeviceType { get; }
        public string DeviceVersion { get; }
    }
}
