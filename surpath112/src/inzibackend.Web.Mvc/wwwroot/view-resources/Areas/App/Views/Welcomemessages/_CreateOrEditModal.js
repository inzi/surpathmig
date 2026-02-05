(function ($) {
  app.modals.CreateOrEditWelcomemessageModal = function () {
    var _welcomemessagesService = abp.services.app.welcomemessages;

    var _modalManager;
    var _$welcomemessageInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$welcomemessageInformationForm = _modalManager.getModal().find('form[name=WelcomemessageInformationsForm]');
      _$welcomemessageInformationForm.validate();
    };

    this.save = function () {
      if (!_$welcomemessageInformationForm.valid()) {
        return;
      }

      var welcomemessage = _$welcomemessageInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _welcomemessagesService
        .createOrEdit(welcomemessage)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditWelcomemessageModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
