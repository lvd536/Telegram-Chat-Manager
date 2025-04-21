using ChatManager.Database;
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
            var userData = await DbMethods.GetUserDataAsync(db, msg);
            var currentUser = await DbMethods.GetUserAsync(msg, userData);
            var message = $"<blockquote>👤 Профиль пользователя {msg.From.FirstName}\n" +
                          $"⭐️ Уровень: {currentUser.Level}\n" +
                          $"✨ Опыт: {currentUser.Points}\n" +
                          $"📊 Всего сообщений: {currentUser.Messages}\n" +
                          $"💬 Текстовых сообщений: {currentUser.TextMessages}\n" +
                          $"🎤 Аудио сообщений: {currentUser.AudioMessages}\n" +
                          $"🔊 Голосовых сообщений: {currentUser.VoiceMessages}\n" +
                          $"⭕️ Видео: {currentUser.VideoMessages}\n" +
                          $"🔵 Кружков: {currentUser.VideoNotesMessages}\n" +
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