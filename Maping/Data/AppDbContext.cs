using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<District> Districts => Set<District>();
    public DbSet<Entity> Entities => Set<Entity>();
    public DbSet<Municipality> Municipalities => Set<Municipality>();
    public DbSet<Province> Provinces => Set<Province>();
    public DbSet<Restaurant> Restaurants => Set<Restaurant>();
    public DbSet<EntityType> Types => Set<EntityType>();
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Mapificacion;User Id=postgres;Password=manu04",
            o => o.UseNetTopologySuite());
    }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Entity>()
            .HasOne(e => e.user)
            .WithMany(u => u.entities)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        mb.Entity<EntityType>()
            .HasMany(et => et.Entities)
            .WithOne(e => e.EntityType)
            .HasForeignKey(e => e.EntityTypeId)
            .OnDelete(DeleteBehavior.Cascade);

        mb.Entity<Restaurant>()
            .HasOne(r => r.Entity)
            .WithOne(e => e.Restaurant)
            .HasForeignKey<Restaurant>(r => r.EntityId)
            .OnDelete(DeleteBehavior.Cascade);

        mb.Entity<Province>()
            .HasMany(p => p.Municipalities)
            .WithOne(m => m.Province)
            .HasForeignKey(m => m.ProvinceId)
            .OnDelete(DeleteBehavior.Cascade);

        mb.Entity<Municipality>()
            .HasMany(m => m.Districts)
            .WithOne(d => d.Municipality)
            .HasForeignKey(d => d.MunicipalityId)
            .OnDelete(DeleteBehavior.Cascade);

        mb.Entity<District>()
            .HasMany(e => e.Entities)
            .WithOne(e => e.District)
            .HasForeignKey(d => d.DistrictId)
            .OnDelete(DeleteBehavior.Cascade);

        mb.Entity<IdentityUserLogin<string>>()
            .HasKey(login => new { login.LoginProvider, login.ProviderKey });

        mb.Entity<IdentityUserRole<string>>()
            .HasKey(role => new { role.UserId, role.RoleId });

        mb.Entity<IdentityUserToken<string>>()
            .HasKey(token => new { token.UserId, token.LoginProvider, token.Name });

        mb.Entity<District>(entity =>
        {
            entity.Property(d => d.Geom).HasColumnType("geometry(Polygon, 4326)");
        });

        mb.Entity<Entity>(entity =>
        {
            entity.Property(e => e.Geom).HasColumnType("geometry(Point, 4326)");
        });

        mb.Entity<Municipality>(entity =>
        {
            entity.Property(m => m.Geom).HasColumnType("geometry(Polygon, 4326)");
        });

        mb.Entity<Province>(entity =>
        {
            entity.Property(p => p.Geom).HasColumnType("geometry(Polygon, 4326)");
        });
    }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}