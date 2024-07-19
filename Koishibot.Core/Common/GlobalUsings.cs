﻿global using FluentValidation;
global using Koishibot.Core.Behaviors;
global using Koishibot.Core.Common;
global using Koishibot.Core.Features.TwitchAuthorization.Interfaces;
global using Koishibot.Core.Persistence.Cache;
global using Koishibot.Core.Persistence.Cache.Interface;
global using Koishibot.Core.Services.SignalR;
global using Koishibot.Core.Services.TwitchEventSub;
global using Koishibot.Core.Services.TwitchEventSub.Interfaces;
global using Koishibot.Core.Services.TwitchIrcClient;
global using Koishibot.Core.Services.TwitchIrcClient.Interfaces;
global using MediatR;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Swashbuckle.AspNetCore.Annotations;
global using TwitchLib.Api.Helix.Models.ChannelPoints.CreateCustomReward;
global using TwitchLib.Api.Interfaces;
global using TwitchLib.EventSub.Websockets;
