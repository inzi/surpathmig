using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Localization;
using Abp.ObjectMapping;
using Abp.UI;
using inzibackend.Authorization.Roles;
using inzibackend.Authorization.Users;
using inzibackend.MultiTenancy.Dto;
using inzibackend.MultiTenancy.Importing.Dto;
using inzibackend.Notifications;
using inzibackend.Storage;
using inzibackend.Surpath;
using Microsoft.AspNetCore.Identity;

namespace inzibackend.MultiTenancy.Importing
{
    public class ImportTenantUsersToExcelJob : AsyncBackgroundJob<ImportTenantUsersFromExcelJobArgs>, ITransientDependency
    {
        private readonly RoleManager _roleManager;
        private readonly ITenantUserListExcelDataReader _tenantUserListExcelDataReader;
        private readonly IInvalidTenantUserExporter _invalidTenantUserExporter;
        private readonly IUserPolicy _userPolicy;
        private readonly IEnumerable<IPasswordValidator<User>> _passwordValidators;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IObjectMapper _objectMapper;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<TenantDepartment, Guid> _tenantDepartmentRepository;
        private readonly IRepository<Cohort, Guid> _cohortRepository;
        private readonly IRepository<DepartmentUser, Guid> _departmentUserRepository;
        private readonly IRepository<CohortUser, Guid> _cohortUserRepository;
        private readonly IRepository<UserPid, Guid> _userPidRepository;
        private readonly IRepository<PidType, Guid> _pidTypeRepository;
        public UserManager UserManager { get; set; }

        public ImportTenantUsersToExcelJob(
            RoleManager roleManager,
            ITenantUserListExcelDataReader tenantUserListExcelDataReader,
            IInvalidTenantUserExporter invalidTenantUserExporter,
            IUserPolicy userPolicy,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            IPasswordHasher<User> passwordHasher,
            IAppNotifier appNotifier,
            IBinaryObjectManager binaryObjectManager,
            IObjectMapper objectMapper,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<TenantDepartment, Guid> tenantDepartmentRepository,
            IRepository<Cohort, Guid> cohortRepository,
            IRepository<DepartmentUser, Guid> departmentUserRepository,
            IRepository<CohortUser, Guid> cohortUserRepository,
            IRepository<UserPid, Guid> userPidRepository,
            IRepository<PidType, Guid> pidTypeRepository)
        {
            _roleManager = roleManager;
            _tenantUserListExcelDataReader = tenantUserListExcelDataReader;
            _invalidTenantUserExporter = invalidTenantUserExporter;
            _userPolicy = userPolicy;
            _passwordValidators = passwordValidators;
            _passwordHasher = passwordHasher;
            _appNotifier = appNotifier;
            _binaryObjectManager = binaryObjectManager;
            _objectMapper = objectMapper;
            _unitOfWorkManager = unitOfWorkManager;
            _tenantDepartmentRepository = tenantDepartmentRepository;
            _cohortRepository = cohortRepository;
            _departmentUserRepository = departmentUserRepository;
            _cohortUserRepository = cohortUserRepository;
            _userPidRepository = userPidRepository;
            _pidTypeRepository = pidTypeRepository;
        }

        public override async Task ExecuteAsync(ImportTenantUsersFromExcelJobArgs args)
        {
            var users = await GetUserListFromExcelOrNullAsync(args);
            if (users == null || !users.Any())
            {
                await SendInvalidExcelNotificationAsync(args);
                return;
            }

            await CreateUsersAsync(args, users);
        }

