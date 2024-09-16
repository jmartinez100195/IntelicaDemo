using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.DTO;
namespace Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.Interfaces
{
    public interface IAuthenticatorAggregate
    {
        BussinesUserResponse? ValidateCredentials(string businessUserEmail, string businessUserPassword, string publicKey);
        AuthenticationResponse GenerateToken(BussinesUserResponse businessUserResponse);
    }
}