using SqliteDemo.Shared.Services;

namespace SqliteDemo.Services;

public class CurrentUserService : ICurrentUserService
{
    public async Task<string?> GetUserId() => await SecureStorage.GetAsync("userId");
    public async Task<string?> GetUserName() => await SecureStorage.GetAsync("userName");
}
