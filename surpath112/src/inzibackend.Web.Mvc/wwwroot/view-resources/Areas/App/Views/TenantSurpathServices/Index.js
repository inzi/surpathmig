(function () {
  $(function () {
    var _$tenantSurpathServicesTable = $('#TenantSurpathServicesTable');
    var _tenantSurpathServicesService = abp.services.app.tenantSurpathServices;
    var _entityTypeFullName = 'inzibackend.Surpath.TenantSurpathService';

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
        getTenantSurpathServices();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getTenantSurpathServices();
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
        getTenantSurpathServices();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getTenantSurpathServices();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.TenantSurpathServices.Create'),
      edit: abp.auth.hasPermission('Pages.TenantSurpathServices.Edit'),
      delete: abp.auth.hasPermission('Pages.TenantSurpathServices.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/TenantSurpathServices/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/TenantSurpathServices/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditTenantSurpathServiceModal',
    });

    var _viewTenantSurpathServiceModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/TenantSurpathServices/ViewtenantSurpathServiceModal',
      modalClass: 'ViewTenantSurpathServiceModal',
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

    var _assignmentModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/TenantSurpathServices/AssignmentModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/TenantSurpathServices/AssignmentModal.js',
      modalClass: 'AssignTenantSurpathServiceModal'
    });

    var dataTable = _$tenantSurpathServicesTable.DataTable({
        paging: true,
        lengthMenu: [5, 10, 25, 50, 100, 250, 500, 1000],
        pageLength: 1000,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _tenantSurpathServicesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#TenantSurpathServicesTableFilter').val(),
            nameFilter: $('#NameFilterId').val(),
            minPriceFilter: $('#MinPriceFilterId').val(),
            maxPriceFilter: $('#MaxPriceFilterId').val(),
            descriptionFilter: $('#DescriptionFilterId').val(),
            isEnabledFilter: $('#IsEnabledFilterId').val(),
            surpathServiceNameFilter: $('#SurpathServiceNameFilterId').val(),
            tenantDepartmentNameFilter: $('#TenantDepartmentNameFilterId').val(),
            cohortNameFilter: $('#CohortNameFilterId').val(),
            userNameFilter: $('#UserNameFilterId').val(),
            recordCategoryRuleNameFilter: $('#RecordCategoryRuleNameFilterId').val(),
          };
        },
      },
      rowCallback: function(row, data) {
        // Add styling for disabled services
        if (!data.tenantSurpathService.isPricingOverrideEnabled) {
          $(row).addClass('service-disabled');
          $(row).css('opacity', '0.6');
          $(row).find('td').css('color', '#999');
        }
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
          width: 30,
          targets: 1,
          data: null,
          orderable: false,
          className: 'dt-center',
          render: function (data, type, row) {
            if (_permissions.edit) {
              return '<input type="checkbox" class="bulk-select-checkbox" data-id="' + row.tenantSurpathService.id + '">';
            }
            return '';
          }
        },
        {
          width: 120,
            targets: 2,
            visible: function() {
                return abp.auth.hasPermission('Pages.Administration.Host.Dashboard') || true==true;
            },
          data: null,
          orderable: false,
          autoWidth: false,
          defaultContent: '',
          rowAction: {
            cssClass: 'btn btn-brand dropdown-toggle',
            text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
            items: [
              {
                text: app.localize('Assign'),
                iconStyle: 'fas fa-user-check mr-2',
                visible: function () {
                  return abp.auth.hasPermission('Pages.TenantSurpathServices.AssignToTenant');
                },
                action: function (data) {
                  assignService(data.record.tenantSurpathService.id);
                },
              },
              {
                text: app.localize('View'),
                action: function (data) {
                  _viewTenantSurpathServiceModal.open({ id: data.record.tenantSurpathService.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.tenantSurpathService.id });
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
                    entityId: data.record.tenantSurpathService.id,
                  });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteTenantSurpathService(data.record.tenantSurpathService);
                },
              },
            ],
          },
        },
        {
          targets: 3,
          data: 'tenantSurpathService.name',
          name: 'name',
        },
        {
          targets: 4,
          data: 'tenantSurpathService.price',
          name: 'price',
        },
        {
          targets: 5,
          data: 'tenantSurpathService.description',
          name: 'description',
        },
        {
          targets: 6,
          data: 'tenantSurpathService.isPricingOverrideEnabled',
          name: 'isPricingOverrideEnabled',
          render: function (isEnabled, type, row) {
            if (_permissions.edit) {
              var switchId = 'switch_' + row.tenantSurpathService.id;
              var checked = isEnabled ? 'checked' : '';
              return '<div class="text-center">' +
                '<div class="form-check form-switch">' +
                '<input class="form-check-input toggle-enabled" type="checkbox" id="' + switchId + '" ' +
                'data-id="' + row.tenantSurpathService.id + '" ' + checked + '>' +
                '</div>' +
                '</div>';
            } else {
              // Read-only view
              if (isEnabled) {
                return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
              }
              return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
            }
          },
        },
        {
          targets: 7,
          data: 'surpathServiceName',
          name: 'surpathServiceFk.name',
        },
        {
          targets: 8,
          data: 'tenantDepartmentName',
          name: 'tenantDepartmentFk.name',
        },
        {
          targets: 9,
          data: 'cohortName',
          name: 'cohortFk.name',
        },
        {
          targets: 10,
          data: 'userName',
          name: 'userFk.name',
        },
        {
          targets: 11,
          data: 'recordCategoryRuleName',
          name: 'recordCategoryRuleFk.name',
          }
      ],
    });

    function getTenantSurpathServices() {
      dataTable.ajax.reload();
    }

    function deleteTenantSurpathService(tenantSurpathService) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _tenantSurpathServicesService
            .delete({
              id: tenantSurpathService.id,
            })
            .done(function () {
              getTenantSurpathServices(true);
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

    $('#CreateNewTenantSurpathServiceButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _tenantSurpathServicesService
        .getTenantSurpathServicesToExcel({
          filter: $('#TenantSurpathServicesTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
          minPriceFilter: $('#MinPriceFilterId').val(),
          maxPriceFilter: $('#MaxPriceFilterId').val(),
          descriptionFilter: $('#DescriptionFilterId').val(),
          isEnabledFilter: $('#IsEnabledFilterId').val(),
          surpathServiceNameFilter: $('#SurpathServiceNameFilterId').val(),
          tenantDepartmentNameFilter: $('#TenantDepartmentNameFilterId').val(),
          cohortNameFilter: $('#CohortNameFilterId').val(),
          userNameFilter: $('#UserNameFilterId').val(),
          recordCategoryRuleNameFilter: $('#RecordCategoryRuleNameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditTenantSurpathServiceModalSaved', function () {
      getTenantSurpathServices();
    });

    $('#GetTenantSurpathServicesButton').click(function (e) {
      e.preventDefault();
      getTenantSurpathServices();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getTenantSurpathServices();
      }
    });

    $('.reload-on-change').change(function (e) {
      getTenantSurpathServices();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getTenantSurpathServices();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getTenantSurpathServices();
    });

    function assignService(id) {
      _assignmentModal.open({ id: id });
    }

    abp.event.on('app.assignmentSaved', function () {
      _$tenantSurpathServicesTable.DataTable().ajax.reload();
    });

    // Handle toggle switch changes
    $(document).on('change', '.toggle-enabled', function () {
      var $switch = $(this);
      var serviceId = $switch.data('id');
      var isEnabled = $switch.is(':checked');
      
      // Disable the switch during update
      $switch.prop('disabled', true);
      
      _tenantSurpathServicesService.toggleEnabled({
        id: serviceId,
        isEnabled: isEnabled
      }).done(function () {
        abp.notify.success(app.localize('SuccessfullyUpdated'));
        // Re-enable the switch
        $switch.prop('disabled', false);
      }).fail(function () {
        // Revert the switch state on failure
        $switch.prop('checked', !isEnabled);
        $switch.prop('disabled', false);
      });
    });

    // Handle select all checkbox
    $('#selectAllCheckbox').on('change', function () {
      var isChecked = $(this).is(':checked');
      $('.bulk-select-checkbox').prop('checked', isChecked);
      updateBulkActionsVisibility();
    });

    // Handle individual checkbox changes
    $(document).on('change', '.bulk-select-checkbox', function () {
      updateSelectAllCheckbox();
      updateBulkActionsVisibility();
    });

    function updateSelectAllCheckbox() {
      var totalCheckboxes = $('.bulk-select-checkbox').length;
      var checkedCheckboxes = $('.bulk-select-checkbox:checked').length;
      
      if (totalCheckboxes === 0) {
        $('#selectAllCheckbox').prop('checked', false);
        $('#selectAllCheckbox').prop('indeterminate', false);
      } else if (checkedCheckboxes === 0) {
        $('#selectAllCheckbox').prop('checked', false);
        $('#selectAllCheckbox').prop('indeterminate', false);
      } else if (checkedCheckboxes === totalCheckboxes) {
        $('#selectAllCheckbox').prop('checked', true);
        $('#selectAllCheckbox').prop('indeterminate', false);
      } else {
        $('#selectAllCheckbox').prop('checked', false);
        $('#selectAllCheckbox').prop('indeterminate', true);
      }
    }

    function updateBulkActionsVisibility() {
      var checkedCount = $('.bulk-select-checkbox:checked').length;
      if (checkedCount > 0) {
        $('#bulkActionsGroup').show();
      } else {
        $('#bulkActionsGroup').hide();
      }
    }

    // Handle bulk enable
    $('.bulk-enable').on('click', function (e) {
      e.preventDefault();
      performBulkAction(true);
    });

    // Handle bulk disable
    $('.bulk-disable').on('click', function (e) {
      e.preventDefault();
      performBulkAction(false);
    });

    function performBulkAction(isEnabled) {
      var selectedIds = [];
      $('.bulk-select-checkbox:checked').each(function () {
        selectedIds.push($(this).data('id'));
      });

      if (selectedIds.length === 0) {
        abp.notify.warn(app.localize('PleaseSelectAtLeastOneRecord'));
        return;
      }

      var message = isEnabled 
        ? app.localize('AreYouSureToEnableTheseRecords', selectedIds.length)
        : app.localize('AreYouSureToDisableTheseRecords', selectedIds.length);

      abp.message.confirm(
        message,
        app.localize('AreYouSure'),
        function (isConfirmed) {
          if (isConfirmed) {
            // Show loading
            abp.ui.setBusy($('#TenantSurpathServicesTable'));
            
            // Process each selected service
            var promises = selectedIds.map(function(id) {
              return _tenantSurpathServicesService.toggleEnabled({
                id: id,
                isEnabled: isEnabled
              });
            });

            // Wait for all promises to complete
            $.when.apply($, promises).done(function() {
              abp.notify.success(app.localize('SuccessfullyUpdated'));
              getTenantSurpathServices();
              $('.bulk-select-checkbox').prop('checked', false);
              updateSelectAllCheckbox();
              updateBulkActionsVisibility();
            }).fail(function() {
              abp.notify.error(app.localize('AnErrorOccurred'));
            }).always(function() {
              abp.ui.clearBusy($('#TenantSurpathServicesTable'));
            });
          }
        }
      );
    }
  });
})();
