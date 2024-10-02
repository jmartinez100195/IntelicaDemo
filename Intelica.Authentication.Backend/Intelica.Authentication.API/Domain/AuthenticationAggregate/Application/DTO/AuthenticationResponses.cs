namespace Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.DTO
{
    public record AuthenticationResponse(string JwtToken, string RefreshToken, bool valid, bool BusinessUserFirstLogin);
    public record BussinesUserResponse(Guid BusinessUserID, string BusinessUserName, string BusinessUserEmail, string BusinessUserPassword, bool BusinessUserFirstLogin,
    List<BusinessUserPageResponse> BusinessUserPages);
    public record BusinessUserPageResponse(Guid PageID, string PageRoot, bool CanUpdate, bool CanCreate, bool CanDelete);
    public record ValidTokenResponse(bool Active, bool Authorize);
    public record RefreshTokenResponse(bool ValidRefreshToken, bool ExpiredRefreshToken, string Token);
}