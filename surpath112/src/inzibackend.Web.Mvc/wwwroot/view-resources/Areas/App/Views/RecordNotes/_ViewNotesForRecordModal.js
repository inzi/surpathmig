(function ($) {
    app.modals.ViewNotesForRecordModal = function () {
        var _recordNotesService = abp.services.app.recordNotes;
        var _$recordNotesTable = $('#RecordNotesTable');
        var _modalManager;


        this.init = function (modalManager) {

            _modalManager = modalManager;

            var modal = _modalManager.getModal();

            //$(modal).addClass('modal-xl');
        };

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
                        recordStateIdFilter: $('#id').val(),
                        recordCategoryIdFilter: ''
                    };
                },
            },
            columnDefs: [
                {
                    targets: 0,
                    data: null,
                    name: 'userFk.name',
                    render: function (data) {

                        var _$c = $('#NoteCardTemplate').clone();

                        _$c.find('.surpath-username')[0].innerHTML = data.userName;

                        if (data.recordNote.hostOnly == true) {
                            _$c.find('.surpath-note-hostonly').removeClass('d-none');
                        }

                        if (data.recordNote.authorizedOnly == true) {
                            _$c.find('.surpath-note-authorizedOnly').removeClass('d-none');
                        }

                        if (data.recordNote.created) {
                            _$c.find('.NoteCardTemplateAgo')[0].innerHTML = 'Created: ' + moment(data.recordNote.created).format('l LT');
                        }

                        _$c.find('.note-body')[0].innerHTML = data.recordNote.note;

                        _$c.removeClass('d-none');

                        _$c.attr('id', '');

                        return _$c[0].outerHTML; // data.userName;
                    }
                },
            ],
            createdRow: function (row, data, index) {
                //console.log(row);
                //$(row).addClass("notecards-row");
                //console.log(row);
            },
            drawCallback: function (settings) {
                console.log('drawCallback');
                var api = this.api();
                var $table = $(api.table().node());

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

                //sizeTableRows();
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


        $('#CreateNewRecordNoteButton').on('click', function () {
            _addNoteToRecordStateModal.open({
                id: $('#id').val()
            });
        })

    }
})(jQuery);