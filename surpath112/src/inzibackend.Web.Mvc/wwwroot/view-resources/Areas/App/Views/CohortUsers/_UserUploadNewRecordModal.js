(function ($) {
  app.modals.CreateOrEditRecordStateModal = function () {
    var _recordStatesService = abp.services.app.recordStates;

    var _modalManager;
    var _$recordStateInformationForm = null;

    var _RecordStaterecordLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RecordStates/RecordLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RecordStates/_RecordStateRecordLookupTableModal.js',
      modalClass: 'RecordLookupTableModal',
    });
    var _RecordStaterecordCategoryLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RecordStates/RecordCategoryLookupTableModal',
      scriptUrl:
        abp.appPath + 'view-resources/Areas/App/Views/RecordStates/_RecordStateRecordCategoryLookupTableModal.js',
      modalClass: 'RecordCategoryLookupTableModal',
    });
    var _RecordStateuserLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RecordStates/UserLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RecordStates/_RecordStateUserLookupTableModal.js',
      modalClass: 'UserLookupTableModal',
    });
    var _RecordStaterecordStatusLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RecordStates/RecordStatusLookupTableModal',
      scriptUrl:
        abp.appPath + 'view-resources/Areas/App/Views/RecordStates/_RecordStateRecordStatusLookupTableModal.js',
      modalClass: 'RecordStatusLookupTableModal',
    });

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
        modal.find('.date-picker').daterangepicker({
            singleDatePicker: true,

             locale: { format: 'MM/DD/YYYY', },
        });

      _$recordStateInformationForm = _modalManager.getModal().find('form[name=RecordStateInformationsForm]');
      _$recordStateInformationForm.validate();
    };

    $('#OpenRecordLookupTableButton').click(function () {
      var recordState = _$recordStateInformationForm.serializeFormToObject();

      _RecordStaterecordLookupTableModal.open(
        { id: recordState.recordId, displayName: recordState.recordfilename },
        function (data) {
          _$recordStateInformationForm.find('input[name=recordfilename]').val(data.displayName);
          _$recordStateInformationForm.find('input[name=recordId]').val(data.id);
        }
      );
    });

    $('#ClearRecordfilenameButton').click(function () {
      _$recordStateInformationForm.find('input[name=recordfilename]').val('');
      _$recordStateInformationForm.find('input[name=recordId]').val('');
    });

    $('#OpenRecordCategoryLookupTableButton').click(function () {
      var recordState = _$recordStateInformationForm.serializeFormToObject();

      _RecordStaterecordCategoryLookupTableModal.open(
        { id: recordState.recordCategoryId, displayName: recordState.recordCategoryName },
        function (data) {
          _$recordStateInformationForm.find('input[name=recordCategoryName]').val(data.displayName);
          _$recordStateInformationForm.find('input[name=recordCategoryId]').val(data.id);
        }
      );
    });

    $('#ClearRecordCategoryNameButton').click(function () {
      _$recordStateInformationForm.find('input[name=recordCategoryName]').val('');
      _$recordStateInformationForm.find('input[name=recordCategoryId]').val('');
    });

    $('#OpenUserLookupTableButton').click(function () {
      var recordState = _$recordStateInformationForm.serializeFormToObject();

      _RecordStateuserLookupTableModal.open(
        { id: recordState.userId, displayName: recordState.userName },
        function (data) {
          _$recordStateInformationForm.find('input[name=userName]').val(data.displayName);
          _$recordStateInformationForm.find('input[name=userId]').val(data.id);
        }
      );
    });

    $('#ClearUserNameButton').click(function () {
      _$recordStateInformationForm.find('input[name=userName]').val('');
      _$recordStateInformationForm.find('input[name=userId]').val('');
    });

    $('#OpenRecordStatusLookupTableButton').click(function () {
      var recordState = _$recordStateInformationForm.serializeFormToObject();

      _RecordStaterecordStatusLookupTableModal.open(
        { id: recordState.recordStatusId, displayName: recordState.recordStatusStatusName },
        function (data) {
          _$recordStateInformationForm.find('input[name=recordStatusStatusName]').val(data.displayName);
          _$recordStateInformationForm.find('input[name=recordStatusId]').val(data.id);
        }
      );
    });

    $('#ClearRecordStatusStatusNameButton').click(function () {
      _$recordStateInformationForm.find('input[name=recordStatusStatusName]').val('');
      _$recordStateInformationForm.find('input[name=recordStatusId]').val('');
    });

    this.save = function () {
      if (!_$recordStateInformationForm.valid()) {
        return;
      }
      if ($('#RecordState_RecordId').prop('required') && $('#RecordState_RecordId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Record')));
        return;
      }
      if ($('#RecordState_RecordCategoryId').prop('required') && $('#RecordState_RecordCategoryId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('RecordCategory')));
        return;
      }
      if ($('#RecordState_UserId').prop('required') && $('#RecordState_UserId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
        return;
      }
      if ($('#RecordState_RecordStatusId').prop('required') && $('#RecordState_RecordStatusId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('RecordStatus')));
        return;
      }

      var recordState = _$recordStateInformationForm.serializeFormToObject();
        
      _modalManager.setBusy(true);
      _recordStatesService
        .createOrEdit(recordState)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditRecordStateModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
