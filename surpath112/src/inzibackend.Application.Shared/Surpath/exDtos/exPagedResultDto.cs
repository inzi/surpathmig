using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace inzibackend.Surpath.exdtos
{
    //public class exPagedResultDto : PagedResultDto<GetRecordStateForViewDto>
    //{
    //    public string SortingUser { get; set; }

    //}
    //
    // Summary:
    //     Implements Abp.Application.Services.Dto.IPagedResult`1.
    //
    // Type parameters:
    //   T:
    //     Type of the items in the Abp.Application.Services.Dto.ListResultDto`1.Items list
    [Serializable]
    public class PagedResultDto<T> : ListResultDto<T>, IPagedResult<T>, IListResult<T>, IHasTotalCount
    {
        //
        // Summary:
        //     Total count of Items.
        public int TotalCount
        {
            get;
            set;
        }

        public string SortingUser { get; set; }

        //
        // Summary:
        //     Creates a new Abp.Application.Services.Dto.PagedResultDto`1 object.
        public PagedResultDto()
        {
        }

        //
        // Summary:
        //     Creates a new Abp.Application.Services.Dto.PagedResultDto`1 object.
        //
        // Parameters:
        //   totalCount:
        //     Total count of Items
        //
        //   items:
        //     List of items in current page
        public PagedResultDto(int totalCount, IReadOnlyList<T> items, string sortingUser = "")
            : base(items)
        {
            TotalCount = totalCount;
            SortingUser = sortingUser;
        }
    }
}
