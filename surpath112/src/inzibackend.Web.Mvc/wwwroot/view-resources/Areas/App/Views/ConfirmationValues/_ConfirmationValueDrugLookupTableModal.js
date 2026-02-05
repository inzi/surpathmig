(function ($) {
  app.modals.DrugLookupTableModal = function () {
    var _modalManager;

    var _confirmationValuesService = abp.services.app.confirmationValues;
    var _$drugTable = $('#DrugTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$drugTable.DataTable({
        paging: true,
        lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
        pageLength: 5000,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _confirmationValuesService.getAllDrugForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#DrugTableFilter').val(),
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

    $('#DrugTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getDrug() {
      dataTable.ajax.reload();
    }

    $('#GetDrugButton').click(function (e) {
      e.preventDefault();
      getDrug();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $(document).keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getDrug();
      }
    });
  };
})(jQuery);
