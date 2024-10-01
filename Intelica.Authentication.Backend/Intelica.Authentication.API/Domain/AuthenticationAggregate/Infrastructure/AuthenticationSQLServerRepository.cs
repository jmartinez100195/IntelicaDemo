using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.DTO;
using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.Interfaces;
using Intelica.Security.Domain.Common.EFCore;
namespace Intelica.Authentication.API.Domain.AuthenticationAggregate.Infrastructure
{
    public class AuthenticationSQLServerRepository(Context context) : IAuthenticationRepository
    {
        public BussinesUserResponse? FindByEmail(string businessUserEmail)
        {
            //return new BussinesUserResponse(Guid.NewGuid(), "Espejo Huerta, Carlos Rufino", "carlos.espejo@intelica.com", [new(Guid.NewGuid(), "Bank", true, true, true), new(Guid.NewGuid(), "Page", true, true, true)]);
            var query = from businessUser in context.BusinessUsers.Where(x => x.BusinessUserEmail.Equals(businessUserEmail))
                        select new BussinesUserResponse(businessUser.BusinessUserID, $"{businessUser.BusinessUserLastName}, {businessUser.BusinessUserName}",
                        businessUser.BusinessUserEmail, businessUser.BusinessUserPassword, businessUser.BusinessUserFirstLogeo,
                        (
                            from businessUserPage in context.BusinessUserPages.Where(x => x.BusinessUserID.Equals(businessUser.BusinessUserID))
                            join page in context.Pages on businessUserPage.PageID equals page.PageID
                            select new BusinessUserPageResponse(businessUserPage.PageID, page.PageRoot, businessUserPage.BusinessUserPageCanUpdate, businessUserPage.BusinessUserPageCanCreate, businessUserPage.BusinessUserPageCanDelete)
                        ).ToList()
                    );
            var list = query.ToList();
            if (list.Count == 0) return null;
            return query.First();
        }
    }
}