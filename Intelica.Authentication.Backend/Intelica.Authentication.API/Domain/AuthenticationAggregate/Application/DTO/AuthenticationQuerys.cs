﻿namespace Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.DTO
{
    public record AuthenticationQuery(string BusinessUserEmail, string BusinessUserPassword, string PublicKey, string ClientID, string CallBack);
    public record ValidateTokenQuery(string Token, Guid RefreshToken, string Ip, string ClientID, string PageRoot, string HttpVerb);    

    public record AuthenticationMailQuery(string BusinessUserEmail);
    public record AuthenticationSendMailQuery(string BusinessUserEmail, string Code, string ClientID, string CallBack);
}
