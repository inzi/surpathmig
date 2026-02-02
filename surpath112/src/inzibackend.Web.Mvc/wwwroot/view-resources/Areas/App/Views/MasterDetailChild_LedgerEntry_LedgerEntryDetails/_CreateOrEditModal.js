(function ($) {
  app.modals.MasterDetailChild_LedgerEntry_CreateOrEditLedgerEntryDetailModal = function () {
    var _ledgerEntryDetailsService = abp.services.app.ledgerEntryDetails;

    var _modalManager;
    var _$ledgerEntryDetailInformationForm = null;

    var _LedgerEntryDetailsurpathServiceLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/MasterDetailChild_LedgerEntry_LedgerEntryDetails/SurpathServiceLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/App/Views/MasterDetailChild_LedgerEntry_LedgerEntryDetails/_LedgerEntryDetailSurpathServiceLookupTableModal.js',
      modalClass: 'MasterDetailChild_LedgerEntry_SurpathServiceLookupTableModal',
    });
    var _LedgerEntryDetailtenantSurpathServiceLookupTableModal = new app.ModalManager({
      viewUrl:
        abp.appPath + 'App/MasterDetailChild_LedgerEntry_LedgerEntryDetails/TenantSurpathServiceLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/App/Views/MasterDetailChild_LedgerEntry_LedgerEntryDetails/_LedgerEntryDetailTenantSurpathServiceLookupTableModal.js',
      modalClass: 'MasterDetailChild_LedgerEntry_TenantSurpathServiceLookupTableModal',
    });

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$ledgerEntryDetailInformationForm = _modalManager
        .getModal()
        .find('form[name=LedgerEntryDetailInformationsForm]');
      _$ledgerEntryDetailInformationForm.validate();
    };

    $('#OpenSurpathServiceLookupTableButton').click(function () {
      var ledgerEntryDetail = _$ledgerEntryDetailInformationForm.serializeFormToObject();

      _LedgerEntryDetailsurpathServiceLookupTableModal.open(
        { id: ledgerEntryDetail.surpathServiceId, displayName: ledgerEntryDetail.surpathServiceName },
        function (data) {
          _$ledgerEntryDetailInformationForm.find('input[name=surpathServiceName]').val(data.displayName);
          _$ledgerEntryDetailInformationForm.find('input[name=surpathServiceId]').val(data.id);
        }
      );
    });

    $('#ClearSurpathServiceNameButton').click(function () {
      _$ledgerEntryDetailInformationForm.find('input[name=surpathServiceName]').val('');
      _$ledgerEntryDetailInformationForm.find('input[name=surpathServiceId]').val('');
    });

    $('#OpenTenantSurpathServiceLookupTableButton').click(function () {
      var ledgerEntryDetail = _$ledgerEntryDetailInformationForm.serializeFormToObject();

      _LedgerEntryDetailtenantSurpathServiceLookupTableModal.open(
        { id: ledgerEntryDetail.tenantSurpathServiceId, displayName: ledgerEntryDetail.tenantSurpathServiceName },
        function (data) {
          _$ledgerEntryDetailInformationForm.find('input[name=tenantSurpathServiceName]').val(data.displayName);
          _$ledgerEntryDetailInformationForm.find('input[name=tenantSurpathServiceId]').val(data.id);
        }
      );
    });

    $('#ClearTenantSurpathServiceNameButton').click(function () {
      _$ledgerEntryDetailInformationForm.find('input[name=tenantSurpathServiceName]').val('');
      _$ledgerEntryDetailInformationForm.find('input[name=tenantSurpathServiceId]').val('');
    });

    this.save = function () {
      if (!_$ledgerEntryDetailInformationForm.valid()) {
        return;
      }
      if (
        $('#LedgerEntryDetail_SurpathServiceId').prop('required') &&
        $('#LedgerEntryDetail_SurpathServiceId').val() == ''
      ) {
        abp.message.error(app.localize('{0}IsRequired', app.localize('SurpathService')));
        return;
      }
      if (
        $('#LedgerEntryDetail_TenantSurpathServiceId').prop('required') &&
        $('#LedgerEntryDetail_TenantSurpathServiceId').val() == ''
      ) {
        abp.message.error(app.localize('{0}IsRequired', app.localize('TenantSurpathService')));
        return;
      }

      var ledgerEntryDetail = _$ledgerEntryDetailInformationForm.serializeFormToObject();

      ledgerEntryDetail.ledgerEntryId = $('#MasterDetailChild_LedgerEntry_LedgerEntryDetailsId').val();

      _modalManager.setBusy(true);
      _ledgerEntryDetailsService
        .createOrEdit(ledgerEntryDetail)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditLedgerEntryDetailModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
