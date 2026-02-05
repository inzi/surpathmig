(function () {
  $(function () {
    var _$recordsTable = $('#RecordsTable');
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
      .daterangepicker({
        autoUpdateInput: false,
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      })
      .on('apply.daterangepicker', (ev, picker) => {
        $selectedDate.startDate = picker.startDate;
        getRecords();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getRecords();
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
        getRecords();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getRecords();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Records.Create'),
      edit: abp.auth.hasPermission('Pages.Records.Edit'),
      delete: abp.auth.hasPermission('Pages.Records.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Records/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Records/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditRecordModal',
    });

    var _viewRecordModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Records/ViewrecordModal',
      modalClass: 'ViewRecordModal',
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
            filter: $('#RecordsTableFilter').val(),
            filedataFilter: $('#filedataFilterId').val(),
            filenameFilter: $('#filenameFilterId').val(),
            physicalfilepathFilter: $('#physicalfilepathFilterId').val(),
            metadataFilter: $('#metadataFilterId').val(),
            binaryObjIdFilter: $('#BinaryObjIdFilterId').val(),
            minDateUploadedFilter: getDateFilter($('#MinDateUploadedFilterId')),
            maxDateUploadedFilter: getMaxDateFilter($('#MaxDateUploadedFilterId')),
            minDateLastUpdatedFilter: getDateFilter($('#MinDateLastUpdatedFilterId')),
            maxDateLastUpdatedFilter: getMaxDateFilter($('#MaxDateLastUpdatedFilterId')),
            instructionsConfirmedFilter: $('#InstructionsConfirmedFilterId').val(),
            minEffectiveDateFilter: getDateFilter($('#MinEffectiveDateFilterId')),
            maxEffectiveDateFilter: getMaxDateFilter($('#MaxEffectiveDateFilterId')),
            minExpirationDateFilter: getDateFilter($('#MinExpirationDateFilterId')),
            maxExpirationDateFilter: getMaxDateFilter($('#MaxExpirationDateFilterId')),
            tenantDocumentCategoryNameFilter: $('#TenantDocumentCategoryNameFilterId').val(),
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
                  _viewRecordModal.open({ id: data.record.record.id });
                },
              },
              {
                  text: app.localize('Edit'),
                  iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.record.id });
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
        {
          targets: 7,
          data: 'record.dateUploaded',
          name: 'dateUploaded',
          render: function (dateUploaded) {
            if (dateUploaded) {
              return moment(dateUploaded).format('L');
            }
            return '';
          },
        },
        {
          targets: 8,
          data: 'record.dateLastUpdated',
          name: 'dateLastUpdated',
          render: function (dateLastUpdated) {
            if (dateLastUpdated) {
              return moment(dateLastUpdated).format('L');
            }
            return '';
          },
        },
        {
          targets: 9,
          data: 'record.instructionsConfirmed',
          name: 'instructionsConfirmed',
          render: function (instructionsConfirmed) {
            if (instructionsConfirmed) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
        //{
        //  targets: 10,
        //  data: 'record.effectiveDate',
        //  name: 'effectiveDate',
        //  render: function (effectiveDate) {
        //    if (effectiveDate) {
        //      return moment(effectiveDate).format('L');
        //    }
        //    return '';
        //  },
        //},
        //{
        //  targets: 11,
        //  data: 'record.expirationDate',
        //  name: 'expirationDate',
        //  render: function (expirationDate) {
        //    if (expirationDate) {
        //      return moment(expirationDate).format('L');
        //    }
        //    return '';
        //  },
        //},
        //{
        //  targets: 12,
        //  data: 'record.metaData',
        //  name: 'metaData',
        //},
        {
          targets: 10,
          data: 'tenantDocumentCategoryName',
          name: 'tenantDocumentCategoryFk.name',
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

    $('#CreateNewRecordButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _recordsService
        .getRecordsToExcel({
          filter: $('#RecordsTableFilter').val(),
          filedataFilter: $('#filedataFilterId').val(),
          filenameFilter: $('#filenameFilterId').val(),
          physicalfilepathFilter: $('#physicalfilepathFilterId').val(),
          metadataFilter: $('#metadataFilterId').val(),
          binaryObjIdFilter: $('#BinaryObjIdFilterId').val(),
          minDateUploadedFilter: getDateFilter($('#MinDateUploadedFilterId')),
          maxDateUploadedFilter: getMaxDateFilter($('#MaxDateUploadedFilterId')),
          minDateLastUpdatedFilter: getDateFilter($('#MinDateLastUpdatedFilterId')),
          maxDateLastUpdatedFilter: getMaxDateFilter($('#MaxDateLastUpdatedFilterId')),
          instructionsConfirmedFilter: $('#InstructionsConfirmedFilterId').val(),
          minEffectiveDateFilter: getDateFilter($('#MinEffectiveDateFilterId')),
          maxEffectiveDateFilter: getMaxDateFilter($('#MaxEffectiveDateFilterId')),
          minExpirationDateFilter: getDateFilter($('#MinExpirationDateFilterId')),
          maxExpirationDateFilter: getMaxDateFilter($('#MaxExpirationDateFilterId')),
          tenantDocumentCategoryNameFilter: $('#TenantDocumentCategoryNameFilterId').val(),
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

    $('.reload-on-change').change(function (e) {
      getRecords();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getRecords();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getRecords();
    });
  });
})();
