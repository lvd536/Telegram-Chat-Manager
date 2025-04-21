using ChatManager.Database;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
namespace ChatManager.Manager.Commands.AdminTools;

public static class UserInfoCommand
{
        public static async Task UserInfo(ITelegramBotClient botClient, Message msg)
    {
        var member = await DbMethods.GetMemberAsync(botClient, msg);
        if (msg.ReplyToMessage is null) return;
        if (member.Status != ChatMemberStatus.Administrator && member.Status != ChatMemberStatus.Creator)
        {
            await botClient.SendMessage(msg.Chat.Id, "У вас недостаточно прав чтобы использовать эту комманду.",
                ParseMode.Html);
            return;
        }

        using (ApplicationContext db = new ApplicationContext())
        {
            var userData = await DbMethods.GetUserDataAsync(db, msg);
            var currentUser = await DbMethods.GetReplyUserAsync(msg, userData);
            if (currentUser?.Ban is null) currentUser.Ban = new EntityList.Ban();
            if (currentUser?.Kick is null) currentUser.Kick = new EntityList.Kick();
            if (currentUser?.Mutes is null) currentUser.Mutes = new List<EntityList.Mute>();
            if (currentUser?.Warns is null) currentUser.Warns = new List<EntityList.Warn>();
            var banDetails = currentUser.Ban.Description == String.Empty ? "Не имеет банов" : currentUser.Ban.Description;
            var kickDetails = currentUser.Kick.Description == String.Empty ? "Не имеет киков" : currentUser.Kick.Description;
            var muteDetails = string.Empty;
            try
            {
                muteDetails = currentUser?.Mutes.Last().Description;
            }
            catch (InvalidOperationException)
            {
                muteDetails = "Нет мутов";
            }
            var message = $"👤 Информация о пользователе: {msg.ReplyToMessage.From?.FirstName}:\n" +
                          $"🆔 ID: {msg.ReplyToMessage.From?.Id}\n" +
                          $"📌 Tag: {msg.ReplyToMessage.From?.Username}\n" +
                          $"🚫 Причина бана: {banDetails}\n" +
                          $"⛔️ Причина кика: {kickDetails}\n" +
                          $"🔇 Причина мута: {muteDetails}\n" +
                          $"⚠️ Кол-во предупреждений: {currentUser?.Warns.Count}\n";

            if (currentUser?.Warns != null && currentUser.Warns.Any())
            {
                message += $"Причина послед. пред-я: {currentUser.Warns.Last().Description}";
            }
            else
            {
                message += "Причина послед. пред-я: Нет предупреждений";
            }

            await botClient.SendMessage(msg.Chat.Id, "<blockquote>" + message + "</blockquote>", ParseMode.Html);
        }
    }
}