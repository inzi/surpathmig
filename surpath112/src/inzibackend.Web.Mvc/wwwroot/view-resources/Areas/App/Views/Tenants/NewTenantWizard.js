(function () {
    $(function () {

        console.log('new tenant wizard');

        //Custom validation type for tenancy name
        $.validator.addMethod(
            'tenancyNameRegex',
            function (value, element, regexpr) {
                return regexpr.test(value);
            },
            app.localize('TenantName_Regex_Description')
        );


        //start
        var _tenantService = abp.services.app.tenant;
        var _$tenantInformationForm = null;
        var _passwordComplexityHelper = new app.PasswordComplexityHelper();

        var $selectedDateTime = {
            startDate: moment()
        };

        var _$NewTenantWizardStepperForm = $('#NewTenantWizardStepperForm');
        _$NewTenantWizardStepperForm.validate({
            rules: {
                TenancyName: {
                    tenancyNameRegex: new RegExp(_$NewTenantWizardStepperForm.find('input[name=TenancyName]').attr('regex')),
                },
            },
        });

        var passwordInputs = _$NewTenantWizardStepperForm.find('input[name=AdminPassword],input[name=AdminPasswordRepeat]');
        var passwordInputGroups = passwordInputs.closest('.tenant-admin-password');

        var userPasswordInputs = _$NewTenantWizardStepperForm.find('input[name=UserAdminPassword],input[name=UserAdminPasswordRepeat]');
        var userPasswordInputGroups = userPasswordInputs.closest('.user-password');

        _passwordComplexityHelper.setPasswordComplexityRules(passwordInputs, window.passwordComplexitySetting);
        _passwordComplexityHelper.setPasswordComplexityRules(userPasswordInputs, window.passwordComplexitySetting);

        $('#CreateTenant_SetRandomPassword').change(function () {
            if ($(this).is(':checked')) {
                passwordInputGroups.slideUp('fast');
                passwordInputs.removeAttr('required');
            } else {
                passwordInputGroups.slideDown('fast');
                passwordInputs.attr('required', 'required');
            }
        });
        $('#UserAdminSetRandomPassword').change(function () {
            if ($(this).is(':checked')) {
                userPasswordInputGroups.slideUp('fast');
                userPasswordInputs.removeAttr('required');
            } else {
                userPasswordInputGroups.slideDown('fast');
                userPasswordInputs.attr('required', 'required');
            }
        });


        _$NewTenantWizardStepperForm.find('.date-time-picker').daterangepicker({
            singleDatePicker: true,
            timePicker: true,
            parentEl: '#CreateTenantInformationsForm',
            startDate: moment().startOf('minute'),
            locale: {
                format: 'MM/DD/YYYY hh:mm A',
            },
        }, (start) => $selectedDateTime.startDate = start);

        var $subscriptionEndDateDiv = _$NewTenantWizardStepperForm.find('input[name=SubscriptionEndDateUtc]').parent('div');
        var $isUnlimitedInput = _$NewTenantWizardStepperForm.find('#CreateTenant_IsUnlimited');
        var subscriptionEndDateUtcInput = _$NewTenantWizardStepperForm.find('input[name=SubscriptionEndDateUtc]');
        function toggleSubscriptionEndDateDiv() {
            if ($isUnlimitedInput.is(':checked')) {
                $subscriptionEndDateDiv.slideUp('fast');
                subscriptionEndDateUtcInput.removeAttr('required');
            } else {
                $subscriptionEndDateDiv.slideDown('fast');
                subscriptionEndDateUtcInput.attr('required', 'required');
            }
        }

        var $isInTrialPeriodInputDiv = _$NewTenantWizardStepperForm.find('#CreateTenant_IsInTrialPeriod').closest('div');
        var $isInTrialPeriodInput = _$NewTenantWizardStepperForm.find('#CreateTenant_IsInTrialPeriod');
        function toggleIsInTrialPeriod() {
            if ($isUnlimitedInput.is(':checked')) {
                $isInTrialPeriodInputDiv.slideUp('fast');
                $isInTrialPeriodInput.prop('checked', false);
            } else {
                $isInTrialPeriodInputDiv.slideDown('fast');
            }
        }

        $isUnlimitedInput.change(function () {
            toggleSubscriptionEndDateDiv();
            toggleIsInTrialPeriod();
        });

        var $editionCombobox = _$NewTenantWizardStepperForm.find('#EditionId');
        $editionCombobox.change(function () {
            var isFree = $('option:selected', this).attr('data-isfree') === 'True';
            var selectedValue = $('option:selected', this).val();

            if (selectedValue === '' || isFree) {
                _$NewTenantWizardStepperForm.find('.subscription-component').slideUp('fast');
                if (isFree) {
                    $isUnlimitedInput.prop('checked', true);
                } else {
                    $isUnlimitedInput.prop('checked', false);
                }
            } else {
                $isUnlimitedInput.prop('checked', false);
                toggleSubscriptionEndDateDiv();
                toggleIsInTrialPeriod();
                _$NewTenantWizardStepperForm.find('.subscription-component').slideDown('fast');
            }
        });

        function getDefaultEdition() {
            abp.services.app.commonLookup.getDefaultEditionName().done(function (defaultEdition) {
                var $editionCombobox = _$NewTenantWizardStepperForm.find('#EditionId');
                $editionCombobox.find('option').each(function () {
                    if ($(this).text() == defaultEdition.name) {
                        $(this).prop('selected', true).trigger('change');
                    }
                });
            });
        };

        toggleSubscriptionEndDateDiv();
        toggleIsInTrialPeriod();
        $editionCombobox.trigger('change');

        getDefaultEdition();


        //'NewTenantWizardStepperForm';


        //// Stepper
        // Stepper lement
        var element = document.querySelector("#newTenantStepperWizard");

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
            //console.log('stepperbuttons');
            //$('#stepper_next_button').removeAttr("disabled");

            if (_step == 1) {
                //if ($("#Name").valid() && $("#TenancyName").valid() && $("#ClientCode").valid()) {
                if ($(".step1NewTenantWizard input").valid()) {
                    //if ($("#Name").val().length > 0 && $("#TenancyName").val().length > 0 && $("#ClientCode").val().length > 0) {
                    setNextTooltip(abp.localization.localize('Continue'));
                    $('#stepper_next_button').removeAttr("disabled");

                }
                else {
                    $('#stepper_next_button').attr('disabled', 'disabled');
                }
            }
            if (_step == 2) {
                //$('#UserAdminUserName').valid();
                if ($(".step2NewTenantWizard input").valid()) {
                    //if ($("#Name").val().length > 0 && $("#TenancyName").val().length > 0 && $("#ClientCode").val().length > 0) {
                    setNextTooltip(abp.localization.localize('Continue'));
                    $('#stepper_next_button').removeAttr("disabled");

                }
                else {
                    $('#stepper_next_button').attr('disabled', 'disabled');
                }

            }
            if (_step == 3) {
                $('#SubmitNewTenantWizard').removeAttr("disabled");
                //    setNextTooltip(abp.localization.localize('ConfirmYouHaveReviewedInstructions'));

                //    if ($("#Record_InstructionsConfirmed").is(":checked") == true) {
                //        setNextTooltip(abp.localization.localize('ClickNextToUploadYourFile'));
                //        $('#stepper_next_button').removeAttr("disabled");
                //    }
                //    else {
                //        $('#stepper_next_button').attr('disabled', 'disabled');

                //    };

            }
            //if (_step == 4) {
            //    setNextTooltip(abp.localization.localize('SelectYourFileToCompleteYourUpload'))

            //    //    if ((_filedataToken + '').length > 0) {
            //    //        _SaveButton.removeAttr("disabled");
            //    //    }
            //    //    else {
            //    //        _SaveButton.attr('disabled', 'disabled');
            //    //    }
            //}
        };

        //$(".step1NewTenant").on("change", stepperButtons);
        //$("#Record_InstructionsConfirmed").on("click", stepperButtons);

        //$("#NewTenantWizardStepperForm input").on("blur", stepperButtons);
        $("#NewTenantWizardStepperForm input").on("blur", function (e) {
            //if ($(e.target).valid() == true) {
            //    stepperButtons();
            //}
            if ($("#NewTenantWizardStepperForm").validate().element(e.target)) {
                stepperButtons();
            }
        });
        // $(".step1NewTenantWizard input").on("blur", stepperButtons);
        //$(".step1NewTenantWizard input").on("blur", stepperButtons);
        //$(".step1NewTenantWizard input").on("change", stepperButtons);
        //$(".step1NewTenantWizard input").on("keyup", stepperButtons);
        //$(".step1NewTenantWizard input").on("blur", function (e) {
        //    //$(".step1NewTenantWizard input").validate().element($(e.target));
        //    $(e.target).valid()
        //}) 
        // End spinner
        //$('.surpath-usernamefromemail').on('change', function (e) {
        //    $('.surpath-usernamefromemail').val($('.surpath-emailtousername').val());
        //});
        //$('.surpath-usernamefromemail').on('keyup', function (e) {
        //    $('.surpath-usernamefromemail').val($('.surpath-emailtousername').val());
        //});

        // save function:
        saveTenant = function () {
            if (!_$NewTenantWizardStepperForm.valid()) {
                return;
            }
            var tenant = _$NewTenantWizardStepperForm.serializeFormToObject();

            //take selected date as UTC
            if ($('#CreateTenant_IsUnlimited').is(':visible') && !$('#CreateTenant_IsUnlimited').is(':checked')) {
                tenant.SubscriptionEndDateUtc = $selectedDateTime.startDate.format('YYYY-MM-DDTHH:mm:ss') + 'Z';
            } else {
                tenant.SubscriptionEndDateUtc = null;
            }

            if ($('#CreateTenant_IsUnlimited').is(':checked')) {
                tenant.IsInTrialPeriod = false;
            }

            if (tenant.SetRandomPassword) {
                tenant.Password = null;
                tenant.AdminPassword = null;
            }

            if (tenant.UseHostDb) {
                tenant.ConnectionString = null;
            }


            abp.ui.setBusy($('body'));

            _tenantService
                .createTenantWizard(tenant)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    // _modalManager.close();
                    // abp.event.trigger('app.createTenantModalSaved');
                })
                .always(function () {
                    abp.ui.clearBusy($('body'));
                });
        }

        $('#SubmitNewTenantWizard').on('click', function (e) {
            saveTenant();
        });
        // end save function

        //$('#UserAdminEmailAddress').keypress(function () {
        //    $('#UserAdminUserName').val($('#UserAdminEmailAddress').val());
        //});

        $('#UserAdminEmailAddress').on('change', function (e) {
            $('#UserAdminUserName').val($('#UserAdminEmailAddress').val());
        });
        $('#UserAdminEmailAddress').on('keyup', function (e) {
            $('#UserAdminUserName').val($('#UserAdminEmailAddress').val());
        });

        var _CreateTenant_IsDonorPerpetualPay = $('#CreateTenant_IsDonorPerpetualPay');
        $('#CreateTenant_IsDonorPay').change(function () {
            if ($(this).is(':checked')) {
                _CreateTenant_IsDonorPerpetualPay.classList.remove('d-none');
            } else {
                _CreateTenant_IsDonorPerpetualPay.classList.add('d-none');
            }
        });
    });
})();
