using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Teleg_training
{
    internal class Program
    {
        static DbContext dbcontext;
        static DBLogic dBLogic;
        static List<ModelList> programlist;
        static List<ProductModel> productlist;
        static string lastprog="";
        private static readonly TimeSpan UpdateInterval = TimeSpan.FromMinutes(1); // Интервал обновления данных
        static void Main(string[] args)
        {
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
            dbcontext = serviceProvider.GetRequiredService<ProgramListContext>();

            dBLogic = new DBLogic((ProgramListContext)dbcontext);
            //var(_programlist, _productlist) = dBLogic.GetLists();
            //programlist = _programlist;
            //productlist = _productlist;
            var timer = new Timer(async _ => await UpdateDataAsync(), null, TimeSpan.Zero, UpdateInterval);
            //productlist = dBLogic.GetProductList();
            botClient.StartReceiving(
                updateHandler: Update,
                pollingErrorHandler: Error,
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
                        return;
                    }
                    else if (update.CallbackQuery?.Data == "male_start")
                    {
                        string progs = "List of programs for begginers";
                        if (programlist.Count > 0)
                        {
                            for (int i = 0; i < programlist.Count; i++)
                            {
                                if (programlist[i].Gender == "male" && programlist[i].Mode == "start")
                                {
                                    int j = i + 1;
                                    progs += $"\n{j}: {programlist[i].Name}. Author: {programlist[i].Author}. Difficult: {programlist[i].Difficult.ToString()}★. Description: {programlist[i].Description}. Program: /{programlist[i].CodeName}";
                                }
                            }
                        }
                        else
                            progs = "No programs yet";
                        Message sentMessage = await client.SendTextMessageAsync(
                        chatId: update.CallbackQuery.From.Id,
                        text: progs,
                        cancellationToken: token);
                        await client.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        return;
                    }
                    else if (update.CallbackQuery?.Data == "male_mid")
                    {
                        string progs = "List of programs for amateur";
                        if (programlist.Count > 0)
                        {
                            for (int i = 0; i < programlist.Count; i++)
                            {
                                if (programlist[i].Gender == "male" && programlist[i].Mode == "mid")
                                {
                                    int j = i + 1;
                                    progs += $"\n{j}: {programlist[i].Name}. Author: {programlist[i].Author}. Difficult: {programlist[i].Difficult.ToString()}★. Description: {programlist[i].Description}. Program: /{programlist[i].CodeName}";
                                }
                            }
                        }
                        else
                            progs = "No programs yet";
                        Message sentMessage = await client.SendTextMessageAsync(
                        chatId: update.CallbackQuery.From.Id,
                        text: progs,
                        cancellationToken: token);
                        await client.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        return;
                    }
                    else if (update.CallbackQuery?.Data == "male_pro")
                    {
                        string progs = "List of programs for pro";
                        if (programlist.Count > 0)
                        {
                            for (int i = 0; i < programlist.Count; i++)
                            {
                                if (programlist[i].Gender == "male" && programlist[i].Mode == "pro")
                                {
                                    int j = i + 1;
                                    progs += $"\n{j}: {programlist[i].Name}. Author: {programlist[i].Author}. Difficult: {programlist[i].Difficult.ToString()}★. Description: {programlist[i].Description}. Program: /{programlist[i].CodeName}";
                                }
                            }    
                        }
                        else
                            progs = "No programs yet";
                        Message sentMessage = await client.SendTextMessageAsync(
                        chatId: update.CallbackQuery.From.Id,
                        text: progs,
                        cancellationToken: token);
                        await client.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        return;
                    }
                    else if (update.CallbackQuery?.Data == "female_start")
                    {
                        string progs = "List of programs for begginers";
                        if (programlist.Count > 0)
                        {
                            for (int i = 0; i < programlist.Count; i++)
                            {
                                if (programlist[i].Gender == "female" && programlist[i].Mode == "start")
                                {
                                    int j = i + 1;
                                    progs += $"\n{j}: {programlist[i].Name}. Author: {programlist[i].Author}. Difficult: {programlist[i].Difficult.ToString()}★. Description: {programlist[i].Description}. Program: /{programlist[i].CodeName}";
                                }
                            }
                        }
                        else
                            progs = "No programs yet";
                        Message sentMessage = await client.SendTextMessageAsync(
                        chatId: update.CallbackQuery.From.Id,
                        text: progs,
                        cancellationToken: token);
                        await client.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        return;
                    }
                    else if (update.CallbackQuery?.Data == "female_mid")
                    {
                        string progs = "List of programs for amateur";
                        if (programlist.Count > 0)
                        {
                            for (int i = 0; i < programlist.Count; i++)
                            {
                                if (programlist[i].Gender == "female" && programlist[i].Mode == "mid")
                                {
                                    int j = i + 1;
                                    progs += $"\n{j}: {programlist[i].Name}. Author: {programlist[i].Author}. Difficult: {programlist[i].Difficult.ToString()}★. Description: {programlist[i].Description}. Program: /{programlist[i].CodeName}";
                                }
                            }
                        }
                        else
                            progs = "No programs yet";
                        Message sentMessage = await client.SendTextMessageAsync(
                        chatId: update.CallbackQuery.From.Id,
                        text: progs,
                        cancellationToken: token);
                        await client.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        return;
                    }
                    else if (update.CallbackQuery?.Data == "female_pro")
                    {
                        string progs = "List of programs for pro";
                        if (programlist.Count > 0)
                        {
                            for (int i = 0; i < programlist.Count; i++)
                            {
                                if (programlist[i].Gender == "female" && programlist[i].Mode == "pro")
                                {
                                    int j = i + 1;
                                    progs += $"\n{j}: {programlist[i].Name}. Author: {programlist[i].Author}. Difficult: {programlist[i].Difficult.ToString()}★. Description: {programlist[i].Description}. Program: /{programlist[i].CodeName}";
                                }
                            }
                        }
                        else
                            progs = "No programs yet";
                        Message sentMessage = await client.SendTextMessageAsync(
                        chatId: update.CallbackQuery.From.Id,
                        text: progs,
                        cancellationToken: token);
                        await client.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        return;
                    }
                    else if (update.CallbackQuery?.Data == "like")
                    {
                        ModelList model = programlist.Find(x => x.CodeName==lastprog);
                        Message sentMessage = await client.SendTextMessageAsync(
                        chatId: update.CallbackQuery.From.Id,
                        text: "like",
                        cancellationToken: token);
                        await client.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        return;
                    }
                }
                return;
            }
            if (message.Text is not { } messageText)
                return;
            if (messageText.ToLower() == "/help"|| messageText.ToLower() == "/start")
            {
                Message sentMessage = await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Help commands",
                replyMarkup: GetButtonsMain(),
                cancellationToken: token);
                return;
            }
            if (messageText.ToLower() == "info")
            {
                Message sentMessage = await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "This bot was created by LordMixa for non-commercial use. This bot was created to help athletes find the necessary training programs and other information. To call the navigation menu: /help",
                replyMarkup: GetButtonsMain(),
                cancellationToken: token);
                return;
            }
            if (messageText.ToLower() == "list of programs")
            {
                Message sentMessage = await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Choose your gender",
                replyMarkup: GetButtonsListProgSex(),
                cancellationToken: token);
                return;
            }
            if (messageText.ToLower() == "top of programs")
            {
                Message sentMessage = await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "This command didn`t ready yet",
                replyMarkup: GetButtonsMain(),
                cancellationToken: token);
                return;
            }
            if (messageText.ToLower() == "products")
            {
                string prod="List of products\n";
                for (int i = 0; i < productlist.Count; i++)
                {
                    prod+= productlist[i].Name+"\nDescription: "+productlist[i].Description+"\n"+$"Calories: {productlist[i].Calories}\tProteins: {productlist[i].Proteins}\tFats: {productlist[i].Fats}\tCarbohydrates: {productlist[i].Carbohydrates}\n\n";
                }
                Message sentMessage = await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: prod,
                replyMarkup: GetButtonsMain(),
                cancellationToken: token);
                return;
            }
            if (messageText.ToLower() == "male")
            {
                Message sentMessage = await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Choose your level",
                replyMarkup: GetButtonsListProgMaleLevel(),
                cancellationToken: token);
                return;
            }
            if (messageText.ToLower() == "female")
            {
                Message sentMessage = await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Choose your level",
                replyMarkup: GetButtonsListProgFemaleLevel(),
                cancellationToken: token);

                return;
            }
            if (messageText.ToLower() == "start m")
            {
                string progs = "List of programs";
                for (int i = 0; i < programlist.Count; i++)
                {
                    int j = i + 1;
                    progs += $"\n{j}: {programlist[i].Name}. Author: {programlist[i].Author}. Difficult: {programlist[i].Difficult.ToString()}★. Description: {programlist[i].Description}. Program: /{programlist[i].CodeName}";
                }
                Message sentMessage = await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: progs,
                cancellationToken: token);
                return;
            }
            else
            {
                string info = "Uknown command";
                for (int i = 0; i < programlist.Count; i++)
                {
                    if (message.Text == $"/{programlist[i].CodeName}")
                    {
                        info = programlist[i].Program;
                        lastprog = programlist[i].CodeName;
                    }
                }
                Message sentMessage = await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: info,
                replyMarkup: GetButtonLike(),
                cancellationToken: token);
                return;
            }
        }

        private static Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        private static IReplyMarkup GetButtonsMain()
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
                new KeyboardButton[] { "List of programs", "Top of programs" },
                new KeyboardButton[] { "Info", "Products" },
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
                    InlineKeyboardButton.WithCallbackData(text: "Like", callbackData: "like")
                },
            });
            return inlineKeyboard;
        }
        public static async Task UpdateDataAsync()
        {
            try
            {
                Console.WriteLine($"Start Data Updating: {DateTime.Now}");

                var (_programlist, _productlist) = dBLogic.GetLists();
                programlist = _programlist;
                productlist = _productlist;

                Console.WriteLine($"Updating Data Ended: {DateTime.Now}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while data-updating: {ex.Message}");
            }
        }
    };
}

