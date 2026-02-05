(function () {
    $(function () {
        var _$recordNotesTable = $('#RecordNotesTable');
        var _recordNotesService = abp.services.app.recordNotes;
        var _entityTypeFullName = 'inzibackend.Surpath.RecordNote';

        $('.date-picker').daterangepicker({
            singleDatePicker: true,

            locale: { format: 'MM/DD/YYYY', },
        });

        app.daterangefilterhelper.fixfilters();

        var _permissions = {
            create: abp.auth.hasPermission('Pages.RecordNotes.Create'),
            edit: abp.auth.hasPermission('Pages.RecordNotes.Edit'),
            delete: abp.auth.hasPermission('Pages.RecordNotes.Delete'),
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RecordNotes/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RecordNotes/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditRecordNoteModal',
        });

        var _viewRecordNoteModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RecordNotes/ViewrecordNoteModal',
            modalClass: 'ViewRecordNoteModal',
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

        var dataTable = _$recordNotesTable.DataTable({
            language: {
                emptyTable: abp.localization.localize('NoNotesAtThisTime'),
            },
            paging: true,
            lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
            pageLength: 5000,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _recordNotesService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#RecordNotesTableFilter').val(),
                        noteFilter: $('#NoteFilterId').val(),
                        minCreatedFilter: getDateFilter($('#MinCreatedFilterId')),
                        maxCreatedFilter: getMaxDateFilter($('#MaxCreatedFilterId')),
                        authorizedOnlyFilter: $('#AuthorizedOnlyFilterId').val(),
                        hostOnlyFilter: $('#HostOnlyFilterId').val(),
                        sendNotificationFilter: $('#SendNotificationFilterId').val(),
                        recordStateNotesFilter: $('#RecordStateNotesFilterId').val(),
                        userNameFilter: $('#UserNameFilterId').val(),
                        userName2Filter: $('#UserName2FilterId').val(),
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
                                    _viewRecordNoteModal.open({ id: data.record.recordNote.id });
                                },
                            },
                            {
                                text: app.localize('Edit'),
                                iconStyle: 'far fa-edit mr-2',
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({ id: data.record.recordNote.id });
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
                                        entityId: data.record.recordNote.id,
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
                                    deleteRecordNote(data.record.recordNote);
                                },
                            },
                        ],
                    },
                },
                {
                    targets: 2,
                    data: 'recordNote.note',
                    name: 'note',
                },
                {
                    targets: 3,
                    data: 'recordNote.created',
                    name: 'created',
                    render: function (created) {
                        if (created) {
                            return moment(created).format('L');
                        }
                        return '';
                    },
                },
                {
                    targets: 4,
                    data: 'recordNote.authorizedOnly',
                    name: 'authorizedOnly',
                    render: function (authorizedOnly) {
                        if (authorizedOnly) {
                            return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
                        }
                        return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
                    },
                },
                {
                    targets: 5,
                    data: 'recordNote.hostOnly',
                    name: 'hostOnly',
                    render: function (hostOnly) {
                        if (hostOnly) {
                            return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
                        }
                        return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
                    },
                },
                {
                    targets: 6,
                    data: 'recordNote.sendNotification',
                    name: 'sendNotification',
                    render: function (sendNotification) {
                        if (sendNotification) {
                            return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
                        }
                        return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
                    },
                },
                {
                    targets: 7,
                    data: 'recordStateNotes',
                    name: 'recordStateFk.notes',
                },
                {
                    targets: 8,
                    data: 'userName',
                    name: 'userFk.name',
                },
                {
                    targets: 9,
                    data: 'userName2',
                    name: 'notifyUserFk.name',
                },
            ],
        });

        function getRecordNotes() {
            dataTable.ajax.reload();
        }

        function deleteRecordNote(recordNote) {
            abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
                if (isConfirmed) {
                    _recordNotesService
                        .delete({
                            id: recordNote.id,
                        })
                        .done(function () {
                            getRecordNotes(true);
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

        $('#CreateNewRecordNoteButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _recordNotesService
                .getRecordNotesToExcel({
                    filter: $('#RecordNotesTableFilter').val(),
                    noteFilter: $('#NoteFilterId').val(),
                    minCreatedFilter: getDateFilter($('#MinCreatedFilterId')),
                    maxCreatedFilter: getMaxDateFilter($('#MaxCreatedFilterId')),
                    authorizedOnlyFilter: $('#AuthorizedOnlyFilterId').val(),
                    hostOnlyFilter: $('#HostOnlyFilterId').val(),
                    sendNotificationFilter: $('#SendNotificationFilterId').val(),
                    recordStateNotesFilter: $('#RecordStateNotesFilterId').val(),
                    userNameFilter: $('#UserNameFilterId').val(),
                    userName2Filter: $('#UserName2FilterId').val(),
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditRecordNoteModalSaved', function () {
            getRecordNotes();
        });

        $('#GetRecordNotesButton').click(function (e) {
            e.preventDefault();
            getRecordNotes();
        });

        $(document).keypress(function (e) {
            if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
                getRecordNotes();
            }
        });
    });
})();
