namespace Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.DTO
{
    public record AuthenticationQuery(string BusinessUserEmail, string BusinessUserPassword, string PublicKey, string ClientID, string CallBack);
    public record RefreshTokenQuery(Guid RefreshToken, string ClientID);
}
