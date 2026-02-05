(function () {
    $(function () {
        var _$tenantDocumentCategoriesTable = $('#TenantDocumentCategoriesTable');
        var _tenantDocumentCategoriesService = abp.services.app.tenantDocumentCategories;
        var _entityTypeFullName = 'inzibackend.Surpath.TenantDocumentCategory';

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
            create: abp.auth.hasPermission('Pages.TenantDocumentCategories.Create'),
            edit: abp.auth.hasPermission('Pages.TenantDocumentCategories.Edit'),
            delete: abp.auth.hasPermission('Pages.TenantDocumentCategories.Delete'),
        };

        var _viewTenantDocumentCategoryModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/TenantDocumentCategories/ViewtenantDocumentCategoryModal',
            modalClass: 'ViewTenantDocumentCategoryModal',
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

        var _LinkClasses = 'text-gray-800 text-hover-primary d-flex flex-column';
        var _SymbolDivClass = 'symbol symbol-60px mb-5';
        var _IconClass = 'fa fa-folder-open text-primary fa-4x';


        var dataTable = _$tenantDocumentCategoriesTable.DataTable({
            paging: true,
            lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
            pageLength: 5000,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _tenantDocumentCategoriesService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#TenantDocumentCategoriesTableFilter').val(),
                        nameFilter: $('#NameFilterId').val(),
                        descriptionFilter: $('#DescriptionFilterId').val(),
                        authorizedOnlyFilter: $('#AuthorizedOnlyFilterId').val(),
                        hostOnlyFilter: $('#HostOnlyFilterId').val(),
                        userNameFilter: $('#UserNameFilterId').val(),
                    };
                },
            },
            columnDefs: [
                {
                  //className: 'control responsive',
                  className: 'd-none',
                  orderable: false,
                  render: function () {
                    return '';
                  },
                  targets: 0,
                },
                {
                    targets: 1,
                    data: null, //'tenantDocumentCategory.name',
                    name: 'name',
                    className: 'surpath-lib-cat-icon text-center',
                    render: function (data) {
                        //debugger;
                        var _$a = $('<a />').addClass(_LinkClasses);
                        var _$d = $('<div/>').addClass(_SymbolDivClass);
                        var _$i = $('<i/>').addClass(_IconClass);
                        // <a href="\App\TenantDocuments\CreateOrEdit?catid=@Model.DocumentCategoryId" class="btn btn-primary blue"><i class="fa fa-plus"></i > @L("CreateNewTenantDocument") </a >
                        _$a.attr('href', '/App/TenantDocuments?catid=' + data.tenantDocumentCategory.id);
                        _$d.append(_$i);
                        _$a.append(_$d);
                        console.log(_$a);
                        return _$a[0].outerHTML;
                        //console.log(_$d);
                        //return _$d[0].outerHTML;


                    },
                },
                {
                    targets: 2,
                    className: 'text-center',
                    data: 'tenantDocumentCategory.description',
                    name: 'description',
                },
                {
                    targets: 3,
                    className: 'cards-show-label',
                    data: 'userName',
                    name: 'userFk.name',
                },
                {
                    targets: 4,
                    data: 'tenantDocumentCategory.authorizedOnly',
                    name: 'authorizedOnly',
                    className: 'cards-show-label',
                    render: function (authorizedOnly) {
                        if (authorizedOnly) {
                            return '<div class="text-center surpath-table-card-icon-wrapper"><i class="fa fa-check text-success" title="True"></i></div>';
                        }
                        return '<div class="text-center surpath-table-card-icon-wrapper"><i class="fa fa-times-circle" title="False"></i></div>';
                    },
                },
                {
                    targets: 5,
                    data: 'tenantDocumentCategory.hostOnly',
                    name: 'hostOnly',
                    className: 'cards-show-label',
                    render: function (hostOnly) {
                        if (hostOnly) {
                            return '<div class="text-center surpath-table-card-icon-wrapper"><i class="fa fa-check text-success" title="True"></i></div>';
                        }
                        return '<div class="text-center surpath-table-card-icon-wrapper"><i class="fa fa-times-circle" title="False"></i></div>';
                    },
                },
                {
                    width: 120,
                    targets: 6,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    className: 'text-center',

                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
                            {
                                text: app.localize('View'),
                                iconStyle: 'far fa-eye mr-2',
                                action: function (data) {
                                    window.location =
                                        '/App/TenantDocumentCategories/ViewTenantDocumentCategory/' + data.record.tenantDocumentCategory.id;
                                },
                            },
                            {
                                text: app.localize('Edit'),
                                iconStyle: 'far fa-edit mr-2',
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    window.location =
                                        '/App/TenantDocumentCategories/CreateOrEdit/' + data.record.tenantDocumentCategory.id;
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
                                        entityId: data.record.tenantDocumentCategory.id,
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
                                    deleteTenantDocumentCategory(data.record.tenantDocumentCategory);
                                },
                            },
                        ],
                    },
                },
            ],
            createdRow: function (row, data, index) {
                //console.log(row);
                //$(row).addClass("text-center");
                //console.log(row);
            },
            drawCallback: function (settings) {
                console.log('drawCallback');
                //console.log(settings);
                var api = this.api();
                var $table = $(api.table().node());
                //var $table = $('#TenantDocumentCategoriesTable')[0];
                //debugger;

                var labels = [];
                $('thead th', $table).each(function () {
                    labels.push($(this).text());
                });

                console.log(labels);

                $('tbody tr', $table).each(function () {
                    $(this).find('td').each(function (column) {
                        console.log(this);
                        if ($(this).hasClass('cards-show-label')) {
                            $(this).attr('data-label', labels[column]);
                        }                        
                    });
                });

                var max = 0;
                $('tbody tr', $table).each(function () {
                    max = Math.max($(this).height(), max);             
                }).height(max);
                /*$('tbody tr', $table).height(max);*/
            }
        });

        function getTenantDocumentCategories() {
            dataTable.ajax.reload();
        }

        function deleteTenantDocumentCategory(tenantDocumentCategory) {
            abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
                if (isConfirmed) {
                    _tenantDocumentCategoriesService
                        .delete({
                            id: tenantDocumentCategory.id,
                        })
                        .done(function () {
                            getTenantDocumentCategories(true);
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
            _tenantDocumentCategoriesService
                .getTenantDocumentCategoriesToExcel({
                    filter: $('#TenantDocumentCategoriesTableFilter').val(),
                    nameFilter: $('#NameFilterId').val(),
                    descriptionFilter: $('#DescriptionFilterId').val(),
                    authorizedOnlyFilter: $('#AuthorizedOnlyFilterId').val(),
                    hostOnlyFilter: $('#HostOnlyFilterId').val(),
                    userNameFilter: $('#UserNameFilterId').val(),
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditTenantDocumentCategoryModalSaved', function () {
            getTenantDocumentCategories();
        });

        $('#GetTenantDocumentCategoriesButton').click(function (e) {
            e.preventDefault();
            getTenantDocumentCategories();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getTenantDocumentCategories();
            }
        });
    });
})();
