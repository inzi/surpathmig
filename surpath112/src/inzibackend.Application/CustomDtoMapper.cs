using inzibackend.Surpath.Dtos;
using inzibackend.Surpath;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.DynamicEntityProperties;
using Abp.EntityHistory;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using Abp.Webhooks;
using AutoMapper;
using IdentityServer4.Extensions;
using inzibackend.Auditing.Dto;
using inzibackend.Authorization.Accounts.Dto;
using inzibackend.Authorization.Delegation;
using inzibackend.Authorization.Permissions.Dto;
using inzibackend.Authorization.Roles;
using inzibackend.Authorization.Roles.Dto;
using inzibackend.Authorization.Users;
using inzibackend.Authorization.Users.Delegation.Dto;
using inzibackend.Authorization.Users.Dto;
using inzibackend.Authorization.Users.Importing.Dto;
using inzibackend.Authorization.Users.Profile.Dto;
using inzibackend.Chat;
using inzibackend.Chat.Dto;
using inzibackend.DynamicEntityProperties.Dto;
using inzibackend.Editions;
using inzibackend.Editions.Dto;
using inzibackend.Friendships;
using inzibackend.Friendships.Cache;
using inzibackend.Friendships.Dto;
using inzibackend.Localization.Dto;
using inzibackend.MultiTenancy;
using inzibackend.MultiTenancy.Dto;
using inzibackend.MultiTenancy.HostDashboard.Dto;
using inzibackend.MultiTenancy.Payments;
using inzibackend.MultiTenancy.Payments.Dto;
using inzibackend.Notifications.Dto;
using inzibackend.Organizations.Dto;
using inzibackend.Sessions.Dto;
using inzibackend.WebHooks.Dto;
using System.Collections.Generic;
using AutoMapper.EquivalencyExpression;
using System.Linq;
using inzibackend.Surpath.Dtos.LegalDocuments;

