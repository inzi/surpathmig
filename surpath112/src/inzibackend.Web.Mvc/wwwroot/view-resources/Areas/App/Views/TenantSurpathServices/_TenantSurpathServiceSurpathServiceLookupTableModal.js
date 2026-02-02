(function ($) {
  app.modals.SurpathServiceLookupTableModal = function () {
    var _modalManager;

    var _tenantSurpathServicesService = abp.services.app.tenantSurpathServices;
    var _$surpathServiceTable = $('#SurpathServiceTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$surpathServiceTable.DataTable({
        paging: true,
        lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
        pageLength: 5000,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _tenantSurpathServicesService.getAllSurpathServiceForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#SurpathServiceTableFilter').val(),
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

    $('#SurpathServiceTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getSurpathService() {
      dataTable.ajax.reload();
    }

    $('#GetSurpathServiceButton').click(function (e) {
      e.preventDefault();
      getSurpathService();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $('#SurpathServiceTableFilter').keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getSurpathService();
      }
    });
  };
})(jQuery);
