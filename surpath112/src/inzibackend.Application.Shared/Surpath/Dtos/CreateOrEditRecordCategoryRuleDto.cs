using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditRecordCategoryRuleDto : EntityDto<Guid?>
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Notify { get; set; }

        public int ExpireInDays { get; set; }

        public int WarnDaysBeforeFirst { get; set; }

        public bool Expires { get; set; }

        public bool Required { get; set; }

        public bool IsSurpathOnly { get; set; }

        public int WarnDaysBeforeSecond { get; set; }

        public int WarnDaysBeforeFinal { get; set; }

        public string MetaData { get; set; }

        public Guid? FirstWarnStatusId { get; set; }

        public Guid? SecondWarnStatusId { get; set; }

        public Guid? FinalWarnStatusId { get; set; }

        public Guid? ExpiredStatusId { get; set; }
    }
}