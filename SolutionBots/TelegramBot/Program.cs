using TelegramBot;

class Project
{
    static async Task Main(string[] args)
    {
        string botToken = "6394130933:AAHWUqGRzjmIy-GvpFI7JCtanD-EIFXUM7E";

        var handle = new BotHandler(botToken);

        try
        {
            await handle.BotHandle();
        }
        catch
        {
            await handle.BotHandle();
        }

    }
}