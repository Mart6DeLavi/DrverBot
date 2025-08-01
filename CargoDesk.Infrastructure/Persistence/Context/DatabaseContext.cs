using CargoDesk.Infrastructure.Persistence.Entities;
using CargoDesk.Infrastructure.Persistence.Entities.Telegram;
using Microsoft.EntityFrameworkCore;

namespace CargoDesk.Infrastructure.Persistence.Context;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<AddressEntity> Addresses => Set<AddressEntity>();
    public DbSet<DriverEntity> Drivers => Set<DriverEntity>();
    public DbSet<CargoEntity> Cargos => Set<CargoEntity>();
    public DbSet<RouteEntity> Routes => Set<RouteEntity>();
    public DbSet<DriverChatMappingEntity> DriverChat => Set<DriverChatMappingEntity>();
    public DbSet<IssueEntity> Issues => Set<IssueEntity>();
    public DbSet<IssueProofEntity> IssueProofs => Set<IssueProofEntity>();
    public DbSet<DelayRequestEntity> DelayRequest => Set<DelayRequestEntity>();
    public DbSet<DriverWorkSessionEntity> DriverWorkSession => Set<DriverWorkSessionEntity>();
    public DbSet<RouteCargoStatusEntity> RouteCargoStatus => Set<RouteCargoStatusEntity>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var now = DateTime.UtcNow;

        SetCreatedTimestamps(now);
        SetUpdatedTimestamps(now);

        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetCreatedTimestamps(DateTime utcNow)
    {
        var entries = ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Added);

        foreach (var entry in entries)
        {
            entry.Entity.CreatedAt = utcNow;
        }
    }

    private void SetUpdatedTimestamps(DateTime utcNow)
    {
        var entries = ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            entry.Entity.UpdatedAt = utcNow;
        }
    }
}