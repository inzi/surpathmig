(function ($) {
  app.modals.PanelLookupTableModal = function () {
    var _modalManager;

    var _drugTestCategoriesService = abp.services.app.drugTestCategories;
    var _$panelTable = $('#PanelTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$panelTable.DataTable({
        paging: false,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _drugTestCategoriesService.getAllPanelForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#PanelTableFilter').val(),
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

    $('#PanelTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getPanel() {
      dataTable.ajax.reload();
    }

    $('#GetPanelButton').click(function (e) {
      e.preventDefault();
      getPanel();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $(document).keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getPanel();
      }
    });
  };
})(jQuery);
