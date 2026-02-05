(function () {
  $(function () {
    var _$welcomemessagesTable = $('#WelcomemessagesTable');
    var _welcomemessagesService = abp.services.app.welcomemessages;
    var _entityTypeFullName = 'inzibackend.Surpath.Welcomemessage';

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
        getWelcomemessages();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getWelcomemessages();
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
        getWelcomemessages();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getWelcomemessages();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.Welcomemessages.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.Welcomemessages.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.Welcomemessages.Delete'),
      isHost: abp.auth.hasPermission('Pages.Administration.Host.Dashboard'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Welcomemessages/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Welcomemessages/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditWelcomemessageModal',
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

    var dataTable = _$welcomemessagesTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _welcomemessagesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#WelcomemessagesTableFilter').val(),
            titleFilter: $('#TitleFilterId').val(),
            messageFilter: $('#MessageFilterId').val(),
            isDefaultFilter: $('#IsDefaultFilterId').val(),
            minDisplayStartFilter: getDateFilter($('#MinDisplayStartFilterId')),
            maxDisplayStartFilter: getMaxDateFilter($('#MaxDisplayStartFilterId')),
            minDisplayEndFilter: getDateFilter($('#MinDisplayEndFilterId')),
            maxDisplayEndFilter: getMaxDateFilter($('#MaxDisplayEndFilterId')),
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
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.welcomemessage.id });
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
                    entityId: data.record.welcomemessage.id,
                  });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteWelcomemessage(data.record.welcomemessage);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'welcomemessage.title',
          name: 'title',
        },
        {
          targets: 3,
          data: 'welcomemessage.message',
          name: 'message',
        },
        {
          targets: 4,
          data: 'welcomemessage.isDefault',
          name: 'isDefault',
          render: function (isDefault) {
            if (isDefault) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
        {
          targets: 5,
          data: 'welcomemessage.displayStart',
          name: 'displayStart',
          render: function (displayStart) {
            if (displayStart) {
              return moment(displayStart).format('L');
            }
            return '';
          },
        },
        {
          targets: 6,
          data: 'welcomemessage.displayEnd',
          name: 'displayEnd',
          render: function (displayEnd) {
            if (displayEnd) {
              return moment(displayEnd).format('L');
            }
            return '';
          },
        },
        {
            targets: 7,
            data: 'tenantName',
            name: 'tenantDepartmentFk.name',

        },
      ],
    })
    .columns([7]).visible(_permissions.isHost);

    function getWelcomemessages() {
      dataTable.ajax.reload();
    }

    function deleteWelcomemessage(welcomemessage) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _welcomemessagesService
            .delete({
              id: welcomemessage.id,
            })
            .done(function () {
              getWelcomemessages(true);
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

    $('#CreateNewWelcomemessageButton').click(function () {
      _createOrEditModal.open();
    });

    abp.event.on('app.createOrEditWelcomemessageModalSaved', function () {
      getWelcomemessages();
    });

    $('#GetWelcomemessagesButton').click(function (e) {
      e.preventDefault();
      getWelcomemessages();
    });

    $(document).keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getWelcomemessages();
      }
    });

    $('.reload-on-change').change(function (e) {
      getWelcomemessages();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getWelcomemessages();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getWelcomemessages();
    });
  });
})();
