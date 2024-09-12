using System.IdentityModel.Tokens.Jwt;
namespace Intelica.Security.API.Extensions
{
    public class UserDataRetriever
    {
        public Guid BussinesUserID { get; private set; }
        public Guid MenuUserID { get; set; }
        public string UserName { get; private set; }
        public UserDataRetriever(string autorizationHeader)
        {
            var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(autorizationHeader[7..^1]);
            UserName = jwtSecurityToken.Claims.Single(x => x.Type.Equals("preferred_username")).Value;
            BussinesUserID = Guid.Parse(jwtSecurityToken.Claims.Single(x => x.Type.Equals("sub")).Value);
        }
        public UserDataRetriever(IHeaderDictionary header)
        {
            var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(header.Authorization.ToString()[7..^1]);
            UserName = jwtSecurityToken.Claims.Single(x => x.Type.Equals("preferred_username")).Value;
            BussinesUserID = Guid.Parse(jwtSecurityToken.Claims.Single(x => x.Type.Equals("sub")).Value);
            if (header["MenuUserID"].ToString().Length > 0)
                MenuUserID = Guid.Parse(header["MenuUserID"].ToString());
        }
    }
}