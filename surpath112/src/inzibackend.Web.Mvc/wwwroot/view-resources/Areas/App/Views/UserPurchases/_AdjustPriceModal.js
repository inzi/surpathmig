(function ($) {
    app.modals.AdjustPriceModal = function () {
        var _userPurchasesService = abp.services.app.userPurchases;

        var _modalManager;
        var _$adjustPriceForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _$adjustPriceForm = _modalManager.getModal().find('form[name=AdjustPriceInformationsForm]');
            _$adjustPriceForm.validate();
        };

        this.save = function () {
            if (!_$adjustPriceForm.valid()) {
                return;
            }

            var adjustment = _$adjustPriceForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _userPurchasesService.adjustPrice(
                adjustment.userPurchaseId,
                adjustment.adjustedPrice,
                adjustment.reason
            ).done(function () {
                abp.notify.info(app.localize('PriceAdjustedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.adjustPriceModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);