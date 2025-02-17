namespace ChatManager.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class Chat
{
    public int Id { get; set; }
    public long ChatId { get; set; }
    public List<User>? Users { get; set; } = new List<User>();
    public List<Word>? Words { get; set; } = new List<Word>();
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
    public List<Warn> Warns { get; set; } = null!;
    public List<Mute> Mutes { get; set; } = null!;
    public Ban Ban { get; set; } = null!;
    public Kick Kick { get; set; } = null!;
}

public class Warn
{
    public int Id { get; set; }
    
    public string Description { get; set; } = string.Empty;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}

public class Ban
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}

public class Mute
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}

public class Kick
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}

public class Word
{
    public int Id { get; set; }
    public string BlockWord { get; set; } = String.Empty;
    public int ChatId { get; set; }
    public Chat Chat { get; set; } = null!;
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
            .HasMany(i => i.Warns)
            .WithOne(w => w.User)
            .HasForeignKey(w => w.UserId);
        
        modelBuilder.Entity<Warn>()
            .HasIndex(w => w.UserId);
        
        modelBuilder.Entity<User>()
            .HasMany(i => i.Mutes)
            .WithOne(w => w.User)
            .HasForeignKey(w => w.UserId);
        
        modelBuilder.Entity<Mute>()
            .HasIndex(w => w.UserId);
        
        modelBuilder.Entity<User>()
            .HasOne(i => i.Ban)
            .WithOne(w => w.User)
            .HasForeignKey<Ban>(w => w.UserId);
        
        modelBuilder.Entity<Ban>()
            .HasIndex(w => w.UserId);
        
        modelBuilder.Entity<User>()
            .HasOne(i => i.Kick)
            .WithOne(w => w.User)
            .HasForeignKey<Kick>(w => w.UserId);
        
        modelBuilder.Entity<Kick>()
            .HasIndex(w => w.UserId);
        
        modelBuilder.Entity<Chat>()
            .HasMany(c => c.Words)
            .WithOne(w => w.Chat)
            .HasForeignKey(w => w.ChatId);
        
        modelBuilder.Entity<Word>()
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