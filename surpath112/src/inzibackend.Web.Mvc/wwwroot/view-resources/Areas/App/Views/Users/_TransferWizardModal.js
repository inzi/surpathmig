(function ($) {
    app.modals.UserTransferWizardModal = function () {
        var _userTransferService = abp.services.app.userTransfer;
        
        // Debug: Check if service is available
        console.log('UserTransfer service available:', !!_userTransferService);
        if (_userTransferService) {
            console.log('Available methods:', Object.keys(_userTransferService));
        }
        
        var _modalManager;
        var _$transferWizardForm = null;
        var _currentStep = 1;
        var _maxSteps = 4;
        var _transferData = {
            selectedUsers: [],
            selectedUserDetails: {} // Store user details by cohortUserId
        };
        var _categoryMappingTable = null;
        var _cohortInfo = {
            cohortId: null,
            cohortName: null
        };
        var _noTransferRequiredCategories = [];
        var _userDataTable = null;

        function addValidationStyles() {
            // Add CSS styles for validation feedback
            if (!document.getElementById('transfer-validation-styles')) {
                var style = document.createElement('style');
                style.id = 'transfer-validation-styles';
                style.textContent = `
                    .validation-error {
                        background-color: #fff5f5 !important;
                        border-left: 4px solid #e53e3e !important;
                    }
                    .validation-warning {
                        background-color: #fffbeb !important;
                        border-left: 4px solid #dd6b20 !important;
                    }
                    .highlight-category {
                        background-color: #e6fffa !important;
                        border: 2px solid #38b2ac !important;
                        animation: highlight-pulse 2s ease-in-out;
                    }
                    @keyframes highlight-pulse {
                        0%, 100% { background-color: #e6fffa; }
                        50% { background-color: #b2f5ea; }
                    }
                    .validation-issue-error {
                        color: #e53e3e;
                        margin-bottom: 8px;
                    }
                    .validation-issue-warning {
                        color: #dd6b20;
                        margin-bottom: 8px;
                    }
                    .goto-category-btn {
                        font-size: 0.75rem;
                        padding: 2px 8px;
                    }
                    /* Hierarchy level badges */
                    .requirement-info .badge-sm {
                        font-size: 0.7rem;
                        padding: 2px 6px;
                        margin-top: 2px;
                        display: inline-block;
                    }
                    .requirement-info .badge-sm i {
                        margin-right: 2px;
                    }
                    /* Confirmation Step Styles */
                    .summary-metric {
                        padding: 15px;
                    }
                    .metric-value {
                        font-size: 2rem;
                        font-weight: bold;
                        display: block;
                        margin-bottom: 5px;
                    }
                    .metric-label {
                        font-size: 0.875rem;
                        color: #6c757d;
                    }
                    .info-item {
                        padding: 5px 0;
                    }
                    /* Requirements analysis styles */
                    .requirements-analysis {
                        margin-top: 15px;
                    }
                    .requirement-list {
                        max-height: 200px;
                        overflow-y: auto;
                        border: 1px solid #e0e0e0;
                        border-radius: 4px;
                        padding: 10px;
                        margin-bottom: 10px;
                        background-color: #f9f9f9;
                    }
                    .requirement-list li {
                        padding: 5px 0;
                        border-bottom: 1px solid #e0e0e0;
                    }
                    .requirement-list li:last-child {
                        border-bottom: none;
                    }
                `;
                document.head.appendChild(style);
            }
        }

        this.init = function (modalManager) {
            _modalManager = modalManager;
            var $modal = _modalManager.getModal();
            _$transferWizardForm = $modal.find('form[name=TransferWizardForm]');
            
            addValidationStyles();
            
            // Initialize cohort info from modal arguments
            var args = modalManager.getArgs();
            if (args && args.cohortId) {
                _cohortInfo.cohortId = args.cohortId;
            } else {
                // Fallback to hidden inputs
                _cohortInfo.cohortId = $modal.find('#transferWizardModelData input[name="UserTransferWizardViewModel_CohortId"]').val();
            }
            _cohortInfo.cohortName = $modal.find('#transferWizardModelData input[name="UserTransferWizardViewModel_CohortName"]').val();
            
            console.log('Modal args:', args);
            console.log('Initialized cohort info:', _cohortInfo);
            
            // Initialize the wizard
            initializeWizard();
            
            // Bind events
            bindEvents();
            
            // Load initial data if needed
            if (_currentStep === 1) {
                // Focus on first input
                setTimeout(function() {
                    $modal.find('input[type="radio"]:first').focus();
                }, 500);
            }
        };

        function initializeWizard() {
            updateStepVisibility();
            updateNavigationButtons();
            updateProgressBar();
        }

        function bindEvents() {
            var $modal = _modalManager.getModal();
            
            // Department option radio buttons
            $modal.find('input[name="departmentOption"]').on('change', function() {
                var value = $(this).val();
                if (value === 'existing') {
                    $('#existingDepartmentSection').removeClass('d-none');
                    $('#newDepartmentSection').addClass('d-none');
                } else {
                    $('#existingDepartmentSection').addClass('d-none');
                    $('#newDepartmentSection').removeClass('d-none');
                }
                _transferData.departmentOption = value;
            });
            
            // Target department selection
            $('#targetDepartmentId').on('change', function() {
                var departmentId = $(this).val();
                if (departmentId) {
                    // Check if "None" was selected (Guid.Empty)
                    if (departmentId === '00000000-0000-0000-0000-000000000000') {
                        loadCohortsWithoutDepartment();
                    } else {
                        loadCohortsForDepartment(departmentId);
                    }
                } else {
                    $('#targetCohortSection').hide();
                    $('#cohortStats').addClass('d-none');
                    $('#analysisResults').addClass('d-none');
                }
                _transferData.targetDepartmentId = departmentId === '00000000-0000-0000-0000-000000000000' ? null : departmentId;
            });
            
            // Target cohort selection
            $('#targetCohortId').on('change', function() {
                var cohortId = $(this).val();
                if (cohortId) {
                    loadCohortStatistics(cohortId);
                    // Automatically analyze transfer when target cohort is selected
                    analyzeTransfer();
                } else {
                    $('#cohortStats').addClass('d-none');
                    $('#analysisResults').addClass('d-none');
                }
                _transferData.targetCohortId = cohortId;
            });
            
            // User selection events
            $('#selectAllUsersBtn').on('click', function() {
                if (_userDataTable) {
                    _userDataTable.$('input[type="checkbox"]').prop('checked', true);
                    
                    // Store all user details
                    _userDataTable.rows().every(function() {
                        var userData = this.data();
                        _transferData.selectedUserDetails[userData.id] = {
                            id: userData.id,
                            userName: userData.userName,
                            fullName: userData.fullName,
                            email: userData.email
                        };
                    });
                    
                    updateSelectedUsersCount();
                }
            });
            
            $('#selectNoneUsersBtn').on('click', function() {
                if (_userDataTable) {
                    _userDataTable.$('input[type="checkbox"]').prop('checked', false);
                    _transferData.selectedUserDetails = {};
                    updateSelectedUsersCount();
                }
            });
            
            $('#searchUsersBtn').on('click', function() {
                if (_userDataTable) {
                    _userDataTable.ajax.reload();
                }
            });
            
            // Search on Enter key
            $('#userSearchFilter').on('keypress', function(e) {
                if (e.which === 13) {
                    e.preventDefault();
                    $('#searchUsersBtn').click();
                }
            });
            
            // Optional: Add real-time search with debounce
            var searchTimeout;
            $('#userSearchFilter').on('input', function() {
                clearTimeout(searchTimeout);
                var $this = $(this);
                searchTimeout = setTimeout(function() {
                    if (_userDataTable) {
                        _userDataTable.ajax.reload();
                    }
                }, 500); // 500ms delay
            });
            
            // Removed analyze button handler - analysis now happens automatically
            
            // Navigation buttons
            $('#nextStepBtn').on('click', function() {
                if (validateCurrentStep()) {
                    moveToNextStep();
                }
            });
            
            $('#prevStepBtn').on('click', function() {
                moveToPreviousStep();
            });
            
            // Execute transfer button
            $('#executeTransferBtn').on('click', function() {
                if ($('#confirmTransfer').is(':checked')) {
                    executeTransfer();
                } else {
                    abp.message.warn(app.localize('PleaseConfirmTransfer'));
                }
            });
        }

        function analyzeTransfer() {
            var targetCohortId = $('#targetCohortId').val();
            
            if (!targetCohortId) {
                // No target cohort selected yet, just return
                return;
            }
            
            $('#analysisLoading').removeClass('d-none');
            $('#analysisResults').addClass('d-none');
            
            // Create input for analyzing cohort-to-cohort transfer
            var input = {
                sourceCohortId: _cohortInfo.cohortId,
                targetCohortId: targetCohortId,
                selectedCohortUserIds: [] // Empty for initial analysis
            };
            
            // Call the method that takes both source and target cohorts
            abp.services.app.userTransfer.analyzeSelectiveUserTransfer(input)
                .done(function(result) {
                    console.log('Analysis result:', result);
                    _transferData.analysisResult = result;
                    displayAnalysisResults(result);
                })
                .fail(function(error) {
                    console.error('Analysis failed:', error);
                    abp.message.error(app.localize('TransferAnalysisFailed'));
                })
                .always(function() {
                    $('#analysisLoading').addClass('d-none');
                });
        }

        function displayAnalysisResults(analysis) {
            $('#usersToTransfer').text(analysis.totalUsersCount);
            $('#estimatedDuration').text(analysis.estimatedDurationMinutes + ' ' + app.localize('Minutes'));
            
            if (analysis.warnings && analysis.warnings.length > 0) {
                var warningsList = $('#warningsList');
                warningsList.empty();
                $.each(analysis.warnings, function(index, warning) {
                    warningsList.append($('<li>').text(warning));
                });
                $('#analysisWarnings').removeClass('d-none');
            } else {
                $('#analysisWarnings').addClass('d-none');
            }
            
            var categoriesCount = analysis.requirementCategories ? analysis.requirementCategories.length : 0;
            var noTransferCount = analysis.noTransferRequiredCategories ? analysis.noTransferRequiredCategories.length : 0;
            
            // Build HTML for categories info with lists
            var categoriesHtml = '<div class="requirements-analysis">';
            
            // Requirements that need mapping
            if (categoriesCount > 0) {
                categoriesHtml += '<div class="mb-3">';
                categoriesHtml += '<h6 class="text-danger">' + app.localize('RequirementsThatNeedMapping') + ' (' + categoriesCount + '):</h6>';
                categoriesHtml += '<ul class="requirement-list">';
                $.each(analysis.requirementCategories, function(index, req) {
                    categoriesHtml += '<li><strong>' + req.categoryName + '</strong>: ' + (req.requirementName || 'No Requirement') + '</li>';
                });
                categoriesHtml += '</ul>';
                categoriesHtml += '</div>';
            }
            
            // Requirements that don't need mapping (already exist in target)
            if (noTransferCount > 0) {
                categoriesHtml += '<div class="mb-3">';
                categoriesHtml += '<h6 class="text-success">' + app.localize('RequirementsAlreadyInTarget') + ' (' + noTransferCount + '):</h6>';
                categoriesHtml += '<ul class="requirement-list">';
                $.each(analysis.noTransferRequiredCategories, function(index, req) {
                    categoriesHtml += '<li><strong>' + req.categoryName + '</strong>: ' + (req.requirementName || 'No Requirement') + '</li>';
                });
                categoriesHtml += '</ul>';
                categoriesHtml += '</div>';
            }
            
            // Summary text
            categoriesHtml += '<div class="alert alert-info">';
            categoriesHtml += '<strong>' + app.localize('Summary') + ':</strong> ';
            categoriesHtml += categoriesCount + ' ' + app.localize('requirementsNeedMapping');
            if (noTransferCount > 0) {
                categoriesHtml += ', ' + noTransferCount + ' ' + app.localize('requirementsAlreadyExist');
            }
            categoriesHtml += '</div>';
            categoriesHtml += '</div>';
            
            $('#categoriesInfo').html(categoriesHtml);
            $('#analysisResults').removeClass('d-none');
            
            // Enable next button if analysis is successful and can transfer
            if (analysis.canTransfer) {
                $('#nextStepBtn').prop('disabled', false);
            } else {
                $('#nextStepBtn').prop('disabled', true);
                abp.message.error(app.localize('CannotProceedWithTransfer'));
            }
        }

        function loadCohortsForDepartment(departmentId) {
            abp.ui.setBusy($('#targetCohortSection'));
            
            abp.services.app.userTransfer.getCohortsForDepartment(departmentId)
                .done(function(result) {
                    console.log('GetCohortsForDepartment result:', result);
                    var cohorts = result;
                    var $select = $('#targetCohortId');
                    $select.empty();
                    $select.append($('<option>').val('').text(app.localize('PleaseSelect')));
                    
                    $.each(cohorts, function(index, cohort) {
                        var optionText = cohort.name + ' (' + cohort.usersCount + ' ' + app.localize('Users') + ')';
                        $select.append($('<option>').val(cohort.id).text(optionText));
                    });
                    
                    $('#targetCohortSection').show();
                })
                .fail(function(error) {
                    console.error('Failed to load cohorts:', error);
                    abp.message.error(app.localize('FailedToLoadCohorts'));
                })
                .always(function() {
                    abp.ui.clearBusy($('#targetCohortSection'));
                });
        }
        
        function loadCohortsWithoutDepartment() {
            abp.ui.setBusy($('#targetCohortSection'));
            
            abp.services.app.userTransfer.getCohortsWithoutDepartment()
                .done(function(result) {
                    console.log('GetCohortsWithoutDepartment result:', result);
                    var cohorts = result;
                    var $select = $('#targetCohortId');
                    $select.empty();
                    $select.append($('<option>').val('').text(app.localize('PleaseSelect')));
                    
                    $.each(cohorts, function(index, cohort) {
                        var optionText = cohort.name + ' (' + cohort.usersCount + ' ' + app.localize('Users') + ')';
                        if (cohort.isDefaultCohort) {
                            optionText += ' [' + app.localize('Default') + ']';
                        }
                        $select.append($('<option>').val(cohort.id).text(optionText));
                    });
                    
                    if (cohorts.length === 0) {
                        $select.append($('<option>').val('').text(app.localize('NoCohortsWithoutDepartment')));
                    }
                    
                    $('#targetCohortSection').show();
                })
                .fail(function(error) {
                    console.error('Failed to load cohorts without department:', error);
                    abp.message.error(app.localize('FailedToLoadCohorts'));
                })
                .always(function() {
                    abp.ui.clearBusy($('#targetCohortSection'));
                });
        }
        
        function loadCohortStatistics(cohortId) {
            // Find the selected cohort from the dropdown to get basic info
            var selectedOption = $('#targetCohortId option:selected');
            var cohortName = selectedOption.text().split(' (')[0];
            var usersCount = selectedOption.text().match(/\((\d+)/)[1];
            var departmentId = $('#targetDepartmentId').val();
            var departmentName = departmentId === '00000000-0000-0000-0000-000000000000' 
                ? app.localize('NoDepartment') 
                : $('#targetDepartmentId option:selected').text();
            
            $('#cohortUsersCount').text(usersCount);
            $('#cohortDepartmentName').text(departmentName);
            $('#isDefaultCohort').text(app.localize('No')); // TODO: Get from cohort data
            
            $('#cohortStats').removeClass('d-none');
        }

        function validateCurrentStep() {
            if (_currentStep === 1) {
                var targetDepartmentId = $('#targetDepartmentId').val();
                var targetCohortId = $('#targetCohortId').val();
                
                // Department selection is now optional (can be "None")
                // We just need to ensure a cohort is selected
                
                if (!targetCohortId) {
                    abp.message.warn(app.localize('PleaseSelectTargetCohort'));
                    return false;
                }
                
                _transferData.targetDepartmentId = targetDepartmentId;
                _transferData.targetCohortId = targetCohortId;
                
                // Check if analysis has been done
                if (!_transferData.analysisResult) {
                    abp.message.warn(app.localize('PleaseWaitForAnalysisToComplete'));
                    return false;
                }
                
                return true;
            } else if (_currentStep === 2) {
                // Validate user selection
                updateSelectedUsers();
                if (_transferData.selectedUsers.length === 0) {
                    abp.message.warn(app.localize('PleaseSelectAtLeastOneUser'));
                    return false;
                }
                return true;
            } else if (_currentStep === 3) {
                // Validate category mappings (if needed)
                if (_transferData.analysisResult && _transferData.analysisResult.requiresCategoryMapping) {
                    var errors = validateCategoryMappings();
                    if (errors.length > 0) {
                        // Show first error
                        abp.message.warn(errors[0]);
                        return false;
                    }
                }
                return true;
            } else if (_currentStep === 4) {
                // Confirmation step
                return true;
            }
            
            return true;
        }

        function validateCategoryMappings() {
            var errors = [];
            
            if (!_transferData.analysisResult || !_transferData.analysisResult.requirementCategories) {
                return errors;
            }
            
            // Get selected category IDs
            var selectedCategoryIds = [];
            $('.category-checkbox:checked').each(function() {
                selectedCategoryIds.push($(this).data('category-id'));
            });
            
            if (selectedCategoryIds.length === 0) {
                errors.push(app.localize('PleaseSelectAtLeastOneCategory'));
                return errors;
            }
            
            _transferData.analysisResult.requirementCategories.forEach(function(category) {
                var categoryId = category.categoryId;
                
                // Only validate selected categories
                if (selectedCategoryIds.indexOf(categoryId) === -1) {
                    return; // Skip unselected categories
                }
                
                var $actionSelect = $('.mapping-action-select[data-category-id="' + categoryId + '"]');
                var action = $actionSelect.val();
                
                if (!action) {
                    errors.push(app.localize('CategoryHasNoMappingAction', category.categoryName));
                } else if (action === 'map') {
                    var targetId = $('.target-category-select[data-category-id="' + categoryId + '"]').val();
                    if (!targetId) {
                        errors.push(app.localize('CategoryNeedsTargetSelection', category.categoryName));
                    }
                } else if (action === 'copy') {
                    var newReq = $('input[data-category-id="' + categoryId + '"][data-field="requirement"]').val();
                    var newCat = $('input[data-category-id="' + categoryId + '"][data-field="category"]').val();
                    if (!newReq || !newReq.trim()) {
                        errors.push(app.localize('CategoryNeedsNewRequirementName', category.categoryName));
                    }
                    if (!newCat || !newCat.trim()) {
                        errors.push(app.localize('CategoryNeedsNewCategoryName', category.categoryName));
                    }
                }
            });
            
            return errors;
        }

        function moveToNextStep() {
            if (_currentStep < _maxSteps) {
                _currentStep++;
                
                if (_currentStep === 2) {
                    loadUserSelectionStep();
                } else if (_currentStep === 3) {
                    // Only load category mappings if required
                    if (_transferData.analysisResult && _transferData.analysisResult.requiresCategoryMapping && 
                        _transferData.analysisResult.requirementCategories && 
                        _transferData.analysisResult.requirementCategories.length > 0) {
                        loadCategoryMappings();
                    } else {
                        // Skip to confirmation if no mapping needed
                        _currentStep++;
                        loadTransferSummary();
                    }
                } else if (_currentStep === 4) {
                    loadTransferSummary();
                }
                
                updateStepVisibility();
                updateNavigationButtons();
                updateProgressBar();
            }
        }

        function moveToPreviousStep() {
            if (_currentStep > 1) {
                _currentStep--;
                updateStepVisibility();
                updateNavigationButtons();
                updateProgressBar();
            }
        }

        function updateStepVisibility() {
            $('.wizard-step').addClass('d-none');
            $('#step' + _currentStep).removeClass('d-none');
            
            // Update step indicators
            $('.step-item').each(function(index) {
                var stepNumber = index + 1;
                if (stepNumber < _currentStep) {
                    $(this).addClass('completed').addClass('active');
                } else if (stepNumber === _currentStep) {
                    $(this).addClass('active').removeClass('completed');
                } else {
                    $(this).removeClass('active').removeClass('completed');
                }
            });
        }

        function updateNavigationButtons() {
            if (_currentStep === 1) {
                $('#prevStepBtn').hide();
                $('#nextStepBtn').show();
                $('#executeTransferBtn').hide();
            } else if (_currentStep === _maxSteps) {
                $('#prevStepBtn').show();
                $('#nextStepBtn').hide();
                $('#executeTransferBtn').show();
            } else {
                $('#prevStepBtn').show();
                $('#nextStepBtn').show();
                $('#executeTransferBtn').hide();
            }
        }

        function updateProgressBar() {
            var progress = ((_currentStep - 1) / (_maxSteps - 1)) * 100;
            $('.progress-bar').css('width', progress + '%').attr('aria-valuenow', progress);
        }
        
        function loadUserSelectionStep() {
            if (!_userDataTable) {
                initializeUserDataTable();
            } else {
                _userDataTable.ajax.reload();
            }
        }
        
        function initializeUserDataTable() {
            _userDataTable = $('#userSelectionTable').DataTable({
                paging: true,
                serverSide: true,
                ajax: function(data, callback, settings) {
                    var input = {
                        cohortId: _cohortInfo.cohortId,
                        filter: $('#userSearchFilter').val(),
                        skipCount: data.start,
                        maxResultCount: data.length
                    };
                    
                    abp.services.app.userTransfer.getCohortUsersForTransfer(input)
                        .done(function(result) {
                            callback({
                                recordsTotal: result.totalCount,
                                recordsFiltered: result.totalCount,
                                data: result.items
                            });
                        })
                        .fail(function() {
                            callback({
                                recordsTotal: 0,
                                recordsFiltered: 0,
                                data: []
                            });
                        });
                },
                columns: [
                    {
                        data: null,
                        orderable: false,
                        render: function(data, type, row) {
                            return '<input type="checkbox" class="user-select-checkbox" value="' + row.id + '">';
                        }
                    },
                    { data: 'userName' },
                    { data: 'fullName' },
                    { data: 'email' },
                    { data: 'complianceStatus' },
                    { data: 'recordStatesCount' }
                ],
                drawCallback: function() {
                    // Re-check previously selected users after search/pagination
                    if (_transferData.selectedUsers && _transferData.selectedUsers.length > 0) {
                        _transferData.selectedUsers.forEach(function(userId) {
                            $('.user-select-checkbox[value="' + userId + '"]').prop('checked', true);
                        });
                    }
                    updateSelectedUsersCount();
                    
                    // Update select all checkbox state
                    var allChecked = $('.user-select-checkbox').length > 0 && 
                                    $('.user-select-checkbox:not(:checked)').length === 0;
                    $('#selectAllCheckbox').prop('checked', allChecked);
                }
            });
            
            // Handle checkbox changes
            $('#userSelectionTable').on('change', '.user-select-checkbox', function() {
                var $checkbox = $(this);
                var cohortUserId = $checkbox.val();
                var $row = $checkbox.closest('tr');
                
                if ($checkbox.is(':checked')) {
                    // Store user details when selected
                    var userData = _userDataTable.row($row).data();
                    _transferData.selectedUserDetails[cohortUserId] = {
                        id: cohortUserId,
                        userName: userData.userName,
                        fullName: userData.fullName,
                        email: userData.email
                    };
                } else {
                    // Remove user details when deselected
                    delete _transferData.selectedUserDetails[cohortUserId];
                }
                
                updateSelectedUsersCount();
            });
            
            // Handle select all checkbox
            $('#selectAllCheckbox').on('change', function() {
                var isChecked = $(this).is(':checked');
                $('.user-select-checkbox').prop('checked', isChecked);
                
                if (isChecked) {
                    // Store all user details when selecting all
                    _userDataTable.rows().every(function() {
                        var userData = this.data();
                        _transferData.selectedUserDetails[userData.id] = {
                            id: userData.id,
                            userName: userData.userName,
                            fullName: userData.fullName,
                            email: userData.email
                        };
                    });
                } else {
                    // Clear all user details when deselecting all
                    _transferData.selectedUserDetails = {};
                }
                
                updateSelectedUsersCount();
            });
        }
        
        function updateSelectedUsersCount() {
            var count = $('.user-select-checkbox:checked').length;
            $('#selectedUsersCount').text(count);
        }
        
        function updateSelectedUsers() {
            _transferData.selectedUsers = [];
            _transferData.selectedUserDetails = {};
            
            $('.user-select-checkbox:checked').each(function() {
                var $checkbox = $(this);
                var cohortUserId = $checkbox.val();
                var $row = $checkbox.closest('tr');
                var userData = _userDataTable.row($row).data();
                
                _transferData.selectedUsers.push(cohortUserId);
                _transferData.selectedUserDetails[cohortUserId] = {
                    id: cohortUserId,
                    userName: userData.userName,
                    fullName: userData.fullName,
                    email: userData.email
                };
            });
        }

        function loadCategoryMappings() {
            // Hide loading state initially
            $('#categoryMappingLoading').show();
            $('#categoryMappingTableContainer').hide();
            $('#categoryMappingSummary').addClass('d-none');
            $('#noMigrationRequiredSection').addClass('d-none');
            $('#noCategoriesMessage').addClass('d-none');
            
            // Check if we have categories that need mapping
            if (!_transferData.analysisResult || !_transferData.analysisResult.requirementCategories || 
                _transferData.analysisResult.requirementCategories.length === 0) {
                $('#categoryMappingLoading').hide();
                $('#noCategoriesMessage').removeClass('d-none');
                return;
            }
            
            // Initialize category mapping data structure
            if (!_transferData.categoryMappings) {
                _transferData.categoryMappings = {};
            }
            if (!_transferData.newCategories) {
                _transferData.newCategories = {};
            }
            if (!_transferData.skippedCategories) {
                _transferData.skippedCategories = [];
            }
            
            // Display summary statistics
            displayCategoryMappingSummary();
            
            // Display requirements that don't need mapping
            if (_transferData.analysisResult.noTransferRequiredCategories && 
                _transferData.analysisResult.noTransferRequiredCategories.length > 0) {
                displayNoMigrationRequiredSection();
            }
            
            // Initialize DataTable for category mapping
            initializeCategoryMappingTable();
            
            // Load target categories for mapping dropdowns
            loadTargetCategoriesForMapping();
            
            // Show the table and hide loading
            setTimeout(function() {
                $('#categoryMappingLoading').hide();
                $('#categoryMappingTableContainer').show();
                $('#categoryMappingSummary').removeClass('d-none');
                
                // Initialize event handlers
                initializeCategoryMappingEventHandlers();
                updateMappingProgress();
            }, 500);
        }
        
        function displayCategoryMappingSummary() {
            var totalCategories = _transferData.analysisResult.requirementCategories.length;
            var totalAffectedUsers = 0;
            var totalRecordStates = 0;
            
            _transferData.analysisResult.requirementCategories.forEach(function(cat) {
                totalAffectedUsers += cat.affectedUsersCount || 0;
                totalRecordStates += cat.recordStatesCount || 0;
            });
            
            $('#totalCategoriesToMap').text(totalCategories);
            $('#totalAffectedUsersMapping').text(totalAffectedUsers);
            $('#totalRecordStatesMapping').text(totalRecordStates);
            $('#mappingProgressBadge').text('0/' + totalCategories);
        }
        
        function displayNoMigrationRequiredSection() {
            var $section = $('#noMigrationRequiredSection');
            var $list = $('#noMigrationRequiredList');
            
            $list.empty();
            var $ul = $('<ul class="mb-0"></ul>');
            
            _transferData.analysisResult.noTransferRequiredCategories.forEach(function(req) {
                $ul.append('<li><strong>' + req.categoryName + '</strong>: ' + (req.requirementName || 'No Requirement') + '</li>');
            });
            
            $list.append($ul);
            $section.removeClass('d-none');
        }
        
        function initializeCategoryMappingTable() {
            if (_categoryMappingTable) {
                _categoryMappingTable.destroy();
            }
            
            // Set default mapping action to 'map' for all categories
            _transferData.analysisResult.requirementCategories.forEach(function(category) {
                if (!category.mappingAction) {
                    category.mappingAction = 'map';
                }
            });
            
            _categoryMappingTable = $('#categoryMappingTable').DataTable({
                paging: false,
                serverSide: false,
                processing: false,
                searching: true,
                ordering: true,
                data: _transferData.analysisResult.requirementCategories,
                columns: [
                    {
                        data: null,
                        orderable: false,
                        render: function(data, type, row) {
                            return '<div class="form-check">' +
                                   '<input type="checkbox" class="form-check-input category-checkbox" ' +
                                   'data-category-id="' + row.categoryId + '">' +
                                   '</div>';
                        }
                    },
                    {
                        data: null,
                        render: function(data, type, row) {
                            var html = '<div class="category-info">';
                            html += '<div class="category-name"><strong>' + row.categoryName + '</strong></div>';
                            html += '</div>';
                            return html;
                        }
                    },
                    {
                        data: null,
                        render: function(data, type, row) {
                            var html = '<div class="requirement-info">';
                            html += '<div class="requirement-name">' + (row.requirementName || 'No Requirement') + '</div>';
                            
                            // Show hierarchy level badge
                            var hierarchyClass = 'badge badge-sm ';
                            var hierarchyIcon = '';
                            var hierarchyLevel = row.hierarchyLevel || 'Unknown';
                            
                            switch(hierarchyLevel) {
                                case 'Tenant':
                                    hierarchyClass += 'badge-primary';
                                    hierarchyIcon = '<i class="fas fa-building"></i> ';
                                    break;
                                case 'Department':
                                    hierarchyClass += 'badge-info';
                                    hierarchyIcon = '<i class="fas fa-sitemap"></i> ';
                                    break;
                                case 'Cohort':
                                    hierarchyClass += 'badge-success';
                                    hierarchyIcon = '<i class="fas fa-users"></i> ';
                                    break;
                                case 'CohortAndDepartment':
                                    hierarchyClass += 'badge-warning';
                                    hierarchyIcon = '<i class="fas fa-users"></i><i class="fas fa-sitemap"></i> ';
                                    break;
                                default:
                                    hierarchyClass += 'badge-secondary';
                            }
                            html += '<span class="' + hierarchyClass + '" style="font-size: 0.8rem;">' + 
                                    hierarchyIcon + hierarchyLevel + '</span>';
                            
                            html += '</div>';
                            return html;
                        }
                    },
                    {
                        data: null,
                        render: function(data, type, row) {
                            var html = '<div class="impact-info text-center">';
                            html += '<div>' + (row.affectedUsersCount || 0) + ' Users</div>';
                            html += '<div>' + (row.recordStatesCount || 0) + ' Records</div>';
                            html += '</div>';
                            return html;
                        }
                    },
                    {
                        data: null,
                        orderable: false,
                        render: function(data, type, row) {
                            return renderMappingActionSelect(row);
                        }
                    },
                    {
                        data: null,
                        orderable: false,
                        render: function(data, type, row) {
                            return renderTargetMappingSection(row);
                        }
                    },
                    {
                        data: null,
                        orderable: false,
                        render: function(data, type, row) {
                            return '<button class="btn btn-sm btn-outline-info suggestions-btn" ' +
                                   'data-category-id="' + row.categoryId + '">' +
                                   '<i class="fas fa-lightbulb"></i></button>';
                        }
                    },
                    {
                        data: null,
                        render: function(data, type, row) {
                            return '<div class="text-center">' +
                                   '<span class="badge badge-warning mapping-status" data-category-id="' + row.categoryId + '">Pending</span>' +
                                   '</div>';
                        }
                    }
                ],
                drawCallback: function() {
                    // Re-initialize any event handlers after redraw
                    setTimeout(function() {
                        restoreMappingSelections();
                    }, 100);
                }
            });
        }
        
        function renderMappingActionSelect(row) {
            var html = '<div class="mapping-action-container" style="display:none;">';
            html += '<select class="form-control form-control-sm mapping-action-select" data-category-id="' + row.categoryId + '">';
            html += '<option value="map"' + (row.mappingAction === 'map' || !row.mappingAction ? ' selected' : '') + '>Map to Existing</option>';
            html += '<option value="copy"' + (row.mappingAction === 'copy' ? ' selected' : '') + '>Copy to New</option>';
            html += '<option value="skip"' + (row.mappingAction === 'skip' ? ' selected' : '') + '>Skip</option>';
            html += '</select>';
            html += '</div>';
            return html;
        }
        
        function renderTargetMappingSection(row) {
            var html = '<div class="target-mapping-section" style="display:none;">';
            
            // Map to existing section
            html += '<div class="target-category-section" style="display:none;">';
            html += '<select class="form-control form-control-sm target-category-select" data-category-id="' + row.categoryId + '">';
            html += '<option value="">Select Target Category</option>';
            html += '</select>';
            html += '</div>';
            
            // Copy to new section
            html += '<div class="new-category-section" style="display:none;">';
            html += '<input type="text" class="form-control form-control-sm mb-1 new-category-input" ' +
                    'placeholder="' + app.localize('NewRequirementName') + '" ' +
                    'data-category-id="' + row.categoryId + '" data-field="requirement">';
            html += '<input type="text" class="form-control form-control-sm new-category-input" ' +
                    'placeholder="' + app.localize('NewCategoryName') + '" ' +
                    'data-category-id="' + row.categoryId + '" data-field="category">';
            html += '</div>';
            
            // Skip warning section
            html += '<div class="skip-warning-section" style="display:none;">';
            html += '<div class="alert alert-warning mb-0 py-1 px-2 small">' +
                    '<i class="fas fa-exclamation-triangle"></i> ' + app.localize('DataWillBeLost') + '</div>';
            html += '</div>';
            
            html += '</div>';
            return html;
        }
        
        function loadTargetCategoriesForMapping() {
            // Target categories are now loaded on-demand for each source category
            // This provides better matching and is consistent with the cohort migration wizard
            console.log('Target categories will be loaded on-demand when mapping action is selected');
        }
        
        function restoreMappingSelections() {
            // Restore any previously made selections after DataTable redraw
            Object.keys(_transferData.categoryMappings).forEach(function(categoryId) {
                var $select = $('.mapping-action-select[data-category-id="' + categoryId + '"]');
                if ($select.length) {
                    $select.val('map').trigger('change');
                    
                    var $targetSelect = $('.target-category-select[data-category-id="' + categoryId + '"]');
                    if ($targetSelect.length && _transferData.categoryMappings[categoryId]) {
                        $targetSelect.val(_transferData.categoryMappings[categoryId]);
                    }
                }
            });
            
            // Restore new category inputs
            if (_transferData.newCategories) {
                Object.keys(_transferData.newCategories).forEach(function(categoryId) {
                    var $select = $('.mapping-action-select[data-category-id="' + categoryId + '"]');
                    if ($select.length) {
                        $select.val('copy').trigger('change');
                        
                        var newCatData = _transferData.newCategories[categoryId];
                        if (newCatData.requirement) {
                            $('input[data-category-id="' + categoryId + '"][data-field="requirement"]').val(newCatData.requirement);
                        }
                        if (newCatData.category) {
                            $('input[data-category-id="' + categoryId + '"][data-field="category"]').val(newCatData.category);
                        }
                    }
                });
            }
            
            // Restore skipped categories
            if (_transferData.skippedCategories) {
                _transferData.skippedCategories.forEach(function(categoryId) {
                    var $select = $('.mapping-action-select[data-category-id="' + categoryId + '"]');
                    if ($select.length) {
                        $select.val('skip').trigger('change');
                    }
                });
            }
        }

        function loadTransferSummary() {
            var container = $('#transferSummary');
            
            var summaryHtml = '<div class="row">';
            summaryHtml += '<div class="col-md-6">';
            summaryHtml += '<h6>' + app.localize('TransferDetails') + '</h6>';
            summaryHtml += '<p><strong>' + app.localize('SourceCohort') + ':</strong> ' + _cohortInfo.cohortName + '</p>';
            
            var targetCohortName = $('#targetCohortId option:selected').text().split(' (')[0];
            summaryHtml += '<p><strong>' + app.localize('TargetCohort') + ':</strong> ' + targetCohortName + '</p>';
            
            var departmentName = $('#targetDepartmentId option:selected').text();
            summaryHtml += '<p><strong>' + app.localize('TargetDepartment') + ':</strong> ' + departmentName + '</p>';
            
            summaryHtml += '</div>';
            summaryHtml += '<div class="col-md-6">';
            summaryHtml += '<h6>' + app.localize('Summary') + '</h6>';
            summaryHtml += '<p><strong>' + app.localize('SelectedUsers') + ':</strong> ' + _transferData.selectedUsers.length + '</p>';
            summaryHtml += '<p><strong>' + app.localize('EstimatedDuration') + ':</strong> ' + (_transferData.analysisResult ? _transferData.analysisResult.estimatedDurationMinutes + ' ' + app.localize('Minutes') : '-') + '</p>';
            
            if (_transferData.analysisResult && _transferData.analysisResult.requiresCategoryMapping) {
                summaryHtml += '<p><strong>' + app.localize('CategoryMappings') + ':</strong> ' + app.localize('Required') + '</p>';
            } else {
                summaryHtml += '<p><strong>' + app.localize('CategoryMappings') + ':</strong> ' + app.localize('NotRequired') + '</p>';
            }
            
            summaryHtml += '</div>';
            summaryHtml += '</div>';
            
            // List selected users
            if (_transferData.selectedUsers.length > 0 && _transferData.selectedUsers.length <= 10) {
                summaryHtml += '<hr>';
                summaryHtml += '<h6>' + app.localize('SelectedUsers') + ':</h6>';
                summaryHtml += '<ul id="selectedUsersList"></ul>';
            } else if (_transferData.selectedUsers.length > 10) {
                summaryHtml += '<hr>';
                summaryHtml += '<p>' + app.localize('TooManyUsersToList', _transferData.selectedUsers.length) + '</p>';
            }
            
            container.html(summaryHtml);
            
            // Load user names if not too many
            if (_transferData.selectedUsers.length > 0 && _transferData.selectedUsers.length <= 10) {
                loadSelectedUserNames();
            }
        }
        
        function loadSelectedUserNames() {
            var $list = $('#selectedUsersList');
            _transferData.selectedUsers.forEach(function(userId) {
                var userDetails = _transferData.selectedUserDetails[userId];
                if (userDetails) {
                    var userInfo = userDetails.fullName || userDetails.userName;
                    if (userDetails.email) {
                        userInfo += ' (' + userDetails.email + ')';
                    }
                    $list.append('<li>' + userInfo + '</li>');
                } else {
                    // Fallback if details not found
                    $list.append('<li>' + app.localize('User') + ' ID: ' + userId + '</li>');
                }
            });
        }

        function initializeCategoryMappingEventHandlers() {
            var modal = _modalManager.getModal();
            
            // Mapping action change handler
            modal.off('change', '.mapping-action-select').on('change', '.mapping-action-select', function() {
                var categoryId = $(this).data('category-id');
                var action = $(this).val();
                console.log('Mapping action changed:', { categoryId: categoryId, action: action });
                
                updateCategoryMappingAction(categoryId, action);
                updateTargetMappingSectionVisibility($(this).closest('tr'), action);
                updateMappingProgress();
            });
            
            // Target category selection handler
            modal.off('change', '.target-category-select').on('change', '.target-category-select', function() {
                var categoryId = $(this).data('category-id');
                var targetCategoryId = $(this).val();
                
                if (targetCategoryId) {
                    _transferData.categoryMappings[categoryId] = targetCategoryId;
                } else {
                    delete _transferData.categoryMappings[categoryId];
                }
                
                updateMappingProgress();
            });
            
            // New category input handlers
            modal.off('input', '.new-category-input').on('input', '.new-category-input', function() {
                var categoryId = $(this).data('category-id');
                var field = $(this).data('field');
                var value = $(this).val();
                
                if (!_transferData.newCategories[categoryId]) {
                    _transferData.newCategories[categoryId] = {};
                }
                
                _transferData.newCategories[categoryId][field] = value;
                updateMappingProgress();
            });
            
            // Suggestions button handler
            modal.off('click', '.suggestions-btn').on('click', '.suggestions-btn', function() {
                var categoryId = $(this).data('category-id');
                loadCategorySuggestions(categoryId, $(this));
            });
            
            // Select all categories checkbox
            modal.off('change', '#selectAllCategories').on('change', '#selectAllCategories', function() {
                var isChecked = $(this).is(':checked');
                $('.category-checkbox').each(function() {
                    var $checkbox = $(this);
                    var categoryId = $checkbox.data('category-id');
                    var $row = $checkbox.closest('tr');
                    
                    $checkbox.prop('checked', isChecked);
                    handleRowSelection($row, categoryId, isChecked);
                });
            });
            
            // Individual category checkbox handler
            modal.off('change', '.category-checkbox').on('change', '.category-checkbox', function() {
                var $checkbox = $(this);
                var categoryId = $checkbox.data('category-id');
                var isChecked = $checkbox.is(':checked');
                var $row = $checkbox.closest('tr');
                
                handleRowSelection($row, categoryId, isChecked);
                updateSelectAllCheckboxState();
            });
            
            // Bulk action handlers
            modal.off('click', '#bulkMapBtn').on('click', '#bulkMapBtn', function() {
                applyBulkAction('map');
            });
            
            modal.off('click', '#bulkCopyBtn').on('click', '#bulkCopyBtn', function() {
                applyBulkAction('copy');
            });
            
            modal.off('click', '#bulkSkipBtn').on('click', '#bulkSkipBtn', function() {
                applyBulkAction('skip');
            });
            
            // Auto suggest handler
            modal.off('click', '#autoSuggestBtn').on('click', '#autoSuggestBtn', function() {
                autoSuggestMappings();
            });
            
            // Validate mappings handler
            modal.off('click', '#validateMappingsBtn').on('click', '#validateMappingsBtn', function() {
                validateAndShowMappingResults();
            });
        }
        
        function handleRowSelection($row, categoryId, isChecked) {
            var $actionContainer = $row.find('.mapping-action-container');
            var $targetMappingSection = $row.find('.target-mapping-section');
            
            if (isChecked) {
                // Show the mapping action dropdown
                $actionContainer.show();
                $targetMappingSection.show();
                
                // Get current action - default to 'map' if none selected
                var currentAction = $row.find('.mapping-action-select').val() || 'map';
                
                // Update the select to ensure 'map' is selected if nothing was selected
                if (!$row.find('.mapping-action-select').val()) {
                    $row.find('.mapping-action-select').val('map');
                }
                
                // Update target mapping visibility and populate dropdown
                updateTargetMappingSectionVisibility($row, currentAction);
                
                // If action is 'map', populate the target category dropdown
                if (currentAction === 'map') {
                    populateTargetCategoryDropdown(categoryId);
                }
            } else {
                // Hide the mapping controls
                $actionContainer.hide();
                $targetMappingSection.hide();
            }
        }
        
        function updateCategoryMappingAction(categoryId, action) {
            if (!action) {
                // Remove from all collections
                delete _transferData.categoryMappings[categoryId];
                delete _transferData.newCategories[categoryId];
                _transferData.skippedCategories = _transferData.skippedCategories.filter(id => id !== categoryId);
            } else if (action === 'skip') {
                // Add to skipped, remove from others
                if (!_transferData.skippedCategories.includes(categoryId)) {
                    _transferData.skippedCategories.push(categoryId);
                }
                delete _transferData.categoryMappings[categoryId];
                delete _transferData.newCategories[categoryId];
            } else if (action === 'map') {
                // Remove from skipped and new categories
                _transferData.skippedCategories = _transferData.skippedCategories.filter(id => id !== categoryId);
                delete _transferData.newCategories[categoryId];
                // categoryMappings will be set when target is selected
            } else if (action === 'copy') {
                // Remove from skipped and mappings
                _transferData.skippedCategories = _transferData.skippedCategories.filter(id => id !== categoryId);
                delete _transferData.categoryMappings[categoryId];
                // newCategories will be set when inputs are filled
            }
        }
        
        function updateTargetMappingSectionVisibility($row, action) {
            var $targetSection = $row.find('.target-mapping-section');
            var $mapSection = $row.find('.target-category-section');
            var $copySection = $row.find('.new-category-section');
            var $skipSection = $row.find('.skip-warning-section');
            
            // Hide all sections first
            $mapSection.hide();
            $copySection.hide();
            $skipSection.hide();
            
            // Show relevant section
            if (action === 'map') {
                $targetSection.show();
                $mapSection.show();
                
                // Load target categories if not already loaded
                var categoryId = $row.find('.mapping-action-select').data('category-id');
                populateTargetCategoryDropdown(categoryId);
            } else if (action === 'copy') {
                $targetSection.show();
                $copySection.show();
            } else if (action === 'skip') {
                $targetSection.show();
                $skipSection.show();
            } else {
                $targetSection.hide();
            }
            
            // Update status icon
            updateCategoryStatusIcon($row, action);
        }
        
        function populateTargetCategoryDropdown(categoryId) {
            var $select = $('.target-category-select[data-category-id="' + categoryId + '"]');
            
            // Get target department/cohort info
            var targetDepartmentId = null;
            if (_transferData.analysisResult && _transferData.analysisResult.targetDepartmentId) {
                targetDepartmentId = _transferData.analysisResult.targetDepartmentId;
            } else {
                var selectedDeptId = $('#targetDepartmentId').val();
                if (selectedDeptId && selectedDeptId !== '' && selectedDeptId !== '00000000-0000-0000-0000-000000000000') {
                    targetDepartmentId = selectedDeptId;
                }
            }
            
            var targetCohortId = _transferData.targetCohortId || $('#targetCohortId').val();
            
            // Clear and add loading option
            $select.empty();
            $select.append('<option value="">' + app.localize('Loading') + '...</option>');
            
            // Load target category options specific to this source category
            _userTransferService.getTargetCategoryOptions({
                sourceCategoryId: categoryId,
                targetDepartmentId: targetDepartmentId,
                targetCohortId: targetCohortId,
                maxResults: 20
            })
            .done(function(result) {
                // Clear and add default option
                $select.empty();
                $select.append('<option value="">' + app.localize('SelectTargetCategory') + '</option>');
                
                if (result && result.length > 0) {
                    // Sort by match score descending
                    result.sort(function(a, b) {
                        var scoreA = a.matchScore || a.MatchScore || 0;
                        var scoreB = b.matchScore || b.MatchScore || 0;
                        return scoreB - scoreA;
                    });
                    
                    result.forEach(function(option) {
                        // Handle both camelCase and PascalCase property names
                        var categoryName = option.categoryName || option.CategoryName;
                        var requirementName = option.requirementName || option.RequirementName;
                        var matchScore = option.matchScore || option.MatchScore;
                        var optionCategoryId = option.categoryId || option.CategoryId;
                        
                        var displayText = categoryName;
                        if (requirementName) {
                            displayText += ' - ' + requirementName;
                        }
                        if (matchScore) {
                            displayText += ' (' + matchScore + '% ' + app.localize('Match') + ')';
                        }
                        
                        $select.append('<option value="' + optionCategoryId + '">' + displayText + '</option>');
                    });
                } else {
                    $select.append('<option value="" disabled>' + app.localize('NoCompatibleCategoriesFound') + '</option>');
                }
                
                // Restore previously selected value if any
                if (_transferData.categoryMappings[categoryId]) {
                    $select.val(_transferData.categoryMappings[categoryId]);
                }
            })
            .fail(function(error) {
                console.error('Failed to load target category options:', error);
                $select.empty();
                $select.append('<option value="">' + app.localize('ErrorLoadingCategories') + '</option>');
            });
        }
        
        function updateCategoryStatusIcon($row, action) {
            var $badge = $row.find('.mapping-status');
            
            if (!action) {
                $badge.removeClass('badge-success badge-danger').addClass('badge-warning');
                $badge.text('Pending');
            } else if (action === 'skip') {
                $badge.removeClass('badge-success badge-warning').addClass('badge-danger');
                $badge.text('Skip');
            } else {
                $badge.removeClass('badge-warning badge-danger').addClass('badge-success');
                $badge.text('Mapped');
            }
        }
        
        function updateMappingProgress() {
            // Get selected categories
            var selectedCategoryIds = [];
            $('.category-checkbox:checked').each(function() {
                selectedCategoryIds.push($(this).data('category-id'));
            });
            
            var totalSelected = selectedCategoryIds.length;
            var mappedCount = 0;
            
            selectedCategoryIds.forEach(function(categoryId) {
                if (_transferData.categoryMappings[categoryId] || 
                    (_transferData.newCategories && _transferData.newCategories[categoryId]) || 
                    (_transferData.skippedCategories && _transferData.skippedCategories.includes(categoryId))) {
                    mappedCount++;
                }
            });
            
            $('#mappingProgressBadge').text(mappedCount + '/' + totalSelected);
            
            // Update next button state based on progress
            // Enable if no categories selected (skip step) or all selected are mapped
            if (totalSelected === 0 || mappedCount === totalSelected) {
                $('#nextStepBtn').prop('disabled', false);
            } else {
                $('#nextStepBtn').prop('disabled', true);
            }
        }
        
        
        function updateSelectAllCheckboxState() {
            var totalCheckboxes = $('.category-checkbox').length;
            var checkedCheckboxes = $('.category-checkbox:checked').length;
            
            $('#selectAllCategories').prop('checked', totalCheckboxes === checkedCheckboxes);
            $('#selectAllCategories').prop('indeterminate', checkedCheckboxes > 0 && checkedCheckboxes < totalCheckboxes);
        }
        
        function applyBulkAction(action) {
            var selectedCategories = [];
            
            $('.category-checkbox:checked').each(function() {
                selectedCategories.push($(this).data('category-id'));
            });
            
            if (selectedCategories.length === 0) {
                abp.message.warn(app.localize('PleaseSelectCategories'));
                return;
            }
            
            selectedCategories.forEach(function(categoryId) {
                var $select = $('.mapping-action-select[data-category-id="' + categoryId + '"]');
                $select.val(action).trigger('change');
            });
            
            abp.message.success(app.localize('BulkActionApplied', selectedCategories.length));
        }
        
        function autoSuggestMappings() {
            abp.ui.setBusy($('#categoryMappingTable'));
            
            var targetDepartmentId = _transferData.analysisResult ? _transferData.analysisResult.targetDepartmentId : $('#targetDepartmentId').val();
            if (targetDepartmentId === '00000000-0000-0000-0000-000000000000') {
                // Convert Guid.Empty to null for cohorts without departments
                targetDepartmentId = null;
            }
            
            var processedCount = 0;
            var suggestedCount = 0;
            var categoriesToProcess = _transferData.analysisResult.requirementCategories.filter(function(sourceCategory) {
                var $select = $('.mapping-action-select[data-category-id="' + sourceCategory.categoryId + '"]');
                return !$select.val(); // Only process unmapped categories
            });
            
            if (categoriesToProcess.length === 0) {
                abp.ui.clearBusy($('#categoryMappingTable'));
                abp.message.info(app.localize('AllCategoriesAlreadyMapped'));
                return;
            }
            
            // Process each category
            categoriesToProcess.forEach(function(sourceCategory, index) {
                setTimeout(function() {
                    _userTransferService.getTargetCategoryOptions({
                        sourceCategoryId: sourceCategory.categoryId,
                        targetDepartmentId: targetDepartmentId,
                        targetCohortId: _transferData.targetCohortId,
                        maxResults: 1
                    })
                    .done(function(result) {
                        processedCount++;
                        
                        if (result && result.length > 0 && result[0].matchScore >= 80) {
                            var bestMatch = result[0];
                            var $select = $('.mapping-action-select[data-category-id="' + sourceCategory.categoryId + '"]');
                            $select.val('map').trigger('change');
                            
                            // Set the target category after the dropdown is populated
                            setTimeout(function() {
                                var $targetSelect = $('.target-category-select[data-category-id="' + sourceCategory.categoryId + '"]');
                                $targetSelect.val(bestMatch.categoryId || bestMatch.CategoryId);
                                _transferData.categoryMappings[sourceCategory.categoryId] = bestMatch.categoryId || bestMatch.CategoryId;
                                updateMappingProgress();
                            }, 500);
                            
                            suggestedCount++;
                        }
                        
                        // Check if all categories have been processed
                        if (processedCount === categoriesToProcess.length) {
                            abp.ui.clearBusy($('#categoryMappingTable'));
                            
                            if (suggestedCount > 0) {
                                abp.message.success(app.localize('AutoSuggestComplete', suggestedCount));
                            } else {
                                abp.message.info(app.localize('NoHighConfidenceMatchesFound'));
                            }
                        }
                    })
                    .fail(function() {
                        processedCount++;
                        if (processedCount === categoriesToProcess.length) {
                            abp.ui.clearBusy($('#categoryMappingTable'));
                        }
                    });
                }, index * 200); // Stagger requests to avoid overwhelming the server
            });
        }
        
        function findBestMatch(sourceCategory, targetCategories) {
            if (!targetCategories || targetCategories.length === 0) {
                return null;
            }
            
            var bestMatch = null;
            var highestScore = 0;
            
            targetCategories.forEach(function(targetCategory) {
                var score = calculateSimilarityScore(
                    sourceCategory.categoryName + ' ' + (sourceCategory.requirementName || 'No Requirement'),
                    targetCategory.categoryName + ' ' + (targetCategory.requirementName || 'No Requirement')
                );
                
                if (score > highestScore) {
                    highestScore = score;
                    bestMatch = {
                        categoryId: targetCategory.id,
                        confidence: score
                    };
                }
            });
            
            return bestMatch;
        }
        
        function calculateSimilarityScore(str1, str2) {
            // Simple similarity calculation based on common words
            var words1 = str1.toLowerCase().split(/\s+/);
            var words2 = str2.toLowerCase().split(/\s+/);
            
            var commonWords = 0;
            words1.forEach(function(word) {
                if (words2.includes(word) && word.length > 2) {
                    commonWords++;
                }
            });
            
            return commonWords / Math.max(words1.length, words2.length);
        }
        
        function loadCategorySuggestions(categoryId, $button) {
            $button.prop('disabled', true);
            $button.html('<i class="fas fa-spinner fa-spin"></i>');
            
            // Find the source category
            var sourceCategory = _transferData.analysisResult.requirementCategories.find(c => c.categoryId === categoryId);
            if (!sourceCategory) {
                $button.prop('disabled', false);
                $button.html('<i class="fas fa-lightbulb"></i>');
                return;
            }
            
            var targetDepartmentId = _transferData.analysisResult ? _transferData.analysisResult.targetDepartmentId : $('#targetDepartmentId').val();
            if (targetDepartmentId === '00000000-0000-0000-0000-000000000000') {
                // Convert Guid.Empty to null for cohorts without departments
                targetDepartmentId = null;
            }
            
            // Load suggestions from server
            _userTransferService.getTargetCategoryOptions({
                sourceCategoryId: categoryId,
                targetDepartmentId: targetDepartmentId,
                targetCohortId: _transferData.targetCohortId,
                maxResults: 3
            })
            .done(function(result) {
                $button.prop('disabled', false);
                $button.html('<i class="fas fa-lightbulb"></i>');
                
                if (result && result.length > 0) {
                    var message = app.localize('TopSuggestions') + ':\n';
                    result.forEach(function(suggestion, index) {
                        var categoryName = suggestion.categoryName || suggestion.CategoryName;
                        var requirementName = suggestion.requirementName || suggestion.RequirementName;
                        var matchScore = suggestion.matchScore || suggestion.MatchScore || 0;
                        
                        message += (index + 1) + '. ' + categoryName + ' - ' + requirementName + 
                                  ' (' + matchScore + '% match)\n';
                    });
                    
                    abp.message.info(message, app.localize('Suggestions'));
                } else {
                    abp.message.info(app.localize('NoSuggestionsFoundForThisCategory'));
                }
            })
            .fail(function(error) {
                $button.prop('disabled', false);
                $button.html('<i class="fas fa-lightbulb"></i>');
                console.error('Failed to load suggestions:', error);
                abp.message.error(app.localize('FailedToLoadSuggestions'));
            });
        }
        
        function validateAndShowMappingResults() {
            var validationErrors = validateCategoryMappings();
            
            if (validationErrors.length === 0) {
                $('#mappingValidationMessages').addClass('d-none');
                abp.message.success(app.localize('AllMappingsValid'));
            } else {
                var $list = $('#validationIssuesList');
                $list.empty();
                
                validationErrors.forEach(function(error) {
                    $list.append('<li>' + error + '</li>');
                });
                
                $('#mappingValidationMessages').removeClass('d-none');
                
                // Scroll to validation messages
                $('html, body').animate({
                    scrollTop: $('#mappingValidationMessages').offset().top - 100
                }, 500);
            }
        }
        
        function executeTransfer() {
            abp.message.confirm(
                app.localize('AreYouSureYouWantToTransferUsers'),
                app.localize('ConfirmTransfer'),
                function(isConfirmed) {
                    if (isConfirmed) {
                        performTransfer();
                    }
                }
            );
        }

        function performTransfer() {
            // Filter mappings to only include selected categories
            var selectedCategoryIds = [];
            $('.category-checkbox:checked').each(function() {
                selectedCategoryIds.push($(this).data('category-id'));
            });
            
            var filteredCategoryMappings = {};
            var filteredNewCategories = {};
            var filteredSkippedCategories = [];
            
            // Filter category mappings
            Object.keys(_transferData.categoryMappings || {}).forEach(function(categoryId) {
                if (selectedCategoryIds.indexOf(categoryId) !== -1) {
                    filteredCategoryMappings[categoryId] = _transferData.categoryMappings[categoryId];
                }
            });
            
            // Filter new categories and check for department-level creation
            var hasDepartmentLevelCreation = false;
            Object.keys(_transferData.newCategories || {}).forEach(function(categoryId) {
                if (selectedCategoryIds.indexOf(categoryId) !== -1) {
                    // Find the category info to check hierarchy level
                    var categoryInfo = _transferData.analysisResult.requirementCategories.find(function(c) {
                        return c.categoryId === categoryId;
                    });
                    
                    if (categoryInfo && categoryInfo.hierarchyLevel === 'Department') {
                        hasDepartmentLevelCreation = true;
                        // Set confirmation flag if not already confirmed
                        if (!_transferData.newCategories[categoryId].confirmedDepartmentLevel) {
                            _transferData.newCategories[categoryId].confirmedDepartmentLevel = false;
                        }
                    }
                    
                    filteredNewCategories[categoryId] = _transferData.newCategories[categoryId];
                }
            });
            
            // Filter skipped categories
            (_transferData.skippedCategories || []).forEach(function(categoryId) {
                if (selectedCategoryIds.indexOf(categoryId) !== -1) {
                    filteredSkippedCategories.push(categoryId);
                }
            });
            
            // Check if we need to show department-level warning
            if (hasDepartmentLevelCreation && !_transferData.confirmedDepartmentLevelCreation) {
                var departmentLevelCategories = [];
                Object.keys(filteredNewCategories).forEach(function(categoryId) {
                    var categoryInfo = _transferData.analysisResult.requirementCategories.find(function(c) {
                        return c.categoryId === categoryId;
                    });
                    if (categoryInfo && categoryInfo.hierarchyLevel === 'Department') {
                        departmentLevelCategories.push({
                            categoryName: categoryInfo.categoryName,
                            requirementName: categoryInfo.requirementName,
                            newRequirementName: filteredNewCategories[categoryId].requirement
                        });
                    }
                });
                
                var warningMessage = app.localize('DepartmentLevelCreationWarning') + '<br><br>';
                warningMessage += '<strong>' + app.localize('TheFollowingRequirementsWillBeCreatedAtDepartmentLevel') + '</strong><br>';
                departmentLevelCategories.forEach(function(cat) {
                    warningMessage += '• ' + cat.newRequirementName + ' (' + app.localize('ReplacingCategory') + ': ' + cat.categoryName + ')<br>';
                });
                warningMessage += '<br>' + app.localize('ThisWillAffectAllCohortsInTargetDepartment');
                
                abp.message.confirm(
                    warningMessage,
                    app.localize('ConfirmDepartmentLevelCreation'),
                    function(isConfirmed) {
                        if (isConfirmed) {
                            _transferData.confirmedDepartmentLevelCreation = true;
                            // Set confirmation flag on each department-level category
                            Object.keys(filteredNewCategories).forEach(function(categoryId) {
                                var categoryInfo = _transferData.analysisResult.requirementCategories.find(function(c) {
                                    return c.categoryId === categoryId;
                                });
                                if (categoryInfo && categoryInfo.hierarchyLevel === 'Department') {
                                    filteredNewCategories[categoryId].confirmedDepartmentLevel = true;
                                }
                            });
                            // Retry the transfer
                            performTransfer();
                        }
                    }
                );
                return;
            }
            
            // Extract category IDs from noTransferRequiredCategories
            var noTransferCategoryIds = [];
            if (_transferData.analysisResult && _transferData.analysisResult.noTransferRequiredCategories) {
                noTransferCategoryIds = _transferData.analysisResult.noTransferRequiredCategories.map(function(cat) {
                    return cat.categoryId;
                });
            }
            
            var transferDto = {
                sourceCohortId: _cohortInfo.cohortId,
                targetCohortId: _transferData.targetCohortId,
                selectedCohortUserIds: _transferData.selectedUsers,
                categoryMappings: filteredCategoryMappings,
                newCategories: filteredNewCategories,
                skippedCategories: filteredSkippedCategories,
                noTransferRequiredCategoryIds: noTransferCategoryIds,
                confirmTransfer: true
            };
            
            abp.ui.setBusy(_modalManager.getModal());
            
            abp.services.app.userTransfer.transferSelectedUsers(transferDto)
                .done(function(result) {
                    if (result.success) {
                        abp.message.success(app.localize('TransferCompletedSuccessfully'));
                        abp.event.trigger('app.userTransferCompleted', result);
                        _modalManager.close();
                    } else {
                        abp.message.error(result.message || app.localize('TransferFailed'));
                    }
                })
                .fail(function(error) {
                    console.error('Transfer failed:', error);
                    abp.message.error(app.localize('TransferFailed'));
                })
                .always(function() {
                    abp.ui.clearBusy(_modalManager.getModal());
                });
        }
    };
})(jQuery);