(function () {
  $(function () {
    var _$confirmationValuesTable = $('#ConfirmationValuesTable');
    var _confirmationValuesService = abp.services.app.confirmationValues;
    var _entityTypeFullName = 'inzibackend.Surpath.ConfirmationValue';

      $('.date-picker').daterangepicker({
          singleDatePicker: true,
          
           locale: { format: 'MM/DD/YYYY', },
      });

      app.daterangefilterhelper.fixfilters();

    var _permissions = {
      create: abp.auth.hasPermission('Pages.ConfirmationValues.Create'),
      edit: abp.auth.hasPermission('Pages.ConfirmationValues.Edit'),
      delete: abp.auth.hasPermission('Pages.ConfirmationValues.Delete'),
    };

    var _viewConfirmationValueModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/ConfirmationValues/ViewconfirmationValueModal',
      modalClass: 'ViewConfirmationValueModal',
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

    var dataTable = _$confirmationValuesTable.DataTable({
        paging: true,
        lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
        pageLength: 5000,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _confirmationValuesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#ConfirmationValuesTableFilter').val(),
            minScreenValueFilter: $('#MinScreenValueFilterId').val(),
            maxScreenValueFilter: $('#MaxScreenValueFilterId').val(),
            minConfirmValueFilter: $('#MinConfirmValueFilterId').val(),
            maxConfirmValueFilter: $('#MaxConfirmValueFilterId').val(),
            unitOfMeasurementFilter: $('#UnitOfMeasurementFilterId').val(),
            drugNameFilter: $('#DrugNameFilterId').val(),
            testCategoryNameFilter: $('#TestCategoryNameFilterId').val(),
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
                  window.location = '/App/ConfirmationValues/ViewConfirmationValue/' + data.record.confirmationValue.id;
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  window.location = '/App/ConfirmationValues/CreateOrEdit/' + data.record.confirmationValue.id;
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
                    entityId: data.record.confirmationValue.id,
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
                  deleteConfirmationValue(data.record.confirmationValue);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'confirmationValue.screenValue',
          name: 'screenValue',
        },
        {
          targets: 3,
          data: 'confirmationValue.confirmValue',
          name: 'confirmValue',
        },
        {
          targets: 4,
          data: 'confirmationValue.unitOfMeasurement',
          name: 'unitOfMeasurement',
          render: function (unitOfMeasurement) {
            return app.localize('Enum_EnumUnitOfMeasurement_' + unitOfMeasurement);
          },
        },
        {
          targets: 5,
          data: 'drugName',
          name: 'drugFk.name',
        },
        {
          targets: 6,
          data: 'testCategoryName',
          name: 'testCategoryFk.name',
        },
      ],
    });

    function getConfirmationValues() {
      dataTable.ajax.reload();
    }

    function deleteConfirmationValue(confirmationValue) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _confirmationValuesService
            .delete({
              id: confirmationValue.id,
            })
            .done(function () {
              getConfirmationValues(true);
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
      _confirmationValuesService
        .getConfirmationValuesToExcel({
          filter: $('#ConfirmationValuesTableFilter').val(),
          minScreenValueFilter: $('#MinScreenValueFilterId').val(),
          maxScreenValueFilter: $('#MaxScreenValueFilterId').val(),
          minConfirmValueFilter: $('#MinConfirmValueFilterId').val(),
          maxConfirmValueFilter: $('#MaxConfirmValueFilterId').val(),
          unitOfMeasurementFilter: $('#UnitOfMeasurementFilterId').val(),
          drugNameFilter: $('#DrugNameFilterId').val(),
          testCategoryNameFilter: $('#TestCategoryNameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditConfirmationValueModalSaved', function () {
      getConfirmationValues();
    });

    $('#GetConfirmationValuesButton').click(function (e) {
      e.preventDefault();
      getConfirmationValues();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getConfirmationValues();
      }
    });
  });
})();
