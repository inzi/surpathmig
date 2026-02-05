(function () {
    $(function () {
        var _$form = $('#ManualDocumentUploadForm');

        _$form.submit(function (e) {
            e.preventDefault();

            if (!_$form.valid()) {
                return;
            }

            var formData = new FormData(_$form[0]);

            abp.ui.setBusy(_$form);
            $.ajax({
                url: abp.appPath + 'App/HostTools/UploadDocument',
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: function (result) {
                    abp.ui.clearBusy(_$form);
                    if (result.success) {
                        abp.notify.info(result.message);
                        // You can do something with result.recordId here if needed
                    } else {
                        abp.notify.error(result.message);
                    }
                },
                error: function (xhr) {
                    abp.ui.clearBusy(_$form);
                    abp.notify.error(app.localize('ErrorOccurred'));
                }
            });
        });
    });
})();