using ChatManager.Manager.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatManager.Services;

public class OnCallbackQueryService
{
    private static TelegramBotClient _bot = null!;
    public OnCallbackQueryService(TelegramBotClient botClient)
    {
        _bot = botClient;
    }
    public async Task OnCallbackQuery(Update update)
    {
        if (update.Type != UpdateType.CallbackQuery || update.CallbackQuery?.Message == null) return;
        await _bot.AnswerCallbackQuery(update.CallbackQuery.Id);
        switch (update.CallbackQuery?.Data)
        {
            case "IdCall":
                await _bot.SendMessage(update.CallbackQuery.Message.Chat.Id, $"ID пользователя {update.CallbackQuery.From.FirstName}: {update.CallbackQuery.From.Id}", ParseMode.Html);
                break;
            case "TopByLevel":
                await TopCommand.TopCmd(_bot, update.CallbackQuery.Message ?? new Message(), 1);
                break;
            case "TopByMessages":
                await TopCommand.TopCmd(_bot, update.CallbackQuery.Message ?? new Message(), 2);
                break;
            case "TopByTextMessages":
                await TopCommand.TopCmd(_bot, update.CallbackQuery.Message ?? new Message(), 3);
                break;
            case "TopByAudioMessages":
                await TopCommand.TopCmd(_bot, update.CallbackQuery.Message ?? new Message(), 4);
                break;
            case "TopByVideoMessages":
                await TopCommand.TopCmd(_bot, update.CallbackQuery.Message ?? new Message(), 5);
                break;
            case "TopBySticker":
                await TopCommand.TopCmd(_bot, update.CallbackQuery.Message ?? new Message(), 6);
                break;
            case "TopByPhoto":
                await TopCommand.TopCmd(_bot, update.CallbackQuery.Message ?? new Message(), 7);
                break;
            case "TopByLocation":
                await TopCommand.TopCmd(_bot, update.CallbackQuery.Message ?? new Message(), 8);
                break;
            case "TopByOther":
                await TopCommand.TopCmd(_bot, update.CallbackQuery.Message ?? new Message(), 9);
                break;
            case "TopByVoice":
                await TopCommand.TopCmd(_bot, update.CallbackQuery.Message ?? new Message(), 10);
                break;
            case "TopByCircle":
                await TopCommand.TopCmd(_bot, update.CallbackQuery.Message ?? new Message(), 11);
                break;
            case "TopByGif":
                await TopCommand.TopCmd(_bot, update.CallbackQuery.Message ?? new Message(), 12);
                break;
        }
    }
}