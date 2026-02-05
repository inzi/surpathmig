using inzibackend.Authorization.Users.Dto;
using System;

namespace inzibackend.Web.Areas.App.Models.LedgerEntries
{
    public class LedgerEntriesViewModel
    {
        public string FilterText { get; set; }

        public UserEditDto UserEditDto { get; set; } = new UserEditDto();
    }
}