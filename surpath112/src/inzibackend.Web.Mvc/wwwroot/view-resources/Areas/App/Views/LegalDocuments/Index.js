(function () {
    $(function () {
        var _$legalDocumentsTable = $('#LegalDocumentsTable');
        var _legalDocumentsService = abp.services.app.legalDocuments;
        var _$legalDocumentsTableFilter = $('#LegalDocumentsTableFilter');
        var _$typeFilterSelect = $('#TypeFilterId');
        var _documentTypeNames = {
            0: app.localize('PrivacyPolicy'),
            1: app.localize('TermsOfService')
        };

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.LegalDocuments.CreateEdit'),
            edit: abp.auth.hasPermission('Pages.Administration.LegalDocuments.CreateEdit'),
            delete: abp.auth.hasPermission('Pages.Administration.LegalDocuments.CreateEdit')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LegalDocuments/CreateOrEdit',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LegalDocuments/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditLegalDocumentModal'
        });

        var dataTable = _$legalDocumentsTable.DataTable({
            paging: false,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _legalDocumentsService.getAll,
                inputFilter: function () {
                    return {
                        filter: _$legalDocumentsTableFilter.val(),
                        typeFilter: _$typeFilterSelect.val() ? parseInt(_$typeFilterSelect.val()) : undefined
                    };
                }
            },
            columnDefs: [
                {
                    className: 'control responsive',
                    orderable: false,
                    render: function () {
                        return '';
                    },
                    targets: 0
                },
                {
                    width: 120,
                    targets: 1,
                    data: 'legalDocument.type',
                    name: 'type',
                    render: function (type) {
                        if (type === 0) {
                            return '<span class="badge badge-info">' + _documentTypeNames[0] + '</span>';
                        } else if (type === 1) {
                            return '<span class="badge badge-success">' + _documentTypeNames[1] + '</span>';
                        }
                        return type;
                    }
                },
                {
                    targets: 2,
                    data: 'legalDocument.fileName',
                    name: 'fileName'
                },
                {
                    targets: 3,
                    data: 'legalDocument.creationTime',
                    name: 'creationTime',
                    render: function (creationTime) {
                        return moment(creationTime).format('L LT');
                    }
                },
                {
                    targets: 4,
                    data: null,
                    orderable: false,
                    defaultContent: '',
                    rowAction: {
                        dropDownStyle: false,
                        cssClass: 'btn btn-primary',
                        items: [
                            {
                                text: app.localize('View'),
                                visible: function (data) {
                                    return abp.auth.hasPermission('Pages.Administration.LegalDocuments.View');
                                },
                                action: function (data) {
                                    if (data.record.legalDocument.externalUrl) {
                                        window.open(data.record.legalDocument.externalUrl, '_blank');
                                    } else {
                                        window.open(abp.appPath + 'App/LegalDocuments/View/' + data.record.legalDocument.type, '_blank');
                                    }
                                }
                            },
                            {
                                text: app.localize('Download'),
                                visible: function (data) {
                                    return abp.auth.hasPermission('Pages.Administration.LegalDocuments.View') && data.record.legalDocument.fileId;
                                },
                                action: function (data) {
                                    window.location.href = abp.appPath + 'App/LegalDocuments/Download/' + data.record.legalDocument.id;
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    window.location.href = abp.appPath + 'App/LegalDocuments/CreateOrEdit/' + data.record.legalDocument.id;
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteLegalDocument(data.record.legalDocument);
                                }
                            }
                        ]
                    }
                }
            ]
        });

        function getLegalDocuments() {
            dataTable.ajax.reload();
        }

        function deleteLegalDocument(legalDocument) {
            var documentTypeName = legalDocument.type === 0 ? app.localize('PrivacyPolicy') : app.localize('TermsOfService');
            var warningMessage = app.localize('LegalDocumentDeleteWarningMessage', documentTypeName, legalDocument.fileName);
            
            _legalDocumentsService.isLastDocumentOfType({
                type: legalDocument.type
            }).done(function(result) {
                if (result) {
                    warningMessage += '<br/><br/><strong class="text-danger">' + 
                        app.localize('LastDocumentOfTypeWarning', documentTypeName) + '</strong>';
                }
                
                abp.message.confirm(
                    warningMessage,
                    app.localize('AreYouSure'),
                    function (isConfirmed) {
                        if (isConfirmed) {
                            abp.ui.setBusy();
                            _legalDocumentsService.delete({
                                id: legalDocument.id
                            }).done(function () {
                                abp.ui.clearBusy();
                                console.log('Legal################ deleteLegalDocument');
                                getLegalDocuments();
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            }).fail(function() {
                                abp.ui.clearBusy();
                                abp.notify.error(app.localize('ErrorDeletingDocument'));
                            });
                        }
                    }
                );
            });
        }

        $('#CreateNewLegalDocumentButton').click(function () {
            window.location.href = abp.appPath + 'App/LegalDocuments/CreateOrEdit';
        });

        $('#GetLegalDocumentsButton').click(function (e) {
            e.preventDefault();
            console.log('Legal################ GetLegalDocumentsButton');
            getLegalDocuments();
        });

        $('#LegalDocumentsTableFilter').on('keypress', function (e) {
            if (e.keyCode === 13) {
                console.log('Legal################ LegalDocumentsTableFilter');
                getLegalDocuments();
            }
        });

        _$typeFilterSelect.change(function () {
            console.log('Legal################ filter change');

            getLegalDocuments();
        });

        abp.event.on('app.createOrEditLegalDocumentModalSaved', function () {
            console.log('Legal################ createOrEditLegalDocumentModalSaved');

            getLegalDocuments();
        });
        console.log('Legal################ Inital Load');

        getLegalDocuments();
    });
})();
