(function () {
    $(function () {
        var _$recordStatesTable = $('#RecordStatesTable');
        var _surpathToolReviewQueue = abp.services.app.surpathToolReviewQueue;
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
                ajaxFunction: _surpathToolReviewQueue.getAll,
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
                                text: app.localize('View User'),
                                iconStyle: 'far fa-eye mr-2',
                                action: function (data) {

                                    console.log(data);
                                    // _viewRecordStateModal.open({ id: data.record.recordState.id });
                                    var _cohortUserId = data.record.cohortUserId;

                                    ////window.location = '/App/CohortUsers/ViewCohortUser/08da5a0b-e16b-448f-89c8-c597622073d6'
                                    window.location = '/App/CohortUsers/ViewCohortUser/' + data.record.cohortUserId + '#documents';
                                    

                                    //window.location = '/App/RecordStates/ReviewRecordState?id=' + data._cohortUserId;
                                },
                            },
                            {
                                text: app.localize('Review'),
                                iconStyle: 'far fa-edit mr-2',
                                visible: function () {
                                    return _permissions.review;
                                },
                                action: function (data) {
                                    //window.location = '/App/ToolReviewQueue/ReviewRecordState?id=' + data.record.recordState.id;
                                    window.location = '/App/RecordStates/ReviewRecordState?id=' + data.record.recordState.id;
                                },
                            },
                            //{
                            //    text: app.localize('Edit'),
                            //    iconStyle: 'far fa-edit mr-2',
                            //    visible: function () {
                            //        return _permissions.edit;
                            //    },
                            //    action: function (data) {
                            //        _createOrEditModal.open({ id: data.record.recordState.id });
                            //    },
                            //},
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
                            //{
                            //    text: app.localize('Delete'),
                            //    iconStyle: 'far fa-trash-alt mr-2',
                            //    visible: function () {
                            //        return _permissions.delete;
                            //    },
                            //    action: function (data) {
                            //        deleteRecordState(data.record.recordState);
                            //    },
                            //},
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
            ],
            createdRow: function (row, data, dataIndex) {
                //console.log('a row was created');
                //console.log(row);
                //console.log(data);
                //console.log(dataIndex);
                $(row).attr('data-statuscolor', data.recordStatus.htmlColor);
                $(row).attr('data-cohortuserid', data.cohortUserId);
                $(row).attr('data-recid', data.recordState.id);
                $(row).addClass('surpath-drill-row');
                var _badge = $(row).find('.record-status-badge')
                $(_badge).attr('data-recid', data.recordState.id);


                $(row).on('click', function (e) {
                    e.preventDefault();
                    var _row = $(e.target).closest('tr');
                    //var _rid = $(e.target).data('recid');
                    var _rid = $(_row).data('recid');
                    
                    var _uid = $(_row).data('cohortuserid');
                    window.location = '/App/RecordStates/ReviewRecordState?id=' + _rid;

                })


                //$(_badge).on('click', function (e) {
                //    e.preventDefault();
                //    window.location = '/App/RecordStates/ReviewRecordState?id=' + _rid;
                //});


                //$(row).on('click', function (e) {
                //    if ($(e.target).get(0).tagName != 'TD') return;
                //    e.preventDefault();
                //    //_$target = $(e.target);

                //    //window.location = '/App/CohortUsers/ViewCohortUser/' + _uid + '#documents';
                //    window.location = '/App/RecordStates/ReviewRecordState?id=' + _rid;
                //});
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
                    _surpathToolReviewQueue
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
            _surpathToolReviewQueue
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

        // $('.sys input[type=text]

        $('.GetRecordStatesButtonFilterArea input').keypress(function (e) {
            if (e.which === 13 && $(e.target.closest('.form')).hasClass('GetRecordStatesButtonFilterArea')) {
                e.preventDefault();
                //debugger;
                getRecordStates();
            }
            
        })
    });
})();
