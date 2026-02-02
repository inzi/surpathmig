(function ($) {
    app.modals.UserPurchaseModal = function () {
        let transactionId;

        var _puchaseService = abp.services.app.purchase;

        var _$UserPurchaseForm = $('form[name=UserPurchaseForm]');
        _$UserPurchaseForm.validate();

        var ApiLoginID;
        var PublicClientKey;
        var cardLastFour;
        var params;
        var isZeroAmountDue = false;
        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();

            params = _modalManager.getParams();
            //console.log('UserPurchaseModal params', params);
            //if (params.getprofile == true) {
            //    // get the user profile
            //}
            $('#firstNameOnCard').val(params.Name);
            $('#lastNameOnCard').val(params.Surname);
            $('#cardNameOnCard').val(params.Name + ' ' + params.Surname);

            var _UserProfileBackToMyAccountButton = modal.find('#UserProfileBackToMyAccountButton');

            _puchaseService.authorizeNetSettings().done(function (data) {
                //console.log(data);
                ApiLoginID = data.apiLoginID;
                PublicClientKey = data.publicClientKey;
                transactionId = data.transactionId;
            });

            _UserProfileBackToMyAccountButton.click(function (e) {
                e.preventDefault();
                abp.ajax({
                    url: abp.appPath + 'Account/BackToImpersonator',
                    success: function () {
                        if (!app.supportsTenancyNameInUrl) {
                            abp.multiTenancy.setTenantIdCookie(abp.session.impersonatorTenantId);
                        }
                    },
                });
            });
            //SnoozePurchaseNag
            var _SnoozePurchaseNag = modal.find('#SnoozePurchaseNag');
            _SnoozePurchaseNag.click(function (e) {
                e.preventDefault();
                purchaseHelperStatusController.snooze();
                _modalManager.close();
            })

            var amountDue = parseFloat($('#amount').val() || '0');
            if (isNaN(amountDue)) {
                amountDue = 0;
            }
            isZeroAmountDue = amountDue <= 0;

            if (isZeroAmountDue) {
                configureZeroAmountState();
            } else {
                resetPaymentState();
            }
        };

        function configureZeroAmountState() {
            $('#buttonMakePurchase').removeAttr('disabled');
            $('#buttonMakePurchase').removeClass('d-none');
            $('#noPaymentRequiredMessage').removeClass('d-none');
            $('#serviceListSection').addClass('d-none');
            $('#amountDueSection').addClass('d-none');
            $('#paymentDetailsSection').addClass('d-none');
            $('#differentBillingSection').addClass('d-none');
            $('#buttonMakePurchase').removeClass('btn-primary').addClass('btn-success');

            var $icon = $('#buttonMakePurchase').find('i.fa');
            $icon.removeClass('fa-plus').addClass('fa-check');

            var registerText = (app.localize && app.localize('Register')) || 'Register';
            $('#buttonMakePurchase').find('.action-text').text(registerText);
        }

        function resetPaymentState() {
            $('#buttonMakePurchase').removeAttr('disabled');
            $('#buttonMakePurchase').removeClass('d-none');
            $('#buttonMakePurchase').removeClass('btn-success').addClass('btn-primary');
            $('#noPaymentRequiredMessage').addClass('d-none');
            $('#serviceListSection').removeClass('d-none');
            $('#amountDueSection').removeClass('d-none');
            $('#paymentDetailsSection').removeClass('d-none');
            $('#differentBillingSection').removeClass('d-none');

            var $icon = $('#buttonMakePurchase').find('i.fa');
            $icon.removeClass('fa-check').addClass('fa-plus');

            var purchaseText = (app.localize && app.localize('MakePurchase')) || 'Make Purchase';
            $('#buttonMakePurchase').find('.action-text').text(purchaseText);
        }

        function preAuth(response) {
            //$('#paymentInfoFeedback').text(app.localize('PaymentSuccess'));
            var _authnetSubmit = _$UserPurchaseForm.serializeFormToObject();
            // sanitize the object
            _authnetSubmit.cardNumber = '';
            _authnetSubmit.expMonth = '';
            _authnetSubmit.expYear = '';
            _authnetSubmit.cardCode = '';
            _authnetSubmit.differentBillingAddress = $('#differentBillingAddressChk').is(':checked');

            _authnetSubmit.cardLastFour = $('#cardNumber').val().slice(-4);
            var _regform = params;
            _regform.AuthNetSubmit = _authnetSubmit;
            _regform.TransactionId = transactionId;
            _puchaseService.preAuth(_regform).done(function (data) {
                //debugger;
                if (data.transactionResponse.responseCode != 1) {
                    $.each(data.transactionResponse.errors, function (i) {
                        abp.notify.error(data.transactionResponse.errors[i].errorText);
                    });
                    abp.notify.error(app.localize('PaymentIssuePleasetryAgain'));
                }
                else {
                    if (data.transactionResponse.authCode.length > 0 && data.transactionResponse.transId.length > 0) {
                        $('#authCode').val(data.transactionResponse.authCode);
                        $('#transactionId').val(data.transactionResponse.transId);
                        paymentFormUpdate(data);
                    }
                }
            });
        }
        function paymentFormUpdate(data) {
            cardLastFour = $('#cardNumber').val().slice(-4);
            $('#cardNumber').val('');
            $('#expMonth').val('');
            $('#expYear').val('');
            $('#cardCode').val('');

            $('.surpath-nosubmit').val('');

            var _authnetSubmit = _$UserPurchaseForm.serializeFormToObject();
            _authnetSubmit.differentBillingAddress = $('#differentBillingAddressChk').is(':checked');
            _authnetSubmit.authnetResponse = data;
            _authnetSubmit.cardLastFour = cardLastFour;
            _modalManager.setResult(_authnetSubmit);

            _modalManager.close();
        }

        function sendPaymentDataToAnet() {
            var authData = {};

            authData.clientKey = PublicClientKey;
            authData.apiLoginID = ApiLoginID;

            var cardData = {};
            cardData.cardNumber = $('#cardNumber').val(); // document.getElementById("cardNumber").value;
            cardData.month = $('#expMonth').val(); //document.getElementById("expMonth").value;
            cardData.year = $('#expYear').val(); //document.getElementById("expYear").value;
            cardData.cardCode = $('#cardCode').val(); // document.getElementById("cardCode").value;
            cardData.zip = $('#billingZipCode').val(); // document.getElementById("zip").value;
            cardData.fullName = $('#cardNameOnCard').val(); // document.getElementById("fullName").value;

            var secureData = {};
            secureData.authData = authData;
            secureData.cardData = cardData;

            _modalManager.setBusy(true);

            Accept.dispatchData(secureData, opaqueHandler);

            function opaqueHandler(response) {
                if (response.messages.resultCode === "Error") {
                    abp.notify.error(app.localize('PaymentIssuePleasetryAgain'));
                    var i = 0;
                    $('#buttonMakePurchase').removeAttr('disabled');
                    $('#buttonMakePurchase').removeClass('d-none');
                    while (i < response.messages.message.length) {
                        abp.notify.error(response.messages.message[i].text);
                        i = i + 1;
                    }
                    _modalManager.setBusy(false);
                } else {
                    abp.notify.info(app.localize('PreAuthorizing'));
                    $('#dataDescriptor').val(response.opaqueData.dataDescriptor);
                    $('#dataValue').val(response.opaqueData.dataValue);
                    preAuth(response);
                }
            }
        }

        function completeWithoutPayment() {
            var zeroResponse = {
                messages: {
                    resultCode: "Ok",
                    message: [{ code: "I00001", text: "Successful." }]
                },
                transactionResponse: {
                    avsResultCode: 'Y',
                    cvvResultCode: 'M',
                    responseCode: 1,
                    authCode: '',
                    transId: ''
                }
            };

            paymentFormUpdate(zeroResponse);
        }

        $('#buttonMakePurchase').on('click', function (e) {
            e.preventDefault();
            $('#buttonMakePurchase').prop("disabled", true);

            if (isZeroAmountDue) {
                completeWithoutPayment();
                return;
            }

            $('#buttonMakePurchase').addClass('d-none');
            sendPaymentDataToAnet();
        });

        $('.card-name-input').on('keyup', function (e) {
            //console.log('cni change');
            $('#cardNameOnCard').val($('#firstNameOnCard').val() + ' ' + $('#lastNameOnCard').val());
        })

        $('#differentBillingAddressChk').on('change', function (e) {
            var $this = $(this);
            $('#differentBillingAddress').val($this.is(':checked'));
            if ($this.is(':checked')) {
                $('#billingAddressWrapper').removeClass('d-none');
            }
            else {
                $('#billingAddressWrapper').addClass('d-none');
            };
        });
    };
})(jQuery);
