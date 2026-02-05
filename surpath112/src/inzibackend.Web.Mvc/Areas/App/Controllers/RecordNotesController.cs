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
    [AbpMvcAuthorize]

    public class RecordNotesController : inzibackendControllerBase
    {
        private readonly IRecordNotesAppService _recordNotesAppService;

        public RecordNotesController(IRecordNotesAppService recordNotesAppService)
        {
            _recordNotesAppService = recordNotesAppService;

        }
        [AbpMvcAuthorize(AppPermissions.Pages_RecordNotes)]

        public ActionResult Index()
        {
            var model = new RecordNotesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_RecordNotes)]

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
            }

            var viewModel = new CreateOrEditRecordNoteModalViewModel()
            {
                RecordNote = getRecordNoteForEditOutput.RecordNote,
                RecordStateNotes = getRecordNoteForEditOutput.RecordStateNotes,
                UserName = getRecordNoteForEditOutput.UserName,
                UserName2 = getRecordNoteForEditOutput.UserName2,
                RecordNoteUserList = await _recordNotesAppService.GetAllUserForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_RecordNotes)]

        [AbpMvcAuthorize(AppPermissions.Pages_RecordNotes_Create, AppPermissions.Pages_RecordNotes_Edit)]
        public async Task<PartialViewResult> AddNoteToRecordStateModal(Guid id, string? note, bool sendNotification = false)
        {
            GetRecordNoteForEditOutput getRecordNoteForEditOutput;
            if (string.IsNullOrEmpty(note))
            {
                note = String.Empty;
            }
            else
            {
                note.EnsureEndsWith(' ');

            }
            getRecordNoteForEditOutput = new GetRecordNoteForEditOutput
            {
                RecordNote = new CreateOrEditRecordNoteDto()
                {
                    Created = DateTime.UtcNow,
                    UserId = AbpSession.UserId,
                    RecordStateId = id,
                    Note = note,
                    SendNotification = sendNotification
                }
            };
            //getRecordNoteForEditOutput.RecordNote.Created = DateTime.Now;
            //getRecordNoteForEditOutput.RecordNote.UserId = AbpSession.UserId;

            var viewModel = new CreateOrEditRecordNoteModalViewModel()
            {
                RecordNote = getRecordNoteForEditOutput.RecordNote,
                RecordStateNotes = getRecordNoteForEditOutput.RecordStateNotes,
                UserName = getRecordNoteForEditOutput.UserName,
                UserName2 = getRecordNoteForEditOutput.UserName2,
                RecordNoteUserList = await _recordNotesAppService.GetAllUserForTableDropdown(),

            };

            return PartialView("_AddNoteToRecordStateModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_RecordNotes)]

        public async Task<PartialViewResult> ViewRecordNoteModal(Guid id)
        {
            var getRecordNoteForViewDto = await _recordNotesAppService.GetRecordNoteForView(id);

            var model = new RecordNoteViewModel()
            {
                RecordNote = getRecordNoteForViewDto.RecordNote
                ,
                RecordStateNotes = getRecordNoteForViewDto.RecordStateNotes

                ,
                UserName = getRecordNoteForViewDto.UserName

                ,
                UserName2 = getRecordNoteForViewDto.UserName2

            };

            return PartialView("_ViewRecordNoteModal", model);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_RecordNotes)]

        [AbpMvcAuthorize(AppPermissions.Pages_RecordNotes_Create, AppPermissions.Pages_RecordNotes_Edit)]
        public PartialViewResult RecordStateLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new RecordNoteRecordStateLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_RecordNoteRecordStateLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_RecordNotes)]

        [AbpMvcAuthorize(AppPermissions.Pages_RecordNotes_Create, AppPermissions.Pages_RecordNotes_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new RecordNoteUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_RecordNoteUserLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_CohortUsers, AppPermissions.Pages_CohortUser)]
        public PartialViewResult ViewNotesForRecord(Guid id)
        {
            ViewBag.RecordStateId = id;

            return PartialView("_ViewNotesForRecordModal");
        }
    }
}