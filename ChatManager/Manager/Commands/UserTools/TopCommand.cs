﻿using ChatManager.Database;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ChatManager.Manager.Commands;

public static class TopCommand
{
    public static async Task TopCmd(ITelegramBotClient botClient, Message msg, short type)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var userData = await DbMethods.GetUserDataAsync(db, msg);
            var users = userData.Users.ToList();
            var message = string.Empty;
            var keyboard = new InlineKeyboardMarkup()
                .AddButton("⭐️ Топ по уровню", "TopByLevel")
                .AddButton("📊 Топ по всем сообщениям", "TopByMessages")
                .AddNewRow()
                .AddButton("💬 Топ по текстовым", "TopByTextMessages")
                .AddButton("🎤 Топ по аудио", "TopByAudioMessages")
                .AddNewRow()
                .AddButton("⭕️ Топ по видео", "TopByVideoMessages")
                .AddButton("😀 Топ по стикерам", "TopBySticker")
                .AddNewRow()
                .AddButton("📷 Топ по фото", "TopByPhoto")
                .AddButton("📍 Топ по гео", "TopByLocation")
                .AddNewRow()
                .AddButton("🔊 Топ по голосовым", "TopByVoice")
                .AddButton("🔵 Топ по кружкам", "TopByCircle")
                .AddNewRow()
                .AddButton("🎞️Топ по гифкам", "TopByGif")
                .AddButton("📦 Топ по другим сообщениям", "TopByOther");
            switch (type)
            {
                case 1:
                    users.Sort((a, b) => (int)(b.Level - a.Level));
                    message = $"⭐️ Топ пользователей по уровню: ";
                    foreach (var u in users)
                    {
                        message += $"\n<blockquote>{u.UserName} - {u.Level} уровень</blockquote>";
                    }
                    try {
                        await botClient.EditMessageText(msg.Chat.Id, msg.Id, message, ParseMode.Html,
                            replyMarkup: keyboard);
                    }
                    catch (Exception) {
                        await botClient.SendMessage(msg.Chat.Id, message, ParseMode.Html, replyMarkup: keyboard);
                    }
                    break;
                case 2:
                    users.Sort((a, b) => (int)(b.Messages - a.Messages));
                    message = $"📊 Топ пользователей по всем сообщениям: ";
                    foreach (var u in users)
                    {
                        message += $"\n<blockquote>{u.UserName} - {u.Messages} сообщений</blockquote>";
                    }
                    try {
                        await botClient.EditMessageText(msg.Chat.Id, msg.Id, message, ParseMode.Html,
                            replyMarkup: keyboard);
                    }
                    catch (Exception) {
                        await botClient.SendMessage(msg.Chat.Id, message, ParseMode.Html, replyMarkup: keyboard);
                    }
                    break;
                case 3:
                    users.Sort((a, b) => (int)(b.TextMessages - a.TextMessages));
                    message = $"💬 Топ пользователей по текстовым сообщениям: ";
                    foreach (var u in users)
                    {
                        message += $"\n<blockquote>{u.UserName} - {u.TextMessages} текстовых сообщений</blockquote>";
                    }
                    try {
                        await botClient.EditMessageText(msg.Chat.Id, msg.Id, message, ParseMode.Html,
                            replyMarkup: keyboard);
                    }
                    catch (Exception) {
                        await botClient.SendMessage(msg.Chat.Id, message, ParseMode.Html, replyMarkup: keyboard);
                    }
                    break;
                case 4:
                    users.Sort((a, b) => (int)(b.AudioMessages - a.AudioMessages));
                    message = $"🎤 Топ пользователей по аудио: ";
                    foreach (var u in users)
                    {
                        message += $"\n<blockquote>{u.UserName} - {u.AudioMessages} аудио</blockquote>";
                    }
                    try {
                        await botClient.EditMessageText(msg.Chat.Id, msg.Id, message, ParseMode.Html,
                            replyMarkup: keyboard);
                    }
                    catch (Exception) {
                        await botClient.SendMessage(msg.Chat.Id, message, ParseMode.Html, replyMarkup: keyboard);
                    }
                    break;
                case 5:
                    users.Sort((a, b) => (int)(b.VideoMessages - a.VideoMessages));
                    message = $"⭕️ Топ пользователей по видео: ";
                    foreach (var u in users)
                    {
                        message += $"\n<blockquote>{u.UserName} - {u.VideoMessages} видео</blockquote>";
                    }
                    try {
                        await botClient.EditMessageText(msg.Chat.Id, msg.Id, message, ParseMode.Html,
                            replyMarkup: keyboard);
                    }
                    catch (Exception) {
                        await botClient.SendMessage(msg.Chat.Id, message, ParseMode.Html, replyMarkup: keyboard);
                    }
                    break;
                case 6:
                    users.Sort((a, b) => (int)(b.StickerMessages - a.StickerMessages));
                    message = $"😀 Топ пользователей по стикерам: ";
                    foreach (var u in users)
                    {
                        message += $"\n<blockquote>{u.UserName} - {u.StickerMessages} стикеров</blockquote>";
                    }
                    try {
                        await botClient.EditMessageText(msg.Chat.Id, msg.Id, message, ParseMode.Html,
                            replyMarkup: keyboard);
                    }
                    catch (Exception) {
                        await botClient.SendMessage(msg.Chat.Id, message, ParseMode.Html, replyMarkup: keyboard);
                    }
                    break;
                case 7:
                    users.Sort((a, b) => (int)(b.PhotoMessages - a.PhotoMessages));
                    message = $"📷 Топ пользователей по фото: ";
                    foreach (var u in users)
                    {
                        message += $"\n<blockquote>{u.UserName} - {u.PhotoMessages} фото</blockquote>";
                    }
                    try {
                        await botClient.EditMessageText(msg.Chat.Id, msg.Id, message, ParseMode.Html,
                            replyMarkup: keyboard);
                    }
                    catch (Exception) {
                        await botClient.SendMessage(msg.Chat.Id, message, ParseMode.Html, replyMarkup: keyboard);
                    }
                    break;
                case 8:
                    users.Sort((a, b) => (int)(b.LocationMessages - a.LocationMessages));
                    message = $"📍 Топ пользователей по гео: ";
                    foreach (var u in users)
                    {
                        message += $"\n<blockquote>{u.UserName} - {u.LocationMessages} гео</blockquote>";
                    }
                    try {
                        await botClient.EditMessageText(msg.Chat.Id, msg.Id, message, ParseMode.Html,
                            replyMarkup: keyboard);
                    }
                    catch (Exception) {
                        await botClient.SendMessage(msg.Chat.Id, message, ParseMode.Html, replyMarkup: keyboard);
                    }
                    break;
                case 9:
                    users.Sort((a, b) => (int)(b.OtherMessages - a.OtherMessages));
                    message = $"📦 Топ пользователей по другим сообщениям: ";
                    foreach (var u in users)
                    {
                        message += $"\n<blockquote>{u.UserName} - {u.OtherMessages} других сообщений</blockquote>";
                    }
                    try {
                        await botClient.EditMessageText(msg.Chat.Id, msg.Id, message, ParseMode.Html,
                            replyMarkup: keyboard);
                    }
                    catch (Exception) {
                        await botClient.SendMessage(msg.Chat.Id, message, ParseMode.Html, replyMarkup: keyboard);
                    }
                    break;
                case 10:
                    users.Sort((a, b) => (int)(b.VoiceMessages - a.VoiceMessages));
                    message = $"🔊 Топ пользователей по голосовым: ";
                    foreach (var u in users)
                    {
                        message += $"\n<blockquote>{u.UserName} - {u.VoiceMessages} голосовых</blockquote>";
                    }
                    try {
                        await botClient.EditMessageText(msg.Chat.Id, msg.Id, message, ParseMode.Html,
                            replyMarkup: keyboard);
                    }
                    catch (Exception) {
                        await botClient.SendMessage(msg.Chat.Id, message, ParseMode.Html, replyMarkup: keyboard);
                    }
                    break;
                case 11:
                    users.Sort((a, b) => (int)(b.VideoNotesMessages - a.VideoNotesMessages));
                    message = $"🔵 Топ пользователей по кружкам: ";
                    foreach (var u in users)
                    {
                        message += $"\n<blockquote>{u.UserName} - {u.VideoNotesMessages} кружков</blockquote>";
                    }
                    try {
                        await botClient.EditMessageText(msg.Chat.Id, msg.Id, message, ParseMode.Html,
                            replyMarkup: keyboard);
                    }
                    catch (Exception) {
                        await botClient.SendMessage(msg.Chat.Id, message, ParseMode.Html, replyMarkup: keyboard);
                    }
                    break;
                case 12:
                    users.Sort((a, b) => (int)(b.AnimationMessages - a.AnimationMessages));
                    message = $"🎞️Топ пользователей по гифкам: ";
                    foreach (var u in users)
                    {
                        message += $"\n<blockquote>{u.UserName} - {u.AnimationMessages} гифок</blockquote>";
                    }
                    try {
                        await botClient.EditMessageText(msg.Chat.Id, msg.Id, message, ParseMode.Html,
                            replyMarkup: keyboard);
                    }
                    catch (Exception) {
                        await botClient.SendMessage(msg.Chat.Id, message, ParseMode.Html, replyMarkup: keyboard);
                    }
                    break;
            }
        }
    }
}