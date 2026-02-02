(function ($) {
    app.modals.CreateOrEditUserPurchaseModal = function () {
        var _userPurchasesService = abp.services.app.userPurchases;

        var _modalManager;
        var _$userPurchaseInformationForm = null;

        var _UserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/UserPurchases/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/UserPurchases/_UserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });

        var _SurpathServiceLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/UserPurchases/SurpathServiceLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/UserPurchases/_SurpathServiceLookupTableModal.js',
            modalClass: 'SurpathServiceLookupTableModal'
        });

        var _TenantSurpathServiceLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/UserPurchases/TenantSurpathServiceLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/UserPurchases/_TenantSurpathServiceLookupTableModal.js',
            modalClass: 'TenantSurpathServiceLookupTableModal'
        });

        var _CohortLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/UserPurchases/CohortLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/UserPurchases/_CohortLookupTableModal.js',
            modalClass: 'CohortLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$userPurchaseInformationForm = _modalManager.getModal().find('form[name=UserPurchaseInformationsForm]');
            _$userPurchaseInformationForm.validate();

            // Initialize calculation logic for prices
            $('#UserPurchase_OriginalPrice, #UserPurchase_DiscountAmount').change(function() {
                calculateAdjustedPrice();
            });

            function calculateAdjustedPrice() {
                var originalPrice = parseFloat($('#UserPurchase_OriginalPrice').val()) || 0;
                var discountAmount = parseFloat($('#UserPurchase_DiscountAmount').val()) || 0;
                var adjustedPrice = originalPrice - discountAmount;
                if (adjustedPrice < 0) adjustedPrice = 0;
                $('#UserPurchase_AdjustedPrice').val(adjustedPrice);
            }

            // If this is a new purchase, initialize the adjusted price
            if (!_modalManager.getArgs().id) {
                calculateAdjustedPrice();
            }

            // User lookup modal
            $('#OpenUserLookupTableButton').click(function () {
                var userPurchase = _$userPurchaseInformationForm.serializeFormToObject();
                _UserLookupTableModal.open({ id: userPurchase.userId, displayName: userPurchase.userNameLookup }, function (data) {
                    _$userPurchaseInformationForm.find('input[name=userNameLookup]').val(data.displayName);
                    _$userPurchaseInformationForm.find('input[name=userId]').val(data.id);
                });
            });

            $('#ClearUserNameButton').click(function () {
                _$userPurchaseInformationForm.find('input[name=userNameLookup]').val('');
                _$userPurchaseInformationForm.find('input[name=userId]').val('');
            });

            // SurpathService lookup modal
            $('#OpenSurpathServiceLookupTableButton').click(function () {
                var userPurchase = _$userPurchaseInformationForm.serializeFormToObject();
                _SurpathServiceLookupTableModal.open({ id: userPurchase.surpathServiceId, displayName: userPurchase.surpathServiceNameLookup }, function (data) {
                    _$userPurchaseInformationForm.find('input[name=surpathServiceNameLookup]').val(data.displayName);
                    _$userPurchaseInformationForm.find('input[name=surpathServiceId]').val(data.id);
                });
            });

            $('#ClearSurpathServiceNameButton').click(function () {
                _$userPurchaseInformationForm.find('input[name=surpathServiceNameLookup]').val('');
                _$userPurchaseInformationForm.find('input[name=surpathServiceId]').val('');
            });

            // TenantSurpathService lookup modal
            $('#OpenTenantSurpathServiceLookupTableButton').click(function () {
                var userPurchase = _$userPurchaseInformationForm.serializeFormToObject();
                _TenantSurpathServiceLookupTableModal.open({ id: userPurchase.tenantSurpathServiceId, displayName: userPurchase.tenantSurpathServiceNameLookup }, function (data) {
                    _$userPurchaseInformationForm.find('input[name=tenantSurpathServiceNameLookup]').val(data.displayName);
                    _$userPurchaseInformationForm.find('input[name=tenantSurpathServiceId]').val(data.id);
                });
            });

            $('#ClearTenantSurpathServiceNameButton').click(function () {
                _$userPurchaseInformationForm.find('input[name=tenantSurpathServiceNameLookup]').val('');
                _$userPurchaseInformationForm.find('input[name=tenantSurpathServiceId]').val('');
            });

            // Cohort lookup modal
            $('#OpenCohortLookupTableButton').click(function () {
                var userPurchase = _$userPurchaseInformationForm.serializeFormToObject();
                _CohortLookupTableModal.open({ id: userPurchase.cohortId, displayName: userPurchase.cohortNameLookup }, function (data) {
                    _$userPurchaseInformationForm.find('input[name=cohortNameLookup]').val(data.displayName);
                    _$userPurchaseInformationForm.find('input[name=cohortId]').val(data.id);
                });
            });

            $('#ClearCohortNameButton').click(function () {
                _$userPurchaseInformationForm.find('input[name=cohortNameLookup]').val('');
                _$userPurchaseInformationForm.find('input[name=cohortId]').val('');
            });
        };

        this.save = function () {
            if (!_$userPurchaseInformationForm.valid()) {
                return;
            }

            var userPurchase = _$userPurchaseInformationForm.serializeFormToObject();

            // Convert checkbox values
            userPurchase.isRecurring = $('#UserPurchase_IsRecurring').is(':checked');

            // Convert date strings to Date objects
            if (userPurchase.purchaseDate) {
                userPurchase.purchaseDate = moment(userPurchase.purchaseDate, 'YYYY-MM-DD').toDate();
            }

            if (userPurchase.expirationDate) {
                userPurchase.expirationDate = moment(userPurchase.expirationDate, 'YYYY-MM-DD').toDate();
            }

            _modalManager.setBusy(true);
            _userPurchasesService.createOrEdit(
                userPurchase
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditUserPurchaseModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);