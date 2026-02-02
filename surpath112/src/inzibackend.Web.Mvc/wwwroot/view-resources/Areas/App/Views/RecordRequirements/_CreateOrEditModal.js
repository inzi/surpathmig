(function ($) {
  app.modals.CreateOrEditRecordRequirementModal = function () {
    var _recordRequirementsService = abp.services.app.recordRequirements;

    var _modalManager;
    var _$recordRequirementInformationForm = null;

    var _RecordRequirementtenantDepartmentLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RecordRequirements/TenantDepartmentLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/App/Views/RecordRequirements/_RecordRequirementTenantDepartmentLookupTableModal.js',
      modalClass: 'TenantDepartmentLookupTableModal',
    });
    var _RecordRequirementcohortLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RecordRequirements/CohortLookupTableModal',
      scriptUrl:
        abp.appPath + 'view-resources/Areas/App/Views/RecordRequirements/_RecordRequirementCohortLookupTableModal.js',
      modalClass: 'CohortLookupTableModal',
    });
    //var _RecordRequirementsurpathServiceLookupTableModal = new app.ModalManager({
    //  viewUrl: abp.appPath + 'App/RecordRequirements/SurpathServiceLookupTableModal',
    //  scriptUrl:
    //    abp.appPath +
    //    'view-resources/Areas/App/Views/RecordRequirements/_RecordRequirementSurpathServiceLookupTableModal.js',
    //  modalClass: 'SurpathServiceLookupTableModal',
    //});
    //var _RecordRequirementtenantSurpathServiceLookupTableModal = new app.ModalManager({
    //  viewUrl: abp.appPath + 'App/RecordRequirements/TenantSurpathServiceLookupTableModal',
    //  scriptUrl:
    //    abp.appPath +
    //    'view-resources/Areas/App/Views/RecordRequirements/_RecordRequirementTenantSurpathServiceLookupTableModal.js',
    //  modalClass: 'TenantSurpathServiceLookupTableModal',
    //});

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$recordRequirementInformationForm = _modalManager
        .getModal()
        .find('form[name=RecordRequirementInformationsForm]');
      _$recordRequirementInformationForm.validate();
    };

    $('#OpenTenantDepartmentLookupTableButton').click(function () {
      var recordRequirement = _$recordRequirementInformationForm.serializeFormToObject();

      _RecordRequirementtenantDepartmentLookupTableModal.open(
        { id: recordRequirement.tenantDepartmentId, displayName: recordRequirement.tenantDepartmentName },
        function (data) {
          _$recordRequirementInformationForm.find('input[name=tenantDepartmentName]').val(data.displayName);
          _$recordRequirementInformationForm.find('input[name=tenantDepartmentId]').val(data.id);
        }
      );
    });

    $('#ClearTenantDepartmentNameButton').click(function () {
      _$recordRequirementInformationForm.find('input[name=tenantDepartmentName]').val('');
      _$recordRequirementInformationForm.find('input[name=tenantDepartmentId]').val('');
    });

    $('#OpenCohortLookupTableButton').click(function () {
      var recordRequirement = _$recordRequirementInformationForm.serializeFormToObject();

      _RecordRequirementcohortLookupTableModal.open(
        { id: recordRequirement.cohortId, displayName: recordRequirement.cohortName },
        function (data) {
          _$recordRequirementInformationForm.find('input[name=cohortName]').val(data.displayName);
          _$recordRequirementInformationForm.find('input[name=cohortId]').val(data.id);
        }
      );
    });

    $('#ClearCohortNameButton').click(function () {
      _$recordRequirementInformationForm.find('input[name=cohortName]').val('');
      _$recordRequirementInformationForm.find('input[name=cohortId]').val('');
    });

    //$('#OpenSurpathServiceLookupTableButton').click(function () {
    //  var recordRequirement = _$recordRequirementInformationForm.serializeFormToObject();

    //  _RecordRequirementsurpathServiceLookupTableModal.open(
    //    { id: recordRequirement.surpathServiceId, displayName: recordRequirement.surpathServiceName },
    //    function (data) {
    //      _$recordRequirementInformationForm.find('input[name=surpathServiceName]').val(data.displayName);
    //      _$recordRequirementInformationForm.find('input[name=surpathServiceId]').val(data.id);
    //    }
    //  );
    //});

    //$('#ClearSurpathServiceNameButton').click(function () {
    //  _$recordRequirementInformationForm.find('input[name=surpathServiceName]').val('');
    //  _$recordRequirementInformationForm.find('input[name=surpathServiceId]').val('');
    //});

    //$('#OpenTenantSurpathServiceLookupTableButton').click(function () {
    //  var recordRequirement = _$recordRequirementInformationForm.serializeFormToObject();

    //  _RecordRequirementtenantSurpathServiceLookupTableModal.open(
    //    { id: recordRequirement.tenantSurpathServiceId, displayName: recordRequirement.tenantSurpathServiceName },
    //    function (data) {
    //      _$recordRequirementInformationForm.find('input[name=tenantSurpathServiceName]').val(data.displayName);
    //      _$recordRequirementInformationForm.find('input[name=tenantSurpathServiceId]').val(data.id);
    //    }
    //  );
    //});

    //$('#ClearTenantSurpathServiceNameButton').click(function () {
    //  _$recordRequirementInformationForm.find('input[name=tenantSurpathServiceName]').val('');
    //  _$recordRequirementInformationForm.find('input[name=tenantSurpathServiceId]').val('');
    //});

    this.save = function () {
      if (!_$recordRequirementInformationForm.valid()) {
        return;
      }
      if (
        $('#RecordRequirement_TenantDepartmentId').prop('required') &&
        $('#RecordRequirement_TenantDepartmentId').val() == ''
      ) {
        abp.message.error(app.localize('{0}IsRequired', app.localize('TenantDepartment')));
        return;
      }
      if ($('#RecordRequirement_CohortId').prop('required') && $('#RecordRequirement_CohortId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Cohort')));
        return;
      }
      //if (
      //  $('#RecordRequirement_SurpathServiceId').prop('required') &&
      //  $('#RecordRequirement_SurpathServiceId').val() == ''
      //) {
      //  abp.message.error(app.localize('{0}IsRequired', app.localize('SurpathService')));
      //  return;
      //}
      //if (
      //  $('#RecordRequirement_TenantSurpathServiceId').prop('required') &&
      //  $('#RecordRequirement_TenantSurpathServiceId').val() == ''
      //) {
      //  abp.message.error(app.localize('{0}IsRequired', app.localize('TenantSurpathService')));
      //  return;
      //}

      var recordRequirement = _$recordRequirementInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _recordRequirementsService
        .createOrEdit(recordRequirement)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditRecordRequirementModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
