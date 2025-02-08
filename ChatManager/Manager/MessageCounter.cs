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
            currentUser.Points += CalculatePoints(type);
            await botClient.SendMessage(msg.Chat.Id, $"У вас {currentUser.Points} поинтов и {currentUser.Level} уровень", ParseMode.Html);
            if (currentUser.Points >= CalculateLevel(currentUser.Level))
            {
                currentUser.Level++;
                await botClient.SendMessage(msg.Chat.Id, $"Поздравляю, {msg?.From?.FirstName}! Вы получили {currentUser.Level} уровень!", ParseMode.Html);
            }
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

    public long CalculateLevel(long level)
    {
        if (level <= 5) return (1500 * level);
        else if (level <= 10) return (250 * level);
        else if (level <= 20) return (350 * level);
        else if (level <= 30) return (550 * level);
        else if (level <= 40) return (750 * level);
        else if (level <= 50) return (950 * level); 
        else if (level <= 60) return (1150 * level);
        else if (level <= 70) return (1350 * level);
        else if (level <= 80) return (1550 * level);
        else if (level <= 90) return (1750 * level);
        else if (level <= 100) return (1950 * level);
        else if (level <= 150) return (2500 * level);
        else if (level <= 160) return (2700 * level);
        else if (level <= 170) return (2900 * level);
        else if (level <= 180) return (3100 * level);
        else if (level <= 190) return (3300 * level);
        else return (4000 * level);
    }
}