using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.DTO;
namespace Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.Interfaces
{
    public interface IAuthenticationRepository
    {
        BussinesUserResponse? FindByEmail(string businessUserEmail);
    }
}