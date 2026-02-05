var EditTenantModal = (function ($) {
    app.modals.EditTenantModal = function () {
        var _modalManager;
        var _tenantService = abp.services.app.tenant;
        var _$tenantInformationForm = null;

        var $selectedDateTime = {
            startDate: moment()
        };

        this.init = function (modalManager) {
            _modalManager = modalManager;
            var modal = _modalManager.getModal();

            _$tenantInformationForm = modal.find('form[name=TenantInformationsForm]');
            _$tenantInformationForm.validate();

            modal.find('.date-time-picker').daterangepicker({
                timePicker: true,
                singleDatePicker: true,
                parentEl: '#EditTenantInformationsForm',
                startDate: modal.find('.date-time-picker').val(),
                locale: {
                    format: 'MM/DD/YYYY hh:mm A',
                },
            }, (start) => $selectedDateTime.startDate = start);

            var $subscriptionEndDateDiv = modal.find('input[name=SubscriptionEndDateUtc]').parent('div');
            var isUnlimitedInput = modal.find('#CreateTenant_IsUnlimited');
            function toggleSubscriptionEndDateDiv() {
                if (isUnlimitedInput.is(':checked')) {
                    $subscriptionEndDateDiv.slideUp('fast');
                } else {
                    $subscriptionEndDateDiv.slideDown('fast');
                }
            }

            var $isInTrialPeriodInputDiv = modal.find('#EditTenant_IsInTrialPeriod').closest('div');
            var $isInTrialPeriodInput = modal.find('#EditTenant_IsInTrialPeriod');
            function toggleIsInTrialPeriod() {
                if (isUnlimitedInput.is(':checked')) {
                    $isInTrialPeriodInputDiv.slideUp('fast');
                    $isInTrialPeriodInput.prop('checked', false);
                } else {
                    $isInTrialPeriodInputDiv.slideDown('fast');
                }
            }

            isUnlimitedInput.change(function () {
                toggleSubscriptionEndDateDiv();
                toggleIsInTrialPeriod();
            });

            var $editionCombobox = modal.find('#EditionId');
            var $isInTrialCheckbox = modal.find('#EditTenant_IsInTrialPeriod');
            $editionCombobox.change(function () {
                var isFree = $('option:selected', this).attr('data-isfree') === 'True';
                var selectedValue = $('option:selected', this).val();
                if (isFree) {
                    $isInTrialCheckbox.closest('div').slideUp('fast');
                } else {
                    $isInTrialCheckbox.closest('div').slideDown('fast');
                }

                if (selectedValue <= 0) {
                    modal.find('.subscription-component').slideUp('fast');
                    if (!isUnlimitedInput.is(':checked')) {
                        $subscriptionEndDateDiv.slideDown('fast');
                    }
                } else {
                    modal.find('.subscription-component').slideDown('fast');
                }
            });

            $editionCombobox.trigger('change');
            toggleSubscriptionEndDateDiv();
            toggleIsInTrialPeriod();

            $('#EditTenant_IsDonorPay').on('click', function () {
                //debugger;
                if ($('#EditTenant_IsDonorPay').is(':checked')) {
                    // offer to defer prompt for payment for users already in system
                    abp.message.confirm('', app.localize('ConfirmDeferDonorPay'), function (isConfirmed) {
                        if (isConfirmed) {
                            $('#DeferDonorPay').prop('checked', true);
                        } else {
                            $('#DeferDonorPay').prop('checked', false);
                        }

                        abp.message.confirm('', app.localize('ConfirmDeferDonorPerpetualPay'), function (isConfirmed) {
                            if (isConfirmed) {
                                $('#DeferDonorPerpetualPay').prop('checked', true);
                            } else {
                                $('#DeferDonorPerpetualPay').prop('checked', false);
                            }
                        });
                    });
                }
            });
        };

        this.save = function () {
            if (!_$tenantInformationForm.valid()) {
                return;
            }

            var tenant = _$tenantInformationForm.serializeFormToObject();

            //take selected date as UTC
            if ($('#CreateTenant_IsUnlimited').is(':visible') && !$('#CreateTenant_IsUnlimited').is(':checked')) {
                tenant.SubscriptionEndDateUtc = $selectedDateTime.startDate.format('YYYY-MM-DDTHH:mm:ss') + 'Z';
            } else {
                tenant.SubscriptionEndDateUtc = null;
            }

            if ($('#CreateTenant_IsUnlimited').is(':checked')) {
                tenant.IsInTrialPeriod = false;
            }
            tenant.ClientPaymentType = 0;
            tenant.IsDonorPay = false;
            if ($('#EditTenant_IsDonorPay').is(':checked')) {
                tenant.IsDonorPay = true;
                tenant.ClientPaymentType = 1;
            }
            tenant.DeferDonorPay = false;
            if ($('#DeferDonorPay').is(':checked')) {
                tenant.DeferDonorPay = true;
            }
            tenant.DeferDonorPerpetualPay = false;
            if ($('#DeferDonorPerpetualPay').is(':checked')) {
                tenant.DeferDonorPay = true;
            }

            _modalManager.setBusy(true);
            _tenantService
                .updateTenant(tenant)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.editTenantModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);