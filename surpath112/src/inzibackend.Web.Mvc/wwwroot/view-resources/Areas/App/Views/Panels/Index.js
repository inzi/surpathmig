(function () {
  $(function () {
    var _$panelsTable = $('#PanelsTable');
    var _panelsService = abp.services.app.panels;
    var _entityTypeFullName = 'inzibackend.Surpath.Panel';

      $('.date-picker').daterangepicker({
          singleDatePicker: true,
          
           locale: { format: 'MM/DD/YYYY', },
      });

      app.daterangefilterhelper.fixfilters();

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.Panels.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.Panels.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.Panels.Delete'),
    };

    var _viewPanelModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Panels/ViewpanelModal',
      modalClass: 'ViewPanelModal',
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

    var dataTable = _$panelsTable.DataTable({
        paging: true,
        lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
        pageLength: 5000,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _panelsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#PanelsTableFilter').val(),
            nameFilter: $('#NameFilterId').val(),
            minCostFilter: $('#MinCostFilterId').val(),
            maxCostFilter: $('#MaxCostFilterId').val(),
            descriptionFilter: $('#DescriptionFilterId').val(),
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
                  window.location = '/App/Panels/ViewPanel/' + data.record.panel.id;
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  window.location = '/App/Panels/CreateOrEdit/' + data.record.panel.id;
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
                    entityId: data.record.panel.id,
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
                  deletePanel(data.record.panel);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'panel.name',
          name: 'name',
        },
        {
          targets: 3,
          data: 'panel.cost',
          name: 'cost',
        },
        {
          targets: 4,
          data: 'panel.description',
          name: 'description',
        },
        {
          targets: 5,
          data: 'testCategoryName',
          name: 'testCategoryFk.name',
        },
      ],
    });

    function getPanels() {
      dataTable.ajax.reload();
    }

    function deletePanel(panel) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _panelsService
            .delete({
              id: panel.id,
            })
            .done(function () {
              getPanels(true);
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
      _panelsService
        .getPanelsToExcel({
          filter: $('#PanelsTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
          minCostFilter: $('#MinCostFilterId').val(),
          maxCostFilter: $('#MaxCostFilterId').val(),
          descriptionFilter: $('#DescriptionFilterId').val(),
          testCategoryNameFilter: $('#TestCategoryNameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditPanelModalSaved', function () {
      getPanels();
    });

    $('#GetPanelsButton').click(function (e) {
      e.preventDefault();
      getPanels();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getPanels();
      }
    });
  });
})();
