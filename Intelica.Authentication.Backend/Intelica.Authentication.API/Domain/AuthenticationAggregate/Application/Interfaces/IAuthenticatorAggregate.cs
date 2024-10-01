using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.DTO;
namespace Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.Interfaces
{
    public interface IAuthenticatorAggregate
    {
        AuthenticationResponse ValidateAuthentication(AuthenticationQuery authenticationQuery, string ip);
        RefreshTokenResponse ValidateRefreshToken(Guid refreshToken, string businessUserEmail, string clientID, string ip);
        ValidTokenResponse ValidateToken(string token, string pageRoot, string httpVerb);
    }
}