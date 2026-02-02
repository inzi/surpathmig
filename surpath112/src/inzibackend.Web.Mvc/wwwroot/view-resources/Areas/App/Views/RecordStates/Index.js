(function () {
    $(function () {
        var _$recordStatesTable = $('#RecordStatesTable');
        var _recordStatesService = abp.services.app.recordStates;
        var _entityTypeFullName = 'inzibackend.Surpath.RecordState';

        $('.date-picker').daterangepicker({
            singleDatePicker: true,
            
             locale: { format: 'MM/DD/YYYY', },
        });

        $('#AdvacedAuditFiltersArea').find('.date-picker').daterangepicker({
            singleDatePicker: true,
            
            locale: {
                cancelLabel: 'Clear'
            },
             locale: { format: 'MM/DD/YYYY', },
            autoUpdateInput: false,
        }).on("apply.daterangepicker", function (e, picker) {
            picker.element.val(picker.startDate.format(picker.locale.format));
        }).on('cancel.daterangepicker', function (ev, picker) {
            $(this).val('');
        }).val('');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.RecordStates.Create'),
            edit: abp.auth.hasPermission('Pages.RecordStates.Edit'),
            delete: abp.auth.hasPermission('Pages.RecordStates.Delete'),
            change: abp.auth.hasPermission('Pages.Surpath.Host.ChangeStatus'),
            review: abp.auth.hasPermission('Surpath.ComplianceReview')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RecordStates/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RecordStates/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditRecordStateModal',
        });

        var _viewRecordStateModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RecordStates/ViewrecordStateModal',
            modalClass: 'ViewRecordStateModal',
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
            if (element.val() == '') {
                return null;
            }
            return element.data('daterangepicker').startDate.format('YYYY-MM-DDT00:00:00Z');
        };
        var getMaxDateFilter = function (element) {
            if (element.val() == '') {
                return null;
            }
            return element.data('daterangepicker').startDate.format('YYYY-MM-DDT00:00:00Z');
        };

        var dataTable = _$recordStatesTable.DataTable({
            paging: true,
            lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
            pageLength: 5000,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _recordStatesService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#RecordStatesTableFilter').val(),
                        stateFilter: $('#StateFilterId').val(),
                        notesFilter: $('#NotesFilterId').val(),
                        recordfilenameFilter: $('#RecordfilenameFilterId').val(),
                        recordCategoryNameFilter: $('#RecordCategoryNameFilterId').val(),
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
                                    _viewRecordStateModal.open({ id: data.record.recordState.id });
                                },
                            },
                            {
                                text: app.localize('Review'),
                                iconStyle: 'far fa-edit mr-2',
                                visible: function () {
                                    return _permissions.review;
                                },
                                action: function (data) {
                                    window.location = '/App/RecordStates/ReviewRecordState?id=' + data.record.recordState.id;
                                },
                            },
                            {
                                text: app.localize('Edit'),
                                iconStyle: 'far fa-edit mr-2',
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({ id: data.record.recordState.id });
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
                                        entityId: data.record.recordState.id,
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
                                    deleteRecordState(data.record.recordState);
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
                    visible: abp.auth.hasPermission('Pages.RecordNotes'),
                    render: function () {
                        return `<button class="btn btn-primary btn-xs Edit_RecordNote_RecordStateId">${app.localize(
                            'EditRecordNote'
                        )}</button>`;
                    },
                },
                {
                    targets: 3,
                    data: 'recordCategoryName',
                    name: 'recordCategoryFk.name'
                },
                {
                    targets: 4,
                    data: 'userName',
                    name: 'userFk.name',
                },


                {
                    targets: 5,
                    data: null,
                    name: 'state',
                    render: function (data, row, full) {
                    
                        return '<span class="badge record-status-badge" style="background: ' + data.recordStatus.htmlColor + '">' + data.recordStatus.statusName + '</span>';
                    
                    },
                },

                //{
                //    targets: 5,
                //    data: 'recordState.state',
                //    name: 'state',
                //    render: function (state) {
                //        return app.localize('Enum_EnumRecordState_' + state);
                //    },
                //},

                //{
                //    targets: 5,
                //    data: 'recordCategoryName',
                //    name: 'recordCategoryFk.name',
                //},
                //{
                //    targets: 3,
                //    data: 'recordState.state',
                //    name: 'state',
                //    render: function (state) {
                //        return app.localize('Enum_EnumRecordState_' + state);
                //    },
                //},
                //{
                //    targets: 4,
                //    data: 'recordState.notes',
                //    name: 'notes',
                //},
                //{
                //    targets: 5,
                //    data: 'recordfilename',
                //    name: 'recordFk.filename',
                //},
                //{
                //    targets: 6,
                //    data: 'recordCategoryName',
                //    name: 'recordCategoryFk.name',
                //},
                //{
                //    targets: 7,
                //    data: 'userName',
                //    name: 'userFk.name',
                //},
            ],
            rowGroup: {
                // Uses the 'row group' plugin
                dataSrc: 'userId',
                startRender: function (rows, group) {

                    /*console.log('group / userId: ' + group);*/
                    var collapsed = !app.collapseGroups.recStateCollapsedGroups[group];
                    var arrSt = [];
                    // debugger;
                    rows.nodes().each(function (r, i) {

                        r.style.display = collapsed ? 'none' : '';
                        var _color = $(r).data('statuscolor');
                        arrSt.push(_color);

                    });
                    // get all unique colors (states)
                    //// but only these rows
                    dataTable.rawData.items.forEach(function (item) {

                        var val = item.recordStatus.htmlColor;

                        //console.log(item);


                    });
                    //debugger;

                    var _showstatbar = false;
                    if (arrSt.length > 0) {

                        // get unique with count - output will be something like { hi: 2, hello: 1 }
                        // Todo, maybe find a way to sort by Id

                        const stats = {};
                        for (var i = 0; i < arrSt.length; i++) {
                            stats[arrSt[i]] = 1 + (stats[arrSt[i]] || 0);
                        };
                        var keyNames = Object.keys(stats);
                        //debugger;

                        //var myObject = { '#dddddd': 3, '#aaaaaa': 2, '#00bbdd': 6 };
                        //var keyNames = Object.keys(myObject);
                        //console.log(keyNames); // Outputs ["a","b","c"]

                        var total = arrSt.length;
                        var _statbar = $('<div class="statbar"/>');
                        var _statsubs = []
                        var _allWidths = 0
                        for (let i = 0; i < keyNames.length; i++)
                        {
                            var _key = keyNames[i];
                            var _count = stats[_key]
                            var _perc = parseInt((_count / total) * 100);
                            _allWidths = _allWidths + _perc;
                            if (i == keyNames.length-1) {
                                if (_allWidths < 100) {
                                    _perc = _perc + 1; // in case we have an odd percentage, we expand the last one
                                }
                            }
                            var _statsub = $('<div />');
                            _statsub.css('width', _perc + '%');
                            _statsub.css('background', _key);
                            _statsub.addClass('statbarsub');
                            _statsub.addClass('statbarmid');
                            //console.log(_statsub)
                            _statsubs.push(_statsub);
                        }
                        ((_statsubs[0]).addClass('statbarleft')).removeClass('statbarmid');
                        _statsubs[_statsubs.length - 1].addClass('statbarright').removeClass('statbarmid');
                        _statbar.append(_statsubs);
                      
                        _showstatbar = true;
                    }


                    // $('.dtrg-start').on('click', function () { console.log('dt click'); });
                    var _tr = $('<tr class="ssRecStateGroup"/>')
                        .attr('data-recordstatename', group)
                        .attr('data-collapsegroup', group)
                        .toggleClass('collapsed', collapsed);

                    _tr.append('<td class="details-control" colspan="3"><i class="groupaction far fa-plus-square icon-lg"></i><i class="groupaction far fa-minus-square icon-lg"></i> ' + rows.data()[0].fullName + ' (' + rows.count() + ')</td>');

                    var _dt2 = $('<td class="details-control ssRecStateStatuses" colspan="3"><div id="statechart"></div></td>'); //.append(_dt2);
                    if (_showstatbar) {
                        _dt2.append(_statbar);
                    }
                    _tr.append(_dt2);
                        /*.append('<td class="details-control ssRecStateStatuses" colspan="3"><div id="statechart"></div></td>')*/
                        
                      

                    $(_tr).on('click', function (e) {
                        var name = $(this).data('recordstatename');
                        toggleCollapsed(name);
                    });
                    return _tr;

              
                },
            },

            createdRow: function (row, data, dataIndex) {
                //console.log('a row was created');
                //console.log(row);
                //console.log(data);
                //console.log(dataIndex);
                $(row).attr('data-statuscolor', data.recordStatus.htmlColor);
            }
        //    rowCallback: function (row, data) {
        //        //if (data.cashflow.manual > 0 || data.cashflow.additional_repayment > 0) {
        //        //    $(row).addClass('fontThick');
        //        //}
        //        ////if it is not the summation row the row should be selectable
        //        //if (data.cashflow.position !== 'L') {
        //        //    $(row).addClass('selectRow');
        //        //}
        //        return;
        //    },
        });
     
        //const countUnique = arr => {
        //    const counts = {};
        //    for (var i = 0; i < arr.length; i++) {
        //        counts[arr[i]] = 1 + (counts[arr[i]] || 0);
        //    };
        //    return counts;
        //};

        //function buildchart() {
        //  </div>
        //}

        function buildHoldChart(id) {

            /*using chart.js: https://codepen.io/jamiecalder/pen/NrROeB*/

            // https://apexcharts.com/javascript-chart-demos/bar-charts/stacked/

            //var options = {
            //    series: [{
            //        name: 'Marine Sprite',
            //        data: [44]
            //    }, {
            //        name: 'Striking Calf',
            //        data: [53]
            //    }, {
            //        name: 'Tank Picture',
            //        data: [12]
            //    }, {
            //        name: 'Bucket Slope',
            //        data: [9]
            //    }, {
            //        name: 'Reborn Kid',
            //        data: [25]
            //    }],
            //    chart: {
            //        type: 'bar',
            //        height: 350,
            //        stacked: true,
            //    },
            //    plotOptions: {
            //        bar: {
            //            horizontal: true,
            //        },
            //    },
            //    stroke: {
            //        width: 1,
            //        colors: ['#fff']
            //    },
            //    title: {
            //        text: 'Fiction Books Sales'
            //    },
            //    xaxis: {
            //        categories: [2008],
            //        labels: {
            //            formatter: function (val) {
            //                return val + "K"
            //            }
            //        }
            //    },
            //    yaxis: {
            //        title: {
            //            text: undefined
            //        },
            //    },
            //    tooltip: {
            //        y: {
            //            formatter: function (val) {
            //                return val + "K"
            //            }
            //        }
            //    },
            //    fill: {
            //        opacity: 1
            //    },
            //    legend: {
            //        position: 'top',
            //        horizontalAlign: 'left',
            //        offsetX: 40
            //    }
            //};

            //var chart = new ApexCharts(document.querySelector("#chart"), options);
            //chart.render();

        }


        function deleteRecordState(recordState) {
            abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
                if (isConfirmed) {
                    _recordStatesService
                        .delete({
                            id: recordState.id,
                        })
                        .done(function () {
                            getRecordStates(true);
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

        $('#CreateNewRecordStateButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _recordStatesService
                .getRecordStatesToExcel({
                    filter: $('#RecordStatesTableFilter').val(),
                    stateFilter: $('#StateFilterId').val(),
                    notesFilter: $('#NotesFilterId').val(),
                    recordfilenameFilter: $('#RecordfilenameFilterId').val(),
                    recordCategoryNameFilter: $('#RecordCategoryNameFilterId').val(),
                    userNameFilter: $('#UserNameFilterId').val(),
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditRecordStateModalSaved', function () {
            getRecordStates();
        });

        $('#GetRecordStatesButton').click(function (e) {
            e.preventDefault();
            getRecordStates();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getRecordStates();
            }
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

        _$recordStatesTable.on('click', '.Edit_RecordNote_RecordStateId', function () {
            var tr = $(this).closest('tr');
            var row = dataTable.row(tr);
            openDetailRow(this, '/App/MasterDetailChild_RecordState_RecordNotes?RecordStateId=' + row.data().recordState.id);
        });

        _$recordStatesTable
            .on('xhr.dt', function (e, settings, json, xhr) {
                //console.log('data back');
                //console.log(settings);
                //console.log(json);
                sortingUser = settings.rawServerResponse.sortingUser;
                dataTable.rawData = settings.rawServerResponse;
        });

        function toggleCollapsed(name) {
            app.collapseGroups.recStateCollapsedGroups[name] = !app.collapseGroups.recStateCollapsedGroups[name];
            dataTable.draw(false);
        };

        function getRecordStates() {
            dataTable.ajax.reload();
        }
    });
})();
