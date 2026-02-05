(function () {
    $(function () {
        console.log('my page module');

        var _unclaimed = $('#MasterDetailChild_Cohort_CohortUsersId_Payload.unclaimed');
        _unclaimed.removeClass('unclaimed');
        var _cohortId = '_' + $(_unclaimed).val();

        var _$cohortUsersTable = $('#MasterDetailChild_Cohort_CohortUsersTable' + _cohortId);
        var _cohortUsersService = abp.services.app.cohortUsers;
        var _entityTypeFullName = 'inzibackend.Surpath.CohortUser';

        $('.date-picker').daterangepicker({
            singleDatePicker: true,

            locale: { format: 'MM/DD/YYYY', },
        });

        app.daterangefilterhelper.fixfilters();

        var _permissions = {
            create: abp.auth.hasPermission('Pages.CohortUsers.Create'),
            edit: abp.auth.hasPermission('Pages.CohortUsers.Edit'),
            delete: abp.auth.hasPermission('Pages.CohortUsers.Delete'),
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/MasterDetailChild_Cohort_CohortUsers/CreateOrEditModal',
            scriptUrl:
                abp.appPath + 'view-resources/Areas/App/Views/MasterDetailChild_Cohort_CohortUsers/_CreateOrEditModal.js',
            modalClass: 'MasterDetailChild_Cohort_CreateOrEditCohortUserModal',
        });

        var _viewCohortUserModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/MasterDetailChild_Cohort_CohortUsers/ViewcohortUserModal',
            modalClass: 'MasterDetailChild_Cohort_ViewCohortUserModal',
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

        var dataTable = _$cohortUsersTable.DataTable({
            paging: true,
            lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
            pageLength: 250,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _cohortUsersService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#MasterDetailChild_Cohort_CohortUsersTableFilter' + _cohortId).val(),
                        userNameFilter: $('#MasterDetailChild_Cohort_UserNameFilterId' + _cohortId).val(),
                        cohortIdFilter: $('#MasterDetailChild_Cohort_CohortUsersId' + _cohortId).val(),
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
                                    //_viewCohortUserModal.open({ id: data.record.cohortUser.id });
                                    window.location = '/App/CohortUsers/ViewCohortUser/' + data.record.cohortUser.id;
                                },
                            },
                            {
                                text: app.localize('Edit'),
                                iconStyle: 'far fa-edit mr-2',
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({ id: data.record.cohortUser.id });
                                },
                            },
                            {
                                text: app.localize('Move'),
                                iconStyle: 'fas fa-angle-double-right mr-2',
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {

                                    moveCohortUser(data.record.cohortUser);
                                    // moveCohortUser(data);
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
                                        entityId: data.record.cohortUser.id,
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
                                    deleteCohortUser(data.record.cohortUser);
                                },
                            },
                        ],
                    },
                },
                {
                    targets: 2,
                    data: 'userEditDto.name',
                    name: 'userFk.name',
                },
                {
                    targets: 3,
                    data: 'userEditDto.surname',
                    name: 'userFk.surname',
                },
                {
                    targets: 4,
                    data: null,
                    visible: (abp.session.multiTenancySide == 2 || abp.features.isEnabled('App.Surpath.DrugTest')),
                    render: function (data, row, full) {

                        if (data.complianceValues.drug == true) {
                            return '<span class="w-100 badge badge-sm surscan-compliance-status-badge" style="background-color: rgb(103, 199, 119);">Compliant</span>';
                        }
                        return '<span class="w-100 badge badge-sm surscan-compliance-status-badge" style="background-color: rgb(140, 40, 80);">Not Compliant</span>';
                    }
                },
                {
                    targets: 5,
                    data: null,
                    visible: (abp.session.multiTenancySide == 2 || abp.features.isEnabled('App.Surpath.BackgroundCheck')),
                    render: function (data, row, full) {

                        if (data.complianceValues.background == true) {
                            return '<span class="w-100 badge badge-sm surscan-compliance-status-badge" style="background-color: rgb(103, 199, 119);">Compliant</span>';
                        }
                        return '<span class="w-100 badge badge-sm surscan-compliance-status-badge" style="background-color: rgb(140, 40, 80);">Not Compliant</span>';
                    }
                },
                {
                    targets: 6,
                    data: null,
                    visible: (abp.session.multiTenancySide == 2 || abp.features.isEnabled('App.Surpath.RecordCompliance')),
                    render: function (data, row, full) {
                        
                        if (data.complianceValues.immunization == true) {
                            return '<span class="w-100 badge badge-sm surscan-compliance-status-badge" style="background-color: rgb(103, 199, 119);">Compliant</span>';
                        }
                        return '<span class="w-100 badge badge-sm surscan-compliance-status-badge" style="background-color: rgb(140, 40, 80);">Not Compliant</span>';
                    }
                },
            ],
            createdRow: function (row, data, index) {
                ////console.log(row);
                //console.log(data);
                ////console.log(index);
                //var _dataid = 'cohort-id';
                //$(row).attr('data-' + _dataid, data.cohort.id);
                //$(row).addClass('surpath-drill-row');
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

        function getCohortUsers() {
            dataTable.ajax.reload();
        }

        function deleteCohortUser(cohortUser) {
            abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
                if (isConfirmed) {
                    _cohortUsersService
                        .delete({
                            id: cohortUser.id,
                        })
                        .done(function () {
                            getCohortUsers(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                }
            });
        }

        $('#MasterDetailChild_Cohort_ShowAdvancedFiltersSpan' + _cohortId).click(function () {
            $('#MasterDetailChild_Cohort_ShowAdvancedFiltersSpan' + _cohortId).hide();
            $('#MasterDetailChild_Cohort_HideAdvancedFiltersSpan' + _cohortId).show();
            $('#MasterDetailChild_Cohort_AdvacedAuditFiltersArea' + _cohortId).slideDown();
        });

        $('#MasterDetailChild_Cohort_HideAdvancedFiltersSpan' + _cohortId).click(function () {
            $('#MasterDetailChild_Cohort_HideAdvancedFiltersSpan' + _cohortId).hide();
            $('#MasterDetailChild_Cohort_ShowAdvancedFiltersSpan' + _cohortId).show();
            $('#MasterDetailChild_Cohort_AdvacedAuditFiltersArea' + _cohortId).slideUp();
        });

        $('#CreateNewCohortUserButton' + _cohortId).click(function () {
            _createOrEditModal.open({cohortId: _cohortId});
        });

        $('#ExportCohortComplianceSummaryButton' + _cohortId).click(function () {
            var cohortid = _cohortId.replace('_', '');
            _cohortUsersService
                .getCohortUsersToExcel({
                    filter: $('#CohortUsersTableFilter' + _cohortId).val(),
                    userNameFilter: $('#MasterDetailChild_Cohort_UserNameFilterId' + _cohortId).val(),
                    cohortId: cohortid,
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#ExportCohortImmunizationReportButton' + _cohortId).click(function () {
            var cohortid = _cohortId.replace('_', '');
            _cohortUsersService
                .getCohortUsersImmunizationReportToExcel({
                    filter: $('#CohortUsersTableFilter' + _cohortId).val(),
                    userNameFilter: $('#MasterDetailChild_Cohort_UserNameFilterId' + _cohortId).val(),
                    cohortId: cohortid,
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#ExportToBGCButton' + _cohortId).click(function () {
            var cohortid = _cohortId.replace('_', '');
            _cohortUsersService
                .getCohortUsersToBGCExcel({
                    filter: $('#CohortUsersTableFilter' + _cohortId).val(),
                    userNameFilter: $('#MasterDetailChild_Cohort_UserNameFilterId' + _cohortId).val(),
                    cohortId: cohortid,
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditCohortUserModalSaved', function () {
            getCohortUsers();
        });

        $('#GetCohortUsersButton' + _cohortId).click(function (e) {
            e.preventDefault();
            getCohortUsers();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getCohortUsers();
            }
        });

        var _CohortLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Compliance/CohortLookupTableRegModal',
            scriptUrl:
                abp.appPath + 'view-resources/Areas/App/Views/Compliance/_CohortLookupTableRegModal.js',
            modalClass: 'CohortLookupTableRegModal',
        });

        function moveCohortUser(cohortUser) {
            _CohortLookupTableModal.open(
                { tenantDepartmentId: '', tenantId: cohortUser.tenantId, excludeCohortId: cohortUser.cohortId },
                function (data) {
                    //console.log('moveCohortUser');
                    //console.log(data);
                    //console.log(cohortUser);
                    abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
                        if (isConfirmed) {
                            _cohortUsersService
                                .createOrEdit({
                                    id: cohortUser.id,
                                    cohortId: data.id,
                                    userId: cohortUser.userId
                                })
                                .done(function () {
                                    getCohortUsers(true);
                                    abp.notify.success(app.localize('SuccessfullyMoved'));
                                    abp.event.trigger('cohortUserMoved',data);
                                })
                            //    _cohortUsersService
                            //        .delete({
                            //            id: cohortUser.id,
                            //        })
                            //        .done(function () {
                            //            getCohortUsers(true);
                            //            abp.notify.success(app.localize('SuccessfullyDeleted'));
                            //        });
                        }
                    });
                }
            );


        }


    });
})();
