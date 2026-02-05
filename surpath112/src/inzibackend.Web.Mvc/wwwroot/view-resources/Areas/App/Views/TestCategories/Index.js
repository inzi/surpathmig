(function () {
  $(function () {
    var _$testCategoriesTable = $('#TestCategoriesTable');
    var _testCategoriesService = abp.services.app.testCategories;
    var _entityTypeFullName = 'inzibackend.Surpath.TestCategory';

      $('.date-picker').daterangepicker({
          singleDatePicker: true,
          
           locale: { format: 'MM/DD/YYYY', },
      });

      app.daterangefilterhelper.fixfilters();

    var _permissions = {
      create: abp.auth.hasPermission('Pages.TestCategories.Create'),
      edit: abp.auth.hasPermission('Pages.TestCategories.Edit'),
      delete: abp.auth.hasPermission('Pages.TestCategories.Delete'),
    };

    var _viewTestCategoryModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/TestCategories/ViewtestCategoryModal',
      modalClass: 'ViewTestCategoryModal',
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

    var dataTable = _$testCategoriesTable.DataTable({
        paging: true,
        lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
        pageLength: 5000,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _testCategoriesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#TestCategoriesTableFilter').val(),
            nameFilter: $('#NameFilterId').val(),
            internalNameFilter: $('#InternalNameFilterId').val(),
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
                  window.location = '/App/TestCategories/ViewTestCategory/' + data.record.testCategory.id;
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  window.location = '/App/TestCategories/CreateOrEdit/' + data.record.testCategory.id;
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
                    entityId: data.record.testCategory.id,
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
                  deleteTestCategory(data.record.testCategory);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'testCategory.name',
          name: 'name',
        },
        {
          targets: 3,
          data: 'testCategory.internalName',
          name: 'internalName',
        },
      ],
    });

    function getTestCategories() {
      dataTable.ajax.reload();
    }

    function deleteTestCategory(testCategory) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _testCategoriesService
            .delete({
              id: testCategory.id,
            })
            .done(function () {
              getTestCategories(true);
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
      _testCategoriesService
        .getTestCategoriesToExcel({
          filter: $('#TestCategoriesTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
          internalNameFilter: $('#InternalNameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditTestCategoryModalSaved', function () {
      getTestCategories();
    });

    $('#GetTestCategoriesButton').click(function (e) {
      e.preventDefault();
      getTestCategories();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getTestCategories();
      }
    });
  });
})();
