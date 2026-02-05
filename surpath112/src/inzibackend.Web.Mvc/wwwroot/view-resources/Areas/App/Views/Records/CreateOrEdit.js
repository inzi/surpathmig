(function () {
  $(function () {
    var _recordsService = abp.services.app.records;

    var _$recordInformationForm = $('form[name=RecordInformationsForm]');
    _$recordInformationForm.validate();

    var _RecordtenantDocumentCategoryLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Records/TenantDocumentCategoryLookupTableModal',
      scriptUrl:
        abp.appPath + 'view-resources/Areas/App/Views/Records/_RecordTenantDocumentCategoryLookupTableModal.js',
      modalClass: 'TenantDocumentCategoryLookupTableModal',
    });
    var _fileUploading = [];
    var _filedataToken;

    $('.date-picker').daterangepicker({
      singleDatePicker: true,
      locale: abp.localization.currentLanguage.name,
      format: 'L',
    });

    $('#OpenTenantDocumentCategoryLookupTableButton').click(function () {
      var record = _$recordInformationForm.serializeFormToObject();

      _RecordtenantDocumentCategoryLookupTableModal.open(
        { id: record.tenantDocumentCategoryId, displayName: record.tenantDocumentCategoryName },
        function (data) {
          _$recordInformationForm.find('input[name=tenantDocumentCategoryName]').val(data.displayName);
          _$recordInformationForm.find('input[name=tenantDocumentCategoryId]').val(data.id);
        }
      );
    });

    $('#ClearTenantDocumentCategoryNameButton').click(function () {
      _$recordInformationForm.find('input[name=tenantDocumentCategoryName]').val('');
      _$recordInformationForm.find('input[name=tenantDocumentCategoryId]').val('');
    });

    function save(successCallback) {
      if (!_$recordInformationForm.valid()) {
        return;
      }
      if ($('#Record_TenantDocumentCategoryId').prop('required') && $('#Record_TenantDocumentCategoryId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('TenantDocumentCategory')));
        return;
      }

      if (_fileUploading != null && _fileUploading.length > 0) {
        abp.notify.info(app.localize('WaitingForFileUpload'));
        return;
      }

      var record = _$recordInformationForm.serializeFormToObject();

      record.filedataToken = _filedataToken;

      abp.ui.setBusy();
      _recordsService
        .createOrEdit(record)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          abp.event.trigger('app.createOrEditRecordModalSaved');

          if (typeof successCallback === 'function') {
            successCallback();
          }
        })
        .always(function () {
          abp.ui.clearBusy();
        });
    }

    function clearForm() {
      _$recordInformationForm[0].reset();
    }

    $('#saveBtn').click(function () {
      save(function () {
        window.location = '/App/Records';
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

    $('#Record_filedata').change(function () {
      var file = $(this)[0].files[0];
      if (!file) {
        _filedataToken = null;
        return;
      }

        var formData = new FormData();
        $('#Record_filename').val(file.name);
      formData.append('file', file);
      _fileUploading.push(true);

      $.ajax({
        url: '/App/Records/UploadfiledataFile',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
      })
        .done(function (resp) {
          if (resp.success && resp.result.fileToken) {
            _filedataToken = resp.result.fileToken;
          } else {
            abp.message.error(resp.result.message);
          }
        })
        .always(function () {
          _fileUploading.pop();
        });
    });

    $('#Record_filedata_Remove').click(function () {
      abp.message.confirm(app.localize('DoYouWantToRemoveTheFile'), app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          var Record = _$recordInformationForm.serializeFormToObject();
          _recordsService
            .removefiledataFile({
              id: Record.id,
            })
            .done(function () {
              abp.notify.success(app.localize('SuccessfullyDeleted'));
              _$recordInformationForm.find('#div_current_file').css('display', 'none');
            });
        }
      });
    });

    $('#Record_filedata').change(function () {
      var fileName = app.localize('ChooseAFile');
      if (this.files && this.files[0]) {
        fileName = this.files[0].name;
      }
      $('#Record_filedataLabel').text(fileName);
    });
  });
})();
