(function ($) {
    app.modals.CreateNewRecordForCategoryModal = function () {
        console.log('CreateNewRecordForCategoryModal compliance');
        var _complianceService = abp.services.app.surpathCompliance;

        var _modalManager;
        var _$recordStateInformationForm = null;
        var _fileUploading = [];
        var _filedataToken;
        var _SaveButton;

        var _RecordCategoryLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Compliance/RecordCategoryLookupTableModal',
            scriptUrl:
                abp.appPath + 'view-resources/Areas/App/Views/Compliance/_RecordCategoryLookupTableModal.js',
            modalClass: 'RecordCategoryLookupTableModal',
        });

        var _RecordCategoryConfirmLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Compliance/RecordCategoryConfirmLookupTableModal',
            scriptUrl:
                abp.appPath + 'view-resources/Areas/App/Views/Compliance/_RecordCategoryConfirmLookupTableModal.js',
            modalClass: 'RecordCategoryConfirmLookupTableModal',
        });
        

        this.init = function (modalManager) {

            _modalManager = modalManager;

            var modal = _modalManager.getModal();

            modal.find('.date-picker').daterangepicker({
                singleDatePicker: true,

                locale: { format: 'MM/DD/YYYY', },
            });
            _SaveButton = _modalManager.getModal().find('button.save-button');
            _SaveButton.attr('disabled', 'disabled');
            _$recordStateInformationForm = _modalManager.getModal().find('form[name=RecordStateInformationsForm]');
            _$recordStateInformationForm.validate();

        };

        //$('#OpenRecordCategoryLookupTableButton').click(function () {
        //    console.log('OpenRecordCategoryLookupTableButton');
        //    var recordState = _$recordStateInformationForm.serializeFormToObject();

        //    _RecordCategoryLookupTableModal.open(
        //        { id: recordState.recordCategoryId, displayName: recordState.recordCategoryName },
        //        function (data) {
        //            //console.log(data);
        //            _$recordStateInformationForm.find('input[name=recordCategoryName]').val(data.displayName);
        //            _$recordStateInformationForm.find('input[name=recordCategoryId]').val(data.id);
        //            _$recordStateInformationForm.find('textarea[name=recordCategoryInstructions]').html(data.displayInstructions);
        //            stepperButtons();
        //        }
        //    );
        //});

        //$('#OpenRecordCategoryConfirmLookupTableButton').click(function () {
        //    //var recordState = _$recordStateInformationForm.serializeFormToObject();

        //    _RecordCategoryConfirmLookupTableModal.open(
        //        { id: "", displayName: "", confirm: true },
        //        function (data) {
        //            _$recordStateInformationForm.find('input[name=confirmRecordCategoryName]').val(data.displayName);
        //            _$recordStateInformationForm.find('input[name=confirmRecordCategoryId]').val(data.id);
        //            stepperButtons();
        //        }
        //    );
        //});


        //$('#ClearRecordCategoryNameButton').click(function () {
        //    _$recordStateInformationForm.find('input[name=recordCategoryName]').val('');
        //    _$recordStateInformationForm.find('input[name=recordCategoryId]').val('');
        //    _$recordStateInformationForm.find('textarea[name=recordCategoryInstructions]').val('');
        //    stepperButtons();

        //});
        //$('#ClearConfirmRecordCategoryNameButton').click(function () {
        //    _$recordStateInformationForm.find('input[name=confirmRecordCategoryName]').val('');
        //    _$recordStateInformationForm.find('input[name=confirmRecordCategoryId]').val('');
        //    stepperButtons();

        //});


        $('#OpenUserLookupTableButton').click(function () {
            var recordState = _$recordStateInformationForm.serializeFormToObject();

            _RecordStateuserLookupTableModal.open(
                { id: recordState.userId, displayName: recordState.userName },
                function (data) {
                    _$recordStateInformationForm.find('input[name=userName]').val(data.displayName);
                    _$recordStateInformationForm.find('input[name=userId]').val(data.id);
                }
            );
        });

        $('#ClearUserNameButton').click(function () {
            _$recordStateInformationForm.find('input[name=userName]').val('');
            _$recordStateInformationForm.find('input[name=userId]').val('');
        });

        $('#OpenRecordStatusLookupTableButton').click(function () {
            var recordState = _$recordStateInformationForm.serializeFormToObject();

            _RecordStaterecordStatusLookupTableModal.open(
                { id: recordState.recordStatusId, displayName: recordState.recordStatusStatusName },
                function (data) {
                    _$recordStateInformationForm.find('input[name=recordStatusStatusName]').val(data.displayName);
                    _$recordStateInformationForm.find('input[name=recordStatusId]').val(data.id);
                }
            );
        });

        $('#ClearRecordStatusStatusNameButton').click(function () {
            _$recordStateInformationForm.find('input[name=recordStatusStatusName]').val('');
            _$recordStateInformationForm.find('input[name=recordStatusId]').val('');
        });

        this.save = function () {
            if (!_$recordStateInformationForm.valid()) {
                return;
            }
            if ($('#RecordState_RecordId').prop('required') && $('#RecordState_RecordId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Record')));
                return;
            }
            if ($('#RecordState_RecordCategoryId').prop('required') && $('#RecordState_RecordCategoryId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('RecordCategory')));
                return;
            }
            if ($('#RecordState_UserId').prop('required') && $('#RecordState_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }
            if ($('#RecordState_RecordStatusId').prop('required') && $('#RecordState_RecordStatusId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('RecordStatus')));
                return;
            }
            if (_fileUploading != null && _fileUploading.length > 0) {
                abp.notify.info(app.localize('WaitingForFileUpload'));
                return;
            }

            var recordState = _$recordStateInformationForm.serializeFormToObject();
            recordState.filedataToken = _filedataToken;
            var record = {
                binaryObjId: "00000000-0000-0000-0000-000000000000",
                dateLastUpdated: "",
                dateUploaded: "",
                filedataToken: _filedataToken,
                filename: recordState.filename,
                metadata: "{}",
                physicalfilepath: "",
                tenantDocumentCategoryId: "",
                tenantDocumentCategoryName: "",
            }

            _modalManager.setBusy(true);

            _complianceService
                .createNewRecord(record)
                .done(function (data) {
                    recordState.binaryObjId = data.record.binaryObjId;
                    recordState.recordId = data.record.id;
                    //console.log(data);
                    //debugger;
                    //// $('#Record_BinaryObjId').val(data)

                })
                .then(function () {
                    _complianceService
                        .createNewRecordState(recordState)
                        .done(function () {
                            abp.notify.info(app.localize('savedsuccessfully'));
                            _modalManager.close();
                            abp.event.trigger('app.CreateNewRecordSaved');
                        })
                        .always(function () {
                            //_modalManager.setBusy(false);
                        });

                })
                .always(function () {
                    _modalManager.setBusy(false);
                }
                );
        };


        $('#Record_filedata').change(function () {
            var file = $(this)[0].files[0];
            if (!file) {
                _filedataToken = null;
                return;
            }

            var formData = new FormData();
            $('#Record_filename').val(file.name);
            formData.append('file', file);
            _fileUploading.push(true);

            $.ajax({
                url: '/App/Compliance/UploadfiledataFile',
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
            })
                .done(function (resp) {
                    if (resp.success && resp.result.fileToken) {
                        _SaveButton.removeAttr("disabled");

                        _filedataToken = resp.result.fileToken;
                    } else {
                        _SaveButton.attr('disabled', 'disabled');
                        abp.message.error(resp.result.message);
                    }
                })
                .always(function () {
                    _fileUploading.pop();
                });
        });

        //$('#Record_filedata_Remove').click(function () {
        //    abp.message.confirm(app.localize('DoYouWantToRemoveTheFile'), app.localize('AreYouSure'), function (isConfirmed) {
        //        if (isConfirmed) {
        //            var Record = _$recordInformationForm.serializeFormToObject();
        //            _recordsService
        //                .removefiledataFile({
        //                    id: Record.id,
        //                })
        //                .done(function () {
        //                    abp.notify.success(app.localize('SuccessfullyDeleted'));
        //                    _$recordInformationForm.find('#div_current_file').css('display', 'none');
        //                });
        //        }
        //    });
        //});

        $('#Record_filedata').change(function () {
            var fileName = app.localize('ChooseAFile');
            if (this.files && this.files[0]) {
                fileName = this.files[0].name;
            }
            $('#Record_filedataLabel').text(fileName);
        });

        $('#CreateNewRecordModalFileUploadButton').on('click', function (e) {
            e.preventDefault();
            $('#Record_filedata').click();
        });


        console.log('CreateNewRecordModal loaded');
        //// Stepper

        // Stepper lement
        var element = document.querySelector("#newRecordStateUploadWizard");

        // Initialize Stepper
        var stepper = new KTStepper(element);

        // Handle next step
        stepper.on("kt.stepper.next", function (stepper) {
            stepper.goNext(); // go next step
        });

        // Handle previous step
        stepper.on("kt.stepper.previous", function (stepper) {
            stepper.goPrevious(); // go previous step
        });
        stepper.on("kt.stepper.changed", function () {
            $('#stepper_next_button').attr('disabled', 'disabled');
            stepperButtons();
        });

        var setNextTooltip = function (msg) {
            $("#stepper_next_button_tooltip").attr("title", msg);
        }

        var stepperButtons = function () {
            var _step = stepper.getCurrentStepIndex();
            //if (_step == 1) {
            //    if ($("#RecordCategoryName").val().length > 0) {
            //        setNextTooltip(abp.localization.localize('ClickNextToConfirmCategory'));
            //        $('#stepper_next_button').removeAttr("disabled");
            //        $("#ConfirmRecordCategoryName").val('');
            //    }
            //    else {
            //        $('#stepper_next_button').attr('disabled', 'disabled');

            //    }
            //}
            //if (_step == 2) {
            //    setNextTooltip(abp.localization.localize('ConfirmCategorySelection'))
            //    if ($("#ConfirmRecordCategoryName").val().length > 0 && $("#ConfirmRecordCategoryName").val() == $("#RecordCategoryName").val()) {
            //        setNextTooltip(abp.localization.localize('ClickNextToReviewInstructions'));
            //        $('#stepper_next_button').removeAttr("disabled");
            //    }
            //    else {
            //        $('#stepper_next_button').attr('disabled', 'disabled');
            //        if ($("#ConfirmRecordCategoryName").val().length > 0) {
            //            abp.message.error(abp.localization.localize('CategoriesDotNotMatch'));
            //        }
            //    }
            //}
            if (_step == 1) {
                setNextTooltip(abp.localization.localize('ConfirmYouHaveReviewedInstructions'));

                if ($("#Record_InstructionsConfirmed").is(":checked") == true) {
                    setNextTooltip(abp.localization.localize('ClickNextToUploadYourFile'));
                    $('#stepper_next_button').removeAttr("disabled");
                }
                else {
                    $('#stepper_next_button').attr('disabled', 'disabled');

                };

            }
            if (_step == 2) {
                setNextTooltip(abp.localization.localize('SelectYourFileToCompleteYourUpload'))

                //    if ((_filedataToken + '').length > 0) {
                //        _SaveButton.removeAttr("disabled");
                //    }
                //    else {
                //        _SaveButton.attr('disabled', 'disabled');
                //    }
            }
        }

        $("#RecordCategoryName").on("change", stepperButtons);
        $("#Record_InstructionsConfirmed").on("click", stepperButtons);

    };
})(jQuery);


