using ChatManager.Database;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatManager.Manager.Commands;

public class ProfileCommand
{
    public async Task ProfileCmd(ITelegramBotClient botClient, Message msg)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var userData = db.Chats
                .Include(u => u.Users)
                .FirstOrDefault(c => c.ChatId == msg.Chat.Id);
            var currentUser = userData.Users.FirstOrDefault(u => u.UserId == msg.From.Id);
            var message = $"Профиль пользователя {msg.From.FirstName}" +
                          $"Уровень: {currentUser.Level}" +
                          $"Опыт: {currentUser.Points}" +
                          $"Всего сообщений: {currentUser.Messages}" +
                          $"Текстовых сообщений: {currentUser.TextMessages}" +
                          $"Голосовых сообщений: {currentUser.AudioMessages}" +
                          $"Кружков: {currentUser.VideoMessages}" +
                          $"Стикеров: {currentUser.StickerMessages}" +
                          $"Фото: {currentUser.PhotoMessages}" +
                          $"Гео: {currentUser.LocationMessages}" +
                          $"Других: {currentUser.OtherMessages}";
            await botClient.SendMessage(msg.Chat.Id, message, ParseMode.Html);
        }
    }
}