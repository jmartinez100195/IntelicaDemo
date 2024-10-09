using Intelica.Authentication.API.Common.DTO;
using Intelica.Infrastructure.Library.Email.DTO;

namespace Intelica.Security.API.Extensions
{
    public static class OptionsExtension
    {
        public static void AddConfiguration(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JWTConfiguration>(configuration.GetSection("JWT"));
            services.Configure<RSAConfiguration>(configuration.GetSection("RSA"));
            services.Configure<SendEmailConfiguration>(configuration.GetSection("EmailSettings"));
        }
    }
}