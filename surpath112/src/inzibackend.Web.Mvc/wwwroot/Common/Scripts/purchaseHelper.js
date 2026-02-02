var purchaseHelperStatusController = (function () {
    // var _userid = app.session
    var _moment;
    var purchaseNagSnoozeTimeStorageKey = 'purchasenagsnooze';

    function writeToStorage() {
        if (localStorage) {
            localStorage.setItem(purchaseNagSnoozeTimeStorageKey, _moment);
        } else {
            abp.utils.setCookieValue(purchaseNagSnoozeTimeStorageKey, _moment);
        }
    }
    function readFromStorage() {
        if (localStorage) {
            _moment = localStorage.getItem(purchaseNagSnoozeTimeStorageKey);
        } else {
            _moment = abp.utils.getCookieValue(purchaseNagSnoozeTimeStorageKey);
        }
    }

    return {
        snoozed: function () {
            readFromStorage();
            if (_moment == undefined) return false;
            var _now = moment(new Date());
            var duration = moment.duration(_now.diff(_moment));
            var minutes = duration.asMinutes();
            return minutes < 1;
        },
        snooze: function () {
            //debugger;

            if (abp.auth.hasPermission('Pages.Tenant.Dashboard')) {
                _moment = moment(new Date());
                writeToStorage();
            }

        },
        init: function (options) {
            // debugger;
            if (app.session.user.isCohortUser == false && app.session.user.isAlwaysDonor == false) return;

            if (this.snoozed() == true) return;
            var _PurchaseModal = new app.ModalManager({
                viewUrl: abp.appPath + 'Payment/PurchaseModal',
                scriptUrl: abp.appPath + 'view-resources/Views/Payment/_PurchaseModal.js', // /view-resources/Views/Payment/Upgrade.js
                modalClass: 'UserPurchaseModal',
                params: {}
            });

            _PurchaseModal.onBeforeClose(function () {
                abp.services.app.purchase.donorCurrent(options.userid)
                    .done(function (data) {
                        if (purchaseHelperStatusController.snoozed() == true) return;

                        if (data == true) {
                            app.session.user.isPaid = true;
                            return;
                        };
                        _PurchaseModal.reopen();

                        //console.log(info);
                        //debugger;
                    });
            });

            abp.services.app.purchase.donorCurrent(options.userid)
                .done(function (data) {
                    if (data == true) return;
                    //console.log(data);
                    //_PurchaseModal.open();
                    //console.log(_PurchaseModal);
                    _PurchaseModal.setParams({ Name: app.session.user.name, Surname: app.session.user.surname, getprofile: true });
                    _PurchaseModal.open({}, function (data) {
                        var response = data.authnetResponse;
                        if (response.messages.resultCode === "Error") {
                            //abp.notify.error(app.localize('PaymentIssuePleasetryAgain'));
                            var i = 0;
                            while (i < response.messages.message.length) {
                                abp.notify.error(response.messages.message[i].text);
                                i = i + 1;
                            }
                        } else {

                            abp.services.app.purchase.doPurchaseFromHelper(data)
                                .done(function (data) {
                                    if (data == true) {
                                        //abp.notify.info(app.localize('PaymentSuccess'));
                                        abp.services.app.session.getCurrentLoginInformations({ async: false }).done(function (result) {
                                            app.session = result;
                                        });
                                        _modalManager.close();
                                        abp.event.trigger('app.userPaymentSuccess');
                                    }
                                    else {
                                        abp.notify.error(app.localize('SavePaymentIssue'));
                                    }
                                });

                            //_userBuyService.userBuy(_authnetSubmit)
                            //    .done(function (data) {
                            //        debugger;
                            //        if (data == true) {
                            //            abp.notify.info(app.localize('PaymentSuccess'));
                            //            _modalManager.close();
                            //            abp.event.trigger('app.userPaymentSuccess');
                            //        }
                            //        else {
                            //            abp.notify.info(app.localize('PaymentIssuePleasetryAgain'));
                            //        }
                            //    })
                            //    .always(function () {
                            //        _modalManager.setBusy(false);
                            //    });
                            ////_$UserPurchaseForm.submit();

                            ////abp.notify.info(app.localize('PaymentSuccess'));
                            //$('#paymentInfoFeedback').text(app.localize('PaymentSuccess'));

                            //// $('AuthNetSubmit.dataValue').val(data.dataValue);
                            //$('input[name="AuthNetSubmit.dataValue"]').val(data.dataValue);
                            //$('input[name="AuthNetSubmit.dataDescriptor"]').val(data.dataDescriptor);
                            //$('input[name="AuthNetSubmit.CardNameOnCard"]').val(data.cardNameOnCard);
                            //$('input[name="AuthNetSubmit.amount"]').val(data.amount);
                            //$('input[name="AuthNetSubmit.CardZipCode"]').val(data.cardZipCode);
                            //$('input[name="AuthNetSubmit.DifferentBillingAddress"]').val(data.differentBillingAddress);
                            //$('input[name="AuthNetSubmit.FirstNameOnCard"]').val(data.firstNameOnCard);
                            //$('input[name="AuthNetSubmit.LastNameOnCard"]').val(data.lastNameOnCard);
                            //$('input[name="AuthNetSubmit.BillingAddress"]').val(data.billingAddress);
                            //$('input[name="AuthNetSubmit.BillingCity"]').val(data.billingCity);
                            //$('input[name="AuthNetSubmit.BillingState"]').val(data.billingState);
                            //$('input[name="AuthNetSubmit.BillingZipCode"]').val(data.billingZipCode);
                            //$('input[name="AuthNetSubmit.CardLastFour"]').val(data.cardLastFour);
                            //// 'input[name="EmailAddress"]'
                            //$('#register-submit-btn').removeAttr("disabled");
                            //$('#register-submit-btn').removeClass('d-none');
                            //$('#register-checkout-btn').addClass('d-none');
                        }

                    });

                    //console.log(info);
                    //debugger;
                });
        },
    };
})();

