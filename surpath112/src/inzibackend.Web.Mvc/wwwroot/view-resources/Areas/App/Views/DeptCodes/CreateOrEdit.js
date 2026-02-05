(function () {
  $(function () {
    var _deptCodesService = abp.services.app.deptCodes;

    var _$deptCodeInformationForm = $('form[name=DeptCodeInformationsForm]');
    _$deptCodeInformationForm.validate();

    var _DeptCodecodeTypeLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/DeptCodes/CodeTypeLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DeptCodes/_DeptCodeCodeTypeLookupTableModal.js',
      modalClass: 'CodeTypeLookupTableModal',
    });
    var _DeptCodetenantDepartmentLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/DeptCodes/TenantDepartmentLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DeptCodes/_DeptCodeTenantDepartmentLookupTableModal.js',
      modalClass: 'TenantDepartmentLookupTableModal',
    });

      $('.date-picker').daterangepicker({
          singleDatePicker: true,
          format: 'L'
      });

    $('#OpenCodeTypeLookupTableButton').click(function () {
      var deptCode = _$deptCodeInformationForm.serializeFormToObject();

      _DeptCodecodeTypeLookupTableModal.open(
        { id: deptCode.codeTypeId, displayName: deptCode.codeTypeName },
        function (data) {
          _$deptCodeInformationForm.find('input[name=codeTypeName]').val(data.displayName);
          _$deptCodeInformationForm.find('input[name=codeTypeId]').val(data.id);
        }
      );
    });

    $('#ClearCodeTypeNameButton').click(function () {
      _$deptCodeInformationForm.find('input[name=codeTypeName]').val('');
      _$deptCodeInformationForm.find('input[name=codeTypeId]').val('');
    });

    $('#OpenTenantDepartmentLookupTableButton').click(function () {
      var deptCode = _$deptCodeInformationForm.serializeFormToObject();

      _DeptCodetenantDepartmentLookupTableModal.open(
        { id: deptCode.tenantDepartmentId, displayName: deptCode.tenantDepartmentName },
        function (data) {
          _$deptCodeInformationForm.find('input[name=tenantDepartmentName]').val(data.displayName);
          _$deptCodeInformationForm.find('input[name=tenantDepartmentId]').val(data.id);
        }
      );
    });

    $('#ClearTenantDepartmentNameButton').click(function () {
      _$deptCodeInformationForm.find('input[name=tenantDepartmentName]').val('');
      _$deptCodeInformationForm.find('input[name=tenantDepartmentId]').val('');
    });

    function save(successCallback) {
      if (!_$deptCodeInformationForm.valid()) {
        return;
      }
      if ($('#DeptCode_CodeTypeId').prop('required') && $('#DeptCode_CodeTypeId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('CodeType')));
        return;
      }
      if ($('#DeptCode_TenantDepartmentId').prop('required') && $('#DeptCode_TenantDepartmentId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('TenantDepartment')));
        return;
      }

      var deptCode = _$deptCodeInformationForm.serializeFormToObject();

      abp.ui.setBusy();
      _deptCodesService
        .createOrEdit(deptCode)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          abp.event.trigger('app.createOrEditDeptCodeModalSaved');

          if (typeof successCallback === 'function') {
            successCallback();
          }
        })
        .always(function () {
          abp.ui.clearBusy();
        });
    }

    function clearForm() {
      _$deptCodeInformationForm[0].reset();
    }

    $('#saveBtn').click(function () {
      save(function () {
        window.location = '/App/DeptCodes';
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
  });
})();
