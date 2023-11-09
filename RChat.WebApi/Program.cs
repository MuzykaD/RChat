
using Microsoft.AspNetCore.Identity;
using RChat.WebApi.Extensions;
using Microsoft.EntityFrameworkCore;
using RChat.Domain.Users;
using RChat.Infrastructure.Context;
using RChat.Application.Contracts.Authentication.JWT;
using RChat.Application.Authentication.JWT;
using RChat.Application.Contracts.Authentication;
using RChat.Application.Authentication;
using RChat.Application.Contracts.Account;
using RChat.Application.Account;
using RChat.Application.Contracts.Users;
using RChat.Application.Users;
using RChat.Application.Contracts.Common;
using RChat.Application.Common;
using RChat.Infrastructure.Contracts.UnitOfWork;
using RChat.Infrastructure.UnitOfWork;
using RChat.Application.Contracts.Chats;
using RChat.Application.Chats;
using RChat.Domain.Chats;
using RChat.Domain.Messages;
using RChat.Application.Contracts.Messages;
using RChat.Application.Messages;

namespace RChat.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<RChatDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("RChatDbConnection"));
            });
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("https://localhost:7206")
                    .AllowAnyHeader()
                    .AllowAnyMethod();

                });
            });

            builder.Services.AddIdentity<User, IdentityRole<int>>()
                .AddEntityFrameworkStores<RChatDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddJwtAuthentication(builder.Configuration.GetJwtSettings());        
            builder.Services.AddAuthorization();

            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            

            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IChatService, ChatService>();
            builder.Services.AddScoped<IMessageService, MessageService>();
           
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
           
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}