(function () {
  $(function () {
    var _recordCategoryRulesService = abp.services.app.recordCategoryRules;

    var _$recordCategoryRuleInformationForm = $('form[name=RecordCategoryRuleInformationsForm]');
    _$recordCategoryRuleInformationForm.validate();

    $('.date-picker').daterangepicker({
      singleDatePicker: true,
      locale: abp.localization.currentLanguage.name,
      format: 'L',
    });

    // Handle Expires checkbox change
    $('#RecordCategoryRule_Expires').change(function() {
      var isChecked = $(this).is(':checked');
      $('#expiredStatusContainer').toggle(isChecked);
      if (!isChecked) {
        $('#expiredStatusId').val('');
      } else if ($('#expiredStatusId').val() === '') {
        abp.message.warn(app.localize('ExpiredStatusRequired'));
      }
    });

    // Handle status dropdown changes
    $('#firstWarnStatusId').change(function() {
      var selectedValue = $(this).val();
      if (selectedValue === "" && $('#RecordCategoryRule_WarnDaysBeforeFirst').val() > 0) {
        abp.message.info(app.localize('ConsiderSelectingAStatusForFirstWarning'));
      }
    });

    $('#secondWarnStatusId').change(function() {
      var selectedValue = $(this).val();
      if (selectedValue === "" && $('#RecordCategoryRule_WarnDaysBeforeSecond').val() > 0) {
        abp.message.info(app.localize('ConsiderSelectingAStatusForSecondWarning'));
      }
    });

    $('#finalWarnStatusId').change(function() {
      var selectedValue = $(this).val();
      if (selectedValue === "" && $('#RecordCategoryRule_WarnDaysBeforeFinal').val() > 0) {
        abp.message.info(app.localize('ConsiderSelectingAStatusForFinalWarning'));
      }
    });

    $('#expiredStatusId').change(function() {
      var selectedValue = $(this).val();
      if (selectedValue === "" && $('#RecordCategoryRule_Expires').is(':checked')) {
        abp.message.warn(app.localize('ExpiredStatusRequired'));
      }
    });

    // Handle warn days changes
    $('#RecordCategoryRule_WarnDaysBeforeFirst').change(function() {
      var days = $(this).val();
      var selectedValue = $('#firstWarnStatusId').val();
      if (days > 0 && selectedValue === "") {
        abp.message.info(app.localize('ConsiderSelectingAStatusForFirstWarning'));
      }
    });

    $('#RecordCategoryRule_WarnDaysBeforeSecond').change(function() {
      var days = $(this).val();
      var selectedValue = $('#secondWarnStatusId').val();
      if (days > 0 && selectedValue === "") {
        abp.message.info(app.localize('ConsiderSelectingAStatusForSecondWarning'));
      }
    });

    $('#RecordCategoryRule_WarnDaysBeforeFinal').change(function() {
      var days = $(this).val();
      var selectedValue = $('#finalWarnStatusId').val();
      if (days > 0 && selectedValue === "") {
        abp.message.info(app.localize('ConsiderSelectingAStatusForFinalWarning'));
      }
    });

    function validateWarningDaysAndStatus() {
      var isValid = true;

      // Only validate expired status - warning statuses are optional
      if ($('#RecordCategoryRule_Expires').is(':checked') && !$('#expiredStatusId').val()) {
        abp.message.error(app.localize('ExpiredStatusRequired'));
        isValid = false;
      }

      return isValid;
    }

    function save(successCallback) {
      if (!_$recordCategoryRuleInformationForm.valid()) {
        return;
      }

      if (!validateWarningDaysAndStatus()) {
        return;
      }

      var recordCategoryRule = _$recordCategoryRuleInformationForm.serializeFormToObject();

      // Convert empty status selections to null
      if (recordCategoryRule.firstWarnStatusId === "") {
        recordCategoryRule.firstWarnStatusId = null;
      }
      if (recordCategoryRule.secondWarnStatusId === "") {
        recordCategoryRule.secondWarnStatusId = null;
      }
      if (recordCategoryRule.finalWarnStatusId === "") {
        recordCategoryRule.finalWarnStatusId = null;
      }
      if (recordCategoryRule.expiredStatusId === "") {
        recordCategoryRule.expiredStatusId = null;
      }
      if (!recordCategoryRule.expires) {
        recordCategoryRule.expiredStatusId = null;
      }

      abp.ui.setBusy();
      _recordCategoryRulesService
        .createOrEdit(recordCategoryRule)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          abp.event.trigger('app.createOrEditRecordCategoryRuleModalSaved');

          if (typeof successCallback === 'function') {
            successCallback();
          }
        })
        .always(function () {
          abp.ui.clearBusy();
        });
    }

    function clearForm() {
      _$recordCategoryRuleInformationForm[0].reset();
      $('#expiredStatusContainer').hide();
    }

    $('#saveBtn').click(function () {
      save(function () {
        window.location = '/App/RecordCategoryRules';
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
