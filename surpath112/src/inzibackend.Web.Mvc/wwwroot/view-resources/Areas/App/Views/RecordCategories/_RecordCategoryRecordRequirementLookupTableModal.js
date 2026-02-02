(function ($) {
  app.modals.RecordRequirementLookupTableModal = function () {
    var _modalManager;

    var _recordCategoriesService = abp.services.app.recordCategories;
    var _$recordRequirementTable = $('#RecordRequirementTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$recordRequirementTable.DataTable({
        paging: true,
        lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
        pageLength: 5000,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _recordCategoriesService.getAllRecordRequirementForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#RecordRequirementTableFilter').val(),
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

    $('#RecordRequirementTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getRecordRequirement() {
      dataTable.ajax.reload();
    }

    $('#GetRecordRequirementButton').click(function (e) {
      e.preventDefault();
      getRecordRequirement();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $('#RecordRequirementTableFilter').keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getRecordRequirement();
      }
    });
  };
})(jQuery);
