var CurrentPage = (function () {
    jQuery.validator.addMethod(
        'customUsername',
        function (value, element) {
            if (value === $('input[name="EmailAddress"]').val()) {
                return true;
            }

            return !$.validator.methods.email.apply(this, arguments);
        },
        abp.localization.localize('RegisterFormUserNameInvalidMessage')
    );

    $.validator.addMethod(
        "validDate",
        function (value, element) {
            return this.optional(element) || !/Invalid|NaN/.test(new Date(value).toString());
        },
        "A valid date is required for date of birth. Example: 02/14/2001"
    );

    $.validator.addMethod(
        "emailAvailable",
        function (value, element, callback) {
            if (value === $('input[name="EmailAddress"]').val()) {
                var isEmailRegistered = $(element).hasClass('surpath-email-registered');
                return !isEmailRegistered;
            }
            return true;
        },
        "This email address is already in use. Please use a different email address or login."
    );

    var _passwordComplexityHelper = new app.PasswordComplexityHelper();

    // parseInt(moment().format("YYYY"), 15),
    // moment().subtract(1, 'year').format('YYYY'),

    var handleRegister = function () {
        var _complianceService = abp.services.app.surpathCompliance;
        var validationErrorClass = 'surpath-validation-error';

        function normalizeGuid(value) {
            if (!value) {
                return null;
            }

            var trimmed = value.toString().trim();
            return trimmed.length ? trimmed : null;
        }

        function buildValidationPayload() {
            return {
                tenantId: abp.session ? abp.session.tenantId : null,
                emailAddress: ($('input[name="EmailAddress"]').val() || '').trim(),
                userName: ($('input[name="UserName"]').val() || '').trim(),
                tenantDepartmentId: normalizeGuid($('#TenantDepartmentId').val() || $('#confirmTenantDepartmentId').val()),
                cohortId: normalizeGuid($('#CohortId').val() || $('#confirmCohortId').val())
            };
        }

        function setFieldError($element, message) {
            if (!$element || $element.length === 0) {
                return;
            }

            var $group = $element.closest('.mb-5');
            $group.find('.' + validationErrorClass).remove();

            if (message) {
                $element.addClass('is-invalid');
                $('<div/>', {
                    'class': validationErrorClass + ' text-danger mt-2 small',
                    text: message
                }).appendTo($group);
            } else {
                $element.removeClass('is-invalid');
            }
        }

        function applyValidationFeedback(result) {
            result = result || {};

            setFieldError($('input[name="EmailAddress"]'), result.emailError || (result.emailAvailable === false ? app.localize('EmailAlreadyRegistered') : null));
            setFieldError($('input[name="UserName"]'), result.usernameError || (result.usernameAvailable === false ? app.localize('UsernameAlreadyRegistered') : null));

            var departmentMessage = result.departmentError || (result.departmentValid === false ? app.localize('TenantDepartmentSelectionInvalid') : null);
            setFieldError($('#TenantDepartmentName'), departmentMessage);
            setFieldError($('#TenantDepartmentConfirmName'), departmentMessage);

            var cohortMessage = result.cohortError || (result.cohortValid === false ? app.localize('CohortSelectionInvalid') : null);
            setFieldError($('#CohortName'), cohortMessage);
            setFieldError($('#CohortConfirmName'), cohortMessage);

            if (result.errors && result.errors.length && result.isValid === false) {
                abp.message.warn(result.errors.join('<br/>'));
            }
        }

        function validateRegistrationBeforeAction() {
            var deferred = $.Deferred();
            var payload = buildValidationPayload();

            _complianceService.validateRegistration(payload).done(function (response) {
                applyValidationFeedback(response);
                deferred.resolve(response);
            }).fail(function () {
                abp.notify.error('Unable to validate registration details. Please try again.');
                deferred.reject();
            });

            return deferred.promise();
        }

        var _modalManager;
        var _$RegisterForm = null;

        //var _CohortConfirmLookupTableModal = new app.ModalManager({
        //    viewUrl: abp.appPath + 'App/Compliance/CohortConfirmLookupTableModal',
        //    scriptUrl:
        //        abp.appPath + 'view-resources/Areas/App/Views/Compliance/_CohortConfirmLookupTableModal.js',
        //    modalClass: 'CohortConfirmLookupTableModal',
        //});

        var _TenantDepartmentLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Compliance/TenantDepartmentLookupTableRegModal',
            scriptUrl:
                abp.appPath + 'view-resources/Areas/App/Views/Compliance/_TenantDepartmentLookupTableRegModal.js',
            modalClass: 'TenantDepartmentLookupTableRegModal',
        });

        var _CohortLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Compliance/CohortLookupTableRegModal',
            scriptUrl:
                abp.appPath + 'view-resources/Areas/App/Views/Compliance/_CohortLookupTableRegModal.js',
            modalClass: 'CohortLookupTableRegModal',
        });

        var registrationPricingConfig = window.registrationPricing || {};
        var tenantServiceIds = Array.isArray(registrationPricingConfig.tenantServiceIds)
            ? registrationPricingConfig.tenantServiceIds.slice()
            : [];
        var cachedPricingState = {
            key: null,
            amountDue: parseFloat(registrationPricingConfig.amountDue || '0'),
            requiresPayment: false,
            allInvoiced: false
        };
        if (isNaN(cachedPricingState.amountDue)) {
            cachedPricingState.amountDue = 0;
        }
        var registerNowText = registrationPricingConfig.registerNowText || 'Register Now';
        var $registerSubmitButton = $('#register-submit-btn');
        var $registerCheckoutButton = $('#register-checkout-btn');
        var submitDefaultText = $registerSubmitButton.data('default-text') || $registerSubmitButton.text();
        var checkoutDefaultText = $registerCheckoutButton.data('default-text') || $registerCheckoutButton.text();

        function getTenantServiceIds() {
            if (tenantServiceIds.length) {
                return tenantServiceIds;
            }

            return $('input[name="AuthNetSubmit.TenantSurpathServiceIds[]"]').map(function () {
                return $(this).val();
            }).get();
        }

        function buildPricingKey(deptId, cohortId) {
            return (deptId || '') + ':' + (cohortId || '');
        }

        function resetPaymentFields() {
            $('input[name="AuthNetSubmit.dataValue"]').val('');
            $('input[name="AuthNetSubmit.dataDescriptor"]').val('');
            $('input[name="AuthNetSubmit.CardNameOnCard"]').val('');
            $('input[name="AuthNetSubmit.amount"]').val('');
            $('input[name="AuthNetSubmit.CardZipCode"]').val('');
            $('input[name="AuthNetSubmit.DifferentBillingAddress"]').val('');
            $('input[name="AuthNetSubmit.FirstNameOnCard"]').val('');
            $('input[name="AuthNetSubmit.LastNameOnCard"]').val('');
            $('input[name="AuthNetSubmit.BillingAddress"]').val('');
            $('input[name="AuthNetSubmit.BillingCity"]').val('');
            $('input[name="AuthNetSubmit.BillingState"]').val('');
            $('input[name="AuthNetSubmit.BillingZipCode"]').val('');
            $('input[name="AuthNetSubmit.CardLastFour"]').val('');
            $('input[name="AuthNetCaptureResultDto.AuthCode"]').val('');
            $('input[name="AuthNetCaptureResultDto.TransactionId"]').val('');
        }

        function hideRegistrationActions() {
            resetPaymentFields();
            $registerCheckoutButton.addClass('d-none').prop('disabled', true).text(checkoutDefaultText);
            $registerSubmitButton.removeClass('d-none').prop('disabled', true).text(submitDefaultText);
        }

        function zeroOutPaymentFields() {
            $('input[name="AuthNetSubmit.dataValue"]').val('na');
            $('input[name="AuthNetSubmit.dataDescriptor"]').val('na');
            $('input[name="AuthNetSubmit.CardNameOnCard"]').val('');
            $('input[name="AuthNetSubmit.amount"]').val('0.00');
            $('input[name="AuthNetSubmit.CardZipCode"]').val('00000');
            $('input[name="AuthNetSubmit.DifferentBillingAddress"]').val('false');
            $('input[name="AuthNetSubmit.FirstNameOnCard"]').val('');
            $('input[name="AuthNetSubmit.LastNameOnCard"]').val('');
            $('input[name="AuthNetSubmit.BillingAddress"]').val('');
            $('input[name="AuthNetSubmit.BillingCity"]').val('');
            $('input[name="AuthNetSubmit.BillingState"]').val('');
            $('input[name="AuthNetSubmit.BillingZipCode"]').val('');
            $('input[name="AuthNetSubmit.CardLastFour"]').val('');
            $('input[name="AuthNetCaptureResultDto.AuthCode"]').val('');
            $('input[name="AuthNetCaptureResultDto.TransactionId"]').val('');
        }

        function applyPricingState(pricing) {
            var amountDue = parseFloat(pricing && pricing.amountDue != null ? pricing.amountDue : 0);
            if (isNaN(amountDue)) {
                amountDue = 0;
            }

            var requiresPayment = pricing && pricing.requiresPayment === true;

            $('input[name="AuthNetSubmit.amount"]').val(amountDue.toFixed(2));

            if (window.isDonorPay === false) {
                requiresPayment = false;
                amountDue = 0;
            }

            if (!requiresPayment) {
                zeroOutPaymentFields();
                $registerCheckoutButton.addClass('d-none').prop('disabled', true).text(checkoutDefaultText);
                $registerSubmitButton.removeClass('d-none').prop('disabled', false).text(registerNowText);
            } else {
                resetPaymentFields();
                $registerSubmitButton.addClass('d-none').prop('disabled', true).text(submitDefaultText);
                $registerCheckoutButton.removeClass('d-none').prop('disabled', false).text(checkoutDefaultText);
                donorPayFn();
            }
        }

        function resolveRegistrationPricing(deptIdInput, cohortIdInput) {
            var deptId = normalizeGuid(deptIdInput);
            var cohortId = normalizeGuid(cohortIdInput);
            var key = buildPricingKey(deptId, cohortId);

            if (!deptId) {
                return $.Deferred().reject('missing-department').promise();
            }

            if (cachedPricingState.key === key) {
                return $.Deferred().resolve($.extend({}, cachedPricingState)).promise();
            }

            if (!registrationPricingConfig.pricingUrl) {
                return $.Deferred().reject('missing-url').promise();
            }

            var ids = getTenantServiceIds().filter(function (id) { return !!id; });
            ids = Array.from(new Set(ids));

            return abp.ajax({
                url: registrationPricingConfig.pricingUrl,
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                data: JSON.stringify({
                    tenantDepartmentId: deptId,
                    cohortId: cohortId,
                    tenantSurpathServiceIds: ids
                })
            }).then(function (response) {
                var amount = parseFloat(response && response.amountDue != null ? response.amountDue : 0);
                if (isNaN(amount)) {
                    amount = 0;
                }

                cachedPricingState = {
                    key: key,
                    amountDue: amount,
                    requiresPayment: response && response.requiresPayment === true,
                    allInvoiced: response && response.allInvoiced === true
                };

                return $.extend({}, cachedPricingState);
            });
        }

        //var _TenantDepartmentConfirmLookupTableModal = new app.ModalManager({
        //    viewUrl: abp.appPath + 'App/Compliance/TenantDepartmentConfirmLookupTableModal',
        //    scriptUrl:
        //        abp.appPath + 'view-resources/Areas/App/Views/Compliance/_TenantDepartmentConfirmLookupTableModal.js',
        //    modalClass: 'TenantDepartmentConfirmLookupTableModal',
        //});

        _$RegisterForm = $('form[name=register_form]');

        $('#OpenTenantDepartmentLookupTableButton').click(function () {
            console.log('OpenTenantDepartmentLookupTableButton');
            //var recordState = _$RegisterForm.serializeFormToObject();

            _TenantDepartmentLookupTableModal.open(
                {},
                function (data) {
                    //console.log('back from data call');
                    //console.log(data);
                    _$RegisterForm.find('input[name=TenantDepartmentName]').val(data.displayName);
                    _$RegisterForm.find('input[name=TenantDepartmentId]').val(data.id);
                    _$RegisterForm.find('textarea[name=TenantDepartmentInstructions]').html(data.displayInstructions);
                    stepperButtons();
                }
            );
        });

        $('#OpenTenantDepartmentConfirmLookupTableButton').click(function () {
            //var recordState = _$RegisterForm.serializeFormToObject();

            _TenantDepartmentLookupTableModal.open(
                { confirm: 'true' },
                function (data) {
                    //console.log('back from data call');
                    //console.log(data);
                    _$RegisterForm.find('input[name=TenantDepartmentConfirmName]').val(data.displayName);
                    _$RegisterForm.find('input[name=confirmTenantDepartmentId]').val(data.id);
                    _$RegisterForm.find('textarea[name=TenantDepartmentInstructions]').html(data.displayInstructions);
                    stepperButtons();
                }
            );
        });

        $('#OpenCohortLookupTableButton').click(function () {
            console.log('OpenCohortLookupTableButton');
            //var recordState = _$RegisterForm.serializeFormToObject();
            var _tenantDepartmentId = $('#TenantDepartmentId').val();

            _CohortLookupTableModal.open(
                { tenantDepartmentId: _tenantDepartmentId },
                function (data) {
                    //console.log(data);
                    _$RegisterForm.find('input[name=CohortName]').val(data.displayName);
                    _$RegisterForm.find('input[name=CohortId]').val(data.id);
                    _$RegisterForm.find('textarea[name=CohortInstructions]').html(data.displayInstructions);
                    stepperButtons();
                }
            );
        });

        $('#OpenCohortConfirmLookupTableButton').click(function () {
            //var recordState = _$RegisterForm.serializeFormToObject();
            var _tenantDepartmentId = $('#TenantDepartmentId').val();

            _CohortLookupTableModal.open(
                { tenantDepartmentId: _tenantDepartmentId, confirm: true },
                function (data) {
                    _$RegisterForm.find('input[name=CohortConfirmName]').val(data.displayName);
                    _$RegisterForm.find('input[name=confirmCohortId]').val(data.id);
                    stepperButtons();
                }
            );
        });

        $('.login-form').closest('.bg-body').removeClass('w-lg-500px'); //.addClass('w-lg-700px');
        $(".register-form").find('.date-picker').daterangepicker({
            singleDatePicker: true,
            showDropdowns: true,
            //minYear: parseInt(moment().subtract(100, 'year').format('YYYY')),
            //maxYear: parseInt(moment().subtract(16, 'year').format('YYYY')),
            //startDate: moment().subtract(1, 'year').format('MM/DD/YYYY'),
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        });

        Inputmask({
            "mask": "99/99/9999"
        }).mask("#DateOfBirth");

        $('.register-form').validate({
            rules: {
                PasswordRepeat: {
                    equalTo: '#RegisterPassword',
                },
                UserName: {
                    required: true,
                    customUsername: true,
                },
                EmailAddress: {
                    required: true,
                    email: true,
                    emailAvailable: true,
                },
                DateOfBirth: {
                    required: true,
                    validDate: true
                },
                PhoneNumber: {
                    required: true,
                    validPhoneNumber: true
                },
                Address: {
                    required: true
                },
                City: {
                    required: true
                },
                State: {
                    required: true
                },
                Zip: {
                    required: true
                }
            },

            submitHandler: function (form) {
                function setCaptchaToken(callback) {
                    callback = callback || function () { };
                    if (!abp.setting.getBoolean('App.UserManagement.UseCaptchaOnRegistration')) {
                        callback();
                    } else {
                        grecaptcha.reExecute(function (token) {
                            $('#recaptchaResponse').val(token);
                            callback();
                        });
                    }
                }

                setCaptchaToken(function () {
                    form.submit();
                });
            },
        });

        $('.register-form input').keypress(function (e) {
            if (e.which === 13) {
                if ($('.register-form').valid()) {
                    $('.register-form').submit();
                }
                return false;
            }
        });

        $('input[name=Password]').pwstrength({
            i18n: {
                t: function (key) {
                    return app.localize(key);
                },
            },
        });

        _passwordComplexityHelper.setPasswordComplexityRules(
            $('input[name=Password], input[name=PasswordRepeat]'),
            window.passwordComplexitySetting
        );

        $('input[name="EmailAddress"]').on('change', function () {
            console.log('email changed');
            var emailValue = $(this).val();
            var isValidEmail = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/.test(emailValue);
            if (isValidEmail) {
                ensureEmailAvailability(true);
            } else {
                $('input[name="EmailAddress"]').removeClass('surpath-email-registered');
                $('input[name="EmailAddress"]').data('validated-email', null);
                $('input[name="EmailAddress"]').data('validated-email-result', false);
            }
        });

        let openPaymentModal = function () {
            var _regform = _$RegisterForm.serializeFormToObject();
            var _PurchaseModal = new app.ModalManager({
                viewUrl: abp.appPath + 'Payment/PurchaseModal',
                scriptUrl: abp.appPath + 'view-resources/Views/Payment/_PurchaseModal.js', // /view-resources/Views/Payment/Upgrade.js
                modalClass: 'UserPurchaseModal',
                params: _regform
            });

            function enableCheckoutButton() {
                $('#register-checkout-btn').prop('disabled', false);
            }

            // Get Dept ID and Cohort ID if they exist
            var _tenantDepartmentId = $('#TenantDepartmentId').val();
            var _cohortId = $('#CohortId').val();
            //debugger;

            _PurchaseModal.open({ deptId: _tenantDepartmentId, cohortId: _cohortId }, function (data) {
                var response = data.authnetResponse;
                if (!(response.transactionResponse.avsResultCode == 'Y' || response.transactionResponse.avsResultCode == 'P')) {
                    abp.notify.error(app.localize('PaymentIssuePleasetryAgain'));
                    abp.notify.error(app.localize('PaymentIssueAVSFailed'));
                    enableCheckoutButton();
                } else if (response.transactionResponse.cvvResultCode == 'N') {
                    abp.notify.error(app.localize('PaymentIssuePleasetryAgain'));
                    abp.notify.error(app.localize('PaymentIssueCVVFailed'));
                    enableCheckoutButton();
                } else if (response.messages.resultCode === "Error") {
                    abp.notify.error(app.localize('PaymentIssuePleasetryAgain'));
                    var i = 0;
                    while (i < response.messages.message.length) {
                        abp.notify.error(response.messages.message[i].text);
                        i = i + 1;
                    }
                    enableCheckoutButton();
                } else {
                    abp.notify.info(app.localize('PaymentSuccess'));
                    $('#paymentInfoFeedback').text(app.localize('PaymentSuccess'));

                    $('input[name="AuthNetSubmit.dataValue"]').val(data.dataValue);
                    $('input[name="AuthNetSubmit.dataDescriptor"]').val(data.dataDescriptor);
                    $('input[name="AuthNetSubmit.CardNameOnCard"]').val(data.cardNameOnCard);
                    $('input[name="AuthNetSubmit.amount"]').val(data.amount);
                    /*                        $('input[name="AuthNetSubmit.CardZipCode"]').val(data.cardZipCode);*/
                    $('input[name="AuthNetSubmit.DifferentBillingAddress"]').val(data.differentBillingAddress);
                    $('input[name="AuthNetSubmit.FirstNameOnCard"]').val(data.firstNameOnCard);
                    $('input[name="AuthNetSubmit.LastNameOnCard"]').val(data.lastNameOnCard);
                    $('input[name="AuthNetSubmit.BillingAddress"]').val(data.billingAddress);
                    $('input[name="AuthNetSubmit.BillingCity"]').val(data.billingCity);
                    $('input[name="AuthNetSubmit.BillingState"]').val(data.billingState);
                    $('input[name="AuthNetSubmit.BillingZipCode"]').val(data.billingZipCode);
                    $('input[name="AuthNetSubmit.CardLastFour"]').val(data.cardLastFour);

                    $('input[name="AuthNetCaptureResultDto.AuthCode"]').val(data.authCode);
                    $('input[name="AuthNetCaptureResultDto.TransactionId"]').val(data.transactionId);

                    $('#register-submit-btn').removeClass('d-none');
                    $('#register-submit-btn').prop('disabled', false);
                    $('#register-checkout-btn').addClass('d-none');
                }
            });
        }

        var registerCheckoutBtnClick = 0

        let donorPayFn = function () {
            $registerCheckoutButton.removeClass('d-none');
            $registerCheckoutButton.prop('disabled', false).text(checkoutDefaultText);

            $registerSubmitButton.addClass('d-none');

            $registerCheckoutButton.off('click').on('click', function (e) {
                e.preventDefault();

                var $button = $(this);
                if ($button.data('validating')) {
                    return;
                }

                $button.data('validating', true);
                $button.prop('disabled', true);

                validateRegistrationBeforeAction().done(function (response) {
                    if (response && response.isValid) {
                        registerCheckoutBtnClick++;
                        $('input[name="RegisterCheckoutBtnClick"]').val(registerCheckoutBtnClick);
                        openPaymentModal();
                    } else {
                        $button.prop('disabled', false);
                    }
                }).fail(function () {
                    $button.prop('disabled', false);
                }).always(function () {
                    $button.data('validating', false);
                });
            });
        }

        $('#register-submit-btn').off('click').on('click', function (e) {
            if ($(this).data('skip-validation') === true) {
                $(this).data('skip-validation', false);
                return true;
            }

            e.preventDefault();

            var $button = $(this);
            if ($button.data('validating')) {
                return;
            }

            $button.data('validating', true);
            $button.prop('disabled', true);

            validateRegistrationBeforeAction().done(function (response) {
                if (response && response.isValid) {
                    $button.data('skip-validation', true);
                    $button.closest('form')[0].submit();
                } else {
                    $button.prop('disabled', false);
                }
            }).fail(function () {
                $button.prop('disabled', false);
            }).always(function () {
                $button.data('validating', false);
            });
        });

        // STEPPER
        // Stepper lement

        var _stepperName = 'RegistrationWizard';
        var element = document.querySelector("#" + _stepperName);

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

        function ensureEmailAvailability(forceRefresh) {
            var $emailInput = $('input[name="EmailAddress"]');
            var value = ($emailInput.val() || '').trim();
            var deferred = $.Deferred();

            if (value.length === 0) {
                $emailInput.removeClass('surpath-email-registered');
                $emailInput.data('validated-email', null);
                $emailInput.data('validated-email-result', true);
                deferred.resolve(true);
                return deferred.promise();
            }

            var normalizedEmail = value.toLowerCase();
            var cachedEmail = $emailInput.data('validated-email');
            var cachedResult = $emailInput.data('validated-email-result');

            if (!forceRefresh && cachedEmail === normalizedEmail && cachedResult !== undefined) {
                var available = cachedResult === true;
                if (available) {
                    $emailInput.removeClass('surpath-email-registered');
                } else {
                    $emailInput.addClass('surpath-email-registered');
                }
                deferred.resolve(available);
                return deferred.promise();
            }

            if ($emailInput.data('email-checking')) {
                var poll = function () {
                    if (!$emailInput.data('email-checking')) {
                        ensureEmailAvailability(false).done(function (result) {
                            deferred.resolve(result);
                        });
                    } else {
                        setTimeout(poll, 150);
                    }
                };
                poll();
                return deferred.promise();
            }

            $emailInput.data('email-checking', true);
            abp.services.app.surpathCompliance.emailAvailable({
                emailAddress: value
            }).done(function (result) {
                var available = result === true;
                $emailInput.data('validated-email', normalizedEmail);
                $emailInput.data('validated-email-result', available);

                if (available) {
                    $emailInput.removeClass('surpath-email-registered');
                } else {
                    $emailInput.addClass('surpath-email-registered');
                }

                deferred.resolve(available);
            }).fail(function () {
                deferred.resolve(false);
            }).always(function () {
                $emailInput.data('email-checking', false);
            });

            return deferred.promise();
        }

        var stepperButtons = function () {
            var _step = stepper.getCurrentStepIndex();
            //console.log('stepperbuttons');
            //$('#stepper_next_button').removeAttr("disabled");

            if (_step == 1) {
                console.log('step 1');
                $('#stepper_next_button').removeClass("d-none");
                $('#stepper_next_button').attr('disabled', 'disabled');

                ensureEmailAvailability(false).done(function (emailAvailable) {
                    var inputsValid = $(".step1Wizard input").valid();

                    if (inputsValid && emailAvailable) {
                        setNextTooltip(abp.localization.localize('Continue'));
                        $('#stepper_next_button').removeAttr("disabled");
                    } else if (!emailAvailable) {
                        setNextTooltip(app.localize('EmailAlreadyRegistered'));
                        $('#stepper_next_button').attr('disabled', 'disabled');
                    } else {
                        $('#stepper_next_button').attr('disabled', 'disabled');
                    }
                });
            }
            if (_step == 2) {
                console.log('step 2');
                $('#stepper_next_button').removeClass("d-none");

                if ($(".step2Wizard input").valid()) {
                    //if ($("#Name").val().length > 0 && $("#TenancyName").val().length > 0 && $("#ClientCode").val().length > 0) {
                    setNextTooltip(abp.localization.localize('Continue'));
                    $('#stepper_next_button').removeAttr("disabled");
                }
                else {
                    $('#stepper_next_button').attr('disabled', 'disabled');
                }
            }
            if (_step == 3) {
                console.log('step 3');
                $('#stepper_next_button').removeClass("d-none");

                if ($(".step3Wizard input").valid()) {
                    //if ($("#Name").val().length > 0 && $("#TenancyName").val().length > 0 && $("#ClientCode").val().length > 0) {
                    setNextTooltip(abp.localization.localize('Continue'));
                    $('#stepper_next_button').removeAttr("disabled");
                    //$('#RegistrationWizardSubmit').removeAttr("disabled");
                }
                else {
                    $('#stepper_next_button').attr('disabled', 'disabled');
                }
            }
            if (_step == 4) {
                console.log('step 4');

                $('#stepper_next_button').addClass("d-none");

                var departmentMatches = $("#TenantDepartmentName").val().length > 0 &&
                    $("#TenantDepartmentName").val() === $("#TenantDepartmentConfirmName").val();

                if (departmentMatches) {
                    $('.cohort-select-inputs').removeClass('d-none');

                    var cohortMatches = $("#CohortName").val().length > 0 &&
                        $("#CohortName").val() === $("#CohortConfirmName").val();

                    if (cohortMatches) {
                        hideRegistrationActions();

                        var deptIdValue = $('#TenantDepartmentId').val() || $('#confirmTenantDepartmentId').val();
                        var cohortIdValue = $('#CohortId').val() || $('#confirmCohortId').val();

                        resolveRegistrationPricing(deptIdValue, cohortIdValue).done(function (pricingState) {
                            applyPricingState(pricingState || {});
                        }).fail(function () {
                            cachedPricingState.key = null;
                            hideRegistrationActions();
                            abp.notify.error('Unable to calculate registration pricing. Please try again.');
                        });
                    } else {
                        hideRegistrationActions();
                        if ($("#CohortConfirmName").val().length > 0) {
                            abp.message.error(abp.localization.localize('CohortsDoNotMatch'));
                        }
                    }
                } else {
                    hideRegistrationActions();
                    if ($("#TenantDepartmentConfirmName").val().length > 0) {
                        abp.message.error(abp.localization.localize('DepartmentsDoNotMatch'));
                    }
                    $('.cohort-select-inputs').addClass('d-none');
                }
            }
        };

        //$(".step1NewTenant").on("change", stepperButtons);
        //$("#Record_InstructionsConfirmed").on("click", stepperButtons);

        // focus on the first element so any action will fire the blue
        $('.stepper-first-element').focus();

        _$pidsDiv = $('#register_pids_content');

        function pidInputFactory(index, data) {
            // create a div for the input

            // <input type="text" id="UserPidViewModels_0__UserPid_Pid" name="UserPidViewModels[0].UserPid.Pid" value="bingo" aria-describedby="UserPidViewModels_0__UserPid_Pid-error" aria-invalid="false">
            // <input type="text" id="UserPidViewModels_0__UserPid_PidType_Description" name="UserPidViewModels[0].UserPid.PidType.Description" value="" aria-describedby="UserPidViewModels_0__UserPid_PidType_Description-error" aria-invalid="false">

            // <input id="UserPidViewModels_0__UserPid_PidTypeId" type="hidden" name="UserPidViewModels[0].UserPid.PidTypeId">

            // <input id="UserPidViewModels_0__UserPid_Pid" type="text" name="UserPidViewModels[0].UserPid.Pid" class="form-control form-control-solid h-auto py-7 px-6 rounded-lg font-size-h6 surpath_empty_pid surpath-inputmask surscan_inputmask_ssn" value="" placeholder="Social Security Number" required="" inputmode="text" aria-describedby="UserPidViewModels_0__UserPid_Pid-error" aria-invalid="true">
            // var _indexedId = 'UserPidViewModels_' + index + '__';

            var _wrapper = $('<div/>', {
                class: 'mb-5'
            });
            var _label = $('<label>', {
                class: 'form-label',
                text: data.pidType.name
            });
            var _pref_id = 'UserPidViewModels_' + index + '__UserPid_'
            var _pref_name = 'UserPidViewModels[' + index + '].UserPid.';

            var _pidMaskClass = '';
            var _required = false;
            if (typeof data.pidType.pidInputMask != 'undefined' && data.pidType.pidInputMask != '') {
                _pidMaskClass = data.pidType.pidInputMask;
                _required = data.pidType.required;
            }
            var _input = $('<input/>', {
                id: _pref_id + 'Pid',
                type: 'text',
                name: _pref_name + 'Pid',
                class: 'form-control form-control-solid h-auto py-7 px-6 rounded-lg font-size-h6 surpath_empty_pid surpath-inputmask ' + _pidMaskClass,
                value: '',
                placeholder: data.pidType.description
                //,data-inputmask: data.pidInputMask
            })
            if (_required == true) {
                $(_input).prop('required', true);
            }
            var _hidden = $('<input/>', {
                id: _pref_id + 'PidTypeId',
                type: 'hidden',
                name: _pref_name + 'PidTypeId',
                value: data.pidType.id
            })

            //if (data.pidType.name.toLowerCase() == 'ssn') {
            //    _input.prop('required', true);
            //}

            $(_wrapper).append(_label).append(_input);
            $(_wrapper).append(_hidden);
            _$pidsDiv.append(_wrapper);

            $(_input).on("blur", function (e) {
                if ($("#" + _stepperName).validate().element(e.target)) {
                    stepperButtons();
                }
            });

            //$('#register-submit-btn').on('click', function () {
            //    debugger;
            //    var reg = _$RegisterForm.serializeFormToObject();
            //});
            //console.log(data);
            //console.log('gen');
            return;
        }

        function setMasking() {
            // Phone
            Inputmask({
                "mask": "(999) 999-9999"
            }).mask(".surscan_inputmask_phone");

            Inputmask({
                "mask": "999-99-9999"
            }).mask(".surscan_inputmask_ssn");

            //console.log('inputmask set');
        }

        // pid list
        var _userPidsService = abp.services.app.userPids;
        //var _surpathComplianceAppService = abp.services.app.surpathComplianceAppService;
        var _pids;
        function getPids() {
            // get empty pid set
            // create inputs and add to form
            // if masked - set input style, but this eventually needs to populate with bad data and revealing it should be a specific, permissioned, call
            _userPidsService.getEmptyPidsList().done(function (result) {
                $.each(result, pidInputFactory);
            }).promise().done(function () {
                setMasking();
            });
        }
        getPids();

        //var _pidFunctionChain = [getPids, setMasking];
        //$.each(_pidFunctionChain, function (i) { _pidFunctionChain[i]() });

        //getPids();

        //$("#NewTenantWizardStepperForm input").on("blur", stepperButtons);
        $("#" + _stepperName + " input").on("blur", function (e) {
            //if ($(e.target).valid() == true) {
            //    stepperButtons();
            //}
            if ($("#" + _stepperName).validate().element(e.target)) {
                stepperButtons();
            }
        });

        //app.emailtousername();
        //$('.surpath-emailtousername').on('change', function (e) {
        //    $('.surpath-usernamefromemail').val($('.surpath-emailtousername').val());
        //});
        //$('.surpath-emailtousername').on('keyup', function (e) {
        //    $('.surpath-usernamefromemail').val($('.surpath-emailtousername').val());
        //});
        $('#surpathRegWizardButtonWrapper').on('mouseenter', function () {
            stepperButtons();
        })
    };

    return {
        init: function () {
            console.log('init', isDonorPay);
            //$('#accountLayoutBodyWrapper').addClass('w-lg-800px');
            //$('#accountLayoutBodyWrapper').removeClass('w-lg-500px');
            handleRegister();
        },
    };
})();
