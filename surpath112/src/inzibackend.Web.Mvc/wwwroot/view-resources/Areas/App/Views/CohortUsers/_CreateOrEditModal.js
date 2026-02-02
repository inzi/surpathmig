(function ($) {
  app.modals.CreateOrEditCohortUserModal = function () {
    var _cohortUsersService = abp.services.app.cohortUsers;

    var _modalManager;
    var _$cohortUserInformationForm = null;

    var _CohortUsercohortLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/CohortUsers/CohortLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CohortUsers/_CohortUserCohortLookupTableModal.js',
      modalClass: 'CohortLookupTableModal',
    });
    var _CohortUseruserLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/CohortUsers/UserLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CohortUsers/_CohortUserUserLookupTableModal.js',
      modalClass: 'UserLookupTableModal',
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

      _$cohortUserInformationForm = _modalManager.getModal().find('form[name=CohortUserInformationsForm]');
      _$cohortUserInformationForm.validate();
    };

    $('#OpenCohortLookupTableButton').click(function () {
      // console.log('OpenCohortLookupTableButton createandeditmodal');
      var cohortUser = _$cohortUserInformationForm.serializeFormToObject();

      _CohortUsercohortLookupTableModal.open(
        { id: cohortUser.cohortId, displayName: cohortUser.cohortName },
        function (data) {
          _$cohortUserInformationForm.find('input[name=cohortName]').val(data.displayName);
          _$cohortUserInformationForm.find('input[name=cohortId]').val(data.id);
        }
      );
    });

    $('#ClearCohortNameButton').click(function () {
      _$cohortUserInformationForm.find('input[name=cohortName]').val('');
      _$cohortUserInformationForm.find('input[name=cohortId]').val('');
    });

    $('#OpenUserLookupTableButton').click(function () {
      var cohortUser = _$cohortUserInformationForm.serializeFormToObject();

      _CohortUseruserLookupTableModal.open(
        { id: cohortUser.userId, displayName: cohortUser.userName },
        function (data) {
          _$cohortUserInformationForm.find('input[name=userName]').val(data.displayName);
          _$cohortUserInformationForm.find('input[name=userId]').val(data.id);
        }
      );
    });

    $('#ClearUserNameButton').click(function () {
      _$cohortUserInformationForm.find('input[name=userName]').val('');
      _$cohortUserInformationForm.find('input[name=userId]').val('');
    });

    this.save = function () {
      if (!_$cohortUserInformationForm.valid()) {
        return;
      }
      if ($('#CohortUser_CohortId').prop('required') && $('#CohortUser_CohortId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Cohort')));
        return;
      }
      if ($('#CohortUser_UserId').prop('required') && $('#CohortUser_UserId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
        return;
      }

      var cohortUser = _$cohortUserInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _cohortUsersService
        .createOrEdit(cohortUser)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditCohortUserModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
