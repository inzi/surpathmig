(function ($) {
    app.modals.CreateOrEditMedicalUnitModal = function () {
        var _medicalUnitsService = abp.services.app.medicalUnits;

        var _modalManager;
        var _$medicalUnitInformationForm = null;

        var _MedicalUnithospitalLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/MedicalUnits/HospitalLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/MedicalUnits/_MedicalUnitHospitalLookupTableModal.js',
            modalClass: 'HospitalLookupTableModal',
        });


        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            $(modal).on('shown.bs.modal', function (e) {

                if ($('#hospitalId option').length ==1) {
                    abp.notify.error(app.localize('HospitalIsRequired'));
                    $('#' + _modalManager.getModalId() + ' form :input').prop('disabled', true);
                    $('#' + _modalManager.getModalId() + ' .save-button').prop('disabled', true);
                }
            })
            modal.find('.date-picker').daterangepicker({
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L',
            });
            
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

            var selectedEntityId = $('#MedicalUnit_HospitalId');

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

            _$medicalUnitInformationForm = _modalManager.getModal().find('form[name=MedicalUnitInformationsForm]');
            _$medicalUnitInformationForm.validate();
        };

        $('#OpenHospitalLookupTableButton').click(function () {
            var medicalUnit = _$medicalUnitInformationForm.serializeFormToObject();

            _MedicalUnithospitalLookupTableModal.open(
                { id: medicalUnit.hospitalId, displayName: medicalUnit.hospitalName },
                function (data) {
                    _$medicalUnitInformationForm.find('input[name=hospitalName]').val(data.displayName);
                    _$medicalUnitInformationForm.find('input[name=hospitalId]').val(data.id);
                }
            );
        });

        $('#ClearHospitalNameButton').click(function () {
            _$medicalUnitInformationForm.find('input[name=hospitalName]').val('');
            _$medicalUnitInformationForm.find('input[name=hospitalId]').val('');
        });

        this.save = function () {
            if (!_$medicalUnitInformationForm.valid()) {
                return;
            }
            if ($('#MedicalUnit_HospitalId').prop('required') && $('#MedicalUnit_HospitalId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Hospital')));
                return;
            }

            var medicalUnit = _$medicalUnitInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _medicalUnitsService
                .createOrEdit(medicalUnit)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.createOrEditMedicalUnitModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);
