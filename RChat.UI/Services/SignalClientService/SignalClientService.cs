﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using Radzen;
using RChat.Domain.Common;
using RChat.Domain.Messages;
using RChat.Domain.Messages.Dto;
using RChat.UI.Services.AccountService;
using RChat.UI.ViewModels.InformationViewModels;
using System.Threading.Channels;

namespace RChat.UI.Services.SignalClientService
{
    //Dispose
    public class SignalClientService : ISignalClientService
    {
        public event Action<MessageInformationDto> OnMessageReceived;
        public event Action<MessageInformationDto> OnMessageDelete;
        public event Action<MessageInformationDto> OnMessageUpdate;

        private HubConnection? _hubConnection;
        private NotificationService _notificationService;
        private NavigationManager _navigationManager;
        private ILocalStorageService _localStorageService;
        private IAccountService _accountService;
        private IJSObjectReference _jsModule;
        private IJSRuntime _jsRuntime;
        private readonly string _hubHostUrl;
        public SignalClientService(
            NavigationManager navigationManager,
            NotificationService notificationService, 
            IConfiguration config,
            ILocalStorageService localStorageService,
            IAccountService accountService,
             IJSRuntime jsRuntime)
        {
            _navigationManager = navigationManager;
            _notificationService = notificationService;
            _localStorageService = localStorageService;
            _accountService = accountService;
            _hubHostUrl = config["ApiHost"]!;
            _jsRuntime = jsRuntime;
            
        }
        private bool IsConnected => _hubConnection.State == HubConnectionState.Connected;
        public async Task CallSendMessageAsync(MessageInformationDto messageDto, NotificationArguments notificationArguments)
        {
            if (_hubConnection == null || !IsConnected)
                return;
            await _hubConnection.SendAsync("SendMessageAsync", messageDto, notificationArguments);
        }

        public async Task StartAsync()
        {
            _hubConnection = new HubConnectionBuilder().WithUrl($"{_hubHostUrl}/rChatHub",
                o => o.AccessTokenProvider = 
                async () => await _localStorageService.GetItemAsync<string>("auth-jwt-token"))
                .Build();
            _jsModule = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/web-rtc-chat.js");
            _hubConnection.On<MessageInformationDto>("ReceiveMessage", message => OnMessageReceived?.Invoke(message));
            _hubConnection.On<MessageInformationDto>("OnMessageDelete", message => OnMessageDelete?.Invoke(message));
            _hubConnection.On<MessageInformationDto>("OnMessageUpdate", message => OnMessageUpdate?.Invoke(message));

            _hubConnection.On<NotificationArguments>("ReceiveNotification", (args) =>
            {
                var navigationLink = args.IsGroup ? $"group?groupId={args.ReferenceId}" : $"private?userId={args.ReferenceId}";
                _notificationService.Notify(new()
                {
                    Summary = $"New message in {args.ChatName}",
                    Severity = NotificationSeverity.Info,
                    Duration = 2000,
                    Click = (notification) => _navigationManager.NavigateTo($"/chats/{navigationLink}")
                });
            });

            _hubConnection.On<string, string, string>("SignalWebRtc", async (signalingChannel, type, payload) =>
            {
                if (_jsModule == null) throw new InvalidOperationException();

               // if (_channel != signalingChannel) return;
                switch (type)
                {
                    case "offer":
                        await _jsModule.InvokeVoidAsync("processOffer", payload);
                        break;
                    case "answer":
                        await _jsModule.InvokeVoidAsync("processAnswer", payload);
                        break;
                    case "candidate":
                        await _jsModule.InvokeVoidAsync("processCandidate", payload);
                        break;
                }
            });
            await _hubConnection.StartAsync();
        }

        public async Task JoinChatGroupAsync(int chatId)
        {           
            if (_hubConnection == null || !IsConnected)
                return;
            await _hubConnection.SendAsync("EnterChatGroupAsync", chatId);
        }

        public async Task LeaveChatGroupAsync(int chatId)
        {
            if (_hubConnection == null || !IsConnected)
                return;
            await _hubConnection.SendAsync("LeaveChatGroupAsync", chatId);
        }

        public async Task StopAsync()
        {
            if (_hubConnection != null && IsConnected)
               await _hubConnection.StopAsync();
        }

        public async Task DeleteMessageAsync(MessageInformationDto message)
        {
            await _hubConnection.SendAsync("DeleteMessageAsync", message);
        }

        public async Task RegisterUserSignalGroupsAsync()
        {
            var groupIds = await _accountService.GetUserSignalGroupsAsync();
            await _hubConnection.SendAsync("RegisterMultipleGroupsAsync", groupIds.Result.SignalIdentifiers);
        }

        public async Task CallUpdateMessageAsync(MessageInformationDto messageToUpdate)
        {
            await _hubConnection.SendAsync("UpdateMessageAsync", messageToUpdate);
        }

        public async Task Join(string channel)
        {
            await _hubConnection.SendAsync("join", channel);
        }
        public async Task SendOffer(string channel,string offer)
        {
            await _hubConnection.SendAsync("SignalWebRtc", channel, "offer", offer);
        }

        public async Task Leave(string? channel)
        {
            await _hubConnection.SendAsync("leave", channel);
        }

        public async Task SendAnswer(string? channel, string answer)
        {
            await _hubConnection.SendAsync("SignalWebRtc", channel, "answer", answer);
        }

        public async Task SendCandidate(string? channel, string candidate)
        {
            await _hubConnection.SendAsync("SignalWebRtc", channel, "candidate", candidate);
        }
    }
}
