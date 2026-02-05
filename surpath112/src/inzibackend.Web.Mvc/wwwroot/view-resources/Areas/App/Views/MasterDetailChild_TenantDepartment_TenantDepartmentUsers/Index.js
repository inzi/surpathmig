(function () {
  $(function () {
    var _$tenantDepartmentUsersTable = $('#MasterDetailChild_TenantDepartment_TenantDepartmentUsersTable');
    var _tenantDepartmentUsersService = abp.services.app.tenantDepartmentUsers;
    var _entityTypeFullName = 'inzibackend.Surpath.TenantDepartmentUser';

      $('.date-picker').daterangepicker({
          singleDatePicker: true,
          
           locale: { format: 'MM/DD/YYYY', },
      });

      app.daterangefilterhelper.fixfilters();

    var _permissions = {
      create: abp.auth.hasPermission('Pages.TenantDepartmentUsers.Create'),
      edit: abp.auth.hasPermission('Pages.TenantDepartmentUsers.Edit'),
      delete: abp.auth.hasPermission('Pages.TenantDepartmentUsers.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/MasterDetailChild_TenantDepartment_TenantDepartmentUsers/CreateOrEditModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/App/Views/MasterDetailChild_TenantDepartment_TenantDepartmentUsers/_CreateOrEditModal.js',
      modalClass: 'MasterDetailChild_TenantDepartment_CreateOrEditTenantDepartmentUserModal',
    });

    var _viewTenantDepartmentUserModal = new app.ModalManager({
      viewUrl:
        abp.appPath + 'App/MasterDetailChild_TenantDepartment_TenantDepartmentUsers/ViewtenantDepartmentUserModal',
      modalClass: 'MasterDetailChild_TenantDepartment_ViewTenantDepartmentUserModal',
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

    var dataTable = _$tenantDepartmentUsersTable.DataTable({
        paging: true,
        lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
        pageLength: 5000,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _tenantDepartmentUsersService.getAll,
        inputFilter: function () {
          return {
            filter: $('#MasterDetailChild_TenantDepartment_TenantDepartmentUsersTableFilter').val(),
            userNameFilter: $('#MasterDetailChild_TenantDepartment_UserNameFilterId').val(),
            tenantDepartmentIdFilter: $('#MasterDetailChild_TenantDepartment_TenantDepartmentUsersId').val(),
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
                  _viewTenantDepartmentUserModal.open({ id: data.record.tenantDepartmentUser.id });
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.tenantDepartmentUser.id });
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
                    entityId: data.record.tenantDepartmentUser.id,
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
                  deleteTenantDepartmentUser(data.record.tenantDepartmentUser);
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
      ],
    });

    function getTenantDepartmentUsers() {
      dataTable.ajax.reload();
    }

    function deleteTenantDepartmentUser(tenantDepartmentUser) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _tenantDepartmentUsersService
            .delete({
              id: tenantDepartmentUser.id,
            })
            .done(function () {
              getTenantDepartmentUsers(true);
              abp.notify.success(app.localize('SuccessfullyDeleted'));
            });
        }
      });
    }

    $('#MasterDetailChild_TenantDepartment_ShowAdvancedFiltersSpan').click(function () {
      $('#MasterDetailChild_TenantDepartment_ShowAdvancedFiltersSpan').hide();
      $('#MasterDetailChild_TenantDepartment_HideAdvancedFiltersSpan').show();
      $('#MasterDetailChild_TenantDepartment_AdvacedAuditFiltersArea').slideDown();
    });

    $('#MasterDetailChild_TenantDepartment_HideAdvancedFiltersSpan').click(function () {
      $('#MasterDetailChild_TenantDepartment_HideAdvancedFiltersSpan').hide();
      $('#MasterDetailChild_TenantDepartment_ShowAdvancedFiltersSpan').show();
      $('#MasterDetailChild_TenantDepartment_AdvacedAuditFiltersArea').slideUp();
    });

    $('#CreateNewTenantDepartmentUserButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _tenantDepartmentUsersService
        .getTenantDepartmentUsersToExcel({
          filter: $('#TenantDepartmentUsersTableFilter').val(),
          userNameFilter: $('#MasterDetailChild_TenantDepartment_UserNameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditTenantDepartmentUserModalSaved', function () {
      getTenantDepartmentUsers();
    });

    $('#GetTenantDepartmentUsersButton').click(function (e) {
      e.preventDefault();
      getTenantDepartmentUsers();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getTenantDepartmentUsers();
      }
    });
  });
})();
