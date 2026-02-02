using inzibackend.Surpath.Dtos;
using System.Collections.Generic;
using Abp.Extensions;
using inzibackend.Web.Areas.App.Models.Records;

namespace inzibackend.Web.Areas.App.Models.RecordStates
{
    public class CreateOrEditRecordStateModalViewModel
    {
        public CreateOrEditRecordStateDto RecordState { get; set; }

        public string Recordfilename { get; set; }

        public string RecordCategoryName { get; set; }
        public string RecordRequirementName { get; set; }
        public string UserName { get; set; }

        public string RecordStatusStatusName { get; set; }
        

        public List<RecordStateRecordStatusLookupTableDto> RecordStateRecordStatusList { get; set; }

        public bool IsEditMode => RecordState.Id.HasValue;
        public CreateOrEditRecordModalViewModel CreateOrEditRecordModalViewModel = new CreateOrEditRecordModalViewModel();
        public CohortUserDto CohortUser { get; set; }
    }
}