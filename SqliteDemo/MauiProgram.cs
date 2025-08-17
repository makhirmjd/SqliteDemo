using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SqliteDemo.Shared.Models;
using SqliteDemo.Repositories;
using SqliteDemo.Services;
using SqliteDemo.ViewModels;
using SqliteDemo.Data;
using SqliteDemo.Shared.Services;

namespace SqliteDemo;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite($"Filename={DatabasePath}"));
        using (AsyncServiceScope scope = builder.Services.BuildServiceProvider().CreateAsyncScope())
        {
            ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
        }
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif
        ConfigureServices(builder.Services);

        return builder.Build();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<BaseRepository<Customer>>();
        services.AddScoped<BaseRepository<Order>>();
        services.AddScoped<MainPageViewModel>();
    }
}
