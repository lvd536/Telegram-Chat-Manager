using Telegram.Bot.Polling;

namespace ChatManager.Services;

public class OnErrorService
{
    private static CancellationTokenSource _cts = null!;
    public OnErrorService(CancellationTokenSource cancellationTokenSource)
    {
        _cts = cancellationTokenSource;
    }
    
    public async Task OnError(Exception exception, HandleErrorSource handler)
    {
        Console.WriteLine(exception.Message + "\n" + exception.StackTrace);
        await Task.Delay(2000, _cts.Token);
    }
}