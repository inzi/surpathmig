(function ($) {
  app.modals.CreateOrEditTenantDepartmentUserModal = function () {
    var _tenantDepartmentUsersService = abp.services.app.tenantDepartmentUsers;

    var _modalManager;
    var _$tenantDepartmentUserInformationForm = null;

    var _TenantDepartmentUseruserLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/TenantDepartmentUsers/UserLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/App/Views/TenantDepartmentUsers/_TenantDepartmentUserUserLookupTableModal.js',
      modalClass: 'UserLookupTableModal',
    });
    var _TenantDepartmentUsertenantDepartmentLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/TenantDepartmentUsers/TenantDepartmentLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/App/Views/TenantDepartmentUsers/_TenantDepartmentUserTenantDepartmentLookupTableModal.js',
      modalClass: 'TenantDepartmentLookupTableModal',
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

    $('#OpenTenantDepartmentLookupTableButton').click(function () {
      var tenantDepartmentUser = _$tenantDepartmentUserInformationForm.serializeFormToObject();

      _TenantDepartmentUsertenantDepartmentLookupTableModal.open(
        { id: tenantDepartmentUser.tenantDepartmentId, displayName: tenantDepartmentUser.tenantDepartmentName },
        function (data) {
          _$tenantDepartmentUserInformationForm.find('input[name=tenantDepartmentName]').val(data.displayName);
          _$tenantDepartmentUserInformationForm.find('input[name=tenantDepartmentId]').val(data.id);
        }
      );
    });

    $('#ClearTenantDepartmentNameButton').click(function () {
      _$tenantDepartmentUserInformationForm.find('input[name=tenantDepartmentName]').val('');
      _$tenantDepartmentUserInformationForm.find('input[name=tenantDepartmentId]').val('');
    });

    this.save = function () {
      if (!_$tenantDepartmentUserInformationForm.valid()) {
        return;
      }
      if ($('#TenantDepartmentUser_UserId').prop('required') && $('#TenantDepartmentUser_UserId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
        return;
      }
      if (
        $('#TenantDepartmentUser_TenantDepartmentId').prop('required') &&
        $('#TenantDepartmentUser_TenantDepartmentId').val() == ''
      ) {
        abp.message.error(app.localize('{0}IsRequired', app.localize('TenantDepartment')));
        return;
      }

      var tenantDepartmentUser = _$tenantDepartmentUserInformationForm.serializeFormToObject();

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
