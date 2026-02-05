(function ($) {
    app.modals.CreateEditRequirementModal = function () {
        console.log('CreateEditRequirementModal compliance');
        var _complianceService = abp.services.app.surpathCompliance;
        var _recordCategoriesService = abp.services.app.recordCategories;


        var _modalManager;
        //var _$recordStateInformationForm = null;
        var _$createEditRequirementForm = null;
        //var _fileUploading = [];
        //var _filedataToken;
        var _SaveButton;
        var _Saving = false;
        var _CancelButton;

        var _RecordRequirementtenantDepartmentLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RecordRequirements/TenantDepartmentLookupTableModal',
            scriptUrl:
                abp.appPath +
                'view-resources/Areas/App/Views/RecordRequirements/_RecordRequirementTenantDepartmentLookupTableModal.js',
            modalClass: 'TenantDepartmentLookupTableModal',
        });
        var _RecordRequirementcohortLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RecordRequirements/CohortLookupTableModal',
            scriptUrl:
                abp.appPath + 'view-resources/Areas/App/Views/RecordRequirements/_RecordRequirementCohortLookupTableModal.js',
            modalClass: 'CohortLookupTableModal',
        });

        var _CreateEditCategoryModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RecordCategories/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RecordCategories/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditRecordCategoryModal',
        });


        this.init = function (modalManager, _args) {

            console.log('CreateEditRequirementModal compliance init');
            console.log(_args);
            _modalManager = modalManager;

            // var modal = _modalManager.getModal();
            _modalManager.onBeforeClose(function () {
                //if (_Saving === false && $('#CreateOrEditRecordRequirement_RecordRequirement_Id').val().length > 0) {
                //    debugger;
                //}
            });




            _SaveButton = _modalManager.getModal().find('button.save-button');
            _CancelButton = _modalManager.getModal().find('button.close-button');
            _SaveButton.attr('disabled', 'disabled');
            _$createEditRequirementForm = _modalManager.getModal().find('form[name=CreateEditRequirementForm]');

            _$createEditRequirementForm.validate();
            stepperButtons();



        };



        $('#OpenTenantDepartmentLookupTableButton').click(function () {
            var recordRequirement = _$createEditRequirementForm.serializeFormToObject();
            _RecordRequirementtenantDepartmentLookupTableModal.open(
                {
                    id: recordRequirement.CreateOrEditRecordRequirement_RecordRequirement_TenantDepartmentId,
                    displayName: recordRequirement.CreateOrEditRecordRequirement_tenantDepartmentName
                },
                function (data) {
                    _$createEditRequirementForm.find('input[id=CreateOrEditRecordRequirement_TenantDepartmentName]').val(data.displayName);
                    _$createEditRequirementForm.find('input[id=CreateOrEditRecordRequirement_RecordRequirement_TenantDepartmentId]').val(data.id);
                    stepperButtons();

                }
            );
        });

        $('#ClearTenantDepartmentNameButton').click(function () {
            _$createEditRequirementForm.find('input[id=CreateOrEditRecordRequirement_TenantDepartmentName]').val('');
            _$createEditRequirementForm.find('input[id=CreateOrEditRecordRequirement_RecordRequirement_TenantDepartmentId]').val('');
            stepperButtons();

        });

        $('#OpenCohortLookupTableButton').click(function () {
            var recordRequirement = _$createEditRequirementForm.serializeFormToObject();

            _RecordRequirementcohortLookupTableModal.open(
                { id: recordRequirement.CreateOrEditRecordRequirement_RecordRequirement_CohortId, displayName: recordRequirement.CreateOrEditRecordRequirement_CohortName },
                function (data) {
                    _$createEditRequirementForm.find('input[id=CreateOrEditRecordRequirement_CohortName]').val(data.displayName);
                    _$createEditRequirementForm.find('input[id=CreateOrEditRecordRequirement_RecordRequirement_CohortId]').val(data.id);
                    stepperButtons();

                }
            );
        });

        $('#ClearCohortNameButton').click(function () {
            _$createEditRequirementForm.find('input[id=CreateOrEditRecordRequirement_CohortName]').val('');
            _$createEditRequirementForm.find('input[id=CreateOrEditRecordRequirement_RecordRequirement_CohortId]').val('');
        });

        //$('select.surpath-rule-dropdown').on('click', 'option', function (e) {
        //    var _this = $(this);
        //    debugger;
        //});

        //$('select.surpath-rule-dropdown').change(function (e) {
        //    var _this = $(this);
        //    debugger;
        //});
        //$('select.surpath-rule-dropdown').on('blur', function (e) {
        //    debugger;
        //});

         $("select.surpath-rule-dropdown").mouseup(function (e) {
            var open = $(this).data("isopen");

            if (open) {
                rulepicked(e);
             };

            $(this).data("isopen", !open);
         }).blur(function () {
             $(this).data('isopen', false);
         }).keyup(function (e) {
             if (ev.keyCode == 13) {
                 rulepicked(e);
             }
                 
         });

        rulepicked = function (e) {
            var _val = $(e.target).val();
            if (_val == "") {
                abp.notify.error(app.localize('ARuleIsRequired'), app.localize('ARuleIsRequiredTitle'));
            }
        }

        $('#CreateNewRecordCategoryButton').click(function (e) {
            e.preventDefault();
            _CreateEditCategoryModal.open({ nosave: true },
                function (data) {
                    AddNewCategory(data);
                    MultipleCategoriesCheckboxEnableCheck();
                    
                    //abp.notify.success(app.localize('SuccessfullyAdded'));
                    //_modalManager.getModal().on('hidden.bs.modal', function (e) {
                    //    _modalManager.reopen({ step: stepper.getCurrentStepIndex() });
                    //});
                    //_modalManager.close();
                }
            );

            //var _reqId = $('#CreateOrEditRecordRequirement_RecordRequirement_Id').val();
            //_CreateEditCategoryModal.open({ reqid: _reqId, nosave: true },
            //    function (data) {
            //        debugger;

            //        abp.notify.success(app.localize('SuccessfullyAdded'));
            //        _modalManager.getModal().on('hidden.bs.modal', function (e) {
            //            _modalManager.reopen({ step: stepper.getCurrentStepIndex() });
            //        });
            //        _modalManager.close();
            //    }
            //);
        });

        AddNewCategory = function (data) {
            var _template = $('#recordCategoryCardTemplate').clone(true);
            var _catcount = $('#categoryWrapper').data('catcount') + 1;
            //var _catcount = $('.recordCategoryCard').length;
            //var _CreateOrEditRecordCategories_x__RecordCategory_Id = "CreateOrEditRecordCategories_" + _catcount + "__RecordCategory_Id";
            //var _CreateOrEditRecordCategories_x__RecordCategory_RecordRequirementId = "_CreateOrEditRecordCategories_" + _catcount + "__RecordCategory_RecordRequirementId";
            //var _CreateOrEditRecordCategories_x__RecordCategory_Id = $(_template).find('#CreateOrEditRecordCategories_x__RecordCategory_Id');
            $(_template).find('#CreateOrEditRecordCategories_x__RecordCategory_Id')
                .attr('id', "CreateOrEditRecordCategories_" + _catcount + "__RecordCategory_Id");
            $(_template).find('#_CreateOrEditRecordCategories_x__RecordCategory_RecordRequirementId')
                .attr('id', "_CreateOrEditRecordCategories_" + _catcount + "__RecordCategory_RecordRequirementId");
            $(_template).find('#CreateOrEditRecordCategories_x__RecordCategory_Name')
                .attr('id', "CreateOrEditRecordCategories_" + _catcount + "__RecordCategory_Name")
                .val(data.name);
            $(_template).find('#CreateOrEditRecordCategories_x__RecordCategory_Instructions')
                .attr('id', "CreateOrEditRecordCategories_" + _catcount + "__RecordCategory_Instructions")
                .val(data.instructions);
            $(_template).find('#CreateOrEditRecordCategories_x__RecordCategory_RecordCategoryRuleId option[value="' + data.recordCategoryRuleId + '"]').attr('selected', 'selected');
            //$(CreateOrEditRecordCategories_x__RecordCategory_RecordCategoryRuleId) = data.
            

            $(_template).addClass('recordCategoryCard');
            $(_template).addClass('recordCategoryCard-' + _catcount);
            
            $(_template).attr('id', '');
            $('#categoryWrapper').append(_template);

            $(_template).data('catnumber', _catcount);
            $(_template).removeClass('d-none');
            $(_template).find('#deleteCategory').addClass('catnumber-' + _catcount);
            $(_template).find('#deleteCategory').data('catnumber', _catcount);
            $('#categoryWrapper').data('catcount', _catcount);


            //var _wrapper = $('#categoryWrapper');
            //$('recordCategoryCard-' + _catcount).data('catnumber', _catcount);
            //$('recordCategoryCard-' + _catcount).removeClass('d-none');
            //$('recordCategoryCard-' + _catcount).find('#deleteCategory').addClass('catnumber-' + _catcount);
            //$('recordCategoryCard-' + _catcount).find('#deleteCategory').data('catnumber', _catcount);
            //$('#categoryWrapper').data('catcount', _catcount);
           // $('.catnumber-' + _catcount).data('catnumber', _catcount);
            //debugger;
        }

        var genNewRequirement = function () {
            var newRequirement = _$createEditRequirementForm.serializeFormToObject();

            //var _CreateEdictRecordRequirement = {};
            //_CreateEdictRecordRequirement.CreateOrEditRecordRequirement = {};
            //_CreateEdictRecordRequirement.CreateOrEditRecordCategories = [];
            ////debugger;


            //if (newRequirement.CreateOrEditRecordCategories.length == 1) {
            //    if (newRequirement.CreateOrEditRecordCategories[0].Name.length < 1) {
            //        newRequirement.CreateOrEditRecordCategories[0].Name = newRequirement.CreateOrEditRecordRequirement.Name;
            //    }
            //    if (newRequirement.CreateOrEditRecordCategories[0].Instructions.length < 1) {
            //        newRequirement.CreateOrEditRecordCategories[0].Instructions = 'Instructions';
            //    }
            //}
            //if (newRequirement.CreateOrEditRecordCategories.length = 1) {
            //    if (newRequirement.CreateOrEditRecordCategories[0].Name.length < 1) {
            //        newRequirement.CreateOrEditRecordCategories[0].Name = newRequirement.CreateOrEditRecordRequirement.Name;
            //    }
            //}
            //debugger;


            return newRequirement;
        }

        var saveAndContinue = function () {

            var newRequirement = genNewRequirement();
            _modalManager.setBusy(true);
            _complianceService
                .createEditRequirement(newRequirement)
                .done(function (data) {
                    _modalManager.setArgs({ id: data.id });
                    _modalManager.getModal().on('hidden.bs.modal', function (e) {
                        _modalManager.reopen();
                    });
                    _Saving = true;
                    _modalManager.close();
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        }

        //this.onClose = function () {
        //    debugger;
        //}



        this.save = function () {
            //if (!_$recordStateInformationForm.valid()) {
            //    return;
            //}
            //if ($('#RecordState_RecordId').prop('required') && $('#RecordState_RecordId').val() == '') {
            //    abp.message.error(app.localize('{0}IsRequired', app.localize('Record')));
            //    return;
            //}
            //if ($('#RecordState_RecordCategoryId').prop('required') && $('#RecordState_RecordCategoryId').val() == '') {
            //    abp.message.error(app.localize('{0}IsRequired', app.localize('RecordCategory')));
            //    return;
            //}
            //if ($('#RecordState_UserId').prop('required') && $('#RecordState_UserId').val() == '') {
            //    abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
            //    return;
            //}
            //if ($('#RecordState_RecordStatusId').prop('required') && $('#RecordState_RecordStatusId').val() == '') {
            //    abp.message.error(app.localize('{0}IsRequired', app.localize('RecordStatus')));
            //    return;
            //}
            //if (_fileUploading != null && _fileUploading.length > 0) {
            //    abp.notify.info(app.localize('WaitingForFileUpload'));
            //    return;
            //}

            //var newRequirement = _$createEditRequirementForm.serializeFormToObject();

            //var _CreateEdictRecordRequirement = {};
            //_CreateEdictRecordRequirement.CreateOrEditRecordRequirement = {};
            //_CreateEdictRecordRequirement.CreateOrEditRecordCategories = [];
            //debugger;
            var newRequirement = genNewRequirement();

            _modalManager.setBusy(true);
            _complianceService
                .createEditRequirement(newRequirement)
                .done(function () {
                    abp.notify.info(app.localize('savedsuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.RecordRequirementSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });


            //var recordState = _$recordStateInformationForm.serializeFormToObject();
            //recordState.filedataToken = _filedataToken;
            //var record = {
            //    binaryObjId: "00000000-0000-0000-0000-000000000000",
            //    dateLastUpdated: "",
            //    dateUploaded: "",
            //    filedataToken: _filedataToken,
            //    filename: recordState.filename,
            //    metadata: "{}",
            //    physicalfilepath: "",
            //    tenantDocumentCategoryId: "",
            //    tenantDocumentCategoryName: "",
            //}

            //_modalManager.setBusy(true);

            //_complianceService
            //    .createNewRecord(record)
            //    .done(function (data) {
            //        recordState.binaryObjId = data.record.binaryObjId;
            //        recordState.recordId = data.record.id;
            //        //console.log(data);
            //        //debugger;
            //        //// $('#Record_BinaryObjId').val(data)

            //    })
            //    .then(function () {
            //        _complianceService
            //            .createNewRecordState(recordState)
            //            .done(function () {
            //                abp.notify.info(app.localize('savedsuccessfully'));
            //                _modalManager.close();
            //                abp.event.trigger('app.CreateNewRecordSaved');
            //            })
            //            .always(function () {
            //                //_modalManager.setBusy(false);
            //            });

            //    })
            //    .always(function () {
            //        _modalManager.setBusy(false);
            //    }
            //    );
        };

        ////// Stepper

        //// Stepper lement
        var element = document.querySelector("#CreateRequirementWizard");

        //// Initialize Stepper
        var stepper = new KTStepper(element);
        //// Handle next step
        stepper.on("kt.stepper.next", function (stepper) {
            stepper.goNext(); // go next step
        });

        //// Handle previous step
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
            if (_step == 1) {
                if ($("#CreateOrEditRecordRequirementModalViewModel_RecordRequirement_Name").val().length > 0) {
                    $('#stepper_next_button').removeAttr("disabled");
                    //setNextTooltip(abp.localization.localize('ClickNextToConfirmCategory'));
                    ////if ($("#CreateOrEditRecordRequirement_RecordRequirement_Id").val().length > 0) {
                    ////    $('#stepper_next_button').removeAttr("disabled");

                    ////}
                    ////else {
                    ////    $('#stepper_save_first_button').removeAttr("disabled");
                    ////    $('#stepper_save_first_button').removeClass("d-none");

                    ////}
                    //$("#ConfirmRecordCategoryName").val('');
                }
                else {

                    $('#stepper_next_button').attr('disabled', 'disabled');

                }
            }
            if (_step == 2) {

                if ($('.surpath-rule-dropdown option:selected[value=""]').length > 0) {
                    $('.surpath-rule-dropdown option:selected[value=""]')[0].focus();
                    return;
                }
                if ($('#MultipleCategories').prop('checked') == false) {
                    if ($('#defaultRule').val() != '' && $('#defaultInstructions').val().length > 5) {
                        $('#stepper_next_button').removeAttr("disabled");

                    }
                }
                else {
                    $('#stepper_next_button').removeAttr("disabled");

                }
                MultipleCategoriesCheckboxEnableCheck();
                // surpath-rule-dropdown class - a rule is required.


                //saveAndContinue();
                ////setNextTooltip(abp.localization.localize('ConfirmCategorySelection'))
                //if ($("#ConfirmRecordCategoryName").val().length > 0 && $("#ConfirmRecordCategoryName").val() == $("#RecordCategoryName").val()) {
                //    setNextTooltip(abp.localization.localize('ClickNextToReviewInstructions'));
                //    $('#stepper_next_button').removeAttr("disabled");
                //}
                //else {
                //    $('#stepper_next_button').attr('disabled', 'disabled');
                //    if ($("#ConfirmRecordCategoryName").val().length > 0) {
                //        abp.message.error(abp.localization.localize('CategoriesDotNotMatch'));
                //    }
                //}
            }
            if (_step == 3) {
                buildreview();
                _SaveButton.removeAttr("disabled");
                //setNextTooltip(abp.localization.localize('ConfirmYouHaveReviewedInstructions'));

                //if ($("#Record_InstructionsConfirmed").is(":checked") == true) {
                //    setNextTooltip(abp.localization.localize('ClickNextToUploadYourFile'));
                //    $('#stepper_next_button').removeAttr("disabled");
                //}
                //else {
                //    $('#stepper_next_button').attr('disabled', 'disabled');

                //};

            }
            //    if (_step == 4) {
            //        setNextTooltip(abp.localization.localize('SelectYourFileToCompleteYourUpload'))

            //        //    if ((_filedataToken + '').length > 0) {
            //        //        _SaveButton.removeAttr("disabled");
            //        //    }
            //        //    else {
            //        //        _SaveButton.attr('disabled', 'disabled');
            //        //    }
            //    }
        }



        buildreview = function () {
            // populate reviewWrapper
//<div id="reviewWrapper">
//    <div id="reviewName" class="my-3"></div>
//    <div id="reviewDescription" class="my-3"></div>
//    <div id="reviewAppliesToDept" class="my-3"></div>
//    <div id="reviewAppliesToCohort" class="my-3"></div>
//    <div id="reviewAppliesToAll" class="my-3"></div>
//    <div id="reviewCatsWrapper" class="my-e">
                            
//    </div>
                        
//</div>
//<div id="catReviewTemplate" class="my-3 d-none">
//    <div id="reviewCatName" class="my-3"></div>
//    <div id="reviewCatInstructions" class="my-3"></div>
//    <div id="reviewCatRule" class="my-3"></div>

//</div>
            //var _label = $('<h6>');
            //_label.text('Name:');
            $('#reviewName')
                .append($('<h6>Name:</h6>'))
                .append($('#CreateOrEditRecordRequirementModalViewModel_RecordRequirement_Name').val());
            $('#CreateOrEditRecordRequirementModalViewModel_RecordRequirement_Description')
                .append($('<h6>Description</h6>'))
                .append($('#CreateOrEditRecordRequirementModalViewModel_RecordRequirement_Description').val());
            if ($('#CreateOrEditRecordRequirement_TenantDepartmentName').val().length > 0) {
                $('#reviewAppliesToDept')
                    .append($('#CreateOrEditRecordRequirement_TenantDepartmentName').val());
            }
            if ($('#CreateOrEditRecordRequirement_CohortName').val().length > 0) {
                $('#reviewAppliesToCohort')
                    .append($('#CreateOrEditRecordRequirement_CohortName').val());
            }
            if ($('#CreateOrEditRecordRequirement_CohortName').val().length < 1 && $('#CreateOrEditRecordRequirement_TenantDepartmentName').val().length <1) {
                $('#reviewAppliesToAll')
                    .append('Applies to everyone');
            }
            var _catWrapper = $('#reviewCatsWrapper');
            _catWrapper
                .append($('<h4>Requirements:</h4>'))
                .append($('<hr>'));

            $('.recordCategoryCard').each(function (_idx) {
                //debugger;

                var _cat = $('.recordCategoryCard')[_idx];
                var _catName = $(_cat).find('[id$="RecordCategory_Name"]').val();
                var _catInstructions = $(_cat).find('[id$="__RecordCategory_Instructions"]').val();
                
                var _catRule = $(_cat).find('select option:selected').text(); // $(_cat).find('[id$ = "__RecordCategory_Instructions"] option:selected').text();
                var _template = $('#catReviewTemplate').clone();
                _template.find('#reviewCatName')
                    .append($('<h6>Requirement Name:</h6>'))
                    .append(_catName);
                _template.find('#reviewCatInstructions')
                    .append($('<h6>Requirement Instructions:</h6>'))
                    .append(_catInstructions);
                _template.find('#reviewCatRule')
                    .append($('<h6>Requirement Rules:</h6>'))
                    .append(_catRule);
                _template.removeClass('d-none');
                _template.attr('id', '');
                console.log(_template);
                _catWrapper.append(_template);
            });

            //$('#reviewDescription').text($('#CreateOrEditRecordRequirementModalViewModel_RecordRequirement_Description').val());
        }
        //$("#RecordCategoryName").on("change", stepperButtons);
        //$("#Record_InstructionsConfirmed").on("click", stepperButtons);

        $('#stepper_save_first_button').on("click", saveAndContinue);

        MultipleCategoriesCheckboxEnableCheck = function () {
            //debugger;
            if ($('.recordCategoryCard').length > 1) {
                $(".MultiStepDisableWarning").removeClass('d-none');
                $("#MultipleCategories").attr('disabled', true);
                // MultipleCategoriesCheckboxEnableCheck
            }
            else {
                $(".MultiStepDisableWarning").addClass('d-none');
                $("#MultipleCategories").attr('disabled', false);
            }
            //$("#MultipleCategories").attr('disabled', $('.recordCategoryCard').length > 1);
            //if ($('.recordCategoryCard').length > 1) {
            //    $("#MultipleCategories").attr('disabled'); //, 'disabled');
            //}
            //else {
            //    $("#MultipleCategories").removeAttr('disabled');
            //}
        }

        $("#MultipleCategories").change(function (e) {
            if ($(this).is(":checked")) {

                $(".defaultInstructions").addClass("d-none");
                $(".defaultRule").addClass("d-none");
                $(".recordCategoryCard, #CreateNewRecordCategoryButtonWrapper").removeClass("d-none");
            }
            else {
                $(".recordCategoryCard, #CreateNewRecordCategoryButtonWrapper").addClass("d-none");
                $(".defaultInstructions").removeClass("d-none");
                $(".defaultRule").removeClass("d-none");
            }
        });
        MultipleCategoriesCheckboxEnableCheck();

        $('#CreateOrEditRecordRequirementModalViewModel_RecordRequirement_Name').on('change', function () {
            // Only update category name if there's exactly one category
            if ($('.recordCategoryCard').length === 1) {
                $('.recordCategoryCard-base [id$="RecordCategory_Name"]').val($('#CreateOrEditRecordRequirementModalViewModel_RecordRequirement_Name').val());
            }
        });

        $('#defaultInstructions').on('blur', function () {
            if ($('.recordCategoryCard-base [id$="__RecordCategory_Instructions"]').val().length < 1) {
                $('.recordCategoryCard-base [id$="__RecordCategory_Instructions"]').val($('#defaultInstructions').val());
            }
        });

        $('#defaultRule').on('change', function () {
            var _selectedVal = $('#defaultRule option:selected').val();
//            $(_template).find('#CreateOrEditRecordCategories_x__RecordCategory_RecordCategoryRuleId option[value="' + data.recordCategoryRuleId + '"]').attr('selected', 'selected');
            $('.recordCategoryCard-base [id$="__RecordCategory_RecordCategoryRuleId"] option[value="' + _selectedVal + '"]').attr('selected', 'selected');
        //    if ($('.recordCategoryCard-base [id$="__RecordCategory_RecordCategoryRuleId"]').val().length < 1) {
        //        $('.recordCategoryCard-base [id$="__RecordCategory_RecordCategoryRuleId"]').val($('#defaultInstructions').val());
        //    }
        });



        $('.btn-delete-category').on('click', function (e) {
            var btn = $(e.target);
            var _id = btn.data('recordcategoryid');
            var _num = btn.data('catnumber');
            console.log(_id);
            console.log(_num);

            //var serializedForm = _$createEditRequirementForm.serializeFormToObject();
            //console.log(serializedForm);
            abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
                if (isConfirmed) {
                    //var serializedForm = _$createEditRequirementForm.serializeFormToObject();
                    //var recordCategory = serializedForm.CreateOrEditRecordCategoryViewModels.find((o) => { return o.RecordCategory.Id === _id }).RecordCategory;
                    //debugger;
                    $('.recordCategoryCard-' + _num).remove();
                    MultipleCategoriesCheckboxEnableCheck();

                    //_recordCategoriesService
                    //    .delete({
                    //        id: _id,
                    //    })
                    //    .done(function () {
                    //        // getRecordCategories(true);
                    //        _modalManager.getModal().on('hidden.bs.modal', function (e) {
                    //            _modalManager.reopen();
                    //        });
                    //        _modalManager.close();
                    //        abp.notify.success(app.localize('SuccessfullyDeleted'));
                    //    });
                   


                };
            });
        });
        $('select').on("change", function (e) {
            stepperButtons();
        });
        $("#CreateEditRequirementForm input").on("blur", function (e) {
            //if ($(e.target).valid() == true) {
            //    stepperButtons();
            //}
            if ($("#CreateEditRequirementForm").validate().element(e.target)) {
                stepperButtons();
            }
        });



        console.log('CreateEditRequirementModal compliance loaded');

    };
})(jQuery);


