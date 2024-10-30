using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.DTO;
using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.Interfaces;
using Intelica.Authentication.API.Domain.AuthenticationAggregate.Domain;
using Intelica.Authentication.Domain.Common.EFCore;
namespace Intelica.Authentication.API.Domain.AuthenticationAggregate.Infrastructure
{
    public class AuthenticationSQLServerRepository(Context context) : IAuthenticationRepository
    {
        public void CreateAccessInformation(AccessInformation accessInformation) => context.AccessInformation.Add(accessInformation); 
        public AccessInformation? FindAccessInformation(Guid accessInformationID)
        {
            var row = context.AccessInformation.SingleOrDefault(x => x.AccessInformationID.Equals(accessInformationID));
            return row;
        }
        public BussinesUserResponse? Find(Guid businessUserID)
        {
            var query = from businessUser in context.BusinessUsers.Where(x => x.BusinessUserID.Equals(businessUserID))
                        join profile in context.Profiles on businessUser.ProfileID equals profile.ProfileID
                        select new BussinesUserResponse(
                            businessUser.BusinessUserID, $"{businessUser.BusinessUserLastName}, {businessUser.BusinessUserName}",
                            businessUser.BusinessUserEmail, businessUser.BusinessUserPassword, profile.ProfileName, businessUser.BusinessUserFirstLogin,null
                        );
            var list = query.ToList();
            if (list.Count == 0) return null;
            return query.First();
        }
        public BussinesUserResponse? FindByEmail(string businessUserEmail)
        {
            var query = from businessUser in context.BusinessUsers.Where(x => x.BusinessUserEmail.Equals(businessUserEmail))
                        join profile in context.Profiles on businessUser.ProfileID equals profile.ProfileID
                        select new BussinesUserResponse(businessUser.BusinessUserID, $"{businessUser.BusinessUserLastName}, {businessUser.BusinessUserName}",
                        businessUser.BusinessUserEmail, businessUser.BusinessUserPassword, profile.ProfileName, businessUser.BusinessUserFirstLogin,
                        (
                            from businessUserPage in context.BusinessUserPages.Where(x => x.BusinessUserID.Equals(businessUser.BusinessUserID))
                            join page in context.Pages on businessUserPage.PageID equals page.PageID
                            select new BusinessUserPageResponse(businessUserPage.PageID, page.PageRoot, businessUserPage.BusinessUserPageCanUpdate, 
                            businessUserPage.BusinessUserPageCanCreate, businessUserPage.BusinessUserPageCanDelete)
                        ).ToList()
                    );
            var list = query.ToList();
            if (list.Count == 0) return null;
            return query.First();
        }
        public void SaveChanges() => context.SaveChanges();
    }
}