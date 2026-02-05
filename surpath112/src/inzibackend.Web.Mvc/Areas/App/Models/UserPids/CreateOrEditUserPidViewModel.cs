using inzibackend.Surpath.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.UserPids
{
    public class CreateOrEditUserPidModalViewModel
    {
        public CreateOrEditUserPidDto UserPid { get; set; }

        public string PidTypeName { get; set; }

        public string UserName { get; set; }

        public List<UserPidPidTypeLookupTableDto> UserPidPidTypeList { get; set; }

        public bool IsEditMode => UserPid.Id.HasValue;
    }
}