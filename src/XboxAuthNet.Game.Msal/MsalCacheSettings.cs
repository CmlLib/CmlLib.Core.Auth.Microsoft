using Microsoft.Identity.Client.Extensions.Msal;

namespace XboxAuthNet.Game.Msal;

public class MsalCacheSettings
{
    public string CacheFileName { get; set; } = "cmllib_msal_cache.txt";
    public string CacheDir { get; set; } = MsalCacheHelper.UserRootDirectory;

    public string KeyChainServiceName { get; set; } = "cmllib_msal_service";
    public string KeyChainAccountName { get; set; } = "cmllib_msal_account";

    public string LinuxKeyRingSchema { get; set; } = "com.github.cmllib.tokencache";
    public string LinuxKeyRingCollection { get; set; } = MsalCacheHelper.LinuxKeyRingDefaultCollection;
    public string LinuxKeyRingLabel { get; set; } = "MSAL token cache for Minecraft launcher based on CmlLib.Core";
    public KeyValuePair<string, string> LinuxKeyRingAttr1 { get; set; } = new KeyValuePair<string, string>("Version", "1");
    public KeyValuePair<string, string> LinuxKeyRingAttr2 { get; set; } = new KeyValuePair<string, string>("ProductGroup", "CmlLib.Core");

    public StorageCreationPropertiesBuilder ToStorageCreationPropertiesBuilder()
    {
        return new StorageCreationPropertiesBuilder(CacheFileName, CacheDir)
            .WithLinuxKeyring(
                LinuxKeyRingSchema,
                LinuxKeyRingCollection,
                LinuxKeyRingLabel,
                LinuxKeyRingAttr1,
                LinuxKeyRingAttr2)
            .WithMacKeyChain(
                KeyChainServiceName,
                KeyChainAccountName);
    }
}
