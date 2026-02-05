(function ($) {
  app.modals.CreateOrEditUserPidModal = function () {
    var _userPidsService = abp.services.app.userPids;

    var _modalManager;
    var _$userPidInformationForm = null;

    var _UserPidpidTypeLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/UserPids/PidTypeLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/UserPids/_UserPidPidTypeLookupTableModal.js',
      modalClass: 'PidTypeLookupTableModal',
    });
    var _UserPiduserLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/UserPids/UserLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/UserPids/_UserPidUserLookupTableModal.js',
      modalClass: 'UserLookupTableModal',
    });

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$userPidInformationForm = _modalManager.getModal().find('form[name=UserPidInformationsForm]');
      _$userPidInformationForm.validate();
    };

    $('#OpenPidTypeLookupTableButton').click(function () {
      var userPid = _$userPidInformationForm.serializeFormToObject();

      _UserPidpidTypeLookupTableModal.open(
        { id: userPid.pidTypeId, displayName: userPid.pidTypeName },
        function (data) {
          _$userPidInformationForm.find('input[name=pidTypeName]').val(data.displayName);
          _$userPidInformationForm.find('input[name=pidTypeId]').val(data.id);
        }
      );
    });

    $('#ClearPidTypeNameButton').click(function () {
      _$userPidInformationForm.find('input[name=pidTypeName]').val('');
      _$userPidInformationForm.find('input[name=pidTypeId]').val('');
    });

    $('#OpenUserLookupTableButton').click(function () {
      var userPid = _$userPidInformationForm.serializeFormToObject();

      _UserPiduserLookupTableModal.open({ id: userPid.userId, displayName: userPid.userName }, function (data) {
        _$userPidInformationForm.find('input[name=userName]').val(data.displayName);
        _$userPidInformationForm.find('input[name=userId]').val(data.id);
      });
    });

    $('#ClearUserNameButton').click(function () {
      _$userPidInformationForm.find('input[name=userName]').val('');
      _$userPidInformationForm.find('input[name=userId]').val('');
    });

    this.save = function () {
      if (!_$userPidInformationForm.valid()) {
        return;
      }
      if ($('#UserPid_PidTypeId').prop('required') && $('#UserPid_PidTypeId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('PidType')));
        return;
      }
      if ($('#UserPid_UserId').prop('required') && $('#UserPid_UserId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
        return;
      }

      var userPid = _$userPidInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _userPidsService
        .createOrEdit(userPid)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditUserPidModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
