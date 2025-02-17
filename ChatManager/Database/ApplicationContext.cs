namespace ChatManager.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class ApplicationContext : DbContext
{
    public DbSet<EntityList.Chat> Chats => Set<EntityList.Chat>();

    public ApplicationContext()
    {
        Database.Migrate();
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=database.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EntityList.Chat>()
            .HasMany(u => u.Users)
            .WithOne(i => i.Chat)
            .HasForeignKey(i => i.ChatId);
        
        modelBuilder.Entity<EntityList.User>()
            .HasIndex(i => i.ChatId);

        modelBuilder.Entity<EntityList.User>()
            .HasMany(i => i.Warns)
            .WithOne(w => w.User)
            .HasForeignKey(w => w.UserId);
        
        modelBuilder.Entity<EntityList.Warn>()
            .HasIndex(w => w.UserId);
        
        modelBuilder.Entity<EntityList.User>()
            .HasMany(i => i.Mutes)
            .WithOne(w => w.User)
            .HasForeignKey(w => w.UserId);
        
        modelBuilder.Entity<EntityList.Mute>()
            .HasIndex(w => w.UserId);
        
        modelBuilder.Entity<EntityList.User>()
            .HasOne(i => i.Ban)
            .WithOne(w => w.User)
            .HasForeignKey<EntityList.Ban>(w => w.UserId);
        
        modelBuilder.Entity<EntityList.Ban>()
            .HasIndex(w => w.UserId);
        
        modelBuilder.Entity<EntityList.User>()
            .HasOne(i => i.Kick)
            .WithOne(w => w.User)
            .HasForeignKey<EntityList.Kick>(w => w.UserId);
        
        modelBuilder.Entity<EntityList.Kick>()
            .HasIndex(w => w.UserId);
        
        modelBuilder.Entity<EntityList.Chat>()
            .HasMany(c => c.Words)
            .WithOne(w => w.Chat)
            .HasForeignKey(w => w.ChatId);
        
        modelBuilder.Entity<EntityList.Word>()
            .HasIndex(w => w.ChatId);
    }
}

public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
{
    public ApplicationContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
        optionsBuilder.UseSqlite("Data Source=database.db");
        return new ApplicationContext(optionsBuilder.Options);
    }
}