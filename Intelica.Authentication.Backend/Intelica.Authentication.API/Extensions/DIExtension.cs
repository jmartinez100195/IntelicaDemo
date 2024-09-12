using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application;
using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.Interfaces;
using Intelica.Authentication.API.Domain.Infrastructure;
using Intelica.Authentication.API.Encriptation;
using Intelica.Infrastructure.Library.Cache;
using Intelica.Infrastructure.Library.Cache.Interface;
using Intelica.Infrastructure.Library.GenericApi;
using Intelica.Infrastructure.Library.GenericApi.Interface;
using Intelica.Infrastructure.Library.Log;
using Intelica.Infrastructure.Library.Log.Interrfaces;
using Intelica.Infrastructure.Library.MessageBroker;
using Intelica.Infrastructure.Library.MessageBroker.Interface;
using Intelica.Infrastructure.Library.WebSocket;
using Intelica.Infrastructure.Library.WebSocket.Interface;
namespace Intelica.Security.API.Extensions
{
    public static class DIExtension
	{
		public static IServiceCollection AddConfigurations(this IServiceCollection services)
		{
            services.AddSingleton<ILog, StandartOuputLog>();
            services.AddSingleton<IGenericCache, InMemoryCache>();
            services.AddTransient<IWebSocket, WebSocketSignalR>();
            services.AddTransient<IMessageBroker, MessageBrokerAWSSQS>();			
			services.AddTransient<IGenericApiProxy, GenericApiProxy>();
            services.AddTransient<IGenericRSA, GenericRSA>();
			services.AddTransient<IAuthenticatorAggregate, AuthenticatorAggregate>();
			services.AddTransient<IAuthenticationRepository, AuthenticationSQLServerRepository>();
            return services;
		}
	}
}