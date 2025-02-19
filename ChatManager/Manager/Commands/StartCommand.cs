using ChatManager.Database;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ChatManager.Manager.Commands;

public static class StartCommand
{
    public static async Task StartCmd(ITelegramBotClient botClient, Message msg)
    {
        await DbMethods.InitializeUserAsync(msg);
        
        var welcomeMessage = 
            $"<b>👋 Добро пожаловать, {msg.From.FirstName}!</b>\n\n" +
            
            "<b>🤖 Chat Manager Bot</b>\n" +
            "<blockquote>Ваш умный помощник в управлении чатом и отслеживании активности пользователей!</blockquote>\n\n" +
            
            "<b>⚡️ Основные возможности:</b>\n" +
            "<blockquote>• 📊 Система уровней и опыта\n" +
            "• 📈 Отслеживание статистики сообщений\n" +
            "• 🛡 Модерация чата\n" +
            "• ⚠️ Система предупреждений\n" +
            "• 🏆 Топы активности\n" +
            "• 🔍 Анализ сообщений</blockquote>\n\n" +
            
            "<b>📝 Доступные команды:</b>\n" +
            "<blockquote>• /help - Полное руководство\n" +
            "• /profile - Просмотр профиля\n" +
            "• /top - Рейтинг участников\n" +
            "• /id - Узнать свой ID</blockquote>\n\n" +
            
            "<b>📊 Система начисления опыта:</b>\n" +
            "<blockquote>• Текст: 5 очков\n" +
            "• Голосовые: 10 очков\n" +
            "• Видео: 20 очков\n" +
            "• Стикеры: 25 очков\n" +
            "• Фото: 30 очков</blockquote>\n\n" +
            
            "<i>💡 Используйте /help для получения подробной информации о всех возможностях бота.</i>";

        var keyboard = new InlineKeyboardMarkup()
            .AddButton("Топ", "TopByLevel")
            .AddButton("Узнать свой TG ID", "IdCall");
            
        await botClient.SendMessage(msg.Chat.Id, welcomeMessage, ParseMode.Html, replyMarkup: keyboard);
    }
}