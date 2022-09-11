using System;

namespace CmlLib.Core.Auth.Microsoft.XboxLive
{
    public class XboxSisuAuthParameters
    {
        public string? ClientId { get; set; }
        public string? DeviceType { get; set; }
        public string? DeviceVersion { get; set; }
        public string? TokenPrefix { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(ClientId))
                throw new ArgumentNullException(nameof(ClientId));

            if (string.IsNullOrEmpty(DeviceType))
                throw new ArgumentNullException(nameof(DeviceType));

            if (string.IsNullOrEmpty(DeviceVersion))
                throw new ArgumentNullException(nameof(DeviceVersion));
        }
    }
}
