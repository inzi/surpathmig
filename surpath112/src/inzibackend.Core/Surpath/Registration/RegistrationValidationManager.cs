using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using inzibackend.Authorization.Users;
using inzibackend.Surpath;
using Microsoft.EntityFrameworkCore;

namespace inzibackend.Surpath.Registration
{
    public class RegistrationValidationManager : inzibackendDomainServiceBase, IRegistrationValidationManager
    {
        private readonly UserManager _userManager;
        private readonly IRepository<TenantDepartment, Guid> _tenantDepartmentRepository;
        private readonly IRepository<Cohort, Guid> _cohortRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IAbpSession _abpSession;

        public RegistrationValidationManager(
            UserManager userManager,
            IRepository<TenantDepartment, Guid> tenantDepartmentRepository,
            IRepository<Cohort, Guid> cohortRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IAbpSession abpSession)
        {
            _userManager = userManager;
            _tenantDepartmentRepository = tenantDepartmentRepository;
            _cohortRepository = cohortRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _abpSession = abpSession;
        }

        public async Task<RegistrationValidationResult> ValidateAsync(RegistrationValidationRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var tenantId = request.TenantId ?? _abpSession.TenantId;
            if (!tenantId.HasValue)
            {
                throw new UserFriendlyException(L("TenantNotSpecified"));
            }

            if (CurrentUnitOfWork != null)
            {
                using (CurrentUnitOfWork.SetTenantId(tenantId.Value))
                {
                    return await ValidateInternalAsync(request, tenantId.Value);
                }
            }

            using (var uow = _unitOfWorkManager.Begin())
            {
                using (_unitOfWorkManager.Current.SetTenantId(tenantId.Value))
                {
                    var result = await ValidateInternalAsync(request, tenantId.Value);
                    await uow.CompleteAsync();
                    return result;
                }
            }
        }

        public async Task EnsureValidAsync(RegistrationValidationRequest request)
        {
            var result = await ValidateAsync(request);
            if (result.IsValid)
            {
                return;
            }

            Logger.Warn($"Registration pre-validation failed. TenantId={request.TenantId ?? _abpSession.TenantId}, Email={request.EmailAddress}, UserName={request.UserName}, DeptId={request.TenantDepartmentId}, CohortId={request.CohortId}");

            var details = string.Join(" ", result.Errors);
            throw new UserFriendlyException(L("FormIsNotValidMessage"), details);
        }

        private async Task<RegistrationValidationResult> ValidateInternalAsync(RegistrationValidationRequest request, int tenantId)
        {
            var result = new RegistrationValidationResult();

            // Email checks
            if (request.EmailAddress.IsNullOrWhiteSpace())
            {
                var message = string.Format(L("{0}IsRequired"), L("EmailAddress"));
                result.EmailError = message;
                result.EmailAvailable = false;
                result.AddError(message);
            }
            else
            {
                var normalizedEmail = _userManager.NormalizeEmail(request.EmailAddress);
                if (normalizedEmail.IsNullOrWhiteSpace())
                {
                    normalizedEmail = request.EmailAddress.Trim().ToUpperInvariant();
                }

                var emailExists = await _userManager.Users
                    .Where(u => u.TenantId == tenantId && !u.IsDeleted && u.NormalizedEmailAddress == normalizedEmail)
                    .AnyAsync();

                if (emailExists)
                {
                    var message = L("EmailAlreadyRegistered");
                    result.EmailError = message;
                    result.EmailAvailable = false;
                    result.AddError(message);
                }
                else
                {
                    result.EmailAvailable = true;
                    result.EmailError = null;
                }
            }

            // Username checks
            if (request.UserName.IsNullOrWhiteSpace())
            {
                var message = string.Format(L("{0}IsRequired"), L("UserName"));
                result.UsernameError = message;
                result.UsernameAvailable = false;
                result.AddError(message);
            }
            else
            {
                var normalizedUserName = _userManager.NormalizeName(request.UserName);
                if (normalizedUserName.IsNullOrWhiteSpace())
                {
                    normalizedUserName = request.UserName.Trim().ToUpperInvariant();
                }

                var usernameExists = await _userManager.Users
                    .Where(u => u.TenantId == tenantId && !u.IsDeleted && u.NormalizedUserName == normalizedUserName)
                    .AnyAsync();

                if (usernameExists)
                {
                    var message = L("UsernameAlreadyRegistered");
                    result.UsernameError = message;
                    result.UsernameAvailable = false;
                    result.AddError(message);
                }
                else
                {
                    result.UsernameAvailable = true;
                    result.UsernameError = null;
                }
            }

            TenantDepartment department = null;
            if (!request.TenantDepartmentId.HasValue)
            {
                var message = string.Format(L("{0}IsRequired"), L("TenantDepartment"));
                result.DepartmentError = message;
                result.DepartmentValid = false;
                result.AddError(message);
            }
            else
            {
                department = await _tenantDepartmentRepository.FirstOrDefaultAsync(request.TenantDepartmentId.Value);
                if (department == null || department.TenantId != tenantId || !department.Active)
                {
                    var message = L("TenantDepartmentSelectionInvalid");
                    result.DepartmentError = message;
                    result.DepartmentValid = false;
                    result.AddError(message);
                }
                else
                {
                    result.DepartmentValid = true;
                    result.DepartmentError = null;
                }
            }

            if (!request.CohortId.HasValue)
            {
                var message = string.Format(L("{0}IsRequired"), L("Cohort"));
                result.CohortError = message;
                result.CohortValid = false;
                result.AddError(message);
            }
            else
            {
                var cohort = await _cohortRepository.FirstOrDefaultAsync(request.CohortId.Value);
                var cohortValid = cohort != null && cohort.TenantId == tenantId;
                if (!cohortValid)
                {
                    var message = L("CohortSelectionInvalid");
                    result.CohortError = message;
                    result.CohortValid = false;
                    result.AddError(message);
                }
                else if (request.TenantDepartmentId.HasValue && cohort.TenantDepartmentId.HasValue &&
                         cohort.TenantDepartmentId != request.TenantDepartmentId.Value)
                {
                    var message = L("CohortSelectionInvalid");
                    result.CohortError = message;
                    result.CohortValid = false;
                    result.AddError(message);
                }
                else
                {
                    result.CohortValid = true;
                    result.CohortError = null;
                }
            }

            return result;
        }
    }
}
