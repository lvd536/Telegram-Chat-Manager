using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace ChatManager.Services;

public class OnErrorService
{
    public static readonly long _ownerId = 1016623551;
    private static CancellationTokenSource _cts = null!;
    private static TelegramBotClient _bot = null!;
    public OnErrorService(CancellationTokenSource cancellationTokenSource, TelegramBotClient botClient)
    {
        _cts = cancellationTokenSource;
        _bot = botClient;
    }
    
    public async Task OnError(Exception exception, HandleErrorSource handler)
    {
        var stackTrace = new StackTrace(exception, true);
        var frame = stackTrace.GetFrame(0);
        if (frame != null)
        {
            int lineNumber = frame.GetFileLineNumber();
            string fileName = frame.GetFileName() ?? "unknown file";
            string methodName = frame.GetMethod()?.Name ?? "unknown name";
            string date = $"Date: {DateTime.Now:dd.MM.yyyy HH:mm:ss}";
            string description = $"Exception: {exception.Message}";
            string message = $"Исключение в {methodName}, файл {fileName}, строка {lineNumber}\nОписание:{description}";
            Console.WriteLine(message);
            await _bot.SendMessage(_ownerId, $"[Error Handler - {date}] " + message);
        }
        
        await Task.Delay(2000, _cts.Token);
    }
}