using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.CodeTypes;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_CodeTypes)]
    public class CodeTypesController : inzibackendControllerBase
    {
        private readonly ICodeTypesAppService _codeTypesAppService;

        public CodeTypesController(ICodeTypesAppService codeTypesAppService)
        {
            _codeTypesAppService = codeTypesAppService;

        }

        public ActionResult Index()
        {
            var model = new CodeTypesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_CodeTypes_Create, AppPermissions.Pages_Administration_CodeTypes_Edit)]
        public async Task<ActionResult> CreateOrEdit(Guid? id)
        {
            GetCodeTypeForEditOutput getCodeTypeForEditOutput;

            if (id.HasValue)
            {
                getCodeTypeForEditOutput = await _codeTypesAppService.GetCodeTypeForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getCodeTypeForEditOutput = new GetCodeTypeForEditOutput
                {
                    CodeType = new CreateOrEditCodeTypeDto()
                };
            }

            var viewModel = new CreateOrEditCodeTypeViewModel()
            {
                CodeType = getCodeTypeForEditOutput.CodeType,
            };

            return View(viewModel);
        }

        public async Task<ActionResult> ViewCodeType(Guid id)
        {
            var getCodeTypeForViewDto = await _codeTypesAppService.GetCodeTypeForView(id);

            var model = new CodeTypeViewModel()
            {
                CodeType = getCodeTypeForViewDto.CodeType
            };

            return View(model);
        }

    }
}