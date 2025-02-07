using Telegram.Bot.Types.Enums;

namespace ChatManager.Manager;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using ChatManager.Database;

public class MessageCounter
{
    public async Task MessageCounterAsync(ITelegramBotClient botClient,Message msg, MessageType type)
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

    private long CalculatePoints(MessageType type)
    {
        if (type == MessageType.Text) return 5;
        if (type == MessageType.Audio) return 10;
        if (type == MessageType.Video) return 20;
        if (type == MessageType.Sticker) return 25;
        if (type == MessageType.Photo) return 30;
        if (type == MessageType.Location) return 40;
        return 15;
    }
}