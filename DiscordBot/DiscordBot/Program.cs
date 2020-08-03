using System;
using System.Data;
using System.Reflection;
using System.Security.Authentication.ExtendedProtection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot
{
    class Program
    {
        private DiscordSocketClient client;
        private CommandService commands;
        private IServiceProvider services;

        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        public async Task RunBotAsync() //Start bot
        {
            client =new DiscordSocketClient();

            commands = new CommandService();

            services = new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton(commands)
                .BuildServiceProvider();

            string token = ""; //Here you add your bot's token

            client.Log += _client_Log;

            await RegisterCommandsAsync();
            
            await client.LoginAsync(TokenType.Bot, token);
            
            await client.StartAsync();

            await Task.Delay(-1);
        }

        private Task _client_Log(LogMessage arg)
        {
            Console.WriteLine(arg.Message);
            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            client.MessageReceived += HandleCommandAsync;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
        }

        private async Task HandleCommandAsync(SocketMessage arg) //Here you add what prefix you want to trigger the bot
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(client,message);

            if (message.Author.IsBot) return;

            int argPos = 0;
            if (message.HasStringPrefix("#",ref argPos)) // Current prefix is "#"
            {
                var result = await commands.ExecuteAsync(context, argPos, services);
                if (!result.IsSuccess)
                {
                    Console.WriteLine(result.Error);
                }
            }

        }
    }
}
