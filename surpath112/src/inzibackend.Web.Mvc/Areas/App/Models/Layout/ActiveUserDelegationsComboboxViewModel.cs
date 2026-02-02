using System.Collections.Generic;
using inzibackend.Authorization.Delegation;
using inzibackend.Authorization.Users.Delegation.Dto;

namespace inzibackend.Web.Areas.App.Models.Layout
{
    public class ActiveUserDelegationsComboboxViewModel
    {
        public IUserDelegationConfiguration UserDelegationConfiguration { get; set; }

        public List<UserDelegationDto> UserDelegations { get; set; }

        public string CssClass { get; set; }
    }
}
