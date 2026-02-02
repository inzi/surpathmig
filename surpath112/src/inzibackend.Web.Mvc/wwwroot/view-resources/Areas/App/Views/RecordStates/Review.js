(function () {
    $(function () {
        //var _recordsService = abp.services.app.records;
        var _recordNotesService = abp.services.app.recordNotes;
        var _recordStatesService = abp.services.app.recordStates;

        var _$recordNotesTable = $('#RecordNotesTable');

        var _$ReviewRecordStateForm = $('form[name=ReviewRecordStateForm]');
        _$ReviewRecordStateForm.validate();

        var $ExpirationDates = {
            EffectiveDate: null,
            ExpirationDate: null,
        };

        Inputmask({
            "mask": "99/99/9999"
        }).mask(".date-picker");

        $('#expirePreset').on('change', function () {
            var _val = $(this).val();
            console.log(_val);
            var _minDate = moment($('#RecordState_RecordDto_EffectiveDate').val(), "MM/DD/YYYY");
            switch (_val) {
                case '1y':
                    //execute code block 1
                    _minDate.add(1, 'years');
                    break;
                case '2y':
                    _minDate.add(2, 'years');
                    //execute code block 2
                    break;
                case '10y':
                    _minDate.add(10, 'years');
                    //execute code block 2
                    break;
                case '9m':
                    _minDate.add(9, 'months');
                    //execute code block 2
                    break;
                case '6m':
                    _minDate.add(6, 'months');
                    //execute code block 2
                    break;
                case '3m':
                    _minDate.add(3, 'months');
                    //execute code block 2
                    break;
                case '1m':
                    _minDate.add(1, 'months');
                    //execute code block 2
                    break;
                default:
            }
            // (1mo, 3mo, 6mo, 9mo 1yr, 2yr, 10yr)-->
            $('#RecordState_RecordDto_ExpirationDate').data('daterangepicker').minDate = _minDate;
            $('#RecordState_RecordDto_ExpirationDate').val(_minDate.format('L'));
            $('#RecordState_RecordDto_ExpirationDate').data('daterangepicker').setStartDate(_minDate);
        });

        var _RecordState_RecordDto_EffectiveDate = '';
        $('#RecordState_RecordDto_EffectiveDate').on('blur change keyup', function (ev) {
            //var _minDate = moment($(ev.target).val(), "MM/DD/YYYY");
            console.log('RecordState_RecordDto_EffectiveDate blur');
            checkEffectiveDate(ev);
        });

        function checkEffectiveDate(ev) {
            //console.log('checkEffectiveDate');
            var _val = $(ev.target).val();
            //console.log(_val);
            //debugger;
            console.log(moment($(ev.target).val(), "MM/DD/YYYY", true).isValid());
            if ($(ev.target).val() === '' || moment($(ev.target).val(), "MM/DD/YYYY", true).isValid() == false) {
                $('#expirePreset').attr('disabled', true);
                $('#RecordState_RecordDto_ExpirationDate').attr('disabled', true);
            }
            else {
                var _minDate = moment($(ev.target).val(), "MM/DD/YYYY");

                $('#RecordState_RecordDto_ExpirationDate').val(_minDate.format('L'));
                $('#RecordState_RecordDto_ExpirationDate').data('daterangepicker').setStartDate(_minDate);
                $('#expirePreset').attr('disabled', false);
                $('#RecordState_RecordDto_ExpirationDate').attr('disabled', false);
            }
        }

        $('#RecordState_RecordDto_EffectiveDate')
            .daterangepicker({
                autoUpdateInput: false,
                autoApply: true,
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L',
            })
            .on('show.daterangepicker', function (ev, picker) {
                _RecordState_RecordDto_EffectiveDate = $(ev.target).val();
            })
            .on('apply.daterangepicker', (ev, picker) => {
                $ExpirationDates.EffectiveDate = picker.startDate;
                //$('.effectivedate').val(picker.startDate.format('L')));
                // $selectedDate.startDate = picker.startDate;
                //getLedgerEntries();
                $(ev.target).val(picker.startDate.format('L'));
                $('#RecordState_RecordDto_ExpirationDate').val('');
                $('#RecordState_RecordDto_ExpirationDate').data('daterangepicker').minDate = picker.startDate;
                $('#expirePreset').attr('disabled', false);
                $('#RecordState_RecordDto_ExpirationDate').attr('disabled', false);
                //$('#RecordState_RecordDto_ExpirationDate').data('daterangepicker').val('');
                //$('#RecordState_RecordDto_ExpirationDate').data('minDate') = picker.startDate;
                //$('#RecordState_RecordDto_ExpirationDate').data('daterangepicker').minDate = true;
            })
            .on('cancel.daterangepicker', function (ev, picker) {
                $(ev.target).val(_RecordState_RecordDto_EffectiveDate);
                $ExpirationDates.EffectiveDate = null; // picker.ExpirationDate

                $('#RecordState_RecordDto_ExpirationDate').val('');
                if ($(ev.target).val() === '') {
                    $('#expirePreset').attr('disabled', true);
                    $('#RecordState_RecordDto_ExpirationDate').attr('disabled', true);
                }

                //$selectedDate.startDate = null;
                //getLedgerEntries();
            });
        $('#RecordState_RecordDto_ExpirationDate')
            .daterangepicker({
                autoUpdateInput: false,
                autoApply: true,
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            })
            .on('apply.daterangepicker', (ev, picker) => {
                $ExpirationDates.ExpirationDate = picker.startDate;
                $(ev.target).val(picker.startDate.format('L'));

                //$('.effectivedate').val(picker.startDate.format('L')));
                // $selectedDate.startDate = picker.startDate;
                //getLedgerEntries();
            })
            .on('cancel.daterangepicker', function (ev, picker) {
                $(this).val('');
                $ExpirationDates.ExpirationDate = null; // picker.ExpirationDate
                //debugger;
                //$selectedDate.startDate = null;
                //getLedgerEntries();
            });

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

        function commit(recordState) {
            //debugger;
            _recordStatesService
                .createOrEdit(recordState)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    //_modalManager.close();
                    //abp.event.trigger('app.createOrEditRecordStateModalSaved');
                    getRecordNotes();
                })
                .always(function () {
                    /*_modalManager.setBusy(false);*/
                });
        }
        var recordState;
        //abp.event.on('app.AddNoteToRecordStateModalSaved', function () {
        //   // commit(recordState);
        //});
        function save(successCallback) {
            if (!_$ReviewRecordStateForm.valid()) {
                return;
            }
            recordState = _$ReviewRecordStateForm.serializeFormToObject();
            var _idState = recordState.recordStatusId;
            recordState.RecordDto = {};
            recordState.RecordDto.Id = $("#RecordState_RecordDto_Id").val();
            recordState.RecordDto.EffectiveDate = $("#RecordState_RecordDto_EffectiveDate").val();
            recordState.RecordDto.ExpirationDate = $("#RecordState_RecordDto_ExpirationDate").val();

            var _requireNote = $('.recordstate_' + _idState).val().toLowerCase() == "true";
            var _DisplayName = $('.recordstate_' + _idState).data('displayname');
            if (_requireNote) {
                _addNoteToRecordStateModal.open({
                    id: $('#id').val(),
                    note: abp.localization.localize('StatusSetToNotePrefix') + ' ' + _DisplayName + ' ' + abp.localization.localize('StatusSetToNoteSuffix') + ' ',
                    sendNotification: true
                }, function (s) {
                    commit(recordState);
                });
            }
            else {
                var _statuschangeNote = {
                    "recordStateNotes": "",
                    "RecordStateId": recordState.id,
                    "userId": app.realUserId(),
                    "created": moment().utc().format('M/D/YYYY H:mm:ss A'),
                    "notifyUserId": "",
                    "note": abp.localization.localize('StatusSetToNotePrefix') + ' ' + _DisplayName + ' ' + abp.localization.localize('StatusSetToNoteSuffix'),
                    "sendNotification": "false"
                }
                _recordNotesService
                    .addNoteToRecordState(_statuschangeNote)
                    .done(function () {
                    })
                    .always(function () {
                        commit(recordState);
                    });
            }
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
                    var allRequirementNotes = '';
                    if ($('#allRequirementNotes').is(':checked')) {
                        allRequirementNotes = $('#RecordState_RecordCategoryId').val();
                    }

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
                        recordStateIdFilter: $('#id').val(),
                        recordCategoryIdFilter: allRequirementNotes
                    };
                },
            },
            columnDefs: [
                {
                    //className: "notecards-header",
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

                        //console.log(data);
                        var _$c = $('#NoteCardTemplate').clone();
                        //var _$s = _$c.find('.NoteCardTemplateWho');
                        //_$s.attr("id", "");
                        //debugger;
                        _$c.find('.surpath-username')[0].innerHTML = data.userName;
                        if (data.recordNote.hostOnly == true) {
                            _$c.find('.surpath-note-hostonly').removeClass('d-none');
                        }
                        if (data.recordNote.authorizedOnly == true) {
                            _$c.find('.surpath-note-authorizedOnly').removeClass('d-none');
                        }

                        //var _$t = _$c.find('.NoteCardTemplateAgo');
                        //debugger;
                        if (data.recordNote.created) {
                            //var _m = moment(data.recordNote.created).format('l LT');
                            //return 'Created: ' + moment(data.created).format('l LT');
                            _$c.find('.NoteCardTemplateAgo')[0].innerHTML = 'Created: ' + moment(data.recordNote.created).format('l LT');
                        }
                        _$c.find('.note-body')[0].innerHTML = data.recordNote.note;
                        _$c.removeClass('d-none');
                        _$c.attr('id', '');
                        //////

                        //var _$s = $('#NoteCardTemplateWho').clone();
                        //_$s.attr("id", "");
                        ////_$s.find('.symbol-label')[0].innerHTML = data.userName.slice(0, 1).toUpperCase();
                        //_$s.find('.surpath-username')[0].innerHTML = data.userName;
                        //if (data.recordNote.hostOnly == true) {
                        //    _$s.find('.surpath-note-hostonly').removeClass('d-none');
                        //}
                        //if (data.recordNote.authorizedOnly == true) {
                        //    _$s.find('.surpath-note-authorizedOnly').removeClass('d-none');
                        //}

                        //////

                        //console.log('coldef1');
                        //debugger;

                        return _$c[0].outerHTML; // data.userName;
                    }
                },
                //{
                //    targets: 1,
                //    //className: "notecards-body",
                //    data: 'recordNote.note',
                //    name: 'note',
                //},
                //{
                //    targets: 2,
                //    //className: "notecards-footer",
                //    data: 'recordNote.created',
                //    name: 'created',
                //    render: function (created) {
                //        if (created) {
                //            var _m = moment(created).format('l LT');
                //            return 'Created: ' + moment(created).format('l LT');
                //        }
                //        return '';
                //    },
                //},

            ],
            createdRow: function (row, data, index) {
                //console.log(row);
                //$(row).addClass("notecards-row");
                //console.log(row);
            },
            drawCallback: function (settings) {
                //console.log('drawCallback');
                //console.log(settings);
                var api = this.api();
                var $table = $(api.table().node());
                //var $table = $('#TenantDocumentCategoriesTable')[0];
                //debugger;

                var labels = [];
                $('thead th', $table).each(function () {
                    labels.push($(this).text());
                });

                //console.log(labels);

                $('tbody tr', $table).each(function () {
                    $(this).find('td').each(function (column) {
                        //console.log(this);
                        if ($(this).hasClass('cards-show-label')) {
                            $(this).attr('data-label', labels[column]);
                        }
                    });
                });

                //sizeTableRows();
                /*$('tbody tr', $table).height(max);*/
            }
        });

        function sizeTableRows() {
            return;
            var max = 0;

            var $table = $(dataTableNotes.table().node());

            $('tbody tr', $table).each(function () {
                max = Math.max($(this).height(), max);
            }).height(max);
        }

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

        //$('#allRequirementNotes').on('click', function () {
        //    //e.PreventDefault();
        //    debugger;
        //})

        $('#allRequirementNotes').change(function () {
            //e.PreventDefault();
            //var _chk = $(e.event.target);
            //var _checked = $('#allRequirementNotes').is(':checked')
            //dataTableNotes.clear();
            //dataTableNotes.draw();
            dataTableNotes.ajax.reload();

            //debugger;
        });

        // END NOTES

        $('#CreateNewRecordNoteButton').on('click', function () {
            _addNoteToRecordStateModal.open({
                id: $('#id').val()
            });
        })
    });
})();