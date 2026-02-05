(function () {
    $(function () {
        var _$tenantDocumentsTable = $('#TenantDocumentsTable');
        var _tenantDocumentsService = abp.services.app.tenantDocuments;
        var _entityTypeFullName = 'inzibackend.Surpath.TenantDocument';

        var $selectedDate = {
            startDate: null,
            endDate: null,
        };

        $('.date-picker').on('apply.daterangepicker', function (ev, picker) {
            $(this).val(picker.startDate.format('MM/DD/YYYY'));
        });

        $('.startDate')
            .daterangepicker(
                {
                    autoUpdateInput: false,
                    singleDatePicker: true,
                    locale: abp.localization.currentLanguage.name,
                    format: 'L',
                },
                (date) => {
                    $selectedDate.startDate = date;
                }
            )
            .on('cancel.daterangepicker', function (ev, picker) {
                $(this).val('');
                $selectedDate.startDate = null;
            });

        $('.endDate')
            .daterangepicker(
                {
                    autoUpdateInput: false,
                    singleDatePicker: true,
                    locale: abp.localization.currentLanguage.name,
                    format: 'L',
                },
                (date) => {
                    $selectedDate.endDate = date;
                }
            )
            .on('cancel.daterangepicker', function (ev, picker) {
                $(this).val('');
                $selectedDate.endDate = null;
            });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.TenantDocuments.Create'),
            edit: abp.auth.hasPermission('Pages.TenantDocuments.Edit'),
            delete: abp.auth.hasPermission('Pages.TenantDocuments.Delete'),
        };

        var _viewTenantDocumentModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/TenantDocuments/ViewtenantDocumentModal',
            modalClass: 'ViewTenantDocumentModal',
        });

        var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();
        function entityHistoryIsEnabled() {
            return (
                abp.auth.hasPermission('Pages.Administration.AuditLogs') &&
                abp.custom.EntityHistory &&
                abp.custom.EntityHistory.IsEnabled &&
                _.filter(abp.custom.EntityHistory.EnabledEntities, (entityType) => entityType === _entityTypeFullName)
                    .length === 1
            );
        }

        var getDateFilter = function (element) {
            if ($selectedDate.startDate == null) {
                return null;
            }
            return $selectedDate.startDate.format('YYYY-MM-DDT00:00:00Z');
        };

        var getMaxDateFilter = function (element) {
            if ($selectedDate.endDate == null) {
                return null;
            }
            return $selectedDate.endDate.format('YYYY-MM-DDT23:59:59Z');
        };

        var dataTable = _$tenantDocumentsTable.DataTable({
            paging: true,
            lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
            pageLength: 5000,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _tenantDocumentsService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#TenantDocumentsTableFilter').val(),
                        nameFilter: $('#NameFilterId').val(),
                        authorizedOnlyFilter: $('#AuthorizedOnlyFilterId').val(),
                        descriptionFilter: $('#DescriptionFilterId').val(),
                        tenantDocumentCategoryNameFilter: $('#TenantDocumentCategoryNameFilterId').val(),
                        tenantDocumentCategoryIdFilter: $("#TenantDocumentCategoryIdFilterId").val(),
                        recordfilenameFilter: $('#RecordfilenameFilterId').val(),
                    };
                },
            },
            columnDefs: [
                {
                    className: 'control responsive',
                    orderable: false,
                    render: function () {
                        return '';
                    },
                    targets: 0,
                },
                {
                    width: 120,
                    targets: 1,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
                            {
                                text: app.localize('View'),
                                iconStyle: 'far fa-eye mr-2',
                                action: function (data) {
                                    window.location = '/App/TenantDocuments/ViewTenantDocument/' + data.record.tenantDocument.id;
                                },
                            },
                            {
                                text: app.localize('Edit'),
                                iconStyle: 'far fa-edit mr-2',
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    window.location = '/App/TenantDocuments/CreateOrEdit/' + data.record.tenantDocument.id;
                                },
                            },
                            {
                                text: app.localize('History'),
                                iconStyle: 'fas fa-history mr-2',
                                visible: function () {
                                    return entityHistoryIsEnabled();
                                },
                                action: function (data) {
                                    _entityTypeHistoryModal.open({
                                        entityTypeFullName: _entityTypeFullName,
                                        entityId: data.record.tenantDocument.id,
                                    });
                                },
                            },
                            {
                                text: app.localize('Delete'),
                                iconStyle: 'far fa-trash-alt mr-2',
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteTenantDocument(data.record.tenantDocument);
                                },
                            },
                        ],
                    },
                },
                {
                    targets: 2,
                    data: 'tenantDocument.name',
                    name: 'name',
                },
                {
                    targets: 3,
                    data: 'tenantDocument.authorizedOnly',
                    name: 'authorizedOnly',
                    render: function (authorizedOnly) {
                        if (authorizedOnly) {
                            return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
                        }
                        return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
                    },
                },
                {
                    targets: 4,
                    data: 'tenantDocument.description',
                    name: 'description',
                },
                {
                    targets: 5,
                    data: 'tenantDocumentCategoryName',
                    name: 'tenantDocumentCategoryFk.name',
                },
                {
                    targets: 6,
                    data: 'recordfilename',
                    name: 'recordFk.filename',
                },
            ],
        });

        function getTenantDocuments() {
            dataTable.ajax.reload();
        }

        function deleteTenantDocument(tenantDocument) {
            abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
                if (isConfirmed) {
                    _tenantDocumentsService
                        .delete({
                            id: tenantDocument.id,
                        })
                        .done(function () {
                            getTenantDocuments(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                }
            });
        }

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#ExportToExcelButton').click(function () {
            _tenantDocumentsService
                .getTenantDocumentsToExcel({
                    filter: $('#TenantDocumentsTableFilter').val(),
                    nameFilter: $('#NameFilterId').val(),
                    authorizedOnlyFilter: $('#AuthorizedOnlyFilterId').val(),
                    descriptionFilter: $('#DescriptionFilterId').val(),
                    tenantDocumentCategoryNameFilter: $('#TenantDocumentCategoryNameFilterId').val(),
                    recordfilenameFilter: $('#RecordfilenameFilterId').val(),
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditTenantDocumentModalSaved', function () {
            getTenantDocuments();
        });

        $('#GetTenantDocumentsButton').click(function (e) {
            e.preventDefault();
            getTenantDocuments();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getTenantDocuments();
            }
        });

        // tenantDocumentCategoryIdFilter
        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Records/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Records/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditRecordModal',
        });

        $('#CreateNewRecordButton').click(function () {
            var _catid = $('#TenantDocumentCategoryIdFilterId').val();
            _createOrEditModal.open({ catid: _catid });
        });
    });
})();
