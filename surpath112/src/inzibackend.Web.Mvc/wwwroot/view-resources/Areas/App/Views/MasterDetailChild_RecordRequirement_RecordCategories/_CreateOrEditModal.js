(function ($) {
  app.modals.MasterDetailChild_RecordRequirement_CreateOrEditRecordCategoryModal = function () {
    var _recordCategoriesService = abp.services.app.recordCategories;

    var _modalManager;
    var _$recordCategoryInformationForm = null;

    var _RecordCategoryrecordCategoryRuleLookupTableModal = new app.ModalManager({
      viewUrl:
        abp.appPath + 'App/MasterDetailChild_RecordRequirement_RecordCategories/RecordCategoryRuleLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/App/Views/MasterDetailChild_RecordRequirement_RecordCategories/_RecordCategoryRecordCategoryRuleLookupTableModal.js',
      modalClass: 'MasterDetailChild_RecordRequirement_RecordCategoryRuleLookupTableModal',
    });

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$recordCategoryInformationForm = _modalManager.getModal().find('form[name=RecordCategoryInformationsForm]');
      _$recordCategoryInformationForm.validate();
    };

    $('#OpenRecordCategoryRuleLookupTableButton').click(function () {
      var recordCategory = _$recordCategoryInformationForm.serializeFormToObject();

      _RecordCategoryrecordCategoryRuleLookupTableModal.open(
        { id: recordCategory.recordCategoryRuleId, displayName: recordCategory.recordCategoryRuleName },
        function (data) {
          _$recordCategoryInformationForm.find('input[name=recordCategoryRuleName]').val(data.displayName);
          _$recordCategoryInformationForm.find('input[name=recordCategoryRuleId]').val(data.id);
        }
      );
    });

    $('#ClearRecordCategoryRuleNameButton').click(function () {
      _$recordCategoryInformationForm.find('input[name=recordCategoryRuleName]').val('');
      _$recordCategoryInformationForm.find('input[name=recordCategoryRuleId]').val('');
    });

    this.save = function () {
      if (!_$recordCategoryInformationForm.valid()) {
        return;
      }
      if (
        $('#RecordCategory_RecordCategoryRuleId').prop('required') &&
        $('#RecordCategory_RecordCategoryRuleId').val() == ''
      ) {
        abp.message.error(app.localize('{0}IsRequired', app.localize('RecordCategoryRule')));
        return;
      }

      var recordCategory = _$recordCategoryInformationForm.serializeFormToObject();

      recordCategory.recordRequirementId = $('#MasterDetailChild_RecordRequirement_RecordCategoriesId').val();

      _modalManager.setBusy(true);
      _recordCategoriesService
        .createOrEdit(recordCategory)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditRecordCategoryModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
