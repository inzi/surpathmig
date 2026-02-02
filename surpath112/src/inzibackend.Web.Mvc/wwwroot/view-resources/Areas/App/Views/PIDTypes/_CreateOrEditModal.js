(function ($) {
  app.modals.CreateOrEditPidTypeModal = function () {
    var _pidTypesService = abp.services.app.pidTypes;

    var _modalManager;
    var _$pidTypeInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$pidTypeInformationForm = _modalManager.getModal().find('form[name=PidTypeInformationsForm]');
      _$pidTypeInformationForm.validate();
    };

    this.save = function () {
      if (!_$pidTypeInformationForm.valid()) {
        return;
      }

      var pidType = _$pidTypeInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _pidTypesService
        .createOrEdit(pidType)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditPidTypeModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
