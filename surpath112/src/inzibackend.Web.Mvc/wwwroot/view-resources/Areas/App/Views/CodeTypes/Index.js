(function () {
  $(function () {
    var _$codeTypesTable = $('#CodeTypesTable');
    var _codeTypesService = abp.services.app.codeTypes;
    var _entityTypeFullName = 'inzibackend.Surpath.CodeType';

      $('.date-picker').daterangepicker({
          singleDatePicker: true,
          
           locale: { format: 'MM/DD/YYYY', },
      });

      app.daterangefilterhelper.fixfilters();

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.CodeTypes.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.CodeTypes.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.CodeTypes.Delete'),
    };

    var _viewCodeTypeModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/CodeTypes/ViewcodeTypeModal',
      modalClass: 'ViewCodeTypeModal',
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

    var dataTable = _$codeTypesTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _codeTypesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#CodeTypesTableFilter').val(),
            nameFilter: $('#NameFilterId').val(),
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
                  window.location = '/App/CodeTypes/ViewCodeType/' + data.record.codeType.id;
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  window.location = '/App/CodeTypes/CreateOrEdit/' + data.record.codeType.id;
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
                    entityId: data.record.codeType.id,
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
                  deleteCodeType(data.record.codeType);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'codeType.name',
          name: 'name',
        },
      ],
    });

    function getCodeTypes() {
      dataTable.ajax.reload();
    }

    function deleteCodeType(codeType) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _codeTypesService
            .delete({
              id: codeType.id,
            })
            .done(function () {
              getCodeTypes(true);
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
      _codeTypesService
        .getCodeTypesToExcel({
          filter: $('#CodeTypesTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditCodeTypeModalSaved', function () {
      getCodeTypes();
    });

    $('#GetCodeTypesButton').click(function (e) {
      e.preventDefault();
      getCodeTypes();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getCodeTypes();
      }
    });
  });
})();
