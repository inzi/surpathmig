(function () {
    $(function () {
        var _$cohortUsersTable = $('#CohortUsersTable');
        var _cohortUsersService = abp.services.app.cohortUsers;
        // Get necessary services and go back and get it?
        var _entityTypeFullName = 'inzibackend.Surpath.CohortUser';
        var isHost = abp.session.multiTenancySide == 2;  
        //debugger;

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
            create: abp.auth.hasPermission('Pages.CohortUsers.Create'),
            edit: abp.auth.hasPermission('Pages.CohortUsers.Edit'),
            delete: abp.auth.hasPermission('Pages.CohortUsers.Delete'),
        };

        var _viewCohortUserModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CohortUsers/ViewcohortUserModal',
            modalClass: 'ViewCohortUserModal',
        });

        var _CohortLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Compliance/CohortLookupTableRegModal',
            scriptUrl:
                abp.appPath + 'view-resources/Areas/App/Views/Compliance/_CohortLookupTableRegModal.js',
            modalClass: 'CohortLookupTableRegModal',
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

        var dataTable = _$cohortUsersTable.DataTable({
            paging: true, 
            lengthMenu: [5, 10, 25, 50, 100, 250, 500,5000],
            pageLength: 25,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _cohortUsersService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#CohortUsersTableFilter').val(),
                        cohortDescriptionFilter: $('#CohortDescriptionFilterId').val(),
                        userNameFilter: $('#UserNameFilterId').val(),
                        cohortId: $('#cohortId').val(),
                        //maxRecordCount: 500,
                        //skipCount: 0
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
                                    //window.location = '/App/CohortUsers/ViewCohortUser/' + data.record.cohortUser.id;
                                    /*console.log(this);*/
                                    //debugger;
                                    //var _id = $(this).data(_dataid);
                                    ////console.log('go get cohort users for ' + $(this).data('cohort-id'));
                                    ////window.location = '/App/CohortUsers?id=' + _id;
                                    window.location = '/App/CohortUsers/ViewCohortUser/' + data.record.cohortUser.id;

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
                                },
                            },
                            //{
                            //    text: app.localize('Edit'),
                            //    iconStyle: 'far fa-edit mr-2',
                            //    visible: function () {
                            //        return _permissions.edit;
                            //    },
                            //    action: function (data) {
                            //        window.location = '/App/CohortUsers/CreateOrEdit/' + data.record.cohortUser.id;
                            //    },
                            //},
                            //{
                            //    text: app.localize('History'),
                            //    iconStyle: 'fas fa-history mr-2',
                            //    visible: function () {
                            //        return entityHistoryIsEnabled();
                            //    },
                            //    action: function (data) {
                            //        _entityTypeHistoryModal.open({
                            //            entityTypeFullName: _entityTypeFullName,
                            //            entityId: data.record.cohortUser.id,
                            //        });
                            //    },
                            //},
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
                    name: 'UserFk.surname',
                },
                {
                    targets: 4,
                    data: 'cohortName',
                    name: 'CohortFk.Name',
                },
                {
                    targets: 5,
                    data: null,
                    visible: (abp.session.multiTenancySide == 2 || abp.features.isEnabled('App.Surpath.DrugTest')),
                    render: function (data) {
                        if (abp.features.isEnabled('App.Surpath.DrugTest') == false) {
                            return '<span>N/A</span>'
                        }
                        if (data.complianceValues.drug  == true) {
                            return '<span class="w-100 badge badge-sm surscan-compliance-status-badge" style="background-color: rgb(103, 199, 119);">Compliant</span>';
                        }
                        return '<span class="w-100 badge badge-sm surscan-compliance-status-badge" style="background-color: rgb(140, 40, 80);">Not Compliant</span>';
                    }
                },
                {
                    targets: 6,
                    data: null,
                    visible: (abp.session.multiTenancySide == 2 || abp.features.isEnabled('App.Surpath.BackgroundCheck')),
                    render: function (data) {
                        if (abp.features.isEnabled('App.Surpath.BackgroundCheck') == false) {
                            return '<span>N/A</span>'
                        }
                        if (data.complianceValues.background  == true) {
                            return '<span class="w-100 badge badge-sm surscan-compliance-status-badge" style="background-color: rgb(103, 199, 119);">Compliant</span>';
                        }
                        return '<span class="w-100 badge badge-sm surscan-compliance-status-badge" style="background-color: rgb(140, 40, 80);">Not Compliant</span>';
                    }
                },
                {
                    targets: 7,
                    data: null,
                    visible: (abp.session.multiTenancySide==2 || abp.features.isEnabled('App.Surpath.RecordCompliance')),
                    render: function (data) {
                        console.log('immunization Compliance check');
                        if (abp.features.isEnabled('App.Surpath.RecordCompliance') == false) {
                            return '<span>N/A</span>'
                        }
                        if (data.complianceValues.immunization == true) {
                            console.log('immunization is true');
                            return '<span class="w-100 badge badge-sm surscan-compliance-status-badge" style="background-color: rgb(103, 199, 119);">Compliant</span>';
                        }
                        console.log('immunization is false');
                        return '<span class="w-100 badge badge-sm surscan-compliance-status-badge" style="background-color: rgb(140, 40, 80);">Not Compliant</span>';
                    }
                },
                {
                    targets: 8,
                    visible: (abp.session.multiTenancySide == 2),
                    data: 'tenantEditDto.tenancyName',
                    name: 'userFk.tenantId',
                    
                },
            ],
            createdRow: function (row, data, index) {
                //console.log(row);
                //console.log(data);
                //console.log(index);
                var _dataid = 'cohortuser-id';
                $(row).attr('data-' + _dataid, data.cohortUser.id);
                $(row).addClass('surpath-drill-row');
                //// $(row).attr('data-id', )
                //$(row).on('click', function () {
                //    /*console.log(this);*/
                //    var _id = $(this).data(_dataid);
                //    //console.log('go get cohort users for ' + $(this).data('cohort-id'));
                //    //window.location = '/App/CohortUsers?id=' + _id;
                //    window.location = '/App/CohortUsers/ViewCohortUser/' + _id;

                //});
            },
        });
        _$cohortUsersTable
            .on('xhr.dt', function (e, settings, json, xhr) {
                console.log('data back');
                console.log(settings);
                console.log(json);
                //sortingUser = settings.rawServerResponse.sortingUser;
                dataTable.rawData = settings.rawServerResponse;
                //debugger;
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

        function moveCohortUser(cohortUser) {
            _CohortLookupTableModal.open(
                { tenantDepartmentId: '', tenantId: cohortUser.tenantId  },
                function (data) {
                    console.log(data);
                    console.log(cohortUser);
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
                                    abp.event.trigger('cohortUserMoved');
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
            _cohortUsersService
                .getCohortUsersToExcel({
                    filter: $('#CohortUsersTableFilter').val(),
                    cohortDescriptionFilter: $('#CohortDescriptionFilterId').val(),
                    userNameFilter: $('#UserNameFilterId').val(),
                    cohortId: $('#cohortId').val(),

                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#ExportToBGCButton').click(function () {
            _cohortUsersService
                .getCohortUsersToBGCExcel({
                    filter: $('#CohortUsersTableFilter').val(),
                    cohortDescriptionFilter: $('#CohortDescriptionFilterId').val(),
                    userNameFilter: $('#UserNameFilterId').val(),
                    cohortId: $('#cohortId').val(),

                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditCohortUserModalSaved', function () {
            getCohortUsers();
        });

        $('#GetCohortUsersButton').click(function (e) {
            e.preventDefault();
            getCohortUsers();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getCohortUsers();
            }
        });


    });
})();
