(function () {
  $(function () {
    var _$userPidsTable = $('#UserPidsTable');
    var _userPidsService = abp.services.app.userPids;
    var _entityTypeFullName = 'inzibackend.Surpath.UserPid';

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
      create: abp.auth.hasPermission('Pages.UserPids.Create'),
      edit: abp.auth.hasPermission('Pages.UserPids.Edit'),
      delete: abp.auth.hasPermission('Pages.UserPids.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/UserPids/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/UserPids/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditUserPidModal',
    });

    var _viewUserPidModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/UserPids/ViewuserPidModal',
      modalClass: 'ViewUserPidModal',
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

    var dataTable = _$userPidsTable.DataTable({
        paging: true,
        lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
        pageLength: 5000,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _userPidsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#UserPidsTableFilter').val(),
            pidFilter: $('#PidFilterId').val(),
            validatedFilter: $('#ValidatedFilterId').val(),
            pidTypeNameFilter: $('#PidTypeNameFilterId').val(),
            userNameFilter: $('#UserNameFilterId').val(),
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
                  _viewUserPidModal.open({ id: data.record.userPid.id });
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.userPid.id });
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
                    entityId: data.record.userPid.id,
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
                  deleteUserPid(data.record.userPid);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'userPid.pid',
          name: 'pid',
        },
        {
          targets: 3,
          data: 'userPid.validated',
          name: 'validated',
          render: function (validated) {
            if (validated) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
        {
          targets: 4,
          data: 'pidTypeName',
          name: 'pidTypeFk.name',
        },
        {
          targets: 5,
          data: 'userName',
          name: 'userFk.name',
        },
      ],
    });

    function getUserPids() {
      dataTable.ajax.reload();
    }

    function deleteUserPid(userPid) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _userPidsService
            .delete({
              id: userPid.id,
            })
            .done(function () {
              getUserPids(true);
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

    $('#CreateNewUserPidButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _userPidsService
        .getUserPidsToExcel({
          filter: $('#UserPidsTableFilter').val(),
          pidFilter: $('#PidFilterId').val(),
          validatedFilter: $('#ValidatedFilterId').val(),
          pidTypeNameFilter: $('#PidTypeNameFilterId').val(),
          userNameFilter: $('#UserNameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditUserPidModalSaved', function () {
      getUserPids();
    });

    $('#GetUserPidsButton').click(function (e) {
      e.preventDefault();
      getUserPids();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getUserPids();
      }
    });
  });
})();
