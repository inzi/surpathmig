(function ($) {
    app.modals.CreateOrEditSurpathServiceModal = function () {
        var _surpathServicesService = abp.services.app.surpathServices;

        var _modalManager;
        var _modalData;
        var _$surpathServiceInformationForm = null;

        var _SurpathServicetenantDepartmentLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/SurpathServices/TenantDepartmentLookupTableModal',
            scriptUrl:
                abp.appPath +
                'view-resources/Areas/App/Views/SurpathServices/_SurpathServiceTenantDepartmentLookupTableModal.js',
            modalClass: 'TenantDepartmentLookupTableModal',
        });
        var _SurpathServicecohortLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/SurpathServices/CohortLookupTableModal',
            scriptUrl:
                abp.appPath + 'view-resources/Areas/App/Views/SurpathServices/_SurpathServiceCohortLookupTableModal.js',
            modalClass: 'CohortLookupTableModal',
        });
        var _SurpathServiceuserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/SurpathServices/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/SurpathServices/_SurpathServiceUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal',
        });
        var _SurpathServicerecordCategoryRuleLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/SurpathServices/RecordCategoryRuleLookupTableModal',
            scriptUrl:
                abp.appPath +
                'view-resources/Areas/App/Views/SurpathServices/_SurpathServiceRecordCategoryRuleLookupTableModal.js',
            modalClass: 'RecordCategoryRuleLookupTableModal',
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _modalData = _modalManager.getArgs();
            console.log('createOrEditModal', _modalData);
            //debugger;
            var modal = _modalManager.getModal();

            modal.find('.date-picker').daterangepicker({
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L',
            });

            _$surpathServiceInformationForm = _modalManager.getModal().find('form[name=SurpathServiceInformationsForm]');
            _$surpathServiceInformationForm.validate();
        };

        $('#OpenTenantDepartmentLookupTableButton').click(function () {
            var surpathService = _$surpathServiceInformationForm.serializeFormToObject();

            _SurpathServicetenantDepartmentLookupTableModal.open(
                { id: surpathService.tenantDepartmentId, displayName: surpathService.tenantDepartmentName, tenantId: _modalData.tenantId },
                function (data) {
                    _$surpathServiceInformationForm.find('input[name=tenantDepartmentName]').val(data.displayName);
                    _$surpathServiceInformationForm.find('input[name=tenantDepartmentId]').val(data.id);
                }
            );
        });

        $('#ClearTenantDepartmentNameButton').click(function () {
            _$surpathServiceInformationForm.find('input[name=tenantDepartmentName]').val('');
            _$surpathServiceInformationForm.find('input[name=tenantDepartmentId]').val('');
        });

        $('#OpenCohortLookupTableButton').click(function () {
            var surpathService = _$surpathServiceInformationForm.serializeFormToObject();

            _SurpathServicecohortLookupTableModal.open(
                { id: surpathService.cohortId, displayName: surpathService.cohortName },
                function (data) {
                    _$surpathServiceInformationForm.find('input[name=cohortName]').val(data.displayName);
                    _$surpathServiceInformationForm.find('input[name=cohortId]').val(data.id);
                }
            );
        });

        $('#ClearCohortNameButton').click(function () {
            _$surpathServiceInformationForm.find('input[name=cohortName]').val('');
            _$surpathServiceInformationForm.find('input[name=cohortId]').val('');
        });

        $('#OpenUserLookupTableButton').click(function () {
            var surpathService = _$surpathServiceInformationForm.serializeFormToObject();

            _SurpathServiceuserLookupTableModal.open(
                { id: surpathService.userId, displayName: surpathService.userName },
                function (data) {
                    _$surpathServiceInformationForm.find('input[name=userName]').val(data.displayName);
                    _$surpathServiceInformationForm.find('input[name=userId]').val(data.id);
                }
            );
        });

        $('#ClearUserNameButton').click(function () {
            _$surpathServiceInformationForm.find('input[name=userName]').val('');
            _$surpathServiceInformationForm.find('input[name=userId]').val('');
        });

        $('#OpenRecordCategoryRuleLookupTableButton').click(function () {
            var surpathService = _$surpathServiceInformationForm.serializeFormToObject();

            _SurpathServicerecordCategoryRuleLookupTableModal.open(
                { id: surpathService.recordCategoryRuleId, displayName: surpathService.recordCategoryRuleName },
                function (data) {
                    _$surpathServiceInformationForm.find('input[name=recordCategoryRuleName]').val(data.displayName);
                    _$surpathServiceInformationForm.find('input[name=recordCategoryRuleId]').val(data.id);
                }
            );
        });

        $('#ClearRecordCategoryRuleNameButton').click(function () {
            _$surpathServiceInformationForm.find('input[name=recordCategoryRuleName]').val('');
            _$surpathServiceInformationForm.find('input[name=recordCategoryRuleId]').val('');
        });

        this.save = function () {
            if (!_$surpathServiceInformationForm.valid()) {
                return;
            }
            if (
                $('#SurpathService_TenantDepartmentId').prop('required') &&
                $('#SurpathService_TenantDepartmentId').val() == ''
            ) {
                abp.message.error(app.localize('{0}IsRequired', app.localize('TenantDepartment')));
                return;
            }
            if ($('#SurpathService_CohortId').prop('required') && $('#SurpathService_CohortId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Cohort')));
                return;
            }
            if ($('#SurpathService_UserId').prop('required') && $('#SurpathService_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }
            if (
                $('#SurpathService_RecordCategoryRuleId').prop('required') &&
                $('#SurpathService_RecordCategoryRuleId').val() == ''
            ) {
                abp.message.error(app.localize('{0}IsRequired', app.localize('RecordCategoryRule')));
                return;
            }

            var surpathService = _$surpathServiceInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _surpathServicesService
                .createOrEdit(surpathService)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.createOrEditSurpathServiceModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);