using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using DiscordBot;
namespace DiscordBot.Modules
{

    public class Commands : ModuleBase<SocketCommandContext>
    {
        private static IUser currentUser;
        private DiscordSocketClient _client;

        [Command("ping")] //Done
        public async Task Ping()
        {
            await ReplyAsync("pong");
        }

        [Command("whoami")] //Done
        public async Task WhoAmI()
        {
            _client = new DiscordSocketClient();

            var id = Context.User.Mention;

            if (currentUser == null)
            {
                foreach (var user in Context.Guild.Users)
                {
                    if (("<@!" + user.Id.ToString() + ">") == id)
                    {
                        currentUser = user;
                        id = user.Mention;
                        break;
                    }
                }
            }


            await ReplyAsync($"You are {currentUser.Username}");
        }

        [Command("is")] //Done
        public async Task IsHeDumb(IGuildUser user, string what)
        {
            var nickname = user.ToString();
            await ReplyAsync($"Yes, **{nickname.Substring(0, nickname.Length - 5)}** is very {what}");
        }

        [Command("shrug")] //Done
        public async Task Shrug()
        {
            await ReplyAsync(@"¯\_(ツ)_/¯");
        }

        [Command("created")] //Done
        public async Task Created()
        {
            _client = new DiscordSocketClient();

            var id = Context.User.Mention;

            if (currentUser == null)
            {
                foreach (var user in Context.Guild.Users)
                {
                    if (("<@!" + user.Id.ToString() + ">") == id)
                    {
                        currentUser = user;
                        id = user.Mention;
                        break;
                    }
                }
            }

            var createdOn = currentUser.CreatedAt.ToString();
            await ReplyAsync($"Your Account Was Created On {createdOn.Substring(0, createdOn.Length - 7)}");
        }

        [Command("roles")] //Done
        public async Task UserRoles(SocketGuildUser userName)
        {
            var user = Context.User as SocketGuildUser;
            var roles = userName.Roles.ToList();

            await ReplyAsync($"{userName} has the following roles : _{string.Join(", ", roles)}_");
        }

        [Command("serverRoles")] //Done
        public async Task ServerRoles()
        {
            var user = Context.User as SocketGuildUser;
            var roles = user.Guild.Roles.ToList();

            await ReplyAsync($"The server has the following roles: _{string.Join(",", roles)}_");
        }

        [Command("help")]  //Done
        public async Task Help()
        {
            StringBuilder sb = new StringBuilder();
            sb
                .AppendLine(@"Commands For _**TheFunGuy**_")
                .AppendLine(@"**#ping** -> pong")
                .AppendLine(@"**#whoami** -> get user's nickname")
                .AppendLine(@"**#shrug** -> sends shrug in ascii")
                .AppendLine(@"**#is <@user> <message>** -> answers your question")
                .AppendLine(@"**#created** -> shows account creation date and time")
                .AppendLine(@"**#roles <@user>** -> get user's roles")
                .AppendLine(@"**#serverRoles** -> gets the roles in the server")
                .AppendLine(@"**#serverInfo** -> gets some info about the server");


            await ReplyAsync(sb.ToString().TrimEnd());
        }

        [Command("serverInfo", RunMode = RunMode.Async)] //Done
        public async Task ServerInfo()
        {;
            StringBuilder sb = new StringBuilder();

            var user = Context.User as SocketGuildUser;
            var server = user.Guild;

            sb.AppendLine($"---=== **{server.Name}** ===---")
                .AppendLine($":calendar_spiral: **Created On :** {server.CreatedAt.ToString("d")}")
                .AppendLine($":bust_in_silhouette:  **Users :** {server.Users.Count(u => u.IsBot == false)}");
            foreach (var person in server.Users.Where(u => u.IsBot == false))
            {
                sb.AppendLine($"        :detective: {person.Username}");
            }


            sb.AppendLine($":beginner: **Channels :** {server.Channels.Count}");
            foreach (var testChannel in server.TextChannels.Take(5))
            {
                sb.AppendLine($"        :writing_hand: {testChannel.Name}");
            }
            sb.AppendLine($"        **( . . . )**");

            foreach (var voiceChannel in server.VoiceChannels.Take(5))
            {
                sb.AppendLine($"        :loud_sound: {voiceChannel.Name}");
            }
            sb.AppendLine($"        **( . . . )**");

            sb.AppendLine($":robot: **Bots :** {server.Users.Count(u => u.IsBot == true)}");
            foreach (var bot in server.Users.Where(u => u.IsBot == true))
            {
                sb.AppendLine($"        :eye_in_speech_bubble: {bot.Username}");
            }

            sb.AppendLine($":wrench: **Roles :** {server.Roles.Count(x => x.IsEveryone == false)}");
            foreach (var role in server.Roles.Where(x=>x.IsEveryone ==false))
            {
                sb.AppendLine($"        :pushpin: {role.Name}");
            }

            string result = sb.ToString().Trim();
            
            await ReplyAsync(result);

        }
    }
}
