using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.DTO;
using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.Interfaces;
using Intelica.Security.Domain.Common.EFCore;
namespace Intelica.Authentication.API.Domain.Infrastructure
{
    public class AuthenticationSQLServerRepository(Context context) : IAuthenticationRepository
    {
        public BussinesUserResponse? FindByCredentials(string businessUserEmail, string businessUserPassword)
        {
            return new BussinesUserResponse(Guid.NewGuid(), "Espejo Huerta, Carlos Rufino", "carlos.espejo@intelica.com", [new(Guid.NewGuid(), "Bank", true, true, true), new(Guid.NewGuid(), "Page", true, true, true)]);
            //var q = from businessUser in context.BusinessUsers.Where(x => x.BusinessUserEmail.Equals(businessUserEmail) && x.BusinessUserPassword.Equals(businessUserPassword))
            //        select new BussinesUserResponse(businessUser.BusinessUserID, $"{businessUser.BusinessUserLastName}, {businessUser.BusinessUserName}", businessUser.BusinessUserEmail,
            //         (from businessUserPage in context.BusinessUserPages.Where(x => x.BusinessUserPageID.Equals(businessUser.BusinessUserID))
            //          select new BusinessUserPageResponse(businessUserPage.PageID, "", businessUserPage.BusinessUserPageCanUpdate, businessUserPage.BusinessUserPageCanCreate, businessUserPage.BusinessUserPageCanDelete)
            //          ).ToList()
            //        );
            //var list = q.ToList();
            //if (!list.Any()) return null;
            //return q.First();
        }
    }
}
