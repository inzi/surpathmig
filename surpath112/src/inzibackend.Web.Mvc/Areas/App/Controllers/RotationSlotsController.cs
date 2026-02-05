using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.RotationSlots;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using inzibackend.Surpath.Statics;
using System.Linq;
using System.Collections.Generic;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_RotationSlots)]
    public class RotationSlotsController : inzibackendControllerBase
    {
        private readonly IRotationSlotsAppService _rotationSlotsAppService;

        public RotationSlotsController(IRotationSlotsAppService rotationSlotsAppService)
        {
            _rotationSlotsAppService = rotationSlotsAppService;

        }

        public ActionResult Index()
        {
            var model = new RotationSlotsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_RotationSlots_Create, AppPermissions.Pages_RotationSlots_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetRotationSlotForEditOutput getRotationSlotForEditOutput;

            if (id.HasValue)
            {
                getRotationSlotForEditOutput = await _rotationSlotsAppService.GetRotationSlotForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getRotationSlotForEditOutput = new GetRotationSlotForEditOutput
                {
                    RotationSlot = new CreateOrEditRotationSlotDto()
                    {
                        SlotAvailableDays = new List<SlotAvailableDayDto>(),
                        SlotRotationDays = new List<SlotRotationDayDto>()
                    }
                };
                getRotationSlotForEditOutput.RotationSlot.ShiftStartDate = DateTime.Now;
                getRotationSlotForEditOutput.RotationSlot.ShiftEndDate = DateTime.Now;
                getRotationSlotForEditOutput.RotationSlot.ShiftStartTime = DateTime.Now;
                getRotationSlotForEditOutput.RotationSlot.ShiftEndTime = DateTime.Now;
                getRotationSlotForEditOutput.RotationSlot.HospitalNotifiedDateTime = DateTime.Now;
                getRotationSlotForEditOutput.RotationSlot.BidStartDateTime = DateTime.Now;
                getRotationSlotForEditOutput.RotationSlot.BidEndDateTime = DateTime.Now;
            }

            if (getRotationSlotForEditOutput.RotationSlot.SlotAvailableDays.Count == 0 || getRotationSlotForEditOutput.RotationSlot.SlotRotationDays.Count == 0)
            {
                foreach (DayOfWeek day in DayOfWeekOrderExtensions.GetDaysStartingWith(DayOfWeek.Monday))
                {
                    if (!getRotationSlotForEditOutput.RotationSlot.SlotAvailableDays.Any(s => s.Day == day))
                    {
                        getRotationSlotForEditOutput.RotationSlot.SlotAvailableDays.Add(new SlotAvailableDayDto
                        {
                            Day = day
                        });
                    }

                    if (!getRotationSlotForEditOutput.RotationSlot.SlotRotationDays.Any(s => s.Day == day))
                    {
                        getRotationSlotForEditOutput.RotationSlot.SlotRotationDays.Add(new SlotRotationDayDto
                        {
                            Day = day
                        });
                    };
                }
            }

            var viewModel = new CreateOrEditRotationSlotModalViewModel()
            {
                RotationSlot = getRotationSlotForEditOutput.RotationSlot,
                HospitalName = getRotationSlotForEditOutput.HospitalName,
                MedicalUnitName = getRotationSlotForEditOutput.MedicalUnitName,
                RotationSlotHospitalList = await _rotationSlotsAppService.GetAllHospitalForTableDropdown(),
                RotationSlotMedicalUnitList = await _rotationSlotsAppService.GetAllMedicalUnitForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewRotationSlotModal(int id)
        {
            var getRotationSlotForViewDto = await _rotationSlotsAppService.GetRotationSlotForView(id);

            var model = new RotationSlotViewModel()
            {
                RotationSlot = getRotationSlotForViewDto.RotationSlot
                ,
                HospitalName = getRotationSlotForViewDto.HospitalName

                ,
                MedicalUnitName = getRotationSlotForViewDto.MedicalUnitName

            };

            return PartialView("_ViewRotationSlotModal", model);
        }

    }
}