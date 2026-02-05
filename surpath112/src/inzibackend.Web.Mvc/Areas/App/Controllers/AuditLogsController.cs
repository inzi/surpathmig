using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Auditing;
using inzibackend.Auditing.Dto;
using inzibackend.Authorization;
using inzibackend.Web.Areas.App.Models.AuditLogs;
using inzibackend.Web.Controllers;
using inzibackend.Surpath.Compliance;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [DisableAuditing]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_AuditLogs)]
    public class AuditLogsController : inzibackendControllerBase
    {
        private readonly IAuditLogAppService _auditLogAppService;
        private SurpathManager _surpathManager;

        public AuditLogsController(IAuditLogAppService auditLogAppService, SurpathManager surpathManager)
        {
            _auditLogAppService = auditLogAppService;
            _surpathManager = surpathManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> EntityChangeDetailModal(EntityChangeListDto entityChangeListDto)
        {
            var output = await _auditLogAppService.GetEntityPropertyChanges(entityChangeListDto.Id);

            var viewModel = new EntityChangeDetailModalViewModel(output, entityChangeListDto);

            foreach(var entityChangeDto in viewModel.EntityPropertyChanges)
            {
                if (entityChangeDto.PropertyName.EndsWith("userid", System.StringComparison.InvariantCultureIgnoreCase))
                {
                    var _id = entityChangeDto.NewValue;
                    if (long.TryParse(_id, out long _userid))
                    {
                        var _name = await _surpathManager.GetUserNameById(_userid);
                        if (!string.IsNullOrEmpty(_name))
                            entityChangeDto.NewValue = entityChangeDto.NewValue + " (" + _name + ")";
                    }
                }
            }

            return PartialView("_EntityChangeDetailModal", viewModel);
        }
    }
}