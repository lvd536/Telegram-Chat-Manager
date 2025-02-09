using ChatManager.Database;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatManager.Manager.Commands;

public class StartCommand
{
    public async Task StartCmd(ITelegramBotClient botClient, Message msg)
    {
        await DbMethods.InitializeUserAsync(msg);
        
        var welcomeMessage = 
            $"<b>👋 Добро пожаловать, {msg.From.FirstName}!</b>\n\n" +
            "<b>🤖 Chat Manager Bot</b> - ваш умный помощник в управлении чатом!\n\n" +
            "<b>Основные возможности:</b>\n" +
            "• 📊 Отслеживание статистики сообщений\n" +
            "• ⭐️ Система уровней и опыта\n" +
            "• 📈 Топы активности участников\n" +
            "• 📱 Подробная статистика по типам сообщений\n\n" +
            "<b>Доступные команды:</b>\n" +
            "• /profile - 👤 Просмотр вашего профиля\n" +
            "• /top - 🏆 Рейтинг участников\n" +
            "• /id - 🆔 Узнать свой Telegram ID\n\n" +
            "<i>💡 Бот автоматически отслеживает все сообщения и начисляет опыт за активность в чате.</i>";

        await botClient.SendMessage(msg.Chat.Id, welcomeMessage, ParseMode.Html);
    }
}