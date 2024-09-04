namespace Intelica.Authentication.API.DTO
{
    public record AuthenticateQuery( );
    public record ChallengeCodeQuery();
    public record PublicKeyResponse( string publicKey );
}
