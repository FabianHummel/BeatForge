using Microsoft.EntityFrameworkCore;

namespace BeatForgeClient.Infrastructure;

public class BeatForgeContext : DbContext
{
    public BeatForgeContext() { }

    public BeatForgeContext(DbContextOptions<BeatForgeContext> options)
        : base(options) { }
    
    public DbSet<Song> Songs => Set<Song>();
    public DbSet<Channel> Channels => Set<Channel>();
    public DbSet<Note> Notes => Set<Note>();
    public DbSet<Preferences> Preferences => Set<Preferences>();
    public DbSet<Instrument> Instruments => Set<Instrument>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("DataSource=BeatForge.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}