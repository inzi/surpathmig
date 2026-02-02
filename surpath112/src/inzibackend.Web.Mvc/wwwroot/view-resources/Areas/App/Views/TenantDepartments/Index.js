(function () {
  $(function () {
    var _$tenantDepartmentsTable = $('#TenantDepartmentsTable');
    var _tenantDepartmentsService = abp.services.app.tenantDepartments;
    var _entityTypeFullName = 'inzibackend.Surpath.TenantDepartment';

      $('.date-picker').daterangepicker({
          singleDatePicker: true,
          
           locale: { format: 'MM/DD/YYYY', },
      });

      app.daterangefilterhelper.fixfilters();

    var _permissions = {
      create: abp.auth.hasPermission('Pages.TenantDepartments.Create'),
      edit: abp.auth.hasPermission('Pages.TenantDepartments.Edit'),
      delete: abp.auth.hasPermission('Pages.TenantDepartments.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/TenantDepartments/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/TenantDepartments/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditTenantDepartmentModal',
    });

    var _viewTenantDepartmentModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/TenantDepartments/ViewtenantDepartmentModal',
      modalClass: 'ViewTenantDepartmentModal',
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

    var dataTable = _$tenantDepartmentsTable.DataTable({
        paging: true,
        lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
        pageLength: 5000,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _tenantDepartmentsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#TenantDepartmentsTableFilter').val(),
            nameFilter: $('#NameFilterId').val(),
            activeFilter: $('#ActiveFilterId').val(),
            mROTypeFilter: $('#MROTypeFilterId').val(),
            descriptionFilter: $('#DescriptionFilterId').val(),
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
                  _viewTenantDepartmentModal.open({ id: data.record.tenantDepartment.id });
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.tenantDepartment.id });
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
                    entityId: data.record.tenantDepartment.id,
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
                  deleteTenantDepartment(data.record.tenantDepartment);
                },
              },
            ],
          },
        },
        {
          className: 'details-control',
          targets: 2,
          orderable: false,
          autoWidth: false,
          visible: abp.auth.hasPermission('Pages.TenantDepartmentUsers'),
          render: function () {
            return `<button class="btn btn-primary btn-xs Edit_TenantDepartmentUser_TenantDepartmentId">${app.localize(
              'EditTenantDepartmentUser'
            )}</button>`;
          },
        },
        {
          targets: 3,
          data: 'tenantDepartment.name',
          name: 'name',
        },
        {
          targets: 4,
          data: 'tenantDepartment.active',
          name: 'active',
          render: function (active) {
            if (active) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
        {
          targets: 5,
          data: 'tenantDepartment.mroType',
          name: 'mroType',
          render: function (mroType) {
            return app.localize('Enum_EnumClientMROTypes_' + mroType);
          },
        },
        {
          targets: 6,
          data: 'tenantDepartment.description',
          name: 'description',
        },
      ],
    });

    function getTenantDepartments() {
      dataTable.ajax.reload();
    }

    function deleteTenantDepartment(tenantDepartment) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _tenantDepartmentsService
            .delete({
              id: tenantDepartment.id,
            })
            .done(function () {
              getTenantDepartments(true);
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

    $('#CreateNewTenantDepartmentButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _tenantDepartmentsService
        .getTenantDepartmentsToExcel({
          filter: $('#TenantDepartmentsTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
          activeFilter: $('#ActiveFilterId').val(),
          mROTypeFilter: $('#MROTypeFilterId').val(),
          descriptionFilter: $('#DescriptionFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditTenantDepartmentModalSaved', function () {
      getTenantDepartments();
    });

    $('#GetTenantDepartmentsButton').click(function (e) {
      e.preventDefault();
      getTenantDepartments();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getTenantDepartments();
      }
    });

    var currentOpenedDetailRow;
    function openDetailRow(e, url) {
      var tr = $(e).closest('tr');
      var row = dataTable.row(tr);

      if (row.child.isShown()) {
        row.child.hide();
        tr.removeClass('shown');
        currentOpenedDetailRow = null;
      } else {
        if (currentOpenedDetailRow) currentOpenedDetailRow.child.hide();

        $.get(url).then((data) => {
          row.child(data).show();
          tr.addClass('shown');
          currentOpenedDetailRow = row;
        });
      }
    }

    _$tenantDepartmentsTable.on('click', '.Edit_TenantDepartmentUser_TenantDepartmentId', function () {
      var tr = $(this).closest('tr');
      var row = dataTable.row(tr);
      openDetailRow(
        this,
        '/App/MasterDetailChild_TenantDepartment_TenantDepartmentUsers?TenantDepartmentId=' +
          row.data().tenantDepartment.id
      );
    });
  });
})();
