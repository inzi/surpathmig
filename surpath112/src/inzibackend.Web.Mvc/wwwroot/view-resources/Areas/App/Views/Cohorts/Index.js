(function () {
    $(function () {
        var _$cohortsTable = $('#CohortsTable');
        var _cohortsService = abp.services.app.cohorts;
        var _entityTypeFullName = 'inzibackend.Surpath.Cohort';

        $('.date-picker').daterangepicker({
            singleDatePicker: true,

            locale: { format: 'MM/DD/YYYY', },
        });

        app.daterangefilterhelper.fixfilters();

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Cohorts.Create'),
            edit: abp.auth.hasPermission('Pages.Cohorts.Edit'),
            delete: abp.auth.hasPermission('Pages.Cohorts.Delete'),
            migrate: abp.auth.hasPermission('Pages.Cohorts.Migrate'),
            transferUsers: abp.auth.hasPermission('Pages.CohortUsers.Transfer'),
        };


        console.log('Permission check!!!');
        console.log(_permissions.transferUsers);
        console.log(_permissions);

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Cohorts/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Cohorts/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditCohortModal',
        });

        var _viewCohortModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Cohorts/ViewcohortModal',
            modalClass: 'ViewCohortModal',
        });

        var _migrationWizardModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Cohorts/MigrationWizardModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Cohorts/_MigrationWizardModal.js',
            modalClass: 'CohortMigrationWizardModal',
            modalSize: 'modal-fullscreen'
        });

        var _userTransferWizardModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Cohorts/UserTransferWizardModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Users/_TransferWizardModal.js',
            modalClass: 'UserTransferWizardModal',
            modalSize: 'modal-fullscreen'
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
        var _openCohorts = [];
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

        abp.event.on('cohortUserMoved', function (data) {
            //console.log('cohortUserMoved', data);
            getCohorts();
        });

        var dataTable = _$cohortsTable.DataTable({
            paging: true,
            lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
            pageLength: 5000,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _cohortsService.getAll,
                inputFilter: function () {
                    var retval = 
                     {
                        filter: $('#CohortsTableFilter').val(),
                        nameFilter: $('#NameFilterId').val(),
                        descriptionFilter: $('#DescriptionFilterId').val(),
                        defaultCohortFilter: $('#DefaultCohortFilterId').val(),
                        tenantDepartmentNameFilter: $('#TenantDepartmentNameFilterId').val(),
                    };
                    if (_openCohorts.length > 0) {
                        retval.reOpen = _openCohorts;
                    }
                    return retval;
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
                                    _viewCohortModal.open({ id: data.record.cohort.id });
                                },
                            },
                            {
                                text: app.localize('Edit'),
                                iconStyle: 'far fa-edit mr-2',
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({ id: data.record.cohort.id });
                                },
                            },
                            {
                                text: app.localize('MigrateCohort'),
                                iconStyle: 'fas fa-exchange-alt mr-2',
                                visible: function () {
                                    return _permissions.migrate;
                                },
                                action: function (data) {
                                    _migrationWizardModal.open({ cohortId: data.record.cohort.id });
                                },
                            },
                            {
                                text: app.localize('TransferUsers'),
                                iconStyle: 'fas fa-users mr-2',
                                visible: function () {
                                    return _permissions.transferUsers;
                                },
                                action: function (data) {
                                    _userTransferWizardModal.open({ cohortId: data.record.cohort.id });
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
                                        entityId: data.record.cohort.id,
                                    });
                                },
                            },
                            {
                                text: app.localize('Delete'),
                                iconStyle: 'far fa-trash-alt mr-2',
                                visible: function (data) {
                                    if (data.record.cohort.defaultCohort == true) return false;
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    if (data.record.cohortusersCount > 0) {
                                        abp.notify.error(app.localize('CohortHasUsersCannotDelete'));
                                    }
                                    else {
                                        deleteCohort(data.record.cohort);
                                    };
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
                    visible: abp.auth.hasPermission('Pages.CohortUsers'),
                    render: function () {
                        return `<button class="btn btn-primary btn-sm Edit_CohortUser_CohortId">${app.localize(
                            'Members'
                        )}</button>`;
                    },
                },
                {
                    targets: 3,
                    data: 'cohort.name',
                    name: 'name',
                },
                {
                    targets: 4,
                    data: 'cohort.description',
                    name: 'description',
                },
                {
                    targets: 5,
                    data: 'cohort.defaultCohort',
                    name: 'defaultCohort',
                    render: function (defaultCohort) {
                        if (defaultCohort) {
                            return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
                        }
                        return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
                    },
                },
                {
                    targets: 6,
                    data: 'tenantDepartmentName',
                    name: 'tenantDepartmentFk.name',
                },
                {
                    targets: 7,
                    visible: abp.auth.hasPermission('Pages.Administration.Host.Dashboard'),
                    data: 'tenantName',
                    name: 'tenantName',
                },
                //{
                //    targets: 7,
                //    visible: abp.auth.hasPermission('Pages.Administration.Host.Dashboard'),
                //    data: null,
                //    //data: 'tenantId',
                //    name: 'tenantId',
                //    render: function (data, row, full) {
                //        //debugger;
                //        return data.tenantName;
                //    }
                //},
            ],
            createdRow: function (row, data, index) {
                ////console.log(row);
                //console.log(data);
                ////console.log(index);
                var _dataid = 'cohortid';
                $(row).attr('data-' + _dataid, data.cohort.id);
                $(row).addClass('cohortparentrow');
                if (data.expandAndSearch == true) {
                    $(row).addClass('expandAndSearch');
                    //$(row).addClass('shown');
                }
                // openDetailRow(this, '/App/MasterDetailChild_Cohort_CohortUsers?CohortId=' + data.cohort.id);
                //openDetailRowAction(row, '/App/MasterDetailChild_Cohort_CohortUsers?CohortId=' + data.cohort.id)
                //////// $(row).attr('data-id', )
                //$(row).on('click', function () {
                //    /*console.log(this);*/
                //    var _id = $(this).data(_dataid);
                //    //console.log('go get cohort users for ' + $(this).data('cohort-id'));
                //    //window.location = '/App/CohortUsers?id=' + _id;
                //    window.location = '/App/CohortUsers/ViewCohortUser/' + _id;

                //});
            },
        });

        function getCohorts() {
            dataTable.ajax.reload();
        }

        function deleteCohort(cohort) {
            abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
                if (isConfirmed) {
                    _cohortsService
                        .delete({
                            id: cohort.id,
                        })
                        .done(function () {
                            getCohorts(true);
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

        $('#CreateNewCohortButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportCohortsButton').click(function () {
            _cohortsService
                .getCohortsToExcel({
                    filter: $('#CohortsTableFilter').val(),
                    nameFilter: $('#NameFilterId').val(),
                    descriptionFilter: $('#DescriptionFilterId').val(),
                    defaultCohortFilter: $('#DefaultCohortFilterId').val(),
                    tenantDepartmentNameFilter: $('#TenantDepartmentNameFilterId').val(),
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#ExportCohortsComplianceSummaryButton').click(function () {
            abp.services.app.cohortUsers
                .getCohortUsersToExcel({
                    filter: $('#CohortsTableFilter').val(),
                    nameFilter: $('#NameFilterId').val(),
                    descriptionFilter: $('#DescriptionFilterId').val(),
                    defaultCohortFilter: $('#DefaultCohortFilterId').val(),
                    tenantDepartmentNameFilter: $('#TenantDepartmentNameFilterId').val(),
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#ExportCohortsImmunizationReportButton').click(function () {
            _cohortsService
                .getCohortsImmunizationReportToExcel({
                    filter: $('#CohortsTableFilter').val(),
                    nameFilter: $('#NameFilterId').val(),
                    descriptionFilter: $('#DescriptionFilterId').val(),
                    defaultCohortFilter: $('#DefaultCohortFilterId').val(),
                    tenantDepartmentNameFilter: $('#TenantDepartmentNameFilterId').val(),
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditCohortModalSaved', function () {
            getCohorts();
        });

        $('#GetCohortsButton').click(function (e) {
            e.preventDefault();
            getCohorts();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getCohorts();
            }
        });

        // var currentOpenedDetailRow;
        var VisibleRows = [];

        function openDetailRow(e, url) {
            var tr = $(e).closest('tr');
            var row = dataTable.row(tr);
            var _idx = $.inArray(VisibleRows, row.data().cohort.id);
            var _cohortId = row.data().cohort.id;
            if (row.child.isShown()) {
                _openCohorts = jQuery.grep(_openCohorts, function (value) {
                    return value != _cohortId;
                });
                row.child.hide();
                tr.removeClass('shown');
                //currentOpenedDetailRow = null;
                VisibleRows.splice(_idx, 1);
            } else {
                _openCohorts.push(_cohortId);
                //if (currentOpenedDetailRow) currentOpenedDetailRow.child.hide();
                $.get(url).then((data) => {
                    var _children = row.child(data, 'cohortchildrow').show();
                    $(row.child()[0]).data('cohortid', row.data().cohort.id);
                    tr.addClass('shown');
                    if (_idx === -1) {
                        VisibleRows.push(row.data().cohort.id);
                    }
                });
            }
        }

        _$cohortsTable.on('click', '.Edit_CohortUser_CohortId', function () {
            var tr = $(this).closest('tr');
            var row = dataTable.row(tr);
            
            openDetailRow(this, '/App/MasterDetailChild_Cohort_CohortUsers?CohortId=' + row.data().cohort.id);
        });

        _$cohortsTable.on('draw.dt', function () {
            console.log('draw.dt');
            DoRowExpansion();
        });

        function DoRowExpansion() {
            _rows = $('.expandAndSearch');
            if (_rows.length < 1) {
                return;
            }
            $('#NoCohortsfoundSearchingUsers').removeClass("d-none");
            $('.expandAndSearch').removeClass('expandAndSearch');
            _rows.each(function (idx) {
                var thisrow = dataTable.row(this);
                var _btn = $(this).find('.Edit_CohortUser_CohortId').click();
            //    var _url = '/App/MasterDetailChild_Cohort_CohortUsers?CohortId=' + thisrow.data().cohort.id;
            //    openDetailRow(thisrow, _url);
            });
            //DoRowExpansions();

        }
    });
})();