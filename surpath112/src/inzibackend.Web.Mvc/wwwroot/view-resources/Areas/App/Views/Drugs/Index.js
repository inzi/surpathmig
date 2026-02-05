(function () {
  $(function () {
    var _$drugsTable = $('#DrugsTable');
    var _drugsService = abp.services.app.drugs;
    var _entityTypeFullName = 'inzibackend.Surpath.Drug';

      $('.date-picker').daterangepicker({
          singleDatePicker: true,
          
           locale: { format: 'MM/DD/YYYY', },
      });

      app.daterangefilterhelper.fixfilters();

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.Drugs.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.Drugs.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.Drugs.Delete'),
    };

    var _viewDrugModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Drugs/ViewdrugModal',
      modalClass: 'ViewDrugModal',
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

    var dataTable = _$drugsTable.DataTable({
        paging: false,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _drugsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#DrugsTableFilter').val(),
            nameFilter: $('#NameFilterId').val(),
            codeFilter: $('#CodeFilterId').val(),
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
                  window.location = '/App/Drugs/ViewDrug/' + data.record.drug.id;
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  window.location = '/App/Drugs/CreateOrEdit/' + data.record.drug.id;
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
                    entityId: data.record.drug.id,
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
                  deleteDrug(data.record.drug);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'drug.name',
          name: 'name',
        },
        {
          targets: 3,
          data: 'drug.code',
          name: 'code',
        },
      ],
    });

    function getDrugs() {
      dataTable.ajax.reload();
    }

    function deleteDrug(drug) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _drugsService
            .delete({
              id: drug.id,
            })
            .done(function () {
              getDrugs(true);
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
      _drugsService
        .getDrugsToExcel({
          filter: $('#DrugsTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
          codeFilter: $('#CodeFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditDrugModalSaved', function () {
      getDrugs();
    });

    $('#GetDrugsButton').click(function (e) {
      e.preventDefault();
      getDrugs();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getDrugs();
      }
    });
  });
})();
