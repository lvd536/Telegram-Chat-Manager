using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ChatManager.Manager.Commands;
public static class HelpCommand
{
    public static async Task HelpCmd(ITelegramBotClient botClient, Message msg)
    {
        var helpMessage = 
            "<b>🤖 Chat Manager Bot V2.5 - Полное руководство</b>\n\n" +
            
            "<b>📋 Основная информация:</b>\n" +
            "<blockquote>Бот предназначен для управления чатом, отслеживания активности пользователей и модерации. Имеет расширенную систему фильтрации и аналитики.</blockquote>\n\n" +
            
            "<b>⚡️ Основные возможности:</b>\n" +
            "<blockquote>• Система уровней и опыта\n" +
            "• Отслеживание статистики сообщений\n" +
            "• Модерация чата\n" +
            "• Система предупреждений\n" +
            "• Топы активности\n" +
            "• Фильтрация сообщений\n" +
            "• Аналитика активности</blockquote>\n\n" +
            
            "<b>📝 Общие команды:</b>\n" +
            "<blockquote>• /help - Показать это сообщение\n" +
            "• /profile - Просмотр вашего профиля\n" +
            "• /top - Рейтинг участников\n" +
            "• /id - Узнать свой Telegram ID\n" +
            "• /add [слово] - Добавить запрещенное слово\n" +
            "• /remove [слово] - Удалить запрещенное слово\n" +
            "• /devblog - Посмотреть список обновлений\n" +
            "• /weather - Узнать текущую погоду в Самаре\n" +
            "• /blocklist - Список запрещенных слов</blockquote>\n\n" +
            
            "<b>🛡 Команды модерации (только для администраторов):</b>\n" +
            "<blockquote>• /mute [минуты] [причина] - Запретить отправку сообщений\n" +
            "• /unmute - Снять ограничение на отправку сообщений\n" +
            "• /ban [минуты] [причина] - Забанить пользователя\n" +
            "• /unban - Разбанить пользователя\n" +
            "• /kick [причина] - Выгнать пользователя\n" +
            "• /warn [причина] - Выдать предупреждение\n" +
            "• /unwarn - Снять предупреждение\n" +
            "• /info - Информация о пользователе</blockquote>\n\n" +
            
            "<b>📊 Система уровней:</b>\n" +
            "<blockquote>• Текстовые сообщения: 5 очков\n" +
            "• Голосовые сообщения: 10 очков\n" +
            "• Видео сообщения: 20 очков\n" +
            "• Стикеры: 25 очков\n" +
            "• Фотографии: 30 очков\n" +
            "• Геолокация: 40 очков\n" +
            "• Другие типы: 15 очков</blockquote>\n\n" +
            
            "<b>⚠️ Система предупреждений:</b>\n" +
            "<blockquote>• 3 предупреждения = бан на 3 дня\n" +
            "• Предупреждения можно снимать командой /unwarn\n" +
            "• История предупреждений хранится в базе данных</blockquote>\n\n" +
            
            "<b>🔒 Система фильтрации:</b>\n" +
            "<blockquote>• Автоматическое удаление запрещенных слов\n" +
            "• Настраиваемый список запрещенных слов\n" +
            "• Уведомления о нарушениях</blockquote>\n\n" +
            
            "<i>💡 Бот автоматически отслеживает все сообщения и начисляет опыт за активность в чате.</i>\n\n" +
            
            "<b>🔗 Разработчик:</b>\n" +
            "<blockquote>По всем вопросам обращаться к @lvdshka</blockquote>";

        var keyboard = new InlineKeyboardMarkup()
            .AddButton("Топ", "TopByLevel")
            .AddButton("Узнать свой TG ID", "IdCall");
            /*.AddNewRow()
            .AddButton("Профиль", "Profile")
            .AddButton("Статистика", "Stats");*/
        
        await botClient.SendMessage(msg.Chat.Id, helpMessage, ParseMode.Html, replyMarkup: keyboard);
    }
}