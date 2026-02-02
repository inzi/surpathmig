(function () {
    $(function () {
        var _tenantSurpathServicesService = abp.services.app.tenantSurpathServices;
        var _$servicesContainer = $('#ServicesContainer');
        var _$loadingIndicator = $('#LoadingIndicator');
        var _selectedTenantId = $('#TenantSelect').val() || null;
        var _isHost = abp.session.multiTenancySide === 0; // Host = 0
        var _hierarchicalData = null;
        var _expandedStates = {};
        var _updateTimer = {};
        var _currentServiceId = null;
        var _availableSurpathServices = null; // Cache available services
        var _$totalCostContainer = $('#TotalCostContainer');
        var _currentTab = 'pricing'; // Track current tab
        
        // Modal for creating pricing overrides
        var _createPricingModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/TenantSurpathServices/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/TenantSurpathServices/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditTenantSurpathServiceModal'
        });
        
        // Initialize
        function init() {
            if (!_isHost) {
                // For tenant users, load data immediately
                _selectedTenantId = abp.session.tenantId;
                loadPricingData();
            }
            bindEvents();
            setupModalHandlers();
        }
        
        // Event Bindings
        function bindEvents() {
            $('#TenantSelect').on('change', function() {
                _selectedTenantId = $(this).val();
                if (_selectedTenantId) {
                    loadPricingData();
                } else {
                    _$servicesContainer.html('<div class="no-data"><p>' + app.localize('SelectTenantToViewPricing') + '</p></div>');
                    _$totalCostContainer.html('<div class="no-data"><p>' + app.localize('SelectTenantToViewCosts') + '</p></div>');
                }
            });
            
            // Tab switching event
            $('a[data-bs-toggle="tab"]').on('shown.bs.tab', function (e) {
                var targetTab = $(e.target).attr('href');
                if (targetTab === '#TotalCostTab') {
                    _currentTab = 'cost';
                    if (_hierarchicalData) {
                        renderTotalCosts();
                    }
                } else {
                    _currentTab = 'pricing';
                }
            });
            
            // Add override button
            _$servicesContainer.on('click', '.add-override-btn', function() {
                var serviceId = $(this).data('service-id');
                var serviceName = $(this).data('service-name');
                _currentServiceId = serviceId;
                
                // Open modal for creating new pricing override
                _createPricingModal.open({
                    id: null,
                    surpathServiceId: serviceId,
                    tenantId: parseInt(_selectedTenantId)
                });
            });
            
            // Enable service button
            _$servicesContainer.on('click', '.enable-service-btn', function() {
                var serviceId = $(this).data('service-id');
                var serviceName = $(this).data('service-name');
                _currentServiceId = serviceId;
                
                // Open modal to create initial tenant-level pricing
                _createPricingModal.open({
                    id: null,
                    surpathServiceId: serviceId,
                    tenantId: parseInt(_selectedTenantId),
                    isInitialSetup: true
                });
            });
            
            // Expand/collapse handling
            _$servicesContainer.on('click', '.expand-icon', function(e) {
                e.stopPropagation();
                var $this = $(this);
                var $row = $this.closest('.pricing-row');
                var targetId = $row.data('target');
                
                $this.toggleClass('expanded');
                
                if ($this.hasClass('expanded')) {
                    $('.' + targetId).show();
                    _expandedStates[targetId] = true;
                } else {
                    $('.' + targetId).hide();
                    // Also hide all children
                    $('.' + targetId).find('.expand-icon').removeClass('expanded');
                    $('.' + targetId).find('[class*="children-"]').hide();
                    _expandedStates[targetId] = false;
                }
            });
            
            // Click row to expand (if it has children)
            _$servicesContainer.on('click', '.pricing-row.collapsible', function(e) {
                if (!$(e.target).is('input') && !$(e.target).hasClass('clear-price') && !$(e.target).hasClass('price-edit-btn')) {
                    $(this).find('.expand-icon').first().click();
                }
            });
            
            // Inline price editing
            _$servicesContainer.on('focus', '.price-input', function() {
                $(this).select();
            });
            
            _$servicesContainer.on('input', '.price-input', function() {
                var $input = $(this);
                var newValue = $input.val();
                
                // Update visual state
                if (newValue && newValue !== '') {
                    $input.removeClass('inherited').addClass('custom');
                } else {
                    $input.removeClass('custom').addClass('inherited');
                }
            });
            
            // Clear price
            _$servicesContainer.on('click', '.clear-price', function(e) {
                e.stopPropagation();
                var $wrapper = $(this).siblings('.price-edit-wrapper');
                var $input = $wrapper.find('.price-input');
                
                // Save current expanded states before clearing
                saveExpandedStates();
                
                // Clear the value and save
                $input.val('');
                savePriceChange($input);
            });
            
            // Edit button click
            _$servicesContainer.on('click', '.price-edit-btn', function(e) {
                e.stopPropagation();
                var $btn = $(this);
                var $wrapper = $btn.siblings('.price-edit-wrapper');
                var $clearBtn = $btn.siblings('.clear-price');
                
                $btn.hide();
                $clearBtn.hide();
                $wrapper.show();
                $wrapper.find('.price-input').focus();
            });
            
            // Save button click
            _$servicesContainer.on('click', '.price-save-btn', function(e) {
                e.stopPropagation();
                var $wrapper = $(this).closest('.price-edit-wrapper');
                var $input = $wrapper.find('.price-input');
                savePriceChange($input);
            });
            
            // Cancel button click
            _$servicesContainer.on('click', '.price-cancel-btn', function(e) {
                e.stopPropagation();
                var $wrapper = $(this).closest('.price-edit-wrapper');
                var $input = $wrapper.find('.price-input');
                var $enabledToggle = $wrapper.find('.price-enabled-toggle');
                var $btn = $wrapper.siblings('.price-edit-btn');
                var $clearBtn = $wrapper.siblings('.clear-price');
                
                // Restore original values
                $input.val($input.data('original-value'));
                if ($enabledToggle.length > 0) {
                    $enabledToggle.prop('checked', $input.data('original-enabled') === 'true' || $input.data('original-enabled') === true);
                }
                
                // Hide edit wrapper and show edit button
                $wrapper.hide();
                $btn.show();
                
                // Show clear button if there's a custom price
                if ($input.data('original-value')) {
                    $clearBtn.show();
                }
            });
        }
        
        // Load hierarchical pricing data
        function loadPricingData() {
            if (!_selectedTenantId) return;
            
            _$loadingIndicator.show();
            _$servicesContainer.empty();
            
            // First load available SurpathServices if not already loaded
            var servicesPromise = _availableSurpathServices 
                ? $.Deferred().resolve(_availableSurpathServices)
                : _tenantSurpathServicesService.getAvailableSurpathServices()
                    .done(function(services) {
                        _availableSurpathServices = services;
                    });
            
            // Then load hierarchical pricing
            servicesPromise.then(function() {
                return _tenantSurpathServicesService.getHierarchicalPricing({
                    tenantId: parseInt(_selectedTenantId),
                    surpathServiceId: null // Get all services
                });
            }).done(function(result) {
                _hierarchicalData = result;
                renderServices(result);
                
                // If we're on the cost tab, render costs too
                if (_currentTab === 'cost') {
                    renderTotalCosts();
                }
            }).always(function() {
                _$loadingIndicator.hide();
            });
        }
        
        // Render all services
        function renderServices(data) {
            if (!data || !data.tenant || !_availableSurpathServices) {
                _$servicesContainer.html('<div class="no-data"><p>' + app.localize('NoDataFound') + '</p></div>');
                return;
            }
            
            // Debug: Log the raw data
            console.log('Raw hierarchical data:', data);
            
            var html = '';
            
            // Create a map of tenant services by service ID
            var tenantServices = data.tenant.services || [];
            var tenantServiceMap = {};
            tenantServices.forEach(function(s) {
                tenantServiceMap[s.serviceId] = s;
            });
            
            // Render each available SurpathService
            _availableSurpathServices.forEach(function(surpathService) {
                var tenantService = tenantServiceMap[surpathService.id];
                
                if (tenantService) {
                    // Service is configured for this tenant (enabled or disabled)
                    html += renderServiceSection(tenantService, data, surpathService.name);
                } else {
                    // Service is not configured for this tenant
                    html += renderPlaceholderServiceSection(surpathService.name, data, surpathService);
                }
            });
            
            _$servicesContainer.html(html);
            
            // Restore expanded states
            for (var key in _expandedStates) {
                if (_expandedStates[key]) {
                    $('[data-target="' + key + '"]').find('.expand-icon').addClass('expanded');
                    $('.' + key).show();
                }
            }
        }
        
        // Render a service section
        function renderServiceSection(service, data, serviceName) {
            var html = '<div class="service-section">';
            
            // Service header
            html += '<div class="service-header">';
            html += '<h3 class="service-title">' + serviceName + '</h3>';
            html += '<div class="service-actions">';
            html += '<div class="service-price">Price: ' + formatPrice(service.effectivePrice || service.price) + '</div>';
            html += '<button class="btn btn-sm btn-outline-primary add-override-btn" data-service-id="' + service.serviceId + '" data-service-name="' + serviceName + '">';
            html += '<i class="fa fa-plus"></i> Add Override';
            html += '</button>';
            html += '</div>';
            html += '</div>';
            
            // Pricing tree
            html += '<div class="pricing-tree ' + (!service.isPricingOverrideEnabled ? 'service-disabled' : '') + '">';
            html += renderPricingHierarchy(data, service);
            html += '</div>';
            
            html += '</div>';
            
            return html;
        }
        
        // Render placeholder service section
        function renderPlaceholderServiceSection(serviceName, data, surpathService) {
            // Find the surpath service if not provided
            if (!surpathService) {
                surpathService = _availableSurpathServices.find(function(s) { 
                    return s.name === serviceName; 
                });
            }
            
            var html = '<div class="service-section">';
            
            // Service header
            html += '<div class="service-header">';
            html += '<h3 class="service-title">' + serviceName + '</h3>';
            html += '<div class="service-actions">';
            html += '<div class="service-price">Not Configured</div>';
            if (surpathService) {
                html += '<button class="btn btn-sm btn-primary enable-service-btn" data-service-id="' + surpathService.id + '" data-service-name="' + serviceName + '">';
                html += '<i class="fa fa-plus"></i> Enable Service';
                html += '</button>';
            }
            html += '</div>';
            html += '</div>';
            
            html += '<div class="pricing-tree">';
            html += '<div class="no-data" style="padding: 20px;"><p>Service not enabled for this tenant</p></div>';
            html += '</div>';
            
            html += '</div>';
            
            return html;
        }
        
        // Render pricing hierarchy
        function renderPricingHierarchy(data, service) {
            var html = '';
            
            // Tenant level
            html += '<div class="pricing-row tenant-level">';
            html += '<div class="entity-info">';
            html += '<span class="entity-name">' + escapeHtml(data.tenant.name) + '</span>';
            html += '<span class="entity-label label-tenant">Tenant</span>';
            html += '</div>';
            html += '<div class="price-input-wrapper">';
            html += renderPriceInput('tenant', data.tenant.id, service, service);
            html += '</div>';
            html += '</div>';
            
            // Show ALL departments
            if (data.departments && data.departments.length > 0) {
                data.departments.forEach(function(dept) {
                    html += renderDepartmentRow(dept, service);
                });
            } else {
                html += '<div class="pricing-row" style="padding-left: 40px; font-style: italic; color: #b5b5c3;">';
                html += '<small>No departments found.</small>';
                html += '</div>';
            }
            
            // Show standalone cohorts (cohorts that don't belong to any department)
            if (data.cohorts && data.cohorts.length > 0) {
                data.cohorts.forEach(function(cohort) {
                    html += renderStandaloneCohortRow(cohort, service);
                });
            }
            
            return html;
        }
        
        // Render department row
        function renderDepartmentRow(dept, baseService) {
            var html = '';
            var deptService = dept.services ? dept.services.find(s => s.serviceId === baseService.serviceId) : null;
            var hasChildren = dept.cohorts && dept.cohorts.length > 0;
            
            // Debug department services
            console.log('Department:', dept.name, 'Services:', dept.services);
            console.log('Looking for service:', baseService.serviceId);
            console.log('Found department service:', deptService);
            
            var deptId = 'dept-' + dept.id + '-service-' + baseService.serviceId;
            
            html += '<div class="pricing-row department-level' + (hasChildren ? ' collapsible' : '') + '" data-target="' + deptId + '-children">';
            html += '<div class="entity-info">';
            if (hasChildren) {
                html += '<i class="fa fa-chevron-right expand-icon"></i>';
            }
            html += '<span class="entity-name">' + escapeHtml(dept.name) + '</span>';
            html += '<span class="entity-label label-department">Department</span>';
            html += '</div>';
            html += '<div class="price-input-wrapper">';
            html += renderPriceInput('department', dept.id, baseService, deptService);
            html += '</div>';
            html += '</div>';
            
            // Show ALL cohorts
            if (hasChildren) {
                dept.cohorts.forEach(function(cohort) {
                    html += renderCohortRow(cohort, baseService, deptId + '-children');
                });
            }
            
            return html;
        }
        
        // Render standalone cohort row (cohorts that don't belong to departments)
        function renderStandaloneCohortRow(cohort, baseService) {
            var html = '';
            var cohortService = cohort.services ? cohort.services.find(s => s.serviceId === baseService.serviceId) : null;
            var hasChildren = cohort.users && cohort.users.length > 0;
            
            var cohortId = 'standalone-cohort-' + cohort.id + '-service-' + baseService.serviceId;
            
            html += '<div class="pricing-row cohort-level standalone-cohort' + (hasChildren ? ' collapsible' : '') + '" data-target="' + cohortId + '-children">';
            html += '<div class="entity-info">';
            if (hasChildren) {
                html += '<i class="fa fa-chevron-right expand-icon"></i>';
            }
            html += '<span class="entity-name">' + escapeHtml(cohort.name) + '</span>';
            html += '<span class="entity-label label-cohort">Cohort (No Dept)</span>';
            html += '</div>';
            html += '<div class="price-input-wrapper">';
            html += renderPriceInput('cohort', cohort.id, baseService, cohortService);
            html += '</div>';
            html += '</div>';
            
            // Show ALL users
            if (hasChildren) {
                cohort.users.forEach(function(user) {
                    html += renderUserRow(user, baseService, cohortId + '-children');
                });
            }
            
            return html;
        }
        
        // Render cohort row
        function renderCohortRow(cohort, baseService, parentClass) {
            var html = '';
            var cohortService = cohort.services ? cohort.services.find(s => s.serviceId === baseService.serviceId) : null;
            var hasChildren = cohort.users && cohort.users.length > 0;
            
            var cohortId = 'cohort-' + cohort.id + '-service-' + baseService.serviceId;
            
            html += '<div class="pricing-row cohort-level ' + parentClass + (hasChildren ? ' collapsible' : '') + '" data-target="' + cohortId + '-children" style="display:none;">';
            html += '<div class="entity-info">';
            if (hasChildren) {
                html += '<i class="fa fa-chevron-right expand-icon"></i>';
            }
            html += '<span class="entity-name">' + escapeHtml(cohort.name) + '</span>';
            html += '<span class="entity-label label-cohort">Cohort</span>';
            html += '</div>';
            html += '<div class="price-input-wrapper">';
            html += renderPriceInput('cohort', cohort.id, baseService, cohortService);
            html += '</div>';
            html += '</div>';
            
            // Show ALL users (limit to first 2 if there are many)
            if (hasChildren) {
                var usersToShow = cohort.users.length > 2 ? 2 : cohort.users.length;
                for (var i = 0; i < usersToShow; i++) {
                    html += renderUserRow(cohort.users[i], baseService, cohortId + '-children ' + parentClass);
                }
                
                if (cohort.users.length > 2) {
                    html += '<div class="pricing-row user-level ' + cohortId + '-children ' + parentClass + '" style="display:none; padding-left: 100px; font-style: italic; color: #b5b5c3;">';
                    html += '<small>Users (' + cohort.users.length + ')</small>';
                    html += '</div>';
                }
            }
            
            return html;
        }
        
        // Render user row
        function renderUserRow(user, baseService, parentClass) {
            var html = '';
            var userService = user.services ? user.services.find(s => s.serviceId === baseService.serviceId) : null;
            
            html += '<div class="pricing-row user-level ' + parentClass + '" style="display:none;">';
            html += '<div class="entity-info">';
            html += '<span class="entity-name">' + escapeHtml(user.fullName) + '</span>';
            html += '<span class="entity-label label-user">User</span>';
            html += '</div>';
            html += '<div class="price-input-wrapper">';
            html += renderPriceInput('user', user.id, baseService, userService);
            html += '</div>';
            html += '</div>';
            
            return html;
        }
        
        // Render price input
        function renderPriceInput(level, targetId, baseService, overrideService) {
            var inputId = level + '-' + targetId + '-service-' + baseService.serviceId;
            var currentPrice = overrideService ? overrideService.overridePrice : null;
            var inheritedPrice = overrideService ? overrideService.effectivePrice : (baseService.price || baseService.basePrice);
            var isInherited = currentPrice === null;
            var isEnabled = overrideService ? (overrideService.isPricingOverrideEnabled !== undefined ? overrideService.isPricingOverrideEnabled : true) : true;
            var tenantSurpathServiceId = overrideService && overrideService.tenantSurpathServiceId ? overrideService.tenantSurpathServiceId : '';
            
            // Debug logging
            if (level === 'department') {
                console.log('Department service:', {
                    level: level,
                    targetId: targetId,
                    baseService: baseService,
                    overrideService: overrideService,
                    tenantSurpathServiceId: tenantSurpathServiceId,
                    isEnabled: isEnabled,
                    currentPrice: currentPrice,
                    isInherited: isInherited
                });
            }
            
            var html = '';
            
            html += '<span class="price-value ' + (isInherited ? 'price-inherited' : 'price-custom') + '">' + formatPrice(isInherited ? inheritedPrice : currentPrice) + '</span>';
            
            // Show appropriate icon based on service configuration
            if (level !== 'tenant') {
                if (tenantSurpathServiceId) {
                    // TenantSurpathService exists at this level
                    if (isEnabled) {
                        html += '<i class="fa fa-exclamation-circle service-status-icon service-enabled" title="' + app.localize('ServiceConfiguredAndEnabled') + '"></i>';
                    } else {
                        html += '<i class="fa fa-exclamation-circle service-status-icon service-disabled" title="' + app.localize('ServiceConfiguredButDisabled') + '"></i>';
                    }
                } else {
                    // No TenantSurpathService at this level, show inherited icon
                    html += '<i class="fa fa-info-circle info-icon" title="' + app.localize('InheritedFromParent') + '"></i>';
                }
            }
            
            // Show enabled/disabled badge when service exists but is disabled
            if (tenantSurpathServiceId && !isEnabled) {
                html += '<span class="badge badge-warning ms-2">Disabled</span>';
            }
            
            // Show input and edit button for all levels
            html += '<div class="price-edit-wrapper" style="display:none;">';
            
            // Price input
            html += '<input type="number" step="0.01" min="0" ';
            html += 'id="' + inputId + '" ';
            html += 'class="price-input ' + (isInherited ? 'inherited' : 'custom') + '" ';
            html += 'value="' + (currentPrice !== null ? currentPrice : '') + '" ';
            html += 'placeholder="' + formatPrice(inheritedPrice) + '" ';
            html += 'data-level="' + level + '" ';
            html += 'data-target-id="' + targetId + '" ';
            html += 'data-service-id="' + baseService.serviceId + '" ';
            html += 'data-tenant-surpath-service-id="' + tenantSurpathServiceId + '" ';
            html += 'data-original-value="' + (currentPrice !== null ? currentPrice : '') + '" ';
            html += 'data-original-enabled="' + isEnabled + '" ';
            html += 'data-inherited-price="' + inheritedPrice + '" />';
            
            // Enable/Disable toggle (only show if service exists at this level)
            if (tenantSurpathServiceId) {
                html += '<div class="form-check form-switch ms-2">';
                html += '<input class="form-check-input price-enabled-toggle" type="checkbox" ';
                html += 'id="enabled-' + inputId + '" ';
                html += (isEnabled ? 'checked' : '') + '>';
                html += '<label class="form-check-label" for="enabled-' + inputId + '">';
                html += 'Enabled';
                html += '</label>';
                html += '</div>';
            }
            
            html += '<div class="price-edit-actions">';
            html += '<button class="btn btn-sm btn-success price-save-btn" title="' + app.localize('Save') + '">';
            html += '<i class="fa fa-check"></i>';
            html += '</button>';
            html += '<button class="btn btn-sm btn-secondary price-cancel-btn" title="' + app.localize('Cancel') + '">';
            html += '<i class="fa fa-times"></i>';
            html += '</button>';
            html += '</div>';
            html += '</div>';
            
            html += '<button class="price-edit-btn" title="' + app.localize('EditPrice') + '">';
            html += '<i class="fa fa-edit"></i> Edit';
            html += '</button>';
            
            if (!isInherited && level !== 'tenant') {
                html += '<span class="clear-price" title="' + app.localize('RemoveCustomPrice') + '">×</span>';
            }
            
            return html;
        }
        
        // Save price change
        function savePriceChange($input) {
            var level = $input.data('level');
            var targetId = $input.data('target-id');
            var serviceId = $input.data('service-id');
            var tenantSurpathServiceId = $input.data('tenant-surpath-service-id');
            var newPrice = $input.val() ? parseFloat($input.val()) : null;
            
            // Get the enabled status from the toggle if it exists
            var $editWrapper = $input.closest('.price-edit-wrapper');
            var $enabledToggle = $editWrapper.find('.price-enabled-toggle');
            var isEnabled = $enabledToggle.length > 0 ? $enabledToggle.is(':checked') : true;
            
            // For tenant level, null means 0
            if (level === 'tenant' && newPrice === null) {
                newPrice = 0;
            }
            
            // Save current expanded states before reload
            saveExpandedStates();
            
            // Get service name from the current row or service header
            var serviceName = '';
            var $serviceSection = $editWrapper.closest('.service-section');
            if ($serviceSection.length > 0) {
                serviceName = $serviceSection.find('.service-title').text();
            }
            
            // Prepare the DTO for CreateOrEdit
            var createOrEditDto = {
                id: tenantSurpathServiceId || null,
                name: serviceName || 'Service', // Use service name or default
                price: newPrice || 0,
                description: '', // Empty description for price updates
                isEnabled: isEnabled,
                surpathServiceId: serviceId
            };
            
            // Set the appropriate hierarchy fields based on level
            if (level === 'tenant') {
                // Tenant level - no additional fields needed
            } else if (level === 'department') {
                createOrEditDto.tenantDepartmentId = targetId;
            } else if (level === 'cohort') {
                createOrEditDto.cohortId = targetId;
            } else if (level === 'user') {
                createOrEditDto.userId = targetId;
            }
            
            // Use CreateOrEdit method
            _tenantSurpathServicesService.createOrEdit(createOrEditDto)
                .done(function() {
                    // Hide edit wrapper immediately
                    var $editBtn = $editWrapper.siblings('.price-edit-btn');
                    $editWrapper.hide();
                    $editBtn.show();
                    
                    abp.notify.success(app.localize('SuccessfullyUpdated'));
                    
                    // Reload data to reflect changes in child entities
                    loadPricingData();
                })
                .fail(function() {
                    $input.val($input.data('original-value'));
                    if ($enabledToggle.length > 0) {
                        var originalEnabled = $input.data('original-enabled');
                        $enabledToggle.prop('checked', originalEnabled === 'true' || originalEnabled === true);
                    }
                    abp.message.error(app.localize('ErrorSaving'));
                });
        }
        
        // Save current expanded states
        function saveExpandedStates() {
            _expandedStates = {};
            $('.expand-icon.expanded').each(function() {
                var $row = $(this).closest('.pricing-row');
                var targetId = $row.data('target');
                if (targetId) {
                    _expandedStates[targetId] = true;
                }
            });
        }
        
        // Format price for display
        function formatPrice(price) {
            if (price === null || price === undefined) return '$0.00';
            return '$' + parseFloat(price).toFixed(2);
        }
        
        // Escape HTML
        function escapeHtml(text) {
            if (!text) return '';
            var map = {
                '&': '&amp;',
                '<': '&lt;',
                '>': '&gt;',
                '"': '&quot;',
                "'": '&#039;'
            };
            return text.replace(/[&<>"']/g, function(m) { return map[m]; });
        }
        
        // Setup modal handlers
        function setupModalHandlers() {
            abp.event.on('app.createOrEditTenantSurpathServiceModalSaved', function() {
                loadPricingData();
            });
        }
        
        // Render total costs view
        function renderTotalCosts() {
            if (!_hierarchicalData || !_hierarchicalData.tenant || !_availableSurpathServices) {
                _$totalCostContainer.html('<div class="no-data"><p>' + app.localize('NoDataFound') + '</p></div>');
                return;
            }
            
            var html = '';
            
            // Calculate costs for each entity
            var tenantCosts = calculateEntityCosts(_hierarchicalData.tenant);
            
            // Tenant section
            html += '<div class="cost-summary-section">';
            html += '<div class="cost-summary-header">';
            html += '<h3 class="cost-summary-title">' + escapeHtml(_hierarchicalData.tenant.name) + '</h3>';
            html += '</div>';
            html += '<div class="pricing-tree">';
            
            // Tenant row
            html += renderCostRow('tenant', _hierarchicalData.tenant, tenantCosts);
            
            // Departments
            if (_hierarchicalData.departments && _hierarchicalData.departments.length > 0) {
                _hierarchicalData.departments.forEach(function(dept) {
                    var deptCosts = calculateEntityCosts(dept);
                    html += renderCostRow('department', dept, deptCosts);
                    
                    // Cohorts within department
                    if (dept.cohorts && dept.cohorts.length > 0) {
                        dept.cohorts.forEach(function(cohort) {
                            var cohortCosts = calculateEntityCosts(cohort);
                            html += renderCostRow('cohort', cohort, cohortCosts);
                            
                            // Users within cohort
                            if (cohort.users && cohort.users.length > 0) {
                                cohort.users.forEach(function(user) {
                                    var userCosts = calculateEntityCosts(user);
                                    html += renderCostRow('user', user, userCosts);
                                });
                            }
                        });
                    }
                });
            }
            
            // Standalone cohorts
            if (_hierarchicalData.cohorts && _hierarchicalData.cohorts.length > 0) {
                _hierarchicalData.cohorts.forEach(function(cohort) {
                    var cohortCosts = calculateEntityCosts(cohort);
                    html += renderCostRow('standalone-cohort', cohort, cohortCosts);
                    
                    // Users within standalone cohort
                    if (cohort.users && cohort.users.length > 0) {
                        cohort.users.forEach(function(user) {
                            var userCosts = calculateEntityCosts(user);
                            html += renderCostRow('user', user, userCosts);
                        });
                    }
                });
            }
            
            html += '</div>';
            html += '</div>';
            
            _$totalCostContainer.html(html);
        }
        
        // Calculate costs for an entity
        function calculateEntityCosts(entity) {
            var costs = {};
            var total = 0;
            
            // Get costs for each available service
            _availableSurpathServices.forEach(function(surpathService) {
                var entityService = entity.services ? entity.services.find(s => s.serviceId === surpathService.id) : null;
                if (entityService) {
                    var price = entityService.effectivePrice || entityService.price || 0;
                    costs[surpathService.id] = {
                        name: surpathService.name,
                        price: price
                    };
                    total += parseFloat(price);
                }
            });
            
            return {
                services: costs,
                total: total
            };
        }
        
        // Render cost row
        function renderCostRow(type, entity, costs) {
            var html = '';
            var levelClass = type + '-level';
            
            html += '<div class="cost-summary-row ' + levelClass + '">';
            html += '<div class="cost-entity-info">';
            
            // Add appropriate padding based on type
            if (type === 'department' || type === 'standalone-cohort') {
                html += '<span style="width: 20px;"></span>';
            } else if (type === 'cohort') {
                html += '<span style="width: 40px;"></span>';
            } else if (type === 'user') {
                html += '<span style="width: 60px;"></span>';
            }
            
            html += '<span class="entity-name">' + escapeHtml(entity.name || entity.fullName) + '</span>';
            html += '<span class="entity-label label-' + type + '">' + type.replace('-', ' ').toUpperCase() + '</span>';
            html += '</div>';
            
            html += '<div class="cost-breakdown">';
            
            // Show individual service costs
            for (var serviceId in costs.services) {
                var service = costs.services[serviceId];
                html += '<div class="cost-item">';
                html += '<div class="cost-service-name">' + escapeHtml(service.name) + '</div>';
                html += '<div class="cost-service-price">' + formatPrice(service.price) + '</div>';
                html += '</div>';
            }
            
            // Show total
            html += '<div class="total-cost">' + formatPrice(costs.total) + '</div>';
            
            html += '</div>';
            html += '</div>';
            
            return html;
        }
        
        // Initialize on document ready
        init();
    });
})();