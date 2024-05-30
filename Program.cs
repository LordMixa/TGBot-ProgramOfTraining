using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using Teleg_training.Models;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Serilog;

namespace Teleg_training
{
    internal class Program
    {
        static DBLogic? dBLogic;
        //static List<ModelList>? programlist;
        //static List<ProductModel>? productlist;
        //static string lastprog = "";
        //static bool _firstrun;
        //private static readonly TimeSpan UpdateInterval = TimeSpan.FromMinutes(1); // Интервал обновления данных
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("C:\\Users\\mishy\\source\\repos\\Teleg_training\\Teleg_training\\logs\\myapp.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            Log.Information($"Starting up ");
            var botClient = new TelegramBotClient("6039744763:AAH2Z5I1jwSzkTXUTD2NglT38NSz06s7Tp8");

            using CancellationTokenSource cts = new();

            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };
            var serviceProvider = new ServiceCollection()
            .AddDbContext<ProgramListContext>()
            .AddAutoMapper(typeof(UserMappingProfile))
            .BuildServiceProvider();
            //_firstrun = true;
            dBLogic = new DBLogic();
            //if (_firstrun)
            //{
            //    UpdateData();
            //    _firstrun = false;
            //}
            //var firstUpdateDelay = TimeSpan.FromMinutes(1);

            //var timer = new Timer(async _ => await UpdateDataAsync(), null, firstUpdateDelay, UpdateInterval);
            botClient.StartReceiving(
                updateHandler: Update,
                pollingErrorHandler: HandleErrorAsync/*Error*/,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );
            Console.ReadLine();

            cts.Cancel();
        }

        async static Task Update(ITelegramBotClient client, Update update, CancellationToken token)
        {
            if (update.Message is not { } message)
            {
                if (update.CallbackQuery?.Data != null)
                {
                    if (update.CallbackQuery?.Data == "male_menu")
                    {
                        Message sentMessage = await client.SendTextMessageAsync(
                        chatId: update.CallbackQuery.From.Id,
                        text: "Choose your level",
                        replyMarkup: GetButtonsListProgMaleLevel(),
                        cancellationToken: token);
                        //await client.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        await client.EditMessageReplyMarkupAsync(
                        chatId: update.CallbackQuery.Message.Chat.Id,
                        messageId: update.CallbackQuery.Message.MessageId,
                        replyMarkup: null
                        );
                        Log.Information($"User {update.CallbackQuery.From.FirstName} male_menu");
                        await client.DeleteMessageAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId);
                        return;
                    }
                    else if (update.CallbackQuery?.Data == "female_menu")
                    {
                        Message sentMessage = await client.SendTextMessageAsync(
                        chatId: update.CallbackQuery.From.Id,
                        text: "Choose your level",
                        replyMarkup: GetButtonsListProgFemaleLevel(),
                        cancellationToken: token);
                        //await client.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        await client.EditMessageReplyMarkupAsync(
                        chatId: update.CallbackQuery.Message.Chat.Id,
                        messageId: update.CallbackQuery.Message.MessageId,
                        replyMarkup: null
                        );
                        Log.Information($"User {update.CallbackQuery.From.FirstName} female_menu");
                        await client.DeleteMessageAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId);

