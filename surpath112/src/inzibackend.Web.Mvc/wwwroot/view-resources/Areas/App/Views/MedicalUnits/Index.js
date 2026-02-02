(function () {
  $(function () {
    var _$medicalUnitsTable = $('#MedicalUnitsTable');
    var _medicalUnitsService = abp.services.app.medicalUnits;
    var _entityTypeFullName = 'inzibackend.Surpath.MedicalUnit';

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
        getMedicalUnits();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getMedicalUnits();
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
        getMedicalUnits();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getMedicalUnits();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.MedicalUnits.Create'),
      edit: abp.auth.hasPermission('Pages.MedicalUnits.Edit'),
      delete: abp.auth.hasPermission('Pages.MedicalUnits.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/MedicalUnits/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/MedicalUnits/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditMedicalUnitModal',
    });

    var _viewMedicalUnitModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/MedicalUnits/ViewmedicalUnitModal',
      modalClass: 'ViewMedicalUnitModal',
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

    var dataTable = _$medicalUnitsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _medicalUnitsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#MedicalUnitsTableFilter').val(),
            nameFilter: $('#NameFilterId').val(),
            primaryContactFilter: $('#PrimaryContactFilterId').val(),
            primaryContactPhoneFilter: $('#PrimaryContactPhoneFilterId').val(),
            primaryContactEmailFilter: $('#PrimaryContactEmailFilterId').val(),
            address1Filter: $('#Address1FilterId').val(),
            address2Filter: $('#Address2FilterId').val(),
            cityFilter: $('#CityFilterId').val(),
            stateFilter: $('#StateFilterId').val(),
            zipCodeFilter: $('#ZipCodeFilterId').val(),
            hospitalNameFilter: $('#HospitalNameFilterId').val(),
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
                  _viewMedicalUnitModal.open({ id: data.record.medicalUnit.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                    _createOrEditModal.open({ id: data.record.medicalUnit.id });
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
                    entityId: data.record.medicalUnit.id,
                  });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteMedicalUnit(data.record.medicalUnit);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'medicalUnit.name',
          name: 'name',
        },
        {
          targets: 3,
          data: 'medicalUnit.primaryContact',
          name: 'primaryContact',
        },
        {
          targets: 4,
          data: 'medicalUnit.primaryContactPhone',
          name: 'primaryContactPhone',
        },
        {
          targets: 5,
          data: 'medicalUnit.primaryContactEmail',
          name: 'primaryContactEmail',
        },
        {
          targets: 6,
          data: 'medicalUnit.address1',
          name: 'address1',
        },
        {
          targets: 7,
          data: 'medicalUnit.address2',
          name: 'address2',
        },
        {
          targets: 8,
          data: 'medicalUnit.city',
          name: 'city',
        },
        {
          targets: 9,
          data: 'medicalUnit.state',
          name: 'state',
          render: function (state) {
            return app.localize('Enum_enumUSStates_' + state);
          },
        },
        {
          targets: 10,
          data: 'medicalUnit.zipCode',
          name: 'zipCode',
        },
        {
          targets: 11,
          data: 'hospitalName',
          name: 'hospitalFk.name',
        },
      ],
    });

    function getMedicalUnits() {
      dataTable.ajax.reload();
    }

    function deleteMedicalUnit(medicalUnit) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _medicalUnitsService
            .delete({
              id: medicalUnit.id,
            })
            .done(function () {
              getMedicalUnits(true);
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

    $('#CreateNewMedicalUnitButton').click(function () {
        _createOrEditModal.open({}, function (data) {
            // console.log('back from open');
        }, function (data) {
            console.log('shown callback');
            console.log(data);
        });
    });

    $('#ExportToExcelButton').click(function () {
      _medicalUnitsService
        .getMedicalUnitsToExcel({
          filter: $('#MedicalUnitsTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
          primaryContactFilter: $('#PrimaryContactFilterId').val(),
          primaryContactPhoneFilter: $('#PrimaryContactPhoneFilterId').val(),
          primaryContactEmailFilter: $('#PrimaryContactEmailFilterId').val(),
          address1Filter: $('#Address1FilterId').val(),
          address2Filter: $('#Address2FilterId').val(),
          cityFilter: $('#CityFilterId').val(),
          stateFilter: $('#StateFilterId').val(),
          zipCodeFilter: $('#ZipCodeFilterId').val(),
          hospitalNameFilter: $('#HospitalNameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditMedicalUnitModalSaved', function () {
      getMedicalUnits();
    });

    $('#GetMedicalUnitsButton').click(function (e) {
      e.preventDefault();
      getMedicalUnits();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getMedicalUnits();
      }
    });

    $('.reload-on-change').change(function (e) {
      getMedicalUnits();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getMedicalUnits();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getMedicalUnits();
    });
  });
})();
