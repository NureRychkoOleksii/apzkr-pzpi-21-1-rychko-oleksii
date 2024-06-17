using Backend.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Core;

public class StarOfLifeContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Newborn> Newborns { get; set; }
    public DbSet<Parent> Parents { get; set; }
    public DbSet<Sensor> Sensors { get; set; }
    public DbSet<SensorSettings> SensorSettings { get; set; }
    public DbSet<MedicalData> MedicalDatas { get; set; }
    public DbSet<Alert> Alerts { get; set; }    
    public DbSet<Analysis> Analyses { get; set; }
    
    public DbSet<UserParent> UserParents { get; set; }

    public StarOfLifeContext(DbContextOptions<StarOfLifeContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        
        builder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
        
        builder.Entity<UserParent>()
            .HasKey(up => new { up.ParentId, up.NewbornId });  
        
        builder.Entity<UserParent>()
            .HasOne(up => up.Parent)
            .WithMany(p => p.UserParents)
            .HasForeignKey(up => up.ParentId);  
        
        builder.Entity<UserParent>()
            .HasOne(up => up.Newborn)
            .WithMany(n => n.UserParents)
            .HasForeignKey(up => up.NewbornId);
    }
}