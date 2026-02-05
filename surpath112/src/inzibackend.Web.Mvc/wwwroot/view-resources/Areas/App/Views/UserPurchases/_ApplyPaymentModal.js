(function ($) {
    app.modals.ApplyPaymentModal = function () {
        var _userPurchasesService = abp.services.app.userPurchases;

        var _modalManager;
        var _$applyPaymentForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _$applyPaymentForm = _modalManager.getModal().find('form[name=ApplyPaymentInformationsForm]');
            _$applyPaymentForm.validate();
        };

        this.save = function () {
            if (!_$applyPaymentForm.valid()) {
                return;
            }

            var payment = _$applyPaymentForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _userPurchasesService.applyPayment(
                payment.userPurchaseId,
                payment.amount,
                payment.paymentMethod,
                payment.notes
            ).done(function () {
                abp.notify.info(app.localize('PaymentAppliedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.applyPaymentModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);