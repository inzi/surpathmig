(function () {
  $(function () {
    var _$recordNotesTable = $('#MasterDetailChild_RecordState_RecordNotesTable');
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
      viewUrl: abp.appPath + 'App/MasterDetailChild_RecordState_RecordNotes/CreateOrEditModal',
      scriptUrl:
        abp.appPath + 'view-resources/Areas/App/Views/MasterDetailChild_RecordState_RecordNotes/_CreateOrEditModal.js',
      modalClass: 'MasterDetailChild_RecordState_CreateOrEditRecordNoteModal',
    });

    var _viewRecordNoteModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/MasterDetailChild_RecordState_RecordNotes/ViewrecordNoteModal',
      modalClass: 'MasterDetailChild_RecordState_ViewRecordNoteModal',
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
        paging: true,
        lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
        pageLength: 5000,
      serverSide: true,
        processing: true,

      listAction: {
        ajaxFunction: _recordNotesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#MasterDetailChild_RecordState_RecordNotesTableFilter').val(),
            noteFilter: $('#MasterDetailChild_RecordState_NoteFilterId').val(),
            minCreatedFilter: getDateFilter($('#MasterDetailChild_RecordState_MinCreatedFilterId')),
            maxCreatedFilter: getMaxDateFilter($('#MasterDetailChild_RecordState_MaxCreatedFilterId')),
            hostOnlyFilter: $('#MasterDetailChild_RecordState_HostOnlyFilterId').val(),
            authorizedOnlyFilter: $('#MasterDetailChild_RecordState_AuthorizedOnlyFilterId').val(),
            userName2Filter: $('#MasterDetailChild_RecordState_UserName2FilterId').val(),
            sendNotificationFilter: $('#MasterDetailChild_RecordState_SendNotificationFilterId').val(),
            userNameFilter: $('#MasterDetailChild_RecordState_UserNameFilterId').val(),
            recordStateIdFilter: $('#MasterDetailChild_RecordState_RecordNotesId').val(),
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
                targets: 1,
                data: 'userName',
                name: 'userFk.name',
            },
            {
                targets: 2,
                data: 'recordNote.note',
                name: 'note',
                render: function (note) {
                   // return '<textarea>' + note + '</textarea>';
                    return '<div style="white-space:pre">' + note + '</div>'
                }
            },
            {
                targets: 3,
                data: 'recordNote.created',
                name: 'created',
                render: function (created) {
                    if (created) {
                        return moment(created).format('L LT');
                    }
                    return '';
                },
            },
        ],
        language: {
            emptyTable: 'Nothing here', // abp.localization.localize('NoNotesAtThisTime'),
        },
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

    $('#MasterDetailChild_RecordState_ShowAdvancedFiltersSpan').click(function () {
      $('#MasterDetailChild_RecordState_ShowAdvancedFiltersSpan').hide();
      $('#MasterDetailChild_RecordState_HideAdvancedFiltersSpan').show();
        $('#MasterDetailChild_RecordState_AdvacedAuditFiltersArea').slideDown();
    });

    $('#MasterDetailChild_RecordState_HideAdvancedFiltersSpan').click(function () {
      $('#MasterDetailChild_RecordState_HideAdvancedFiltersSpan').hide();
      $('#MasterDetailChild_RecordState_ShowAdvancedFiltersSpan').show();
      $('#MasterDetailChild_RecordState_AdvacedAuditFiltersArea').slideUp();
    });

    $('#CreateNewRecordNoteButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _recordNotesService
        .getRecordNotesToExcel({
          filter: $('#RecordNotesTableFilter').val(),
          noteFilter: $('#MasterDetailChild_RecordState_NoteFilterId').val(),
          minCreatedFilter: getDateFilter($('#MasterDetailChild_RecordState_MinCreatedFilterId')),
          maxCreatedFilter: getMaxDateFilter($('#MasterDetailChild_RecordState_MaxCreatedFilterId')),
          authorizedOnlyFilter: $('#MasterDetailChild_RecordState_AuthorizedOnlyFilterId').val(),
          hostOnlyFilter: $('#MasterDetailChild_RecordState_HostOnlyFilterId').val(),
          sendNotificationFilter: $('#MasterDetailChild_RecordState_SendNotificationFilterId').val(),
          userNameFilter: $('#MasterDetailChild_RecordState_UserNameFilterId').val(),
          userName2Filter: $('#MasterDetailChild_RecordState_UserName2FilterId').val(),
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
      
      $('.GetRecordNotesButtonFilterArea input').keypress(function (e) {
          if (e.which === 13) {
              e.preventDefault();
              getRecordNotes();
          }
          //if (e.which === 13 && $(e.target.closest('.form')).hasClass('GetRecordNotesButtonFilterArea')) {
          //    e.preventDefault();
          //    debugger;
          //    getRecordStates();
          //}
      })
      $('#MasterDetailChild_RecordState_AdvacedAuditFiltersArea').slideUp();
  });
})();
