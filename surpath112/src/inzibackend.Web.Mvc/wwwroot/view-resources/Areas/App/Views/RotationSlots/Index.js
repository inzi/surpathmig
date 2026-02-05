(function () {
    $(function () {
        var _$rotationSlotsTable = $('#RotationSlotsTable');
        var _rotationSlotsService = abp.services.app.rotationSlots;
        var _entityTypeFullName = 'inzibackend.Surpath.RotationSlot';

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
                getRotationSlots();
            })
            .on('cancel.daterangepicker', function (ev, picker) {
                $(this).val('');
                $selectedDate.startDate = null;
                getRotationSlots();
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
                getRotationSlots();
            })
            .on('cancel.daterangepicker', function (ev, picker) {
                $(this).val('');
                $selectedDate.endDate = null;
                getRotationSlots();
            });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.RotationSlots.Create'),
            edit: abp.auth.hasPermission('Pages.RotationSlots.Edit'),
            delete: abp.auth.hasPermission('Pages.RotationSlots.Delete'),
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RotationSlots/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RotationSlots/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditRotationSlotModal',
        });

        var _viewRotationSlotModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RotationSlots/ViewrotationSlotModal',
            modalClass: 'ViewRotationSlotModal',
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
       
        var dataTable = _$rotationSlotsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            select: {
                style: 'multi',
            },
            listAction: {
                ajaxFunction: _rotationSlotsService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#RotationSlotsTableFilter').val(),
                        slotIdFilter: $('#SlotIdFilterId').val(),
                        minAvailableSlotsFilter: $('#MinAvailableSlotsFilterId').val(),
                        maxAvailableSlotsFilter: $('#MaxAvailableSlotsFilterId').val(),
                        minShiftStartDateFilter: getDateFilter($('#MinShiftStartDateFilterId')),
                        maxShiftStartDateFilter: getMaxDateFilter($('#MaxShiftStartDateFilterId')),
                        minShiftEndDateFilter: getDateFilter($('#MinShiftEndDateFilterId')),
                        maxShiftEndDateFilter: getMaxDateFilter($('#MaxShiftEndDateFilterId')),
                        minShiftStartTimeFilter: getDateFilter($('#MinShiftStartTimeFilterId')),
                        maxShiftStartTimeFilter: getMaxDateFilter($('#MaxShiftStartTimeFilterId')),
                        minShiftEndTimeFilter: getDateFilter($('#MinShiftEndTimeFilterId')),
                        maxShiftEndTimeFilter: getMaxDateFilter($('#MaxShiftEndTimeFilterId')),
                        minShiftHoursFilter: $('#MinShiftHoursFilterId').val(),
                        maxShiftHoursFilter: $('#MaxShiftHoursFilterId').val(),
                        notifyHospitalFilter: $('#NotifyHospitalFilterId').val(),
                        minHospitalNotifiedDateTimeFilter: getDateFilter($('#MinHospitalNotifiedDateTimeFilterId')),
                        maxHospitalNotifiedDateTimeFilter: getMaxDateFilter($('#MaxHospitalNotifiedDateTimeFilterId')),
                        shiftTypeFilter: $('#ShiftTypeFilterId').val(),
                        minBidStartDateTimeFilter: getDateFilter($('#MinBidStartDateTimeFilterId')),
                        maxBidStartDateTimeFilter: getMaxDateFilter($('#MaxBidStartDateTimeFilterId')),
                        minBidEndDateTimeFilter: getDateFilter($('#MinBidEndDateTimeFilterId')),
                        maxBidEndDateTimeFilter: getMaxDateFilter($('#MaxBidEndDateTimeFilterId')),
                        hospitalNameFilter: $('#HospitalNameFilterId').val(),
                        medicalUnitNameFilter: $('#MedicalUnitNameFilterId').val(),
                    };
                },
            },
            columnDefs: [
                {
                    'targets': 0, // Target the first column
                    'searchable': false,
                    'orderable': false,
                    //'className': 'select-checkbox theme0-chk-sm',
                    'width': '1%',
                    'className': 'dt-body-center',
                    'render': function (data, type, full, meta) {
                        /*return '<input class="theme0-chk-sm" type="checkbox" name="isSelected[]" value="' + $('<div/>').text(data).html() + '">';*/
                        /*return '<input class="" type="checkbox" name="isSelected[]" value="' + $('<div/>').text(data).html() + '">';*/
                        return '<input type="checkbox">';
                    }
                },
                {
                    targets: 1,
                    data: 'rotationSlot.slotId',
                    name: 'slotId',
                },
                {
                    targets: 2,
                    data: 'hospitalName',
                    name: 'hospitalFk.name',
                },
                {
                    targets: 3,
                    data: 'medicalUnitName',
                    name: 'medicalUnitFk.name',
                },

                {
                    targets: 4,
                    data: 'rotationSlot.shiftStartDate',
                    name: 'shiftStartDate',
                    render: function (shiftStartDate) {
                        if (shiftStartDate) {
                            return moment(shiftStartDate).format('L');
                        }
                        return '';
                    },
                },
                {
                    targets: 5,
                    data: 'rotationSlot.shiftEndDate',
                    name: 'shiftEndDate',
                    render: function (shiftEndDate) {
                        if (shiftEndDate) {
                            return moment(shiftEndDate).format('L');
                        }
                        return '';
                    },
                },
                {
                    targets: 6,
                    data: 'rotationSlot.shiftStartTime',
                    name: 'shiftStartTime',
                    render: function (shiftStartTime) {
                        if (shiftStartTime) {
                            return moment(shiftStartTime).format('LT');
                        }
                        return '';
                    },
                },
                {
                    targets: 7,
                    data: 'rotationSlot.shiftEndTime',
                    name: 'shiftEndTime',
                    render: function (shiftEndTime) {
                        if (shiftEndTime) {
                            return moment(shiftEndTime).format('LT');
                        }
                        return '';
                    },
                },
                {
                    targets: 8,
                    data: 'rotationSlot.availableSlots',
                    name: 'availableSlots',
                },
            ]
        });
     
        $('#RotationSlotsTable').on('click', 'input[type = "checkbox"]', function (e) {

            var $row = $(this).closest('tr');
          
            if (this.checked) {
                $row.addClass('selected');
            } else {
                $row.removeClass('selected');
            }
            
            // Prevent click event from propagating to parent
            e.stopPropagation();
        });
        // Handle click on table cells with checkboxes
        $('#RotationSlotsTable').on('click', 'tbody td, thead th:first-child', function (e) {
            $(this).parent().find('input[type="checkbox"]').trigger('click');
        });

        function getRotationSlots() {
            dataTable.ajax.reload();
        }

        function deleteRotationSlot(rotationSlot) {
            var _msg = `Are you sure you want to delete the ${dataTable.rows({ selected: true }).count() } selected slots?`;

            if (dataTable.rows({ selected: true }).count()== 1) {
                _msg = 'Are you sure you want to delete the selected slot?';
            }
            var _selectedRowsEntityIds = selectedRowsEntityIds();
            abp.message.confirm(_msg, app.localize('AreYouSure'), function (isConfirmed) {
                if (isConfirmed) {
                    _rotationSlotsService
                        .multiDelete(_selectedRowsEntityIds)
                        .done(function () {
                            getRotationSlots(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                            deseletAll();
                            setButtons();

                        });
                }
            });
        }

        function cloneRotationSlots(data) {
            // This will send the list of ids to the server which will clone each with a new slot id, then reload the page.
            // confirm that the user wants to clone the selected slots
            var _msg = `Are you sure you want to clone the ${dataTable.rows({ selected: true }).count() } selected slots?`;

            if (dataTable.rows({ selected: true }).count() == 1) { 
             _msg = 'Are you sure you want to clone the selected slot?';
            }
            var _selectedRowsEntityIds = selectedRowsEntityIds();
            
            abp.message.confirm(_msg, app.localize('AreYouSure'), function (isConfirmed) {
                if (isConfirmed) {

                   
                    _rotationSlotsService
                        .clone(_selectedRowsEntityIds)
                        .done(function () {
                            getRotationSlots(true);
                            abp.notify.success(app.localize('SuccessfullyCloned'));
                            deseletAll();
                            setButtons();

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

        $('#CreateNewRotationSlotButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _rotationSlotsService
                .getRotationSlotsToExcel({
                    filter: $('#RotationSlotsTableFilter').val(),
                    slotIdFilter: $('#SlotIdFilterId').val(),
                    minAvailableSlotsFilter: $('#MinAvailableSlotsFilterId').val(),
                    maxAvailableSlotsFilter: $('#MaxAvailableSlotsFilterId').val(),
                    minShiftStartDateFilter: getDateFilter($('#MinShiftStartDateFilterId')),
                    maxShiftStartDateFilter: getMaxDateFilter($('#MaxShiftStartDateFilterId')),
                    minShiftEndDateFilter: getDateFilter($('#MinShiftEndDateFilterId')),
                    maxShiftEndDateFilter: getMaxDateFilter($('#MaxShiftEndDateFilterId')),
                    minShiftStartTimeFilter: getDateFilter($('#MinShiftStartTimeFilterId')),
                    maxShiftStartTimeFilter: getMaxDateFilter($('#MaxShiftStartTimeFilterId')),
                    minShiftEndTimeFilter: getDateFilter($('#MinShiftEndTimeFilterId')),
                    maxShiftEndTimeFilter: getMaxDateFilter($('#MaxShiftEndTimeFilterId')),
                    minShiftHoursFilter: $('#MinShiftHoursFilterId').val(),
                    maxShiftHoursFilter: $('#MaxShiftHoursFilterId').val(),
                    notifyHospitalFilter: $('#NotifyHospitalFilterId').val(),
                    minHospitalNotifiedDateTimeFilter: getDateFilter($('#MinHospitalNotifiedDateTimeFilterId')),
                    maxHospitalNotifiedDateTimeFilter: getMaxDateFilter($('#MaxHospitalNotifiedDateTimeFilterId')),
                    shiftTypeFilter: $('#ShiftTypeFilterId').val(),
                    minBidStartDateTimeFilter: getDateFilter($('#MinBidStartDateTimeFilterId')),
                    maxBidStartDateTimeFilter: getMaxDateFilter($('#MaxBidStartDateTimeFilterId')),
                    minBidEndDateTimeFilter: getDateFilter($('#MinBidEndDateTimeFilterId')),
                    maxBidEndDateTimeFilter: getMaxDateFilter($('#MaxBidEndDateTimeFilterId')),
                    hospitalNameFilter: $('#HospitalNameFilterId').val(),
                    medicalUnitNameFilter: $('#MedicalUnitNameFilterId').val(),
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditRotationSlotModalSaved', function () {
            getRotationSlots();
        });

        //$('#GetRotationSlotsButton').click(function (e) {
        //    e.preventDefault();
        //    getRotationSlots();
        //});

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getRotationSlots();
            }
        });

        $('.reload-on-change').change(function (e) {
            getRotationSlots();
        });

        $('.reload-on-keyup').keyup(function (e) {
            getRotationSlots();
        });

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getRotationSlots();
        });

        $('#slotsSelectAll').on('click', function () {
            dataTable.rows().select();
            $('input[type="checkbox"]', dataTable.rows().nodes()).prop('checked', true);

        });

        $('#slotsDeselectAll').on('click', function () {
            dataTable.rows().deselect();
            $('input[type="checkbox"]', dataTable.rows().nodes()).prop('checked', false);
        });

        $('#btnSlotView').on('click', function () {
            if (selectedRowsLength() != 1) {
                return {};
            };
            data = selectedRowData();
            _viewRotationSlotModal.open({ id: data.rotationSlot.id });
        });
        $('#btnSlotEdit').on('click', function () {
            if (selectedRowsLength() != 1) {
                return {};
            };
            data = selectedRowData();
            _createOrEditModal.open({ id: data.rotationSlot.id }, function () {
                deseletAll();
                setButtons();
            });
        });
        $('#btnSlotClone').on('click', function () {
            if (selectedRowsLength() <1) {
                return {};
            };
            data = selectedRowData();

            cloneRotationSlots(data);
        });
        $('#btnSlotDelete').on('click', function () {
            if (selectedRowsLength() <1) {
                return {};
            };
            data = selectedRowData();
            deleteRotationSlot(data.rotationSlot);
        });

        setButtons = function () {
            console.log('setButtons', selectedRowsLength());
            
            if (selectedRowsLength() != 1) {
                $('.btnSlotAction').prop('disabled', true);
            }
            else {
                $('.btnSlotAction').prop('disabled', false);
            }
            if (selectedRowsLength() >0 ) {
                $('.btnSlotActionMulti').prop('disabled', false);
            }
            else {
                $('.btnSlotActionMulti').prop('disabled', true);
            }
        }
        deseletAll = function () {
            dataTable.rows('.selected')
                .nodes()
                .to$()
                .removeClass('selected');
        }
        dataTable.on('select', function (e, dt, type, indexes) { setButtons(); });
        dataTable.on('deselect', function (e, dt, type, indexes) { setButtons(); });

        selectedRowsLength = function () {
            return dataTable.rows('.selected').nodes().to$().length;
        }
        selectedRowData = function () {

            //return dataTable.rows('.selected').nodes().to$()[0];
            return dataTable.row(dataTable.rows('.selected')[0]).data()
        }

        selectedRowsEntityIds = function () {
            var ids = [];
            var _selectedRowsEntityIds = dataTable.rows('.selected').data().map(function (r) { return r.rotationSlot.id; }).toArray();
            for (var i = 0; i < _selectedRowsEntityIds.length; i++) {
                ids.push({ id: _selectedRowsEntityIds[i] });
            }
            return ids;
        }
        
    });
})();
function updateDataTableSelectAllCtrl(table) {
    var $table = table.table().node();
    /*var $chkbox_all = $('tbody input[type="checkbox"]', $table);*/
    var $chkbox_checked = $('tbody input[type="checkbox"]:checked', $table);
    var chkbox_select_all = $('thead input[name="select_all"]', $table).get(0);

    // If none of the checkboxes are checked
    if ($chkbox_checked.length === 0) {
        chkbox_select_all.checked = false;
        if ('indeterminate' in chkbox_select_all) {
            chkbox_select_all.indeterminate = false;
        }

        // If all of the checkboxes are checked
    } else if ($chkbox_checked.length === $chkbox_all.length) {
        chkbox_select_all.checked = true;
        if ('indeterminate' in chkbox_select_all) {
            chkbox_select_all.indeterminate = false;
        }

        // If some of the checkboxes are checked
    } else {
        chkbox_select_all.checked = true;
        if ('indeterminate' in chkbox_select_all) {
            chkbox_select_all.indeterminate = true;
        }
    }
}