(function ($) {
  app.modals.MasterDetailChild_Cohort_UserLookupTableModal = function () {
    var _modalManager;

    var _cohortUsersService = abp.services.app.cohortUsers;
    var _$userTable = $('#UserTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$userTable.DataTable({
        paging: true,
        lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
        pageLength: 5000,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _cohortUsersService.getAllUserForLookupTable,
          inputFilter: function () {
          return {
            filter: $('#UserTableFilter').val(),
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
          {
              autoWidth: false,
              orderable: false,
              targets: 2,
              data: 'surname',
          },
      ],
    });

    $('#UserTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getUser() {
      dataTable.ajax.reload();
    }

    $('#GetUserButton').click(function (e) {
      e.preventDefault();
      getUser();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

      $(document).keypress(function (e) {
          
          if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
              e.preventDefault();
        getUser();
      }
    });
  };
})(jQuery);