                        return;
                    }
                    else if (update.CallbackQuery?.Data == "back_to_main")
                    {
                        Message sentMessage = await client.SendTextMessageAsync(
                        chatId: update.CallbackQuery.From.Id,
                        text: "Menu",
                        replyMarkup: GetButtonsMain(),
                        cancellationToken: token);
                        //await client.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        await client.EditMessageReplyMarkupAsync(
                        chatId: update.CallbackQuery.Message.Chat.Id,
                        messageId: update.CallbackQuery.Message.MessageId,
                        replyMarkup: null
                        );
                        Log.Information($"User {update.CallbackQuery.From.FirstName} back_to_main");

                        await client.DeleteMessageAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId);

                        return;
                    }
                    else if (update.CallbackQuery?.Data == "back_to_gender")
                    {
                        Message sentMessage = await client.SendTextMessageAsync(
                        chatId: update.CallbackQuery.From.Id,
                        text: "Choose your gender",
                        replyMarkup: GetButtonsListProgSex(),
                        cancellationToken: token);
                        //await client.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        await client.EditMessageReplyMarkupAsync(
                        chatId: update.CallbackQuery.Message.Chat.Id,
                        messageId: update.CallbackQuery.Message.MessageId,
                        replyMarkup: null
                        );
                        Log.Information($"User {update.CallbackQuery.From.FirstName} back_to_gender");

                        await client.DeleteMessageAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId);

                        return;
                    }
                    else if (update.CallbackQuery?.Data == "male_start")
                    {
                        string progs = dBLogic.GetStringListOfPrograms("male", "start");
                        Message sentMessage = await client.SendTextMessageAsync(
                        chatId: update.CallbackQuery.From.Id,
                        text: progs,
                        cancellationToken: token);
                        //await client.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        await client.DeleteMessageAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId);
                        Log.Information($"User {update.CallbackQuery.From.FirstName} male_start");

                        return;
                    }
                    else if (update.CallbackQuery?.Data == "male_mid")
                    {
                        string progs = dBLogic.GetStringListOfPrograms("male", "mid");
                        Message sentMessage = await client.SendTextMessageAsync(
                        chatId: update.CallbackQuery.From.Id,
                        text: progs,
                        cancellationToken: token);
                        //await client.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        await client.DeleteMessageAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId);
                        Log.Information($"User {update.CallbackQuery.From.FirstName} male_mid");

                        return;
                    }
                    else if (update.CallbackQuery?.Data == "male_pro")
                    {
                        string progs = dBLogic.GetStringListOfPrograms("male", "pro");
                        Message sentMessage = await client.SendTextMessageAsync(
                        chatId: update.CallbackQuery.From.Id,
                        text: progs,
                        cancellationToken: token);
                        //await client.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        await client.DeleteMessageAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId);
                        Log.Information($"User {update.CallbackQuery.From.FirstName} male_pro");

                        return;
                    }
                    else if (update.CallbackQuery?.Data == "female_start")
                    {
                        string progs = dBLogic.GetStringListOfPrograms("female", "start");
                        Message sentMessage = await client.SendTextMessageAsync(
                        chatId: update.CallbackQuery.From.Id,
                        text: progs,
                        cancellationToken: token);
                        //await client.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        await client.DeleteMessageAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId);
                        Log.Information($"User {update.CallbackQuery.From.FirstName} female_start");

                        return;
                    }
                    else if (update.CallbackQuery?.Data == "female_mid")
                    {
                        string progs = dBLogic.GetStringListOfPrograms("female", "mid");
                        Message sentMessage = await client.SendTextMessageAsync(
                        chatId: update.CallbackQuery.From.Id,
                        text: progs,
                        cancellationToken: token);
                        //await client.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        await client.DeleteMessageAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId);
                        Log.Information($"User {update.CallbackQuery.From.FirstName} female_mid");

                        return;
                    }
                    else if (update.CallbackQuery?.Data == "female_pro")
                    {
                        string progs = dBLogic.GetStringListOfPrograms("female", "pro");
                        Message sentMessage = await client.SendTextMessageAsync(
                        chatId: update.CallbackQuery.From.Id,
                        text: progs,
                        cancellationToken: token);
                        //await client.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        await client.DeleteMessageAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId);
                        Log.Information($"User {update.CallbackQuery.From.FirstName} female_pro");

                        return;
                    }
                    else if (update.CallbackQuery?.Data == "like")
                    {
                        string[] lines = update.CallbackQuery.Message.Text.Split('\n');
                        string firstLine = lines[0];
                        ModelList model = dBLogic.GetProgram('\\' + firstLine);
                        string infolike = await dBLogic.LikeProgram(model, update.CallbackQuery.From.Id);
                        if(infolike=="like")
                            Log.Information($"User {update.CallbackQuery.From.FirstName} like {model.CodeName}");
                        else
                            Log.Information($"User {update.CallbackQuery.From.FirstName} unlike {model.CodeName}");
                        //await UpdateDataAsync();
                        //Message sentMessage = await client.SendTextMessageAsync(
                        //chatId: update.CallbackQuery.From.Id,
                        //text: "♡",
                        //cancellationToken: token);
                        //await client.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        await client.DeleteMessageAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId);

                        return;
                    }
                }
                return;
            }
            if (message.Text is not { } messageText)
                return;
            if (messageText.ToLower() == "/help" || messageText.ToLower() == "/start")
            {
                if (!dBLogic.GetInfoUserExist(message.From.Id))
                {
                    dBLogic.AddUser(message.From.Id, message.From.Username);
                    Log.Information($"New User {update.Message.Chat.Username} {message.From.Id} added on DB");
                }
                Message sentMessage = await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Help commands",
                replyMarkup: GetButtonsMain(),
                cancellationToken: token);
                Log.Information($"User {update.Message.Chat.Username} help");

                return;
            }
            if (messageText.ToLower() == "info")
            {
                Message sentMessage = await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "This bot was created by LordMixa for non-commercial use. This bot was created to help athletes find the necessary training programs and other information. To call the navigation menu: /help",
                replyMarkup: GetButtonsMain(),
                cancellationToken: token);
                Log.Information($"User {message.From.FirstName} info");

                return;
            }
            if (messageText.ToLower() == "list of programs")
            {
                Message sentMessage = await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Choose your gender",
                replyMarkup: GetButtonsListProgSex(),
                cancellationToken: token);
                Log.Information($"User {message.From.FirstName} list of progs");
                return;
            }
            if (messageText.ToLower() == "top of programs")
            {
                string progs = dBLogic.GetTop();
                Message sentMessage = await client.SendTextMessageAsync(
                        message.Chat.Id,
                        text: progs,
                        replyMarkup: GetButtonsMain(),
                        cancellationToken: token);
                Log.Information($"User {message.From.FirstName} top of programs");

                return;
            }
            if (messageText.ToLower() == "products")
            {
                string prod = dBLogic.GetProducts();
                Message sentMessage = await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: prod,
                replyMarkup: GetButtonsMain(),
                cancellationToken: token);
                Log.Information($"User {message.From.FirstName} products");

                return;
            }
            if (messageText.ToLower() == "liked programs")
            {
                string progs = dBLogic.GetLikedLists(update.Message.From.Id);

                Message sentMessage = await client.SendTextMessageAsync(
                        message.Chat.Id,
                        text: progs,
                        replyMarkup: GetButtonsMain(),
                        cancellationToken: token);
                Log.Information($"User {message.From.FirstName} liked programs");

                return;
            }
            //if (messageText.ToLower() == "male")
            //{
            //    Message sentMessage = await client.SendTextMessageAsync(
            //    chatId: message.Chat.Id,
            //    text: "Choose your level",
            //    replyMarkup: GetButtonsListProgMaleLevel(),
            //    cancellationToken: token);
            //    Log.Information($"User {message.From.FirstName} male");

            //    return;
            //}
            //if (messageText.ToLower() == "female")
            //{
            //    Message sentMessage = await client.SendTextMessageAsync(
            //    chatId: message.Chat.Id,
            //    text: "Choose your level",
            //    replyMarkup: GetButtonsListProgFemaleLevel(),
            //    cancellationToken: token);

            //    return;
            //}
            else
            {
                ModelList model = dBLogic.GetProgram(messageText);
                if (model != null)
                {
                    if (dBLogic.GetInfoLikeProgram(model, message.From.Id) == "like")
                    {
                        Message sentMessage1 = await client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: model.CodeName + '\n' + model.Program,
                        replyMarkup: GetButtonLike(),
                        cancellationToken: token);
                        Log.Information($"User {message.From.FirstName} get program {model.CodeName}");

                        return;
                    }
                    else
                    {
                        Message sentMessage2 = await client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: model.CodeName + '\n' + model.Program,
                        replyMarkup: GetButtonUnLike(),
                        cancellationToken: token);
                        Log.Information($"User {message.From.FirstName} get program {model.CodeName}");

                        return;
                    }
                }
                else
                {
                    Message sentMessage3 = await client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Unknown command",
                    replyMarkup: GetButtonsMain(),
                    cancellationToken: token);
                    Log.Information($"User {message.From.FirstName} unknown command");

                    return;
                }
            }
        }
        private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Log.Error(errorMessage);

            if (exception is ApiRequestException apiException)
            {
                // Обрабатываем только определенные типы ошибок
                switch (apiException.ErrorCode)
                {
                    case 400:
                        // Логируем и игнорируем ошибку
                        Log.Warning("Ignoring 400 error");
                        break;
                    default:
                        // Логируем и продолжаем работу
                        Log.Warning("Unhandled API error");
                        break;
                }
            }

            return Task.CompletedTask;
        }
        //private static Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
        //{
        //    var ErrorMessage = exception switch
        //    {
        //        ApiRequestException apiRequestException
        //            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        //        _ => exception.ToString()
        //    };

        //    Console.WriteLine(ErrorMessage);
        //    return Task.CompletedTask;
        //}

        private static IReplyMarkup GetButtonsMain()
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
                new KeyboardButton[] { "List of programs", "Top of programs" },
                new KeyboardButton[] { "Info", "Products", "Liked Programs"},
            })
            {
                ResizeKeyboard = true
            };
            replyKeyboardMarkup.OneTimeKeyboard = true;
            return replyKeyboardMarkup;
        }
        private static IReplyMarkup GetButtonsListProgSex()
        {
            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Male", callbackData: "male_menu")
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Female", callbackData: "female_menu"),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Back", callbackData: "back_to_main"),
                },
            });
            return inlineKeyboard;
        }
        private static IReplyMarkup GetButtonsListProgMaleLevel()
        {
            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Start", callbackData: "male_start")
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Middle", callbackData: "male_mid"),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Pro", callbackData: "male_pro"),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Back", callbackData: "back_to_gender"),
                },
            });
            return inlineKeyboard;
        }
        private static IReplyMarkup GetButtonsListProgFemaleLevel()
        {
            InlineKeyboardMarkup inlineKeyboard = new(new[]
             {
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Start", callbackData: "female_start")
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Middle", callbackData: "female_mid"),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Pro", callbackData: "female_pro"),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Back", callbackData: "back_to_gender"),
                },
            });
            return inlineKeyboard;
        }
        private static IReplyMarkup GetButtonLike()
        {
            InlineKeyboardMarkup inlineKeyboard = new(new[]
             {
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Like♡", callbackData: "like")
                },
            });
            return inlineKeyboard;
        }
        private static IReplyMarkup GetButtonUnLike()
        {
            InlineKeyboardMarkup inlineKeyboard = new(new[]
             {
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Like♥", callbackData: "like")
                },
            });
            return inlineKeyboard;
        }
        //public static async Task UpdateDataAsync()
        //{
        //    try
        //    {
        //        Console.WriteLine($"Start Data Updating: {DateTime.Now}");

        //        programlist = await dBLogic.GetProgsAsync();
        //        productlist = await dBLogic.GetProdsAsync();

        //        Console.WriteLine($"Updating Data Ended: {DateTime.Now}");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error while data-updating: {ex.Message}");
        //    }
        //}
        //public static void UpdateData()
        //{
        //    try
        //    {
        //        Console.WriteLine($"Start Data Updating: {DateTime.Now}");

        //        var (_programlist, _productlist) = dBLogic.GetLists();
        //        programlist = _programlist;
        //        productlist = _productlist;

        //        Console.WriteLine($"Updating Data Ended: {DateTime.Now}");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error while data-updating: {ex.Message}");
        //    }
        //}
        //public static string ProgramString(string gender, string mode, string progs)
        //{
        //    if (programlist != null && programlist.Count > 0)
        //    {
        //        int count = 0;
        //        for (int i = 0; i < programlist.Count; i++)
        //        {
        //            if (programlist[i].Gender == gender && programlist[i].Mode == mode)
        //            {
        //                count++;
        //                int j = i + 1;
        //                progs += $"\n{j}: {programlist[i].Name}. Author: {programlist[i].Author}. Difficult: {programlist[i].Difficult.ToString()}★. Description: {programlist[i].Description}. Count of likes: {programlist[i].Likes}❤. Program: /{programlist[i].CodeName}";
        //            }
        //        }
        //        if (count == 0)
        //            progs = "No programs yet";
        //        return progs;
        //    }else
        //        progs = "No programs yet";
        //    return progs;
        //}
    }
}

