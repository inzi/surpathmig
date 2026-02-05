(function () {
  $(function () {
    var _$recordCategoriesTable = $('#RecordCategoriesTable');
    var _recordCategoriesService = abp.services.app.recordCategories;
    var _entityTypeFullName = 'inzibackend.Surpath.RecordCategory';

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
        getRecordCategories();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getRecordCategories();
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
        getRecordCategories();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getRecordCategories();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.RecordCategories.Create'),
      edit: abp.auth.hasPermission('Pages.RecordCategories.Edit'),
      delete: abp.auth.hasPermission('Pages.RecordCategories.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RecordCategories/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RecordCategories/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditRecordCategoryModal',
    });

    var _viewRecordCategoryModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RecordCategories/ViewrecordCategoryModal',
      modalClass: 'ViewRecordCategoryModal',
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

    var dataTable = _$recordCategoriesTable.DataTable({
        paging: true,
        lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
        pageLength: 5000,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _recordCategoriesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#RecordCategoriesTableFilter').val(),
            nameFilter: $('#NameFilterId').val(),
            instructionsFilter: $('#InstructionsFilterId').val(),
            recordRequirementNameFilter: $('#RecordRequirementNameFilterId').val(),
            recordCategoryRuleNameFilter: $('#RecordCategoryRuleNameFilterId').val(),
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
                  _viewRecordCategoryModal.open({ id: data.record.recordCategory.id });
                },
              },
              {
                  text: app.localize('Edit'),
                  iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.recordCategory.id });
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
                    entityId: data.record.recordCategory.id,
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
                  deleteRecordCategory(data.record.recordCategory);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'recordCategory.name',
          name: 'name',
        },
        {
          targets: 3,
          data: 'recordCategory.instructions',
          name: 'instructions',
        },
        {
          targets: 4,
          data: 'recordRequirementName',
          name: 'recordRequirementFk.name',
        },
        {
          targets: 5,
          data: 'recordCategoryRuleName',
          name: 'recordCategoryRuleFk.name',
        },
      ],
    });

    function getRecordCategories() {
      dataTable.ajax.reload();
    }

    function deleteRecordCategory(recordCategory) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _recordCategoriesService
            .delete({
              id: recordCategory.id,
            })
            .done(function () {
              getRecordCategories(true);
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

    $('#CreateNewRecordCategoryButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _recordCategoriesService
        .getRecordCategoriesToExcel({
          filter: $('#RecordCategoriesTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
          instructionsFilter: $('#InstructionsFilterId').val(),
          recordRequirementNameFilter: $('#RecordRequirementNameFilterId').val(),
          recordCategoryRuleNameFilter: $('#RecordCategoryRuleNameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditRecordCategoryModalSaved', function () {
      getRecordCategories();
    });

    $('#GetRecordCategoriesButton').click(function (e) {
      e.preventDefault();
      getRecordCategories();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getRecordCategories();
      }
    });

    $('.reload-on-change').change(function (e) {
      getRecordCategories();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getRecordCategories();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getRecordCategories();
    });
  });
})();
