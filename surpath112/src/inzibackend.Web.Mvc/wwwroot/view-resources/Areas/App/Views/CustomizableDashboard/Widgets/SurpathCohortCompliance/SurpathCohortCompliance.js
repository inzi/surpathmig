(function () {
    // Surscan Working Filter

    app.widgets.Widgets_Tenant_CohortCompliance = function () {
        var _applicationPrefix = 'Mvc';

        var _cohortsService = abp.services.app.cohorts;
        var _dashboardCustomizationService = abp.services.app.dashboardCustomization;

        var _widget;
        var _programName;
        var _$WidgetComplianceTable;
        var _$openWidgetFiltersButton;

        var dataTable;

        var _TenantDepartmentLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/TenantDepartments/TenantDepartmentLookupTableModal',
            scriptUrl:
                abp.appPath +
                'view-resources/Areas/App/Views/TenantDepartments/_TenantDepartmentLookupTableModal.js',
            modalClass: 'TenantDepartmentLookupTableModal',
        });

        this.init = function (widgetManager) {
            _widget = widgetManager.getWidget();
            _$WidgetComplianceTable = _widget.find('.compliance-widget-table');
            _$openWidgetFiltersButton = _widget.find('.open-widget-filters-btn');
            _programName = _widget.parent().parent().data('settings_tenantdepartmentname');
            _programName = _programName + '';
            if (typeof _programName != "undefined") {
                if (_programName.length > 0) {
                    _widget.find('.widget_display_departmentName').text(_programName);
                };
            };            
            setupButton();
            setDatTable();
            initialize();
        };

        function setupButton() {

            _$openWidgetFiltersButton.on('click', function () {
                //debugger;

                _TenantDepartmentLookupTableModal.open(
                    { id: '', displayName: '' },
                    function (data) {
                        // set the data attribute
                        _widget.parent().parent().data('settings_tenantdepartmentid', data.id);
                        _widget.parent().parent().data('settings_tenantdepartmentname', data.displayName);
                        _widget.find('#departmentName').text(data.displayName);
                        //$('#savePageButton').click(function () {
                        //    console.log('back');
                        //});
                        //var f2 = getCohorts();
                        //$.when(f1).then(f2);
                        savePageData(getCohorts);
                    }
                );
            });
        }

        function initialize() {
            console.log("cc init");
            //getCohorts();
        }

        function getFilter() {
            return {
                filter: '',
                namefilter: '',
                descriptionfilter: '',
                defaultcohortfilter: '',
                tenantdepartmentnamefilter: _widget.parent().parent().data('settings_tenantdepartmentname'),
            };

        };

        function setDatTable() {

            dataTable = _$WidgetComplianceTable.DataTable({
                paging: true,
                lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
                pageLength: 5000,
                serverSide: true,
                processing: true,
                listAction: {
                    ajaxFunction: _cohortsService.getCompliance,
                    inputFilter: getFilter,
                },
                columnDefs: [
                    {
                        targets: 0,
                        data: 'cohort.name',
                        name: 'Cohort',
                    },
                    {
                        targets: 1,
                        data: 'cohort.description',
                        name: 'description',
                    },
                    {
                        targets: 2,
                        name: 'compliance',
                        
                        render: function (raw, data, item) {
                            //console.log(raw);
                            //console.log(data);
                            //console.log(item);
                            if (item.complianceRecords < 1) {
                                return 'no records';
                            }
                            return app.complianceRender.render(item.complianceRecords, item.cohort.complianceSummary);
                        }
                    }
                ],
                createdRow: function (row, data, index) {
                    //console.log(row);
                    //console.log(data);
                    //console.log(index);
                    $(row).attr('data-cohort-id', data.cohort.id);
                    $(row).addClass('surpath-drill-row');
                    // $(row).attr('data-id', )
                    $(row).on('click', function () {
                        /*console.log(this);*/
                        var _id = $(this).data('cohort-id');
                        //console.log('go get cohort users for ' + $(this).data('cohort-id'));
                        window.location = '/App/CohortUsers?id=' + _id;

                    });
                },
            });

        }


        function getCohorts() {
            console.log("getCohorts");

            dataTable.ajax.reload();
        }

        var savePageData = function (callback) {
            abp.ui.setBusy($('body'));

            var pageContent = [];
            var pages = $('#PagesDiv').find('.page');

            for (var j = 0; j < pages.length; j++) {
                var page = pages[j];
                var pageId = $(page).find('input[name="PageId"]').val();
                var pageName = $(page).find('input[name="PageName"]').val();
                var widgetStackItems = $(page).find('.grid-stack-item');
                var widgets = [];

                for (var i = 0; i < widgetStackItems.length; i++) {
                    var widget = {};
                    widget.widgetId = $(widgetStackItems[i]).attr('data-widget-id');
                    widget.height = $(widgetStackItems[i]).attr('gs-h');
                    widget.width = $(widgetStackItems[i]).attr('gs-w');
                    widget.positionX = $(widgetStackItems[i]).attr('gs-x');
                    widget.positionY = $(widgetStackItems[i]).attr('gs-y');
                    widget.settings = getSettings($(widgetStackItems[i]).data());
                    widgets[i] = widget;
                }
                pageContent.push({
                    id: pageId,
                    name: pageName,
                    widgets: widgets,
                });
            }


            function getSettings(_data) {
                var obj = {};
                $.each(Object.keys(_data), function (i) {
                    if (Object.keys(_data)[i].indexOf("settings_") == 0) {
                        var key = Object.keys(_data)[i].replace("settings_", "");
                        obj[key] = _data[Object.keys(_data)[i]].valueOf();
                    }
                });
                return obj;
            };

            var filters = [];

            var filterDiv = $('#FiltersDiv');
            if (filterDiv) {
                var filtersStackItems = $(filterDiv).find('.grid-stack-item');

                for (var i = 0; i < filtersStackItems.length; i++) {
                    var filter = {};
                    filter.widgetFilterId = $(filtersStackItems[i]).attr('data-filter-id');
                    filter.height = $(filtersStackItems[i]).attr('data-gs-height');
                    filter.width = $(filtersStackItems[i]).attr('data-gs-width');
                    filter.positionX = $(filtersStackItems[i]).attr('data-gs-x');
                    filter.positionY = $(filtersStackItems[i]).attr('data-gs-y');
                    filters[i] = filter;
                }
            }

            _dashboardCustomizationService
                .savePage({
                    dashboardName: $('#DashboardName').val(),
                    pages: pageContent,
                    widgetFilters: filters,
                    application: _applicationPrefix,
                })
                .done(function (result) {
                    abp.notify.success(app.localize('Saved'));
                    $('#EditableCheckbox').prop('checked', false).trigger('change');
                })
                .always(function () {
                    abp.ui.clearBusy($('body'));
                    if (callback != undefined) {
                        callback();
                    }
                    
                });
        };


    };// End of widget
})();



