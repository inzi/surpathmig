(function () {
    $(function () {

        var _refreshTimeout = null;
        var _serviceCountCache = new Map();
        var _tenantServicesData = [];
        var _serviceCountMap = new Map();

        var _tenantService = abp.services.app.tenant;
        var _$tenantsTable = $('#TenantsTable');
        var _$tenantsTableFilter = $('#TenantsTableFilter');
        var _$tenantsFormFilter = $('#TenantsFormFilter');
        var _$subscriptionEndDateRangeActive = $('#TenantsTable_SubscriptionEndDateRangeActive');
        var _$subscriptionEndDateRange = _$tenantsFormFilter.find("input[name='SubscriptionEndDateRange']");
        var _$creationDateRangeActive = $('#TenantsTable_CreationDateRangeActive');
        var _$creationDateRange = _$tenantsFormFilter.find("input[name='CreationDateRange']");
        var _$refreshButton = _$tenantsFormFilter.find("button[name='RefreshButton']");
        var _$editionDropdown = _$tenantsFormFilter.find('#EditionDropdown');
        var _entityTypeFullName = 'inzibackend.MultiTenancy.Tenant';

        var _$surpathServicesTable = $('#SurpathServicesTable');
        var _surpathServicesService = abp.services.app.surpathServices;
        var _entityTypeFullName = 'inzibackend.Surpath.SurpathService';

        var _$tenantSurpathServicesTable = $('#TenantSurpathServicesTable');
        var _tenantSurpathServicesService = abp.services.app.tenantSurpathServices;
        var _entityTypeFullName = 'inzibackend.Surpath.TenantSurpathService';

        var _selectedTenantId = '';
        var _$tenantsTableContainer = $('#manage_tenants_collapsible')

        var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();
        
       
        
        

        // Create a debounced refresh function
        function debouncedRefresh() {
            // Clear any existing timeout
            if (_refreshTimeout) {
                clearTimeout(_refreshTimeout);
            }

            // Set new timeout
            _refreshTimeout = setTimeout(function() {
                if (_selectedTenantId) { // Only refresh if a tenant is selected
                    getTenantSurpathServices();
                }
            }, 5000); // 5000ms = 5 seconds
        }


        function updateServiceCounts() {
            _serviceCountMap.clear();
            _tenantServicesData.forEach(item => {
                // Add null check and logging for debugging
                if (!item || !item.surpathService) {
                    console.warn('Invalid item in _tenantServicesData:', item);
                    return; // Skip this item
                }
                
                const key = `${item.surpathService.name}_${item.tenantId}`;
                _serviceCountMap.set(key, 
                    (_serviceCountMap.get(key) || 0) + 1
                );
            });
        }

        // Function to check if service can be deleted
        function canDeleteService(serviceName, tenantId) {
            const key = `${serviceName}_${tenantId}`;
            return (_serviceCountMap.get(key) || 0) > 1;
        }

        // Function to refresh table without server call
        function refreshTableFromLocalData() {
            // console.log('Refreshing table with data:', _tenantServicesData);
            
            try {
                dataTableTenantSurpathServices.clear();
                dataTableTenantSurpathServices.rows.add(_tenantServicesData);
                dataTableTenantSurpathServices.draw();
                
                // console.log('Table refresh completed');
            } catch (error) {
                console.error('Error refreshing table:', error);
            }
        }

        var _permissionsTenant = {
            create: abp.auth.hasPermission('Pages.Tenants.Create'),
            edit: abp.auth.hasPermission('Pages.Tenants.Edit'),
            changeFeatures: abp.auth.hasPermission('Pages.Tenants.ChangeFeatures'),
            impersonation: abp.auth.hasPermission('Pages.Tenants.Impersonation'),
            delete: abp.auth.hasPermission('Pages.Tenants.Delete'),
        };

        var _urlParams = {
            creationDateStart: $.url('?creationDateStart'),
            creationDateEnd: $.url('?creationDateEnd'),
            subscriptionEndDateStart: $.url('?subscriptionEndDateStart'),
            subscriptionEndDateEnd: $.url('?subscriptionEndDateEnd'),
        };


        var _editModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Tenants/EditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Tenants/_EditModal.js',
            modalClass: 'EditTenantModal',
        });

        var _featuresModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Tenants/FeaturesModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Tenants/_FeaturesModal.js',
            modalClass: 'TenantFeaturesModal',
        });


        var getFilter = function () {
            var editionId = _$editionDropdown.find(':selected').val();

            var filter = {
                filter: _$tenantsTableFilter.val(),
                editionId: editionId,
                editionIdSpecified: false,
            };
            return filter;
        };

        function entityHistoryIsEnabledTenants() {
            return (
                abp.custom.EntityHistory &&
                abp.custom.EntityHistory.IsEnabled &&
                _.filter(abp.custom.EntityHistory.EnabledEntities, function (entityType) {
                    return entityType === _entityTypeFullName;
                }).length === 1
            );
        }

        var dataTableTenants = _$tenantsTable.DataTable({
            paging: true,
            lengthMenu: [5, 10, 25, 50, 100, 250, 500],
            pageLength: 500,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _tenantService.getTenants,
                inputFilter: function () {
                    return getFilter();
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
                    targets: 1,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        text:
                            '<i class="fa fa-cog"></i> <span class="d-none d-md-inline-block d-lg-inline-block d-xl-inline-block">' +
                            app.localize('Actions') +
                            '</span> <span class="caret"></span>',
                        items: [
                            {
                                text: app.localize('Features'),
                                visible: function () {
                                    return _permissionsTenant.changeFeatures;
                                },
                                action: function (data) {
                                    _featuresModal.open({ id: data.record.id }, function () {
                                        getTenantSurpathServices();
                                    });
                                },
                            },

                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissionsTenant.edit;
                                },
                                action: function (data) {
                                    _editModal.open({ id: data.record.id });
                                },
                            },
                            {
                                text: app.localize('History'),
                                visible: function () {
                                    return entityHistoryIsEnabledTenants();
                                },
                                action: function (data) {
                                    _entityTypeHistoryModal.open({
                                        entityTypeFullName: _entityTypeFullName,
                                        entityId: data.record.id,
                                        entityTypeDescription: data.record.name,
                                    });
                                },
                            },
                        ],
                    },
                },
                {
                    targets: 2,
                    data: 'tenancyName',
                },
                {
                    targets: 3,
                    data: 'name',
                },
                
            ],
            createdRow: function (row, data, dataIndex) {

                $(row).attr('data-tenancyname', data.tenancyName);
                $(row).attr('data-tenantname', data.name);
                $(row).attr('data-tenantid', data.id);
                $(row).addClass('cursor-pointer');
                $(row).on('click', TenantRowClick);

            },
           
        });

        function getQueryStringParameter(name) {
            var uri = URI.parseQuery(document.location.href);
            return uri[name];
        }

        function getTenants() {
            dataTableTenants.ajax.reload();
        }

        function deleteTenant(tenant) {
            abp.message.confirm(
                app.localize('TenantDeleteWarningMessage', tenant.tenancyName),
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _tenantService
                            .deleteTenant({
                                id: tenant.id,
                            })
                            .done(function () {
                                getTenants();
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }

        //$('#CreateNewTenantButton').click(function () {
        //    _createModal.open();
        //});

        //$('#NewTenantWizard').on('click', function () {
        //    window.location = '/App/Tenants/NewTenantWizard';
        //});

        $('#GetTenantsButton').click(function (e) {
            e.preventDefault();
            getTenants();
        });

        abp.event.on('app.editTenantModalSaved', function () {
            getTenants(true);
        });

        abp.event.on('app.createTenantModalSaved', function () {
            getTenants(true);
        });

        _$subscriptionEndDateRangeActive.change(function () {
            _$subscriptionEndDateRange.prop('disabled', !$(this).prop('checked'));
        });

        if (_urlParams.subscriptionEndDateStart || _urlParams.subscriptionEndDateEnd) {
            _$subscriptionEndDateRangeActive.prop('checked', true);
        } else {
            _$subscriptionEndDateRange.prop('disabled', true);
        }

        _$creationDateRangeActive.change(function () {
            _$creationDateRange.prop('disabled', !$(this).prop('checked'));
        });

        if (_urlParams.creationDateStart || _urlParams.creationDateEnd) {
            _$creationDateRangeActive.prop('checked', true);
        } else {
            _$creationDateRange.prop('disabled', true);
        }

        _$refreshButton.click(function (e) {
            e.preventDefault();
            getTenants();
        });

        _$tenantsTableFilter.focus();

        //#endregion

        //#region Services


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
                getSurpathServices();
            })
            .on('cancel.daterangepicker', function (ev, picker) {
                $(this).val('');
                $selectedDate.startDate = null;
                getSurpathServices();
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
                getSurpathServices();
            })
            .on('cancel.daterangepicker', function (ev, picker) {
                $(this).val('');
                $selectedDate.endDate = null;
                getSurpathServices();
            });

        var _permissionsSurpathServices = {
            create: abp.auth.hasPermission('Pages.SurpathServices.Create'),
            edit: abp.auth.hasPermission('Pages.SurpathServices.Edit'),
            delete: abp.auth.hasPermission('Pages.SurpathServices.Delete'),
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/SurpathServices/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/SurpathServices/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditSurpathServiceModal',
        });

        var _viewSurpathServiceModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/SurpathServices/ViewsurpathServiceModal',
            modalClass: 'ViewSurpathServiceModal',
        });

        //var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();
        function entityHistoryIsEnabledTenants() {
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

        var dataTableSurpathServices = _$surpathServicesTable.DataTable({
            paging: true,
            lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
            pageLength: 5000,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _surpathServicesService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#SurpathServicesTableFilter').val(),
                        nameFilter: $('#NameFilterId').val(),
                        minPriceFilter: $('#MinPriceFilterId').val(),
                        maxPriceFilter: $('#MaxPriceFilterId').val(),
                        minDiscountFilter: $('#MinDiscountFilterId').val(),
                        maxDiscountFilter: $('#MaxDiscountFilterId').val(),
                        descriptionFilter: $('#DescriptionFilterId').val(),
                        isEnabledByDefaultFilter: $('#IsEnabledByDefaultFilterId').val(),
                        tenantDepartmentNameFilter: $('#TenantDepartmentNameFilterId').val(),
                        cohortNameFilter: $('#CohortNameFilterId').val(),
                        userNameFilter: $('#UserNameFilterId').val(),
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
                    visible: false,
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
                            //{
                            //    text: app.localize('View'),
                            //    action: function (data) {
                            //        _viewSurpathServiceModal.open({ id: data.record.surpathService.id });
                            //    },
                            //},
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissionsSurpathServices.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({ id: data.record.surpathService.id, tenantId: _selectedTenantId });
                                },
                            },
                            
                            
                            {
                                text: app.localize('History'),
                                iconStyle: 'fas fa-history mr-2',
                                visible: function () {
                                    return entityHistoryIsEnabledTenants();
                                },
                                action: function (data) {
                                    _entityTypeHistoryModal.open({
                                        entityTypeFullName: _entityTypeFullName,
                                        entityId: data.record.surpathService.id,
                                    });
                                },
                            },
                            //{
                            //    text: app.localize('Delete'),
                            //    visible: function () {
                            //        return _permissionsSurpathServices.delete;
                            //    },
                            //    action: function (data) {
                            //        deleteSurpathService(data.record.surpathService);
                            //    },
                            //},
                        ],
                    },
                },
                {
                    targets: 2,
                    data: 'surpathService.name',
                    name: 'name',
                },
                {
                    targets: 3,
                    data: 'surpathService.price',
                    name: 'price',
                },
                {
                    targets: 4,
                    data: 'surpathService.discount',
                    name: 'discount',
                },
                {
                    targets: 5,
                    data: 'surpathService.description',
                    name: 'description',
                },
                {
                    targets: 6,
                    data: 'surpathService.isEnabledByDefault',
                    name: 'isEnabledByDefault',
                    render: function (isEnabledByDefault) {
                        if (isEnabledByDefault) {
                            return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
                        }
                        return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
                    },
                },
                //{
                //    targets: 7,
                //    data: 'tenantDepartmentName',
                //    name: 'tenantDepartmentFk.name',
                //},
                //{
                //    targets: 8,
                //    data: 'cohortName',
                //    name: 'cohortFk.name',
                //},
                //{
                //    targets: 9,
                //    data: 'userName',
                //    name: 'userFk.name',
                //},
            ],
        });

        function getSurpathServices() {
            dataTableSurpathServices.ajax.reload();
        }

        function deleteSurpathService(surpathService) {
            abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
                if (isConfirmed) {
                    _surpathServicesService
                        .delete({
                            id: surpathService.id,
                        })
                        .done(function () {
                            getSurpathServices(true);
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

        $('#CreateNewSurpathServiceButton').click(function () {
            _createOrEditModal.open({tenantId: _selectedTenantId});
        });

        $('#ExportToExcelButton').click(function () {
            _surpathServicesService
                .getSurpathServicesToExcel({
                    filter: $('#SurpathServicesTableFilter').val(),
                    nameFilter: $('#NameFilterId').val(),
                    minPriceFilter: $('#MinPriceFilterId').val(),
                    maxPriceFilter: $('#MaxPriceFilterId').val(),
                    minDiscountFilter: $('#MinDiscountFilterId').val(),
                    maxDiscountFilter: $('#MaxDiscountFilterId').val(),
                    descriptionFilter: $('#DescriptionFilterId').val(),
                    isEnabledByDefaultFilter: $('#IsEnabledByDefaultFilterId').val(),
                    tenantDepartmentNameFilter: $('#TenantDepartmentNameFilterId').val(),
                    cohortNameFilter: $('#CohortNameFilterId').val(),
                    userNameFilter: $('#UserNameFilterId').val(),
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditSurpathServiceModalSaved', function () {
            getSurpathServices();
        });

        $('#GetSurpathServicesButton').click(function (e) {
            e.preventDefault();
            getSurpathServices();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getSurpathServices();
            }
        });

        $('.reload-on-change').change(function (e) {
            getSurpathServices();
        });

        $('.reload-on-keyup').keyup(function (e) {
            getSurpathServices();
        });

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getSurpathServices();
        });

        //#endregion 


        //#region TenantSurpathServices


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
                getTenantSurpathServices();
            })
            .on('cancel.daterangepicker', function (ev, picker) {
                $(this).val('');
                $selectedDate.startDate = null;
                getTenantSurpathServices();
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
                getTenantSurpathServices();
            })
            .on('cancel.daterangepicker', function (ev, picker) {
                $(this).val('');
                $selectedDate.endDate = null;
                getTenantSurpathServices();
            });

        var _permissionsTeantSurpathServices = {
            create: abp.auth.hasPermission('Pages.TenantSurpathServices.Create'),
            edit: abp.auth.hasPermission('Pages.TenantSurpathServices.Edit'),
            delete: abp.auth.hasPermission('Pages.TenantSurpathServices.Delete'),
        };

        var _createOrEditModalSPS = new app.ModalManager({
            viewUrl: abp.appPath + 'App/TenantSurpathServices/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/TenantSurpathServices/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditTenantSurpathServiceModal',
        });

        var _viewTenantSurpathServiceModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/TenantSurpathServices/ViewtenantSurpathServiceModal',
            modalClass: 'ViewTenantSurpathServiceModal',
        });

        //var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();

        function entityHistoryIsEnabledTenantSurpathServices() {
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

        var dataTableTenantSurpathServices = _$tenantSurpathServicesTable.DataTable({
            paging: true,
            lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
            pageLength: 5000,
            serverSide: false,
            processing: true,
            listAction: {
                ajaxFunction: _tenantSurpathServicesService.getAllTenantServices,
                inputFilter: function () {

                    console.log('dataTableTenantSurpathServices listaction');
                    return {
                        filter: $('#TenantSurpathServicesTableFilter').val(),
                        isEnabledFilter: $('#IsEnabledFilterId').val(),
                        surpathServiceNameFilter: $('#SurpathServiceNameFilterId').val(),
                        recordCategoryRuleNameFilter: $('#RecordCategoryRuleNameFilterId').val(),
                        tenantId: _selectedTenantId
                    };
                },
            },
            columnDefs: [
                {
                    className: 'control responsive',
                    orderable: false,
                    visible: false,
                    render: function () {
                        return '';;
                    },
                    targets: 0,
                    width: '20px'
                },
                {
                    width: '80px',
                    targets: 1,
                    visible: abp.auth.hasPermission('Pages.Administration.Host.Dashboard'),
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',

                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissionsTeantSurpathServices.edit;
                                },
                                action: function (data) {
                                    console.log('edit action', data, _selectedTenantId);
                                    _createOrEditModalSPS.open({ id: data.record.surpathService.id, tenantId: _selectedTenantId });
                                },
                            },
                            {
                                text: app.localize('Clone'),
                                visible: function () {
                                    return _permissionsSurpathServices.edit;
                                },
                                action: function (data) {
                                    cloneService(data, _selectedTenantId);
                                },
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    if (!_permissionsSurpathServices.delete) {
                                        return false;
                                    }
                                    var currentTenantId = $('#SelectedTenantId').val() || _selectedTenantId;
                                    //console.log('_permissionsSurpathServices.delete', _permissionsSurpathServices.delete);
                                    //console.log('currentTenantId', currentTenantId);
                                    // Store tenant ID in the data attribute
                                    
                                    return canDeleteService(data.record.surpathService.name, currentTenantId);
                                },
                                action: function (data) {
                                    // Get tenant ID from both possible sources
                                    var currentTenantId = $('#SelectedTenantId').val() || _selectedTenantId;
                                    console.log('Delete action - current tenant ID:', currentTenantId);
                                    console.log('Delete action - data:', data);
                                    
                                    if (!currentTenantId) {
                                        console.warn('No tenant ID found when attempting delete');
                                        return;
                                    }
                                    
                                    deleteService(data, currentTenantId);
                                }
                            },
                            {
                                text: app.localize('History'),
                                iconStyle: 'fas fa-history mr-2',
                                visible: function () {
                                    return entityHistoryIsEnabledTenantSurpathServices();
                                },
                                action: function (data) {
                                    _entityTypeHistoryModal.open({
                                        entityTypeFullName: _entityTypeFullName,
                                        entityId: data.record.tenantSurpathService.id,
                                    });
                                },
                            }, 
                        ],
                    },
                },
                {
                    width: '20px',
                    targets: 2,
                    data: 'tenantSurpathService.isEnabled',
                    name: 'isEnabled',
                    render: function (isEnabled, type, row, meta) {
                        //console.log('render', isEnabled, type, row, meta);
                        if (row.isEnabled) {
                            return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
                        }
                        return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
                    },
                },
                {
                    targets: 3,
                    data: 'name', // Changed from 'name' to match the actual data structure
                    name: 'name',
                },
                {
                    targets: 4,
                    data: 'surpathService.name',
                    name: 'surpathServiceFk.name',
                },
                {
                    targets: 5,
                    data: 'recordCategoryRuleName',
                    name: 'recordCategoryRuleFk.name',
                },
                //{
                //    targets: 5,
                //    visible: abp.auth.hasPermission('Pages.Administration.Host.Dashboard'),
                //    data: 'tenantName',
                //    name: 'tenantName'
                //},
            ],
            createdRow: function (row, data, dataIndex) {
                //console.log('a row was created');
                //console.log(row);
                //console.log(data);
                //console.log(dataIndex);
                //$(row).data('tenancyName', data.tenancyName);
                //$(row).attr('data-tenancyname', data.tenancyName);
                //$(row).attr('data-tenantname', data.name);
                //$(row).attr('data-tenantid', data.id);
                //$(row).addClass('cursor-pointer');
                //$(row).on('click', TenantRowClick);

            },
        });

        // Initial data load
        function getTenantSurpathServices() {
            abp.ui.setBusy(_$tenantSurpathServicesTable);
            
            console.log('Fetching tenant services for tenant:', _selectedTenantId);
            
            _tenantSurpathServicesService
                .getAllTenantServices({
                    filter: $('#TenantSurpathServicesTableFilter').val(),
                    isEnabledFilter: $('#IsEnabledFilterId').val(),
                    surpathServiceNameFilter: $('#SurpathServiceNameFilterId').val(),
                    recordCategoryRuleNameFilter: $('#RecordCategoryRuleNameFilterId').val(),
                    tenantId: _selectedTenantId
                })
                .done(function (result) {
                    // console.log('API response received:', result);
                    _tenantServicesData = result.items;
                    // console.log('Stored tenant services data:', _tenantServicesData);
                    
                    updateServiceCounts();
                    // console.log('Service counts updated:', Array.from(_serviceCountMap.entries()));
                    
                    refreshTableFromLocalData();
                })
                .fail(function(error) {
                    console.error('API call failed:', error);
                })
                .always(function() {
                    abp.ui.clearBusy(_$tenantSurpathServicesTable);
                });
        }

        function deleteService(data, tenantId) {
            // Store tenant ID before delete operation
            const currentTenantId = tenantId;
            console.log('deleteService currentTenantId', currentTenantId);
            const currentTenantName = $('#TenantSurpathServiceTenantName').text();
            
            abp.message.confirm(
                app.localize('DeleteTenantServiceWarningMessage', 
                    data.record.surpathService.name + ' ' + data.record.recordCategoryRuleName),
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _tenantSurpathServicesService
                            .delete({
                                id: data.record.surpathService.id
                            })
                            .done(function () {
                                // Restore tenant context
                                _selectedTenantId = currentTenantId;
                                $('#SelectedTenantId').val(currentTenantId);
                                
                                // Remove from local data
                                _tenantServicesData = _tenantServicesData.filter(
                                    x => x.id !== data.record.surpathService.id
                                );
                                console.log('tenantServicesData', _tenantServicesData);
                                console.log('tenantId', _selectedTenantId);
                                console.log('currentTenantId', currentTenantId);
                                // Update counts
                                updateServiceCounts();
                                refreshTableFromLocalData();
                                
                                abp.notify.success(app.localize('SuccessfullyDeleted'));

                                // Force an immediate refresh while maintaining tenant context
                                getTenantSurpathServices();
                            });
                    }
                }
            );
        }

        // Handle cloning
        function cloneService(data, tenantId) {
            // Store tenant context
            const currentTenantId = tenantId;
            
            // Get the correct service ID
            const serviceId = data.record.surpathService.id;
            
            console.log('Cloning service:', {
                data: data,
                serviceId: serviceId,
                tenantId: currentTenantId,
                serviceName: data.record.surpathService.name
            });
            
            abp.message.confirm(
                app.localize('CloneTenantServiceWarningMessage', 
                    data.record.surpathService.name + ' ' + data.record.recordCategoryRuleName),
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _tenantSurpathServicesService
                            .clone({id: serviceId})
                            .done(function (result) {
                                _selectedTenantId = currentTenantId;
                                // result should be true/false
                                if (result === true) {
                                    // Restore tenant context
                                    
                                    $('#SelectedTenantId').val(currentTenantId);
                                    
                                    abp.notify.success(app.localize('SuccessfullyCloned'));
                                    
                                    // Force an immediate refresh to show the new clone
                                    getTenantSurpathServices();
                                } else {
                                    console.error('Clone operation failed');
                                    abp.notify.error(app.localize('ErrorDuringClone'));
                                }
                            })
                            .fail(function(error) {
                                console.error('Clone operation failed:', error);
                                abp.notify.error(app.localize('ErrorDuringClone'));
                            });
                    }
                }
            );
        }

        // Handle modal saves
        abp.event.on('app.createOrEditTenantSurpathServiceModalSaved', function (result) {
            getTenantSurpathServices();
            updateServiceCounts();
            refreshTableFromLocalData();
            debouncedRefresh(); // Schedule a refresh
        });

        function deleteTenantSurpathService(tenantSurpathService) {
            abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
                if (isConfirmed) {
                    _tenantSurpathServicesService
                        .delete({
                            id: tenantSurpathService.id,
                        })
                        .done(function () {
                            getTenantSurpathServices(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                            debouncedRefresh(); // Schedule a refresh
                        });
                }
            });
        }

        function disableTenantSurpathService(tenantSurpathService) {
            abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
                if (isConfirmed) {
                    _tenantSurpathServicesService
                        .disable({
                            id: tenantSurpathService.id,
                        })
                        .done(function () {
                            getTenantSurpathServices(true);
                            abp.notify.success(app.localize('SuccessfullyDisabled'));
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

        $('#CreateNewTenantSurpathServiceButton').click(function () {
            _createOrEditModal.open({tenantId: _selectedTenantId});
        });

        $('#ExportToExcelButton').click(function () {
            _tenantSurpathServicesService
                .getTenantSurpathServicesToExcel({
                    filter: $('#TenantSurpathServicesTableFilter').val(),
                    isEnabledFilter: $('#IsEnabledFilterId').val(),
                    surpathServiceNameFilter: $('#SurpathServiceNameFilterId').val(),
                    recordCategoryRuleNameFilter: $('#RecordCategoryRuleNameFilterId').val(),
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        // abp.event.on('app.createOrEditTenantSurpathServiceModalSaved', function () {
        //     getTenantSurpathServices();
        // });

        $('#GetTenantSurpathServicesButton').click(function (e) {
            e.preventDefault();
            getTenantSurpathServices();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getTenantSurpathServices();
            }
        });

        $('.reload-on-change').change(function (e) {
            getTenantSurpathServices();
        });

        $('.reload-on-keyup').keyup(function (e) {
            getTenantSurpathServices();
        });

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getTenantSurpathServices();
        });
        //#endregion TenantSurpathServices

        // var hideTenantSurpathServices = function() {
        //     $('#SelectedTenantId').val('');
        //     $('#TenantSurpathServiceTenantName').text('');
        //     $('#TenantSurpathServicesRow').addClass('d-none');
            
        //     // Clear the data
        //     _tenantServicesData = [];
        //     _serviceCountMap.clear();
        //     refreshTableFromLocalData();
        // }

        // Clear local data when switching tenants
        function loadTenantSurpathServices(id, tenantName) {
            if (_refreshTimeout) {
                clearTimeout(_refreshTimeout);
            }
            _selectedTenantId = id;
            $('#SelectedTenantId').val(id);

            _$tenantSurpathServicesTable.data('tenant-id', id);
            $('#tenant_surpath_services_collapsible').collapse('show');
            //console.log('Loading tenant services for tenant:', id, tenantName);

            $('#manage_tenants_collapsible').collapse('hide');
            

            // Clear existing data
            _tenantServicesData = [];
            _serviceCountMap.clear();

            
            
            // Load new data
            getTenantSurpathServices();
            console.log('loadTenantSurpathServices',id,'xx',tenantName, _tenantServicesData);
            $('#TenantSurpathServiceTenantName').text(tenantName);

            // debugger;
        }

        var TenantRowClick = function (e) {
            
            var _$row = $(e.currentTarget);
            if ($(e.target).get(0).tagName != 'TD') return;
            _selectedTenantId = _$row.data('tenantid');
            $('#SelectedTenantId').val(_selectedTenantId);
            // debugger;
            //console.log('TenantRowClick', _$row.data, _$row.data('tenantid'), _$row.data('tenantname'));
            loadTenantSurpathServices(_$row.data('tenantid'), _$row.data('tenantname'))
            // if ($('#manage_tenants_collapsible').hasClass('collapsed') == false) {
            $('#manage_tenants_collapsible').collapse('hide');
            // }
            // toggleTenantsTable(false); // Collapse the tenants table
        };

      

        $('#manage_tenants_collapsible').on('hidden.bs.collapse', function () {
            var $tenantSurpathServicesRow = $('#TenantSurpathServicesRow');
            // this was modified to only show the services row, don't clear data
            // Only show the services row, don't clear data
            $tenantSurpathServicesRow.removeClass('d-none');
            
            // Log current state
            console.log('Tenant collapsible hidden - maintaining data for tenant:', _selectedTenantId);
        });

        $('#manage_tenants_collapsible').on('shown.bs.collapse', function () {
            // console.log('manage_tenants_collapsible shown');
            // This fires after the collapsible is fully shown
            var $tenantSurpathServicesRow = $('#TenantSurpathServicesRow');
            // var $tenantSurpathServiceTenantName = $('#TenantSurpathServiceTenantName');
            
            // Hide and reset tenant services section
            $tenantSurpathServicesRow.addClass('d-none');
            //$tenantSurpathServiceTenantName.text('');
            
            // Clear the data
            // console.log('loadTenantSurpathServices clearing _selectedTenantId', _selectedTenantId);
            _selectedTenantId = '';
            $('#SelectedTenantId').val('');
            _tenantServicesData = [];
            _serviceCountMap.clear();
            refreshTableFromLocalData();
        });

    });
})();

