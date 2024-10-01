using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.DTO;
using Intelica.Authentication.API.Domain.AuthenticationAggregate.Domain;
namespace Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.Interfaces
{
    public interface IAuthenticationRepository
    {
        BussinesUserResponse? FindByEmail(string businessUserEmail);
        void CreateAccessInformation(AccessInformation accessInformation);
        AccessInformation? FindAccessInformation(Guid accessInformationID);
        void SaveChanges();
    }
}