using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public bool Shuffle { get; set; } = false;
        public long TenantId { get; set; }
        public long GuidId { get; set; }
    }
}