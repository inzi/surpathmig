(function () {
  $(function () {
    var _departmentUsersService = abp.services.app.departmentUsers;

    var _$departmentUserInformationForm = $('form[name=DepartmentUserInformationsForm]');
    _$departmentUserInformationForm.validate();

    var _DepartmentUseruserLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/DepartmentUsers/UserLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DepartmentUsers/_DepartmentUserUserLookupTableModal.js',
      modalClass: 'UserLookupTableModal',
    });
    var _DepartmentUsertenantDepartmentLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/DepartmentUsers/TenantDepartmentLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/App/Views/DepartmentUsers/_DepartmentUserTenantDepartmentLookupTableModal.js',
      modalClass: 'TenantDepartmentLookupTableModal',
    });

      $('.date-picker').daterangepicker({
          singleDatePicker: true,
          format: 'L'
      });

    $('#OpenUserLookupTableButton').click(function () {
      var departmentUser = _$departmentUserInformationForm.serializeFormToObject();

      _DepartmentUseruserLookupTableModal.open(
        { id: departmentUser.userId, displayName: departmentUser.userName },
        function (data) {
          _$departmentUserInformationForm.find('input[name=userName]').val(data.displayName);
          _$departmentUserInformationForm.find('input[name=userId]').val(data.id);
        }
      );
    });

    $('#ClearUserNameButton').click(function () {
      _$departmentUserInformationForm.find('input[name=userName]').val('');
      _$departmentUserInformationForm.find('input[name=userId]').val('');
    });

    $('#OpenTenantDepartmentLookupTableButton').click(function () {
      var departmentUser = _$departmentUserInformationForm.serializeFormToObject();

      _DepartmentUsertenantDepartmentLookupTableModal.open(
        { id: departmentUser.tenantDepartmentId, displayName: departmentUser.tenantDepartmentName },
        function (data) {
          _$departmentUserInformationForm.find('input[name=tenantDepartmentName]').val(data.displayName);
          _$departmentUserInformationForm.find('input[name=tenantDepartmentId]').val(data.id);
        }
      );
    });

    $('#ClearTenantDepartmentNameButton').click(function () {
      _$departmentUserInformationForm.find('input[name=tenantDepartmentName]').val('');
      _$departmentUserInformationForm.find('input[name=tenantDepartmentId]').val('');
    });

    function save(successCallback) {
      if (!_$departmentUserInformationForm.valid()) {
        return;
      }
      if ($('#DepartmentUser_UserId').prop('required') && $('#DepartmentUser_UserId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
        return;
      }
      if (
        $('#DepartmentUser_TenantDepartmentId').prop('required') &&
        $('#DepartmentUser_TenantDepartmentId').val() == ''
      ) {
        abp.message.error(app.localize('{0}IsRequired', app.localize('TenantDepartment')));
        return;
      }

      var departmentUser = _$departmentUserInformationForm.serializeFormToObject();

      abp.ui.setBusy();
      _departmentUsersService
        .createOrEdit(departmentUser)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          abp.event.trigger('app.createOrEditDepartmentUserModalSaved');

          if (typeof successCallback === 'function') {
            successCallback();
          }
        })
        .always(function () {
          abp.ui.clearBusy();
        });
    }

    function clearForm() {
      _$departmentUserInformationForm[0].reset();
    }

    $('#saveBtn').click(function () {
      save(function () {
        window.location = '/App/DepartmentUsers';
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
