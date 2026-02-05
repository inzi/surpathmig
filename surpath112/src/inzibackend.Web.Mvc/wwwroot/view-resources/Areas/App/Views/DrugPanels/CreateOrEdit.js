(function () {
  $(function () {
    var _drugPanelsService = abp.services.app.drugPanels;

    var _$drugPanelInformationForm = $('form[name=DrugPanelInformationsForm]');
    _$drugPanelInformationForm.validate();

    var _DrugPaneldrugLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/DrugPanels/DrugLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DrugPanels/_DrugPanelDrugLookupTableModal.js',
      modalClass: 'DrugLookupTableModal',
    });
    var _DrugPanelpanelLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/DrugPanels/PanelLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DrugPanels/_DrugPanelPanelLookupTableModal.js',
      modalClass: 'PanelLookupTableModal',
    });

      $('.date-picker').daterangepicker({
          singleDatePicker: true,
          format: 'L'
      });

    $('#OpenDrugLookupTableButton').click(function () {
      var drugPanel = _$drugPanelInformationForm.serializeFormToObject();

      _DrugPaneldrugLookupTableModal.open({ id: drugPanel.drugId, displayName: drugPanel.drugName }, function (data) {
        _$drugPanelInformationForm.find('input[name=drugName]').val(data.displayName);
        _$drugPanelInformationForm.find('input[name=drugId]').val(data.id);
      });
    });

    $('#ClearDrugNameButton').click(function () {
      _$drugPanelInformationForm.find('input[name=drugName]').val('');
      _$drugPanelInformationForm.find('input[name=drugId]').val('');
    });

    $('#OpenPanelLookupTableButton').click(function () {
      var drugPanel = _$drugPanelInformationForm.serializeFormToObject();

      _DrugPanelpanelLookupTableModal.open(
        { id: drugPanel.panelId, displayName: drugPanel.panelName },
        function (data) {
          _$drugPanelInformationForm.find('input[name=panelName]').val(data.displayName);
          _$drugPanelInformationForm.find('input[name=panelId]').val(data.id);
        }
      );
    });

    $('#ClearPanelNameButton').click(function () {
      _$drugPanelInformationForm.find('input[name=panelName]').val('');
      _$drugPanelInformationForm.find('input[name=panelId]').val('');
    });

    function save(successCallback) {
      if (!_$drugPanelInformationForm.valid()) {
        return;
      }
      if ($('#DrugPanel_DrugId').prop('required') && $('#DrugPanel_DrugId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Drug')));
        return;
      }
      if ($('#DrugPanel_PanelId').prop('required') && $('#DrugPanel_PanelId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Panel')));
        return;
      }

      var drugPanel = _$drugPanelInformationForm.serializeFormToObject();

      abp.ui.setBusy();
      _drugPanelsService
        .createOrEdit(drugPanel)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          abp.event.trigger('app.createOrEditDrugPanelModalSaved');

          if (typeof successCallback === 'function') {
            successCallback();
          }
        })
        .always(function () {
          abp.ui.clearBusy();
        });
    }

    function clearForm() {
      _$drugPanelInformationForm[0].reset();
    }

    $('#saveBtn').click(function () {
      save(function () {
        window.location = '/App/DrugPanels';
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
