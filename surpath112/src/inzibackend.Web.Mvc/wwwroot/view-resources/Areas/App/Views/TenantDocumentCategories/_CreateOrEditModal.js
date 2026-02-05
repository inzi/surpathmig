(function ($) {
  app.modals.CreateOrEditTenantDocumentCategoryModal = function () {
    var _tenantDocumentCategoriesService = abp.services.app.tenantDocumentCategories;

    var _modalManager;
    var _$tenantDocumentCategoryInformationForm = null;

    var _TenantDocumentCategoryuserLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/TenantDocumentCategories/UserLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/App/Views/TenantDocumentCategories/_TenantDocumentCategoryUserLookupTableModal.js',
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

      _$tenantDocumentCategoryInformationForm = _modalManager
        .getModal()
        .find('form[name=TenantDocumentCategoryInformationsForm]');
      _$tenantDocumentCategoryInformationForm.validate();
    };

    $('#OpenUserLookupTableButton').click(function () {
      var tenantDocumentCategory = _$tenantDocumentCategoryInformationForm.serializeFormToObject();

      _TenantDocumentCategoryuserLookupTableModal.open(
        { id: tenantDocumentCategory.userId, displayName: tenantDocumentCategory.userName },
        function (data) {
          _$tenantDocumentCategoryInformationForm.find('input[name=userName]').val(data.displayName);
          _$tenantDocumentCategoryInformationForm.find('input[name=userId]').val(data.id);
        }
      );
    });

    $('#ClearUserNameButton').click(function () {
      _$tenantDocumentCategoryInformationForm.find('input[name=userName]').val('');
      _$tenantDocumentCategoryInformationForm.find('input[name=userId]').val('');
    });

    this.save = function () {
      if (!_$tenantDocumentCategoryInformationForm.valid()) {
        return;
      }
      if ($('#TenantDocumentCategory_UserId').prop('required') && $('#TenantDocumentCategory_UserId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
        return;
      }

      var tenantDocumentCategory = _$tenantDocumentCategoryInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _tenantDocumentCategoriesService
        .createOrEdit(tenantDocumentCategory)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditTenantDocumentCategoryModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
