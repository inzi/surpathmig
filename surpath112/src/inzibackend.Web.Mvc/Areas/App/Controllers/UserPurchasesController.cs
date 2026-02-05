using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.UserPurchases;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using inzibackend.Authorization.Users;
using inzibackend.Common;
using Abp.UI;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Host_Dashboard)]
    public class UserPurchasesController : inzibackendControllerBase
    {
        private readonly IUserPurchasesAppService _userPurchasesAppService;
        private readonly ICommonLookupAppService _commonLookupAppService;

        public UserPurchasesController(
            IUserPurchasesAppService userPurchasesAppService,
            ICommonLookupAppService commonLookupAppService)
        {
            _userPurchasesAppService = userPurchasesAppService;
            _commonLookupAppService = commonLookupAppService;
        }

        public ActionResult Index()
        {
            var model = new UserPurchasesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        public async Task<ActionResult> UserPurchases(long id)
        {
            var model = new UserPurchasesViewModel
            {
                FilterText = "",
                UserIdFilter = id
            };

            return View("Index", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_UserPurchases_Create, AppPermissions.Pages_UserPurchases_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetUserPurchaseForEditOutput getUserPurchaseForEditOutput;

            if (id.HasValue)
            {
                getUserPurchaseForEditOutput = await _userPurchasesAppService.GetUserPurchaseForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getUserPurchaseForEditOutput = new GetUserPurchaseForEditOutput
                {
                    UserPurchase = new CreateOrEditUserPurchaseDto()
                };
                getUserPurchaseForEditOutput.UserPurchase.PurchaseDate = DateTime.Now;
                getUserPurchaseForEditOutput.UserPurchase.Status = EnumPurchaseStatus.New;
            }

            var viewModel = new CreateOrEditUserPurchaseModalViewModel()
            {
                UserPurchase = getUserPurchaseForEditOutput.UserPurchase,
                UserName = getUserPurchaseForEditOutput.UserName,
                SurpathServiceName = getUserPurchaseForEditOutput.SurpathServiceName,
                TenantSurpathServiceName = getUserPurchaseForEditOutput.TenantSurpathServiceName,
                CohortName = getUserPurchaseForEditOutput.CohortName
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewUserPurchaseModal(Guid id)
        {
            var getUserPurchaseForViewDto = await _userPurchasesAppService.GetUserPurchaseForView(id);

            var model = new UserPurchaseViewModel()
            {
                UserPurchase = getUserPurchaseForViewDto.UserPurchase,
                UserName = getUserPurchaseForViewDto.UserName,
                SurpathServiceName = getUserPurchaseForViewDto.SurpathServiceName,
                TenantSurpathServiceName = getUserPurchaseForViewDto.TenantSurpathServiceName,
                CohortName = getUserPurchaseForViewDto.CohortName
            };

            return PartialView("_ViewUserPurchaseModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_UserPurchases_ApplyPayment)]
        public async Task<PartialViewResult> ApplyPaymentModal(Guid id)
        {
            var getUserPurchaseForViewDto = await _userPurchasesAppService.GetUserPurchaseForView(id);

            var model = new ApplyPaymentViewModel()
            {
                UserPurchaseId = id,
                UserPurchaseName = getUserPurchaseForViewDto.UserPurchase.Name,
                BalanceDue = getUserPurchaseForViewDto.UserPurchase.BalanceDue,
                Amount = 0,
                PaymentMethod = "Credit Card",
                Notes = ""
            };

            return PartialView("_ApplyPaymentModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_UserPurchases_AdjustPrice)]
        public async Task<PartialViewResult> AdjustPriceModal(Guid id)
        {
            var getUserPurchaseForViewDto = await _userPurchasesAppService.GetUserPurchaseForView(id);

            var model = new AdjustPriceViewModel()
            {
                UserPurchaseId = id,
                UserPurchaseName = getUserPurchaseForViewDto.UserPurchase.Name,
                OriginalPrice = getUserPurchaseForViewDto.UserPurchase.OriginalPrice,
                CurrentAdjustedPrice = getUserPurchaseForViewDto.UserPurchase.AdjustedPrice,
                NewAdjustedPrice = getUserPurchaseForViewDto.UserPurchase.AdjustedPrice,
                Reason = ""
            };

            return PartialView("_AdjustPriceModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_UserPurchases_Create, AppPermissions.Pages_UserPurchases_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new UserPurchaseUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_UserPurchaseUserLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_UserPurchases_Create, AppPermissions.Pages_UserPurchases_Edit)]
        public PartialViewResult SurpathServiceLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new UserPurchaseSurpathServiceLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_UserPurchaseSurpathServiceLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_UserPurchases_Create, AppPermissions.Pages_UserPurchases_Edit)]
        public PartialViewResult TenantSurpathServiceLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new UserPurchaseTenantSurpathServiceLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_UserPurchaseTenantSurpathServiceLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_UserPurchases_Create, AppPermissions.Pages_UserPurchases_Edit)]
        public PartialViewResult CohortLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new UserPurchaseCohortLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_UserPurchaseCohortLookupTableModal", viewModel);
        }
    }
}