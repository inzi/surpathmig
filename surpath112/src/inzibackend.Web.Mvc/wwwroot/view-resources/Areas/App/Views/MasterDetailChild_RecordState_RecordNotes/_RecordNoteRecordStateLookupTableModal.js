(function ($) {
  app.modals.MasterDetailChild_RecordState_RecordStateLookupTableModal = function () {
    var _modalManager;

    var _recordNotesService = abp.services.app.recordNotes;
    var _$recordStateTable = $('#RecordStateTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$recordStateTable.DataTable({
        paging: true,
        lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
        pageLength: 5000,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _recordNotesService.getAllRecordStateForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#RecordStateTableFilter').val(),
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

    $('#RecordStateTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getRecordState() {
      dataTable.ajax.reload();
    }

    $('#GetRecordStateButton').click(function (e) {
      e.preventDefault();
      getRecordState();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $(document).keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getRecordState();
      }
    });
  };
})(jQuery);
