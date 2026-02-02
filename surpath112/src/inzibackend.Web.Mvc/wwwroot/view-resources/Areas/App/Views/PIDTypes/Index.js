(function () {
  $(function () {
    var _$pidTypesTable = $('#PidTypesTable');
    var _pidTypesService = abp.services.app.pidTypes;
    var _entityTypeFullName = 'inzibackend.Surpath.PidType';

    var $selectedDate = {
      startDate: null,
      endDate: null,
    };

    $('.date-picker').on('apply.daterangepicker', function (ev, picker) {
      $(this).val(picker.startDate.format('MM/DD/YYYY'));
    });

    $('.startDate')
      .daterangepicker(
        {
          autoUpdateInput: false,
          singleDatePicker: true,
          locale: abp.localization.currentLanguage.name,
          format: 'L',
        },
        (date) => {
          $selectedDate.startDate = date;
        }
      )
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
      });

    $('.endDate')
      .daterangepicker(
        {
          autoUpdateInput: false,
          singleDatePicker: true,
          locale: abp.localization.currentLanguage.name,
          format: 'L',
        },
        (date) => {
          $selectedDate.endDate = date;
        }
      )
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.PidTypes.Create'),
      edit: abp.auth.hasPermission('Pages.PidTypes.Edit'),
      delete: abp.auth.hasPermission('Pages.PidTypes.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/PidTypes/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PidTypes/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditPidTypeModal',
    });

    var _viewPidTypeModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/PidTypes/ViewpidTypeModal',
      modalClass: 'ViewPidTypeModal',
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

    var dataTable = _$pidTypesTable.DataTable({
        paging: true,
        lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
        pageLength: 5000,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _pidTypesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#PidTypesTableFilter').val(),
            nameFilter: $('#NameFilterId').val(),
            descriptionFilter: $('#DescriptionFilterId').val(),
            maskPidFilter: $('#MaskPidFilterId').val(),
            minCreatedOnFilter: getDateFilter($('#MinCreatedOnFilterId')),
            maxCreatedOnFilter: getMaxDateFilter($('#MaxCreatedOnFilterId')),
            minModifiedOnFilter: getDateFilter($('#MinModifiedOnFilterId')),
            maxModifiedOnFilter: getMaxDateFilter($('#MaxModifiedOnFilterId')),
            minCreatedByFilter: $('#MinCreatedByFilterId').val(),
            maxCreatedByFilter: $('#MaxCreatedByFilterId').val(),
            minLastModifiedByFilter: $('#MinLastModifiedByFilterId').val(),
            maxLastModifiedByFilter: $('#MaxLastModifiedByFilterId').val(),
            isActiveFilter: $('#IsActiveFilterId').val(),
            pidInputMaskFilter: $('#PidInputMaskFilterId').val(),
            requiredFilter: $('#RequiredFilterId').val(),
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
                  _viewPidTypeModal.open({ id: data.record.pidType.id });
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.pidType.id });
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
                    entityId: data.record.pidType.id,
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
                  deletePidType(data.record.pidType);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'pidType.name',
          name: 'name',
        },
        {
          targets: 3,
          data: 'pidType.description',
          name: 'description',
        },
        {
          targets: 4,
          data: 'pidType.maskPid',
          name: 'maskPid',
          render: function (maskPid) {
            if (maskPid) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
        {
          targets: 5,
          data: 'pidType.createdOn',
          name: 'createdOn',
          render: function (createdOn) {
            if (createdOn) {
              return moment(createdOn).format('L');
            }
            return '';
          },
        },
        {
          targets: 6,
          data: 'pidType.modifiedOn',
          name: 'modifiedOn',
          render: function (modifiedOn) {
            if (modifiedOn) {
              return moment(modifiedOn).format('L');
            }
            return '';
          },
        },
        {
          targets: 7,
          data: 'pidType.createdBy',
          name: 'createdBy',
        },
        {
          targets: 8,
          data: 'pidType.lastModifiedBy',
          name: 'lastModifiedBy',
        },
        {
          targets: 9,
          data: 'pidType.isActive',
          name: 'isActive',
          render: function (isActive) {
            if (isActive) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
        {
          targets: 10,
          data: 'pidType.pidInputMask',
          name: 'pidInputMask',
        },
        {
          targets: 11,
          data: 'pidType.required',
          name: 'required',
          render: function (required) {
            if (required) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
      ],
    });

    function getPidTypes() {
      dataTable.ajax.reload();
    }

    function deletePidType(pidType) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _pidTypesService
            .delete({
              id: pidType.id,
            })
            .done(function () {
              getPidTypes(true);
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

    $('#CreateNewPidTypeButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _pidTypesService
        .getPidTypesToExcel({
          filter: $('#PidTypesTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
          descriptionFilter: $('#DescriptionFilterId').val(),
          maskPidFilter: $('#MaskPidFilterId').val(),
          minCreatedOnFilter: getDateFilter($('#MinCreatedOnFilterId')),
          maxCreatedOnFilter: getMaxDateFilter($('#MaxCreatedOnFilterId')),
          minModifiedOnFilter: getDateFilter($('#MinModifiedOnFilterId')),
          maxModifiedOnFilter: getMaxDateFilter($('#MaxModifiedOnFilterId')),
          minCreatedByFilter: $('#MinCreatedByFilterId').val(),
          maxCreatedByFilter: $('#MaxCreatedByFilterId').val(),
          minLastModifiedByFilter: $('#MinLastModifiedByFilterId').val(),
          maxLastModifiedByFilter: $('#MaxLastModifiedByFilterId').val(),
          isActiveFilter: $('#IsActiveFilterId').val(),
          pidInputMaskFilter: $('#PidInputMaskFilterId').val(),
          requiredFilter: $('#RequiredFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditPidTypeModalSaved', function () {
      getPidTypes();
    });

    $('#GetPidTypesButton').click(function (e) {
      e.preventDefault();
      getPidTypes();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getPidTypes();
      }
    });
  });
})();