        private async Task<List<ImportTenantUserDto>> GetUserListFromExcelOrNullAsync(ImportTenantUsersFromExcelJobArgs args)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                using (CurrentUnitOfWork.SetTenantId(args.TenantId))
                {
                    try
                    {
                        var file = await _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId);
                        return _tenantUserListExcelDataReader.GetTenantUsersFromExcel(file.Bytes);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                    finally
                    {
                        await uow.CompleteAsync();
                    }
                }
            }
        }

        private async Task CreateUsersAsync(ImportTenantUsersFromExcelJobArgs args, List<ImportTenantUserDto> users)
        {
            var invalidUsers = new List<ImportTenantUserDto>();

            foreach (var user in users)
            {
                using (var uow = _unitOfWorkManager.Begin())
                {
                    using (CurrentUnitOfWork.SetTenantId(args.TenantId))
                    {
                        if (user.CanBeImported())
                        {
                            try
                            {
                                // Validate department and cohort before creating user
                                await ValidateDepartmentAndCohortAsync(user, args.TenantId);

                                if (!user.CanBeImported())
                                {
                                    invalidUsers.Add(user);
                                }
                                else
                                {
                                    await CreateUserWithAssociationsAsync(user, args.TenantId);
                                }
                            }
                            catch (UserFriendlyException exception)
                            {
                                user.Exception = exception.Message;
                                invalidUsers.Add(user);
                            }
                            catch (Exception exception)
                            {
                                user.Exception = exception.ToString();
                                invalidUsers.Add(user);
                            }
                        }
                        else
                        {
                            invalidUsers.Add(user);
                        }
                    }

                    await uow.CompleteAsync();
                }
            }

            using (var uow = _unitOfWorkManager.Begin())
            {
                using (CurrentUnitOfWork.SetTenantId(args.TenantId))
                {
                    await ProcessImportUsersResultAsync(args, invalidUsers);
                }

                await uow.CompleteAsync();
            }
        }

        private async Task ValidateDepartmentAndCohortAsync(ImportTenantUserDto user, int tenantId)
        {
            Guid? departmentId = null;
            Guid? cohortId = null;

            // Validate and get department
            if (!string.IsNullOrWhiteSpace(user.DepartmentName))
            {
                var department = await _tenantDepartmentRepository.FirstOrDefaultAsync(
                    d => d.TenantId == tenantId && d.Name.ToLower() == user.DepartmentName.ToLower()
                );

                if (department == null)
                {
                    user.Exception = $"Department '{user.DepartmentName}' not found in tenant.";
                    return;
                }

                departmentId = department.Id;
            }

            // Validate and get cohort
            if (!string.IsNullOrWhiteSpace(user.CohortName))
            {
                var cohort = await _cohortRepository.FirstOrDefaultAsync(
                    c => c.TenantId == tenantId && c.Name.ToLower() == user.CohortName.ToLower()
                );

                if (cohort == null)
                {
                    user.Exception = $"Cohort '{user.CohortName}' not found in tenant.";
                    return;
                }

                cohortId = cohort.Id;

                // If only cohort specified, auto-determine department
                if (!departmentId.HasValue && cohort.TenantDepartmentId.HasValue)
                {
                    departmentId = cohort.TenantDepartmentId.Value;
                }
                // If both specified, validate cohort belongs to department
                else if (departmentId.HasValue && cohort.TenantDepartmentId.HasValue && cohort.TenantDepartmentId.Value != departmentId.Value)
                {
                    user.Exception = $"Cohort '{user.CohortName}' does not belong to department '{user.DepartmentName}'.";
                    return;
                }
            }

            // Store the validated IDs in a temporary property (we'll use a dictionary)
            user.Exception = null; // Clear any previous errors

            // Store IDs in temp storage (we'll use tag-along properties)
            // Note: We'll pass these through as a tuple to CreateUserWithAssociationsAsync
        }

        private async Task CreateUserWithAssociationsAsync(ImportTenantUserDto input, int tenantId)
        {
            // Create the user first
            var userId = await CreateUserAsync(input, tenantId);

            // Now create associations
            Guid? departmentId = null;
            Guid? cohortId = null;

            // Re-lookup department
            if (!string.IsNullOrWhiteSpace(input.DepartmentName))
            {
                var department = await _tenantDepartmentRepository.FirstOrDefaultAsync(
                    d => d.TenantId == tenantId && d.Name.ToLower() == input.DepartmentName.ToLower()
                );
                departmentId = department?.Id;
            }

            // Re-lookup cohort
            if (!string.IsNullOrWhiteSpace(input.CohortName))
            {
                var cohort = await _cohortRepository.FirstOrDefaultAsync(
                    c => c.TenantId == tenantId && c.Name.ToLower() == input.CohortName.ToLower()
                );
                cohortId = cohort?.Id;

                // Auto-determine department from cohort if not specified
                if (!departmentId.HasValue && cohort?.TenantDepartmentId.HasValue == true)
                {
                    departmentId = cohort.TenantDepartmentId.Value;
                }
            }

            // Create DepartmentUser association
            if (departmentId.HasValue)
            {
                var departmentUser = new DepartmentUser
                {
                    TenantId = tenantId,
                    UserId = userId,
                    TenantDepartmentId = departmentId.Value
                };

                await _departmentUserRepository.InsertAsync(departmentUser);
            }

            // Create CohortUser association
            if (cohortId.HasValue)
            {
                var cohortUser = new CohortUser
                {
                    TenantId = tenantId,
                    UserId = userId,
                    CohortId = cohortId.Value
                };

                await _cohortUserRepository.InsertAsync(cohortUser);
            }

            // Create SSN UserPid if provided
            if (!string.IsNullOrWhiteSpace(input.SSN))
            {
                // Look up SSN PidType
                var ssnPidType = await _pidTypeRepository.FirstOrDefaultAsync(
                    pt => pt.TenantId == tenantId && pt.Name.ToLower() == "ssn"
                );

                if (ssnPidType != null)
                {
                    var userPid = new UserPid
                    {
                        TenantId = tenantId,
                        UserId = userId,
                        PidTypeId = ssnPidType.Id,
                        Pid = input.SSN,
                        Validated = false
                    };

                    await _userPidRepository.InsertAsync(userPid);
                }
            }
        }

        private async Task<long> CreateUserAsync(ImportTenantUserDto input, int tenantId)
        {
            await _userPolicy.CheckMaxUserCountAsync(tenantId);

            var user = new User
            {
                TenantId = tenantId,
                Name = input.Name,
                Surname = input.Surname,
                UserName = input.UserName,
                EmailAddress = input.EmailAddress,
                PhoneNumber = input.PhoneNumber,
                IsActive = true,
                Address = input.Address,
                SuiteApt = input.SuiteApt,
                City = input.City,
                State = input.State,
                Zip = input.Zip
            };

            // Parse DateOfBirth if provided
            if (!string.IsNullOrWhiteSpace(input.DateOfBirth))
            {
                if (DateTime.TryParse(input.DateOfBirth, out DateTime dob))
                {
                    user.DateOfBirth = dob;
                }
            }

            // Hash password if provided, otherwise generate a random one
            string passwordToUse = input.Password;
            if (string.IsNullOrWhiteSpace(passwordToUse))
            {
                // Generate random password if none provided
                passwordToUse = await UserManager.CreateRandomPassword();
            }

            await UserManager.InitializeOptionsAsync(tenantId);
            foreach (var validator in _passwordValidators)
            {
                (await validator.ValidateAsync(UserManager, user, passwordToUse)).CheckErrors();
            }

            user.Password = _passwordHasher.HashPassword(user, passwordToUse);

            // Assign roles
            user.Roles = new List<UserRole>();
            var roleList = _roleManager.Roles.ToList();

            foreach (var roleName in input.AssignedRoleNames ?? new string[0])
            {
                var correspondingRoleName = GetRoleNameFromDisplayName(roleName, roleList);
                if (!string.IsNullOrEmpty(correspondingRoleName))
                {
                    var role = await _roleManager.GetRoleByNameAsync(correspondingRoleName);
                    user.Roles.Add(new UserRole(tenantId, user.Id, role.Id));
                }
            }

            (await UserManager.CreateAsync(user)).CheckErrors();

            return user.Id;
        }

        private async Task ProcessImportUsersResultAsync(ImportTenantUsersFromExcelJobArgs args, List<ImportTenantUserDto> invalidUsers)
        {
            if (invalidUsers.Any())
            {
                var file = _invalidTenantUserExporter.ExportToFile(invalidUsers);
                await _appNotifier.SomeUsersCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName);
            }
            else
            {
                await _appNotifier.SendMessageAsync(
                    args.User,
                    new LocalizableString("AllUsersSuccessfullyImportedFromExcel",
                        inzibackendConsts.LocalizationSourceName),
                    null,
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private async Task SendInvalidExcelNotificationAsync(ImportTenantUsersFromExcelJobArgs args)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                using (CurrentUnitOfWork.SetTenantId(args.TenantId))
                {
                    await _appNotifier.SendMessageAsync(
                        args.User,
                        new LocalizableString(
                            "FileCantBeConvertedToUserList",
                            inzibackendConsts.LocalizationSourceName
                        ),
                        null,
                        Abp.Notifications.NotificationSeverity.Warn);
                }

                await uow.CompleteAsync();
            }
        }

        private string GetRoleNameFromDisplayName(string displayName, List<Role> roleList)
        {
            return roleList.FirstOrDefault(
                r => r.DisplayName?.ToLowerInvariant() == displayName?.ToLowerInvariant()
            )?.Name;
        }
    }
}
