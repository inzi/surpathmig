(function ($) {
    //tssultm
  app.modals.UserLookupTableModal = function () {
    var _modalManager;

    var _tenantSurpathServicesService = abp.services.app.tenantSurpathServices;
    var _$userTable = $('#UserTable');
    var _modalData;


    this.init = function (modalManager) {
      _modalManager = modalManager;
      _modalData = _modalManager.getArgs();
    };

    var dataTable = _$userTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _tenantSurpathServicesService.getAllUserForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#UserTableFilter').val(),
            tenantId: _modalData.tenantId,
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

    $('#UserTableFilter').keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getUser();
      }
    });
  };
})(jQuery);
