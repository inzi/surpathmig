(function ($) {
    app.modals.MoveCategoryModal = function () {
        var _modalManager;
        var _$modal;
        var _categoryManagementService = abp.services.app.categoryManagement;

        var _permissions = {
            moveCategories: abp.auth.hasPermission('Pages.Administration.RecordRequirements.MoveCategories')
        };

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _$modal = _modalManager.getModal();
            
            initializeEventHandlers();
            validateInitialState();
        };

        function initializeEventHandlers() {
            // Create new requirement toggle
            _$modal.on('click', '#createNewRequirementBtn', function () {
                var $newSection = _$modal.find('#newRequirementSection');
                var $targetSelect = _$modal.find('#targetRequirementId');
                var $btn = $(this);
                
                if ($newSection.is(':visible')) {
                    // Hide new requirement section
                    $newSection.hide();
                    $targetSelect.prop('disabled', false);
                    $btn.html('<i class="fa fa-plus"></i> ' + app.localize('CreateNew'));
                } else {
                    // Show new requirement section
                    $newSection.show();
                    $targetSelect.prop('disabled', true).val('');
                    $btn.html('<i class="fa fa-times"></i> ' + app.localize('Cancel'));
                }
                
                validateOperation();
            });

            // Target requirement selection change
            _$modal.on('change', '#targetRequirementId', function () {
                validateOperation();
            });

            // New requirement name input
            _$modal.on('input', '#newRequirementName', function () {
                validateOperation();
            });

            // Confirm move button
            _$modal.on('click', '#confirmMoveBtn', function () {
                moveCategories();
            });

            // Confirmation checkbox change
            _$modal.on('change', '#confirmRequirementDeletion', function () {
                validateOperation();
            });
        }

        function validateInitialState() {
            // Initial validation
            validateOperation();
        }

        function validateOperation() {
            var categoryIds = getCategoryIds();
            var targetRequirementId = _$modal.find('#targetRequirementId').val();
            var isCreatingNew = _$modal.find('#newRequirementSection').is(':visible');
            var newRequirementName = _$modal.find('#newRequirementName').val();
            
            // Basic validation
            var isValid = false;
            if (isCreatingNew) {
                isValid = newRequirementName && newRequirementName.trim().length > 0;
            } else {
                isValid = targetRequirementId && targetRequirementId.length > 0;
            }
            
            _$modal.find('#confirmMoveBtn').prop('disabled', !isValid);
            
            // If basic validation passes, get detailed validation from server
            if (isValid && categoryIds.length > 0) {
                var firstCategoryId = categoryIds[0];
                
                abp.ui.setBusy(_$modal.find('#impactAnalysisSection'));
                
                _categoryManagementService.validateCategoryOperation({
                    categoryId: firstCategoryId,
                    targetRequirementId: isCreatingNew ? null : targetRequirementId,
                    operationType: 'move'
                }).done(function (result) {
                    displayValidationResults(result.validationResult);
                }).fail(function (xhr, status, error) {
                    console.error('Validation failed:', error);
                    abp.message.error('Failed to validate category operation: ' + error);
                }).always(function () {
                    abp.ui.clearBusy(_$modal.find('#impactAnalysisSection'));
                });
            } else {
                // Clear validation display
                _$modal.find('#validationSection').hide();
                _$modal.find('#impactAnalysisSection').hide();
                _$modal.find('#confirmationSection').hide();
            }
        }

        function displayValidationResults(validation) {
            // Handle case where validation might be wrapped in a result object
            if (validation && validation.result) {
                validation = validation.result;
            }
            
            // Display warnings
            var $validationSection = _$modal.find('#validationSection');
            var $warnings = _$modal.find('#validationWarnings');
            
            if (validation && validation.warnings && validation.warnings.length > 0) {
                $warnings.empty();
                $.each(validation.warnings, function (i, warning) {
                    $warnings.append('<li>' + warning + '</li>');
                });
                $validationSection.show();
            } else {
                $validationSection.hide();
            }

            // Display impact analysis
            var $impactSection = _$modal.find('#impactAnalysisSection');
            var $impactText = _$modal.find('#impactAnalysisText');
            
            if (validation && (validation.affectedRecordStatesCount > 0 || validation.affectedUsersCount > 0)) {
                var impactMessage = app.localize('MoveImpactAnalysis')
                    .replace('{0}', validation.affectedRecordStatesCount || 0)
                    .replace('{1}', validation.affectedUsersCount || 0);
                $impactText.text(impactMessage);
                $impactSection.show();
            } else {
                $impactSection.hide();
            }

            // Show confirmation checkbox if requirement will be deleted
            var $confirmationSection = _$modal.find('#confirmationSection');
            if (validation && validation.willDeleteSourceRequirement) {
                $confirmationSection.show();
            } else {
                $confirmationSection.hide();
            }

            // Update button state based on validation
            var canProceed = validation && validation.canMove && (!validation.willDeleteSourceRequirement || _$modal.find('#confirmRequirementDeletion').is(':checked'));
            _$modal.find('#confirmMoveBtn').prop('disabled', !canProceed);
        }

        function getCategoryIds() {
            var categoryIdsStr = _$modal.find('#moveCategoryIds').val();
            return categoryIdsStr ? categoryIdsStr.split(',') : [];
        }

        function moveCategories() {
            var categoryIds = getCategoryIds();
            var isCreatingNew = _$modal.find('#newRequirementSection').is(':visible');
            var targetRequirementId = isCreatingNew ? null : _$modal.find('#targetRequirementId').val();
            var newRequirementName = _$modal.find('#newRequirementName').val();
            var newRequirementDescription = _$modal.find('#newRequirementDescription').val();
            var confirmDeletion = _$modal.find('#confirmRequirementDeletion').is(':checked');

            if (categoryIds.length === 0) {
                abp.message.warn(app.localize('PleaseSelectAtLeastOneCategory'));
                return;
            }

            // Prepare move data for each category
            var movePromises = [];
            
            $.each(categoryIds, function (i, categoryId) {
                var moveData = {
                    categoryId: categoryId,
                    targetRequirementId: targetRequirementId,
                    newRequirementName: newRequirementName,
                    newRequirementDescription: newRequirementDescription,
                    confirmRequirementDeletion: confirmDeletion
                };

                var promise;
                if (isCreatingNew) {
                    promise = _categoryManagementService.moveCategoryToNewRequirement(moveData);
                } else {
                    promise = _categoryManagementService.moveCategoryToExistingRequirement(moveData);
                }
                
                movePromises.push(promise);
            });

            abp.ui.setBusy(_$modal);

            // Execute all moves
            $.when.apply($, movePromises).done(function () {
                // ABP's dynamic web API returns the result directly
                var result = arguments[0];
                
                if (result && result.success === true) {
                    abp.message.success(result.message || app.localize('CategoriesMovedSuccessfully'));
                    _modalManager.close();
                    
                    // Trigger refresh events
                    abp.event.trigger('app.categoryManagementCompleted');
                } else {
                    var errorMessage = (result && result.message) ? result.message : app.localize('AnErrorOccurred');
                    abp.message.error(errorMessage);
                }
            }).fail(function (xhr, status, error) {
                console.error('Move operation failed:', error);
                abp.message.error(app.localize('AnErrorOccurred'));
            }).always(function () {
                abp.ui.clearBusy(_$modal);
            });
        }

        this.close = function () {
            _modalManager.close();
            // Trigger refresh event when modal is closed
            abp.event.trigger('app.categoryManagementCompleted');
        };
    };
})(jQuery); 