namespace inzibackend
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.AddCollectionMappers();

            configuration.CreateMap<CreateOrEditMedicalUnitDto, MedicalUnit>().ReverseMap();

            configuration.CreateMap<MedicalUnitDto, MedicalUnit>().ReverseMap();

            configuration.CreateMap<CreateOrEditRotationSlotDto, RotationSlot>().ReverseMap();

            configuration.CreateMap<RotationSlotDto, RotationSlot>().ReverseMap();
            configuration.CreateMap<CreateOrEditHospitalDto, Hospital>().ReverseMap();
            configuration.CreateMap<HospitalDto, Hospital>().ReverseMap();
            configuration.CreateMap<CreateOrEditWelcomemessageDto, Welcomemessage>().ReverseMap();
            configuration.CreateMap<WelcomemessageDto, Welcomemessage>().ReverseMap();
            configuration.CreateMap<CreateOrEditTenantSurpathServiceDto, TenantSurpathService>().ReverseMap();
            configuration.CreateMap<TenantSurpathServiceDto, TenantSurpathService>().ReverseMap();
            configuration.CreateMap<CreateOrEditLedgerEntryDetailDto, LedgerEntryDetail>().ReverseMap();
            configuration.CreateMap<LedgerEntryDetailDto, LedgerEntryDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditSurpathServiceDto, SurpathService>().ReverseMap();
            configuration.CreateMap<SurpathServiceDto, SurpathService>().ReverseMap();
            configuration.CreateMap<CreateOrEditLedgerEntryDto, LedgerEntry>().ReverseMap();
            configuration.CreateMap<LedgerEntryDto, LedgerEntry>().ReverseMap();
            configuration.CreateMap<CreateOrEditPidTypeDto, PidType>().ReverseMap();
            configuration.CreateMap<PidTypeDto, PidType>().ReverseMap();
            configuration.CreateMap<CreateOrEditUserPidDto, UserPid>().ReverseMap();
            configuration.CreateMap<UserPidDto, UserPid>().ReverseMap();
            configuration.CreateMap<CreateOrEditTenantDocumentDto, TenantDocument>().ReverseMap();
            configuration.CreateMap<TenantDocumentDto, TenantDocument>().ReverseMap();
            configuration.CreateMap<CreateOrEditTenantDepartmentUserDto, TenantDepartmentUser>().ReverseMap();
            configuration.CreateMap<TenantDepartmentUserDto, TenantDepartmentUser>().ReverseMap();
            configuration.CreateMap<CreateOrEditRecordStatusDto, RecordStatus>().ReverseMap();
            configuration.CreateMap<RecordStatusDto, RecordStatus>().ReverseMap();
            configuration.CreateMap<CreateOrEditRecordStateDto, RecordState>().ReverseMap();
            configuration.CreateMap<RecordStateDto, RecordState>().ReverseMap();
            configuration.CreateMap<CreateOrEditTenantDocumentCategoryDto, TenantDocumentCategory>().ReverseMap();
            configuration.CreateMap<TenantDocumentCategoryDto, TenantDocumentCategory>().ReverseMap();
            configuration.CreateMap<CreateOrEditDrugTestCategoryDto, DrugTestCategory>().ReverseMap();
            configuration.CreateMap<DrugTestCategoryDto, DrugTestCategory>().ReverseMap();
            configuration.CreateMap<CreateOrEditDeptCodeDto, DeptCode>().ReverseMap();
            configuration.CreateMap<DeptCodeDto, DeptCode>().ReverseMap();
            configuration.CreateMap<CreateOrEditDepartmentUserDto, DepartmentUser>().ReverseMap();
            configuration.CreateMap<DepartmentUserDto, DepartmentUser>().ReverseMap();
            configuration.CreateMap<CreateOrEditCohortUserDto, CohortUser>().ReverseMap();
            configuration.CreateMap<CohortUserDto, CohortUser>().ReverseMap();
            configuration.CreateMap<CreateOrEditCohortDto, Cohort>().ReverseMap();
            configuration.CreateMap<CohortDto, Cohort>().ReverseMap();
            configuration.CreateMap<CreateOrEditTenantDepartmentDto, TenantDepartment>().ReverseMap();
            configuration.CreateMap<TenantDepartmentDto, TenantDepartment>().ReverseMap();
            
            // Hierarchical pricing mappings
            configuration.CreateMap<TenantDepartment, DepartmentPricingDto>()
                .IncludeBase<TenantDepartment, TenantDepartmentDto>();
            configuration.CreateMap<Cohort, CohortPricingDto>()
                .IncludeBase<Cohort, CohortDto>();
            
            // Domain model to DTO mappings for hierarchical pricing
            configuration.CreateMap<HierarchicalPricing, HierarchicalPricingDto>();
            configuration.CreateMap<TenantPricing, TenantPricingDto>();
            configuration.CreateMap<DepartmentPricing, DepartmentPricingDto>();
            configuration.CreateMap<CohortPricing, CohortPricingDto>();
            configuration.CreateMap<UserPricing, UserPricingDto>();
            configuration.CreateMap<ServicePrice, ServicePriceDto>();
            configuration.CreateMap<CreateOrEditConfirmationValueDto, ConfirmationValue>().ReverseMap();
            configuration.CreateMap<ConfirmationValueDto, ConfirmationValue>().ReverseMap();
            configuration.CreateMap<CreateOrEditCodeTypeDto, CodeType>().ReverseMap();
            configuration.CreateMap<CodeTypeDto, CodeType>().ReverseMap();
            configuration.CreateMap<CreateOrEditRecordNoteDto, RecordNote>().ReverseMap();
            configuration.CreateMap<RecordNoteDto, RecordNote>().ReverseMap();
            configuration.CreateMap<CreateOrEditRecordCategoryDto, RecordCategory>().ReverseMap();
            configuration.CreateMap<RecordCategoryDto, RecordCategory>().ReverseMap();
            configuration.CreateMap<CreateOrEditRecordRequirementDto, RecordRequirement>().ReverseMap();
            configuration.CreateMap<RecordRequirementDto, RecordRequirement>().ReverseMap();
            configuration.CreateMap<CreateOrEditRecordCategoryRuleDto, RecordCategoryRule>().ReverseMap();
            configuration.CreateMap<RecordCategoryRuleDto, RecordCategoryRule>().ReverseMap();
            configuration.CreateMap<CreateOrEditRecordDto, Record>().ReverseMap();
            configuration.CreateMap<RecordDto, Record>().ReverseMap();
            configuration.CreateMap<CreateOrEditDrugPanelDto, DrugPanel>().ReverseMap();
            configuration.CreateMap<DrugPanelDto, DrugPanel>().ReverseMap();
            configuration.CreateMap<CreateOrEditPanelDto, Panel>().ReverseMap();
            configuration.CreateMap<PanelDto, Panel>().ReverseMap();
            configuration.CreateMap<CreateOrEditTestCategoryDto, TestCategory>().ReverseMap();
            configuration.CreateMap<TestCategoryDto, TestCategory>().ReverseMap();
            configuration.CreateMap<CreateOrEditDrugDto, Drug>().ReverseMap();
            configuration.CreateMap<DrugDto, Drug>().ReverseMap();
            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionCreateDto, SubscribableEdition>();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<SubscribableEdition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();

            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<ImportUserDto, User>();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();
            configuration.CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();

            //Webhooks
            configuration.CreateMap<WebhookSubscription, GetAllSubscriptionsOutput>();
            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOutput>()
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.WebhookName,
                    options => options.MapFrom(l => l.WebhookEvent.WebhookName))
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.Data,
                    options => options.MapFrom(l => l.WebhookEvent.Data));

            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOfWebhookEventOutput>();

            configuration.CreateMap<DynamicProperty, DynamicPropertyDto>().ReverseMap();
            configuration.CreateMap<DynamicPropertyValue, DynamicPropertyValueDto>().ReverseMap();
            configuration.CreateMap<DynamicEntityProperty, DynamicEntityPropertyDto>()
                .ForMember(dto => dto.DynamicPropertyName,
                    options => options.MapFrom(entity => entity.DynamicProperty.DisplayName.IsNullOrEmpty() ? entity.DynamicProperty.PropertyName : entity.DynamicProperty.DisplayName));
            configuration.CreateMap<DynamicEntityPropertyDto, DynamicEntityProperty>();

            configuration.CreateMap<DynamicEntityPropertyValue, DynamicEntityPropertyValueDto>().ReverseMap();

            //User Delegations
            configuration.CreateMap<CreateUserDelegationDto, UserDelegation>();

            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */

            configuration.CreateMap<SlotAvailableDay, SlotAvailableDayDto>().EqualityComparison((odto, o) => odto.Id == o.Id).ReverseMap();
            configuration.CreateMap<SlotRotationDay, SlotRotationDayDto>().EqualityComparison((odto, o) => odto.Id == o.Id).ReverseMap();

            configuration.CreateMap<TenantDepartment, OrganizationUnitDepartmentListDto>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            // User Purchase mappings
            configuration.CreateMap<CreateOrEditUserPurchaseDto, UserPurchase>().ReverseMap();
            configuration.CreateMap<UserPurchaseDto, UserPurchase>().ReverseMap();

            // Legal Document mappings
            configuration.CreateMap<LegalDocumentDto, LegalDocument>().ReverseMap();
            configuration.CreateMap<CreateOrUpdateLegalDocumentDto, LegalDocument>();
        }
    }
}