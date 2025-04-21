using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ChatManager.Database;

public static class DbMethods
{
    public static async Task InitializeUserAsync(Message message)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var userData = db.Chats
                .Include(u => u.Users)
                .FirstOrDefault(u => u.ChatId == message.Chat.Id);
            var currentUser = userData?.Users?.FirstOrDefault(u => u.UserId == message.From?.Id);
            if (userData is not null && currentUser is not null) return;
            if (userData?.ChatId is null)
            {
                var newChat = new EntityList.Chat
                {
                    ChatId = message.Chat.Id
                };
                db.Chats.Add(newChat);
                await db.SaveChangesAsync();
            }

            userData = db.Chats
                .Include(u => u.Users)
                .FirstOrDefault(u => u.ChatId == message.Chat.Id);
            if (currentUser is null)
            {
                var newUser = new EntityList.User
                {
                    UserName = message.From.FirstName,
                    UserId = message.From.Id,
                    IsAdmin = false
                };
                userData?.Users.Add(newUser);
                await db.SaveChangesAsync();
            }
        }
    }

    public static async Task<EntityList.Chat> GetUserDataAsync(ApplicationContext db, Message msg)
    {
        var userData = db.Chats
            .Include(u => u.Users)
            .ThenInclude(u => u.Warns)
            .Include(u => u.Users)
            .ThenInclude(u => u.Ban)
            .Include(u => u.Users)
            .ThenInclude(u => u.Kick)
            .Include(u => u.Users)
            .ThenInclude(u => u.Mutes)
            .Include(w => w.Words)
            .FirstOrDefault(u => u.ChatId == msg.Chat.Id);
        if (userData is null)
        {
            await InitializeUserAsync(msg);
            userData = db.Chats
                .Include(u => u.Users)
                .ThenInclude(u => u.Warns)
                .Include(u => u.Users)
                .ThenInclude(u => u.Ban)
                .Include(u => u.Users)
                .ThenInclude(u => u.Kick)
                .Include(u => u.Users)
                .ThenInclude(u => u.Mutes)
                .Include(w => w.Words)
                .FirstOrDefault(u => u.ChatId == msg.Chat.Id);
        }

        return userData;
    }
    
    public static async Task<EntityList.User> GetUserAsync(Message msg, EntityList.Chat chat)
    {
        var currentUser = chat.Users.FirstOrDefault(u => u.UserId == msg.From?.Id);
        if (currentUser is null)
        {
            await InitializeUserAsync(msg);
            currentUser = chat.Users.FirstOrDefault(u => u.UserId == msg.From?.Id);
        }
        
        return currentUser;
    }
    
    public static async Task<EntityList.User> GetReplyUserAsync(Message msg, EntityList.Chat chat)
    {
        var targetUser = chat.Users.FirstOrDefault(u => u.UserId == msg.ReplyToMessage.From.Id);
        return targetUser;
    }
    
    public static async Task<ChatMember> GetMemberAsync(ITelegramBotClient botClient, Message msg)
    {
        var member = await botClient.GetChatMember(msg.Chat.Id, msg.From.Id);
        return member;
    }
}