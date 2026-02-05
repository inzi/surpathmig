(function () {
  $(function () {
    var _$drugPanelsTable = $('#DrugPanelsTable');
    var _drugPanelsService = abp.services.app.drugPanels;
    var _entityTypeFullName = 'inzibackend.Surpath.DrugPanel';

      $('.date-picker').daterangepicker({
          singleDatePicker: true,
          
           locale: { format: 'MM/DD/YYYY', },
      });

      app.daterangefilterhelper.fixfilters();

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.DrugPanels.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.DrugPanels.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.DrugPanels.Delete'),
    };

    var _viewDrugPanelModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/DrugPanels/ViewdrugPanelModal',
      modalClass: 'ViewDrugPanelModal',
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

    var dataTable = _$drugPanelsTable.DataTable({
        paging: false,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _drugPanelsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#DrugPanelsTableFilter').val(),
            drugNameFilter: $('#DrugNameFilterId').val(),
            panelNameFilter: $('#PanelNameFilterId').val(),
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
                  window.location = '/App/DrugPanels/ViewDrugPanel/' + data.record.drugPanel.id;
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  window.location = '/App/DrugPanels/CreateOrEdit/' + data.record.drugPanel.id;
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
                    entityId: data.record.drugPanel.id,
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
                  deleteDrugPanel(data.record.drugPanel);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'drugName',
          name: 'drugFk.name',
        },
        {
          targets: 3,
          data: 'panelName',
          name: 'panelFk.name',
        },
      ],
    });

    function getDrugPanels() {
      dataTable.ajax.reload();
    }

    function deleteDrugPanel(drugPanel) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _drugPanelsService
            .delete({
              id: drugPanel.id,
            })
            .done(function () {
              getDrugPanels(true);
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
      _drugPanelsService
        .getDrugPanelsToExcel({
          filter: $('#DrugPanelsTableFilter').val(),
          drugNameFilter: $('#DrugNameFilterId').val(),
          panelNameFilter: $('#PanelNameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditDrugPanelModalSaved', function () {
      getDrugPanels();
    });

    $('#GetDrugPanelsButton').click(function (e) {
      e.preventDefault();
      getDrugPanels();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getDrugPanels();
      }
    });
  });
})();
