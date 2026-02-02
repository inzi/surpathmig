(function ($) {
  app.modals.CohortLookupTableModal = function () {
    var _modalManager;
    var _modalData;

    var _tenantSurpathServicesService = abp.services.app.tenantSurpathServices;
      var _$cohortTable = $('#CohortTable');
      var _TenantId = $('#TenantId').val();

    this.init = function (modalManager) {
      _modalManager = modalManager;
      _modalData = _modalManager.getArgs();
      console.log('cohortLookupTableModal', _modalData);
    };

    var dataTable = _$cohortTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _tenantSurpathServicesService.getAllCohortForLookupTable,
        inputFilter: function () {
          return {
              filter: $('#CohortTableFilter').val(),
              tenantId: GetTenantIdFilter()
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
              targets: 2,
              data: null,
              orderable: false,
              autoWidth: false,
              render: function (data, row, full) {
                  return data.tenantInfoDto.tenancyName;
              },
              visible: function () {
                  return abp.session.multiTenancySide == abp.multiTenancy.sides.HOST;
              }
          }
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

    $('#CohortTableFilter').keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getCohort();
      }
    });

      function GetTenantIdFilter() {
          // First check if tenantId is passed in modal data
          if (_modalData && _modalData.tenantId) {
              console.log('Using tenantId from modal data:', _modalData.tenantId);
              return _modalData.tenantId;
          }
          
          // Fall back to looking in the form
          var tenantId = $('#TenantSurpathServiceInformationsForm').find('#tenantId').val();
          console.log('GetTenantIdFilter tenantId: ' + tenantId);
          if (tenantId == undefined || tenantId == null || tenantId == '') {
              return;
          }
          return tenantId;
      }
  };
})(jQuery);
