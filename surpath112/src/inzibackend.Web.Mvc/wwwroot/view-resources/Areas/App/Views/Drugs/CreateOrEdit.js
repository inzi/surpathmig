(function () {
  $(function () {
    var _drugsService = abp.services.app.drugs;

    var _$drugInformationForm = $('form[name=DrugInformationsForm]');
    _$drugInformationForm.validate();

      $('.date-picker').daterangepicker({
          singleDatePicker: true,
          format: 'L'
      });

    function save(successCallback) {
      if (!_$drugInformationForm.valid()) {
        return;
      }

      var drug = _$drugInformationForm.serializeFormToObject();

      abp.ui.setBusy();
      _drugsService
        .createOrEdit(drug)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          abp.event.trigger('app.createOrEditDrugModalSaved');

          if (typeof successCallback === 'function') {
            successCallback();
          }
        })
        .always(function () {
          abp.ui.clearBusy();
        });
    }

    function clearForm() {
      _$drugInformationForm[0].reset();
    }

    $('#saveBtn').click(function () {
      save(function () {
        window.location = '/App/Drugs';
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
