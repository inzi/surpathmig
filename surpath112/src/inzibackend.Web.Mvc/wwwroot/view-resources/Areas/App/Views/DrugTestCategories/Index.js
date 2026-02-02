(function () {
  $(function () {
    var _$drugTestCategoriesTable = $('#DrugTestCategoriesTable');
    var _drugTestCategoriesService = abp.services.app.drugTestCategories;
    var _entityTypeFullName = 'inzibackend.Surpath.DrugTestCategory';

      $('.date-picker').daterangepicker({
          singleDatePicker: true,
          
           locale: { format: 'MM/DD/YYYY', },
      });

      app.daterangefilterhelper.fixfilters();

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.DrugTestCategories.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.DrugTestCategories.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.DrugTestCategories.Delete'),
    };

    var _viewDrugTestCategoryModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/DrugTestCategories/ViewdrugTestCategoryModal',
      modalClass: 'ViewDrugTestCategoryModal',
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

    var dataTable = _$drugTestCategoriesTable.DataTable({
        paging: true,
        lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
        pageLength: 5000,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _drugTestCategoriesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#DrugTestCategoriesTableFilter').val(),
            drugNameFilter: $('#DrugNameFilterId').val(),
            panelNameFilter: $('#PanelNameFilterId').val(),
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
                  window.location = '/App/DrugTestCategories/ViewDrugTestCategory/' + data.record.drugTestCategory.id;
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  window.location = '/App/DrugTestCategories/CreateOrEdit/' + data.record.drugTestCategory.id;
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
                    entityId: data.record.drugTestCategory.id,
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
                  deleteDrugTestCategory(data.record.drugTestCategory);
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
        {
          targets: 4,
          data: 'testCategoryName',
          name: 'testCategoryFk.name',
        },
      ],
    });

    function getDrugTestCategories() {
      dataTable.ajax.reload();
    }

    function deleteDrugTestCategory(drugTestCategory) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _drugTestCategoriesService
            .delete({
              id: drugTestCategory.id,
            })
            .done(function () {
              getDrugTestCategories(true);
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
      _drugTestCategoriesService
        .getDrugTestCategoriesToExcel({
          filter: $('#DrugTestCategoriesTableFilter').val(),
          drugNameFilter: $('#DrugNameFilterId').val(),
          panelNameFilter: $('#PanelNameFilterId').val(),
          testCategoryNameFilter: $('#TestCategoryNameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditDrugTestCategoryModalSaved', function () {
      getDrugTestCategories();
    });

    $('#GetDrugTestCategoriesButton').click(function (e) {
      e.preventDefault();
      getDrugTestCategories();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getDrugTestCategories();
      }
    });
  });
})();
