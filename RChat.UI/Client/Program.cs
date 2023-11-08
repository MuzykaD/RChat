using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using RChat.UI;
using RChat.UI.Common.AuthenticationProvider;
using RChat.UI.Common.HttpClientPwa;
using RChat.UI.Common.HttpClientPwa.Interfaces;
using RChat.UI.Common.JwtTokenParser;
using RChat.UI.Common.JwtTokenParser.Interfaces;
using RChat.UI.Services.BlazorAuthService;
using RChat.UI.Services.AccountService;
using RChat.UI.Services.UserService;
using RChat.UI.Extensions;
using RChat.UI.Services.ChatService;
using RChat.UI.Services.MessageService;

namespace RChat.UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            builder.Services.AddBlazorBootstrap();

            builder.Services.ConfigureHttpClient(builder.Configuration);
            builder.Services.AddRadzenComponents();
            builder.Services.AddScoped<NotificationService>();
            builder.Services.AddScoped<IJwtTokenParser, JwtTokenParser>();
            builder.Services.AddScoped<IHttpClientPwa, HttpClientPwa>();
            builder.Services.AddScoped<IBlazorAuthService, BlazorAuthService>();
            builder.Services.AddScoped<IAccountService, AccountService>(); 
            builder.Services.AddScoped<IUserService, UserService>(); 
            builder.Services.AddScoped<IChatService, ChatService>(); 
            builder.Services.AddScoped<IMessageService, MessageService>(); 

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<AuthenticationStateProvider, ChatAuthenticationProvider>();
            builder.Services.AddAuthorizationCore();
            await builder.Build().RunAsync();
        }
    }
}