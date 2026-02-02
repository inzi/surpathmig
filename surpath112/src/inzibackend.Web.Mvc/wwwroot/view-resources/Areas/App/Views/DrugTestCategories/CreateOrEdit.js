(function () {
  $(function () {
    var _drugTestCategoriesService = abp.services.app.drugTestCategories;

    var _$drugTestCategoryInformationForm = $('form[name=DrugTestCategoryInformationsForm]');
    _$drugTestCategoryInformationForm.validate();

    var _DrugTestCategorydrugLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/DrugTestCategories/DrugLookupTableModal',
      scriptUrl:
        abp.appPath + 'view-resources/Areas/App/Views/DrugTestCategories/_DrugTestCategoryDrugLookupTableModal.js',
      modalClass: 'DrugLookupTableModal',
    });
    var _DrugTestCategorypanelLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/DrugTestCategories/PanelLookupTableModal',
      scriptUrl:
        abp.appPath + 'view-resources/Areas/App/Views/DrugTestCategories/_DrugTestCategoryPanelLookupTableModal.js',
      modalClass: 'PanelLookupTableModal',
    });
    var _DrugTestCategorytestCategoryLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/DrugTestCategories/TestCategoryLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/App/Views/DrugTestCategories/_DrugTestCategoryTestCategoryLookupTableModal.js',
      modalClass: 'TestCategoryLookupTableModal',
    });

      $('.date-picker').daterangepicker({
          singleDatePicker: true,
          format: 'L'
      });

    $('#OpenDrugLookupTableButton').click(function () {
      var drugTestCategory = _$drugTestCategoryInformationForm.serializeFormToObject();

      _DrugTestCategorydrugLookupTableModal.open(
        { id: drugTestCategory.drugId, displayName: drugTestCategory.drugName },
        function (data) {
          _$drugTestCategoryInformationForm.find('input[name=drugName]').val(data.displayName);
          _$drugTestCategoryInformationForm.find('input[name=drugId]').val(data.id);
        }
      );
    });

    $('#ClearDrugNameButton').click(function () {
      _$drugTestCategoryInformationForm.find('input[name=drugName]').val('');
      _$drugTestCategoryInformationForm.find('input[name=drugId]').val('');
    });

    $('#OpenPanelLookupTableButton').click(function () {
      var drugTestCategory = _$drugTestCategoryInformationForm.serializeFormToObject();

      _DrugTestCategorypanelLookupTableModal.open(
        { id: drugTestCategory.panelId, displayName: drugTestCategory.panelName },
        function (data) {
          _$drugTestCategoryInformationForm.find('input[name=panelName]').val(data.displayName);
          _$drugTestCategoryInformationForm.find('input[name=panelId]').val(data.id);
        }
      );
    });

    $('#ClearPanelNameButton').click(function () {
      _$drugTestCategoryInformationForm.find('input[name=panelName]').val('');
      _$drugTestCategoryInformationForm.find('input[name=panelId]').val('');
    });

    $('#OpenTestCategoryLookupTableButton').click(function () {
      var drugTestCategory = _$drugTestCategoryInformationForm.serializeFormToObject();

      _DrugTestCategorytestCategoryLookupTableModal.open(
        { id: drugTestCategory.testCategoryId, displayName: drugTestCategory.testCategoryName },
        function (data) {
          _$drugTestCategoryInformationForm.find('input[name=testCategoryName]').val(data.displayName);
          _$drugTestCategoryInformationForm.find('input[name=testCategoryId]').val(data.id);
        }
      );
    });

    $('#ClearTestCategoryNameButton').click(function () {
      _$drugTestCategoryInformationForm.find('input[name=testCategoryName]').val('');
      _$drugTestCategoryInformationForm.find('input[name=testCategoryId]').val('');
    });

    function save(successCallback) {
      if (!_$drugTestCategoryInformationForm.valid()) {
        return;
      }
      if ($('#DrugTestCategory_DrugId').prop('required') && $('#DrugTestCategory_DrugId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Drug')));
        return;
      }
      if ($('#DrugTestCategory_PanelId').prop('required') && $('#DrugTestCategory_PanelId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Panel')));
        return;
      }
      if ($('#DrugTestCategory_TestCategoryId').prop('required') && $('#DrugTestCategory_TestCategoryId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('TestCategory')));
        return;
      }

      var drugTestCategory = _$drugTestCategoryInformationForm.serializeFormToObject();

      abp.ui.setBusy();
      _drugTestCategoriesService
        .createOrEdit(drugTestCategory)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          abp.event.trigger('app.createOrEditDrugTestCategoryModalSaved');

          if (typeof successCallback === 'function') {
            successCallback();
          }
        })
        .always(function () {
          abp.ui.clearBusy();
        });
    }

    function clearForm() {
      _$drugTestCategoryInformationForm[0].reset();
    }

    $('#saveBtn').click(function () {
      save(function () {
        window.location = '/App/DrugTestCategories';
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
