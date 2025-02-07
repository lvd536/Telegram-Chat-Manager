using System.ComponentModel.DataAnnotations;
using Telegram.Bot.Types;

namespace ChatManager.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class Chat
{
    public int Id { get; set; }
    public long ChatId { get; set; }
    public List<Users>? Users { get; set; }
}

public class Users
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public long Messages { get; set; }
    public long Level { get; set; } = 1;
    public long Points { get; set; }
    public bool IsAdmin { get; set; }
    public Chat Chat { get; set; }
    public int ChatId { get; set; }
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
            .HasMany(c => c.Users);
        
        modelBuilder.Entity<Chat>()
            .HasMany(p => p.Users)
            .WithOne(i => i.Chat)
            .HasForeignKey(i => i.ChatId);
        
        modelBuilder.Entity<Users>()
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