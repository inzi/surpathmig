(function () {
  $(function () {
    var _$recordsTable = $('#MasterDetailChild_TenantDocumentCategory_RecordsTable');
    var _recordsService = abp.services.app.records;
    var _entityTypeFullName = 'inzibackend.Surpath.Record';

    var $selectedDate = {
      startDate: null,
      endDate: null,
    };

    $('.date-picker').on('apply.daterangepicker', function (ev, picker) {
      $(this).val(picker.startDate.format('MM/DD/YYYY'));
    });

    $('.startDate')
      .daterangepicker(
        {
          autoUpdateInput: false,
          singleDatePicker: true,
          locale: abp.localization.currentLanguage.name,
          format: 'L',
        },
        (date) => {
          $selectedDate.startDate = date;
        }
      )
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
      });

    $('.endDate')
      .daterangepicker(
        {
          autoUpdateInput: false,
          singleDatePicker: true,
          locale: abp.localization.currentLanguage.name,
          format: 'L',
        },
        (date) => {
          $selectedDate.endDate = date;
        }
      )
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Records.Create'),
      edit: abp.auth.hasPermission('Pages.Records.Edit'),
      delete: abp.auth.hasPermission('Pages.Records.Delete'),
    };

    var _viewRecordModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/MasterDetailChild_TenantDocumentCategory_Records/ViewrecordModal',
      modalClass: 'MasterDetailChild_TenantDocumentCategory_ViewRecordModal',
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

    var dataTable = _$recordsTable.DataTable({
        paging: true,
        lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
        pageLength: 5000,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _recordsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#MasterDetailChild_TenantDocumentCategory_RecordsTableFilter').val(),
            filedataFilter: $('#MasterDetailChild_TenantDocumentCategory_filedataFilterId').val(),
            filenameFilter: $('#MasterDetailChild_TenantDocumentCategory_filenameFilterId').val(),
            physicalfilepathFilter: $('#MasterDetailChild_TenantDocumentCategory_physicalfilepathFilterId').val(),
            metadataFilter: $('#MasterDetailChild_TenantDocumentCategory_metadataFilterId').val(),
            binaryObjIdFilter: $('#MasterDetailChild_TenantDocumentCategory_BinaryObjIdFilterId').val(),
            tenantDocumentCategoryIdFilter: $('#MasterDetailChild_TenantDocumentCategory_RecordsId').val(),
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
                  window.location = '/App/Records/ViewRecord/' + data.record.record.id;
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  window.location = '/App/Records/CreateOrEdit/' + data.record.record.id;
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
                    entityId: data.record.record.id,
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
                  deleteRecord(data.record.record);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'record',
          render: function (record) {
            if (!record.filedata) {
              return '';
            }
            return `<a href="/File/DownloadBinaryFile?id=${record.filedata}" target="_blank">${record.filedataFileName}</a>`;
          },
        },
        {
          targets: 3,
          data: 'record.filename',
          name: 'filename',
        },
        {
          targets: 4,
          data: 'record.physicalfilepath',
          name: 'physicalfilepath',
        },
        {
          targets: 5,
          data: 'record.metadata',
          name: 'metadata',
        },
        {
          targets: 6,
          data: 'record.binaryObjId',
          name: 'binaryObjId',
        },
      ],
    });

    function getRecords() {
      dataTable.ajax.reload();
    }

    function deleteRecord(record) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _recordsService
            .delete({
              id: record.id,
            })
            .done(function () {
              getRecords(true);
              abp.notify.success(app.localize('SuccessfullyDeleted'));
            });
        }
      });
    }

    $('#MasterDetailChild_TenantDocumentCategory_ShowAdvancedFiltersSpan').click(function () {
      $('#MasterDetailChild_TenantDocumentCategory_ShowAdvancedFiltersSpan').hide();
      $('#MasterDetailChild_TenantDocumentCategory_HideAdvancedFiltersSpan').show();
      $('#MasterDetailChild_TenantDocumentCategory_AdvacedAuditFiltersArea').slideDown();
    });

    $('#MasterDetailChild_TenantDocumentCategory_HideAdvancedFiltersSpan').click(function () {
      $('#MasterDetailChild_TenantDocumentCategory_HideAdvancedFiltersSpan').hide();
      $('#MasterDetailChild_TenantDocumentCategory_ShowAdvancedFiltersSpan').show();
      $('#MasterDetailChild_TenantDocumentCategory_AdvacedAuditFiltersArea').slideUp();
    });

    $('#ExportToExcelButton').click(function () {
      _recordsService
        .getRecordsToExcel({
          filter: $('#RecordsTableFilter').val(),
          filedataFilter: $('#MasterDetailChild_TenantDocumentCategory_filedataFilterId').val(),
          filenameFilter: $('#MasterDetailChild_TenantDocumentCategory_filenameFilterId').val(),
          physicalfilepathFilter: $('#MasterDetailChild_TenantDocumentCategory_physicalfilepathFilterId').val(),
          metadataFilter: $('#MasterDetailChild_TenantDocumentCategory_metadataFilterId').val(),
          binaryObjIdFilter: $('#MasterDetailChild_TenantDocumentCategory_BinaryObjIdFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditRecordModalSaved', function () {
      getRecords();
    });

    $('#GetRecordsButton').click(function (e) {
      e.preventDefault();
      getRecords();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getRecords();
      }
    });
  });
})();
