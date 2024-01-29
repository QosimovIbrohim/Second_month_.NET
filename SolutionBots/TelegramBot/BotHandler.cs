using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.ReplyMarkups;
using System.Text;
using System.Runtime.CompilerServices;

namespace TelegramBot
{
    public class BotHandler
    {
        public string BotToken { get; set; }
        public bool isContactShare = false;

        public StringBuilder sb = new StringBuilder();
        public BotHandler(string token)
        {
            BotToken = token;
        }

        public async Task BotHandle()
        {
            var botClient = new TelegramBotClient(BotToken);

            using CancellationTokenSource cts = new();

            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            cts.Cancel();
        }

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Contact? contact = null;
            if (update.Message is not { } message)
                return;
            if (message.Contact != null)
            {
                contact = message.Contact;
            }
            var chatId = message.Chat.Id;
            CRUD.Create(new User()
            {
                chatID = chatId,
                status = 0,
                phoneNumber = ""
            }) ;

            if (message.Text == "/start")
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                    {
                    KeyboardButton.WithRequestContact("Contact")
                })
                {
                    ResizeKeyboard = true
                };
                await botClient.SendTextMessageAsync(
                      chatId: chatId,
                      text: "Assalomu aleykum, Botimizga xush kelibsiz bu bot orqali siz nimadir qila olishingiz mumkin\nBotdan to'liq foydalanish uchun telefon raqamingizni jo'nating",
                      replyMarkup: replyKeyboardMarkup,
                      cancellationToken: cancellationToken);
                return;

            }
            if (contact != null)
            {
                CRUD.Update(chatId, contact.PhoneNumber);

                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "/go - buyrug'ini kiriting",
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: cancellationToken);
                return;
            }
            else if (CRUD.IsPhoneNumberNull(chatId) == false)
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Iltimos Telefon raqamingizni yuboring",
                    cancellationToken: cancellationToken);
                return;
            }
            else if(message.Text == "🛍Buyurtma berish")
            {
                CRUD.ChangeStatusCode(chatId, 50);
            }
            switch (CRUD.GetStatusCode(chatId))
            {
                case 0:
                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                    {
                        new[]
                         {
                            new KeyboardButton("🛍Buyurtma berish"),

                        },
                        new[]
                        {
                            new KeyboardButton("✍️Fikr bildirish"),
                            new KeyboardButton("☎️Biz bilan aloqa")
                        },
                        new[]
                         {
                            new KeyboardButton("ℹ️Ma'lumot"),
                            new KeyboardButton("⚙️Sozlamalar")
                        }
                    }
                    )
                    {
                        ResizeKeyboard = true
                    };
                    await botClient.SendTextMessageAsync(
                        chatId:chatId,
                        text:"Buyurtma berishingiz mumkin",
                        replyMarkup: replyKeyboardMarkup,
                        cancellationToken: cancellationToken);
                    break;
                case 50:
                    ReplyKeyboardMarkup replyKeyboardMarkupOrder = new(
                        new[]
                         {
                            new KeyboardButton("🚖Yetkazib berish"),
                            new KeyboardButton("🏃Olib ketish"),

                        })
                    {
                        ResizeKeyboard = true
                    };
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: message.Text,
                        replyMarkup: replyKeyboardMarkupOrder,
                        cancellationToken: cancellationToken);
                    break;
                default:
                    break;
            }
        }



        async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
        }
    }
}
