using System.ComponentModel.DataAnnotations;
using Abp.Runtime.Validation;
using inzibackend.Common;
using inzibackend.Dto;

namespace inzibackend.Organizations.Dto
{
    public class GetOrganizationUnitTenantDepartmentInput : PagedAndSortedInputDto, IShouldNormalize
    {
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "department.Name";
            }

            Sorting = DtoSortingHelper.ReplaceSorting(Sorting, s =>
            {
                //if (s.Contains("Name"))
                //{
                //    s = s.Replace("Name", "department.Name");
                //}

                if (s.Contains("addedTime"))
                {
                    s = s.Replace("addedTime", "ouDepartment.creationTime");
                }

                return s;
            });
        }
    }
}
