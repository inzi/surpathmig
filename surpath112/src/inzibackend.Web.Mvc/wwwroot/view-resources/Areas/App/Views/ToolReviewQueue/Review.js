(function () {
    $(function () {
        //var _recordsService = abp.services.app.records;
        var _recordNotesService = abp.services.app.recordNotes;
        var _recordStatesService = abp.services.app.recordStates;

        var _$recordNotesTable = $('#RecordNotesTable');


        var _$ReviewRecordStateForm = $('form[name=ReviewRecordStateForm]');
        _$ReviewRecordStateForm.validate();


        //var _$recordInformationForm = $('form[name=RecordInformationsForm]');
        //_$recordInformationForm.validate();

        //var _RecordtenantDocumentCategoryLookupTableModal = new app.ModalManager({
        //    viewUrl: abp.appPath + 'App/Records/TenantDocumentCategoryLookupTableModal',
        //    scriptUrl:
        //        abp.appPath + 'view-resources/Areas/App/Views/Records/_RecordTenantDocumentCategoryLookupTableModal.js',
        //    modalClass: 'TenantDocumentCategoryLookupTableModal',
        //});
        //var _fileUploading = [];
        //var _filedataToken;

        //$('.date-picker').daterangepicker({
        //    singleDatePicker: true,
        //    locale: abp.localization.currentLanguage.name,
        //    format: 'L',
        //});

        //$('#OpenTenantDocumentCategoryLookupTableButton').click(function () {
        //    var record = _$recordInformationForm.serializeFormToObject();

        //    _RecordtenantDocumentCategoryLookupTableModal.open(
        //        { id: record.tenantDocumentCategoryId, displayName: record.tenantDocumentCategoryName },
        //        function (data) {
        //            _$recordInformationForm.find('input[name=tenantDocumentCategoryName]').val(data.displayName);
        //            _$recordInformationForm.find('input[name=tenantDocumentCategoryId]').val(data.id);
        //        }
        //    );
        //});

        //$('#ClearTenantDocumentCategoryNameButton').click(function () {
        //    _$recordInformationForm.find('input[name=tenantDocumentCategoryName]').val('');
        //    _$recordInformationForm.find('input[name=tenantDocumentCategoryId]').val('');
        //});

        function save(successCallback) {
            if (!_$ReviewRecordStateForm.valid()) {
                return;
            }
            //if ($('#Record_TenantDocumentCategoryId').prop('required') && $('#Record_TenantDocumentCategoryId').val() == '') {
            //    abp.message.error(app.localize('{0}IsRequired', app.localize('TenantDocumentCategory')));
            //    return;
            //}

            //if (_fileUploading != null && _fileUploading.length > 0) {
            //    abp.notify.info(app.localize('WaitingForFileUpload'));
            //    return;
            //}
            var recordState = _$ReviewRecordStateForm.serializeFormToObject();

            _recordStatesService
                .createOrEdit(recordState)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    //_modalManager.close();
                    //abp.event.trigger('app.createOrEditRecordStateModalSaved');
                })
                .always(function () {
                    /*_modalManager.setBusy(false);*/
                });
            //var record = _$recordInformationForm.serializeFormToObject();

            //record.filedataToken = _filedataToken;
            //debugger;

            //abp.ui.setBusy();
            //_recordsService
            //    .createOrEdit(record)
            //    .done(function () {
            //        abp.notify.info(app.localize('SavedSuccessfully'));
            //        abp.event.trigger('app.createOrEditRecordModalSaved');

            //        if (typeof successCallback === 'function') {
            //            successCallback();
            //        }
            //    })
            //    .always(function () {
            //        abp.ui.clearBusy();
            //    });
        }

        //function clearForm() {
        //    _$recordInformationForm[0].reset();
        //}

        $('#saveBtn').click(function () {
            save(function () {
                //window.location = '/App/Records';

            });
        });

        //$('#saveAndNewBtn').click(function () {
        //    save(function () {
        //        if (!$('input[name=id]').val()) {
        //            //if it is create page
        //            clearForm();
        //        }
        //    });
        //});

        //$('#Record_filedata').change(function () {
        //    var file = $(this)[0].files[0];
        //    if (!file) {
        //        _filedataToken = null;
        //        return;
        //    }

        //    var formData = new FormData();
        //    $('#Record_filename').val(file.name);

        //    formData.append('file', file);
        //    _fileUploading.push(true);

        //    $.ajax({
        //        url: '/App/Records/UploadfiledataFile',
        //        type: 'POST',
        //        data: formData,
        //        processData: false,
        //        contentType: false,
        //    })
        //        .done(function (resp) {
        //            if (resp.success && resp.result.fileToken) {
        //                _filedataToken = resp.result.fileToken;
        //            } else {
        //                abp.message.error(resp.result.message);
        //            }
        //        })
        //        .always(function () {
        //            _fileUploading.pop();
        //        });
        //});

        //$('#Record_filedata_Remove').click(function () {
        //    abp.message.confirm(app.localize('DoYouWantToRemoveTheFile'), app.localize('AreYouSure'), function (isConfirmed) {
        //        if (isConfirmed) {
        //            var Record = _$recordInformationForm.serializeFormToObject();
        //            _recordsService
        //                .removefiledataFile({
        //                    id: Record.id,
        //                })
        //                .done(function () {
        //                    abp.notify.success(app.localize('SuccessfullyDeleted'));
        //                    _$recordInformationForm.find('#div_current_file').css('display', 'none');
        //                });
        //        }
        //    });
        //});

        //$('#Record_filedata').change(function () {
        //    var fileName = app.localize('ChooseAFile');
        //    if (this.files && this.files[0]) {
        //        fileName = this.files[0].name;
        //    }
        //    $('#Record_filedataLabel').text(fileName);
        //});



        // NOTES

        var dataTableNotes = _$recordNotesTable.DataTable({
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
                        //filter: $('#RecordNotesTableFilter').val(),
                        //noteFilter: $('#NoteFilterId').val(),
                        //minCreatedFilter: getDateFilter($('#MinCreatedFilterId')),
                        //maxCreatedFilter: getMaxDateFilter($('#MaxCreatedFilterId')),
                        //authorizedOnlyFilter: $('#AuthorizedOnlyFilterId').val(),
                        //hostOnlyFilter: $('#HostOnlyFilterId').val(),
                        //sendNotificationFilter: $('#SendNotificationFilterId').val(),
                        //recordStateNotesFilter: $('#RecordStateNotesFilterId').val(),
                        //userNameFilter: $('#UserNameFilterId').val(),
                        //userName2Filter: $('#UserName2FilterId').val(),
                        recordStateIdFilter: $('RecordState_Id').val()
                    };
                },
            },
            columnDefs: [
                {
                    className: "notecards-header",
                    targets: 0,
                    //data: 'userName',
                    data: null,
                    name: 'userFk.name',
                    render: function (data) {
                        //<div class="symbol symbol-50px me-5">
                        //    <div class="symbol-label fs-1 fw-bold bg-light-success text-success">S</div>
                        //</div>
                        //_$symbol = $('<div class="symbol symbol-50px me-5">');
                        //_$symbollabel = $('<div class="symbol-label fs-1 fw-bold bg-light-success text-success">' + name.slice(0, 1) + '</div>');
                        //_$symbol[0].append(_$symbollabel[0]);
                        //var _$symbol = $($('#NoteCardTemplateWho1')[0].innerHTML);
                        
                        ////_$symbol.attr('id')
                        //return _$symbol[0].outerHTML + name;

                        var _$s = $('#NoteCardTemplateWho').clone();
                        _$s.attr("id", "");
                        _$s.find('.symbol-label')[0].innerHTML = data.userName.slice(0, 1).toUpperCase();
                        _$s.find('.surpath-username')[0].innerHTML = data.userName;
                        //console.log('coldef1');
                        //debugger;

                        return _$s[0].outerHTML; // data.userName;
                    }
                },
                {
                    targets: 1,
                    className: "notecards-body",
                    data: 'recordNote.note',
                    name: 'note',
                },
                {
                    targets: 2,
                    className: "notecards-footer",
                    data: 'recordNote.created',
                    name: 'created',
                    render: function (created) {
                        if (created) {
                            return 'Created: ' + moment(created).format('l LT');
                        }
                        return '';
                    },
                },
               
                
            ],
            createdRow: function (row, data, index) {
                //console.log(row);
                $(row).addClass("notecards-row");
                //console.log(row);
            },
            drawCallback: function (settings) {
                console.log('drawCallback');
                //console.log(settings);
                var api = this.api();
                var $table = $(api.table().node());
                //var $table = $('#TenantDocumentCategoriesTable')[0];
                //debugger;

                var labels = [];
                $('thead th', $table).each(function () {
                    labels.push($(this).text());
                });

                console.log(labels);

                $('tbody tr', $table).each(function () {
                    $(this).find('td').each(function (column) {
                        console.log(this);
                        if ($(this).hasClass('cards-show-label')) {
                            $(this).attr('data-label', labels[column]);
                        }
                    });
                });

                var max = 0;
                $('tbody tr', $table).each(function () {
                    max = Math.max($(this).height(), max);
                }).height(max);
                /*$('tbody tr', $table).height(max);*/
            }
        });

        function getRecordNotes() {
            dataTableNotes.ajax.reload();
        }

        var _addNoteToRecordStateModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RecordNotes/AddNoteToRecordStateModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RecordNotes/_AddNoteToRecordStateModal.js',
            modalClass: 'CreateOrEditRecordNoteModal',
        });

        abp.event.on('app.AddNoteToRecordStateModalSaved', function () {
            getRecordNotes();
        });

        // END NOTES


        $('#CreateNewRecordNoteButton').on('click', function () {
            _addNoteToRecordStateModal.open();
        })
    });
})();
