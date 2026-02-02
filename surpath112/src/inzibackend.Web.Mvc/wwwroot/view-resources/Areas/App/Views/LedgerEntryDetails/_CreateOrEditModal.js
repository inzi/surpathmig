(function ($) {
  app.modals.CreateOrEditLedgerEntryDetailModal = function () {
    var _ledgerEntryDetailsService = abp.services.app.ledgerEntryDetails;

    var _modalManager;
    var _$ledgerEntryDetailInformationForm = null;

    var _LedgerEntryDetailledgerEntryLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/LedgerEntryDetails/LedgerEntryLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/App/Views/LedgerEntryDetails/_LedgerEntryDetailLedgerEntryLookupTableModal.js',
      modalClass: 'LedgerEntryLookupTableModal',
    });
    var _LedgerEntryDetailsurpathServiceLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/LedgerEntryDetails/SurpathServiceLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/App/Views/LedgerEntryDetails/_LedgerEntryDetailSurpathServiceLookupTableModal.js',
      modalClass: 'SurpathServiceLookupTableModal',
    });
    var _LedgerEntryDetailtenantSurpathServiceLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/LedgerEntryDetails/TenantSurpathServiceLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/App/Views/LedgerEntryDetails/_LedgerEntryDetailTenantSurpathServiceLookupTableModal.js',
      modalClass: 'TenantSurpathServiceLookupTableModal',
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

    $('#OpenLedgerEntryLookupTableButton').click(function () {
      var ledgerEntryDetail = _$ledgerEntryDetailInformationForm.serializeFormToObject();

      _LedgerEntryDetailledgerEntryLookupTableModal.open(
        { id: ledgerEntryDetail.ledgerEntryId, displayName: ledgerEntryDetail.ledgerEntryTransactionId },
        function (data) {
          _$ledgerEntryDetailInformationForm.find('input[name=ledgerEntryTransactionId]').val(data.displayName);
          _$ledgerEntryDetailInformationForm.find('input[name=ledgerEntryId]').val(data.id);
        }
      );
    });

    $('#ClearLedgerEntryTransactionIdButton').click(function () {
      _$ledgerEntryDetailInformationForm.find('input[name=ledgerEntryTransactionId]').val('');
      _$ledgerEntryDetailInformationForm.find('input[name=ledgerEntryId]').val('');
    });

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
      if ($('#LedgerEntryDetail_LedgerEntryId').prop('required') && $('#LedgerEntryDetail_LedgerEntryId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('LedgerEntry')));
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
