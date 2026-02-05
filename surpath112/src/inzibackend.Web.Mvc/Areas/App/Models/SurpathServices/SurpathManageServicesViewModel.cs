using inzibackend.MultiTenancy.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Web.Areas.App.Models.RecordCategoryRules;
using inzibackend.Web.Areas.App.Models.RecordRequirements;
using inzibackend.Web.Areas.App.Models.Tenants;
using inzibackend.Web.Areas.App.Models.TenantSurpathServices;
using System;
using System.Collections.Generic;

namespace inzibackend.Web.Areas.App.Models.SurpathServices
{
    public class SurpathManageServicesViewModel
    {
        public string FilterText { get; set; }

        // List of Tenants
        public List<TenantListDto> Tenants { get; set; }

        // List of Surpath Services
        public List<GetSurpathServiceForViewDto> SurpathServices { get; set; }

        // List of TenantServices
        public List<TenantSurpathServiceDto> TenantSurpathServices { get; set; }

        // List of Surpath Service Rules
        public List<RecordCategoryRuleDto> SurpathServiceRules { get; set; }


        // List of Surpath Service Requirements
        public List<RecordRequirementDto> ServiceRequirements { get; set; }


    }
}