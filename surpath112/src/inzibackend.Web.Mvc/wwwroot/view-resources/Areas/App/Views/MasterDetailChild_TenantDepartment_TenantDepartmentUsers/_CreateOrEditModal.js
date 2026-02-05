(function ($) {
  app.modals.MasterDetailChild_TenantDepartment_CreateOrEditTenantDepartmentUserModal = function () {
    var _tenantDepartmentUsersService = abp.services.app.tenantDepartmentUsers;

    var _modalManager;
    var _$tenantDepartmentUserInformationForm = null;

    var _TenantDepartmentUseruserLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/MasterDetailChild_TenantDepartment_TenantDepartmentUsers/UserLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/App/Views/MasterDetailChild_TenantDepartment_TenantDepartmentUsers/_TenantDepartmentUserUserLookupTableModal.js',
      modalClass: 'MasterDetailChild_TenantDepartment_UserLookupTableModal',
    });

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
        modal.find('.date-picker').daterangepicker({
            singleDatePicker: true,
            format: 'L'
        });

      _$tenantDepartmentUserInformationForm = _modalManager
        .getModal()
        .find('form[name=TenantDepartmentUserInformationsForm]');
      _$tenantDepartmentUserInformationForm.validate();
    };

    $('#OpenUserLookupTableButton').click(function () {
      var tenantDepartmentUser = _$tenantDepartmentUserInformationForm.serializeFormToObject();

      _TenantDepartmentUseruserLookupTableModal.open(
        { id: tenantDepartmentUser.userId, displayName: tenantDepartmentUser.userName },
        function (data) {
          _$tenantDepartmentUserInformationForm.find('input[name=userName]').val(data.displayName);
          _$tenantDepartmentUserInformationForm.find('input[name=userId]').val(data.id);
        }
      );
    });

    $('#ClearUserNameButton').click(function () {
      _$tenantDepartmentUserInformationForm.find('input[name=userName]').val('');
      _$tenantDepartmentUserInformationForm.find('input[name=userId]').val('');
    });

    this.save = function () {
      if (!_$tenantDepartmentUserInformationForm.valid()) {
        return;
      }
      if ($('#TenantDepartmentUser_UserId').prop('required') && $('#TenantDepartmentUser_UserId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
        return;
      }

      var tenantDepartmentUser = _$tenantDepartmentUserInformationForm.serializeFormToObject();

      tenantDepartmentUser.tenantDepartmentId = $('#MasterDetailChild_TenantDepartment_TenantDepartmentUsersId').val();

      _modalManager.setBusy(true);
      _tenantDepartmentUsersService
        .createOrEdit(tenantDepartmentUser)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditTenantDepartmentUserModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
