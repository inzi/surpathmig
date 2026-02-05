(function () {
    // Surscan Working Filter
    app.widgets.Widgets_Tenant_SurpathDept = function () {
        var _applicationPrefix = 'Mvc';
        var _tenantDepartmentsService = abp.services.app.tenantDepartments;

        var _widget;
        var _$WidgetComplianceTable;
        var _$openWidgetFiltersButton;

        var _TenantDepartmentLookupTableModal = new app.ModalManager({
        	viewUrl: abp.appPath + 'App/TenantDepartments/TenantDepartmentLookupTableModal',
        	scriptUrl:
        		abp.appPath +
        		'view-resources/Areas/App/Views/TenantDepartments/_TenantDepartmentLookupTableModal.js',
        	modalClass: 'TenantDepartmentLookupTableModal',
        });

        var initComplianceTableInfo = function (recentTenantsDayCount, maxRecentTenantsShownCount, creationDateStart) {
            // get the button

            _$openWidgetFiltersButton = _widget.find('.open-widget-filters-btn');
            _$openWidgetFiltersButton.on('click', function () {
                //debugger;

                _TenantDepartmentLookupTableModal.open(
                    { id: '', displayName:'' },
                    function (data) {
                        // set the data attribute
                        _widget.parent().parent().data('settings_tenantdepartmentid', data.id);
                        _widget.parent().parent().data('settings_tenantdepartmentname', data.displayName);
                        _widget.find('#departmentName').text(data.displayName);
                        $('#savePageButton').click();
                    }
                );
            });
        };

        function getFilter() {
            return {
                filter: '',
                namefilter: '',
                descriptionfilter: '',
                defaultcohortfilter: '',
                tenantdepartmentnamefilter: _widget.parent().parent().data('settings_tenantdepartmentname'),
            };

        };

        this.init = function (widgetManager) {

            _widget = widgetManager.getWidget();
            _$WidgetComplianceTable = _widget.find('.compliance-widget-table');

            initialize();
        };

        function initialize() {

            abp.ui.setBusy(_widget);
            var _filter = getFilter();
            _tenantDepartmentsService
                //.getCompliance(_filter)
                .getAll(_filter)
                .done(function (result) {

                    //initTableInfo(
                    //    result.recentTenantsDayCount,
                    //    result.maxRecentTenantsShownCount,
                    //    result.tenantCreationStartDate
                    //);
                    //debugger;
                    console.log(result);

                    InitWidgetTable(result.items);
                })
                .always(function () {
                    abp.ui.clearBusy(_widget);
                });
        }

        function InitWidgetTable(items) {
            _$WidgetComplianceTable.DataTable({
                paging: true,
                lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
                pageLength: 5000,
                serverSide: false,
                processing: false,
                info: false,
                ajax: function (data, callback, settings) {
                    callback({ data: items });
                },
                columnDefs: [
                    {
                        targets: 0,
                        data: 'tenantDepartment.name',
                    },
                    //{
                    //    targets: 1,
                    //    data: 'creationTime',
                    //    render: function (creationTime) {
                    //        return moment(creationTime).format('L LT');
                    //    },
                    //},
                ],
            });
        }
        
    };// End of widget
})();


    ////Widgets_Tenant_HelloWorld must match with the WidgetViewDefinition name defined in Step 3.
    //app.widgets.Widgets_Tenant_SurpathDept = function () {
    //    var _applicationPrefix = 'Mvc';
    //    var _tenantDashboardService = abp.services.app.tenantDashboard;
    //    var _dashboardCustomizationService = abp.services.app.dashboardCustomization;
    //    var _tenantDepartmentsService = abp.services.app.tenantDepartments;

    //    var _widget;
    //    var _$WidgetComplianceTable;

    //    //var _$openWidgetFiltersButton;

    //    this.init = function (widgetManager) {
    //        _widget = widgetManager.getWidget();
    //        _$WidgetComplianceTable = _widget.find('.compliance-widget-table');
    //        initialize();
    //    };

    //    function getFilter() {
    //        return {
    //            filter: _widget.parent().parent().data('settings_tenantdepartmentname'),
    //            namefilter: '',
    //            descriptionfilter: '',
    //            defaultcohortfilter: '',
    //            tenantdepartmentnamefilter: '',
    //        };

    //    };

    //    //var initTableInfo = function (tableCount, maxtableShownCount, creationDateStart) {
    //    //    //_$recentTenantsCaptionHelper.text(
    //    //    //    app.localize('RecentTenantsHelpText', tableCount, maxtableShownCount)
    //    //    //);

    //    //    //_$seeAllRecentTenantsButton.data('creationDateStart', creationDateStart).click(function () {
    //    //    //    window.open(
    //    //    //        abp.appPath +
    //    //    //        'App/Tenants?' +
    //    //    //        'creationDateStart=' +
    //    //    //        encodeURIComponent($(this).data('creationDateStart'))
    //    //    //    );
    //    //    //});
    //    //};

    //    function initialize() {

    //        abp.ui.setBusy(_widget);
    //        var _filter = getFilter();
    //        _tenantDepartmentsService
    //            .getCompliance(_filter)
    //            .done(function (result) {

    //                //initTableInfo(
    //                //    result.recentTenantsDayCount,
    //                //    result.maxRecentTenantsShownCount,
    //                //    result.tenantCreationStartDate
    //                //);
    //                debugger;

    //                initWidgetTable(result.items);
    //            })
    //            .always(function () {
    //            abp.ui.clearBusy(_widget);
    //        });

    //        //_hostDashboardService
    //        //    .getRecentTenantsData()
    //        //    .done(function (result) {
    //        //        initRecentTenantsTableInfo(
    //        //            result.recentTenantsDayCount,
    //        //            result.maxRecentTenantsShownCount,
    //        //            result.tenantCreationStartDate
    //        //        );
    //        //        initRecentTenantsTable(result.recentTenants);
    //        //    })
    //        //    .always(function () {
    //        //        abp.ui.clearBusy(_widget);
    //        //    });
    //    }
    //    function initWidgetTable(tenantDepartments) {
    //        _$WidgetComplianceTable.DataTable({
    //            paging: false,
    //            serverSide: false,
    //            processing: false,
    //            info: false,
    //            ajax: function (data, callback, settings) {

    //                callback({ data: tenantDepartments });
    //            },
    //            columnDefs: [
    //                {
    //                    targets: 0,
    //                    data: 'name',
    //                },
    //                //{
    //                //    targets: 1,
    //                //    data: 'creationTime',
    //                //    render: function (creationTime) {
    //                //        return moment(creationTime).format('L LT');
    //                //    },
    //                //},
    //            ],
    //        });
    //    }

    //    //////////var _$openWidgetFiltersButton = $('#open-widget-filters-btn');
    //    ////////var _widgetBase = app.widgetBase.create();
    //    ////////var _widget;
    //    ////////var _$openWidgetFiltersButton;

    //    ////////var _TenantDepartmentLookupTableModal = new app.ModalManager({
    //    ////////	viewUrl: abp.appPath + 'App/TenantDepartments/TenantDepartmentLookupTableModal',
    //    ////////	scriptUrl:
    //    ////////		abp.appPath +
    //    ////////		'view-resources/Areas/App/Views/TenantDepartments/_TenantDepartmentLookupTableModal.js',
    //    ////////	modalClass: 'TenantDepartmentLookupTableModal',
    //    ////////});

    //    ////////this.init = function (widgetManager) {
    //    ////////    _widget = widgetManager.getWidget();
    //    ////////    _widget.find('.widget_display_departmentName').text(_widget.parent().parent().data('settings_tenantdepartmentname'));
    //    ////////    debugger;
    //    ////////    //_$WidgetComplianceTable = $('#WidgetComplianceTable'); // WidgetComplianceTable
    //    ////////    _$WidgetComplianceTable = _widget.find('.compliance-widget-table');

    //    ////////    var dataTable = _$WidgetComplianceTable.DataTable({
    //    ////////        paging: true,
    //    ////////        serverSide: true,
    //    ////////        processing: true,
    //    ////////        listAction: {
    //    ////////            ajaxFunction: _tenantDepartmentsService.getAll,
    //    ////////            //ajaxFunction: function () {
    //    ////////            //    abp.ui.setBusy(_widget);
    //    ////////            //    var res = [];
    //    ////////            //    _tenantDepartmentsService.getCompliance({ filter: name })
    //    ////////            //        .done(function (result) {
    //    ////////            //            //_widget.find(".hello-response")//it is how you should select item in widget
    //    ////////            //            //    .text(result.outPutName);
    //    ////////            //            //debugger;
    //    ////////            //            res = result;
    //    ////////            //        }).always(function () {
    //    ////////            //            abp.ui.clearBusy(_widget);
    //    ////////            //        });
    //    ////////            //    return res;
    //    ////////            //},
    //    ////////            inputFilter: function () {
    //    ////////                return {
    //    ////////                    filter: _widget.parent().parent().data('settings_tenantdepartmentname'),
    //    ////////                    nameFilter: '',
    //    ////////                    descriptionFilter: '',
    //    ////////                    defaultCohortFilter: '',
    //    ////////                    tenantDepartmentNameFilter: '',
    //    ////////                };
    //    ////////            },
    //    ////////        },
    //    ////////        columnDefs: [
    //    ////////            {
    //    ////////                targets: 1,
    //    ////////                data: 'tenantDepartment.name',
    //    ////////                name: 'name',
    //    ////////            },
    //    ////////            //{
    //    ////////            //    targets: 0,
    //    ////////            //    //data: 'tenantDepartment.name',
    //    ////////            //    //name: 'Name',
    //    ////////            //    //render: function (data) {
    //    ////////            //    //    //debugger;
    //    ////////            //    //    console.log(data);
    //    ////////            //    //    return `column 0`;
    //    ////////            //    //},
    //    ////////            //    render: function () { return 'some data'; },

    //    ////////            //},
    //    ////////            //{
    //    ////////            //    targets: 1,
    //    ////////            //    //data: 'complianceSummary.count',
    //    ////////            //    //name: 'count',
    //    ////////            //    render: function () { return 'some data';},
    //    ////////            //},
    //    ////////            //{
    //    ////////            //    targets: 2,
    //    ////////            //    //data: 'complianceSummary.statusName',
    //    ////////            //    //name: 'statusName',
    //    ////////            //    render: function () { return 'some data'; },
    //    ////////            //},
    //    ////////        ],
    //    ////////    });



    //    ////////    // get the button

    //    ////////    _$openWidgetFiltersButton = _widget.find('#open-widget-filters-btn');
    //    ////////    _$openWidgetFiltersButton.on('click', function () {
    //    ////////        //debugger;

    //    ////////        _TenantDepartmentLookupTableModal.open(
    //    ////////            { id: '', displayName:'' },
    //    ////////            function (data) {
    //    ////////                // set the data attribute
    //    ////////                var _settings_tenantdepartmentid = '';
    //    ////////                _widget.parent().parent().data('settings_tenantdepartmentid', data.id);
    //    ////////                _widget.parent().parent().data('settings_tenantdepartmentname', data.displayName);
    //    ////////                _widget.find('#departmentName').text(data.displayName);
    //    ////////                savePageData();
    //    ////////                //debugger;
    //    ////////            }
    //    ////////        );
    //    ////////    });

    //    ////////    _widgetBase.runDelayed(function () {
    //    ////////        var _name = _widget.parent().parent().data('settings_tenantdepartmentname');
    //    ////////       // getDeptCompliance(_name);
    //    ////////        //	getSurpathDept("First Attempt");
    //    ////////        // get the dept from data attributes and populate the table
    //    ////////    });

    //    ////////    //event which your filter send
    //    ////////    // This is listenting to the event defined in the filter
    //    ////////    //abp.event.on('app.dashboardFilters.surpathDeptFilter.onNameChange', function (name) {
    //    ////////    //	_widgetBase.runDelayed(function () {
    //    ////////    //		getSurpathDept(name);
    //    ////////    //	});
    //    ////////    //});
    //    ////////};

    //    ////////var getDeptCompliance = function (name) {
    //    ////////    //GetCompliance
    //    ////////    abp.ui.setBusy(_widget);
    //    ////////    _tenantDepartmentsService.getCompliance({ filter: name })
    //    ////////        .done(function (result) {
    //    ////////            //_widget.find(".hello-response")//it is how you should select item in widget
    //    ////////            //    .text(result.outPutName);
    //    ////////            //debugger;
    //    ////////        }).always(function () {
    //    ////////            abp.ui.clearBusy(_widget);
    //    ////////        });
    //    ////////}


    //    //////////var getSurpathDept = function (name) {
    //    //////////    abp.ui.setBusy(_widget);
    //    //////////    _tenantDashboardService.getSurpathDeptData({ name: name })
    //    //////////        .done(function (result) {
    //    //////////            _widget.find(".hello-response")//it is how you should select item in widget
    //    //////////                .text(result.outPutName);
    //    //////////        }).always(function () {
    //    //////////            abp.ui.clearBusy(_widget);
    //    //////////        });
    //    //////////};


    //    var savePageData = function () {
    //        $('#savePageButton').click();
    //    };




    //    //////////debugger;


    //    //////////var _viewTenantDepartmentUserModal = new app.ModalManager({
    //    //////////	viewUrl:
    //    //////////		abp.appPath + 'App/TenantDepartments/_TenantDepartmentLookupTableModal',
    //    //////////	modalClass: 'MasterDetailChild_TenantDepartment_ViewTenantDepartmentUserModal',
    //    //////////});

    //    //////////var _TenantDepartmentLookupTableModal = new app.ModalManager({
    //    //////////	viewUrl: abp.appPath + 'App/TenantDepartments/TenantDepartmentLookupTableModal',
    //    //////////	scriptUrl:
    //    //////////		abp.appPath +
    //    //////////		'view-resources/Areas/App/Views/TenantDepartments/_TenantDepartmentLookupTableModal.js',
    //    //////////	modalClass: 'TenantDepartmentLookupTableModal',
    //    //////////});


    //    //////////debugger;

    //}; // End of widget

