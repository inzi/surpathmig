(function ($) {
  app.modals.SurpathServiceLookupTableModal = function () {
    var _modalManager;

    var _ledgerEntryDetailsService = abp.services.app.ledgerEntryDetails;
    var _$surpathServiceTable = $('#SurpathServiceTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$surpathServiceTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _ledgerEntryDetailsService.getAllSurpathServiceForLookupTable,
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
