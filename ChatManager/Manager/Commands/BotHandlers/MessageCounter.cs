﻿using Telegram.Bot.Types.Enums;

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
        if (type == MessageType.Text) return Math.Max((int)(message.Text.Length * 0.2), 5);
        if (type == MessageType.Audio) return 10;
        if (type == MessageType.Video) return 20;
        if (type == MessageType.Sticker) return 25;
        if (type == MessageType.Photo) return 30;
        if (type == MessageType.Location) return 40;
        return 15;
    }

    private static long CalculateLevel(long level)
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