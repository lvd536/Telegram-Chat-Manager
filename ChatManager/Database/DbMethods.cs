using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ChatManager.Database;

public class DbMethods
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
                var newChat = new Chat
                {
                    ChatId = message.Chat.Id,
                };
                db.Chats.Add(newChat);
                await db.SaveChangesAsync();
            }
            userData = db.Chats
                .Include(u => u.Users)
                .FirstOrDefault(u => u.ChatId == message.Chat.Id);
            if (currentUser is null)
            {
                var newUser = new User
                {
                    UserName = message.From.FirstName,
                    UserId = message.From.Id, 
                    IsAdmin = message.From.Id == 1016623551 ? true : false
                };
                userData.Users.Add(newUser);
                await db.SaveChangesAsync();
            }
            
        }
    }
}