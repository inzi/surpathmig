(function ($) {
  app.modals.MasterDetailChild_LedgerEntry_TenantSurpathServiceLookupTableModal = function () {
    var _modalManager;

    var _ledgerEntryDetailsService = abp.services.app.ledgerEntryDetails;
    var _$tenantSurpathServiceTable = $('#TenantSurpathServiceTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$tenantSurpathServiceTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _ledgerEntryDetailsService.getAllTenantSurpathServiceForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#TenantSurpathServiceTableFilter').val(),
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

    $('#TenantSurpathServiceTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getTenantSurpathService() {
      dataTable.ajax.reload();
    }

    $('#GetTenantSurpathServiceButton').click(function (e) {
      e.preventDefault();
      getTenantSurpathService();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $('#TenantSurpathServiceTableFilter').keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getTenantSurpathService();
      }
    });
  };
})(jQuery);
