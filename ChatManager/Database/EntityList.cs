namespace ChatManager.Database;

public class EntityList
{
    public class Chat
    {
        public int Id { get; set; }
        public long ChatId { get; set; }
        public List<User>? Users { get; set; } = new List<User>();
        public List<Word> Words { get; set; } = null!;
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
        public string BlockWord { get; set; } = string.Empty;
        public int ChatId { get; set; }
        public Chat Chat { get; set; } = null!;
    }
}