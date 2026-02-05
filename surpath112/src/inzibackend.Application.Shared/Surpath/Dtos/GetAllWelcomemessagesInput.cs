using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllWelcomemessagesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string TitleFilter { get; set; }

        public string MessageFilter { get; set; }

        public int? IsDefaultFilter { get; set; }

        public DateTime? MaxDisplayStartFilter { get; set; }
        public DateTime? MinDisplayStartFilter { get; set; }

        public DateTime? MaxDisplayEndFilter { get; set; }
        public DateTime? MinDisplayEndFilter { get; set; }

    }
}