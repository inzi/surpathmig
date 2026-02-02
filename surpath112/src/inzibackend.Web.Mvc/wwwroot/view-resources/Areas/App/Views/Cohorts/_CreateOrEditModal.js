(function ($) {
    app.modals.CreateOrEditCohortModal = function () {
        var _cohortsService = abp.services.app.cohorts;

        var _modalManager;
        var _$cohortInformationForm = null;

        var _CohorttenantDepartmentLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Cohorts/TenantDepartmentLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Cohorts/_CohortTenantDepartmentLookupTableModal.js',
            modalClass: 'TenantDepartmentLookupTableModal',
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-time-picker').daterangepicker({
                timePicker: true,
                singleDatePicker: true,
                startDate: moment().startOf('minute'),
                locale: {
                    format: 'MM/DD/YYYY hh:mm A',
                },
            });

            _$cohortInformationForm = _modalManager.getModal().find('form[name=CohortInformationsForm]');
            _$cohortInformationForm.validate();
        };

        $('#OpenTenantDepartmentLookupTableButton').click(function () {
            var cohort = _$cohortInformationForm.serializeFormToObject();

            _CohorttenantDepartmentLookupTableModal.open(
                { id: cohort.tenantDepartmentId, displayName: cohort.tenantDepartmentName },
                function (data) {
                    _$cohortInformationForm.find('input[name=tenantDepartmentName]').val(data.displayName);
                    _$cohortInformationForm.find('input[name=tenantDepartmentId]').val(data.id);
                }
            );
        });

        $('#ClearTenantDepartmentNameButton').click(function () {
            _$cohortInformationForm.find('input[name=tenantDepartmentName]').val('');
            _$cohortInformationForm.find('input[name=tenantDepartmentId]').val('');
        });

        this.save = function () {
            if (!_$cohortInformationForm.valid()) {
                return;
            }
            if ($('#Cohort_TenantDepartmentId').prop('required') && $('#Cohort_TenantDepartmentId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('TenantDepartment')));
                return;
            }

            var cohort = _$cohortInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _cohortsService
                .createOrEdit(cohort)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.createOrEditCohortModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);
