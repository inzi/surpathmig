(function ($) {
  app.modals.MasterDetailChild_Cohort_CreateOrEditCohortUserModal = function () {
    var _cohortUsersService = abp.services.app.cohortUsers;

    var _modalManager;
    var _$cohortUserInformationForm = null;

    var _CohortUseruserLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/MasterDetailChild_Cohort_CohortUsers/UserLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/App/Views/MasterDetailChild_Cohort_CohortUsers/_CohortUserUserLookupTableModal.js',
      modalClass: 'MasterDetailChild_Cohort_UserLookupTableModal',
    });

    this.init = function (modalManager) {
      _modalManager = modalManager;

      // Access the data passed to the modal
      var modalData = _modalManager.getArgs();

      var modal = _modalManager.getModal();
        modal.find('.date-picker').daterangepicker({
            singleDatePicker: true,
            format: 'L'
        });

      _$cohortUserInformationForm = _modalManager.getModal().find('form[name=CohortUserInformationsForm]');
      _$cohortUserInformationForm.validate();
    };

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
      if ($('#CohortUser_UserId').prop('required') && $('#CohortUser_UserId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
        return;
      }

      var cohortUser = _$cohortUserInformationForm.serializeFormToObject();

      cohortUser.cohortId = $('#MasterDetailChild_Cohort_CohortUsersId_Payload').val();
      // console.log('cohortUser saving');
      // debugger;
      _modalManager.setBusy(true);
      _cohortUsersService
        .createOrEdit(cohortUser)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          // console.log('cohortUser saved');
          abp.event.trigger('app.createOrEditCohortUserModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
