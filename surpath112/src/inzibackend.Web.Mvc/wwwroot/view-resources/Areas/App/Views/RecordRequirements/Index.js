(function () {
    $(function () {
        var _$recordRequirementsTable = $('#RecordRequirementsTable');
        var _recordRequirementsService = abp.services.app.recordRequirements;
        var _entityTypeFullName = 'inzibackend.Surpath.RecordRequirement';

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
                getRecordRequirements();
            })
            .on('cancel.daterangepicker', function (ev, picker) {
                $(this).val('');
                $selectedDate.startDate = null;
                getRecordRequirements();
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
                getRecordRequirements();
            })
            .on('cancel.daterangepicker', function (ev, picker) {
                $(this).val('');
                $selectedDate.endDate = null;
                getRecordRequirements();
            });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.RecordRequirements.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.RecordRequirements.Edit'),
            delete: abp.auth.hasPermission('Pages.Administration.RecordRequirements.Delete'),
            manageCategories: abp.auth.hasPermission('Pages.Administration.RecordRequirements.ManageCategories'),
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RecordRequirements/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RecordRequirements/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditRecordRequirementModal',
        });

        var _createEditRequirementModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Compliance/CreateEditRequirementModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Compliance/_CreateEditRequirementModal.js',
            modalClass: 'CreateEditRequirementModal',
        });

        var _viewRecordRequirementModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RecordRequirements/ViewrecordRequirementModal',
            modalClass: 'ViewRecordRequirementModal',
        });

        var _categoryManagementModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RecordRequirements/CategoryManagementModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RecordRequirements/_CategoryManagementModal.js',
            modalClass: 'CategoryManagementModal',
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

        var dataTable = _$recordRequirementsTable.DataTable({
            paging: true,
            lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
            pageLength: 5000,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _recordRequirementsService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#RecordRequirementsTableFilter').val(),
                        nameFilter: $('#NameFilterId').val(),
                        descriptionFilter: $('#DescriptionFilterId').val(),
                        metadataFilter: $('#MetadataFilterId').val(),
                        isSurpathOnlyFilter: $('#IsSurpathOnlyFilterId').val(),
                        tenantDepartmentNameFilter: $('#TenantDepartmentNameFilterId').val(),
                        cohortNameFilter: $('#CohortNameFilterId').val(),
                        surpathServiceNameFilter: $('#SurpathServiceNameFilterId').val(),
                        tenantSurpathServiceNameFilter: $('#TenantSurpathServiceNameFilterId').val(),
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
                                    _viewRecordRequirementModal.open({ id: data.record.recordRequirement.id });
                                },
                            },
                            //{
                            //  text: app.localize('Edit'),
                            //  visible: function () {
                            //    return _permissions.edit;
                            //  },
                            //  action: function (data) {
                            //      _createEditRequirementModal.open({ id: data.record.recordRequirement.id });
                            //  },
                            //},
                            {
                                text: app.localize('Edit'),
                                iconStyle: 'far fa-edit mr-2',
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createEditRequirementModal.open({ id: data.record.recordRequirement.id });
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
                                        entityId: data.record.recordRequirement.id,
                                    });
                                },
                            },
                            {
                                text: app.localize('ManageCategories'),
                                iconStyle: 'fa fa-folder mr-2',
                                visible: function () {
                                    return _permissions.manageCategories;
                                },
                                action: function (data) {
                                    _categoryManagementModal.open({ 
                                        requirementId: data.record.recordRequirement.id 
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
                                    deleteRecordRequirement(data.record.recordRequirement);
                                },
                            },
                        ],
                    },
                },
                //{
                //  className: 'details-control',
                //  targets: 2,
                //  orderable: false,
                //  autoWidth: false,
                //  visible: abp.auth.hasPermission('Pages.RecordCategories'),
                //  render: function () {
                //    return `<button class="btn btn-primary btn-xs Edit_RecordCategory_RecordRequirementId">${app.localize(
                //      'EditRecordCategory'
                //    )}</button>`;
                //  },
                //},
                {
                    targets: 2,
                    data: null, //'recordRequirement.name',
                    //name: 'name',
                    render: function (data) {
                        var retval = "";
                        retval = retval + data.recordRequirement.name;
                        var noRuleCount = 0;
                        if (data.recordRequirement.categoryDTOs.length > 1) {
                            retval = retval + ' <span class="badge badge-sm badge-warning data-bs-toggle="tooltip" title="' + data.recordRequirement.categoryDTOs.length + ' steps must be completed to be compliant!">' + data.recordRequirement.categoryDTOs.length + '</span>';
                        }
                        $.each(data.recordRequirement.categoryDTOs, function (i, v) { if (v.recordCategoryRuleId === null) noRuleCount++; });
                        if (noRuleCount > 0)
                            retval = retval + ' <span class="badge badge-sm badge-danger data-bs-toggle="tooltip" title="This requirement has no rule set, this can produce unexpected results!">!</span>';

                        return retval;
                        //if (abp.features.isEnabled('App.Surpath.DrugTest') == false) {
                        //    return '<span>N/A</span>'
                        //}
                        //if (data.complianceValues.drug == true) {
                        //    return '<span class="w-100 badge badge-sm surscan-compliance-status-badge" style="background-color: rgb(103, 199, 119);">Compliant</span>';
                        //}
                        //return '<span class="w-100 badge badge-sm surscan-compliance-status-badge" style="background-color: rgb(140, 40, 80);">Not Compliant</span>';
                    }
                },
                {
                    targets: 3,
                    data: 'recordRequirement.description',
                    name: 'description',
                },
                //{
                //  targets: 5,
                //  data: 'recordRequirement.metadata',
                //  name: 'metadata',
                //},
                //{
                //  targets: 6,
                //  data: 'recordRequirement.isSurpathOnly',
                //  name: 'isSurpathOnly',
                //  render: function (isSurpathOnly) {
                //    if (isSurpathOnly) {
                //      return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
                //    }
                //    return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
                //  },
                //},
                {
                    targets: 4,
                    visible: { expression: abp.session.multiTenancySide = abp.multiTenancy.sides.HOST, defaultValue: false },
                    data: 'tenantName',
                    name: 'recordRequirement.tenantName',
                },
                {
                    targets: 5,
                    data: 'tenantDepartmentName',
                    name: 'tenantDepartmentFk.name',
                },
                {
                    targets: 6,
                    data: 'cohortName',
                    name: 'cohortFk.name',
                },
                //{
                //  targets: 9,
                //  data: 'surpathServiceName',
                //  name: 'surpathServiceFk.name',
                //},
                //{
                //  targets: 10,
                //  data: 'tenantSurpathServiceName',
                //  name: 'tenantSurpathServiceFk.name',
                //},
            ],
        });

        function getRecordRequirements() {
            dataTable.ajax.reload();
        }

        function deleteRecordRequirement(recordRequirement) {
            abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
                if (isConfirmed) {
                    _recordRequirementsService
                        .delete({
                            id: recordRequirement.id,
                        })
                        .done(function () {
                            getRecordRequirements(true);
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

        $('#CreateNewRecordRequirementButton').click(function () {
            _createOrEditModal.open();
        });

        $('#CreateNewRequirementButton').click(function () {
            _createEditRequirementModal.open({}, function (callback) {
                console.log(callback);
                //debugger;
            });
        });

        $('#ExportToExcelButton').click(function () {
            _recordRequirementsService
                .getRecordRequirementsToExcel({
                    filter: $('#RecordRequirementsTableFilter').val(),
                    nameFilter: $('#NameFilterId').val(),
                    descriptionFilter: $('#DescriptionFilterId').val(),
                    metadataFilter: $('#MetadataFilterId').val(),
                    isSurpathOnlyFilter: $('#IsSurpathOnlyFilterId').val(),
                    tenantDepartmentNameFilter: $('#TenantDepartmentNameFilterId').val(),
                    cohortNameFilter: $('#CohortNameFilterId').val(),
                    surpathServiceNameFilter: $('#SurpathServiceNameFilterId').val(),
                    tenantSurpathServiceNameFilter: $('#TenantSurpathServiceNameFilterId').val(),
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditRecordRequirementModalSaved', function () {
            getRecordRequirements();
        });

        abp.event.on('app.categoryManagementCompleted', function () {
            getRecordRequirements();
        });

        $('#GetRecordRequirementsButton').click(function (e) {
            e.preventDefault();
            getRecordRequirements();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getRecordRequirements();
            }
        });

        $('.reload-on-change').change(function (e) {
            getRecordRequirements();
        });

        $('.reload-on-keyup').keyup(function (e) {
            getRecordRequirements();
        });

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getRecordRequirements();
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

        _$recordRequirementsTable.on('click', '.Edit_RecordCategory_RecordRequirementId', function () {
            var tr = $(this).closest('tr');
            var row = dataTable.row(tr);
            openDetailRow(
                this,
                '/App/MasterDetailChild_RecordRequirement_RecordCategories?RecordRequirementId=' +
                row.data().recordRequirement.id
            );
        });
    });
})();