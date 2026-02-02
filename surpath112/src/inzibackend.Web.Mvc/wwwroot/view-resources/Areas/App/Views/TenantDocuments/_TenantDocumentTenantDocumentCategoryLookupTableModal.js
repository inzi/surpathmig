(function ($) {
  app.modals.TenantDocumentCategoryLookupTableModal = function () {
    var _modalManager;

    var _tenantDocumentsService = abp.services.app.tenantDocuments;
    var _$tenantDocumentCategoryTable = $('#TenantDocumentCategoryTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$tenantDocumentCategoryTable.DataTable({
        paging: true,
        lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
        pageLength: 5000,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _tenantDocumentsService.getAllTenantDocumentCategoryForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#TenantDocumentCategoryTableFilter').val(),
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

    $('#TenantDocumentCategoryTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getTenantDocumentCategory() {
      dataTable.ajax.reload();
    }

    $('#GetTenantDocumentCategoryButton').click(function (e) {
      e.preventDefault();
      getTenantDocumentCategory();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $(document).keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getTenantDocumentCategory();
      }
    });
  };
})(jQuery);
