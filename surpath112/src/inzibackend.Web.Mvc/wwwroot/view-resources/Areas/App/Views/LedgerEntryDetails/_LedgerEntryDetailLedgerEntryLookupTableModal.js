(function ($) {
  app.modals.LedgerEntryLookupTableModal = function () {
    var _modalManager;

    var _ledgerEntryDetailsService = abp.services.app.ledgerEntryDetails;
    var _$ledgerEntryTable = $('#LedgerEntryTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$ledgerEntryTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _ledgerEntryDetailsService.getAllLedgerEntryForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#LedgerEntryTableFilter').val(),
          };
        },
      },
      columnDefs: [
        {
          targets: 0,
          data: null,
          orderable: false,
          autoWidth: false,
          defaultContent:
            "<div class=\"text-center\"><input id='selectbtn' class='btn btn-success' type='button' width='25px' value='" +
            app.localize('Select') +
            "' /></div>",
        },
        {
          autoWidth: false,
          orderable: false,
          targets: 1,
          data: 'displayName',
        },
      ],
    });

    $('#LedgerEntryTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getLedgerEntry() {
      dataTable.ajax.reload();
    }

    $('#GetLedgerEntryButton').click(function (e) {
      e.preventDefault();
      getLedgerEntry();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $('#LedgerEntryTableFilter').keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getLedgerEntry();
      }
    });
  };
})(jQuery);
