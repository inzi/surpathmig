using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using inzibackend.Surpath.Exporting;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using Abp.Application.Services.Dto;
using inzibackend.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using inzibackend.Storage;
using Abp.Domain.Uow;

namespace inzibackend.Surpath
{
    [AbpAuthorize(AppPermissions.Pages_PidTypes)]
    public class PidTypesAppService : inzibackendAppServiceBase, IPidTypesAppService
    {
        private readonly IRepository<PidType, Guid> _pidTypeRepository;
        private readonly IPidTypesExcelExporter _pidTypesExcelExporter;

        public PidTypesAppService(IRepository<PidType, Guid> pidTypeRepository, IPidTypesExcelExporter pidTypesExcelExporter)
        {
            _pidTypeRepository = pidTypeRepository;
            _pidTypesExcelExporter = pidTypesExcelExporter;

        }

        public async Task<PagedResultDto<GetPidTypeForViewDto>> GetAll(GetAllPidTypesInput input)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            }

            var filteredPidTypes = _pidTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.PidRegex.Contains(input.Filter) || e.PidInputMask.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(input.MaskPidFilter.HasValue && input.MaskPidFilter > -1, e => (input.MaskPidFilter == 1 && e.MaskPid) || (input.MaskPidFilter == 0 && !e.MaskPid))
                        .WhereIf(input.MinCreatedOnFilter != null, e => e.CreatedOn >= input.MinCreatedOnFilter)
                        .WhereIf(input.MaxCreatedOnFilter != null, e => e.CreatedOn <= input.MaxCreatedOnFilter)
                        .WhereIf(input.MinModifiedOnFilter != null, e => e.ModifiedOn >= input.MinModifiedOnFilter)
                        .WhereIf(input.MaxModifiedOnFilter != null, e => e.ModifiedOn <= input.MaxModifiedOnFilter)
                        .WhereIf(input.MinCreatedByFilter != null, e => e.CreatedBy >= input.MinCreatedByFilter)
                        .WhereIf(input.MaxCreatedByFilter != null, e => e.CreatedBy <= input.MaxCreatedByFilter)
                        .WhereIf(input.MinLastModifiedByFilter != null, e => e.LastModifiedBy >= input.MinLastModifiedByFilter)
                        .WhereIf(input.MaxLastModifiedByFilter != null, e => e.LastModifiedBy <= input.MaxLastModifiedByFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PidInputMaskFilter), e => e.PidInputMask == input.PidInputMaskFilter)
                        .WhereIf(input.RequiredFilter.HasValue && input.RequiredFilter > -1, e => (input.RequiredFilter == 1 && e.Required) || (input.RequiredFilter == 0 && !e.Required));

            var pagedAndFilteredPidTypes = filteredPidTypes
                .OrderBy(input.Sorting ?? "name asc")
                .PageBy(input);

            var pidTypes = from o in pagedAndFilteredPidTypes
                           select new
                           {

                               o.Name,
                               o.Description,
                               o.MaskPid,
                               //o.CreatedOn,
                               CreatedOn = DateTime.SpecifyKind(o.CreatedOn, DateTimeKind.Utc),

                               o.ModifiedOn,
                               o.CreatedBy,
                               o.LastModifiedBy,
                               o.IsActive,
                               o.PidInputMask,
                               o.Required,
                               Id = o.Id
                           };

            var totalCount = await filteredPidTypes.CountAsync();

            var dbList = await pidTypes.ToListAsync();
            var results = new List<GetPidTypeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPidTypeForViewDto()
                {
                    PidType = new PidTypeDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        MaskPid = o.MaskPid,
                        CreatedOn = o.CreatedOn,
                        ModifiedOn = o.ModifiedOn,
                        CreatedBy = o.CreatedBy,
                        LastModifiedBy = o.LastModifiedBy,
                        IsActive = o.IsActive,
                        PidInputMask = o.PidInputMask,
                        Required = o.Required,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetPidTypeForViewDto>(
                totalCount,
                results
            );
        }

        public async Task<GetPidTypeForViewDto> GetPidTypeForView(Guid id)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var pidType = await _pidTypeRepository.GetAsync(id);

            var output = new GetPidTypeForViewDto { PidType = ObjectMapper.Map<PidTypeDto>(pidType) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_PidTypes_Edit)]
        public async Task<GetPidTypeForEditOutput> GetPidTypeForEdit(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var pidType = await _pidTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPidTypeForEditOutput { PidType = ObjectMapper.Map<CreateOrEditPidTypeDto>(pidType) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPidTypeDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_PidTypes_Create)]
        protected virtual async Task Create(CreateOrEditPidTypeDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var pidType = ObjectMapper.Map<PidType>(input);

            if (AbpSession.TenantId != null)
            {
                pidType.TenantId = (int?)AbpSession.TenantId;
            }

            await _pidTypeRepository.InsertAsync(pidType);

        }

        [AbpAuthorize(AppPermissions.Pages_PidTypes_Edit)]
        protected virtual async Task Update(CreateOrEditPidTypeDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var pidType = await _pidTypeRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, pidType);

        }

        [AbpAuthorize(AppPermissions.Pages_PidTypes_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            await _pidTypeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetPidTypesToExcel(GetAllPidTypesForExcelInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var filteredPidTypes = _pidTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.PidRegex.Contains(input.Filter) || e.PidInputMask.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(input.MaskPidFilter.HasValue && input.MaskPidFilter > -1, e => (input.MaskPidFilter == 1 && e.MaskPid) || (input.MaskPidFilter == 0 && !e.MaskPid))
                        .WhereIf(input.MinCreatedOnFilter != null, e => e.CreatedOn >= input.MinCreatedOnFilter)
                        .WhereIf(input.MaxCreatedOnFilter != null, e => e.CreatedOn <= input.MaxCreatedOnFilter)
                        .WhereIf(input.MinModifiedOnFilter != null, e => e.ModifiedOn >= input.MinModifiedOnFilter)
                        .WhereIf(input.MaxModifiedOnFilter != null, e => e.ModifiedOn <= input.MaxModifiedOnFilter)
                        .WhereIf(input.MinCreatedByFilter != null, e => e.CreatedBy >= input.MinCreatedByFilter)
                        .WhereIf(input.MaxCreatedByFilter != null, e => e.CreatedBy <= input.MaxCreatedByFilter)
                        .WhereIf(input.MinLastModifiedByFilter != null, e => e.LastModifiedBy >= input.MinLastModifiedByFilter)
                        .WhereIf(input.MaxLastModifiedByFilter != null, e => e.LastModifiedBy <= input.MaxLastModifiedByFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PidInputMaskFilter), e => e.PidInputMask == input.PidInputMaskFilter)
                        .WhereIf(input.RequiredFilter.HasValue && input.RequiredFilter > -1, e => (input.RequiredFilter == 1 && e.Required) || (input.RequiredFilter == 0 && !e.Required));

            var query = (from o in filteredPidTypes
                         select new GetPidTypeForViewDto()
                         {
                             PidType = new PidTypeDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 MaskPid = o.MaskPid,
                                 CreatedOn = o.CreatedOn,
                                 ModifiedOn = o.ModifiedOn,
                                 CreatedBy = o.CreatedBy,
                                 LastModifiedBy = o.LastModifiedBy,
                                 IsActive = o.IsActive,
                                 PidInputMask = o.PidInputMask,
                                 Required = o.Required,
                                 Id = o.Id
                             }
                         });

            var pidTypeListDtos = await query.ToListAsync();

            return _pidTypesExcelExporter.ExportToFile(pidTypeListDtos);
        }

        //private async Task VerifyPidTypes()
        //{
        //    if (_pidTypeRepository.GetAll().Any()) return;

        //    using (CurrentUnitOfWork.DisableFilter())
        //    {
        //        CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
        //        var _HostPidTypes = new List<PidType>();

        //    }

        //    if (!_context.PidTypes.IgnoreQueryFilters().Any(r => r.TenantId == args.NewTenantId))
        //    {
        //        // create default pids from host
        //        var _HostPidTypes = new List<PidType>();
        //        if (!_context.PidTypes.IgnoreQueryFilters().Any(r => r.TenantId == null))
        //        {
        //            // host doesn't have defaults - create them.
        //            _HostPidTypes = new List<PidType>()
        //            {
        //                new PidType(){Name="SSN",Description="Social Security Number",MaskPid=true, CreatedBy = 1, CreatedOn = DateTime.Now, IsActive = true, PidRegex = "", TenantId = null},
        //                new PidType(){Name="DL",Description="Driver's License",MaskPid=false, CreatedBy = 1, CreatedOn = DateTime.Now, IsActive = true, PidRegex = "", TenantId = null},
        //                new PidType(){Name="Passport",Description="Passport Number",MaskPid=false, CreatedBy = 1, CreatedOn = DateTime.Now, IsActive = true, PidRegex = "", TenantId = null},
        //                new PidType(){Name="StudentID",Description="Student Id",MaskPid=false, CreatedBy = 1, CreatedOn = DateTime.Now, IsActive = true, PidRegex = "", TenantId = null},
        //                //new PidType(){Name="StudentID",Description="Student Id",MaskPid=false, CreatedBy = 1, CreatedOn = DateTime.Now, IsActive = true, PidRegex = "", TenantId = null},

        //            };
        //            _HostPidTypes.ForEach(p =>
        //            {
        //                _context.PidTypes.Add(p);
        //            });
        //            _context.SaveChanges();
        //        }
        //        _HostPidTypes.ForEach(p =>
        //        {
        //            _context.PidTypes.Add(new PidType()
        //            {
        //                Name = p.Name,
        //                Description = p.Description,
        //                MaskPid = p.MaskPid,
        //                CreatedBy = p.CreatedBy,
        //                CreatedOn = DateTime.Now,
        //                IsActive = p.IsActive,
        //                PidRegex = p.PidRegex,
        //                TenantId = args.NewTenantId
        //            });
        //        });
        //        _context.SaveChanges();

        //    }
        //}


    }
}