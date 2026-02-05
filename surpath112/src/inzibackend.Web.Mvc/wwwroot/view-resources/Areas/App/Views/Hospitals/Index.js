(function () {
  $(function () {
    var _$hospitalsTable = $('#HospitalsTable');
    var _hospitalsService = abp.services.app.hospitals;
    var _entityTypeFullName = 'inzibackend.Surpath.Hospital';

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
        getHospitals();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getHospitals();
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
        getHospitals();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getHospitals();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.Hospitals.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.Hospitals.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.Hospitals.Delete'),
    };

    var _viewHospitalModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Hospitals/ViewhospitalModal',
      modalClass: 'ViewHospitalModal',
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

    var dataTable = _$hospitalsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _hospitalsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#HospitalsTableFilter').val(),
            nameFilter: $('#NameFilterId').val(),
            primaryContactFilter: $('#PrimaryContactFilterId').val(),
            primaryContactPhoneFilter: $('#PrimaryContactPhoneFilterId').val(),
            primaryContactEmailFilter: $('#PrimaryContactEmailFilterId').val(),
            address1Filter: $('#Address1FilterId').val(),
            address2Filter: $('#Address2FilterId').val(),
            cityFilter: $('#CityFilterId').val(),
            stateFilter: $('#StateFilterId').val(),
            zipCodeFilter: $('#ZipCodeFilterId').val(),
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
                  window.location = '/App/Hospitals/ViewHospital/' + data.record.hospital.id;
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  window.location = '/App/Hospitals/CreateOrEdit/' + data.record.hospital.id;
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
                    entityId: data.record.hospital.id,
                  });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteHospital(data.record.hospital);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'hospital.name',
          name: 'name',
        },
        {
          targets: 3,
          data: 'hospital.primaryContact',
          name: 'primaryContact',
        },
        {
          targets: 4,
          data: 'hospital.primaryContactPhone',
          name: 'primaryContactPhone',
        },
        {
          targets: 5,
          data: 'hospital.primaryContactEmail',
          name: 'primaryContactEmail',
        },
        {
          targets: 6,
          data: 'hospital.address1',
          name: 'address1',
        },
        {
          targets: 7,
          data: 'hospital.address2',
          name: 'address2',
        },
        {
          targets: 8,
          data: 'hospital.city',
          name: 'city',
        },
        {
          targets: 9,
          data: 'hospital.state',
          name: 'state',
          render: function (state) {
            return app.localize('Enum_enumUSStates_' + state);
          },
        },
        {
          targets: 10,
          data: 'hospital.zipCode',
          name: 'zipCode',
        },
      ],
    });

    function getHospitals() {
      dataTable.ajax.reload();
    }

    function deleteHospital(hospital) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _hospitalsService
            .delete({
              id: hospital.id,
            })
            .done(function () {
              getHospitals(true);
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
      _hospitalsService
        .getHospitalsToExcel({
          filter: $('#HospitalsTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
          primaryContactFilter: $('#PrimaryContactFilterId').val(),
          primaryContactPhoneFilter: $('#PrimaryContactPhoneFilterId').val(),
          primaryContactEmailFilter: $('#PrimaryContactEmailFilterId').val(),
          address1Filter: $('#Address1FilterId').val(),
          address2Filter: $('#Address2FilterId').val(),
          cityFilter: $('#CityFilterId').val(),
          stateFilter: $('#StateFilterId').val(),
          zipCodeFilter: $('#ZipCodeFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditHospitalModalSaved', function () {
      getHospitals();
    });

    $('#GetHospitalsButton').click(function (e) {
      e.preventDefault();
      getHospitals();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getHospitals();
      }
    });

    $('.reload-on-change').change(function (e) {
      getHospitals();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getHospitals();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getHospitals();
    });
  });
})();
