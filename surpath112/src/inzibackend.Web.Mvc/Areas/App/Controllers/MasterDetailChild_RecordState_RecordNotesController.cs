using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.RecordNotes;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_RecordNotes)]
    public class MasterDetailChild_RecordState_RecordNotesController : inzibackendControllerBase
    {
        private readonly IRecordNotesAppService _recordNotesAppService;

        public MasterDetailChild_RecordState_RecordNotesController(IRecordNotesAppService recordNotesAppService)
        {
            _recordNotesAppService = recordNotesAppService;
        }

        public ActionResult Index(Guid recordStateId)
        {
            var model = new MasterDetailChild_RecordState_RecordNotesViewModel
            {
                FilterText = "",
                RecordStateId = recordStateId
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_RecordNotes_Create, AppPermissions.Pages_RecordNotes_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetRecordNoteForEditOutput getRecordNoteForEditOutput;

            if (id.HasValue)
            {
                getRecordNoteForEditOutput = await _recordNotesAppService.GetRecordNoteForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getRecordNoteForEditOutput = new GetRecordNoteForEditOutput
                {
                    RecordNote = new CreateOrEditRecordNoteDto()
                };
                getRecordNoteForEditOutput.RecordNote.Created = DateTime.UtcNow;
                getRecordNoteForEditOutput.RecordNote.UserId = AbpSession.UserId;
                getRecordNoteForEditOutput.RecordNote.AuthorizedOnly = true;
            }

            var viewModel = new MasterDetailChild_RecordState_CreateOrEditRecordNoteModalViewModel()
            {
                RecordNote = getRecordNoteForEditOutput.RecordNote,
                UserName = getRecordNoteForEditOutput.UserName,
                UserName2 = getRecordNoteForEditOutput.UserName2,
                RecordNoteUserList = await _recordNotesAppService.GetAllUserForTableDropdown(),
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewRecordNoteModal(Guid id)
        {
            var getRecordNoteForViewDto = await _recordNotesAppService.GetRecordNoteForView(id);

            var model = new MasterDetailChild_RecordState_RecordNoteViewModel()
            {
                RecordNote = getRecordNoteForViewDto.RecordNote
                ,
                UserName = getRecordNoteForViewDto.UserName

                ,
                UserName2 = getRecordNoteForViewDto.UserName2

            };

            return PartialView("_ViewRecordNoteModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_RecordNotes_Create, AppPermissions.Pages_RecordNotes_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new MasterDetailChild_RecordState_RecordNoteUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_RecordNoteUserLookupTableModal", viewModel);
        }

    }
}