using ChatManager.Database;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatManager.Manager.Commands;

public class TopCommand
{
    public async Task TopCmd(ITelegramBotClient botClient, Message msg)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var userData = db.Chats
                .Include(u => u.Users)
                .FirstOrDefault(u => u.ChatId == msg.Chat.Id);
            var users = userData.Users.ToList();
            users.Sort((a, b) => (int)(b.Level - a.Level));
            var message = $"Топ пользователей по уровню: ";
            foreach (var u in users)
            {
                message += $"\n<blockquote>{u.UserName} - {u.Level} уровень</blockquote>";
            }
            await botClient.SendMessage(msg.Chat.Id, message, ParseMode.Html);
        }
    }
}