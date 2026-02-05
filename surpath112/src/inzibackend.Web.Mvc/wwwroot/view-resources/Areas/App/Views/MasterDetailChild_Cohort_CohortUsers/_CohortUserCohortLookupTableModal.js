(function ($) {
  app.modals.MasterDetailChild_Cohort_CohortLookupTableModal = function () {
    var _modalManager;

    var _cohortUsersService = abp.services.app.cohortUsers;
    var _$cohortTable = $('#CohortTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$cohortTable.DataTable({
        paging: true,
        lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
        pageLength: 5000,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _cohortUsersService.getAllCohortForLookupTable,
          inputFilter: function () {
          return {
              filter: $('#CohortTableFilter').val(),
              /*cohortId:*/ 
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

    $('#CohortTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getCohort() {
      dataTable.ajax.reload();
    }

    $('#GetCohortButton').click(function (e) {
      e.preventDefault();
      getCohort();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $(document).keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getCohort();
      }
    });
  };
})(jQuery);
