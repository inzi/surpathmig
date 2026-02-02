(function ($) {
  app.modals.CreateOrEditTenantDepartmentModal = function () {
    var _tenantDepartmentsService = abp.services.app.tenantDepartments;

    var _modalManager;
    var _$tenantDepartmentInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
        modal.find('.date-picker').daterangepicker({
            singleDatePicker: true,
            format: 'L'
        });

      _$tenantDepartmentInformationForm = _modalManager.getModal().find('form[name=TenantDepartmentInformationsForm]');
      _$tenantDepartmentInformationForm.validate();
    };

    this.save = function () {
      if (!_$tenantDepartmentInformationForm.valid()) {
        return;
      }

      var tenantDepartment = _$tenantDepartmentInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _tenantDepartmentsService
        .createOrEdit(tenantDepartment)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditTenantDepartmentModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
