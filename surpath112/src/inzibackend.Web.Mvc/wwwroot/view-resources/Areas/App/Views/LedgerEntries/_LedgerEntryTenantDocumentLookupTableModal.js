(function ($) {
  app.modals.TenantDocumentLookupTableModal = function () {
    var _modalManager;

    var _ledgerEntriesService = abp.services.app.ledgerEntries;
    var _$tenantDocumentTable = $('#TenantDocumentTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$tenantDocumentTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _ledgerEntriesService.getAllTenantDocumentForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#TenantDocumentTableFilter').val(),
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

    $('#TenantDocumentTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getTenantDocument() {
      dataTable.ajax.reload();
    }

    $('#GetTenantDocumentButton').click(function (e) {
      e.preventDefault();
      getTenantDocument();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $('#TenantDocumentTableFilter').keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getTenantDocument();
      }
    });
  };
})(jQuery);
