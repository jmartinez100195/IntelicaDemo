using Intelica.Infrastructure.Library.Log.DTO;

namespace Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.DTO
{
    public record AuthenticationResponse(string JwtToken, string RefreshToken, bool valid, string message);
    public record BussinesUserResponse(Guid BusinessUserID, string BusinessUserName, string BusinessUserEmail, string BusinessUserPassword,
    List<BusinessUserPageResponse> BusinessUserPages);
    public record BusinessUserPageResponse(Guid PageID, string PageRoot, bool CanUpdate, bool CanCreate, bool CanDelete);
    public record ValidTokenResponse(bool active , bool autorize, string message);
}