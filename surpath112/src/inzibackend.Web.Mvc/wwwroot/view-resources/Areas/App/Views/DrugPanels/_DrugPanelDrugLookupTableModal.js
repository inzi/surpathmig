(function ($) {
  app.modals.DrugLookupTableModal = function () {
    var _modalManager;

    var _drugPanelsService = abp.services.app.drugPanels;
    var _$drugTable = $('#DrugTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$drugTable.DataTable({
        paging: false,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _drugPanelsService.getAllDrugForLookupTable,
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
