(function ($) {
    app.modals.CohortMigrationWizardModal = function () {
        var _cohortMigrationService = abp.services.app.cohortMigration;
        
        // Debug: Check if service is available
        console.log('CohortMigration service available:', !!_cohortMigrationService);
        if (_cohortMigrationService) {
            console.log('Available methods:', Object.keys(_cohortMigrationService));
        } else {
            console.log('abp.services.app available services:', Object.keys(abp.services.app || {}));
        }
        
        var _modalManager;
        var _$migrationWizardForm = null;
        var _currentStep = 1;
        var _maxSteps = 3;
        var _migrationData = {};
        var _cohortInfo = {
            cohortId: null,
            cohortName: null
        };
        var _noMigrationRequiredCategories = [];

        function addValidationStyles() {
            // Add CSS styles for validation feedback
            if (!document.getElementById('migration-validation-styles')) {
                var style = document.createElement('style');
                style.id = 'migration-validation-styles';
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
                    /* Fixed Table Header Styles */
                    .fixed-table-header {
                        background: #f8f9fa;
                        border: 1px solid #dee2e6;
                        border-bottom: 2px solid #dee2e6;
                        border-radius: 0.375rem 0.375rem 0 0;
                        position: relative;
                        z-index: 20;
                    }
                    .header-row {
                        display: flex;
                        align-items: center;
                        min-height: 48px;
                    }
                    .header-cell {
                        padding: 0.75rem 0.5rem;
                        font-weight: 600;
                        color: #495057;
                        border-right: 1px solid #dee2e6;
                        display: flex;
                        align-items: center;
                        justify-content: center;
                        text-align: center;
                        font-size: 0.875rem;
                        flex-shrink: 0;
                        box-sizing: border-box;
                    }
                    .header-cell:last-child {
                        border-right: none;
                    }
                    .header-checkbox {
                        justify-content: center;
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
                    .header-category {
                        justify-content: flex-start;
                        text-align: left;
                    }
                    .header-requirement {
                        justify-content: flex-start;
                        text-align: left;
                    }
                    .header-impact {
                        justify-content: center;
                    }
                    .header-action {
                        justify-content: center;
                    }
                    .header-target {
                        justify-content: center;
                    }
                    .header-suggestions {
                        justify-content: center;
                    }
                    .header-status {
                        justify-content: center;
                    }
                    /* Enhanced select all checkbox styling */
                    #selectAllCategories {
                        transform: scale(1.2);
                        cursor: pointer;
                    }
                    .table-scroll-container {
                        position: relative;
                        background: white;
                        border-radius: 0 0 0.375rem 0.375rem;
                        border-top: none;
                    }
                    .table-scroll-container::-webkit-scrollbar {
                        width: 8px;
                    }
                    .table-scroll-container::-webkit-scrollbar-track {
                        background: #f1f1f1;
                        border-radius: 4px;
                    }
                    .table-scroll-container::-webkit-scrollbar-thumb {
                        background: #c1c1c1;
                        border-radius: 4px;
                    }
                    .table-scroll-container::-webkit-scrollbar-thumb:hover {
                        background: #a8a8a8;
                    }
                    .table-scroll-container .table {
                        margin-bottom: 0;
                        border-top: none;
                    }
                    .table-scroll-container .table thead {
                        display: none; /* Hide the original table header */
                    }
                    .table-scroll-container .table tbody td {
                        vertical-align: middle;
                        padding: 0.75rem 0.5rem;
                        border-top: 1px solid #dee2e6;
                    }
                    .table-scroll-container .table tbody tr:first-child td {
                        border-top: none;
                    }
                    /* Status badge styles */
                    .status-badge {
                        font-size: 0.75rem;
                        padding: 0.25rem 0.5rem;
                        border-radius: 0.375rem;
                    }
                    .status-pending {
                        background-color: #ffeaa7;
                        color: #856404;
                    }
                    .status-mapped {
                        background-color: #d4edda;
                        color: #155724;
                    }
                    .status-copied {
                        background-color: #d1ecf1;
                        color: #0c5460;
                    }
                    .status-skipped {
                        background-color: #f8d7da;
                        color: #721c24;
                    }
                `;
                document.head.appendChild(style);
            }
        }

        function loadModelDataFromHiddenInputs() {
            var modal = _modalManager.getModal();
            
            // Extract model data from hidden inputs
            var cohortId = modal.find('input[name="CohortMigrationWizardViewModel_CohortId"]').val();
            var cohortName = modal.find('input[name="CohortMigrationWizardViewModel_CohortName"]').val();
            var sourceDepartmentName = modal.find('input[name="CohortMigrationWizardViewModel_SourceDepartmentName"]').val();
            var departmentOption = modal.find('input[name="CohortMigrationWizardViewModel_DepartmentOption"]').val();
            var targetDepartmentId = modal.find('input[name="CohortMigrationWizardViewModel_TargetDepartmentId"]').val();
            var newDepartmentName = modal.find('input[name="CohortMigrationWizardViewModel_NewDepartmentName"]').val();
            var newDepartmentDescription = modal.find('input[name="CohortMigrationWizardViewModel_NewDepartmentDescription"]').val();
            var currentStep = modal.find('input[name="CohortMigrationWizardViewModel_CurrentStep"]').val();
            
            // Store in cohort info object
            if (cohortId) {
                _cohortInfo.cohortId = cohortId;
                _migrationData.cohortId = cohortId;
            }
            
            if (cohortName) {
                _cohortInfo.cohortName = cohortName;
                _migrationData.cohortName = cohortName;
            }
            
            // Store other model data in migration data
            if (sourceDepartmentName) {
                _migrationData.sourceDepartmentName = sourceDepartmentName;
            }
            
            if (departmentOption) {
                _migrationData.departmentOption = departmentOption;
            }
            
            if (targetDepartmentId) {
                _migrationData.targetDepartmentId = targetDepartmentId;
            }
            
            if (newDepartmentName) {
                _migrationData.newDepartmentName = newDepartmentName;
            }
            
            if (newDepartmentDescription) {
                _migrationData.newDepartmentDescription = newDepartmentDescription;
            }
            
            if (currentStep) {
                _currentStep = parseInt(currentStep) || 1;
            }
            
            console.log('Model data loaded from hidden inputs:', {
                cohortInfo: _cohortInfo,
                migrationData: _migrationData,
                currentStep: _currentStep
            });
        }

        this.init = function (modalManager) {
            _modalManager = modalManager;
            var modal = _modalManager.getModal();
            _$migrationWizardForm = modal.find('form[name=MigrationWizardForm]');
            
            // Add validation styles
            addValidationStyles();
            
            // Automatically extract and set model data from hidden inputs
            loadModelDataFromHiddenInputs();
            
            // Initialize form validation
            _$migrationWizardForm.validate({
                rules: {
                    targetDepartmentId: {
                        required: function() {
                            return $('input[name="departmentOption"]:checked').val() === 'existing';
                        }
                    },
                    newDepartmentName: {
                        required: function() {
                            return $('input[name="departmentOption"]:checked').val() === 'new';
                        },
                        minlength: 2,
                        maxlength: 255
                    }
                },
                messages: {
                    targetDepartmentId: {
                        required: app.localize('PleaseSelectDepartment')
                    },
                    newDepartmentName: {
                        required: app.localize('DepartmentNameRequired'),
                        minlength: app.localize('DepartmentNameTooShort'),
                        maxlength: app.localize('DepartmentNameTooLong')
                    }
                }
            });

            // Initialize event handlers
            initializeEventHandlers();
            
            // Initialize current step
            updateStepDisplay();
        };

        this.setCohortInfo = function(cohortId, cohortName) {
            _cohortInfo.cohortId = cohortId;
            _cohortInfo.cohortName = cohortName;
            
            // Store in migration data as well for consistency
            _migrationData.cohortId = cohortId;
            _migrationData.cohortName = cohortName;
            
            console.log('Cohort info set:', _cohortInfo);
        };

        function initializeEventHandlers() {
            var modal = _modalManager.getModal();

            // Department option change handler
            modal.find('input[name="departmentOption"]').on('change', function() {
                var selectedOption = $(this).val();
                toggleDepartmentSections(selectedOption);
                clearMigrationAnalysis();
            });

            // Target department selection change handler
            modal.find('#targetDepartmentId').on('change', function() {
                var selectedDepartmentId = $(this).val();
                if (selectedDepartmentId) {
                    showDepartmentStatistics(selectedDepartmentId);
                    performMigrationAnalysis();
                } else {
                    hideDepartmentStatistics();
                    clearMigrationAnalysis();
                }
            });

            // New department name change handler
            modal.find('#newDepartmentName').on('input', function() {
                if ($(this).val().trim()) {
                    performMigrationAnalysis();
                } else {
                    clearMigrationAnalysis();
                }
            });

            // Wizard navigation handlers
            modal.find('#nextStepBtn').on('click', function() {
                nextStep();
            });

            modal.find('#prevStepBtn').on('click', function() {
                previousStep();
            });

            modal.find('#executeBtn').on('click', function() {
                executeMigration();
            });
        }

        function toggleDepartmentSections(selectedOption) {
            var modal = _modalManager.getModal();
            
            if (selectedOption === 'existing') {
                modal.find('#existingDepartmentSection').removeClass('d-none');
                modal.find('#newDepartmentSection').addClass('d-none');
            } else {
                modal.find('#existingDepartmentSection').addClass('d-none');
                modal.find('#newDepartmentSection').removeClass('d-none');
                hideDepartmentStatistics();
            }
        }

        function showDepartmentStatistics(departmentId) {
            var modal = _modalManager.getModal();
            var statsContainer = modal.find('#departmentStats');
            
            // Find department data from available departments
            var departmentData = findDepartmentById(departmentId);
            if (departmentData) {
                modal.find('#statsCohortsCount').text(departmentData.CohortsCount || 0);
                modal.find('#statsUsersCount').text(departmentData.UsersCount || 0);
                modal.find('#statsRequirementsCount').text(departmentData.RequirementsCount || 0);
                
                statsContainer.removeClass('d-none');
            }
        }

        function hideDepartmentStatistics() {
            var modal = _modalManager.getModal();
            modal.find('#departmentStats').addClass('d-none');
        }

        function findDepartmentById(departmentId) {
            // This would typically come from the view model or cached data
            // For now, we'll use the cached department data if available
            if (_migrationData.availableDepartments) {
                return _migrationData.availableDepartments.find(d => d.DepartmentId === departmentId);
            }
            
            // If no cached data, we'll need to load it via the service
            // This should be done asynchronously in a proper implementation
            return null;
        }

        function performMigrationAnalysis() {
            var modal = _modalManager.getModal();
            var cohortId = modal.find('input[name="cohortId"]').val();
            var departmentOption = modal.find('input[name="departmentOption"]:checked').val();
            var targetDepartmentId = null;

            if (departmentOption === 'existing') {
                targetDepartmentId = modal.find('#targetDepartmentId').val();
                if (!targetDepartmentId) {
                    return;
                }
            } else {
                var newDepartmentName = modal.find('#newDepartmentName').val().trim();
                if (!newDepartmentName) {
                    return;
                }
            }

            // Show loading state
            showAnalysisLoading();

            // Perform analysis using ABP service proxy
            _cohortMigrationService.analyzeCohortMigration(cohortId, targetDepartmentId)
                .done(function(result) {
                    console.log('Migration analysis completed:', result);
                    console.log('Requirement categories count:', result.requirementCategories ? result.requirementCategories.length : 'undefined');
                    console.log('No migration required categories count:', result.noMigrationRequiredCategories ? result.noMigrationRequiredCategories.length : 'undefined');
                    
                    // Store no migration required categories separately
                    _noMigrationRequiredCategories = result.noMigrationRequiredCategories || [];
                    
                    displayMigrationAnalysis(result);
                    _migrationData.analysis = result;
                })
                .fail(function(error) {
                    console.error('Migration analysis failed:', error);
                    hideAnalysisLoading();
                    abp.message.error(error.message || app.localize('MigrationAnalysisFailed'));
                });
        }

        function showAnalysisLoading() {
            var modal = _modalManager.getModal();
            var analysisContainer = modal.find('#migrationAnalysis');
            var contentContainer = modal.find('#analysisContent');
            
            contentContainer.html('<div class="text-center"><span class="loading-spinner"></span> ' + app.localize('AnalyzingMigration') + '...</div>');
            analysisContainer.removeClass('d-none');
        }

        function hideAnalysisLoading() {
            var modal = _modalManager.getModal();
            modal.find('#migrationAnalysis').addClass('d-none');
        }

        function displayMigrationAnalysis(analysis) {
            var modal = _modalManager.getModal();
            var contentContainer = modal.find('#analysisContent');
            
            // Debug: Log the analysis object to see its structure
            console.log('Analysis object received:', analysis);
            console.log('Analysis type:', typeof analysis);
            console.log('Analysis keys:', Object.keys(analysis));
            console.log('totalUsersCount value:', analysis.totalUsersCount);
            console.log('requirementCategories value:', analysis.requirementCategories);
            console.log('requirementCategories length:', analysis.requirementCategories ? analysis.requirementCategories.length : 'undefined');
            
            // Safe access to properties with defaults (using camelCase from JSON response)
            var migrationComplexity = analysis.migrationComplexity || 'Unknown';
            var complexityClass = 'complexity-' + migrationComplexity.toLowerCase();
            var totalUsersCount = analysis.totalUsersCount || 0;
            var requirementCategories = analysis.requirementCategories || [];
            var estimatedDurationMinutes = analysis.estimatedDurationMinutes || 0;
            var warnings = analysis.warnings || [];
            
            // Debug: Log the extracted values
            console.log('Extracted totalUsersCount:', totalUsersCount);
            console.log('Extracted requirementCategories length:', requirementCategories.length);
            
            // Calculate hierarchy breakdown
            var hierarchyBreakdown = {
                'Tenant': 0,
                'Department': 0,
                'Cohort': 0,
                'CohortAndDepartment': 0
            };
            requirementCategories.forEach(function(cat) {
                var level = cat.hierarchyLevel || 'Unknown';
                if (hierarchyBreakdown.hasOwnProperty(level)) {
                    hierarchyBreakdown[level]++;
                }
            });
            
            var html = '<div class="analysis-summary">';
            html += '<div class="analysis-item">';
            html += '<span class="analysis-label">' + app.localize('TotalUsers') + ':</span>';
            html += '<span class="analysis-value">' + totalUsersCount + '</span>';
            html += '</div>';
            
            html += '<div class="analysis-item">';
            html += '<span class="analysis-label">' + app.localize('RequirementCategories') + ':</span>';
            html += '<span class="analysis-value">' + requirementCategories.length + '</span>';
            if (_noMigrationRequiredCategories.length > 0) {
                html += ' <span class="text-muted">(' + _noMigrationRequiredCategories.length + ' ' + app.localize('NoMigrationRequired') + ')</span>';
            }
            html += '</div>';
            
            // Show hierarchy breakdown if we have categories
            if (requirementCategories.length > 0) {
                html += '<div class="analysis-item">';
                html += '<span class="analysis-label">' + app.localize('HierarchyBreakdown') + ':</span>';
                html += '<div class="hierarchy-breakdown">';
                
                if (hierarchyBreakdown.Tenant > 0) {
                    html += '<span class="badge badge-primary badge-sm mx-1"><i class="fa fa-building"></i> ' + 
                            app.localize('Tenant') + ': ' + hierarchyBreakdown.Tenant + '</span>';
                }
                if (hierarchyBreakdown.Department > 0) {
                    html += '<span class="badge badge-info badge-sm mx-1"><i class="fa fa-sitemap"></i> ' + 
                            app.localize('Department') + ': ' + hierarchyBreakdown.Department + '</span>';
                }
                if (hierarchyBreakdown.Cohort > 0) {
                    html += '<span class="badge badge-success badge-sm mx-1"><i class="fa fa-users"></i> ' + 
                            app.localize('Cohort') + ': ' + hierarchyBreakdown.Cohort + '</span>';
                }
                if (hierarchyBreakdown.CohortAndDepartment > 0) {
                    html += '<span class="badge badge-warning badge-sm mx-1"><i class="fa fa-users"></i><i class="fa fa-sitemap"></i> ' + 
                            app.localize('Combined') + ': ' + hierarchyBreakdown.CohortAndDepartment + '</span>';
                }
                
                html += '</div>';
                html += '</div>';
            }
            
            html += '<div class="analysis-item">';
            html += '<span class="analysis-label">' + app.localize('MigrationComplexity') + ':</span>';
            html += '<span class="analysis-value ' + complexityClass + '">' + migrationComplexity + '</span>';
            html += '</div>';
            
            html += '<div class="analysis-item d-none">';
            html += '<span class="analysis-label">' + app.localize('EstimatedDuration') + ':</span>';
            html += '<span class="analysis-value">' + estimatedDurationMinutes + ' ' + app.localize('Minutes') + '</span>';
            html += '</div>';
            
            if (warnings.length > 0) {
                html += '<div class="mt-3 d-none">';
                html += '<h6 class="text-warning">' + app.localize('Warnings') + ':</h6>';
                html += '<ul class="mb-0">';
                warnings.forEach(function(warning) {
                    html += '<li class="text-warning">' + warning + '</li>';
                });
                html += '</ul>';
                html += '</div>';
            }
            
            html += '</div>';
            
            contentContainer.html(html);
        }

        function clearMigrationAnalysis() {
            var modal = _modalManager.getModal();
            modal.find('#migrationAnalysis').addClass('d-none');
            _migrationData.analysis = null;
        }

        function nextStep() {
            if (_currentStep < _maxSteps) {
                if (validateCurrentStep()) {
                    _currentStep++;
                    updateStepDisplay();
                }
            }
        }

        function previousStep() {
            if (_currentStep > 1) {
                _currentStep--;
                updateStepDisplay();
            }
        }

        function validateCurrentStep() {
            switch (_currentStep) {
                case 1:
                    return validateStep1();
                case 2:
                    return validateStep2();
                case 3:
                    return validateStep3();
                default:
                    return true;
            }
        }

        function validateStep1() {
            if (!_$migrationWizardForm.valid()) {
                return false;
            }

            var departmentOption = _$migrationWizardForm.find('input[name="departmentOption"]:checked').val();
            
            if (departmentOption === 'existing') {
                var targetDepartmentId = _$migrationWizardForm.find('#targetDepartmentId').val();
                if (!targetDepartmentId) {
                    abp.message.error(app.localize('PleaseSelectDepartment'));
                    return false;
                }
            } else {
                var newDepartmentName = _$migrationWizardForm.find('#newDepartmentName').val().trim();
                if (!newDepartmentName) {
                    abp.message.error(app.localize('DepartmentNameRequired'));
                    return false;
                }
            }

            if (!_migrationData.analysis) {
                abp.message.error(app.localize('PleaseWaitForAnalysis'));
                return false;
            }

            if (!_migrationData.analysis.canMigrate) {
                abp.message.error(app.localize('MigrationNotPossible'));
                return false;
            }

            return true;
        }

        function validateStep2() {
            // Check if we have any categories at all (either to map or no migration required)
            var hasCategoriesToMap = _categoryData && _categoryData.length > 0;
            var hasNoMigrationRequired = _noMigrationRequiredCategories && _noMigrationRequiredCategories.length > 0;
            
            if (!hasCategoriesToMap && !hasNoMigrationRequired) {
                abp.message.error(app.localize('NoCategoriesFoundForMigration'));
                return false;
            }
            
            // If we only have "no migration required" categories, that's valid - we can proceed
            if (!hasCategoriesToMap && hasNoMigrationRequired) {
                // No mapping decisions needed, just proceed
                _migrationData.categoryMappings = [];
                _migrationData.mappingValidation = { isValid: true, errorCount: 0, warningCount: 0, hasWarnings: false };
                _migrationData.impactSummary = {
                    totalCategories: 0,
                    mappedCategories: 0,
                    copiedCategories: 0,
                    skippedCategories: 0,
                    totalAffectedUsers: 0,
                    totalAffectedRecords: 0,
                    dataLossWarnings: 0,
                    newRequirementsCreated: 0,
                    newCategoriesCreated: 0
                };
                return true;
            }

            // Get only selected categories for validation
            var modal = _modalManager.getModal();
            var selectedCategoryIds = [];
            modal.find('.category-checkbox:checked').each(function() {
                selectedCategoryIds.push($(this).data('category-id'));
            });

            if (selectedCategoryIds.length === 0) {
                abp.message.error('Please select at least one category to migrate.');
                return false;
            }

            // Filter mapping decisions to only include selected categories
            var allMappingDecisions = getCategoryMappingDecisions();
            var selectedMappingDecisions = allMappingDecisions.filter(function(mapping) {
                return selectedCategoryIds.indexOf(mapping.SourceCategoryId) !== -1;
            });

            // Run comprehensive validation on selected categories only
            var validationResult = validateSelectedMappings(selectedMappingDecisions);
            
            if (!validationResult.isValid) {
                abp.message.error('Please fix validation errors for selected categories (' + validationResult.errorCount + ' errors found).');
                return false;
            }

            // Show warnings if any exist
            if (validationResult.hasWarnings) {
                abp.message.warn('Validation warnings exist for selected categories (' + validationResult.warningCount + ' warnings).');
            }

            // Store mapping decisions in migration data for next step (only selected categories)
            _migrationData.categoryMappings = selectedMappingDecisions;
            _migrationData.mappingValidation = validationResult;

            // Calculate migration impact summary
            var impactSummary = calculateMigrationImpact(selectedMappingDecisions);
            _migrationData.impactSummary = impactSummary;

            return true;
        }

        function calculateMigrationImpact(mappingDecisions) {
            var impact = {
                totalCategories: mappingDecisions.length,
                mappedCategories: 0,
                copiedCategories: 0,
                skippedCategories: 0,
                totalAffectedUsers: 0,
                totalAffectedRecords: 0,
                dataLossWarnings: 0,
                newRequirementsCreated: 0,
                newCategoriesCreated: 0
            };

            mappingDecisions.forEach(function(mapping) {
                impact.totalAffectedUsers += mapping.AffectedUsersCount || 0;
                impact.totalAffectedRecords += mapping.AffectedRecordStatesCount || 0;

                switch (mapping.Action) {
                    case 'map':
                        impact.mappedCategories++;
                        break;
                    case 'copy':
                        impact.copiedCategories++;
                        if (mapping.NewRequirementName) impact.newRequirementsCreated++;
                        if (mapping.NewCategoryName) impact.newCategoriesCreated++;
                        break;
                    case 'skip':
                        impact.skippedCategories++;
                        if (mapping.AffectedRecordStatesCount > 0) {
                            impact.dataLossWarnings++;
                        }
                        break;
                }
            });

            return impact;
        }

        function validateStep3() {
            var modal = _modalManager.getModal();
            
            // Check if confirmation checkbox is checked
            var confirmationChecked = modal.find('#confirmMigration').is(':checked');
            if (!confirmationChecked) {
                abp.message.error(app.localize('PleaseConfirmMigration'));
                return false;
            }

            // Ensure all previous steps are valid
            if (!_migrationData.analysis) {
                abp.message.error(app.localize('MigrationAnalysisRequired'));
                return false;
            }

            if (!_migrationData.categoryMappings || _migrationData.categoryMappings.length === 0) {
                abp.message.error(app.localize('CategoryMappingsRequired'));
                return false;
            }

            // Prepare final migration request data
            _migrationData.finalConfirmation = {
                confirmedAt: new Date().toISOString(),
                userConfirmed: true,
                impactAcknowledged: true
            };

            return true;
        }

        // Category Mapping Functions
        var _categoryMappingTable = null;
        var _categoryData = [];

        function initializeCategoryMapping() {
            console.log('Initializing category mapping for step 2');
            console.log('Current step:', _currentStep);
            console.log('Migration data:', _migrationData);
            console.log('Analysis data exists:', !!_migrationData.analysis);
            
            if (_currentStep === 2 && _migrationData.analysis) {
                loadCategoryMappingData();
                initializeCategoryMappingTable();
                initializeCategoryMappingEventHandlers();
            } else {
                console.error('Cannot initialize category mapping - missing requirements:', {
                    currentStep: _currentStep,
                    hasAnalysis: !!_migrationData.analysis
                });
            }
        }

        function loadCategoryMappingData() {
            var modal = _modalManager.getModal();
            
            // Show loading state
            modal.find('#categoryMappingTable').hide();
            modal.find('#categoryMappingLoading').show();
            
            // Update migration summary
            updateMigrationSummary();
            
            // Check if analysis data exists
            if (!_migrationData.analysis) {
                console.error('Migration analysis data is missing');
                modal.find('#categoryMappingLoading').html(
                    '<div class="text-center text-danger">' +
                    '<i class="fa fa-exclamation-triangle"></i> ' +
                    app.localize('MigrationAnalysisDataMissing') +
                    '</div>'
                );
                return;
            }
            
            // Check if requirementCategories exists
            if (!_migrationData.analysis.requirementCategories) {
                console.error('Requirement categories data is missing from analysis');
                modal.find('#categoryMappingLoading').html(
                    '<div class="text-center text-danger">' +
                    '<i class="fa fa-exclamation-triangle"></i> ' +
                    app.localize('RequirementCategoriesNotFound') +
                    '</div>'
                );
                return;
            }
            
            // Check if requirementCategories array is empty
            if (_migrationData.analysis.requirementCategories.length === 0) {
                console.warn('No requirement categories need mapping for this cohort');
                
                // Check if we have no migration required categories to display
                if (!_migrationData.analysis.noMigrationRequiredCategories || 
                    _migrationData.analysis.noMigrationRequiredCategories.length === 0) {
                    // Only show the "no requirements" message if both arrays are empty
                    modal.find('#categoryMappingLoading').html(
                        '<div class="text-center text-warning">' +
                        '<i class="fa fa-info-circle"></i> ' +
                        '<p class="mb-2">' + app.localize('NoRequirementCategoriesFound') + '</p>' +
                        '<p class="text-muted small">' + app.localize('NoRequirementCategoriesExplanation', 
                            'This cohort has no requirement categories associated with its department or directly assigned to it. ' +
                            'You may need to assign requirements to the department or cohort first.') + '</p>' +
                        '</div>'
                    );
                    
                    // Hide the table and show empty state
                    modal.find('#categoryMappingTable').hide();
                    return;
                }
                
                // We have no categories to map, but we do have categories that don't require migration
                // Continue to show those
                _categoryData = [];
                
                // Make sure the no migration required categories are set
                _noMigrationRequiredCategories = _migrationData.analysis.noMigrationRequiredCategories || [];
                
                showCategoryMappingTable();
                return;
            }
            
            // Load category data from analysis
            _categoryData = _migrationData.analysis.requirementCategories.map(function(category) {
                return {
                    CategoryId: category.categoryId,
                    CategoryName: category.categoryName,
                    RequirementId: category.requirementId,
                    RequirementName: category.requirementName,
                    IsDepartmentSpecific: category.isDepartmentSpecific,
                    IsCohortSpecific: category.isCohortSpecific || false,
                    HierarchyLevel: category.hierarchyLevel || 'Unknown',
                    RecordStatesCount: category.recordStatesCount,
                    AffectedUsersCount: category.affectedUsersCount,
                    RequiresMapping: category.requiresMapping,
                    TargetOptions: [],
                    MappingAction: 'map', // Default action
                    TargetCategoryId: null,
                    NewRequirementName: '',
                    NewCategoryName: '',
                    Status: 'pending'
                };
            });
            
            // Load all target category options upfront
            loadAllTargetCategoryOptions();
        }

        function loadAllTargetCategoryOptions() {
            var targetDepartmentId = getTargetDepartmentId();
            if (!targetDepartmentId) {
                console.log('No target department ID found, showing table without options');
                showCategoryMappingTable();
                return;
            }

            console.log('Loading target category options for all categories, target department:', targetDepartmentId);

            var totalCategories = _categoryData.length;
            var processedCategories = 0;
            var modal = _modalManager.getModal();

            // Update loading text to show progress
            modal.find('#categoryMappingLoading').html(
                '<div class="text-center">' +
                '<div class="spinner-border" role="status"></div>' +
                '<div class="mt-2">' + app.localize('LoadingTargetCategoryOptions') + '</div>' +
                '<div class="text-muted">' + app.localize('ProcessingCategories', 0, totalCategories) + '</div>' +
                '</div>'
            );

            // Load options for each category
            _categoryData.forEach(function(category, index) {
                setTimeout(function() {
                    loadTargetCategoryOptionsForCategory(category, targetDepartmentId, function() {
                        processedCategories++;
                        
                        // Update progress
                        modal.find('#categoryMappingLoading .text-muted').text(
                            app.localize('ProcessingCategories', processedCategories, totalCategories)
                        );
                        
                        // Show table when all categories are processed
                        if (processedCategories === totalCategories) {
                            showCategoryMappingTable();
                        }
                    });
                }, index * 100); // 100ms delay between each request to avoid overwhelming the server
            });
        }

        function loadTargetCategoryOptionsForCategory(category, targetDepartmentId, callback) {
            console.log('Loading options for category:', category.CategoryName, 'ID:', category.CategoryId);

            _cohortMigrationService.getTargetCategoryOptions({
                sourceCategoryId: category.CategoryId,
                targetDepartmentId: targetDepartmentId,
                maxResults: 20
            })
            .done(function(result) {
                console.log('Target category options received for', category.CategoryName, ':', result);
                console.log('Result type:', typeof result, 'Length:', result ? result.length : 'null/undefined');
                
                // Store the options in the category data
                category.TargetOptions = result || [];
                
                // Sort options by match score descending
                category.TargetOptions.sort(function(a, b) { 
                    var scoreA = a.matchScore || a.MatchScore || 0;
                    var scoreB = b.matchScore || b.MatchScore || 0;
                    return scoreB - scoreA; 
                });
                
                console.log('Stored TargetOptions for', category.CategoryName, ':', category.TargetOptions);
                callback();
            })
            .fail(function(error) {
                console.error('Failed to load target category options for', category.CategoryName, ':', error);
                category.TargetOptions = [];
                callback();
            });
        }

        function showCategoryMappingTable() {
            var modal = _modalManager.getModal();
            
            console.log('Showing category mapping table. Category data summary:');
            _categoryData.forEach(function(category, index) {
                console.log('Category', index, ':', {
                    name: category.CategoryName,
                    id: category.CategoryId,
                    targetOptionsCount: category.TargetOptions ? category.TargetOptions.length : 'undefined',
                    mappingAction: category.MappingAction
                });
            });
            
            // Hide loading state
            modal.find('#categoryMappingLoading').hide();
            
            // Always show the no migration required section first
            displayNoMigrationRequiredCategories();
            
            // Only show the mapping table and header if there are categories to map
            if (_categoryData && _categoryData.length > 0) {
                // Create fixed header outside scroll container
                createFixedTableHeader();
                
                // Show table
                modal.find('#categoryMappingTable').show();
                
                // After table is shown, populate all dropdowns for 'map' actions
                setTimeout(function() {
                    populateAllMapDropdowns();
                    updateHeaderVisibility(); // Initialize header visibility based on current state
                }, 100); // Small delay to ensure DataTable is fully rendered
            } else {
                // Hide the mapping table and bulk actions if no categories need mapping
                modal.find('#categoryMappingTable').hide();
                modal.find('.btn-group').parent().parent().hide(); // Hide bulk actions row
            }
        }

        function createFixedTableHeader() {
            var modal = _modalManager.getModal();
            var $tableContainer = modal.find('#categoryMappingTable').parent();
            
            // Remove existing fixed header if it exists
            modal.find('#fixedTableHeader').remove();
            
            // Create fixed header HTML
            var headerHtml = '<div id="fixedTableHeader" class="fixed-table-header">' +
                '<div class="header-row">' +
                '<div class="header-cell header-checkbox">' +
                '<div class="form-check">' +
                '<input class="form-check-input" type="checkbox" id="selectAllCategories">' +
                '</div>' +
                '</div>' +
                '<div class="header-cell header-category">' + app.localize('SourceCategory') + '</div>' +
                '<div class="header-cell header-requirement">' + app.localize('Requirement') + '</div>' +
                '<div class="header-cell header-impact">' + app.localize('Impact') + '</div>' +
                '<div class="header-cell header-action">' + app.localize('MappingAction') + '</div>' +
                '<div class="header-cell header-target">' + app.localize('TargetMapping') + '</div>' +
                '<div class="header-cell header-suggestions">' + app.localize('Suggestions') + '</div>' +
                '<div class="header-cell header-status">' + app.localize('Status') + '</div>' +
                '</div>' +
                '</div>';
            
            // Insert fixed header before the table container
            $tableContainer.before(headerHtml);
            
            // Initialize select all functionality
            setTimeout(function() {
                initializeSelectAllFunctionality();
                syncHeaderWithTableColumns();
                setupHeaderSyncHandlers();
            }, 100);
        }

        function syncHeaderWithTableColumns() {
            var modal = _modalManager.getModal();
            var $table = modal.find('#categoryMappingTable');
            var $fixedHeader = modal.find('#fixedTableHeader');
            
            if ($table.length && $fixedHeader.length && $table.is(':visible')) {
                // First update header visibility based on row selections
                updateHeaderVisibility();
                
                // Wait for the table to be fully rendered
                setTimeout(function() {
                    var $firstRow = $table.find('tbody tr:first-child');
                    if ($firstRow.length && $firstRow.is(':visible')) {
                        var $headerCells = $fixedHeader.find('.header-cell:visible');
                        var $tableCells = $firstRow.find('td');
                        
                        // Filter table cells to only visible ones by checking if their content is visible
                        var visibleTableCells = [];
                        $tableCells.each(function(index) {
                            var $cell = $(this);
                            var isVisible = true;
                            
                            // Check if this is a mapping action cell and if it's hidden
                            if (index === 4) { // Mapping Action column
                                isVisible = $cell.find('.mapping-action-container:visible').length > 0;
                            }
                            // Check if this is a target mapping cell and if it's hidden
                            else if (index === 5) { // Target Mapping column
                                isVisible = $cell.find('.target-mapping-section:visible').length > 0;
                            }
                            
                            if (isVisible) {
                                visibleTableCells.push($cell);
                            }
                        });
                        
                        if (visibleTableCells.length === $headerCells.length) {
                            // Reset header row to flex layout to get natural widths if needed
                            $fixedHeader.find('.header-row').css('display', 'flex');
                            
                            // Sync each visible header cell width with corresponding visible table cell
                            visibleTableCells.forEach(function(cell, index) {
                                var cellWidth = $(cell).outerWidth();
                                var $headerCell = $headerCells.eq(index);
                                
                                $headerCell.css({
                                    'width': cellWidth + 'px',
                                    'min-width': cellWidth + 'px',
                                    'max-width': cellWidth + 'px',
                                    'flex': 'none'
                                });
                                
                                console.log('Column ' + index + ' synced: ' + cellWidth + 'px');
                            });
                            
                            console.log('Header columns synchronized with visible table columns');
                        } else {
                            console.warn('Header and visible table column count mismatch:', $headerCells.length, 'vs', visibleTableCells.length);
                        }
                    } else {
                        console.log('Table not ready for header sync, retrying...');
                        // Retry after a longer delay if table isn't ready
                        setTimeout(syncHeaderWithTableColumns, 500);
                    }
                }, 200); // Additional delay to ensure DataTable rendering is complete
            }
        }

        function updateHeaderVisibility() {
            var modal = _modalManager.getModal();
            var $fixedHeader = modal.find('#fixedTableHeader');
            var $table = modal.find('#categoryMappingTable');
            
            if ($fixedHeader.length && $table.length) {
                // Check if any mapping action containers are visible
                var anyMappingActionsVisible = $table.find('.mapping-action-container:visible').length > 0;
                var $mappingActionHeader = $fixedHeader.find('.header-action');
                
                if (anyMappingActionsVisible) {
                    $mappingActionHeader.show();
                } else {
                    $mappingActionHeader.hide();
                }
                
                // Check if any target mapping sections are visible
                var anyTargetMappingsVisible = $table.find('.target-mapping-section:visible').length > 0;
                var $targetMappingHeader = $fixedHeader.find('.header-target');
                
                if (anyTargetMappingsVisible) {
                    $targetMappingHeader.show();
                } else {
                    $targetMappingHeader.hide();
                }
                
                console.log('Header visibility updated - Actions:', anyMappingActionsVisible, 'Targets:', anyTargetMappingsVisible);
            }
        }

        function setupHeaderSyncHandlers() {
            var modal = _modalManager.getModal();
            
            // Sync headers when modal is shown or resized
            $(window).off('resize.categoryHeader').on('resize.categoryHeader', function() {
                syncHeaderWithTableColumns();
            });
            
            // Sync when DataTable is redrawn
            if (_categoryMappingTable) {
                _categoryMappingTable.off('draw.headerSync').on('draw.headerSync', function() {
                    syncHeaderWithTableColumns();
                });
            }
            
            // Sync when modal becomes visible
            modal.off('shown.bs.modal.headerSync').on('shown.bs.modal.headerSync', function() {
                syncHeaderWithTableColumns();
            });
        }

        function populateAllMapDropdowns() {
            console.log('Populating all map dropdowns after table render');
            var modal = _modalManager.getModal();
            
            // Find all rows with map action and populate their dropdowns, but only for selected rows
            modal.find('.mapping-action-select').each(function() {
                var $actionSelect = $(this);
                var categoryId = $actionSelect.data('category-id');
                var selectedAction = $actionSelect.val();
                var $row = $actionSelect.closest('tr');
                var $checkbox = $row.find('.category-checkbox');
                var isRowSelected = $checkbox.is(':checked');
                
                if (selectedAction === 'map' && isRowSelected) {
                    var $targetSelect = $row.find('.target-category-select');
                    console.log('Populating dropdown for category ID:', categoryId, 'in map action');
                    populateTargetCategorySelectFromPreloaded(categoryId, $targetSelect);
                }
            });
        }

        function initializeCategoryMappingTable() {
            var modal = _modalManager.getModal();
            
            if (_categoryMappingTable) {
                _categoryMappingTable.destroy();
            }
            
            // Wrap the table in a scrolling container if not already done
            var $table = modal.find('#categoryMappingTable');
            if (!$table.parent().hasClass('table-scroll-container')) {
                $table.wrap('<div class="table-scroll-container" style="max-height: 400px; overflow-y: auto; border: 1px solid #dee2e6; border-radius: 0.375rem;"></div>');
            }
            
            _categoryMappingTable = $table.DataTable({
                data: _categoryData,
                destroy: true, // Allow reinitialization
                columns: [
                    {
                        data: null,
                        orderable: false,
                        width: "5%",
                        className: "text-center",
                        render: function(data, type, row, meta) {
                            return '<div class="form-check"><input class="form-check-input category-checkbox" type="checkbox" data-category-id="' + row.CategoryId + '"></div>';
                        }
                    },
                    {
                        data: null,
                        width: "20%",
                        render: function(data, type, row) {
                            var html = '<div class="category-info">';
                            html += '<div class="category-name">' + row.CategoryName + '</div>';
                            html += '</div>';
                            return html;
                        }
                    },
                    {
                        data: null,
                        width: "15%",
                        render: function(data, type, row) {
                            var html = '<div class="requirement-info">';
                            html += '<div class="requirement-name">' + row.RequirementName + '</div>';
                            
                            // Show hierarchy level badge
                            var hierarchyClass = 'badge badge-sm ';
                            var hierarchyIcon = '';
                            switch(row.HierarchyLevel) {
                                case 'Tenant':
                                    hierarchyClass += 'badge-primary';
                                    hierarchyIcon = '<i class="fa fa-building"></i> ';
                                    break;
                                case 'Department':
                                    hierarchyClass += 'badge-info';
                                    hierarchyIcon = '<i class="fa fa-sitemap"></i> ';
                                    break;
                                case 'Cohort':
                                    hierarchyClass += 'badge-success';
                                    hierarchyIcon = '<i class="fa fa-users"></i> ';
                                    break;
                                case 'CohortAndDepartment':
                                    hierarchyClass += 'badge-warning';
                                    hierarchyIcon = '<i class="fa fa-users"></i><i class="fa fa-sitemap"></i> ';
                                    break;
                                default:
                                    hierarchyClass += 'badge-secondary';
                            }
                            html += '<span class="' + hierarchyClass + '" title="' + app.localize('HierarchyLevel') + ': ' + row.HierarchyLevel + '">' + 
                                    hierarchyIcon + row.HierarchyLevel + '</span>';
                            
                            html += '</div>';
                            return html;
                        }
                    },
                    {
                        data: null,
                        width: "10%",
                        className: "text-center",
                        render: function(data, type, row) {
                            var html = '<div class="impact-info">';
                            html += '<div class="impact-users">' + row.AffectedUsersCount + ' ' + app.localize('Users') + '</div>';
                            html += '<div class="impact-records">' + row.RecordStatesCount + ' ' + app.localize('Records') + '</div>';
                            html += '</div>';
                            return html;
                        }
                    },
                    {
                        data: null,
                        width: "15%",
                        render: function(data, type, row) {
                            var html = '<div class="mapping-action-container" style="display:none;">';
                            html += '<select class="form-control mapping-action-select" data-category-id="' + row.CategoryId + '">';
                            html += '<option value="map"' + (row.MappingAction === 'map' ? ' selected' : '') + '>' + app.localize('MapToExisting') + '</option>';
                            html += '<option value="copy"' + (row.MappingAction === 'copy' ? ' selected' : '') + '>' + app.localize('CopyToNew') + '</option>';
                            html += '<option value="skip"' + (row.MappingAction === 'skip' ? ' selected' : '') + '>' + app.localize('Skip') + '</option>';
                            html += '</select>';
                            html += '</div>';
                            return html;
                        }
                    },
                    {
                        data: null,
                        width: "20%",
                        render: function(data, type, row) {
                            return renderTargetMappingSection(row);
                        }
                    },
                    {
                        data: null,
                        width: "10%",
                        className: "text-center",
                        render: function(data, type, row) {
                            return '<div class="suggestions-container"><button type="button" class="btn btn-outline-info btn-sm suggestions-btn" data-category-id="' + row.CategoryId + '"><i class="fa fa-lightbulb"></i></button></div>';
                        }
                    },
                    {
                        data: null,
                        width: "5%",
                        className: "text-center",
                        render: function(data, type, row) {
                            return '<div class="mapping-status"><span class="status-badge status-' + row.Status + '">' + app.localize('Status' + row.Status.charAt(0).toUpperCase() + row.Status.slice(1)) + '</span></div>';
                        }
                    }
                ],
                paging: false, // Disable paging for bulk operations
                searching: false, // Disable search since we have our own header
                ordering: false, // Disable ordering since we have our own header
                info: false, // Hide info text (e.g., "Showing 1 to 10 of 50 entries")
                responsive: false, // Disable responsive since we're using custom scroll
                scrollY: false, // Disable DataTables built-in scrolling since we're using our own container
                scrollX: false, // Disable horizontal scrolling
                autoWidth: false, // Maintain column widths
                dom: 't', // Only show the table body, no headers or other controls
                language: {
                    url: abp.localization.currentLanguage.name === 'en' ? '' : '/lib/datatables/plug-ins/i18n/' + abp.localization.currentLanguage.name + '.json'
                }
            });
            
            // Ensure the select all checkbox is functional after table initialization
            setTimeout(function() {
                initializeSelectAllFunctionality();
                syncHeaderWithTableColumns();
                setupHeaderSyncHandlers();
            }, 100);
        }

        function initializeSelectAllFunctionality() {
            var modal = _modalManager.getModal();
            var $selectAllCheckbox = modal.find('#fixedTableHeader #selectAllCategories');
            
            // Remove any existing handlers to prevent duplicates
            $selectAllCheckbox.off('change.selectAll');
            
            // Add select all functionality
            $selectAllCheckbox.on('change.selectAll', function() {
                var isChecked = $(this).is(':checked');
                modal.find('.category-checkbox').each(function() {
                    var $checkbox = $(this);
                    var categoryId = $checkbox.data('category-id');
                    var $row = $checkbox.closest('tr');
                    
                    $checkbox.prop('checked', isChecked);
                    handleRowSelection($row, categoryId, isChecked);
                });
                
                // Update header visibility after all selections are processed
                setTimeout(function() {
                    updateHeaderVisibility();
                    syncHeaderWithTableColumns();
                }, 100); // Longer delay since we're processing multiple rows
            });
        }

        function renderTargetMappingSection(row) {
            var html = '<div class="target-mapping-section" style="display:none;">';
            
            // Map to existing section - will be populated dynamically when action changes
            html += '<div class="target-category-section" style="' + (row.MappingAction !== 'map' ? 'display:none;' : '') + '">';
            html += '<select class="form-control target-category-select" data-category-id="' + row.CategoryId + '">';
            html += '<option value="">' + app.localize('SelectTargetCategory') + '</option>';
            html += '</select>';
            html += '</div>';
            
            // Copy to new section
            html += '<div class="new-category-section" style="' + (row.MappingAction !== 'copy' ? 'display:none;' : '') + '">';
            html += '<input type="text" class="form-control new-category-input" placeholder="' + app.localize('NewRequirementName') + '" data-category-id="' + row.CategoryId + '" data-field="requirement" value="' + row.NewRequirementName + '">';
            html += '<input type="text" class="form-control new-category-input" placeholder="' + app.localize('NewCategoryName') + '" data-category-id="' + row.CategoryId + '" data-field="category" value="' + row.NewCategoryName + '">';
            html += '</div>';
            
            // Skip warning section
            html += '<div class="skip-warning-section" style="' + (row.MappingAction !== 'skip' ? 'display:none;' : '') + '">';
            html += '<div class="skip-warning"><i class="fa fa-exclamation-triangle"></i> ' + app.localize('DataWillBeLost') + '</div>';
            html += '</div>';
            
            html += '</div>';
            return html;
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
                updateCategoryTargetSelection(categoryId, targetCategoryId);
                updateMappingProgress();
            });
            
            // New category input handlers
            modal.off('input', '.new-category-input').on('input', '.new-category-input', function() {
                var categoryId = $(this).data('category-id');
                var field = $(this).data('field');
                var value = $(this).val();
                updateCategoryNewInput(categoryId, field, value);
                updateMappingProgress();
            });
            
            // Suggestions button handler
            modal.off('click', '.suggestions-btn').on('click', '.suggestions-btn', function() {
                var categoryId = $(this).data('category-id');
                loadCategorySuggestions(categoryId, $(this));
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
                // Get selected categories for validation
                var selectedCategoryIds = [];
                modal.find('.category-checkbox:checked').each(function() {
                    selectedCategoryIds.push($(this).data('category-id'));
                });

                if (selectedCategoryIds.length === 0) {
                    abp.message.warn('Please select at least one category to validate.');
                    return;
                }

                // Filter mapping decisions to only include selected categories
                var allMappingDecisions = getCategoryMappingDecisions();
                var selectedMappingDecisions = allMappingDecisions.filter(function(mapping) {
                    return selectedCategoryIds.indexOf(mapping.SourceCategoryId) !== -1;
                });

                // Validate selected mappings
                var validationResult = validateSelectedMappings(selectedMappingDecisions);
                
                if (validationResult.isValid && !validationResult.hasWarnings) {
                    abp.message.success('All selected category mappings are valid!');
                } else if (validationResult.isValid && validationResult.hasWarnings) {
                    abp.message.warn('Selected mappings are valid but have ' + validationResult.warningCount + ' warnings.');
                } else {
                    abp.message.error('Found ' + validationResult.errorCount + ' validation errors in selected mappings.');
                }
            });
            
            // Select all checkbox handler
            modal.off('change', '#selectAllCategories').on('change', '#selectAllCategories', function() {
                var isChecked = $(this).is(':checked');
                modal.find('.category-checkbox').each(function() {
                    var $checkbox = $(this);
                    var categoryId = $checkbox.data('category-id');
                    var $row = $checkbox.closest('tr');
                    
                    $checkbox.prop('checked', isChecked);
                    handleRowSelection($row, categoryId, isChecked);
                });
            });
        }

        function updateCategoryMappingAction(categoryId, action) {
            var category = _categoryData.find(c => c.CategoryId === categoryId);
            if (category) {
                category.MappingAction = action;
                category.Status = 'pending';
                
                // Clear previous selections when action changes
                if (action !== 'map') {
                    category.TargetCategoryId = null;
                }
                if (action !== 'copy') {
                    category.NewRequirementName = '';
                    category.NewCategoryName = '';
                }
            }
        }

        function updateTargetMappingSectionVisibility($row, action) {
            console.log('updateTargetMappingSectionVisibility called with action:', action);
            
            // Only show/hide sections if the row is selected (checkbox is checked)
            var $checkbox = $row.find('.category-checkbox');
            var isRowSelected = $checkbox.is(':checked');
            
            if (isRowSelected) {
                $row.find('.target-category-section').toggle(action === 'map');
                $row.find('.new-category-section').toggle(action === 'copy');
                $row.find('.skip-warning-section').toggle(action === 'skip');
                
                // If switching to map action, populate the dropdown with preloaded options
                if (action === 'map') {
                    var categoryId = $row.find('.mapping-action-select').data('category-id');
                    var $targetSelect = $row.find('.target-category-select');
                    populateTargetCategorySelectFromPreloaded(categoryId, $targetSelect);
                }
            }
        }

        function populateTargetCategorySelectFromPreloaded(categoryId, $targetSelect) {
            // Find the category with preloaded options
            var category = _categoryData.find(c => c.CategoryId === categoryId);
            if (!category) {
                console.log('Category not found:', categoryId);
                console.log('Available categories:', _categoryData.map(c => ({ id: c.CategoryId, name: c.CategoryName })));
                return;
            }

            console.log('Populating dropdown for category:', category.CategoryName, 'with options:', category.TargetOptions);
            console.log('TargetOptions type:', typeof category.TargetOptions, 'Length:', category.TargetOptions ? category.TargetOptions.length : 'null/undefined');

            var html = '<option value="">' + app.localize('SelectTargetCategory') + '</option>';
            
            if (category.TargetOptions && category.TargetOptions.length > 0) {
                console.log('Processing', category.TargetOptions.length, 'target options');
                category.TargetOptions.forEach(function(option, index) {
                    // Handle both camelCase and PascalCase property names
                    var categoryName = option.categoryName || option.CategoryName;
                    var matchScore = option.matchScore || option.MatchScore;
                    var optionCategoryId = option.categoryId || option.CategoryId;
                    
                    console.log('Option', index, ':', {
                        categoryName: categoryName,
                        matchScore: matchScore,
                        optionCategoryId: optionCategoryId,
                        originalOption: option
                    });
                    
                    var displayText = categoryName || 'Unknown Category';
                    if (matchScore) {
                        displayText += ' (' + matchScore + '% ' + app.localize('Match') + ')';
                    }
                    
                    var selected = category.TargetCategoryId === optionCategoryId ? ' selected' : '';
                    html += '<option value="' + optionCategoryId + '"' + selected + '>' + displayText + '</option>';
                });
            } else {
                console.log('No target options available - showing NoCompatibleCategoriesFound');
                html += '<option value="" disabled>' + app.localize('NoCompatibleCategoriesFound') + '</option>';
            }
            
            console.log('Final HTML for dropdown:', html);
            $targetSelect.html(html);
        }

        function updateCategoryTargetSelection(categoryId, targetCategoryId) {
            var category = _categoryData.find(c => c.CategoryId === categoryId);
            if (category) {
                category.TargetCategoryId = targetCategoryId;
                category.Status = targetCategoryId ? 'mapped' : 'pending';
            }
        }

        function updateCategoryNewInput(categoryId, field, value) {
            var category = _categoryData.find(c => c.CategoryId === categoryId);
            if (category) {
                if (field === 'requirement') {
                    category.NewRequirementName = value;
                } else if (field === 'category') {
                    category.NewCategoryName = value;
                }
                
                category.Status = (category.NewRequirementName && category.NewCategoryName) ? 'copied' : 'pending';
            }
        }

        function updateMigrationSummary() {
            var modal = _modalManager.getModal();
            var analysis = _migrationData.analysis;
            
            if (analysis) {
                // Count total categories (both mapping required and no migration required)
                var totalCategories = analysis.requirementCategories.length + 
                    (analysis.noMigrationRequiredCategories ? analysis.noMigrationRequiredCategories.length : 0);
                
                modal.find('#totalCategoriesCount').text(totalCategories);
                modal.find('#totalAffectedUsers').text(analysis.totalUsersCount);
                
                // Calculate total record states from both arrays
                var totalRecordStates = analysis.requirementCategories.reduce((sum, cat) => sum + cat.recordStatesCount, 0);
                if (analysis.noMigrationRequiredCategories) {
                    totalRecordStates += analysis.noMigrationRequiredCategories.reduce((sum, cat) => sum + cat.recordStatesCount, 0);
                }
                
                modal.find('#totalRecordStates').text(totalRecordStates);
                modal.find('#migrationSummary').removeClass('d-none');
            }
        }

        function updateMappingProgress() {
            var modal = _modalManager.getModal();
            var completedMappings = _categoryData.filter(c => c.Status !== 'pending').length;
            var totalMappings = _categoryData.length;
            
            modal.find('#mappingProgress').text(completedMappings + '/' + totalMappings);
        }

        function handleRowSelection($row, categoryId, isChecked) {
            var $actionContainer = $row.find('.mapping-action-container');
            var $targetMappingSection = $row.find('.target-mapping-section');
            
            if (isChecked) {
                // Show the mapping action dropdown
                $actionContainer.show();
                $targetMappingSection.show();
                
                // Populate the target category dropdown if action is 'map'
                var currentAction = $row.find('.mapping-action-select').val();
                if (currentAction === 'map') {
                    var $targetSelect = $row.find('.target-category-select');
                    populateTargetCategorySelectFromPreloaded(categoryId, $targetSelect);
                }
            } else {
                // Hide the mapping controls
                $actionContainer.hide();
                $targetMappingSection.hide();
            }
            
            // Update header visibility and column alignment
            setTimeout(function() {
                updateHeaderVisibility();
                syncHeaderWithTableColumns();
            }, 50); // Small delay to ensure DOM updates are complete
        }

        function updateSelectAllCheckboxState() {
            var modal = _modalManager.getModal();
            var $selectAllCheckbox = modal.find('#fixedTableHeader #selectAllCategories');
            var $categoryCheckboxes = modal.find('.category-checkbox');
            var checkedCount = $categoryCheckboxes.filter(':checked').length;
            var totalCount = $categoryCheckboxes.length;
            
            if (checkedCount === 0) {
                $selectAllCheckbox.prop('indeterminate', false);
                $selectAllCheckbox.prop('checked', false);
            } else if (checkedCount === totalCount) {
                $selectAllCheckbox.prop('indeterminate', false);
                $selectAllCheckbox.prop('checked', true);
            } else {
                $selectAllCheckbox.prop('indeterminate', true);
                $selectAllCheckbox.prop('checked', false);
            }
        }

        function displayNoMigrationRequiredCategories() {
            var modal = _modalManager.getModal();
            var container = modal.find('#noMigrationRequiredList');
            var section = modal.find('#noMigrationRequiredSection');
            
            // Always show the section
            section.removeClass('d-none');
            
            if (!_noMigrationRequiredCategories || _noMigrationRequiredCategories.length === 0) {
                container.html('<p class="text-muted text-center mb-0">' + 
                    '<i class="fa fa-info-circle"></i> ' + 
                    app.localize('NoRequirementsFoundThatApplyBothBeforeAndAfter') + 
                    '</p>');
                return;
            }
            
            // Group categories by hierarchy level
            var grouped = {};
            _noMigrationRequiredCategories.forEach(function(category) {
                var level = category.hierarchyLevel || 'Unknown';
                if (!grouped[level]) {
                    grouped[level] = [];
                }
                grouped[level].push(category);
            });
            
            var html = '';
            
            // Display each hierarchy level group
            Object.keys(grouped).forEach(function(level) {
                var categories = grouped[level];
                var badgeClass = '';
                var iconHtml = '';
                
                switch(level) {
                    case 'Tenant':
                        badgeClass = 'badge-primary';
                        iconHtml = '<i class="fa fa-building"></i> ';
                        break;
                    case 'Department':
                        badgeClass = 'badge-info';
                        iconHtml = '<i class="fa fa-sitemap"></i> ';
                        break;
                    case 'Cohort':
                        badgeClass = 'badge-success';
                        iconHtml = '<i class="fa fa-users"></i> ';
                        break;
                    case 'CohortAndDepartment':
                        badgeClass = 'badge-warning';
                        iconHtml = '<i class="fa fa-users"></i><i class="fa fa-sitemap"></i> ';
                        break;
                }
                
                html += '<div class="mb-3">';
                html += '<h6><span class="badge ' + badgeClass + '">' + iconHtml + level + ' ' + app.localize('Level') + '</span></h6>';
                html += '<ul class="list-unstyled ms-3">';
                
                categories.forEach(function(category) {
                    html += '<li class="mb-1">';
                    html += '<strong>' + category.categoryName + '</strong>';
                    html += ' <small class="text-muted">(' + category.requirementName + ')</small>';
                    if (category.affectedUsersCount > 0) {
                        html += ' <span class="badge badge-sm badge-secondary">' + category.affectedUsersCount + ' ' + app.localize('Users') + '</span>';
                    }
                    html += '</li>';
                });
                
                html += '</ul>';
                html += '</div>';
            });
            
            container.html(html);
            section.removeClass('d-none');
        }

        function getCategoryMappingDecisions() {
            return _categoryData.map(function(category) {
                return {
                    SourceCategoryId: category.CategoryId,
                    SourceCategoryName: category.CategoryName,
                    SourceRequirementName: category.RequirementName,
                    SourceRequirementId: category.RequirementId,
                    Action: category.MappingAction,
                    TargetCategoryId: category.TargetCategoryId,
                    NewRequirementName: category.NewRequirementName,
                    NewCategoryName: category.NewCategoryName,
                    AffectedRecordStatesCount: category.RecordStatesCount,
                    AffectedUsersCount: category.AffectedUsersCount,
                    HasDataLoss: category.MappingAction === 'skip'
                };
            });
        }

        function loadCategorySuggestions(categoryId, $button) {
            // Find the category data with preloaded options
            var category = _categoryData.find(c => c.CategoryId === categoryId);
            if (!category) {
                abp.message.error(app.localize('CategoryNotFound'));
                return;
            }

            // Show loading state briefly for visual feedback
            $button.prop('disabled', true).html('<i class="fa fa-spinner fa-spin"></i>');

            setTimeout(function() {
                // Filter preloaded options to show only good matches (70%+)
                var goodMatches = category.TargetOptions.filter(function(option) {
                    var matchScore = option.matchScore || option.MatchScore || 0;
                    return matchScore >= 70;
                }).slice(0, 5); // Limit to top 5 suggestions

                displayCategorySuggestions(categoryId, goodMatches, $button);
                $button.prop('disabled', false).html('<i class="fa fa-lightbulb"></i>');
            }, 200); // Brief delay for visual feedback
        }

        function displayCategorySuggestions(categoryId, suggestions, $button) {
            // Remove existing dropdown
            $button.closest('.suggestions-container').find('.suggestions-dropdown').remove();

            if (!suggestions || suggestions.length === 0) {
                abp.message.info(app.localize('NoSuggestionsFound'));
                return;
            }

            // Create suggestions dropdown
            var html = '<div class="suggestions-dropdown">';
            suggestions.forEach(function(suggestion) {
                // Handle both camelCase and PascalCase property names
                var categoryName = suggestion.categoryName || suggestion.CategoryName;
                var matchScore = suggestion.matchScore || suggestion.MatchScore;
                var categoryId = suggestion.categoryId || suggestion.CategoryId;
                var requirementId = suggestion.requirementId || suggestion.RequirementId;
                var requirementName = suggestion.requirementName || suggestion.RequirementName;
                
                var matchClass = matchScore >= 90 ? 'suggestion-match-high' :
                               matchScore >= 70 ? 'suggestion-match-medium' : 'suggestion-match-low';
                
                html += '<div class="suggestion-item" data-category-id="' + categoryId + '" data-requirement-id="' + requirementId + '">';
                html += '<div class="suggestion-name">' + categoryName + '</div>';
                html += '<div class="suggestion-score ' + matchClass + '">' + app.localize('MatchScore') + ': ' + matchScore + '%</div>';
                if (requirementName) {
                    html += '<div class="text-muted small">' + requirementName + '</div>';
                }
                html += '</div>';
            });
            html += '</div>';

            // Add dropdown to container
            var $container = $button.closest('.suggestions-container');
            $container.append(html);

            // Handle suggestion selection
            $container.find('.suggestion-item').on('click', function() {
                var targetCategoryId = $(this).data('category-id');
                var $row = $button.closest('tr');
                var $actionSelect = $row.find('.mapping-action-select');
                var $targetSelect = $row.find('.target-category-select');

                // Set action to map and select the suggested category
                $actionSelect.val('map').trigger('change');
                
                // Add option to target select if not exists
                if ($targetSelect.find('option[value="' + targetCategoryId + '"]').length === 0) {
                    var suggestionName = $(this).find('.suggestion-name').text();
                    $targetSelect.append('<option value="' + targetCategoryId + '">' + suggestionName + '</option>');
                }
                
                $targetSelect.val(targetCategoryId).trigger('change');
                
                // Remove dropdown
                $container.find('.suggestions-dropdown').remove();
                
                abp.message.success(app.localize('SuggestionApplied'));
            });

            // Close dropdown when clicking outside
            $(document).on('click.suggestions', function(e) {
                if (!$(e.target).closest('.suggestions-container').length) {
                    $('.suggestions-dropdown').remove();
                    $(document).off('click.suggestions');
                }
            });
        }

        function getTargetDepartmentId() {
            var modal = _modalManager.getModal();
            var departmentOption = modal.find('input[name="departmentOption"]:checked').val();
            
            console.log('Getting target department ID:', {
                departmentOption: departmentOption,
                existingDepartmentValue: modal.find('#targetDepartmentId').val(),
                migrationDataAnalysis: _migrationData.analysis,
                analysisTargetDepartmentId: _migrationData.analysis ? _migrationData.analysis.targetDepartmentId : 'no analysis'
            });
            
            if (departmentOption === 'existing') {
                var targetId = modal.find('#targetDepartmentId').val();
                console.log('Returning existing department ID:', targetId);
                return targetId;
            } else if (departmentOption === 'new' && _migrationData.analysis && _migrationData.analysis.targetDepartmentId) {
                var targetId = _migrationData.analysis.targetDepartmentId;
                console.log('Returning new department ID from analysis:', targetId);
                return targetId;
            }
            
            console.log('No target department ID found, returning null');
            return null;
        }

        function applyBulkAction(action) {
            var modal = _modalManager.getModal();
            var selectedCategories = modal.find('.category-checkbox:checked');
            
            if (selectedCategories.length === 0) {
                abp.message.warn(app.localize('PleaseSelectCategories'));
                return;
            }

            // Show confirmation for destructive actions
            if (action === 'skip') {
                var affectedRecords = 0;
                var affectedUsers = 0;
                
                selectedCategories.each(function() {
                    var categoryId = $(this).data('category-id');
                    var category = _categoryData.find(c => c.CategoryId === categoryId);
                    if (category) {
                        affectedRecords += category.RecordStatesCount || 0;
                        affectedUsers += category.AffectedUsersCount || 0;
                    }
                });

                var confirmMessage = app.localize('ConfirmBulkSkipAction', selectedCategories.length, affectedUsers, affectedRecords);
                
                abp.message.confirm(
                    confirmMessage,
                    app.localize('ConfirmBulkAction'),
                    function(isConfirmed) {
                        if (isConfirmed) {
                            executeBulkAction(action, selectedCategories);
                        }
                    }
                );
            } else {
                executeBulkAction(action, selectedCategories);
            }
        }

        function executeBulkAction(action, selectedCategories) {
            var modal = _modalManager.getModal();
            var totalCategories = selectedCategories.length;
            var processedCategories = 0;
            
            // Show progress indicator
            showBulkActionProgress(action, totalCategories);
            
            // Process each category with a small delay for UI responsiveness
            selectedCategories.each(function(index) {
                var $checkbox = $(this);
                var categoryId = $checkbox.data('category-id');
                var $row = $checkbox.closest('tr');
                var $actionSelect = $row.find('.mapping-action-select');
                
                setTimeout(function() {
                    // Ensure the row is selected (checkbox checked) and controls are visible
                    if (!$checkbox.is(':checked')) {
                        $checkbox.prop('checked', true);
                        handleRowSelection($row, categoryId, true);
                    }
                    
                    // Apply the action
                    $actionSelect.val(action).trigger('change');
                    
                    // Update progress
                    processedCategories++;
                    updateBulkActionProgress(processedCategories, totalCategories);
                    
                    // Complete when all processed
                    if (processedCategories === totalCategories) {
                        completeBulkAction(action, totalCategories);
                    }
                }, index * 50); // 50ms delay between each category
            });
        }

        function showBulkActionProgress(action, totalCategories) {
            var modal = _modalManager.getModal();
            var actionText = app.localize('BulkAction' + action.charAt(0).toUpperCase() + action.slice(1));
            
            var progressHtml = '<div id="bulkActionProgress" class="alert alert-info">' +
                '<div class="d-flex align-items-center">' +
                '<div class="spinner-border spinner-border-sm me-2" role="status"></div>' +
                '<div class="flex-grow-1">' +
                '<strong>' + actionText + '</strong><br>' +
                '<span id="bulkProgressText">' + app.localize('ProcessingCategories', 0, totalCategories) + '</span>' +
                '</div>' +
                '</div>' +
                '<div class="progress mt-2">' +
                '<div id="bulkProgressBar" class="progress-bar" role="progressbar" style="width: 0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100"></div>' +
                '</div>' +
                '</div>';
            
            modal.find('#categoryMappingTable').before(progressHtml);
        }

        function updateBulkActionProgress(processed, total) {
            var modal = _modalManager.getModal();
            var percentage = Math.round((processed / total) * 100);
            
            modal.find('#bulkProgressText').text(app.localize('ProcessingCategories', processed, total));
            modal.find('#bulkProgressBar').css('width', percentage + '%').attr('aria-valuenow', percentage);
        }

        function completeBulkAction(action, totalCategories) {
            var modal = _modalManager.getModal();
            var actionText = app.localize('BulkAction' + action.charAt(0).toUpperCase() + action.slice(1));
            
            // Update progress to success state
            var $progressAlert = modal.find('#bulkActionProgress');
            $progressAlert.removeClass('alert-info').addClass('alert-success');
            $progressAlert.find('.spinner-border').remove();
            $progressAlert.find('#bulkProgressText').text(app.localize('BulkActionCompleted', actionText, totalCategories));
            $progressAlert.find('#bulkProgressBar').addClass('bg-success');
            
            // Remove progress indicator after 3 seconds
            setTimeout(function() {
                $progressAlert.fadeOut(function() {
                    $(this).remove();
                });
            }, 3000);
            
            // Update mapping progress
            updateMappingProgress();
            
            // Clear selection
            modal.find('#selectAllCategories').prop('checked', false);
            modal.find('.category-checkbox').prop('checked', false);
            
            // Show success message
            abp.message.success(app.localize('BulkActionSuccess', actionText, totalCategories));
        }

        function autoSuggestMappings() {
            var modal = _modalManager.getModal();
            var targetDepartmentId = getTargetDepartmentId();
            
            if (!targetDepartmentId) {
                abp.message.warn(app.localize('PleaseSelectTargetDepartmentFirst'));
                return;
            }

            // Check if any categories are currently selected
            var selectedCategories = modal.find('.category-checkbox:checked');
            var hasSelectedCategories = selectedCategories.length > 0;
            
            if (hasSelectedCategories) {
                // Process only selected categories
                autoSuggestForSelectedCategories(selectedCategories);
            } else {
                // No categories selected - select all and process with auto-deselection
                autoSuggestForAllCategories();
            }
        }

        function autoSuggestForSelectedCategories(selectedCategories) {
            var modal = _modalManager.getModal();
            var categoriesToProcess = [];
            
            // Filter to only categories set to "map to existing" without target selection
            selectedCategories.each(function() {
                var $checkbox = $(this);
                var categoryId = $checkbox.data('category-id');
                var $row = $checkbox.closest('tr');
                var currentAction = $row.find('.mapping-action-select').val();
                var currentTarget = $row.find('.target-category-select').val();
                
                // Only process if action is "map" and no target is selected
                if (currentAction === 'map' && (!currentTarget || currentTarget === '')) {
                    categoriesToProcess.push({
                        categoryId: categoryId,
                        $row: $row,
                        category: _categoryData.find(c => c.CategoryId === categoryId)
                    });
                }
            });
            
            if (categoriesToProcess.length === 0) {
                abp.message.info(app.localize('NoEligibleCategoriesForAutoSuggest'));
                return;
            }
            
            // Show confirmation dialog
            abp.message.confirm(
                app.localize('ConfirmAutoSuggestSelected', categoriesToProcess.length),
                app.localize('AutoSuggestMappings'),
                function(isConfirmed) {
                    if (isConfirmed) {
                        executeAutoSuggestForSelected(categoriesToProcess);
                    }
                }
            );
        }

        function autoSuggestForAllCategories() {
            var modal = _modalManager.getModal();
            
            // Show confirmation dialog
            abp.message.confirm(
                app.localize('ConfirmAutoSuggestAll'),
                app.localize('AutoSuggestMappings'),
                function(isConfirmed) {
                    if (isConfirmed) {
                        executeAutoSuggestForAll();
                    }
                }
            );
        }

        function executeAutoSuggestForSelected(categoriesToProcess) {
            var modal = _modalManager.getModal();
            var successfulSuggestions = 0;
            
            // Process each eligible category
            categoriesToProcess.forEach(function(item) {
                var category = item.category;
                if (!category || !category.TargetOptions) return;
                
                // Find 90%+ matches
                var highConfidenceMatches = category.TargetOptions.filter(function(option) {
                    var matchScore = option.matchScore || option.MatchScore || 0;
                    return matchScore >= 90;
                });
                
                if (highConfidenceMatches.length > 0) {
                    var bestMatch = highConfidenceMatches[0]; // Already sorted by match score
                    applySuggestionToCategory(item.categoryId, bestMatch, item.$row);
                    successfulSuggestions++;
                }
                // If no 90%+ match, leave unchanged
            });
            
            // Update UI and show results
            updateHeaderVisibility();
            syncHeaderWithTableColumns();
            updateMappingProgress();
            
            if (successfulSuggestions > 0) {
                abp.message.success(app.localize('AutoSuggestSelectedSuccess', successfulSuggestions, categoriesToProcess.length));
            } else {
                abp.message.info(app.localize('NoHighConfidenceMatches'));
            }
        }

        function executeAutoSuggestForAll() {
            var modal = _modalManager.getModal();
            var successfulSelections = 0;
            var totalCategories = _categoryData.length;
            
            // First, select all categories
            modal.find('.category-checkbox').each(function() {
                var $checkbox = $(this);
                var categoryId = $checkbox.data('category-id');
                var $row = $checkbox.closest('tr');
                
                // Select the category
                $checkbox.prop('checked', true);
                handleRowSelection($row, categoryId, true);
                
                // Find the category data
                var category = _categoryData.find(c => c.CategoryId === categoryId);
                if (!category || !category.TargetOptions) return;
                
                // Check for 90%+ matches
                var highConfidenceMatches = category.TargetOptions.filter(function(option) {
                    var matchScore = option.matchScore || option.MatchScore || 0;
                    return matchScore >= 90;
                });
                
                if (highConfidenceMatches.length > 0) {
                    // Set to map action and apply best match
                    var $actionSelect = $row.find('.mapping-action-select');
                    $actionSelect.val('map').trigger('change');
                    
                    var bestMatch = highConfidenceMatches[0];
                    applySuggestionToCategory(categoryId, bestMatch, $row);
                    successfulSelections++;
                } else {
                    // No good match - deselect this category
                    $checkbox.prop('checked', false);
                    handleRowSelection($row, categoryId, false);
                }
            });
            
            // Update select all checkbox state
            updateSelectAllCheckboxState();
            
            // Update UI
            updateHeaderVisibility();
            syncHeaderWithTableColumns();
            updateMappingProgress();
            
            if (successfulSelections > 0) {
                abp.message.success(app.localize('AutoSuggestAllSuccess', successfulSelections, totalCategories));
            } else {
                abp.message.warn(app.localize('NoHighConfidenceMatchesFound'));
            }
        }

        function applySuggestionToCategory(categoryId, suggestion, $row) {
            var modal = _modalManager.getModal();
            
            // If $row not provided, find it
            if (!$row) {
                $row = modal.find('tr').filter(function() {
                    return $(this).find('.category-checkbox').data('category-id') === categoryId;
                });
            }
            
            if ($row.length === 0) return;
            
            var $actionSelect = $row.find('.mapping-action-select');
            var $targetSelect = $row.find('.target-category-select');
            
            // Set action to map (this will trigger showing the target section)
            $actionSelect.val('map').trigger('change');
            
            // Handle both camelCase and PascalCase property names
            var categoryName = suggestion.categoryName || suggestion.CategoryName;
            var matchScore = suggestion.matchScore || suggestion.MatchScore;
            var suggestionCategoryId = suggestion.categoryId || suggestion.CategoryId;
            
            // Add option to target select if not exists
            if ($targetSelect.find('option[value="' + suggestionCategoryId + '"]').length === 0) {
                var displayText = categoryName;
                if (matchScore) {
                    displayText += ' (' + matchScore + '% ' + app.localize('Match') + ')';
                }
                $targetSelect.append('<option value="' + suggestionCategoryId + '">' + displayText + '</option>');
            }
            
            // Select the suggested option
            $targetSelect.val(suggestionCategoryId).trigger('change');
        }

        function validateAllMappings() {
            var mappingDecisions = getCategoryMappingDecisions();
            return validateSelectedMappings(mappingDecisions);
        }

        function validateSelectedMappings(mappingDecisions) {
            var issues = [];
            var warnings = [];
            var modal = _modalManager.getModal();
            
            // Clear previous validation highlighting
            modal.find('.validation-error').removeClass('validation-error');
            modal.find('.validation-warning').removeClass('validation-warning');
            
            mappingDecisions.forEach(function(mapping, index) {
                var $row = modal.find('tr').filter(function() {
                    return $(this).find('.category-checkbox').data('category-id') === mapping.SourceCategoryId;
                });
                var $checkbox = $row.find('.category-checkbox');
                var isRowSelected = $checkbox.is(':checked');
                var hasError = false;
                var hasWarning = false;
                
                // Only validate selected rows
                if (!isRowSelected) {
                    return; // Skip validation for unselected rows
                }
                
                // Validate mapping action selection
                if (!mapping.Action || mapping.Action === '') {
                    issues.push({
                        type: 'error',
                        message: 'Category "' + mapping.SourceCategoryName + '" needs an action selected (Map, Copy, or Skip)',
                        categoryId: mapping.SourceCategoryId,
                        field: 'action'
                    });
                    hasError = true;
                }
                
                // Validate map action requirements
                if (mapping.Action === 'map') {
                    if (!mapping.TargetCategoryId) {
                        issues.push({
                            type: 'error',
                            message: 'Category "' + mapping.SourceCategoryName + '" needs a target category selected',
                            categoryId: mapping.SourceCategoryId,
                            field: 'target'
                        });
                        hasError = true;
                    }
                }
                
                // Validate copy action requirements
                if (mapping.Action === 'copy') {
                    if (!mapping.NewRequirementName || mapping.NewRequirementName.trim() === '') {
                        issues.push({
                            type: 'error',
                            message: 'Category "' + mapping.SourceCategoryName + '" needs a new requirement name',
                            categoryId: mapping.SourceCategoryId,
                            field: 'newRequirement'
                        });
                        hasError = true;
                    }
                    if (!mapping.NewCategoryName || mapping.NewCategoryName.trim() === '') {
                        issues.push({
                            type: 'error',
                            message: 'Category "' + mapping.SourceCategoryName + '" needs a new category name',
                            categoryId: mapping.SourceCategoryId,
                            field: 'newCategory'
                        });
                        hasError = true;
                    }
                    
                    // Check for duplicate names
                    if (mapping.NewRequirementName && isDuplicateRequirementName(mapping.NewRequirementName, mapping.SourceCategoryId)) {
                        warnings.push({
                            type: 'warning',
                            message: 'Duplicate requirement name warning: "' + mapping.NewRequirementName + '"',
                            categoryId: mapping.SourceCategoryId,
                            field: 'newRequirement'
                        });
                        hasWarning = true;
                    }
                    
                    if (mapping.NewCategoryName && isDuplicateCategoryName(mapping.NewCategoryName, mapping.SourceCategoryId)) {
                        warnings.push({
                            type: 'warning',
                            message: 'Duplicate category name warning: "' + mapping.NewCategoryName + '"',
                            categoryId: mapping.SourceCategoryId,
                            field: 'newCategory'
                        });
                        hasWarning = true;
                    }
                }
                
                // Validate skip action warnings
                if (mapping.Action === 'skip') {
                    if (mapping.AffectedRecordStatesCount > 0) {
                        warnings.push({
                            type: 'warning',
                            message: 'Skipping "' + mapping.SourceCategoryName + '" will cause data loss (' + mapping.AffectedRecordStatesCount + ' records)',
                            categoryId: mapping.SourceCategoryId,
                            field: 'action'
                        });
                        hasWarning = true;
                    }
                }
                
                // Apply visual highlighting
                if (hasError) {
                    $row.addClass('validation-error');
                } else if (hasWarning) {
                    $row.addClass('validation-warning');
                }
            });
            
            // Display validation results
            displayValidationResults(issues, warnings);
            
            // Return validation status
            return {
                isValid: issues.length === 0,
                hasWarnings: warnings.length > 0,
                errorCount: issues.length,
                warningCount: warnings.length
            };
        }

        function displayValidationResults(issues, warnings) {
            var modal = _modalManager.getModal();
            var validationContainer = modal.find('#mappingValidationMessages');
            var issuesList = modal.find('#validationIssuesList');
            
            if (issues.length > 0 || warnings.length > 0) {
                issuesList.empty();
                
                // Add errors
                issues.forEach(function(issue) {
                    var iconClass = issue.type === 'error' ? 'fa-exclamation-circle text-danger' : 'fa-exclamation-triangle text-warning';
                    var listItem = '<li class="validation-issue-' + issue.type + '">' +
                        '<i class="fa ' + iconClass + ' me-2"></i>' +
                        '<span class="validation-message">' + issue.message + '</span>' +
                        '<button type="button" class="btn btn-sm btn-outline-primary ms-2 goto-category-btn" data-category-id="' + issue.categoryId + '">' +
                        '<i class="fa fa-arrow-right"></i> ' + app.localize('GoTo') +
                        '</button>' +
                        '</li>';
                    issuesList.append(listItem);
                });
                
                // Add warnings
                warnings.forEach(function(warning) {
                    var iconClass = 'fa-exclamation-triangle text-warning';
                    var listItem = '<li class="validation-issue-warning">' +
                        '<i class="fa ' + iconClass + ' me-2"></i>' +
                        '<span class="validation-message">' + warning.message + '</span>' +
                        '<button type="button" class="btn btn-sm btn-outline-primary ms-2 goto-category-btn" data-category-id="' + warning.categoryId + '">' +
                        '<i class="fa fa-arrow-right"></i> ' + app.localize('GoTo') +
                        '</button>' +
                        '</li>';
                    issuesList.append(listItem);
                });
                
                // Update validation container styling
                validationContainer.removeClass('alert-warning alert-success').addClass(issues.length > 0 ? 'alert-danger' : 'alert-warning');
                validationContainer.find('.alert-heading').text(
                    issues.length > 0 ? app.localize('ValidationErrors') : app.localize('ValidationWarnings')
                );
                
                validationContainer.removeClass('d-none');
                
                // Add event handlers for "Go To" buttons
                modal.find('.goto-category-btn').off('click').on('click', function() {
                    var categoryId = $(this).data('category-id');
                    highlightAndScrollToCategory(categoryId);
                });
                
            } else {
                validationContainer.addClass('d-none');
                abp.message.success(app.localize('AllMappingsValid'));
            }
        }

        function highlightAndScrollToCategory(categoryId) {
            var modal = _modalManager.getModal();
            
            // Find the row by looking for the checkbox with the matching category ID
            var $targetRow = modal.find('tr').filter(function() {
                return $(this).find('.category-checkbox').data('category-id') === categoryId;
            });
            
            if ($targetRow.length > 0) {
                // Remove previous highlights
                modal.find('.highlight-category').removeClass('highlight-category');
                
                // Add highlight to target row
                $targetRow.addClass('highlight-category');
                
                // Ensure the row is selected and controls are visible
                var $checkbox = $targetRow.find('.category-checkbox');
                if (!$checkbox.is(':checked')) {
                    $checkbox.prop('checked', true);
                    handleRowSelection($targetRow, categoryId, true);
                }
                
                // Scroll to the row using our custom scroll container
                var $scrollContainer = modal.find('.table-scroll-container');
                
                if ($scrollContainer.length > 0) {
                    setTimeout(function() {
                        var rowPosition = $targetRow.position();
                        if (rowPosition) {
                            var containerHeight = $scrollContainer.height();
                            var scrollTop = $scrollContainer.scrollTop();
                            var rowTop = rowPosition.top + scrollTop;
                            var rowHeight = $targetRow.outerHeight();
                            
                            // Calculate the optimal scroll position to center the row
                            var targetScrollTop = rowTop - (containerHeight / 2) + (rowHeight / 2);
                            
                            // Ensure we don't scroll beyond the container bounds
                            var maxScrollTop = $scrollContainer[0].scrollHeight - containerHeight;
                            targetScrollTop = Math.max(0, Math.min(targetScrollTop, maxScrollTop));
                            
                            $scrollContainer.animate({
                                scrollTop: targetScrollTop
                            }, 500);
                        }
                    }, 100); // Small delay to ensure rendering is complete
                }
                
                // Remove highlight after 5 seconds
                setTimeout(function() {
                    $targetRow.removeClass('highlight-category');
                }, 5000);
                
                // Show a message indicating which category was highlighted
                abp.message.info('Highlighted category: "' + getCategoryNameById(categoryId) + '"');
            } else {
                abp.message.warn('Category not found in the current view.');
            }
        }

        function getCategoryNameById(categoryId) {
            var category = _categoryData.find(c => c.CategoryId === categoryId);
            return category ? category.CategoryName : 'Unknown Category';
        }

        function isDuplicateRequirementName(name, excludeCategoryId) {
            // Check against existing requirements in target department
            // This would need to be implemented with an AJAX call to the backend
            // For now, return false as placeholder
            return false;
        }

        function isDuplicateCategoryName(name, excludeCategoryId) {
            // Check against existing categories in target department
            // This would need to be implemented with an AJAX call to the backend
            // For now, return false as placeholder
            return false;
        }

        function updateStepDisplay() {
            var modal = _modalManager.getModal();
            
            // Update progress indicator
            updateProgressIndicator();
            
            // Show/hide steps
            modal.find('.wizard-step').addClass('d-none');
            modal.find('#step' + _currentStep).removeClass('d-none');
            
            // Initialize step-specific functionality
            if (_currentStep === 1) {
                initializeStep1();
            } else if (_currentStep === 2) {
                initializeCategoryMapping();
            } else if (_currentStep === 3) {
                initializeStep3();
            }
            
            // Update navigation buttons
            updateNavigationButtons();
            
            // Save current step state
            saveWizardState();
        }

        function initializeStep1() {
            // Step 1 is already initialized, just ensure state is restored
            restoreStep1State();
        }

        function initializeStep3() {
            // Initialize confirmation step with migration summary
            if (_migrationData.impactSummary) {
                displayMigrationConfirmation();
            }
        }

        function restoreStep1State() {
            var modal = _modalManager.getModal();
            
            // Restore department option selection
            if (_migrationData.departmentOption) {
                modal.find('input[name="departmentOption"][value="' + _migrationData.departmentOption + '"]').prop('checked', true);
                toggleDepartmentSections(_migrationData.departmentOption);
            }
            
            // Restore target department selection
            if (_migrationData.targetDepartmentId) {
                modal.find('#targetDepartmentId').val(_migrationData.targetDepartmentId);
                showDepartmentStatistics(_migrationData.targetDepartmentId);
            }
            
            // Restore new department fields
            if (_migrationData.newDepartmentName) {
                modal.find('#newDepartmentName').val(_migrationData.newDepartmentName);
            }
            if (_migrationData.newDepartmentDescription) {
                modal.find('#newDepartmentDescription').val(_migrationData.newDepartmentDescription);
            }
        }

        function getSourceCohortName() {
            // Use the cohort name loaded from hidden inputs during initialization
            if (_cohortInfo.cohortName) {
                return _cohortInfo.cohortName;
            }
            
            // Fallback to migration data
            if (_migrationData.cohortName) {
                return _migrationData.cohortName;
            }
            
            // This should rarely happen now that we load from hidden inputs
            console.warn('Cohort name not found in stored data, this should not happen');
            return app.localize('UnknownCohort');
        }

        function displayMigrationConfirmation() {
            var modal = _modalManager.getModal();
            var impact = _migrationData.impactSummary || {};
            var analysis = _migrationData.analysis;
            
            // Get source cohort information - check multiple sources
            var sourceCohortName = getSourceCohortName();
            
            // Get source department name from analysis or form
            var sourceDepartmentName = analysis && analysis.sourceDepartmentName ? 
                                     analysis.sourceDepartmentName : 
                                     (modal.find('#sourceDepartmentName').text() || app.localize('NoDepartment'));
            
            // Populate Migration Summary
            modal.find('#confirmCohortName').text(sourceCohortName);
            modal.find('#confirmSourceDepartment').text(sourceDepartmentName);
            modal.find('#confirmTargetDepartment').text(getTargetDepartmentDisplayName());
            modal.find('#confirmTotalUsers').text(analysis ? analysis.totalUsersCount : 0);
            modal.find('#confirmEstimatedDuration').text(analysis ? analysis.estimatedDurationMinutes : 0);
            modal.find('#confirmComplexity').text(analysis ? analysis.migrationComplexity : 'Unknown');
            
            // Populate Category Mapping Summary
            var noMigrationRequiredCount = _noMigrationRequiredCategories ? _noMigrationRequiredCategories.length : 0;
            var totalCategories = impact.totalCategories + noMigrationRequiredCount;
            
            modal.find('#confirmMappedCategories').text(impact.mappedCategories || 0);
            modal.find('#confirmCopiedCategories').text(impact.copiedCategories || 0);
            modal.find('#confirmSkippedCategories').text(impact.skippedCategories || 0);
            modal.find('#confirmNoMigrationRequired').text(noMigrationRequiredCount);
            modal.find('#confirmTotalCategories').text(totalCategories);
            
            // Populate Impact Summary
            if (impact.totalAffectedUsers > 0 || impact.totalAffectedRecords > 0) {
                modal.find('#impactSummarySection').show();
                modal.find('#confirmAffectedUsers').text(impact.totalAffectedUsers || 0);
                modal.find('#confirmAffectedRecords').text(impact.totalAffectedRecords || 0);
                
                // Show/hide new requirements/categories items
                if (impact.newRequirementsCreated > 0) {
                    modal.find('#newRequirementsItem').show();
                    modal.find('#confirmNewRequirements').text(impact.newRequirementsCreated);
                } else {
                    modal.find('#newRequirementsItem').hide();
                }
                
                if (impact.newCategoriesCreated > 0) {
                    modal.find('#newCategoriesItem').show();
                    modal.find('#confirmNewCategories').text(impact.newCategoriesCreated);
                } else {
                    modal.find('#newCategoriesItem').hide();
                }
                
                // Show data loss warning if applicable
                if (impact.dataLossWarnings > 0) {
                    modal.find('#dataLossWarning').show();
                    modal.find('#dataLossMessage').text(
                        app.localize('SkippingCategoriesWillResultInDataLoss', impact.dataLossWarnings, impact.skippedCategories)
                    );
                } else {
                    modal.find('#dataLossWarning').hide();
                }
            } else {
                modal.find('#impactSummarySection').hide();
            }
            
            // Populate Warnings
            if (analysis && analysis.warnings && analysis.warnings.length > 0) {
                modal.find('#warningsSection').show();
                var warningsList = modal.find('#warningsList');
                warningsList.empty();
                analysis.warnings.forEach(function(warning) {
                    warningsList.append('<li>' + warning + '</li>');
                });
            } else {
                modal.find('#warningsSection').hide();
            }
            
            // Reset confirmation checkbox
            modal.find('#confirmMigration').prop('checked', false);
        }

        function getTargetDepartmentDisplayName() {
            var modal = _modalManager.getModal();
            
            if (_migrationData.departmentOption === 'new') {
                var newDeptName = _migrationData.newDepartmentName || modal.find('#newDepartmentName').val();
                return newDeptName || app.localize('NewDepartment');
            } else {
                var targetDeptId = _migrationData.targetDepartmentId || modal.find('#targetDepartmentId').val();
                if (targetDeptId) {
                    // Try to get department name from the dropdown option text
                    var $selectedOption = modal.find('#targetDepartmentId option:selected');
                    if ($selectedOption.length && $selectedOption.text() !== '') {
                        return $selectedOption.text();
                    }
                    
                    // Try to find department in cached data
                    var dept = findDepartmentById(targetDeptId);
                    if (dept) {
                        return dept.DepartmentName || dept.departmentName;
                    }
                    
                    // Try to get from analysis data
                    if (_migrationData.analysis && _migrationData.analysis.targetDepartmentName) {
                        return _migrationData.analysis.targetDepartmentName;
                    }
                    
                    // Fallback to ID if name not found
                    return app.localize('SelectedDepartment') + ' (ID: ' + targetDeptId + ')';
                } else {
                    return app.localize('SelectedDepartment');
                }
            }
        }

        function saveWizardState() {
            // Save current wizard state to migration data
            var modal = _modalManager.getModal();
            
            _migrationData.currentStep = _currentStep;
            
            // Save cohort information - use stored values or form inputs as fallback
            _migrationData.cohortId = _cohortInfo.cohortId || modal.find('input[name="cohortId"]').val();
            _migrationData.cohortName = _cohortInfo.cohortName || getSourceCohortName();
            
            if (_currentStep >= 1) {
                // Save step 1 data
                _migrationData.departmentOption = modal.find('input[name="departmentOption"]:checked').val();
                _migrationData.targetDepartmentId = modal.find('#targetDepartmentId').val();
                _migrationData.newDepartmentName = modal.find('#newDepartmentName').val();
                _migrationData.newDepartmentDescription = modal.find('#newDepartmentDescription').val();
                
                // Save target department name for display
                if (_migrationData.departmentOption === 'existing' && _migrationData.targetDepartmentId) {
                    var $selectedOption = modal.find('#targetDepartmentId option:selected');
                    if ($selectedOption.length) {
                        _migrationData.targetDepartmentName = $selectedOption.text();
                    }
                }
            }
            
            if (_currentStep >= 2 && _categoryData) {
                // Category mapping data is already saved in validateStep2
                _migrationData.categoryMappingComplete = true;
            }
        }

        function updateProgressIndicator() {
            var modal = _modalManager.getModal();
            
            // Update step items
            modal.find('.step-item').each(function(index) {
                var stepNumber = index + 1;
                var $stepItem = $(this);
                
                $stepItem.removeClass('active completed');
                
                if (stepNumber < _currentStep) {
                    $stepItem.addClass('completed');
                } else if (stepNumber === _currentStep) {
                    $stepItem.addClass('active');
                }
            });
            
            // Update progress bar
            var progressPercentage = ((_currentStep - 1) / (_maxSteps - 1)) * 100;
            modal.find('.progress-bar').css('width', progressPercentage + '%').attr('aria-valuenow', progressPercentage);
        }

        function updateNavigationButtons() {
            var modal = _modalManager.getModal();
            var prevBtn = modal.find('#prevStepBtn');
            var nextBtn = modal.find('#nextStepBtn');
            var executeBtn = modal.find('#executeBtn');
            
            // Previous button
            if (_currentStep > 1) {
                prevBtn.show();
            } else {
                prevBtn.hide();
            }
            
            // Next/Execute buttons
            if (_currentStep < _maxSteps) {
                nextBtn.show();
                executeBtn.hide();
            } else {
                nextBtn.hide();
                executeBtn.show();
            }
        }

        function executeMigration() {
            if (!validateStep3()) {
                return;
            }

            // Collect all migration data
            var migrationRequest = prepareMigrationRequest();
            
            // Show confirmation dialog with impact summary
            var confirmationMessage = buildExecutionConfirmationMessage();
            
            abp.message.confirm(
                confirmationMessage,
                app.localize('ConfirmMigrationExecution'),
                function(isConfirmed) {
                    if (isConfirmed) {
                        executeConfirmedMigration(migrationRequest);
                    }
                }
            );
        }

        function prepareMigrationRequest() {
            var modal = _modalManager.getModal();
            
            return {
                CohortId: modal.find('input[name="cohortId"]').val(),
                DepartmentOption: _migrationData.departmentOption,
                TargetDepartmentId: _migrationData.targetDepartmentId,
                NewDepartmentName: _migrationData.newDepartmentName,
                NewDepartmentDescription: _migrationData.newDepartmentDescription,
                CategoryMappings: _migrationData.categoryMappings,
                NoMigrationRequiredCategories: _noMigrationRequiredCategories,
                AnalysisData: _migrationData.analysis,
                ImpactSummary: _migrationData.impactSummary,
                UserConfirmation: _migrationData.finalConfirmation
            };
        }

        function buildExecutionConfirmationMessage() {
            var impact = _migrationData.impactSummary;
            var message = app.localize('FinalMigrationConfirmation') + '\n\n';
            
            message += app.localize('AffectedUsers') + ': ' + impact.totalAffectedUsers + '\n';
            message += app.localize('AffectedRecords') + ': ' + impact.totalAffectedRecords + '\n';
            message += app.localize('TotalCategories') + ': ' + impact.totalCategories;
            if (_noMigrationRequiredCategories && _noMigrationRequiredCategories.length > 0) {
                message += ' (' + _noMigrationRequiredCategories.length + ' ' + app.localize('NoMigrationRequired') + ')';
            }
            message += '\n\n';
            
            message += app.localize('Actions') + ':\n';
            message += '• ' + app.localize('MappedToExisting') + ': ' + impact.mappedCategories + '\n';
            message += '• ' + app.localize('CopiedAsNew') + ': ' + impact.copiedCategories + '\n';
            message += '• ' + app.localize('Skipped') + ': ' + impact.skippedCategories + '\n';
            if (_noMigrationRequiredCategories && _noMigrationRequiredCategories.length > 0) {
                message += '• ' + app.localize('NoMigrationRequired') + ': ' + _noMigrationRequiredCategories.length + '\n';
            }
            message += '\n';
            
            if (impact.dataLossWarnings > 0) {
                message += '⚠️ ' + app.localize('DataLossWarning') + ': ' + impact.dataLossWarnings + ' ' + app.localize('CategoriesWillLoseData') + '\n\n';
            }
            
            message += app.localize('ThisActionCannotBeUndone');
            
            return message;
        }

        function executeConfirmedMigration(migrationRequest) {
            var modal = _modalManager.getModal();
            
            // Disable all navigation and form elements
            disableWizardNavigation();
            
            // Show migration progress UI
            showMigrationProgress();
            
            // Convert our request to the standard CohortMigrationDto format
            var standardMigrationRequest = convertToStandardMigrationDto(migrationRequest);
            
            // Start migration execution using ABP service proxy
            _cohortMigrationService.executeMigration(standardMigrationRequest)
                .done(function(result) {
                    handleMigrationSuccess(result);
                })
                .fail(function(error) {
                    var errorMessage = error.message || app.localize('MigrationFailed');
                    handleMigrationError(errorMessage);
                });
        }

        function convertToStandardMigrationDto(wizardRequest) {
            // Convert our wizard request format to the standard CohortMigrationDto
            var categoryMappings = (wizardRequest.CategoryMappings || []).map(function(mapping) {
                // Convert string action to enum value
                var action;
                switch (mapping.Action) {
                    case 'map':
                        action = 1; // MapToExisting
                        break;
                    case 'copy':
                        action = 2; // CopyToNew
                        break;
                    case 'skip':
                        action = 3; // Skip
                        break;
                    default:
                        action = 1; // Default to MapToExisting
                        break;
                }
                
                return {
                    SourceCategoryId: mapping.SourceCategoryId,
                    SourceCategoryName: mapping.SourceCategoryName,
                    SourceRequirementName: mapping.SourceRequirementName,
                    SourceRequirementId: mapping.SourceRequirementId,
                    Action: action, // Use enum value instead of string
                    TargetCategoryId: mapping.TargetCategoryId,
                    NewRequirementName: mapping.NewRequirementName,
                    NewCategoryName: mapping.NewCategoryName,
                    AffectedRecordStatesCount: mapping.AffectedRecordStatesCount,
                    AffectedUsersCount: mapping.AffectedUsersCount,
                    HasDataLoss: mapping.HasDataLoss
                };
            });
            
            return {
                CohortId: wizardRequest.CohortId,
                TargetDepartmentId: wizardRequest.DepartmentOption === 'existing' ? wizardRequest.TargetDepartmentId : null,
                NewDepartmentName: wizardRequest.DepartmentOption === 'new' ? wizardRequest.NewDepartmentName : null,
                NewDepartmentDescription: wizardRequest.DepartmentOption === 'new' ? wizardRequest.NewDepartmentDescription : null,
                CategoryMappings: categoryMappings,
                ConfirmMigration: true
            };
        }

        function disableWizardNavigation() {
            var modal = _modalManager.getModal();
            
            // Disable all buttons
            modal.find('#prevStepBtn, #nextStepBtn, #executeBtn').prop('disabled', true);
            modal.find('.btn-close, [data-bs-dismiss="modal"]').prop('disabled', true);
            
            // Disable form elements
            modal.find('input, select, textarea, button').not('#migrationProgressContainer *').prop('disabled', true);
        }

        function enableWizardNavigation() {
            var modal = _modalManager.getModal();
            
            // Re-enable buttons
            modal.find('#prevStepBtn, #nextStepBtn, #executeBtn').prop('disabled', false);
            modal.find('.btn-close, [data-bs-dismiss="modal"]').prop('disabled', false);
            
            // Re-enable form elements
            modal.find('input, select, textarea, button').prop('disabled', false);
        }

        function showMigrationProgress() {
            var modal = _modalManager.getModal();
            
            var progressHtml = '<div id="migrationProgressContainer" class="migration-progress-overlay">' +
                '<div class="migration-progress-content">' +
                '<div class="text-center mb-4">' +
                '<div class="migration-progress-icon">' +
                '<i class="fa fa-cogs fa-3x text-primary"></i>' +
                '</div>' +
                '<h4 class="mt-3">' + app.localize('MigrationInProgress') + '</h4>' +
                '<p class="text-muted">' + app.localize('PleaseWaitMigrationProcessing') + '</p>' +
                '</div>' +
                
                '<div class="migration-progress-steps">' +
                '<div class="progress-step" id="step-validation">' +
                '<div class="progress-step-icon"><i class="fa fa-check-circle"></i></div>' +
                '<div class="progress-step-text">' + app.localize('ValidatingMigrationData') + '</div>' +
                '</div>' +
                '<div class="progress-step" id="step-preparation">' +
                '<div class="progress-step-icon"><i class="fa fa-spinner fa-spin"></i></div>' +
                '<div class="progress-step-text">' + app.localize('PreparingMigration') + '</div>' +
                '</div>' +
                '<div class="progress-step" id="step-execution">' +
                '<div class="progress-step-icon"><i class="fa fa-circle-o"></i></div>' +
                '<div class="progress-step-text">' + app.localize('ExecutingMigration') + '</div>' +
                '</div>' +
                '<div class="progress-step" id="step-completion">' +
                '<div class="progress-step-icon"><i class="fa fa-circle-o"></i></div>' +
                '<div class="progress-step-text">' + app.localize('CompletingMigration') + '</div>' +
                '</div>' +
                '</div>' +
                
                '<div class="progress mt-4">' +
                '<div id="migrationProgressBar" class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" style="width: 25%" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100"></div>' +
                '</div>' +
                
                '<div class="migration-progress-details mt-3">' +
                '<div id="migrationProgressText" class="text-center text-muted">' + app.localize('InitializingMigration') + '</div>' +
                '</div>' +
                
                '</div>' +
                '</div>';
            
            modal.find('.modal-body').append(progressHtml);
            
            // Start progress simulation
            simulateMigrationProgress();
        }

        function simulateMigrationProgress() {
            var currentProgress = 25;
            var progressInterval = setInterval(function() {
                currentProgress += Math.random() * 15;
                
                if (currentProgress >= 50 && currentProgress < 75) {
                    updateMigrationProgressStep('step-preparation', 'completed');
                    updateMigrationProgressStep('step-execution', 'active');
                    updateMigrationProgressText(app.localize('MigratingCohortData'));
                } else if (currentProgress >= 75 && currentProgress < 95) {
                    updateMigrationProgressStep('step-execution', 'completed');
                    updateMigrationProgressStep('step-completion', 'active');
                    updateMigrationProgressText(app.localize('FinalizingMigration'));
                }
                
                if (currentProgress >= 95) {
                    currentProgress = 95; // Cap at 95% until actual completion
                    clearInterval(progressInterval);
                }
                
                updateMigrationProgressBar(Math.min(currentProgress, 95));
            }, 1000);
            
            // Store interval for cleanup
            _migrationData.progressInterval = progressInterval;
        }

        function updateMigrationProgressStep(stepId, status) {
            var modal = _modalManager.getModal();
            var $step = modal.find('#' + stepId);
            
            $step.removeClass('active completed');
            $step.addClass(status);
            
            var $icon = $step.find('.progress-step-icon i');
            $icon.removeClass('fa-spinner fa-spin fa-circle-o fa-check-circle');
            
            if (status === 'active') {
                $icon.addClass('fa-spinner fa-spin');
            } else if (status === 'completed') {
                $icon.addClass('fa-check-circle');
            } else {
                $icon.addClass('fa-circle-o');
            }
        }

        function updateMigrationProgressBar(percentage) {
            var modal = _modalManager.getModal();
            modal.find('#migrationProgressBar').css('width', percentage + '%').attr('aria-valuenow', percentage);
        }

        function updateMigrationProgressText(text) {
            var modal = _modalManager.getModal();
            modal.find('#migrationProgressText').text(text);
        }

        function handleMigrationSuccess(migrationResult) {
            var modal = _modalManager.getModal();
            
            // Clear progress interval
            if (_migrationData.progressInterval) {
                clearInterval(_migrationData.progressInterval);
            }
            
            // Complete all steps
            updateMigrationProgressStep('step-completion', 'completed');
            updateMigrationProgressBar(100);
            updateMigrationProgressText(app.localize('MigrationCompletedSuccessfully'));
            
            // Show success UI
            setTimeout(function() {
                showMigrationSuccessUI(migrationResult);
            }, 1500);
        }

        function showMigrationSuccessUI(migrationResult) {
            var modal = _modalManager.getModal();
            
            // Calculate duration from start/end times if available
            var duration = 0;
            if (migrationResult.MigrationStartTime && migrationResult.MigrationEndTime) {
                var startTime = new Date(migrationResult.MigrationStartTime);
                var endTime = new Date(migrationResult.MigrationEndTime);
                duration = Math.round((endTime - startTime) / 1000); // Duration in seconds
            }
            
            var successHtml = '<div class="migration-success-container text-center">' +
                '<div class="migration-success-icon mb-4">' +
                '<i class="fa fa-check-circle fa-5x text-success"></i>' +
                '</div>' +
                '<h3 class="text-success mb-3">' + app.localize('MigrationCompleted') + '</h3>' +
                '<p class="lead mb-4">' + (migrationResult.Message || app.localize('CohortMigrationSuccessful')) + '</p>' +
                
                '<div class="migration-results-summary">' +
                '<div class="row">' +
                '<div class="col-md-3">' +
                '<div class="result-stat">' +
                '<span class="result-number">' + (migrationResult.AffectedUsersCount || 0) + '</span>' +
                '<small>' + app.localize('UsersProcessed') + '</small>' +
                '</div>' +
                '</div>' +
                '<div class="col-md-3">' +
                '<div class="result-stat">' +
                '<span class="result-number">' + (migrationResult.MigratedRecordStatesCount || 0) + '</span>' +
                '<small>' + app.localize('RecordsProcessed') + '</small>' +
                '</div>' +
                '</div>' +
                '<div class="col-md-3">' +
                '<div class="result-stat">' +
                '<span class="result-number">' + (_migrationData.categoryMappings ? _migrationData.categoryMappings.length : 0) + '</span>' +
                '<small>' + app.localize('CategoriesMapped') + '</small>' +
                '</div>' +
                '</div>' +
                '<div class="col-md-3">' +
                '<div class="result-stat">' +
                '<span class="result-number">' + formatDuration(duration) + '</span>' +
                '<small>' + app.localize('Duration') + '</small>' +
                '</div>' +
                '</div>' +
                '</div>' +
                '</div>';
                
            // Show warnings if any
            if (migrationResult.Warnings && migrationResult.Warnings.length > 0) {
                successHtml += '<div class="alert alert-warning mt-3">' +
                    '<h6 class="alert-heading">' + app.localize('MigrationWarnings') + '</h6>' +
                    '<ul class="mb-0">';
                migrationResult.Warnings.forEach(function(warning) {
                    successHtml += '<li>' + warning + '</li>';
                });
                successHtml += '</ul></div>';
            }
                
            successHtml += '<div class="migration-actions mt-4">' +
                '<button type="button" class="btn btn-primary me-2" id="viewMigratedCohortBtn">' +
                '<i class="fa fa-eye"></i> ' + app.localize('ViewMigratedCohort') +
                '</button>' +
                '<button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">' +
                '<i class="fa fa-times"></i> ' + app.localize('Close') +
                '</button>' +
                '</div>' +
                '</div>';
            
            modal.find('#migrationProgressContainer').html(successHtml);
            
            // Add event handler for view cohort button
            modal.find('#viewMigratedCohortBtn').on('click', function() {
                // Navigate to the migrated cohort - use the original cohort ID since it's been migrated
                var cohortId = modal.find('input[name="cohortId"]').val();
                window.location.href = abp.appPath + 'App/Cohorts?cohortId=' + cohortId;
            });
        }

        function handleMigrationError(errorMessage) {
            var modal = _modalManager.getModal();
            
            // Clear progress interval
            if (_migrationData.progressInterval) {
                clearInterval(_migrationData.progressInterval);
            }
            
            // Show error UI
            var errorHtml = '<div class="migration-error-container text-center">' +
                '<div class="migration-error-icon mb-4">' +
                '<i class="fa fa-exclamation-triangle fa-5x text-danger"></i>' +
                '</div>' +
                '<h3 class="text-danger mb-3">' + app.localize('MigrationFailed') + '</h3>' +
                '<p class="lead mb-4">' + app.localize('MigrationEncounteredError') + '</p>' +
                
                '<div class="alert alert-danger text-start">' +
                '<h6 class="alert-heading">' + app.localize('ErrorDetails') + '</h6>' +
                '<p class="mb-0">' + errorMessage + '</p>' +
                '</div>' +
                
                '<div class="migration-error-actions mt-4">' +
                '<button type="button" class="btn btn-primary me-2" id="retryMigrationBtn">' +
                '<i class="fa fa-refresh"></i> ' + app.localize('RetryMigration') +
                '</button>' +
                '<button type="button" class="btn btn-outline-secondary me-2" id="backToMappingBtn">' +
                '<i class="fa fa-arrow-left"></i> ' + app.localize('BackToMapping') +
                '</button>' +
                '<button type="button" class="btn btn-outline-danger" data-bs-dismiss="modal">' +
                '<i class="fa fa-times"></i> ' + app.localize('Cancel') +
                '</button>' +
                '</div>' +
                '</div>';
            
            modal.find('#migrationProgressContainer').html(errorHtml);
            
            // Re-enable navigation
            enableWizardNavigation();
            
            // Add event handlers
            modal.find('#retryMigrationBtn').on('click', function() {
                modal.find('#migrationProgressContainer').remove();
                executeMigration();
            });
            
            modal.find('#backToMappingBtn').on('click', function() {
                modal.find('#migrationProgressContainer').remove();
                _currentStep = 2;
                updateStepDisplay();
            });
        }

        function formatDuration(seconds) {
            if (seconds < 60) {
                return seconds + 's';
            } else if (seconds < 3600) {
                return Math.floor(seconds / 60) + 'm ' + (seconds % 60) + 's';
            } else {
                var hours = Math.floor(seconds / 3600);
                var minutes = Math.floor((seconds % 3600) / 60);
                return hours + 'h ' + minutes + 'm';
            }
        }

        // Public methods
        this.save = function () {
            // This will be called when the save button is clicked
            // For now, we'll handle this through the wizard navigation
        };
    };
})(jQuery); 