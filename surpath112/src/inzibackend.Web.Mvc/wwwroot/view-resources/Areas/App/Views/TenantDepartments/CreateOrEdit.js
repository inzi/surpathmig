(function () {
  $(function () {
    var _tenantDepartmentsService = abp.services.app.tenantDepartments;

    var _$tenantDepartmentInformationForm = $('form[name=TenantDepartmentInformationsForm]');
    _$tenantDepartmentInformationForm.validate();

      $('.date-picker').daterangepicker({
          singleDatePicker: true,
          format: 'L'
      });

    function save(successCallback) {
      if (!_$tenantDepartmentInformationForm.valid()) {
        return;
      }

      var tenantDepartment = _$tenantDepartmentInformationForm.serializeFormToObject();

      abp.ui.setBusy();
      _tenantDepartmentsService
        .createOrEdit(tenantDepartment)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          abp.event.trigger('app.createOrEditTenantDepartmentModalSaved');

          if (typeof successCallback === 'function') {
            successCallback();
          }
        })
        .always(function () {
          abp.ui.clearBusy();
        });
    }

    function clearForm() {
      _$tenantDepartmentInformationForm[0].reset();
    }

    $('#saveBtn').click(function () {
      save(function () {
        window.location = '/App/TenantDepartments';
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
