(function () {
  $(function () {
    var _testCategoriesService = abp.services.app.testCategories;

    var _$testCategoryInformationForm = $('form[name=TestCategoryInformationsForm]');
    _$testCategoryInformationForm.validate();

      $('.date-picker').daterangepicker({
          singleDatePicker: true,
          format: 'L'
      });

    function save(successCallback) {
      if (!_$testCategoryInformationForm.valid()) {
        return;
      }

      var testCategory = _$testCategoryInformationForm.serializeFormToObject();

      abp.ui.setBusy();
      _testCategoriesService
        .createOrEdit(testCategory)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          abp.event.trigger('app.createOrEditTestCategoryModalSaved');

          if (typeof successCallback === 'function') {
            successCallback();
          }
        })
        .always(function () {
          abp.ui.clearBusy();
        });
    }

    function clearForm() {
      _$testCategoryInformationForm[0].reset();
    }

    $('#saveBtn').click(function () {
      save(function () {
        window.location = '/App/TestCategories';
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
