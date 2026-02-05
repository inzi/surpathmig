(function ($) {
    app.modals.CreateOrEditTenantSurpathServiceModal = function () {
        var _tenantSurpathServicesService = abp.services.app.tenantSurpathServices;

        var _modalManager;
        var _modalData;

        var _$tenantSurpathServiceInformationForm = null;

        var _workingTenantId = null;
        var _workingDeptId = null;

        
        var _TenantSurpathServicesurpathServiceLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/TenantSurpathServices/SurpathServiceLookupTableModal',
            scriptUrl:
                abp.appPath +
                'view-resources/Areas/App/Views/TenantSurpathServices/_TenantSurpathServiceSurpathServiceLookupTableModal.js',
            modalClass: 'SurpathServiceLookupTableModal',
        });
        var _TenantSurpathServicetenantDepartmentLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/TenantSurpathServices/TenantDepartmentLookupTableModal',
            scriptUrl:
                abp.appPath +
                'view-resources/Areas/App/Views/TenantSurpathServices/_TenantSurpathServiceTenantDepartmentLookupTableModal.js',
            modalClass: 'TenantDepartmentLookupTableModal',
        });
        var _TenantSurpathServicecohortLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/TenantSurpathServices/CohortLookupTableModal',
            scriptUrl:
                abp.appPath +
                'view-resources/Areas/App/Views/TenantSurpathServices/_TenantSurpathServiceCohortLookupTableModal.js',
            modalClass: 'CohortLookupTableModal',
        });
        var _TenantSurpathServiceuserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/TenantSurpathServices/UserLookupTableModal',
            scriptUrl:
                abp.appPath +
                'view-resources/Areas/App/Views/TenantSurpathServices/_TenantSurpathServiceUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal',
        });
        var _TenantSurpathServicerecordCategoryRuleLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/TenantSurpathServices/RecordCategoryRuleLookupTableModal',
            scriptUrl:
                abp.appPath +
                'view-resources/Areas/App/Views/TenantSurpathServices/_TenantSurpathServiceRecordCategoryRuleLookupTableModal.js',
            modalClass: 'RecordCategoryRuleLookupTableModal',
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _modalData = _modalManager.getArgs();
            _workingTenantId = _modalData.tenantId;
            console.log('createOrEditModal TSPS', _modalData);
            SetTenantIdFilterValue();  

            var modal = _modalManager.getModal();
            modal.find('.date-picker').daterangepicker({
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L',
            });

            _$tenantSurpathServiceInformationForm = _modalManager
                .getModal()
                .find('form[name=TenantSurpathServiceInformationsForm]');
            _$tenantSurpathServiceInformationForm.validate();
            
            // If surpathServiceId is provided, set it and populate name/description
            if (_modalData.surpathServiceId) {
                _$tenantSurpathServiceInformationForm.find('input[name=surpathServiceId]').val(_modalData.surpathServiceId);
                
                // Get service details to populate name and description
                abp.services.app.tenantSurpathServices.getAvailableSurpathServices()
                    .done(function(services) {
                        var service = services.find(s => s.id === _modalData.surpathServiceId);
                        if (service) {
                            _$tenantSurpathServiceInformationForm.find('input[name=name]').val(service.name);
                            _$tenantSurpathServiceInformationForm.find('input[name=description]').val(service.description);
                        }
                    });
            }
        };

        $('#OpenSurpathServiceLookupTableButton').click(function () {
            var tenantSurpathService = _$tenantSurpathServiceInformationForm.serializeFormToObject();

            _TenantSurpathServicesurpathServiceLookupTableModal.open(
                { id: tenantSurpathService.surpathServiceId, displayName: tenantSurpathService.surpathServiceName, tenantId: _modalData.tenantId },
                function (data) {
                    _$tenantSurpathServiceInformationForm.find('input[name=surpathServiceName]').val(data.displayName);
                    _$tenantSurpathServiceInformationForm.find('input[name=surpathServiceId]').val(data.id);
                }
            );
        });

        $('#ClearSurpathServiceNameButton').click(function () {
            _$tenantSurpathServiceInformationForm.find('input[name=surpathServiceName]').val('');
            _$tenantSurpathServiceInformationForm.find('input[name=surpathServiceId]').val('');
        });

        $('#OpenTenantDepartmentLookupTableButton').click(function () {
            var tenantSurpathService = _$tenantSurpathServiceInformationForm.serializeFormToObject();
            var _payload = { id: tenantSurpathService.tenantDepartmentId, displayName: tenantSurpathService.tenantDepartmentName, tenantId: _workingTenantId }
            console.log('OpenTenantDepartmentLookupTableButton tss', _modalData,
                { id: tenantSurpathService.tenantDepartmentId, displayName: tenantSurpathService.tenantDepartmentName, tenantId: _workingTenantId }
            );
            _TenantSurpathServicetenantDepartmentLookupTableModal.open(
                { id: tenantSurpathService.tenantDepartmentId, displayName: tenantSurpathService.tenantDepartmentName, tenantId: _modalData.tenantId },
                function (data) {
                    _workingTenantId = data.tenantInfoDto.id;
                    _workingDeptId = data.id;
                    setModelSubtitle(data.tenantInfoDto.name);
                    _$tenantSurpathServiceInformationForm.find('input[name=tenantDepartmentName]').val(data.displayName);
                    _$tenantSurpathServiceInformationForm.find('input[name=tenantDepartmentId]').val(data.id);
                }
            );
        });

        $('#ClearTenantDepartmentNameButton').click(function () {
            _$tenantSurpathServiceInformationForm.find('input[name=tenantDepartmentName]').val('');
            _$tenantSurpathServiceInformationForm.find('input[name=tenantDepartmentId]').val('');
            if (_$tenantSurpathServiceInformationForm.find('input[name=cohortName]').val() == '') {
                _workingTenantId = null;
                setModelSubtitle('');
            };
        });

        $('#OpenCohortLookupTableButton').click(function () {
            var tenantSurpathService = _$tenantSurpathServiceInformationForm.serializeFormToObject();

            _TenantSurpathServicecohortLookupTableModal.open(
                { id: tenantSurpathService.cohortId, displayName: tenantSurpathService.cohortName, tenantId: _modalData.tenantId || _workingTenantId },
                function (data) {
                    _workingTenantId = data.tenantInfoDto.id;
                    setModelSubtitle(data.tenantInfoDto.name);
                    _$tenantSurpathServiceInformationForm.find('input[name=cohortName]').val(data.displayName);
                    _$tenantSurpathServiceInformationForm.find('input[name=cohortId]').val(data.id);
                }
            );
        });

        $('#ClearCohortNameButton').click(function () {
            _$tenantSurpathServiceInformationForm.find('input[name=cohortName]').val('');
            _$tenantSurpathServiceInformationForm.find('input[name=cohortId]').val('');
            if (_$tenantSurpathServiceInformationForm.find('input[name=tenantDepartmentName]').val() == '') {
                _workingTenantId = null;
                setModelSubtitle('');
            };
        });

        $('#OpenUserLookupTableButton').click(function () {
            var tenantSurpathService = _$tenantSurpathServiceInformationForm.serializeFormToObject();

            _TenantSurpathServiceuserLookupTableModal.open(
                { id: tenantSurpathService.userId, displayName: tenantSurpathService.userName, tenantId: _modalData.tenantId },
                function (data) {
                    _$tenantSurpathServiceInformationForm.find('input[name=userName]').val(data.displayName);
                    _$tenantSurpathServiceInformationForm.find('input[name=userId]').val(data.id);
                }
            );
        });

        $('#ClearUserNameButton').click(function () {
            _$tenantSurpathServiceInformationForm.find('input[name=userName]').val('');
            _$tenantSurpathServiceInformationForm.find('input[name=userId]').val('');
        });

        $('#OpenRecordCategoryRuleLookupTableButton').click(function () {
            var tenantSurpathService = _$tenantSurpathServiceInformationForm.serializeFormToObject();

            _TenantSurpathServicerecordCategoryRuleLookupTableModal.open(
                { id: tenantSurpathService.recordCategoryRuleId, displayName: tenantSurpathService.recordCategoryRuleName },
                function (data) {
                    _$tenantSurpathServiceInformationForm.find('input[name=recordCategoryRuleName]').val(data.displayName);
                    _$tenantSurpathServiceInformationForm.find('input[name=recordCategoryRuleId]').val(data.id);
                }
            );
        });

        $('#ClearRecordCategoryRuleNameButton').click(function () {
            _$tenantSurpathServiceInformationForm.find('input[name=recordCategoryRuleName]').val('');
            _$tenantSurpathServiceInformationForm.find('input[name=recordCategoryRuleId]').val('');
        });

        this.save = function () {
            if (!_$tenantSurpathServiceInformationForm.valid()) {
                return;
            }
            if (
                $('#TenantSurpathService_SurpathServiceId').prop('required') &&
                $('#TenantSurpathService_SurpathServiceId').val() == ''
            ) {
                abp.message.error(app.localize('{0}IsRequired', app.localize('SurpathService')));
                return;
            }
            if (
                $('#TenantSurpathService_TenantDepartmentId').prop('required') &&
                $('#TenantSurpathService_TenantDepartmentId').val() == ''
            ) {
                abp.message.error(app.localize('{0}IsRequired', app.localize('TenantDepartment')));
                return;
            }
            if ($('#TenantSurpathService_CohortId').prop('required') && $('#TenantSurpathService_CohortId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Cohort')));
                return;
            }
            if ($('#TenantSurpathService_UserId').prop('required') && $('#TenantSurpathService_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }

            var tenantSurpathService = _$tenantSurpathServiceInformationForm.serializeFormToObject();
            
            // Add tenantId to the DTO if provided in modal data
            if (_modalData.tenantId) {
                tenantSurpathService.tenantId = _modalData.tenantId;
            }

            _modalManager.setBusy(true);
            _tenantSurpathServicesService
                .createOrEdit(tenantSurpathService)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.createOrEditTenantSurpathServiceModalSaved',tenantSurpathService);
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        function setModelSubtitle(data) {
            if (data == undefined || data == null || data == '') {
                $('.modal-subtitle').find('span').text('');
                return;
            }
            $('.modal-subtitle').find('span').text('('+data+')');
        }

        function SetTenantIdFilterValue() {
            var tenantId = $('#manage-tenant-surpath-services').find('#SelectedTenantId').val();
            if (tenantId == undefined || tenantId == null || tenantId == '') {
                $('#tenantId').val('');
            } else {
                $('#tenantId').val(tenantId);
            }
            console.log('SetTenantIdFilterValue tenantId: ' + tenantId);
        }
    };
})(jQuery);
