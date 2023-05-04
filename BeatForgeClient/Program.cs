using Avalonia;
using Avalonia.ReactiveUI;
using System;
using AutoMapper;
using BeatForgeClient.Dto;
using BeatForgeClient.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BeatForgeClient;

internal static class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        var db = BuildContext().Database;
        {
            if (args.Length > 0 && args[0] == "--reset-db")
            {
                db.EnsureDeleted();
            }
            db.EnsureCreated();
        }

        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }
    
    public static IMapper Mapper { get; } = BuildMapper();

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();
    
    public static BeatForgeContext BuildContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<BeatForgeContext>();
        optionsBuilder.UseSqlite("DataSource=BeatForge.db");
        return new BeatForgeContext(optionsBuilder.Options);
    }

    public static IMapper BuildMapper() =>
        new MapperConfiguration(cfg => 
                cfg.AddMaps(typeof(DtoMappingProfile)))
            .CreateMapper();
}