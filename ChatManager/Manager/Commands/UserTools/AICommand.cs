using ChatManager.Database;
using ChatManager.Services;
using Microsoft.VisualBasic;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ChatManager.Manager.Commands.UserTools;

public static class AICommand
{
    public static async Task AiCmd(ITelegramBotClient botClient, Message msg, AiService service)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var userData = await DbMethods.GetUserDataAsync(db, msg);
            var currentUser = await DbMethods.GetUserAsync(msg, userData);
            bool canSendRequest = await HaveDailyRequests(botClient, msg, db, currentUser);
            if (!canSendRequest) return;
            currentUser.AiRequests--;
            await db.SaveChangesAsync();
        }

        if (msg.Text != null)
        {
            await service.MakeTextRequest(msg);
        }
        else if (msg.Photo != null)
        {
            await service.ProcessImageMessage(msg);
        }
    }

    public static async Task CheckLimitsCmd(Message msg, AiService service)
    {
        await service.SendCheckLimitsRequest(msg);
    }

    public static async Task AddRequestsCmd(ITelegramBotClient botClient, Message msg, short value)
    {
        if (msg.From is null) return;
        if (msg.ReplyToMessage == null)
        {
            await botClient.SendMessage(msg.Chat.Id, "Используйте ответ на сообщение пользователя, которому хотите выдать запросы!");
            return;
        }
        if (OnErrorService._ownerId != msg.From.Id)
        {
            await botClient.SendMessage(msg.Chat.Id, "Эту комманду может использовать только создатель бота!");
            return;
        }
        using (ApplicationContext db = new ApplicationContext())
        {
            var userData = await DbMethods.GetUserDataAsync(db, msg);
            var currentUser = await DbMethods.GetReplyUserAsync(msg, userData);

            currentUser.AiRequests += value;
            await botClient.SendMessage(msg.Chat.Id, $"Успешно выдал пользователю {currentUser.UserName} запросы!");
            await db.SaveChangesAsync();
        }
    }
    
    private static async Task<bool> HaveDailyRequests(ITelegramBotClient botClient, Message msg, ApplicationContext db, EntityList.User user)
    {
        if (user.LastRequestsReach == DateTime.MinValue && user.AiRequests == 0)
        {
            user.LastRequestsReach = DateTime.Now;
            user.AiRequests = 10;
            await db.SaveChangesAsync();
            return true;
        }

        if (user.AiRequests <= 0 && user.LastRequestsReach.AddDays(1) < DateTime.Now)
        {
            user.LastRequestsReach = DateTime.Now;
            user.AiRequests = 10;
            await db.SaveChangesAsync();
            await botClient.SendMessage(msg.Chat.Id, "Ваше кол-во ежедневных запросов восстановлено!");
            return true;
        }

        if (user.AiRequests <= 0 && user.LastRequestsReach.AddDays(1) > DateTime.Now)
        {
            await botClient.SendMessage(msg.Chat.Id, "У Вас кончились запросы. Попробуйте позже!");
            return false;
        }

        if (user.AiRequests > 0) return true;

        return false;
    }
}