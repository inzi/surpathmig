(function () {
  $(function () {
    var _codeTypesService = abp.services.app.codeTypes;

    var _$codeTypeInformationForm = $('form[name=CodeTypeInformationsForm]');
    _$codeTypeInformationForm.validate();

      $('.date-picker').daterangepicker({
          singleDatePicker: true,
          format: 'L'
      });

    function save(successCallback) {
      if (!_$codeTypeInformationForm.valid()) {
        return;
      }

      var codeType = _$codeTypeInformationForm.serializeFormToObject();

      abp.ui.setBusy();
      _codeTypesService
        .createOrEdit(codeType)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          abp.event.trigger('app.createOrEditCodeTypeModalSaved');

          if (typeof successCallback === 'function') {
            successCallback();
          }
        })
        .always(function () {
          abp.ui.clearBusy();
        });
    }

    function clearForm() {
      _$codeTypeInformationForm[0].reset();
    }

    $('#saveBtn').click(function () {
      save(function () {
        window.location = '/App/CodeTypes';
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
