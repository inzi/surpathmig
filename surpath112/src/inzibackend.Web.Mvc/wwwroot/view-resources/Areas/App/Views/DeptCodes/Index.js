(function () {
  $(function () {
    var _$deptCodesTable = $('#DeptCodesTable');
    var _deptCodesService = abp.services.app.deptCodes;
    var _entityTypeFullName = 'inzibackend.Surpath.DeptCode';

      $('.date-picker').daterangepicker({
          singleDatePicker: true,
          
           locale: { format: 'MM/DD/YYYY', },
      });

      app.daterangefilterhelper.fixfilters();

    var _permissions = {
      create: abp.auth.hasPermission('Pages.DeptCodes.Create'),
      edit: abp.auth.hasPermission('Pages.DeptCodes.Edit'),
      delete: abp.auth.hasPermission('Pages.DeptCodes.Delete'),
    };

    var _viewDeptCodeModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/DeptCodes/ViewdeptCodeModal',
      modalClass: 'ViewDeptCodeModal',
    });

    var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();
    function entityHistoryIsEnabled() {
      return (
        abp.auth.hasPermission('Pages.Administration.AuditLogs') &&
        abp.custom.EntityHistory &&
        abp.custom.EntityHistory.IsEnabled &&
        _.filter(abp.custom.EntityHistory.EnabledEntities, (entityType) => entityType === _entityTypeFullName)
          .length === 1
      );
    }

      var getDateFilter = function (element) {
          if (element.val() == '') {
              return null;
          }
          return element.data('daterangepicker').startDate.format('YYYY-MM-DDT00:00:00Z');
      };
      var getMaxDateFilter = function (element) {
          if (element.val() == '') {
              return null;
          }
          return element.data('daterangepicker').startDate.format('YYYY-MM-DDT00:00:00Z');
      };

    var dataTable = _$deptCodesTable.DataTable({
        paging: true,
        lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
        pageLength: 5000,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _deptCodesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#DeptCodesTableFilter').val(),
            codeFilter: $('#CodeFilterId').val(),
            codeTypeNameFilter: $('#CodeTypeNameFilterId').val(),
            tenantDepartmentNameFilter: $('#TenantDepartmentNameFilterId').val(),
          };
        },
      },
      columnDefs: [
        {
          className: 'control responsive',
          orderable: false,
          render: function () {
            return '';
          },
          targets: 0,
        },
        {
          width: 120,
          targets: 1,
          data: null,
          orderable: false,
          autoWidth: false,
          defaultContent: '',
          rowAction: {
            cssClass: 'btn btn-brand dropdown-toggle',
            text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
            items: [
              {
                text: app.localize('View'),
                iconStyle: 'far fa-eye mr-2',
                action: function (data) {
                  window.location = '/App/DeptCodes/ViewDeptCode/' + data.record.deptCode.id;
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  window.location = '/App/DeptCodes/CreateOrEdit/' + data.record.deptCode.id;
                },
              },
              {
                text: app.localize('History'),
                iconStyle: 'fas fa-history mr-2',
                visible: function () {
                  return entityHistoryIsEnabled();
                },
                action: function (data) {
                  _entityTypeHistoryModal.open({
                    entityTypeFullName: _entityTypeFullName,
                    entityId: data.record.deptCode.id,
                  });
                },
              },
              {
                text: app.localize('Delete'),
                iconStyle: 'far fa-trash-alt mr-2',
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteDeptCode(data.record.deptCode);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'deptCode.code',
          name: 'code',
        },
        {
          targets: 3,
          data: 'codeTypeName',
          name: 'codeTypeFk.name',
        },
        {
          targets: 4,
          data: 'tenantDepartmentName',
          name: 'tenantDepartmentFk.name',
        },
      ],
    });

    function getDeptCodes() {
      dataTable.ajax.reload();
    }

    function deleteDeptCode(deptCode) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _deptCodesService
            .delete({
              id: deptCode.id,
            })
            .done(function () {
              getDeptCodes(true);
              abp.notify.success(app.localize('SuccessfullyDeleted'));
            });
        }
      });
    }

    $('#ShowAdvancedFiltersSpan').click(function () {
      $('#ShowAdvancedFiltersSpan').hide();
      $('#HideAdvancedFiltersSpan').show();
      $('#AdvacedAuditFiltersArea').slideDown();
    });

    $('#HideAdvancedFiltersSpan').click(function () {
      $('#HideAdvancedFiltersSpan').hide();
      $('#ShowAdvancedFiltersSpan').show();
      $('#AdvacedAuditFiltersArea').slideUp();
    });

    $('#ExportToExcelButton').click(function () {
      _deptCodesService
        .getDeptCodesToExcel({
          filter: $('#DeptCodesTableFilter').val(),
          codeFilter: $('#CodeFilterId').val(),
          codeTypeNameFilter: $('#CodeTypeNameFilterId').val(),
          tenantDepartmentNameFilter: $('#TenantDepartmentNameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditDeptCodeModalSaved', function () {
      getDeptCodes();
    });

    $('#GetDeptCodesButton').click(function (e) {
      e.preventDefault();
      getDeptCodes();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getDeptCodes();
      }
    });
  });
})();
