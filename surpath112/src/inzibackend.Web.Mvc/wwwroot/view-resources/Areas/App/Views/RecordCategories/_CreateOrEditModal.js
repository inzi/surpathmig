(function ($) {
  app.modals.CreateOrEditRecordCategoryModal = function () {
    var _recordCategoriesService = abp.services.app.recordCategories;

    var _modalManager;
    var _$recordCategoryInformationForm = null;

    var _RecordCategoryrecordRequirementLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RecordCategories/RecordRequirementLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/App/Views/RecordCategories/_RecordCategoryRecordRequirementLookupTableModal.js',
      modalClass: 'RecordRequirementLookupTableModal',
    });
    var _RecordCategoryrecordCategoryRuleLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RecordCategories/RecordCategoryRuleLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/App/Views/RecordCategories/_RecordCategoryRecordCategoryRuleLookupTableModal.js',
      modalClass: 'RecordCategoryRuleLookupTableModal',
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

    $('#OpenRecordRequirementLookupTableButton').click(function () {
      var recordCategory = _$recordCategoryInformationForm.serializeFormToObject();

      _RecordCategoryrecordRequirementLookupTableModal.open(
        { id: recordCategory.recordRequirementId, displayName: recordCategory.recordRequirementName },
        function (data) {
          _$recordCategoryInformationForm.find('input[name=recordRequirementName]').val(data.displayName);
          _$recordCategoryInformationForm.find('input[name=recordRequirementId]').val(data.id);
        }
      );
    });

    $('#ClearRecordRequirementNameButton').click(function () {
      _$recordCategoryInformationForm.find('input[name=recordRequirementName]').val('');
      _$recordCategoryInformationForm.find('input[name=recordRequirementId]').val('');
    });

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
        $('#RecordCategory_RecordRequirementId').prop('required') &&
        $('#RecordCategory_RecordRequirementId').val() == ''
      ) {
        abp.message.error(app.localize('{0}IsRequired', app.localize('RecordRequirement')));
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

        _modalManager.setBusy(true);

        if ($('nosave').val() == "false") {
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
        }
        else {
            _modalManager.setBusy(false);
            _modalManager.setResult(recordCategory);
            _modalManager.close();
            abp.event.trigger('app.createOrEditRecordCategoryModalSaved');
        }
    };
  };
})(jQuery);
