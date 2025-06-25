using ChatManager.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ChatManager.Manager.Commands.UserTools;

public static class AICommand
{
    public static async Task AiCmd(ITelegramBotClient botClient, Message msg, AiService service)
    {
        if (msg.Text != null)
        {
            await service.MakeTextRequest(msg);
        }
        else if (msg.Photo != null)
        {
            await service.ProcessImageMessage(msg);
        }
    }
}