jQuery(document).ready(function () {
    // Check if we have a tenant and user
    if (!app.session.tenant) return;
    if (app.session.user == undefined) return;
    if (!app.session.user.roles || !app.session.user.roles.some(role => role.toLowerCase() === "user")) return;
    if (app.session.user.isPaid == true) return;
    
    // Exit early if user is marked as invoiced (bypasses payment)
    if (app.session.user.isInvoiced == true) return;

    // Check if the global payment popup setting is enabled at the host level
    var isGlobalPaymentPopupEnabled = abp.setting.get(AppSettings.PaymentPopup.EnableGlobalPaymentPopup);
    if (isGlobalPaymentPopupEnabled.toLowerCase() !== "true") return;

    // Required basic checks
    if (app.session.tenant.isDonorPay == false) return;
    if (app.session.surpathSettings.enableInSessionPayment == false) return;
    
    var shouldShowPopup = false;
    var cohortId = null;
    var departmentId = null;
    
    // Check if the user is in a cohort that has payment popup enabled
    if (app.session.user.isCohortUser == true) {
        cohortId = app.session.user.cohortId;
        var enabledCohorts = abp.setting.get(AppSettings.PaymentPopup.EnablePaymentPopupForCohort);
        
        // If no specific cohorts are configured, show to all cohort users
        if (!enabledCohorts || enabledCohorts === "") {
            shouldShowPopup = true;
        } else {
            // Check if user's cohort is in the enabled list
            var cohortList = enabledCohorts.split(',');
            if (cohortList.includes(cohortId.toString())) {
                shouldShowPopup = true;
            }
        }
    }
    
    // Check if the user is in a department that has payment popup enabled
    if (!shouldShowPopup && app.session.user.departmentId) {
        departmentId = app.session.user.departmentId;
        var enabledDepartments = abp.setting.get(AppSettings.PaymentPopup.EnablePaymentPopupForDepartment);
        
        // If specific departments are configured, check if user's department is in the list
        if (enabledDepartments && enabledDepartments !== "") {
            var departmentList = enabledDepartments.split(',');
            if (departmentList.includes(departmentId.toString())) {
                shouldShowPopup = true;
            }
        }
    }
    
    // If the popup should be shown based on the conditions, initialize it
    if (shouldShowPopup) {
        purchaseHelperStatusController.init({ userid: app.session.user.id });
    }
});

