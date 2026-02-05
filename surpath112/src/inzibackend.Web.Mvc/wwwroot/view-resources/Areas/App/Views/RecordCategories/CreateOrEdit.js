(function () {
  $(function () {
    var _recordCategoriesService = abp.services.app.recordCategories;

    var _$recordCategoryInformationForm = $('form[name=RecordCategoryInformationsForm]');
    _$recordCategoryInformationForm.validate();

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

    $('.date-picker').daterangepicker({
      singleDatePicker: true,
      locale: abp.localization.currentLanguage.name,
      format: 'L',
    });

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

    function save(successCallback) {
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

      abp.ui.setBusy();
      _recordCategoriesService
        .createOrEdit(recordCategory)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          abp.event.trigger('app.createOrEditRecordCategoryModalSaved');

          if (typeof successCallback === 'function') {
            successCallback();
          }
        })
        .always(function () {
          abp.ui.clearBusy();
        });
    }

    function clearForm() {
      _$recordCategoryInformationForm[0].reset();
    }

    $('#saveBtn').click(function () {
      save(function () {
        window.location = '/App/RecordCategories';
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
