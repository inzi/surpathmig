(function () {
    $(function () {
        var _$ledgerEntryDetailsTable = $('#MasterDetailChild_LedgerEntry_LedgerEntryDetailsTable');
        var _ledgerEntryDetailsService = abp.services.app.ledgerEntryDetails;
        var _entityTypeFullName = 'inzibackend.Surpath.LedgerEntryDetail';

        var $selectedDate = {
            startDate: null,
            endDate: null,
        };

        $('.date-picker').on('apply.daterangepicker', function (ev, picker) {
            $(this).val(picker.startDate.format('MM/DD/YYYY'));
        });

        $('.startDate')
            .daterangepicker({
                autoUpdateInput: false,
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L',
            })
            .on('apply.daterangepicker', (ev, picker) => {
                $selectedDate.startDate = picker.startDate;
                getLedgerEntryDetails();
            })
            .on('cancel.daterangepicker', function (ev, picker) {
                $(this).val('');
                $selectedDate.startDate = null;
                getLedgerEntryDetails();
            });

        $('.endDate')
            .daterangepicker({
                autoUpdateInput: false,
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L',
            })
            .on('apply.daterangepicker', (ev, picker) => {
                $selectedDate.endDate = picker.startDate;
                getLedgerEntryDetails();
            })
            .on('cancel.daterangepicker', function (ev, picker) {
                $(this).val('');
                $selectedDate.endDate = null;
                getLedgerEntryDetails();
            });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.LedgerEntryDetails.Create'),
            edit: abp.auth.hasPermission('Pages.LedgerEntryDetails.Edit'),
            delete: abp.auth.hasPermission('Pages.LedgerEntryDetails.Delete'),
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/MasterDetailChild_LedgerEntry_LedgerEntryDetails/CreateOrEditModal',
            scriptUrl:
                abp.appPath +
                'view-resources/Areas/App/Views/MasterDetailChild_LedgerEntry_LedgerEntryDetails/_CreateOrEditModal.js',
            modalClass: 'MasterDetailChild_LedgerEntry_CreateOrEditLedgerEntryDetailModal',
        });

        var _viewLedgerEntryDetailModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/MasterDetailChild_LedgerEntry_LedgerEntryDetails/ViewledgerEntryDetailModal',
            modalClass: 'MasterDetailChild_LedgerEntry_ViewLedgerEntryDetailModal',
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

        var dataTable = _$ledgerEntryDetailsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _ledgerEntryDetailsService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#MasterDetailChild_LedgerEntry_LedgerEntryDetailsTableFilter').val(),
                        noteFilter: $('#MasterDetailChild_LedgerEntry_NoteFilterId').val(),
                        minAmountFilter: $('#MasterDetailChild_LedgerEntry_MinAmountFilterId').val(),
                        maxAmountFilter: $('#MasterDetailChild_LedgerEntry_MaxAmountFilterId').val(),
                        minDiscountFilter: $('#MasterDetailChild_LedgerEntry_MinDiscountFilterId').val(),
                        maxDiscountFilter: $('#MasterDetailChild_LedgerEntry_MaxDiscountFilterId').val(),
                        minDiscountAmountFilter: $('#MasterDetailChild_LedgerEntry_MinDiscountAmountFilterId').val(),
                        maxDiscountAmountFilter: $('#MasterDetailChild_LedgerEntry_MaxDiscountAmountFilterId').val(),
                        metaDataFilter: $('#MasterDetailChild_LedgerEntry_MetaDataFilterId').val(),
                        minAmountPaidFilter: $('#MasterDetailChild_LedgerEntry_MinAmountPaidFilterId').val(),
                        maxAmountPaidFilter: $('#MasterDetailChild_LedgerEntry_MaxAmountPaidFilterId').val(),
                        minDatePaidOnFilter: getDateFilter($('#MasterDetailChild_LedgerEntry_MinDatePaidOnFilterId')),
                        maxDatePaidOnFilter: getMaxDateFilter($('#MasterDetailChild_LedgerEntry_MaxDatePaidOnFilterId')),
                        surpathServiceNameFilter: $('#MasterDetailChild_LedgerEntry_SurpathServiceNameFilterId').val(),
                        tenantSurpathServiceNameFilter: $('#MasterDetailChild_LedgerEntry_TenantSurpathServiceNameFilterId').val(),
                        ledgerEntryIdFilter: $('#MasterDetailChild_LedgerEntry_LedgerEntryDetailsId').val(),
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
                                action: function (data) {
                                    _viewLedgerEntryDetailModal.open({ id: data.record.ledgerEntryDetail.id });
                                },
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({ id: data.record.ledgerEntryDetail.id });
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
                                        entityId: data.record.ledgerEntryDetail.id,
                                    });
                                },
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteLedgerEntryDetail(data.record.ledgerEntryDetail);
                                },
                            },
                        ],
                    },
                },
                //<th>@L("Amount")</th>
                //<th>@L("AmountPaid")</th>
                //<th>@L("DatePaidOn")</th>
                //<th>@L("SurpathServiceName")</th>
                //<th>@L("Note")</th>
                {
                    targets: 2,
                    data: 'ledgerEntryDetail.amount',
                    name: 'amount',
                },
                {
                    targets: 3,
                    data: 'ledgerEntryDetail.amountPaid',
                    name: 'amountPaid',
                },
                {
                    targets: 4,
                    data: 'ledgerEntryDetail.datePaidOn',
                    name: 'datePaidOn',
                    render: function (datePaidOn) {
                        if (datePaidOn) {
                            return moment(datePaidOn).format('L');
                        }
                        return '';
                    },
                },
                {
                    targets: 5,
                    data: 'surpathServiceName',
                    name: 'surpathServiceFk.name',
                },
                {
                    targets: 6,
                    data: 'ledgerEntryDetail.note',
                    name: 'note',
                },


                //{
                //    targets: 2,
                //    data: 'ledgerEntryDetail.note',
                //    name: 'note',
                //},
                //{
                //    targets: 3,
                //    data: 'ledgerEntryDetail.amount',
                //    name: 'amount',
                //},
                //{
                //    targets: 4,
                //    data: 'ledgerEntryDetail.discount',
                //    name: 'discount',
                //},
                //{
                //    targets: 5,
                //    data: 'ledgerEntryDetail.discountAmount',
                //    name: 'discountAmount',
                //},
                //{
                //    targets: 6,
                //    data: 'ledgerEntryDetail.metaData',
                //    name: 'metaData',
                //},
                //{
                //    targets: 7,
                //    data: 'ledgerEntryDetail.amountPaid',
                //    name: 'amountPaid',
                //},
                //{
                //    targets: 8,
                //    data: 'ledgerEntryDetail.datePaidOn',
                //    name: 'datePaidOn',
                //    render: function (datePaidOn) {
                //        if (datePaidOn) {
                //            return moment(datePaidOn).format('L');
                //        }
                //        return '';
                //    },
                //},
                //{
                //    targets: 9,
                //    data: 'surpathServiceName',
                //    name: 'surpathServiceFk.name',
                //},
                //{
                //    targets: 10,
                //    data: 'tenantSurpathServiceName',
                //    name: 'tenantSurpathServiceFk.name',
                //},
            ],
        });

        function getLedgerEntryDetails() {
            dataTable.ajax.reload();
        }

        function deleteLedgerEntryDetail(ledgerEntryDetail) {
            abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
                if (isConfirmed) {
                    _ledgerEntryDetailsService
                        .delete({
                            id: ledgerEntryDetail.id,
                        })
                        .done(function () {
                            getLedgerEntryDetails(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                }
            });
        }

        $('#MasterDetailChild_LedgerEntry_ShowAdvancedFiltersSpan').click(function () {
            $('#MasterDetailChild_LedgerEntry_ShowAdvancedFiltersSpan').hide();
            $('#MasterDetailChild_LedgerEntry_HideAdvancedFiltersSpan').show();
            $('#MasterDetailChild_LedgerEntry_AdvacedAuditFiltersArea').slideDown();
        });

        $('#MasterDetailChild_LedgerEntry_HideAdvancedFiltersSpan').click(function () {
            $('#MasterDetailChild_LedgerEntry_HideAdvancedFiltersSpan').hide();
            $('#MasterDetailChild_LedgerEntry_ShowAdvancedFiltersSpan').show();
            $('#MasterDetailChild_LedgerEntry_AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewLedgerEntryDetailButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _ledgerEntryDetailsService
                .getLedgerEntryDetailsToExcel({
                    filter: $('#LedgerEntryDetailsTableFilter').val(),
                    noteFilter: $('#MasterDetailChild_LedgerEntry_NoteFilterId').val(),
                    minAmountFilter: $('#MasterDetailChild_LedgerEntry_MinAmountFilterId').val(),
                    maxAmountFilter: $('#MasterDetailChild_LedgerEntry_MaxAmountFilterId').val(),
                    minDiscountFilter: $('#MasterDetailChild_LedgerEntry_MinDiscountFilterId').val(),
                    maxDiscountFilter: $('#MasterDetailChild_LedgerEntry_MaxDiscountFilterId').val(),
                    minDiscountAmountFilter: $('#MasterDetailChild_LedgerEntry_MinDiscountAmountFilterId').val(),
                    maxDiscountAmountFilter: $('#MasterDetailChild_LedgerEntry_MaxDiscountAmountFilterId').val(),
                    metaDataFilter: $('#MasterDetailChild_LedgerEntry_MetaDataFilterId').val(),
                    minAmountPaidFilter: $('#MasterDetailChild_LedgerEntry_MinAmountPaidFilterId').val(),
                    maxAmountPaidFilter: $('#MasterDetailChild_LedgerEntry_MaxAmountPaidFilterId').val(),
                    minDatePaidOnFilter: getDateFilter($('#MasterDetailChild_LedgerEntry_MinDatePaidOnFilterId')),
                    maxDatePaidOnFilter: getMaxDateFilter($('#MasterDetailChild_LedgerEntry_MaxDatePaidOnFilterId')),
                    surpathServiceNameFilter: $('#MasterDetailChild_LedgerEntry_SurpathServiceNameFilterId').val(),
                    tenantSurpathServiceNameFilter: $('#MasterDetailChild_LedgerEntry_TenantSurpathServiceNameFilterId').val(),
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditLedgerEntryDetailModalSaved', function () {
            getLedgerEntryDetails();
        });

        $('#GetLedgerEntryDetailsButton').click(function (e) {
            e.preventDefault();
            getLedgerEntryDetails();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getLedgerEntryDetails();
            }
        });

        $('.reload-on-change').change(function (e) {
            getLedgerEntryDetails();
        });

        $('.reload-on-keyup').keyup(function (e) {
            getLedgerEntryDetails();
        });

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getLedgerEntryDetails();
        });
    });
})();
