using Telegram.Bot.Types.Enums;

namespace ChatManager.Manager;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Database;

public static class MessageCounter
{
    public static async Task MessageCounterAsync(ITelegramBotClient botClient, Message msg, MessageType type)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var userData = await db.Chats
                .Include(c => c.Users)
                .FirstOrDefaultAsync(u => u.ChatId == msg.Chat.Id);
            var currentUser = userData?.Users?.FirstOrDefault(u => u.UserId == msg.From?.Id);
            if (userData is null || currentUser is null)
            {
                await DbMethods.InitializeUserAsync(msg);
                return;
            }

            currentUser.Messages++;
            if (type == MessageType.Audio) currentUser.AudioMessages++;
            else if (type == MessageType.Text) currentUser.TextMessages++;
            else if (type == MessageType.Video) currentUser.VideoMessages++;
            else if (type == MessageType.Sticker) currentUser.StickerMessages++;
            else if (type == MessageType.Photo) currentUser.PhotoMessages++;
            else if (type == MessageType.Location) currentUser.LocationMessages++;
            else if (type == MessageType.Voice) currentUser.VoiceMessages++;
            else if (type == MessageType.VideoNote) currentUser.VideoNotesMessages++;
            else currentUser.OtherMessages++;
            currentUser.Points += CalculatePoints(type, msg);
            if (currentUser.Points >= CalculateLevel(currentUser.Level))
            {
                currentUser.Level++;
                await botClient.SendMessage(msg.Chat.Id, $"Поздравляю, {msg?.From?.FirstName}! Вы получили {currentUser.Level} уровень!", ParseMode.Html);
            }
            await db.SaveChangesAsync();
        }
    }
    private static long CalculatePoints(MessageType type, Message message)
    {
        switch (type)
        {
            case MessageType.Text:
                return message.Text != null ? Math.Max((int)(message.Text.Length * 0.2), 5) : 0;
            case MessageType.Audio:
                return 10;
            case MessageType.Video:
                return 20;
            case MessageType.Sticker:
                return 25;
            case MessageType.Photo:
                return 30;
            case MessageType.Location:
                return 40;
            case MessageType.Voice:
                return 25;
            case MessageType.VideoNote:
                return 35;
            default:
                return 15;
        }
    }

    private static long CalculateLevel(long level)
    {
        switch (level)
        {
            case <= 5: 
                return 150 * level;
            case <= 10: 
                return 250 * level;
            case <= 20: 
                return 350 * level;
            case <= 30: 
                return 550 * level;
            case <= 40: 
                return 750 * level;
            case <= 50: 
                return 950 * level;
            case <= 60: 
                return 1150 * level;
            case <= 70: 
                return 1350 * level;
            case <= 80: 
                return 1550 * level;
            case <= 90: 
                return 1750 * level;
            case <= 100: 
                return 1950 * level;
            case <= 150: 
                return 2500 * level;
            case <= 160: 
                return 2700 * level;
            case <= 170: 
                return 2900 * level;
            case <= 180: 
                return 3000 * level;
            case <= 190: 
                return 3500 * level;
            case > 190: 
                return 4000 * level;
        }
    }
}