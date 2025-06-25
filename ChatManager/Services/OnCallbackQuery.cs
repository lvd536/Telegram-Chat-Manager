using ChatManager.Manager.Commands.UserTools;
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
        await _bot.SendChatAction(update.CallbackQuery.Message.Chat.Id, ChatAction.Typing);
        await _bot.AnswerCallbackQuery(update.CallbackQuery.Id);
        switch (update.CallbackQuery?.Data)
        {
            case "IdCall":
                await _bot.SendMessage(update.CallbackQuery.Message.Chat.Id, $"ID пользователя {update.CallbackQuery.From.FirstName}: {update.CallbackQuery.From.Id}", ParseMode.Html);
                break;
            case "TopByLevel":
                await CreateTopHandler(1, update);
                break;
            case "TopByMessages":
                await CreateTopHandler(2, update);
                break;
            case "TopByTextMessages":
                await CreateTopHandler(3, update);
                break;
            case "TopByAudioMessages":
                await CreateTopHandler(4, update);
                break;
            case "TopByVideoMessages":
                await CreateTopHandler(5, update);
                break;
            case "TopBySticker":
                await CreateTopHandler(6, update);
                break;
            case "TopByPhoto":
                await CreateTopHandler(7, update);
                break;
            case "TopByLocation":
                await CreateTopHandler(8, update);
                break;
            case "TopByOther":
                await CreateTopHandler(9, update);
                break;
            case "TopByVoice":
                await CreateTopHandler(10, update);
                break;
            case "TopByCircle":
                await CreateTopHandler(11, update);
                break;
            case "TopByGif":
                await CreateTopHandler(12, update);
                break;
        }
    }

    private async Task CreateTopHandler(short id, Update update)
    {
        await TopCommand.TopCmd(_bot, update.CallbackQuery?.Message ?? new Message(), id);
    }
}