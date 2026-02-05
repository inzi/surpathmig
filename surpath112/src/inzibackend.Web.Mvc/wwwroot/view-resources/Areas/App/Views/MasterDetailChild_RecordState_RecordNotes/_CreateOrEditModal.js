(function ($) {
  app.modals.MasterDetailChild_RecordState_CreateOrEditRecordNoteModal = function () {
    var _recordNotesService = abp.services.app.recordNotes;

    var _modalManager;
    var _$recordNoteInformationForm = null;

    var _RecordNoteuserLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/MasterDetailChild_RecordState_RecordNotes/UserLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/App/Views/MasterDetailChild_RecordState_RecordNotes/_RecordNoteUserLookupTableModal.js',
      modalClass: 'MasterDetailChild_RecordState_UserLookupTableModal',
    });

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();

        modal.find('.date-time-picker').daterangepicker({
            timePicker: true,
            singleDatePicker: true,
            startDate: moment().startOf('minute'),
            locale: {
                format: 'MM/DD/YYYY hh:mm A',
            },
        });

      _$recordNoteInformationForm = _modalManager.getModal().find('form[name=RecordNoteInformationsForm]');
      _$recordNoteInformationForm.validate();
    };

    $('#OpenUserLookupTableButton').click(function () {
      var recordNote = _$recordNoteInformationForm.serializeFormToObject();

      _RecordNoteuserLookupTableModal.open(
        { id: recordNote.userId, displayName: recordNote.userName },
        function (data) {
          _$recordNoteInformationForm.find('input[name=userName]').val(data.displayName);
          _$recordNoteInformationForm.find('input[name=userId]').val(data.id);
        }
      );
    });

    $('#ClearUserNameButton').click(function () {
      _$recordNoteInformationForm.find('input[name=userName]').val('');
      _$recordNoteInformationForm.find('input[name=userId]').val('');
    });

    $('#OpenUser2LookupTableButton').click(function () {
      var recordNote = _$recordNoteInformationForm.serializeFormToObject();

      _RecordNoteuserLookupTableModal.open(
        { id: recordNote.notifyUserId, displayName: recordNote.userName2 },
        function (data) {
          _$recordNoteInformationForm.find('input[name=userName2]').val(data.displayName);
          _$recordNoteInformationForm.find('input[name=notifyUserId]').val(data.id);
        }
      );
    });

    $('#ClearUserName2Button').click(function () {
      _$recordNoteInformationForm.find('input[name=userName2]').val('');
      _$recordNoteInformationForm.find('input[name=notifyUserId]').val('');
    });

    this.save = function () {
      if (!_$recordNoteInformationForm.valid()) {
        return;
      }
      if ($('#RecordNote_UserId').prop('required') && $('#RecordNote_UserId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
        return;
      }
      if ($('#RecordNote_NotifyUserId').prop('required') && $('#RecordNote_NotifyUserId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
        return;
      }

      var recordNote = _$recordNoteInformationForm.serializeFormToObject();

      recordNote.recordStateId = $('#MasterDetailChild_RecordState_RecordNotesId').val();

      _modalManager.setBusy(true);
      _recordNotesService
        .createOrEdit(recordNote)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditRecordNoteModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
