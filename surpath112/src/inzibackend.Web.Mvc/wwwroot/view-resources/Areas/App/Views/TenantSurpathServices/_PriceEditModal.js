(function ($) {
    app.modals.PriceEditModal = function () {

        var _tenantSurpathServicesService = abp.services.app.tenantSurpathServices;
        var _modalManager;
        var _$form = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            _$form = modal.find('form[name=PriceEditForm]');
            _$form.validate();

            // Handle remove override checkbox
            modal.find('#RemoveOverride').on('change', function () {
                var isChecked = $(this).is(':checked');
                var $newPrice = modal.find('#NewPrice');
                
                if (isChecked) {
                    $newPrice.val('').prop('disabled', true);
                } else {
                    $newPrice.prop('disabled', false);
                }
            });
        };

        this.save = function () {
            if (!_$form.valid()) {
                return;
            }

            var level = _$form.find('#Level').val();
            var targetId = _$form.find('#TargetId').val();
            var serviceId = _$form.find('#ServiceId').val();
            var tenantId = parseInt(_$form.find('#TenantId').val());
            var removeOverride = _$form.find('#RemoveOverride').is(':checked');
            var newPrice = removeOverride ? null : parseFloat(_$form.find('#NewPrice').val()) || null;
            var isInvoiced = _$form.find('#IsInvoiced').is(':checked');

            _modalManager.setBusy(true);

            var input = {
                id: null, // Will be determined by the service based on existing record
                surpathServiceId: serviceId,
                price: newPrice,
                isInvoiced: isInvoiced,
                level: level,
                targetId: targetId,
                tenantId: tenantId
            };

            _tenantSurpathServicesService.updateServicePrice(input)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.priceEditModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);