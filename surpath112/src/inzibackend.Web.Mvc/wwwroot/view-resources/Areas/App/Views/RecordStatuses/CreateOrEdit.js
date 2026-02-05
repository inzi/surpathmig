(function () {
  $(function () {
    var _recordStatusesService = abp.services.app.recordStatuses;

    var _$recordStatusInformationForm = $('form[name=RecordStatusInformationsForm]');
    _$recordStatusInformationForm.validate();

    var _RecordStatustenantDepartmentLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RecordStatuses/TenantDepartmentLookupTableModal',
      scriptUrl:
        abp.appPath + 'view-resources/Areas/App/Views/RecordStatuses/_RecordStatusTenantDepartmentLookupTableModal.js',
      modalClass: 'TenantDepartmentLookupTableModal',
    });

    $('.date-picker').daterangepicker({
      singleDatePicker: true,
      locale: abp.localization.currentLanguage.name,
      format: 'L',
    });

    $('#OpenTenantDepartmentLookupTableButton').click(function () {
      var recordStatus = _$recordStatusInformationForm.serializeFormToObject();

      _RecordStatustenantDepartmentLookupTableModal.open(
        { id: recordStatus.tenantDepartmentId, displayName: recordStatus.tenantDepartmentName },
        function (data) {
          _$recordStatusInformationForm.find('input[name=tenantDepartmentName]').val(data.displayName);
          _$recordStatusInformationForm.find('input[name=tenantDepartmentId]').val(data.id);
        }
      );
    });

    $('#ClearTenantDepartmentNameButton').click(function () {
      _$recordStatusInformationForm.find('input[name=tenantDepartmentName]').val('');
      _$recordStatusInformationForm.find('input[name=tenantDepartmentId]').val('');
    });

    function save(successCallback) {
      if (!_$recordStatusInformationForm.valid()) {
        return;
      }
      if ($('#RecordStatus_TenantDepartmentId').prop('required') && $('#RecordStatus_TenantDepartmentId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('TenantDepartment')));
        return;
      }

      var recordStatus = _$recordStatusInformationForm.serializeFormToObject();

      abp.ui.setBusy();
      _recordStatusesService
        .createOrEdit(recordStatus)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          abp.event.trigger('app.createOrEditRecordStatusModalSaved');

          if (typeof successCallback === 'function') {
            successCallback();
          }
        })
        .always(function () {
          abp.ui.clearBusy();
        });
    }

    function clearForm() {
      _$recordStatusInformationForm[0].reset();
    }

    $('#saveBtn').click(function () {
      save(function () {
        window.location = '/App/RecordStatuses';
      });
    });

    $('#saveAndNewBtn').click(function () {
      save(function () {
        if (!$('input[name=id]').val()) {
          //if it is create page
          clearForm();
        }
      });
    });

      function colorChangeCallback(color) {
          //console.log(color.hexString);
          $('#RecordStatus_HtmlColor').val(color.hexString);
          $('#pickerExampleBadge').css('background', color.hexString);
      };
      var _initcol = $('#RecordStatus_HtmlColor').val();
      if (_initcol == "") {
          _initcol = "#AADDFF";
      }

      var colorPicker = new iro.ColorPicker('#iropicker', {
          width: 200,
          color: _initcol,
          //layout: [
          //    {
          //        component: iro.ui.Box,
          //    },
          //]
      });
      colorPicker.on("color:change", colorChangeCallback);
  });
})();
