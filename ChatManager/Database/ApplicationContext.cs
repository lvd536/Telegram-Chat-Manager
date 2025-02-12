namespace ChatManager.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class Chat
{
    public int Id { get; set; }
    public long ChatId { get; set; }
    public List<User>? Users { get; set; } = new List<User>();
}

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public long Messages { get; set; }
    public long TextMessages { get; set; }
    public long AudioMessages { get; set; }
    public long VideoMessages { get; set; }
    public long StickerMessages { get; set; }
    public long PhotoMessages { get; set; }
    public long LocationMessages { get; set; }
    public long OtherMessages { get; set; }
    public long Level { get; set; } = 1;
    public long Points { get; set; }
    public long UserId { get; set; }
    public int ChatId { get; set; }
    public bool IsAdmin { get; set; }
    public Chat Chat { get; set; } = null!;
    public Warn Warn { get; set; } = null!;
}

public class Warn
{
    public int Id { get; set; }
    public short Warns { get; set; }
    public string OneDescription { get; set; } = string.Empty;
    public string TwoDescription { get; set; } = string.Empty;
    public string ThreeDescription { get; set; } = string.Empty;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}

public class ApplicationContext : DbContext
{
    public DbSet<Chat> Chats => Set<Chat>();

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
        modelBuilder.Entity<Chat>()
            .HasMany(u => u.Users)
            .WithOne(i => i.Chat)
            .HasForeignKey(i => i.ChatId);
        
        modelBuilder.Entity<User>()
            .HasIndex(i => i.ChatId);

        modelBuilder.Entity<User>()
            .HasOne(i => i.Warn)
            .WithOne(w => w.User)
            .HasForeignKey<Warn>(w => w.UserId);
        
        modelBuilder.Entity<Warn>()
            .HasIndex(w => w.UserId);
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