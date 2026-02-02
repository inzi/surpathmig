(function () {
  $(function () {
    var _panelsService = abp.services.app.panels;

    var _$panelInformationForm = $('form[name=PanelInformationsForm]');
    _$panelInformationForm.validate();

    var _PaneltestCategoryLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Panels/TestCategoryLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Panels/_PanelTestCategoryLookupTableModal.js',
      modalClass: 'TestCategoryLookupTableModal',
    });

      $('.date-picker').daterangepicker({
          singleDatePicker: true,
          format: 'L'
      });

    $('#OpenTestCategoryLookupTableButton').click(function () {
      var panel = _$panelInformationForm.serializeFormToObject();

      _PaneltestCategoryLookupTableModal.open(
        { id: panel.testCategoryId, displayName: panel.testCategoryName },
        function (data) {
          _$panelInformationForm.find('input[name=testCategoryName]').val(data.displayName);
          _$panelInformationForm.find('input[name=testCategoryId]').val(data.id);
        }
      );
    });

    $('#ClearTestCategoryNameButton').click(function () {
      _$panelInformationForm.find('input[name=testCategoryName]').val('');
      _$panelInformationForm.find('input[name=testCategoryId]').val('');
    });

    function save(successCallback) {
      if (!_$panelInformationForm.valid()) {
        return;
      }
      if ($('#Panel_TestCategoryId').prop('required') && $('#Panel_TestCategoryId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('TestCategory')));
        return;
      }

      var panel = _$panelInformationForm.serializeFormToObject();

      abp.ui.setBusy();
      _panelsService
        .createOrEdit(panel)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          abp.event.trigger('app.createOrEditPanelModalSaved');

          if (typeof successCallback === 'function') {
            successCallback();
          }
        })
        .always(function () {
          abp.ui.clearBusy();
        });
    }

    function clearForm() {
      _$panelInformationForm[0].reset();
    }

    $('#saveBtn').click(function () {
      save(function () {
        window.location = '/App/Panels';
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
