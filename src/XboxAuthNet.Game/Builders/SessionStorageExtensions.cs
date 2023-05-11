using XboxAuthNet.Game.Accounts;

namespace XboxAuthNet.Game.Builders;

public static class SessionStorageExtensions
{
    public static TBuilder WithAccount<TBuilder>(
        this IBuilderWithSessionStorage<TBuilder> self,
        IXboxGameAccount account)
    {
        return self.WithSessionStorage(account.SessionStorage);
    }

    public static TBuilder WithDefaultAccount<TBuilder>(
        this IBuilderWithSessionStorage<TBuilder> self,
        IXboxGameAccountManager accountManager)
    {
        var defaultAccount = accountManager.GetDefaultAccount();
        return self.WithAccount(defaultAccount);
    }

    public static TBuilder WithNewAccount<TBuilder>(
        this IBuilderWithSessionStorage<TBuilder> self,
        IXboxGameAccountManager accountManager)
        where TBuilder : XboxGameAuthenticationBuilder<TBuilder>
    {
        var newAccount = accountManager.NewAccount();
        return self.WithAccount(newAccount);
    }
}