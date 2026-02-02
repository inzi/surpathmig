(function () {
  $(function () {
    var _$recordStatusesTable = $('#RecordStatusesTable');
    var _recordStatusesService = abp.services.app.recordStatuses;
    var _entityTypeFullName = 'inzibackend.Surpath.RecordStatus';

    var $selectedDate = {
      startDate: null,
      endDate: null,
    };

    $('.date-picker').on('apply.daterangepicker', function (ev, picker) {
      $(this).val(picker.startDate.format('MM/DD/YYYY'));
    });

    $('.startDate')
      .daterangepicker({
        autoUpdateInput: false,
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      })
      .on('apply.daterangepicker', (ev, picker) => {
        $selectedDate.startDate = picker.startDate;
        getRecordStatuses();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getRecordStatuses();
      });

    $('.endDate')
      .daterangepicker({
        autoUpdateInput: false,
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      })
      .on('apply.daterangepicker', (ev, picker) => {
        $selectedDate.endDate = picker.startDate;
        getRecordStatuses();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getRecordStatuses();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.RecordStatuses.Create'),
      edit: abp.auth.hasPermission('Pages.RecordStatuses.Edit'),
        delete: abp.auth.hasPermission('Pages.RecordStatuses.Delete'),
        isHost: abp.auth.hasPermission('Pages.Administration.Host.Dashboard'),

    };

    var _viewRecordStatusModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RecordStatuses/ViewrecordStatusModal',
      modalClass: 'ViewRecordStatusModal',
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
      if ($selectedDate.startDate == null) {
        return null;
      }
      return $selectedDate.startDate.format('YYYY-MM-DDT00:00:00Z');
    };

    var getMaxDateFilter = function (element) {
      if ($selectedDate.endDate == null) {
        return null;
      }
      return $selectedDate.endDate.format('YYYY-MM-DDT23:59:59Z');
    };

    var dataTable = _$recordStatusesTable.DataTable({
        paging: true,
        lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
        pageLength: 5000,
      serverSide: true,
      processing: true,
      createdRow: function(row, data, dataIndex) {
        if (data.recordStatus.isSurpathServiceStatus) {
          $(row).css('background-color', '#e6f3ff');  // Light blue background
        }
      },
      listAction: {
        ajaxFunction: _recordStatusesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#RecordStatusesTableFilter').val(),
            statusNameFilter: $('#StatusNameFilterId').val(),
            htmlColorFilter: $('#HtmlColorFilterId').val(),
            cSSCLassFilter: $('#CSSCLassFilterId').val(),
            isDefaultFilter: $('#IsDefaultFilterId').val(),
            requireNoteOnSetFilter: $('#RequireNoteOnSetFilterId').val(),
            isSurpathServiceStatusFilter: $('#IsSurpathServiceStatusFilterId').val(),
            complianceImpactFilter: $('#ComplianceImpactFilterId').val(),
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
                action: function (data) {
                  window.location = '/App/RecordStatuses/ViewRecordStatus/' + data.record.recordStatus.id;
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  window.location = '/App/RecordStatuses/CreateOrEdit/' + data.record.recordStatus.id;
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
                    entityId: data.record.recordStatus.id,
                  });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteRecordStatus(data.record.recordStatus);
                },
              },
            ],
          },
        },
        {
                targets: 2,
                data: null, //'recordStatus.statusName',
                name: 'statusName',
                render: function (data, row, full) {
                    //console.log(data);
                    console.log(data.recordStatus.htmlColor);
                    //console.log(row);
                    //console.log(full);
                    // return app.localize('Enum_EnumRecordState_' + state);
                    /*return '<span class="label font-weight-bold label-lg label-light-danger label-inline">Danger</span>';*/
                    //return '<span class="badge" data-toggle="tooltip" title="" data-placement="top" style="margin-right: 5px;" data-bs-original-title="Assign to new users by default.">' + data.recordStatusStatusName + '</span>'
                    // return '<span class="label pulse pulse-success mr-10"><span class="position-relative">1</span><span class="pulse-ring"></span></span >';
                    //return '<div style="background: ' + data.recordStatus.htmlColor+ '">' + data.recordStatusStatusName + '</div>';
                    return '<span class="badge record-status-badge" style="background: ' + data.recordStatus.htmlColor + '">' + data.recordStatus.statusName + '</span>';
                    //return '';
                },
            },
            //{
            //  targets: 3,
            //  data: 'recordStatus.htmlColor',
            //  name: 'htmlColor',
            //},
            //{
            //  targets: 4,
            //  data: 'recordStatus.csscLass',
            //  name: 'csscLass',
            //},
            {
                targets: 3,
                data: 'recordStatus.isDefault',
                name: 'isDefault',
                render: function (isDefault) {
                    if (isDefault) {
                        return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
                    }
                    return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
                },
            },
            {
                targets: 4,
                data: 'recordStatus.requireNoteOnSet',
                name: 'requireNoteOnSet',
                render: function (requireNoteOnSet) {
                    if (requireNoteOnSet) {
                        return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
                    }
                    return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
                },
            },
            {
                targets: 5,
                data: 'recordStatus.complianceImpact',
                name: 'complianceImpact',
                render: function (complianceImpact) {
                    return app.localize('Enum_EnumStatusComplianceImpact_' + complianceImpact);
                },
            },
            {
                targets: 6,
                data: 'tenantDepartmentName',
                name: 'tenantDepartmentFk.name',
            },
            {
                targets: 7,
                data: 'tenantName',
                name: 'tenantDepartmentFk.name',

            },
            {
                targets: 8,
                data: 'recordStatus.isSurpathServiceStatus',
                name: 'isSurpathServiceStatus',
                render: function (isSurpathServiceStatus) {
                    if (isSurpathServiceStatus) {
                        return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
                    }
                    return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
                },
            },



        ],
    })
        .columns([6, 7, 8]).visible(_permissions.isHost);

    function getRecordStatuses() {
      dataTable.ajax.reload();
    }

    function deleteRecordStatus(recordStatus) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _recordStatusesService
            .delete({
              id: recordStatus.id,
            })
            .done(function () {
              getRecordStatuses(true);
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
      _recordStatusesService
        .getRecordStatusesToExcel({
          filter: $('#RecordStatusesTableFilter').val(),
          statusNameFilter: $('#StatusNameFilterId').val(),
          htmlColorFilter: $('#HtmlColorFilterId').val(),
          cSSCLassFilter: $('#CSSCLassFilterId').val(),
          isDefaultFilter: $('#IsDefaultFilterId').val(),
          requireNoteOnSetFilter: $('#RequireNoteOnSetFilterId').val(),
          isSurpathServiceStatusFilter: $('#IsSurpathServiceStatusFilterId').val(),
          complianceImpactFilter: $('#ComplianceImpactFilterId').val(),
          tenantDepartmentNameFilter: $('#TenantDepartmentNameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditRecordStatusModalSaved', function () {
      getRecordStatuses();
    });

    $('#GetRecordStatusesButton').click(function (e) {
      e.preventDefault();
      getRecordStatuses();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getRecordStatuses();
      }
    });

    $('.reload-on-change').change(function (e) {
      getRecordStatuses();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getRecordStatuses();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getRecordStatuses();
    });
  });
})();
