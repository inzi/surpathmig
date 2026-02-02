(function ($) {
    app.modals.SetAllServicesPriceModal = function () {

        var _tenantSurpathServicesService = abp.services.app.tenantSurpathServices;
        var _modalManager;
        var _$form = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            _$form = modal.find('form[name=SetAllServicesPriceForm]');
            _$form.validate();
        };

        this.save = function () {
            if (!_$form.valid()) {
                return;
            }

            var targetType = _$form.find('#TargetType').val();
            var targetId = _$form.find('#TargetId').val();
            var tenantId = parseInt(_$form.find('#TenantId').val());
            var price = parseFloat(_$form.find('#UniformPrice').val());

            if (isNaN(price) || price < 0) {
                abp.message.error(app.localize('PleaseEnterValidPrice'));
                return;
            }

            _modalManager.setBusy(true);

            var input = {
                price: price,
                targetType: targetType,
                targetId: targetId,
                tenantId: tenantId
            };

            _tenantSurpathServicesService.setAllServicesPrice(input)
                .done(function () {
                    abp.notify.info(app.localize('AllServicesPricesUpdatedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.setAllServicesPriceModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);