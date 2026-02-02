(function () {
  $(function () {
    var _confirmationValuesService = abp.services.app.confirmationValues;

    var _$confirmationValueInformationForm = $('form[name=ConfirmationValueInformationsForm]');
    _$confirmationValueInformationForm.validate();

    var _ConfirmationValuedrugLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/ConfirmationValues/DrugLookupTableModal',
      scriptUrl:
        abp.appPath + 'view-resources/Areas/App/Views/ConfirmationValues/_ConfirmationValueDrugLookupTableModal.js',
      modalClass: 'DrugLookupTableModal',
    });
    var _ConfirmationValuetestCategoryLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/ConfirmationValues/TestCategoryLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/App/Views/ConfirmationValues/_ConfirmationValueTestCategoryLookupTableModal.js',
      modalClass: 'TestCategoryLookupTableModal',
    });

      $('.date-picker').daterangepicker({
          singleDatePicker: true,
          format: 'L'
      });

    $('#OpenDrugLookupTableButton').click(function () {
      var confirmationValue = _$confirmationValueInformationForm.serializeFormToObject();

      _ConfirmationValuedrugLookupTableModal.open(
        { id: confirmationValue.drugId, displayName: confirmationValue.drugName },
        function (data) {
          _$confirmationValueInformationForm.find('input[name=drugName]').val(data.displayName);
          _$confirmationValueInformationForm.find('input[name=drugId]').val(data.id);
        }
      );
    });

    $('#ClearDrugNameButton').click(function () {
      _$confirmationValueInformationForm.find('input[name=drugName]').val('');
      _$confirmationValueInformationForm.find('input[name=drugId]').val('');
    });

    $('#OpenTestCategoryLookupTableButton').click(function () {
      var confirmationValue = _$confirmationValueInformationForm.serializeFormToObject();

      _ConfirmationValuetestCategoryLookupTableModal.open(
        { id: confirmationValue.testCategoryId, displayName: confirmationValue.testCategoryName },
        function (data) {
          _$confirmationValueInformationForm.find('input[name=testCategoryName]').val(data.displayName);
          _$confirmationValueInformationForm.find('input[name=testCategoryId]').val(data.id);
        }
      );
    });

    $('#ClearTestCategoryNameButton').click(function () {
      _$confirmationValueInformationForm.find('input[name=testCategoryName]').val('');
      _$confirmationValueInformationForm.find('input[name=testCategoryId]').val('');
    });

    function save(successCallback) {
      if (!_$confirmationValueInformationForm.valid()) {
        return;
      }
      if ($('#ConfirmationValue_DrugId').prop('required') && $('#ConfirmationValue_DrugId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Drug')));
        return;
      }
      if (
        $('#ConfirmationValue_TestCategoryId').prop('required') &&
        $('#ConfirmationValue_TestCategoryId').val() == ''
      ) {
        abp.message.error(app.localize('{0}IsRequired', app.localize('TestCategory')));
        return;
      }

      var confirmationValue = _$confirmationValueInformationForm.serializeFormToObject();

      abp.ui.setBusy();
      _confirmationValuesService
        .createOrEdit(confirmationValue)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          abp.event.trigger('app.createOrEditConfirmationValueModalSaved');

          if (typeof successCallback === 'function') {
            successCallback();
          }
        })
        .always(function () {
          abp.ui.clearBusy();
        });
    }

    function clearForm() {
      _$confirmationValueInformationForm[0].reset();
    }

    $('#saveBtn').click(function () {
      save(function () {
        window.location = '/App/ConfirmationValues';
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
