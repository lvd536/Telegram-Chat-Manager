using System.ComponentModel.DataAnnotations;
using Telegram.Bot.Types;

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
    public bool IsAdmin { get; set; }
    public int ChatId { get; set; }
    public long UserId { get; set; }
    public Chat Chat { get; set; }
}

public class ApplicationContext : DbContext
{
    public DbSet<Chat> Chats => Set<Chat>();
    public ApplicationContext()
    {
        Database.Migrate();
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
    }
}

public class SampleContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
{
    public ApplicationContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
        optionsBuilder.UseSqlite("Data Source=database.db");
        
        return new ApplicationContext();
    }
}