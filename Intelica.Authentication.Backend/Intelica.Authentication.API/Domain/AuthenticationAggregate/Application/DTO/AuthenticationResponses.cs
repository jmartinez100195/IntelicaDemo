namespace Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.DTO
{
    public record AuthenticationResponse(string JwtToken, string RefreshToken);
    public record BussinesUserResponse(Guid BusinessUserID, string BusinessUserName, string BusinessUserEmail, List<BusinessUserPageResponse> BusinessUserPages);
    public record BusinessUserPageResponse(Guid PageID, string PageRoot, bool CanUpdate, bool CanCreate, bool CanDelete);
}