﻿using ChatManager.Database;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ChatManager.Manager.Commands;

public static class ProfileCommand
{
    public static async Task ProfileCmd(ITelegramBotClient botClient, Message msg)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var userData = db.Chats
                .Include(u => u.Users)
                .FirstOrDefault(c => c.ChatId == msg.Chat.Id);
            var currentUser = userData.Users.FirstOrDefault(u => u.UserId == msg.From.Id);
            var message = $"<blockquote>👤 Профиль пользователя {msg.From.FirstName}\n" +
                          $"⭐️ Уровень: {currentUser.Level}\n" +
                          $"✨ Опыт: {currentUser.Points}\n" +
                          $"📊 Всего сообщений: {currentUser.Messages}\n" +
                          $"💬 Текстовых сообщений: {currentUser.TextMessages}\n" +
                          $"🎤 Аудио сообщений: {currentUser.AudioMessages}\n" +
                          $"⭕️ Видео: {currentUser.VideoMessages}\n" +
                          $"😀 Стикеров: {currentUser.StickerMessages}\n" +
                          $"📷 Фото: {currentUser.PhotoMessages}\n" +
                          $"📍 Гео: {currentUser.LocationMessages}\n" +
                          $"📦 Других: {currentUser.OtherMessages} </blockquote>";
            var keyboard = new InlineKeyboardMarkup()
                .AddButton("Топ", "TopByLevel")
                .AddButton("Узнать свой TG ID", "IdCall");
            await botClient.SendMessage(msg.Chat.Id, message, ParseMode.Html, replyMarkup: keyboard);
        }
    }
}