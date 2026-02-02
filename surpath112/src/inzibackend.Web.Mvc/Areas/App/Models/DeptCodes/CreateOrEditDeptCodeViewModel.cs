using inzibackend.Surpath.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.DeptCodes
{
    public class CreateOrEditDeptCodeViewModel
    {
        public CreateOrEditDeptCodeDto DeptCode { get; set; }

        public string CodeTypeName { get; set; }

        public string TenantDepartmentName { get; set; }

        public List<DeptCodeCodeTypeLookupTableDto> DeptCodeCodeTypeList { get; set; }

        public bool IsEditMode => DeptCode.Id.HasValue;
    }
}