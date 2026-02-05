(function ($) {
  app.modals.CreateOrEditLedgerEntryModal = function () {
    var _ledgerEntriesService = abp.services.app.ledgerEntries;

    var _modalManager;
    var _$ledgerEntryInformationForm = null;

    var _LedgerEntryuserLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/LedgerEntries/UserLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LedgerEntries/_LedgerEntryUserLookupTableModal.js',
      modalClass: 'UserLookupTableModal',
    });
    var _LedgerEntrytenantDocumentLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/LedgerEntries/TenantDocumentLookupTableModal',
      scriptUrl:
        abp.appPath + 'view-resources/Areas/App/Views/LedgerEntries/_LedgerEntryTenantDocumentLookupTableModal.js',
      modalClass: 'TenantDocumentLookupTableModal',
    });
    var _LedgerEntrycohortLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/LedgerEntries/CohortLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LedgerEntries/_LedgerEntryCohortLookupTableModal.js',
      modalClass: 'CohortLookupTableModal',
    });

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$ledgerEntryInformationForm = _modalManager.getModal().find('form[name=LedgerEntryInformationsForm]');
      _$ledgerEntryInformationForm.validate();
    };

    $('#OpenUserLookupTableButton').click(function () {
      var ledgerEntry = _$ledgerEntryInformationForm.serializeFormToObject();

      _LedgerEntryuserLookupTableModal.open(
        { id: ledgerEntry.userId, displayName: ledgerEntry.userName },
        function (data) {
          _$ledgerEntryInformationForm.find('input[name=userName]').val(data.displayName);
          _$ledgerEntryInformationForm.find('input[name=userId]').val(data.id);
        }
      );
    });

    $('#ClearUserNameButton').click(function () {
      _$ledgerEntryInformationForm.find('input[name=userName]').val('');
      _$ledgerEntryInformationForm.find('input[name=userId]').val('');
    });

    $('#OpenTenantDocumentLookupTableButton').click(function () {
      var ledgerEntry = _$ledgerEntryInformationForm.serializeFormToObject();

      _LedgerEntrytenantDocumentLookupTableModal.open(
        { id: ledgerEntry.tenantDocumentId, displayName: ledgerEntry.tenantDocumentName },
        function (data) {
          _$ledgerEntryInformationForm.find('input[name=tenantDocumentName]').val(data.displayName);
          _$ledgerEntryInformationForm.find('input[name=tenantDocumentId]').val(data.id);
        }
      );
    });

    $('#ClearTenantDocumentNameButton').click(function () {
      _$ledgerEntryInformationForm.find('input[name=tenantDocumentName]').val('');
      _$ledgerEntryInformationForm.find('input[name=tenantDocumentId]').val('');
    });

    $('#OpenCohortLookupTableButton').click(function () {
      var ledgerEntry = _$ledgerEntryInformationForm.serializeFormToObject();

      _LedgerEntrycohortLookupTableModal.open(
        { id: ledgerEntry.cohortId, displayName: ledgerEntry.cohortName },
        function (data) {
          _$ledgerEntryInformationForm.find('input[name=cohortName]').val(data.displayName);
          _$ledgerEntryInformationForm.find('input[name=cohortId]').val(data.id);
        }
      );
    });

    $('#ClearCohortNameButton').click(function () {
      _$ledgerEntryInformationForm.find('input[name=cohortName]').val('');
      _$ledgerEntryInformationForm.find('input[name=cohortId]').val('');
    });

    this.save = function () {
      if (!_$ledgerEntryInformationForm.valid()) {
        return;
      }
      if ($('#LedgerEntry_UserId').prop('required') && $('#LedgerEntry_UserId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
        return;
      }
      if ($('#LedgerEntry_TenantDocumentId').prop('required') && $('#LedgerEntry_TenantDocumentId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('TenantDocument')));
        return;
      }
      if ($('#LedgerEntry_CohortId').prop('required') && $('#LedgerEntry_CohortId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Cohort')));
        return;
      }

      var ledgerEntry = _$ledgerEntryInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _ledgerEntriesService
        .createOrEdit(ledgerEntry)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditLedgerEntryModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
