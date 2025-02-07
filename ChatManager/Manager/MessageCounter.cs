namespace ChatManager.Manager;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using ChatManager.Database;

public class MessageCounter
{
    public async Task MessageCounterAsync(Message msg)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var userData = db.Chats
                .Include(u => u.Users)
                .FirstOrDefault(u => u.ChatId == msg.Chat.Id);
            var currentUser = userData?.Users?.FirstOrDefault(u => u.UserId == msg.From?.Id);
            if (userData is null || currentUser is null)
            {
                await DbMethods.InitializeUserAsync(msg);
                return;
            }

            currentUser.Messages++;
            await db.SaveChangesAsync();
        }
    }
}