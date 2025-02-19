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
        var member = await botClient.GetChatMember(msg.Chat.Id, msg.From.Id);
        if (msg.ReplyToMessage is null) return;
        if (member.Status != ChatMemberStatus.Administrator && member.Status != ChatMemberStatus.Creator)
        {
            await botClient.SendMessage(msg.Chat.Id, "У вас недостаточно прав чтобы использовать эту комманду.",
                ParseMode.Html);
            return;
        }

        using (ApplicationContext db = new ApplicationContext())
        {
            var userData = db.Chats
                .Include(u => u.Users)
                .ThenInclude(u => u.Warns)
                .Include(u => u.Users)
                .ThenInclude(u => u.Mutes)
                .Include(u => u.Users)
                .ThenInclude(u => u.Ban)
                .Include(u => u.Users)
                .ThenInclude(u => u.Kick)
                .FirstOrDefault(u => u.ChatId == msg.Chat.Id);
            var currentUser = userData?.Users?
                .FirstOrDefault(u => u.UserId == msg.ReplyToMessage.From?.Id);
            if (currentUser?.Ban is null) currentUser.Ban = new EntityList.Ban();
            if (currentUser?.Kick is null) currentUser.Kick = new EntityList.Kick();
            if (currentUser?.Mutes is null) currentUser.Mutes = new List<EntityList.Mute>();
            if (currentUser?.Warns is null) currentUser.Warns = new List<EntityList.Warn>();
            var banDetails = currentUser?.Ban.Description ?? "Не имеет банов";
            var kickDetails = currentUser?.Kick.Description ?? "Не имеет киков";
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