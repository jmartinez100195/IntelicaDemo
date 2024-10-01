namespace Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.DTO
{
    public record AuthenticationResponse(string JwtToken, string RefreshToken, bool valid,  bool BusinessUserFirstLogeo);
    public record BussinesUserResponse(Guid BusinessUserID, string BusinessUserName, string BusinessUserEmail, string BusinessUserPassword, bool BusinessUserFirstLogeo,
    List<BusinessUserPageResponse> BusinessUserPages);
    public record BusinessUserPageResponse(Guid PageID, string PageRoot, bool CanUpdate, bool CanCreate, bool CanDelete);
    public record ValidTokenResponse(bool Active, bool Authorize);
    public record RefreshTokenResponse(bool ValidRefreshToken, bool ExpiredRefreshToken, string Token);
}