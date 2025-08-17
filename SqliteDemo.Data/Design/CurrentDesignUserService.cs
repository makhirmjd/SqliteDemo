using SqliteDemo.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqliteDemo.Data.Design;

public class CurrentDesignUserService : ICurrentUserService
{
    public Task<string?> GetUserId() => Task.FromResult("makhirmjd@gmail.com")!;

    public Task<string?> GetUserName() => Task.FromResult("Muhammad Abdulmalik")!;
}
