using Intelica.Authentication.API.Common.DTO;
namespace Intelica.Security.API.Extensions
{
    public static class OptionsExtension
	{
		public static void AddConfiguration(IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<JWTConfiguration>(configuration.GetSection("JWT"));
			services.Configure<RSAConfiguration>(configuration.GetSection("RSA"));
        }
	}
}