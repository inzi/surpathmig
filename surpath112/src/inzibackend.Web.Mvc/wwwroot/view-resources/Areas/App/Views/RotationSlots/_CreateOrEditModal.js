(function ($) {
    app.modals.CreateOrEditRotationSlotModal = function () {
        var _rotationSlotsService = abp.services.app.rotationSlots;

        var _modalManager;
        var _$rotationSlotInformationForm = null;

        var _RotationSlothospitalLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RotationSlots/HospitalLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RotationSlots/_RotationSlotHospitalLookupTableModal.js',
            modalClass: 'HospitalLookupTableModal',
        });
        var _RotationSlotmedicalUnitLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RotationSlots/MedicalUnitLookupTableModal',
            scriptUrl:
                abp.appPath + 'view-resources/Areas/App/Views/RotationSlots/_RotationSlotMedicalUnitLookupTableModal.js',
            modalClass: 'MedicalUnitLookupTableModal',
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').daterangepicker({
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L',
            });

            $('.j-time-picker').timepicker({ 'timeFormat': 'g:i a', 'scrollDefault': 'now' });

            $('.j-time-picker').on('change', function () {
                // this needs to calculate the difference between the start and end time in hours and set the value of the duration field
                var startTime = $('#RotationSlot_ShiftStartTime').val();
                var endTime = $('#RotationSlot_ShiftEndTime').val();
                if (startTime && endTime) {
                    var start = moment(startTime, 'h:mm a');
                    var end = moment(endTime, 'h:mm a');
                    var duration = moment.duration(end.diff(start));
                    var hours = duration.asHours(); // Get total hours as a decimal number

                    $('#RotationSlot_ShiftHours').val(hours.toFixed(2));

                    //var duration = moment.duration(end.diff(start));  
                    //var hours = parseInt(duration.asHours());
                    //var minutes = parseInt(duration.asMinutes()) % 60;
                    //$('#RotationSlot_ShiftHours').val(hours + ':' + minutes);
                }
                
            });
            // Init select2
            var typeaheadHospitalId = $('#hospitalId-typeahead-selector');

            typeaheadHospitalId.select2({
                placeholder: 'Select',
                theme: 'bootstrap5',
                selectionCssClass: 'form-select',
                dropdownParent: _modalManager.getModal(),
                minimumInputLength: 2,
                ajax: {
                    url: abp.appPath + 'api/services/app/Hospitals/GetAll',
                    dataType: 'json',
                    delay: 250,
                    data: function (params) {
                        return {
                            NameFilter: params.term, // search term
                            SkipCount: (params.page || 0) * 10,
                            MaxResultCount: 10,
                        };
                    },
                    processResults: function (data, params) {
                        params.page = params.page || 0;

                        return {
                            results: $.map(data.result.items, function (item) {
                                return {
                                    text: item.hospital.name,
                                    id: item.hospital.id,
                                };
                            }),
                            pagination: {
                                more: params.page * 10 < data.result.totalCount,
                            },
                        };
                    },
                    cache: true,
                },
            });

            var selectedEntityId = $('#RotationSlot_HospitalId');

            if (selectedEntityId && selectedEntityId.val()) {
                abp
                    .ajax({
                        type: 'GET',
                        url: '/api/services/app/Hospitals/GetHospitalForView',
                        data: {
                            id: selectedEntityId.val(),
                        },
                    })
                    .done(function (data) {
                        var option = new Option(data.hospital.name, data.hospital.id, true, true);
                        typeaheadHospitalId.append(option).trigger('change');
                    });
            }
            // Init select2
            var typeaheadMedicalUnitId = $('#medicalUnitId-typeahead-selector');

            typeaheadMedicalUnitId.select2({
                placeholder: 'Select',
                theme: 'bootstrap5',
                selectionCssClass: 'form-select',
                dropdownParent: _modalManager.getModal(),
                minimumInputLength: 2,
                ajax: {
                    url: abp.appPath + 'api/services/app/MedicalUnits/GetAll',
                    dataType: 'json',
                    delay: 250,
                    data: function (params) {
                        return {
                            NameFilter: params.term, // search term
                            SkipCount: (params.page || 0) * 10,
                            MaxResultCount: 10,
                        };
                    },
                    processResults: function (data, params) {
                        params.page = params.page || 0;

                        return {
                            results: $.map(data.result.items, function (item) {
                                return {
                                    text: item.medicalUnit.name,
                                    id: item.medicalUnit.id,
                                };
                            }),
                            pagination: {
                                more: params.page * 10 < data.result.totalCount,
                            },
                        };
                    },
                    cache: true,
                },
            });

            var selectedEntityId = $('#RotationSlot_MedicalUnitId');

            if (selectedEntityId && selectedEntityId.val()) {
                abp
                    .ajax({
                        type: 'GET',
                        url: '/api/services/app/MedicalUnits/GetMedicalUnitForView',
                        data: {
                            id: selectedEntityId.val(),
                        },
                    })
                    .done(function (data) {
                        var option = new Option(data.medicalUnit.name, data.medicalUnit.id, true, true);
                        typeaheadMedicalUnitId.append(option).trigger('change');
                    });
            }

            _$rotationSlotInformationForm = _modalManager.getModal().find('form[name=RotationSlotInformationsForm]');
            _$rotationSlotInformationForm.validate();
        };

        $('#OpenHospitalLookupTableButton').click(function () {
            var rotationSlot = _$rotationSlotInformationForm.serializeFormToObject();

            _RotationSlothospitalLookupTableModal.open(
                { id: rotationSlot.hospitalId, displayName: rotationSlot.hospitalName },
                function (data) {
                    _$rotationSlotInformationForm.find('input[name=hospitalName]').val(data.displayName);
                    _$rotationSlotInformationForm.find('input[name=hospitalId]').val(data.id);
                }
            );
        });

        $('#ClearHospitalNameButton').click(function () {
            _$rotationSlotInformationForm.find('input[name=hospitalName]').val('');
            _$rotationSlotInformationForm.find('input[name=hospitalId]').val('');
        });

        $('#OpenMedicalUnitLookupTableButton').click(function () {
            var rotationSlot = _$rotationSlotInformationForm.serializeFormToObject();

            _RotationSlotmedicalUnitLookupTableModal.open(
                { id: rotationSlot.medicalUnitId, displayName: rotationSlot.medicalUnitName },
                function (data) {
                    _$rotationSlotInformationForm.find('input[name=medicalUnitName]').val(data.displayName);
                    _$rotationSlotInformationForm.find('input[name=medicalUnitId]').val(data.id);
                }
            );
        });

        $('#ClearMedicalUnitNameButton').click(function () {
            _$rotationSlotInformationForm.find('input[name=medicalUnitName]').val('');
            _$rotationSlotInformationForm.find('input[name=medicalUnitId]').val('');
        });

        this.save = function () {
            if (!_$rotationSlotInformationForm.valid()) {
                return;
            }
            if ($('#RotationSlot_HospitalId').prop('required') && $('#RotationSlot_HospitalId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Hospital')));
                return;
            }
            if ($('#RotationSlot_MedicalUnitId').prop('required') && $('#RotationSlot_MedicalUnitId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('MedicalUnit')));
                return;
            }

            var model = _$rotationSlotInformationForm.serializeFormToObject();
            // debugger;
            _modalManager.setBusy(true);
            _rotationSlotsService
                .createOrEdit(model.RotationSlot)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.createOrEditRotationSlotModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);
