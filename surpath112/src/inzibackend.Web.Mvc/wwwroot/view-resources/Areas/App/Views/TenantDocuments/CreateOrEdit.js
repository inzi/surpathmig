(function () {
    $(function () {
        var _tenantDocumentsService = abp.services.app.tenantDocuments;


        var _documentCategoryid = KTUtil.getURLParam('catid');

        var _$tenantDocumentInformationForm = $('form[name=TenantDocumentInformationsForm]');
        _$tenantDocumentInformationForm.validate();

        var _TenantDocumenttenantDocumentCategoryLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/TenantDocuments/TenantDocumentCategoryLookupTableModal',
            scriptUrl:
                abp.appPath +
                'view-resources/Areas/App/Views/TenantDocuments/_TenantDocumentTenantDocumentCategoryLookupTableModal.js',
            modalClass: 'TenantDocumentCategoryLookupTableModal',
        });
        var _TenantDocumentrecordLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/TenantDocuments/RecordLookupTableModal',
            scriptUrl:
                abp.appPath + 'view-resources/Areas/App/Views/TenantDocuments/_TenantDocumentRecordLookupTableModal.js',
            modalClass: 'RecordLookupTableModal',
        });

        $('.date-picker').daterangepicker({
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        });

        $('#OpenTenantDocumentCategoryLookupTableButton').click(function () {
            var tenantDocument = _$tenantDocumentInformationForm.serializeFormToObject();

            _TenantDocumenttenantDocumentCategoryLookupTableModal.open(
                { id: tenantDocument.tenantDocumentCategoryId, displayName: tenantDocument.tenantDocumentCategoryName },
                function (data) {
                    _$tenantDocumentInformationForm.find('input[name=tenantDocumentCategoryName]').val(data.displayName);
                    _$tenantDocumentInformationForm.find('input[name=tenantDocumentCategoryId]').val(data.id);
                }
            );
        });

        $('#ClearTenantDocumentCategoryNameButton').click(function () {
            _$tenantDocumentInformationForm.find('input[name=tenantDocumentCategoryName]').val('');
            _$tenantDocumentInformationForm.find('input[name=tenantDocumentCategoryId]').val('');
        });

        $('#OpenRecordLookupTableButton').click(function () {
            var tenantDocument = _$tenantDocumentInformationForm.serializeFormToObject();

            _TenantDocumentrecordLookupTableModal.open(
                { id: tenantDocument.recordId, displayName: tenantDocument.recordfilename },
                function (data) {
                    _$tenantDocumentInformationForm.find('input[name=recordfilename]').val(data.displayName);
                    _$tenantDocumentInformationForm.find('input[name=recordId]').val(data.id);
                }
            );
        });

        $('#ClearRecordfilenameButton').click(function () {
            _$tenantDocumentInformationForm.find('input[name=recordfilename]').val('');
            _$tenantDocumentInformationForm.find('input[name=recordId]').val('');
        });

        function save(successCallback) {
            if (!_$tenantDocumentInformationForm.valid()) {
                return;
            }
            if (
                $('#TenantDocument_TenantDocumentCategoryId').prop('required') &&
                $('#TenantDocument_TenantDocumentCategoryId').val() == ''
            ) {
                abp.message.error(app.localize('{0}IsRequired', app.localize('TenantDocumentCategory')));
                return;
            }
            if ($('#TenantDocument_RecordId').prop('required') && $('#TenantDocument_RecordId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Record')));
                return;
            }

            var tenantDocument = _$tenantDocumentInformationForm.serializeFormToObject();
            _documentCategoryid = tenantDocument.tenantDocumentCategoryId;

            abp.ui.setBusy();
            _tenantDocumentsService
                .createOrEdit(tenantDocument)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    abp.event.trigger('app.createOrEditTenantDocumentModalSaved');

                    if (typeof successCallback === 'function') {
                        successCallback();
                    }
                })
                .always(function () {
                    abp.ui.clearBusy();
                });
        }

        function clearForm() {
            _$tenantDocumentInformationForm[0].reset();
        }

        $('#saveBtn').click(function () {

            // ?catid=08da92f1-ebb5-48dc-8695-e904b4aa6767
            // tenantdocuments must have a category id

            save(function () {
                window.location = '/App/TenantDocuments?catid=' + _documentCategoryid;
            });
        });

        $('#saveAndNewBtn').click(function () {
            save(function () {
                if (!$('input[name=id]').val()) {
                    //if it is create page
                    clearForm();
                }
            });
        });
    });
})();
