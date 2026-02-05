(function () {
  $(function () {
    var _tenantDocumentCategoriesService = abp.services.app.tenantDocumentCategories;

    var _$tenantDocumentCategoryInformationForm = $('form[name=TenantDocumentCategoryInformationsForm]');
    _$tenantDocumentCategoryInformationForm.validate();

    var _TenantDocumentCategoryuserLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/TenantDocumentCategories/UserLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/App/Views/TenantDocumentCategories/_TenantDocumentCategoryUserLookupTableModal.js',
      modalClass: 'UserLookupTableModal',
    });

    $('.date-picker').daterangepicker({
      singleDatePicker: true,
      locale: abp.localization.currentLanguage.name,
      format: 'L',
    });

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

    function save(successCallback) {
      if (!_$tenantDocumentCategoryInformationForm.valid()) {
        return;
      }
      if ($('#TenantDocumentCategory_UserId').prop('required') && $('#TenantDocumentCategory_UserId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
        return;
      }

      var tenantDocumentCategory = _$tenantDocumentCategoryInformationForm.serializeFormToObject();

      abp.ui.setBusy();
      _tenantDocumentCategoriesService
        .createOrEdit(tenantDocumentCategory)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          abp.event.trigger('app.createOrEditTenantDocumentCategoryModalSaved');

          if (typeof successCallback === 'function') {
            successCallback();
          }
        })
        .always(function () {
          abp.ui.clearBusy();
        });
    }

    function clearForm() {
      _$tenantDocumentCategoryInformationForm[0].reset();
    }

    $('#saveBtn').click(function () {
      save(function () {
        window.location = '/App/TenantDocumentCategories';
      });
    });

    $('#saveAndNewBtn').click(function () {
      save(function () {
        if (!$('input[name=id]').val()) {
          //if it is create page
          clearForm();
        }
      });
    });
  });
})();
