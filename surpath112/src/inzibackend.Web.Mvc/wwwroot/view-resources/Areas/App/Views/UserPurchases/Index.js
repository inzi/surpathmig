(function () {
    $(function () {
        var _userPurchasesService = abp.services.app.userPurchases;
        var _$userPurchasesTable = $('#UserPurchasesTable');
        var _$userPurchasesTableFilter = $('#UserPurchasesTableFilter');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.UserPurchases.Create'),
            edit: abp.auth.hasPermission('Pages.UserPurchases.Edit'),
            delete: abp.auth.hasPermission('Pages.UserPurchases.Delete'),
            applyPayment: abp.auth.hasPermission('Pages.UserPurchases.ApplyPayment'),
            adjustPrice: abp.auth.hasPermission('Pages.UserPurchases.AdjustPrice')
        };

        var _userIdFilter = $('#hiddenUserIdFilter').val();

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/UserPurchases/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/UserPurchases/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditUserPurchaseModal'
        });

        var _viewUserPurchaseModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/UserPurchases/ViewUserPurchaseModal',
            modalClass: 'ViewUserPurchaseModal'
        });

        var _applyPaymentModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/UserPurchases/ApplyPaymentModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/UserPurchases/_ApplyPaymentModal.js',
            modalClass: 'ApplyPaymentModal'
        });

        var _adjustPriceModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/UserPurchases/AdjustPriceModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/UserPurchases/_AdjustPriceModal.js',
            modalClass: 'AdjustPriceModal'
        });

        var getDateFilter = function (element) {
            if (element.val() == '') {
                return null;
            }
            return element.data('daterangepicker').startDate.format('YYYY-MM-DDT00:00:00Z');
        };

        var getMaxDateFilter = function (element) {
            if (element.val() == '') {
                return null;
            }
            return element.data('daterangepicker').startDate.format('YYYY-MM-DDT23:59:59Z');
        };

        var dataTable = _$userPurchasesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _userPurchasesService.getAll,
                inputFilter: function () {
                    return {
                        filter: _$userPurchasesTableFilter.val(),
                        nameFilter: $('#NameFilterId').val(),
                        descriptionFilter: $('#DescriptionFilterId').val(),
                        statusFilter: $('#StatusFilterId').val(),
                        minOriginalPriceFilter: $('#MinOriginalPriceFilterId').val(),
                        maxOriginalPriceFilter: $('#MaxOriginalPriceFilterId').val(),
                        minAdjustedPriceFilter: $('#MinAdjustedPriceFilterId').val(),
                        maxAdjustedPriceFilter: $('#MaxAdjustedPriceFilterId').val(),
                        minDiscountAmountFilter: $('#MinDiscountAmountFilterId').val(),
                        maxDiscountAmountFilter: $('#MaxDiscountAmountFilterId').val(),
                        minAmountPaidFilter: $('#MinAmountPaidFilterId').val(),
                        maxAmountPaidFilter: $('#MaxAmountPaidFilterId').val(),
                        paymentPeriodTypeFilter: $('#PaymentPeriodTypeFilterId').val(),
                        minPurchaseDateFilter: getDateFilter($('#MinPurchaseDateFilterId')),
                        maxPurchaseDateFilter: getMaxDateFilter($('#MaxPurchaseDateFilterId')),
                        minExpirationDateFilter: getDateFilter($('#MinExpirationDateFilterId')),
                        maxExpirationDateFilter: getMaxDateFilter($('#MaxExpirationDateFilterId')),
                        isRecurringFilter: $('#IsRecurringFilterId').val(),
                        notesFilter: $('#NotesFilterId').val(),
                        userNameFilter: $('#UserNameFilterId').val(),
                        surpathServiceNameFilter: $('#SurpathServiceNameFilterId').val(),
                        tenantSurpathServiceNameFilter: $('#TenantSurpathServiceNameFilterId').val(),
                        cohortNameFilter: $('#CohortNameFilterId').val(),
                        userIdFilter: _userIdFilter
                    };
                }
            },
            columnDefs: [
                {
                    className: 'dtr-control responsive',
                    orderable: false,
                    render: function () {
                        return '';
                    },
                    targets: 0
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
                                    _viewUserPurchaseModal.open({ id: data.record.userPurchase.id });
                                }
                            },
                            {
                                text: app.localize('ApplyPayment'),
                                visible: function (data) {
                                    return _permissions.applyPayment;
                                },
                                action: function (data) {
                                    _applyPaymentModal.open({ id: data.record.userPurchase.id });
                                }
                            },
                            {
                                text: app.localize('AdjustPrice'),
                                visible: function (data) {
                                    return _permissions.adjustPrice;
                                },
                                action: function (data) {
                                    _adjustPriceModal.open({ id: data.record.userPurchase.id });
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function (data) {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({ id: data.record.userPurchase.id });
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteUserPurchase(data.record.userPurchase);
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 2,
                    data: "userPurchase.name",
                    name: "name"
                },
                {
                    targets: 3,
                    data: "userPurchase.description",
                    name: "description"
                },
                {
                    targets: 4,
                    data: "userPurchase.status",
                    name: "status",
                    render: function (status) {
                        return app.localize('Enum_EnumPurchaseStatus_' + status);
                    }
                },
                {
                    targets: 5,
                    data: "userPurchase.originalPrice",
                    name: "originalPrice",
                    render: function (price) {
                        return '$' + price.toFixed(2);
                    }
                },
                {
                    targets: 6,
                    data: "userPurchase.adjustedPrice",
                    name: "adjustedPrice",
                    render: function (price) {
                        return '$' + price.toFixed(2);
                    }
                },
                {
                    targets: 7,
                    data: "userPurchase.discountAmount",
                    name: "discountAmount",
                    render: function (price) {
                        return '$' + price.toFixed(2);
                    }
                },
                {
                    targets: 8,
                    data: "userPurchase.amountPaid",
                    name: "amountPaid",
                    render: function (price) {
                        return '$' + price.toFixed(2);
                    }
                },
                {
                    targets: 9,
                    data: "userPurchase.balanceDue",
                    name: "balanceDue",
                    render: function (price) {
                        var color = price > 0 ? 'danger' : price < 0 ? 'success' : 'secondary';
                        return '<span class="text-' + color + '">$' + price.toFixed(2) + '</span>';
                    }
                },
                {
                    targets: 10,
                    data: "userPurchase.paymentPeriodType",
                    name: "paymentPeriodType",
                    render: function (type) {
                        return app.localize('Enum_PaymentPeriodType_' + type);
                    }
                },
                {
                    targets: 11,
                    data: "userPurchase.purchaseDate",
                    name: "purchaseDate",
                    render: function (date) {
                        if (date) {
                            return moment(date).format('L');
                        }
                        return '';
                    }
                },
                {
                    targets: 12,
                    data: "userPurchase.expirationDate",
                    name: "expirationDate",
                    render: function (date) {
                        if (date) {
                            return moment(date).format('L');
                        }
                        return '';
                    }
                },
                {
                    targets: 13,
                    data: "userPurchase.isRecurring",
                    name: "isRecurring",
                    render: function (isRecurring) {
                        if (isRecurring) {
                            return '<span class="badge badge-success">' + app.localize('Yes') + '</span>';
                        }
                        return '<span class="badge badge-dark">' + app.localize('No') + '</span>';
                    }
                },
                {
                    targets: 14,
                    data: "userName",
                    name: "userFk.name"
                },
                {
                    targets: 15,
                    data: "surpathServiceName",
                    name: "surpathServiceFk.name"
                },
                {
                    targets: 16,
                    data: "tenantSurpathServiceName",
                    name: "tenantSurpathServiceFk.name"
                },
                {
                    targets: 17,
                    data: "cohortName",
                    name: "cohortFk.name"
                }
            ]
        });

        function getUserPurchases() {
            dataTable.ajax.reload();
        }

        function deleteUserPurchase(userPurchase) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _userPurchasesService.delete({
                            id: userPurchase.id
                        }).done(function () {
                            getUserPurchases();
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

        $('#CreateNewUserPurchaseButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _userPurchasesService
                .getUserPurchasesToExcel({
                    filter: _$userPurchasesTableFilter.val(),
                    nameFilter: $('#NameFilterId').val(),
                    descriptionFilter: $('#DescriptionFilterId').val(),
                    statusFilter: $('#StatusFilterId').val(),
                    minOriginalPriceFilter: $('#MinOriginalPriceFilterId').val(),
                    maxOriginalPriceFilter: $('#MaxOriginalPriceFilterId').val(),
                    minAdjustedPriceFilter: $('#MinAdjustedPriceFilterId').val(),
                    maxAdjustedPriceFilter: $('#MaxAdjustedPriceFilterId').val(),
                    minDiscountAmountFilter: $('#MinDiscountAmountFilterId').val(),
                    maxDiscountAmountFilter: $('#MaxDiscountAmountFilterId').val(),
                    minAmountPaidFilter: $('#MinAmountPaidFilterId').val(),
                    maxAmountPaidFilter: $('#MaxAmountPaidFilterId').val(),
                    paymentPeriodTypeFilter: $('#PaymentPeriodTypeFilterId').val(),
                    minPurchaseDateFilter: getDateFilter($('#MinPurchaseDateFilterId')),
                    maxPurchaseDateFilter: getMaxDateFilter($('#MaxPurchaseDateFilterId')),
                    minExpirationDateFilter: getDateFilter($('#MinExpirationDateFilterId')),
                    maxExpirationDateFilter: getMaxDateFilter($('#MaxExpirationDateFilterId')),
                    isRecurringFilter: $('#IsRecurringFilterId').val(),
                    notesFilter: $('#NotesFilterId').val(),
                    userNameFilter: $('#UserNameFilterId').val(),
                    surpathServiceNameFilter: $('#SurpathServiceNameFilterId').val(),
                    tenantSurpathServiceNameFilter: $('#TenantSurpathServiceNameFilterId').val(),
                    cohortNameFilter: $('#CohortNameFilterId').val(),
                    userIdFilter: _userIdFilter
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetUserPurchasesButton').click(function (e) {
            e.preventDefault();
            getUserPurchases();
        });

        $('#btn-reset-filters').click(function () {
            $('.reload-on-change').val('');
            $('.reload-on-keyup').val('');
            $('.date-picker').val('');
            getUserPurchases();
        });

        abp.event.on('app.createOrEditUserPurchaseModalSaved', function () {
            getUserPurchases();
        });

        abp.event.on('app.applyPaymentModalSaved', function () {
            getUserPurchases();
        });

        abp.event.on('app.adjustPriceModalSaved', function () {
            getUserPurchases();
        });

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

        $('.date-picker').daterangepicker({
            singleDatePicker: true,
            locale: {
                format: 'MM/DD/YYYY'
            }
        });

        app.daterangefilterhelper.fixfilters();

        $('.reload-on-change').change(function (e) {
            getUserPurchases();
        });

        $('.reload-on-keyup').keyup(function (e) {
            getUserPurchases();
        });

        $('#unionTypeFilter').change(function () {
            getUserPurchases();
        });

        // Create a hidden field to store the user ID from the URL if available
        if (typeof getUrlParameter === 'function' && getUrlParameter('userId')) {
            _userIdFilter = getUrlParameter('userId');
            $('<input>').attr({
                type: 'hidden',
                id: 'hiddenUserIdFilter',
                value: _userIdFilter
            }).appendTo('body');
        }
    });
})();