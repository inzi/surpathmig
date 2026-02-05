(function () {
  $(function () {
    var _$departmentUsersTable = $('#DepartmentUsersTable');
    var _departmentUsersService = abp.services.app.departmentUsers;
    var _entityTypeFullName = 'inzibackend.Surpath.DepartmentUser';

      $('.date-picker').daterangepicker({
          singleDatePicker: true,
          
           locale: { format: 'MM/DD/YYYY', },
      });

      app.daterangefilterhelper.fixfilters();

    var _permissions = {
      create: abp.auth.hasPermission('Pages.DepartmentUsers.Create'),
      edit: abp.auth.hasPermission('Pages.DepartmentUsers.Edit'),
      delete: abp.auth.hasPermission('Pages.DepartmentUsers.Delete'),
    };

    var _viewDepartmentUserModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/DepartmentUsers/ViewdepartmentUserModal',
      modalClass: 'ViewDepartmentUserModal',
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

    var dataTable = _$departmentUsersTable.DataTable({
        paging: true,
        lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
        pageLength: 5000,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _departmentUsersService.getAll,
        inputFilter: function () {
          return {
            filter: $('#DepartmentUsersTableFilter').val(),
            userNameFilter: $('#UserNameFilterId').val(),
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
                  window.location = '/App/DepartmentUsers/ViewDepartmentUser/' + data.record.departmentUser.id;
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  window.location = '/App/DepartmentUsers/CreateOrEdit/' + data.record.departmentUser.id;
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
                    entityId: data.record.departmentUser.id,
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
                  deleteDepartmentUser(data.record.departmentUser);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'userName',
          name: 'userFk.name',
        },
        {
          targets: 3,
          data: 'tenantDepartmentName',
          name: 'tenantDepartmentFk.name',
        },
      ],
    });

    function getDepartmentUsers() {
      dataTable.ajax.reload();
    }

    function deleteDepartmentUser(departmentUser) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _departmentUsersService
            .delete({
              id: departmentUser.id,
            })
            .done(function () {
              getDepartmentUsers(true);
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
      _departmentUsersService
        .getDepartmentUsersToExcel({
          filter: $('#DepartmentUsersTableFilter').val(),
          userNameFilter: $('#UserNameFilterId').val(),
          tenantDepartmentNameFilter: $('#TenantDepartmentNameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditDepartmentUserModalSaved', function () {
      getDepartmentUsers();
    });

    $('#GetDepartmentUsersButton').click(function (e) {
      e.preventDefault();
      getDepartmentUsers();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getDepartmentUsers();
      }
    });
  });
})();
