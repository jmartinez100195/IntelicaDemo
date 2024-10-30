namespace Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.DTO
{
    public record AuthenticationResponse(string JwtToken, string RefreshToken, bool valid, bool BusinessUserFirstLogin);
    public record AuthenticationLocalResponse(string JwtToken, bool valid);
    public record BussinesUserResponse(Guid BusinessUserID, string BusinessUserName, string BusinessUserEmail, string BusinessUserPassword, bool BusinessUserFirstLogin,
    List<BusinessUserPageResponse>? BusinessUserPages);
    public record BusinessUserPageResponse(Guid PageID, string PageRoot, bool CanUpdate, bool CanCreate, bool CanDelete);
    public record ValidateTokenResponse(bool Expired, bool Unauthorized, string NewToken);
    public record AuthenticationMailResponse(bool valid, string Code);
    public record AuthenticationSendMailResponse(string JwtToken, string RefreshToken, bool valid);
}