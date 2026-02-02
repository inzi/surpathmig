(function () {
  $(function () {
    var _hospitalsService = abp.services.app.hospitals;

    var _$hospitalInformationForm = $('form[name=HospitalInformationsForm]');
    _$hospitalInformationForm.validate();

    $('.date-picker').daterangepicker({
      singleDatePicker: true,
      locale: abp.localization.currentLanguage.name,
      format: 'L',
    });

    function save(successCallback) {
      if (!_$hospitalInformationForm.valid()) {
        return;
      }

      var hospital = _$hospitalInformationForm.serializeFormToObject();

      abp.ui.setBusy();
      _hospitalsService
        .createOrEdit(hospital)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          abp.event.trigger('app.createOrEditHospitalModalSaved');

          if (typeof successCallback === 'function') {
            successCallback();
          }
        })
        .always(function () {
          abp.ui.clearBusy();
        });
    }

    function clearForm() {
      _$hospitalInformationForm[0].reset();
    }

    $('#saveBtn').click(function () {
      save(function () {
        window.location = '/App/Hospitals';
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
