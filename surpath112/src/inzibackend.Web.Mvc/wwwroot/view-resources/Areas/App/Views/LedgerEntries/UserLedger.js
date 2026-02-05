(function () {
    $(function () {
        var _$ledgerEntriesTable = $('#LedgerEntriesTable');
        var _ledgerEntriesService = abp.services.app.ledgerEntries;
        var _entityTypeFullName = 'inzibackend.Surpath.LedgerEntry';
        var _userId = $('#UserId').val();

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
                getLedgerEntries();
            })
            .on('cancel.daterangepicker', function (ev, picker) {
                $(this).val('');
                $selectedDate.startDate = null;
                getLedgerEntries();
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
                getLedgerEntries();
            })
            .on('cancel.daterangepicker', function (ev, picker) {
                $(this).val('');
                $selectedDate.endDate = null;
                getLedgerEntries();
            });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.LedgerEntries.Create'),
            edit: abp.auth.hasPermission('Pages.LedgerEntries.Edit'),
            delete: abp.auth.hasPermission('Pages.LedgerEntries.Delete'),
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LedgerEntries/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LedgerEntries/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditLedgerEntryModal',
        });

        var _viewLedgerEntryModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LedgerEntries/ViewledgerEntryModal',
            modalClass: 'ViewLedgerEntryModal',
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

        var dataTable = _$ledgerEntriesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _ledgerEntriesService.getAllForUserId,
                inputFilter: function () {
                    return {
                        filter: $('#LedgerEntriesTableFilter').val(),
                        nameFilter: $('#NameFilterId').val(),
                        serviceTypeFilter: $('#ServiceTypeFilterId').val(),
                        minAmountFilter: $('#MinAmountFilterId').val(),
                        maxAmountFilter: $('#MaxAmountFilterId').val(),
                        minDiscountAmountFilter: $('#MinDiscountAmountFilterId').val(),
                        maxDiscountAmountFilter: $('#MaxDiscountAmountFilterId').val(),
                        minTotalPriceFilter: $('#MinTotalPriceFilterId').val(),
                        maxTotalPriceFilter: $('#MaxTotalPriceFilterId').val(),
                        paymentPeriodTypeFilter: $('#PaymentPeriodTypeFilterId').val(),
                        minExpirationDateFilter: getDateFilter($('#MinExpirationDateFilterId')),
                        maxExpirationDateFilter: getMaxDateFilter($('#MaxExpirationDateFilterId')),
                        transactionNameFilter: $('#TransactionNameFilterId').val(),
                        transactionKeyFilter: $('#TransactionKeyFilterId').val(),
                        transactionIdFilter: $('#TransactionIdFilterId').val(),
                        settledFilter: $('#SettledFilterId').val(),
                        minAmountDueFilter: $('#MinAmountDueFilterId').val(),
                        maxAmountDueFilter: $('#MaxAmountDueFilterId').val(),
                        paymentTokenFilter: $('#PaymentTokenFilterId').val(),
                        authNetCustomerProfileIdFilter: $('#AuthNetCustomerProfileIdFilterId').val(),
                        authNetCustomerPaymentProfileIdFilter: $('#AuthNetCustomerPaymentProfileIdFilterId').val(),
                        authNetCustomerAddressIdFilter: $('#AuthNetCustomerAddressIdFilterId').val(),
                        accountNumberFilter: $('#AccountNumberFilterId').val(),
                        noteFilter: $('#NoteFilterId').val(),
                        metaDataFilter: $('#MetaDataFilterId').val(),
                        authCodeFilter: $('#AuthCodeFilterId').val(),
                        referenceTransactionIdFilter: $('#ReferenceTransactionIdFilterId').val(),
                        transactionHashFilter: $('#TransactionHashFilterId').val(),
                        accountTypeFilter: $('#AccountTypeFilterId').val(),
                        transactionCodeFilter: $('#TransactionCodeFilterId').val(),
                        transactionMessageFilter: $('#TransactionMessageFilterId').val(),
                        authNetTransHashSha2Filter: $('#AuthNetTransHashSha2FilterId').val(),
                        authNetNetworkTransIdFilter: $('#AuthNetNetworkTransIdFilterId').val(),
                        minPaidAmountFilter: $('#MinPaidAmountFilterId').val(),
                        maxPaidAmountFilter: $('#MaxPaidAmountFilterId').val(),
                        minPaidInCashFilter: $('#MinPaidInCashFilterId').val(),
                        maxPaidInCashFilter: $('#MaxPaidInCashFilterId').val(),
                        minAvailableUserBalanceFilter: $('#MinAvailableUserBalanceFilterId').val(),
                        maxAvailableUserBalanceFilter: $('#MaxAvailableUserBalanceFilterId').val(),
                        cardNameOnCardFilter: $('#CardNameOnCardFilterId').val(),
                        cardZipCodeFilter: $('#CardZipCodeFilterId').val(),
                        minBalanceForwardFilter: $('#MinBalanceForwardFilterId').val(),
                        maxBalanceForwardFilter: $('#MaxBalanceForwardFilterId').val(),
                        userNameFilter: $('#UserNameFilterId').val(),
                        tenantDocumentNameFilter: $('#TenantDocumentNameFilterId').val(),
                        cohortNameFilter: $('#CohortNameFilterId').val(),
                        userIdFilter: _userId
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
                                    _viewLedgerEntryModal.open({ id: data.record.ledgerEntry.id });
                                },
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({ id: data.record.ledgerEntry.id });
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
                                        entityId: data.record.ledgerEntry.id,
                                    });
                                },
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteLedgerEntry(data.record.ledgerEntry);
                                },
                            },
                        ],
                    },
                },
                {
                    className: 'details-control',
                    targets: 2,
                    orderable: false,
                    autoWidth: false,
                    visible: abp.auth.hasPermission('Pages.LedgerEntryDetails'),
                    render: function () {
                        return `<button class="btn btn-primary btn-xs Edit_LedgerEntryDetail_LedgerEntryId">${app.localize(
                            'EditLedgerEntryDetail'
                        )}</button>`;
                    },
                },
                //<th>@L("TotalPrice")</th>
                //<th>@L("TransactionId")</th>
                //<th>@L("AuthCode")</th>
                //<th>@L("AccountNumber")</th>
                //<th>@L("CardNameOnCard")</th>
                //<th>@L("CardZipCode")</th>

                {
                    targets: 3,
                    data: 'ledgerEntry.paidAmount',
                    name: 'paidAmount',
                },
                {
                    targets: 4,
                    data: 'ledgerEntry.totalPrice',
                    name: 'totalPrice',
                },
                {
                    targets: 5,
                    data: 'ledgerEntry.transactionId',
                    name: 'transactionId',
                },
                {
                    targets: 6,
                    data: 'ledgerEntry.authCode',
                    name: 'authCode',
                },
                {
                    targets: 7,
                    data: 'ledgerEntry.accountNumber',
                    name: 'accountNumber',
                },
                {
                    targets: 8,
                    data: 'ledgerEntry.cardNameOnCard',
                    name: 'cardNameOnCard',
                },
                {
                    targets: 9,
                    data: 'ledgerEntry.cardZipCode',
                    name: 'cardZipCode',
                },


                //{
                //    targets: 3,
                //    data: 'ledgerEntry.name',
                //    name: 'name',
                //},
                //{
                //    targets: 4,
                //    data: 'ledgerEntry.serviceType',
                //    name: 'serviceType',
                //    render: function (serviceType) {
                //        return app.localize('Enum_EnumServiceType_' + serviceType);
                //    },
                //},
                //{
                //    targets: 5,
                //    data: 'ledgerEntry.amount',
                //    name: 'amount',
                //},
                //{
                //    targets: 6,
                //    data: 'ledgerEntry.discountAmount',
                //    name: 'discountAmount',
                //},
                //{
                //    targets: 7,
                //    data: 'ledgerEntry.totalPrice',
                //    name: 'totalPrice',
                //},
                //{
                //    targets: 8,
                //    data: 'ledgerEntry.paymentPeriodType',
                //    name: 'paymentPeriodType',
                //    render: function (paymentPeriodType) {
                //        return app.localize('Enum_PaymentPeriodType_' + paymentPeriodType);
                //    },
                //},
                //{
                //    targets: 9,
                //    data: 'ledgerEntry.expirationDate',
                //    name: 'expirationDate',
                //    render: function (expirationDate) {
                //        if (expirationDate) {
                //            return moment(expirationDate).format('L');
                //        }
                //        return '';
                //    },
                //},
                //{
                //    targets: 10,
                //    data: 'ledgerEntry.transactionName',
                //    name: 'transactionName',
                //},
                //{
                //    targets: 11,
                //    data: 'ledgerEntry.transactionKey',
                //    name: 'transactionKey',
                //},
                //{
                //    targets: 12,
                //    data: 'ledgerEntry.transactionId',
                //    name: 'transactionId',
                //},
                //{
                //    targets: 13,
                //    data: 'ledgerEntry.settled',
                //    name: 'settled',
                //    render: function (settled) {
                //        if (settled) {
                //            return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
                //        }
                //        return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
                //    },
                //},
                //{
                //    targets: 14,
                //    data: 'ledgerEntry.amountDue',
                //    name: 'amountDue',
                //},
                //{
                //    targets: 15,
                //    data: 'ledgerEntry.paymentToken',
                //    name: 'paymentToken',
                //},
                //{
                //    targets: 16,
                //    data: 'ledgerEntry.authNetCustomerProfileId',
                //    name: 'authNetCustomerProfileId',
                //},
                //{
                //    targets: 17,
                //    data: 'ledgerEntry.authNetCustomerPaymentProfileId',
                //    name: 'authNetCustomerPaymentProfileId',
                //},
                //{
                //    targets: 18,
                //    data: 'ledgerEntry.authNetCustomerAddressId',
                //    name: 'authNetCustomerAddressId',
                //},
                //{
                //    targets: 19,
                //    data: 'ledgerEntry.accountNumber',
                //    name: 'accountNumber',
                //},
                //{
                //    targets: 20,
                //    data: 'ledgerEntry.note',
                //    name: 'note',
                //},
                //{
                //    targets: 21,
                //    data: 'ledgerEntry.metaData',
                //    name: 'metaData',
                //},
                //{
                //    targets: 22,
                //    data: 'ledgerEntry.authCode',
                //    name: 'authCode',
                //},
                //{
                //    targets: 23,
                //    data: 'ledgerEntry.referenceTransactionId',
                //    name: 'referenceTransactionId',
                //},
                //{
                //    targets: 24,
                //    data: 'ledgerEntry.transactionHash',
                //    name: 'transactionHash',
                //},
                //{
                //    targets: 25,
                //    data: 'ledgerEntry.accountType',
                //    name: 'accountType',
                //},
                //{
                //    targets: 26,
                //    data: 'ledgerEntry.transactionCode',
                //    name: 'transactionCode',
                //},
                //{
                //    targets: 27,
                //    data: 'ledgerEntry.transactionMessage',
                //    name: 'transactionMessage',
                //},
                //{
                //    targets: 28,
                //    data: 'ledgerEntry.authNetTransHashSha2',
                //    name: 'authNetTransHashSha2',
                //},
                //{
                //    targets: 29,
                //    data: 'ledgerEntry.authNetNetworkTransId',
                //    name: 'authNetNetworkTransId',
                //},
                //{
                //    targets: 30,
                //    data: 'ledgerEntry.paidAmount',
                //    name: 'paidAmount',
                //},
                //{
                //    targets: 31,
                //    data: 'ledgerEntry.paidInCash',
                //    name: 'paidInCash',
                //},
                //{
                //    targets: 32,
                //    data: 'ledgerEntry.availableUserBalance',
                //    name: 'availableUserBalance',
                //},
                //{
                //    targets: 33,
                //    data: 'ledgerEntry.cardNameOnCard',
                //    name: 'cardNameOnCard',
                //},
                //{
                //    targets: 34,
                //    data: 'ledgerEntry.cardZipCode',
                //    name: 'cardZipCode',
                //},
                //{
                //    targets: 35,
                //    data: 'ledgerEntry.balanceForward',
                //    name: 'balanceForward',
                //},
                //{
                //    targets: 36,
                //    data: 'userName',
                //    name: 'userFk.name',
                //},
                //{
                //    targets: 37,
                //    data: 'tenantDocumentName',
                //    name: 'tenantDocumentFk.name',
                //},
                //{
                //    targets: 38,
                //    data: 'cohortName',
                //    name: 'cohortFk.name',
                //},
            ],
        });

        function getLedgerEntries() {
            dataTable.ajax.reload();
        }

        function deleteLedgerEntry(ledgerEntry) {
            abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
                if (isConfirmed) {
                    _ledgerEntriesService
                        .delete({
                            id: ledgerEntry.id,
                        })
                        .done(function () {
                            getLedgerEntries(true);
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

        $('#CreateNewLedgerEntryButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _ledgerEntriesService
                .getLedgerEntriesToExcel({
                    filter: $('#LedgerEntriesTableFilter').val(),
                    nameFilter: $('#NameFilterId').val(),
                    serviceTypeFilter: $('#ServiceTypeFilterId').val(),
                    minAmountFilter: $('#MinAmountFilterId').val(),
                    maxAmountFilter: $('#MaxAmountFilterId').val(),
                    minDiscountAmountFilter: $('#MinDiscountAmountFilterId').val(),
                    maxDiscountAmountFilter: $('#MaxDiscountAmountFilterId').val(),
                    minTotalPriceFilter: $('#MinTotalPriceFilterId').val(),
                    maxTotalPriceFilter: $('#MaxTotalPriceFilterId').val(),
                    paymentPeriodTypeFilter: $('#PaymentPeriodTypeFilterId').val(),
                    minExpirationDateFilter: getDateFilter($('#MinExpirationDateFilterId')),
                    maxExpirationDateFilter: getMaxDateFilter($('#MaxExpirationDateFilterId')),
                    transactionNameFilter: $('#TransactionNameFilterId').val(),
                    transactionKeyFilter: $('#TransactionKeyFilterId').val(),
                    transactionIdFilter: $('#TransactionIdFilterId').val(),
                    settledFilter: $('#SettledFilterId').val(),
                    minAmountDueFilter: $('#MinAmountDueFilterId').val(),
                    maxAmountDueFilter: $('#MaxAmountDueFilterId').val(),
                    paymentTokenFilter: $('#PaymentTokenFilterId').val(),
                    authNetCustomerProfileIdFilter: $('#AuthNetCustomerProfileIdFilterId').val(),
                    authNetCustomerPaymentProfileIdFilter: $('#AuthNetCustomerPaymentProfileIdFilterId').val(),
                    authNetCustomerAddressIdFilter: $('#AuthNetCustomerAddressIdFilterId').val(),
                    accountNumberFilter: $('#AccountNumberFilterId').val(),
                    noteFilter: $('#NoteFilterId').val(),
                    metaDataFilter: $('#MetaDataFilterId').val(),
                    authCodeFilter: $('#AuthCodeFilterId').val(),
                    referenceTransactionIdFilter: $('#ReferenceTransactionIdFilterId').val(),
                    transactionHashFilter: $('#TransactionHashFilterId').val(),
                    accountTypeFilter: $('#AccountTypeFilterId').val(),
                    transactionCodeFilter: $('#TransactionCodeFilterId').val(),
                    transactionMessageFilter: $('#TransactionMessageFilterId').val(),
                    authNetTransHashSha2Filter: $('#AuthNetTransHashSha2FilterId').val(),
                    authNetNetworkTransIdFilter: $('#AuthNetNetworkTransIdFilterId').val(),
                    minPaidAmountFilter: $('#MinPaidAmountFilterId').val(),
                    maxPaidAmountFilter: $('#MaxPaidAmountFilterId').val(),
                    minPaidInCashFilter: $('#MinPaidInCashFilterId').val(),
                    maxPaidInCashFilter: $('#MaxPaidInCashFilterId').val(),
                    minAvailableUserBalanceFilter: $('#MinAvailableUserBalanceFilterId').val(),
                    maxAvailableUserBalanceFilter: $('#MaxAvailableUserBalanceFilterId').val(),
                    cardNameOnCardFilter: $('#CardNameOnCardFilterId').val(),
                    cardZipCodeFilter: $('#CardZipCodeFilterId').val(),
                    minBalanceForwardFilter: $('#MinBalanceForwardFilterId').val(),
                    maxBalanceForwardFilter: $('#MaxBalanceForwardFilterId').val(),
                    userNameFilter: $('#UserNameFilterId').val(),
                    tenantDocumentNameFilter: $('#TenantDocumentNameFilterId').val(),
                    cohortNameFilter: $('#CohortNameFilterId').val(),
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditLedgerEntryModalSaved', function () {
            getLedgerEntries();
        });

        $('#GetLedgerEntriesButton').click(function (e) {
            e.preventDefault();
            getLedgerEntries();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getLedgerEntries();
            }
        });

        $('.reload-on-change').change(function (e) {
            getLedgerEntries();
        });

        $('.reload-on-keyup').keyup(function (e) {
            getLedgerEntries();
        });

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getLedgerEntries();
        });

        var currentOpenedDetailRow;
        function openDetailRow(e, url) {
            var tr = $(e).closest('tr');
            var row = dataTable.row(tr);

            if (row.child.isShown()) {
                row.child.hide();
                tr.removeClass('shown');
                currentOpenedDetailRow = null;
            } else {
                if (currentOpenedDetailRow) currentOpenedDetailRow.child.hide();

                $.get(url).then((data) => {
                    row.child(data).show();
                    tr.addClass('shown');
                    currentOpenedDetailRow = row;
                });
            }
        }

        _$ledgerEntriesTable.on('click', '.Edit_LedgerEntryDetail_LedgerEntryId', function () {
            var tr = $(this).closest('tr');
            var row = dataTable.row(tr);
            openDetailRow(
                this,
                '/App/MasterDetailChild_LedgerEntry_LedgerEntryDetails/UserLedgerDetails?LedgerEntryId=' + row.data().ledgerEntry.id
            );
        });
    });
})();
