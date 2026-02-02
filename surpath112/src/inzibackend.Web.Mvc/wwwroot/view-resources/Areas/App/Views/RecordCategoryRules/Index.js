(function () {
  $(function () {
    var _$recordCategoryRulesTable = $('#RecordCategoryRulesTable');
    var _recordCategoryRulesService = abp.services.app.recordCategoryRules;
    var _entityTypeFullName = 'inzibackend.Surpath.RecordCategoryRule';

    var $selectedDate = {
      startDate: null,
      endDate: null,
    };

    $('.date-picker').on('apply.daterangepicker', function (ev, picker) {
      $(this).val(picker.startDate.format('MM/DD/YYYY'));
    });

    $('.startDate')
      .daterangepicker({
        autoUpdateInput: false,
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      })
      .on('apply.daterangepicker', (ev, picker) => {
        $selectedDate.startDate = picker.startDate;
        getRecordCategoryRules();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getRecordCategoryRules();
      });

    $('.endDate')
      .daterangepicker({
        autoUpdateInput: false,
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      })
      .on('apply.daterangepicker', (ev, picker) => {
        $selectedDate.endDate = picker.startDate;
        getRecordCategoryRules();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getRecordCategoryRules();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.RecordCategoryRules.Create'),
      edit: abp.auth.hasPermission('Pages.RecordCategoryRules.Edit'),
      delete: abp.auth.hasPermission('Pages.RecordCategoryRules.Delete'),
    };

    var _viewRecordCategoryRuleModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RecordCategoryRules/ViewrecordCategoryRuleModal',
      modalClass: 'ViewRecordCategoryRuleModal',
    });

    var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();
    function entityHistoryIsEnabled() {
      return (
        abp.auth.hasPermission('Pages.Administration.AuditLogs') &&
        abp.custom.EntityHistory &&
        abp.custom.EntityHistory.IsEnabled &&
        _.filter(abp.custom.EntityHistory.EnabledEntities, (entityType) => entityType === _entityTypeFullName)
          .length === 1
      );
    }

    var getDateFilter = function (element) {
      if ($selectedDate.startDate == null) {
        return null;
      }
      return $selectedDate.startDate.format('YYYY-MM-DDT00:00:00Z');
    };

    var getMaxDateFilter = function (element) {
      if ($selectedDate.endDate == null) {
        return null;
      }
      return $selectedDate.endDate.format('YYYY-MM-DDT23:59:59Z');
    };

    var dataTable = _$recordCategoryRulesTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _recordCategoryRulesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#RecordCategoryRulesTableFilter').val(),
            nameFilter: $('#NameFilterId').val(),
            descriptionFilter: $('#DescriptionFilterId').val(),
            notifyFilter: $('#NotifyFilterId').val(),
            minExpireInDaysFilter: $('#MinExpireInDaysFilterId').val(),
            maxExpireInDaysFilter: $('#MaxExpireInDaysFilterId').val(),
            minWarnDaysBeforeFirstFilter: $('#MinWarnDaysBeforeFirstFilterId').val(),
            maxWarnDaysBeforeFirstFilter: $('#MaxWarnDaysBeforeFirstFilterId').val(),
            expiresFilter: $('#ExpiresFilterId').val(),
            requiredFilter: $('#RequiredFilterId').val(),
            isSurpathOnlyFilter: $('#IsSurpathOnlyFilterId').val(),
            minWarnDaysBeforeSecondFilter: $('#MinWarnDaysBeforeSecondFilterId').val(),
            maxWarnDaysBeforeSecondFilter: $('#MaxWarnDaysBeforeSecondFilterId').val(),
            minWarnDaysBeforeFinalFilter: $('#MinWarnDaysBeforeFinalFilterId').val(),
            maxWarnDaysBeforeFinalFilter: $('#MaxWarnDaysBeforeFinalFilterId').val(),
            metaDataFilter: $('#MetaDataFilterId').val(),
          };
        },
      },
      columnDefs: [
        {
          className: 'control responsive',
          orderable: false,
          render: function () {
            return '';
          },
          targets: 0,
        },
        {
          width: 120,
          targets: 1,
          data: null,
          orderable: false,
          autoWidth: false,
          defaultContent: '',
          rowAction: {
            cssClass: 'btn btn-brand dropdown-toggle',
            text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
            items: [
              {
                text: app.localize('View'),
                action: function (data) {
                  window.location =
                    '/App/RecordCategoryRules/ViewRecordCategoryRule/' + data.record.recordCategoryRule.id;
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  window.location = '/App/RecordCategoryRules/CreateOrEdit/' + data.record.recordCategoryRule.id;
                },
              },
              {
                text: app.localize('History'),
                iconStyle: 'fas fa-history mr-2',
                visible: function () {
                  return entityHistoryIsEnabled();
                },
                action: function (data) {
                  _entityTypeHistoryModal.open({
                    entityTypeFullName: _entityTypeFullName,
                    entityId: data.record.recordCategoryRule.id,
                  });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteRecordCategoryRule(data.record.recordCategoryRule);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'recordCategoryRule.name',
          name: 'name',
        },
        {
          targets: 3,
          data: 'recordCategoryRule.description',
          name: 'description',
        },
        {
          targets: 4,
          data: 'recordCategoryRule.notify',
          name: 'notify',
          render: function (notify) {
            if (notify) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
        {
          targets: 5,
          data: 'recordCategoryRule.expireInDays',
          name: 'expireInDays',
        },
        {
          targets: 6,
          data: 'recordCategoryRule.warnDaysBeforeFirst',
          name: 'warnDaysBeforeFirst',
        },
        {
          targets: 7,
          data: 'recordCategoryRule.expires',
          name: 'expires',
          render: function (expires) {
            if (expires) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
        {
          targets: 8,
          data: 'recordCategoryRule.required',
          name: 'required',
          render: function (required) {
            if (required) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
        {
          targets: 9,
          data: 'recordCategoryRule.isSurpathOnly',
            name: 'isSurpathOnly',
            visible: abp.session.multiTenancySide == abp.multiTenancy.sides.HOST,
              //    function () {
              //    debugger;
              //    return abp.session.multiTenancySide == abp.multiTenancy.sides.HOST;
              //},
          render: function (isSurpathOnly) {
            if (isSurpathOnly) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
        {
          targets: 10,
          data: 'recordCategoryRule.warnDaysBeforeSecond',
          name: 'warnDaysBeforeSecond',
        },
        {
          targets: 11,
          data: 'recordCategoryRule.warnDaysBeforeFinal',
          name: 'warnDaysBeforeFinal',
        },
        //{
        //  targets: 12,
        //  data: 'recordCategoryRule.metaData',
        //  name: 'metaData',
        //},
      ],
    });

    function getRecordCategoryRules() {
      dataTable.ajax.reload();
    }

    function deleteRecordCategoryRule(recordCategoryRule) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _recordCategoryRulesService
            .delete({
              id: recordCategoryRule.id,
            })
            .done(function () {
              getRecordCategoryRules(true);
              abp.notify.success(app.localize('SuccessfullyDeleted'));
            });
        }
      });
    }

    $('#ShowAdvancedFiltersSpan').click(function () {
      $('#ShowAdvancedFiltersSpan').hide();
      $('#HideAdvancedFiltersSpan').show();
      $('#AdvacedAuditFiltersArea').slideDown();
    });

    $('#HideAdvancedFiltersSpan').click(function () {
      $('#HideAdvancedFiltersSpan').hide();
      $('#ShowAdvancedFiltersSpan').show();
      $('#AdvacedAuditFiltersArea').slideUp();
    });

    $('#ExportToExcelButton').click(function () {
      _recordCategoryRulesService
        .getRecordCategoryRulesToExcel({
          filter: $('#RecordCategoryRulesTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
          descriptionFilter: $('#DescriptionFilterId').val(),
          notifyFilter: $('#NotifyFilterId').val(),
          minExpireInDaysFilter: $('#MinExpireInDaysFilterId').val(),
          maxExpireInDaysFilter: $('#MaxExpireInDaysFilterId').val(),
          minWarnDaysBeforeFirstFilter: $('#MinWarnDaysBeforeFirstFilterId').val(),
          maxWarnDaysBeforeFirstFilter: $('#MaxWarnDaysBeforeFirstFilterId').val(),
          expiresFilter: $('#ExpiresFilterId').val(),
          requiredFilter: $('#RequiredFilterId').val(),
          isSurpathOnlyFilter: $('#IsSurpathOnlyFilterId').val(),
          minWarnDaysBeforeSecondFilter: $('#MinWarnDaysBeforeSecondFilterId').val(),
          maxWarnDaysBeforeSecondFilter: $('#MaxWarnDaysBeforeSecondFilterId').val(),
          minWarnDaysBeforeFinalFilter: $('#MinWarnDaysBeforeFinalFilterId').val(),
          maxWarnDaysBeforeFinalFilter: $('#MaxWarnDaysBeforeFinalFilterId').val(),
          metaDataFilter: $('#MetaDataFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditRecordCategoryRuleModalSaved', function () {
      getRecordCategoryRules();
    });

    $('#GetRecordCategoryRulesButton').click(function (e) {
      e.preventDefault();
      getRecordCategoryRules();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getRecordCategoryRules();
      }
    });

    $('.reload-on-change').change(function (e) {
      getRecordCategoryRules();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getRecordCategoryRules();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getRecordCategoryRules();
    });
  });
})();
