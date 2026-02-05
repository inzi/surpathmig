(function () {
    app.modals.ImportTenantUsersModal = function () {
        var _tenantService = abp.services.app.tenant;
        var _modalManager;
        var _tenantId;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _tenantId = $('#TenantId').val();

            // Export button handler - uses service proxy
            $('#ExportTenantUsersButton').click(function () {
                _tenantService
                    .getTenantUsersToExcel({
                        id: _tenantId
                    })
                    .done(function (result) {
                        app.downloadTempFile(result);
                    });
            });

            // Import button handler
            $('#ImportButton').click(function () {
                var fileInput = $('#ImportTenantUsersFileInput')[0];
                if (!fileInput.files || !fileInput.files.length) {
                    abp.message.warn(app.localize('PleaseSelectAFile'));
                    return;
                }

                var file = fileInput.files[0];
                var formData = new FormData();
                formData.append('tenantId', _tenantId);
                formData.append('file', file);

                _modalManager.setBusy(true);

                abp.ajax({
                    url: abp.appPath + 'App/Tenants/ImportUsersFromExcel',
                    type: 'POST',
                    data: formData,
                    processData: false,
                    contentType: false
                }).done(function (result) {
                    if (result.success) {
                        abp.notify.info(app.localize('ImportUsersProcessStart'));
                        _modalManager.close();
                    } else {
                        abp.message.error(result.error);
                    }
                }).always(function () {
                    _modalManager.setBusy(false);
                });
            });
        };
    };
})();
