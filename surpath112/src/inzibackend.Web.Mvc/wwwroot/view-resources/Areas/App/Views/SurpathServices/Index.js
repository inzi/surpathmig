(function () {
  $(function () {
    var _$surpathServicesTable = $('#SurpathServicesTable');
    var _surpathServicesService = abp.services.app.surpathServices;
    var _entityTypeFullName = 'inzibackend.Surpath.SurpathService';

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
        getSurpathServices();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getSurpathServices();
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
        getSurpathServices();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getSurpathServices();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.SurpathServices.Create'),
      edit: abp.auth.hasPermission('Pages.SurpathServices.Edit'),
      delete: abp.auth.hasPermission('Pages.SurpathServices.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/SurpathServices/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/SurpathServices/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditSurpathServiceModal',
    });

    var _viewSurpathServiceModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/SurpathServices/ViewsurpathServiceModal',
      modalClass: 'ViewSurpathServiceModal',
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

    var dataTable = _$surpathServicesTable.DataTable({
        paging: true,
        lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
        pageLength: 5000,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _surpathServicesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#SurpathServicesTableFilter').val(),
            nameFilter: $('#NameFilterId').val(),
            minPriceFilter: $('#MinPriceFilterId').val(),
            maxPriceFilter: $('#MaxPriceFilterId').val(),
            minDiscountFilter: $('#MinDiscountFilterId').val(),
            maxDiscountFilter: $('#MaxDiscountFilterId').val(),
            descriptionFilter: $('#DescriptionFilterId').val(),
            isEnabledByDefaultFilter: $('#IsEnabledByDefaultFilterId').val(),
            tenantDepartmentNameFilter: $('#TenantDepartmentNameFilterId').val(),
            cohortNameFilter: $('#CohortNameFilterId').val(),
            userNameFilter: $('#UserNameFilterId').val(),
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
                action: function (data) {
                  _viewSurpathServiceModal.open({ id: data.record.surpathService.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.surpathService.id });
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
                    entityId: data.record.surpathService.id,
                  });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteSurpathService(data.record.surpathService);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'surpathService.name',
          name: 'name',
        },
        {
          targets: 3,
          data: 'surpathService.price',
          name: 'price',
        },
        {
          targets: 4,
          data: 'surpathService.discount',
          name: 'discount',
        },
        {
          targets: 5,
          data: 'surpathService.description',
          name: 'description',
        },
        {
          targets: 6,
          data: 'surpathService.isEnabledByDefault',
          name: 'isEnabledByDefault',
          render: function (isEnabledByDefault) {
            if (isEnabledByDefault) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
        {
          targets: 7,
          data: 'tenantDepartmentName',
          name: 'tenantDepartmentFk.name',
        },
        {
          targets: 8,
          data: 'cohortName',
          name: 'cohortFk.name',
        },
        {
          targets: 9,
          data: 'userName',
          name: 'userFk.name',
        },
        {
          targets: 10,
          data: 'recordCategoryRuleName',
          name: 'recordCategoryRuleFk.name',
        },
      ],
    });

    function getSurpathServices() {
      dataTable.ajax.reload();
    }

    function deleteSurpathService(surpathService) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _surpathServicesService
            .delete({
              id: surpathService.id,
            })
            .done(function () {
              getSurpathServices(true);
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

    $('#CreateNewSurpathServiceButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _surpathServicesService
        .getSurpathServicesToExcel({
          filter: $('#SurpathServicesTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
          minPriceFilter: $('#MinPriceFilterId').val(),
          maxPriceFilter: $('#MaxPriceFilterId').val(),
          minDiscountFilter: $('#MinDiscountFilterId').val(),
          maxDiscountFilter: $('#MaxDiscountFilterId').val(),
          descriptionFilter: $('#DescriptionFilterId').val(),
          isEnabledByDefaultFilter: $('#IsEnabledByDefaultFilterId').val(),
          tenantDepartmentNameFilter: $('#TenantDepartmentNameFilterId').val(),
          cohortNameFilter: $('#CohortNameFilterId').val(),
          userNameFilter: $('#UserNameFilterId').val(),
          recordCategoryRuleNameFilter: $('#RecordCategoryRuleNameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditSurpathServiceModalSaved', function () {
      getSurpathServices();
    });

    $('#GetSurpathServicesButton').click(function (e) {
      e.preventDefault();
      getSurpathServices();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getSurpathServices();
      }
    });

    $('.reload-on-change').change(function (e) {
      getSurpathServices();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getSurpathServices();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getSurpathServices();
    });
  });
})();
