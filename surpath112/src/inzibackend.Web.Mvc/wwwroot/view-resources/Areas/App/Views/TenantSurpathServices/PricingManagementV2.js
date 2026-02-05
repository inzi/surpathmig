(function () {
    $(function () {
        var _tenantSurpathServicesService = abp.services.app.tenantSurpathServices;
        var _$servicesContainer = $('#ServicesContainer');
        var _$loadingIndicator = $('#LoadingIndicator');
        var _selectedTenantId = $('#TenantSelect').val() || null;
        var _isHost = abp.session.multiTenancySide === 0; // Host = 0
        var _hierarchicalData = null;
        var _expandedStates = {};
        var _availableSurpathServices = null; // Cache available services
        var _$totalCostContainer = $('#TotalCostContainer');
        var _currentTab = 'pricing'; // Track current tab
        
        // Initialize
        function init() {
            console.log('PricingManagementV2 initializing...');
            console.log('Is Host:', _isHost);
            console.log('TenantSelect element exists:', $('#TenantSelect').length > 0);
            
            if (!_isHost) {
                // For tenant users, load data immediately
                _selectedTenantId = abp.session.tenantId;
                loadPricingData();
            }
            bindEvents();
        }
        
        // Event Bindings
        function bindEvents() {
            console.log('Binding events...');
            console.log('TenantSelect found:', $('#TenantSelect').length);
            
            $('#TenantSelect').on('change', function() {
                console.log('TenantSelect changed!');
                _selectedTenantId = $(this).val();
                console.log('Selected tenant ID:', _selectedTenantId);
                
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
            
            // Expand/collapse handling - click on row
            _$servicesContainer.on('click', '.pricing-row.collapsible', function(e) {
                // Don't expand/collapse if clicking on buttons or inputs
                if ($(e.target).closest('.price-edit-btn, .price-delete-btn, .price-edit-wrapper, .add-service-btn').length > 0) {
                    return;
                }
                
                e.stopPropagation();
                var $row = $(this);
                var nodeId = $row.data('node-id');
                var $expandIcon = $row.find('.expand-icon').first();
                
                $expandIcon.toggleClass('expanded');
                
                if ($expandIcon.hasClass('expanded')) {
                    $row.siblings('[data-parent-id="' + nodeId + '"]').show();
                    _expandedStates[nodeId] = true;
                } else {
                    // Hide all children recursively
                    hideChildren(nodeId);
                    _expandedStates[nodeId] = false;
                }
            });
            
            // Edit button click
            _$servicesContainer.on('click', '.price-edit-btn', function(e) {
                e.stopPropagation();
                var $btn = $(this);
                var $wrapper = $btn.siblings('.price-edit-wrapper');
                var $clearBtn = $btn.siblings('.price-delete-btn');
                
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
                var $clearBtn = $wrapper.siblings('.price-delete-btn');
                
                // Restore original values
                $input.val($input.data('original-value'));
                if ($enabledToggle.length > 0) {
                    $enabledToggle.prop('checked', $input.data('original-enabled') === 'true' || $input.data('original-enabled') === true);
                }
                var $invoicedToggle = $wrapper.find('.price-invoiced-toggle');
                if ($invoicedToggle.length > 0) {
                    $invoicedToggle.prop('checked', $input.data('original-invoiced') === 'true' || $input.data('original-invoiced') === true);
                }
                
                // Hide edit wrapper and show edit button
                $wrapper.hide();
                $btn.show();
                
                // Show clear button if there's a custom price
                if ($input.data('original-value')) {
                    $clearBtn.show();
                }
            });
            
            // Clear price (delete service)
            _$servicesContainer.on('click', '.price-delete-btn', function(e) {
                e.stopPropagation();
                var $clearBtn = $(this);
                var serviceId = $clearBtn.data('service-id');
                var $row = $clearBtn.closest('.pricing-row');
                var nodeType = $row.data('node-type');
                var entityName = $row.find('.entity-name').text();
                
                // Build confirmation message
                var message = 'Are you sure you want to remove this service override';
                if (nodeType && entityName) {
                    message += ' for ' + nodeType + ' "' + entityName + '"';
                }
                message += '?';
                
                abp.message.confirm(
                    message,
                    app.localize('RemoveServiceOverride'),
                    function(isConfirmed) {
                        if (isConfirmed) {
                            // Delete the service
                            console.log('Deleting service with ID:', serviceId);
                            abp.ui.setBusy($row);
                            _tenantSurpathServicesService.delete({ id: serviceId })
                                .done(function() {
                                    abp.notify.success(app.localize('SuccessfullyDeleted'));
                                    // Reload data to reflect the deletion
                                    loadPricingData();
                                })
                                .fail(function(error) {
                                    console.error('Failed to delete service:', error);
                                    abp.message.error(app.localize('ErrorDeletingService'));
                                })
                                .always(function() {
                                    abp.ui.clearBusy($row);
                                });
                        }
                    }
                );
            });
            
            // Add service at level - shows inline edit form
            _$servicesContainer.on('click', '.add-service-btn', function() {
                var $btn = $(this);
                var nodeId = $btn.data('node-id');
                var nodeType = $btn.data('node-type');
                var surpathServiceId = $btn.data('service-id');
                var $priceWrapper = $btn.closest('.price-input-wrapper');
                
                // Find the inherited price from parent
                var inheritedPrice = 0;
                // Look up the hierarchy to find the closest defined price
                if (_hierarchicalData) {
                    var effectivePrice = findEffectivePrice(_hierarchicalData, surpathServiceId);
                    if (effectivePrice !== null) {
                        inheritedPrice = effectivePrice;
                    }
                }
                
                // Create inline edit form
                var inputId = nodeType + '-' + nodeId + '-service-' + surpathServiceId + '-new';
                var editHtml = '<div class="price-edit-wrapper">';
                
                // Price input
                editHtml += '<input type="number" step="0.01" min="0" ';
                editHtml += 'id="' + inputId + '" ';
                editHtml += 'class="price-input custom" ';
                editHtml += 'value="' + inheritedPrice + '" ';
                editHtml += 'data-node-id="' + nodeId + '" ';
                editHtml += 'data-node-type="' + nodeType + '" ';
                editHtml += 'data-service-id="" '; // Empty because it's a new service
                editHtml += 'data-surpath-service-id="' + surpathServiceId + '" ';
                editHtml += 'data-is-new="true" />'; // Flag to indicate this is a new override
                
                // Enable/Disable toggle
                editHtml += '<div class="form-check form-switch ms-2">';
                editHtml += '<input class="form-check-input price-enabled-toggle" type="checkbox" ';
                editHtml += 'id="enabled-' + inputId + '" checked>';
                editHtml += '<label class="form-check-label" for="enabled-' + inputId + '">';
                editHtml += 'Enabled';
                editHtml += '</label>';
                editHtml += '</div>';
                
                // IsInvoiced toggle
                editHtml += '<div class="form-check form-switch ms-2">';
                editHtml += '<input class="form-check-input price-invoiced-toggle" type="checkbox" ';
                editHtml += 'id="invoiced-' + inputId + '">';
                editHtml += '<label class="form-check-label" for="invoiced-' + inputId + '">';
                editHtml += app.localize('IsInvoiced');
                editHtml += '</label>';
                editHtml += '</div>';
                
                editHtml += '<div class="price-edit-actions">';
                editHtml += '<button class="btn btn-sm btn-success price-save-btn" title="' + app.localize('Save') + '">';
                editHtml += '<i class="fa fa-check"></i>';
                editHtml += '</button>';
                editHtml += '<button class="btn btn-sm btn-secondary price-cancel-override-btn" title="' + app.localize('Cancel') + '">';
                editHtml += '<i class="fa fa-times"></i>';
                editHtml += '</button>';
                editHtml += '</div>';
                editHtml += '</div>';
                
                // Replace the button and inherited text with the edit form
                $priceWrapper.html(editHtml);
                $priceWrapper.find('.price-input').focus();
            });
            
            // Cancel new override
            _$servicesContainer.on('click', '.price-cancel-override-btn', function(e) {
                e.stopPropagation();
                // Simply reload the data to restore the original state
                loadPricingData();
            });
            
            // Expand/collapse handler for Total Cost view
            _$totalCostContainer.on('click', '.cost-summary-row.collapsible', function(e) {
                e.stopPropagation();
                var $row = $(this);
                var $expandIcon = $row.find('.expand-icon').first();
                var nodeId = $row.data('node-id');
                
                $expandIcon.toggleClass('expanded');
                
                if ($expandIcon.hasClass('expanded')) {
                    _$totalCostContainer.find('[data-parent-id="' + nodeId + '"]').show();
                    _expandedStates['cost-' + nodeId] = true;
                } else {
                    // Hide all children recursively in cost view
                    hideChildrenInCostView(nodeId);
                    _expandedStates['cost-' + nodeId] = false;
                }
            });
            
            // Filter input handler
            _$servicesContainer.on('input', '.service-filter-input', function() {
                var $input = $(this);
                var filterText = $input.val().toLowerCase();
                var serviceId = $input.data('service-id');
                var $serviceSection = $input.closest('.service-section');
                
                applyServiceFilter($serviceSection, filterText);
            });
            
            // Clear filter button handler
            _$servicesContainer.on('click', '.service-filter-clear', function() {
                var $btn = $(this);
                var serviceId = $btn.data('service-id');
                var $serviceSection = $btn.closest('.service-section');
                var $input = $serviceSection.find('.service-filter-input');
                
                $input.val('');
                applyServiceFilter($serviceSection, '');
            });
            
            // Total Cost filter input handler
            _$totalCostContainer.on('input', '.cost-filter-input', function() {
                var $input = $(this);
                var filterText = $input.val().toLowerCase();
                
                applyCostFilter(filterText);
            });
            
            // Total Cost clear filter button handler
            _$totalCostContainer.on('click', '.cost-filter-clear', function() {
                var $input = _$totalCostContainer.find('.cost-filter-input');
                $input.val('');
                applyCostFilter('');
            });
        }
        
        // Sort children alphabetically by name
        function sortChildren(children) {
            if (!children || children.length === 0) return children;
            
            return children.slice().sort(function(a, b) {
                var nameA = (a.fullName || a.name || '').toLowerCase();
                var nameB = (b.fullName || b.name || '').toLowerCase();
                
                if (nameA < nameB) return -1;
                if (nameA > nameB) return 1;
                return 0;
            });
        }
        
        // Apply filter to service section
        function applyServiceFilter($serviceSection, filterText) {
            var $rows = $serviceSection.find('.pricing-row');
            
            if (!filterText) {
                // Clear filter - show all rows but respect expansion state
                $rows.each(function() {
                    var $row = $(this);
                    var parentId = $row.data('parent-id');
                    
                    if (!parentId) {
                        // Top level - always show
                        $row.show();
                    } else {
                        // Check if parent is expanded
                        var shouldShow = isParentExpanded($serviceSection, parentId);
                        if (shouldShow) {
                            $row.show();
                        } else {
                            $row.hide();
                        }
                    }
                });
                return;
            }
            
            // Apply filter
            var matchedNodeIds = new Set();
            var nodesToExpand = new Set();
            
            // First pass - find all matching rows
            $rows.each(function() {
                var $row = $(this);
                var rowText = $row.text().toLowerCase();
                
                if (rowText.indexOf(filterText) > -1) {
                    var nodeId = $row.data('node-id');
                    matchedNodeIds.add(nodeId);
                    
                    // Track all ancestors that need to be visible
                    var parentId = $row.data('parent-id');
                    while (parentId) {
                        matchedNodeIds.add(parentId);
                        nodesToExpand.add(parentId);
                        
                        // Find parent's parent
                        var $parentRow = $serviceSection.find('.pricing-row[data-node-id="' + parentId + '"]');
                        if ($parentRow.length > 0) {
                            parentId = $parentRow.data('parent-id');
                        } else {
                            parentId = null;
                        }
                    }
                }
            });
            
            // Second pass - show/hide rows and expand as needed
            $rows.each(function() {
                var $row = $(this);
                var nodeId = $row.data('node-id');
                
                if (matchedNodeIds.has(nodeId)) {
                    $row.show();
                    
                    // Expand if needed to show children
                    if (nodesToExpand.has(nodeId)) {
                        var $expandIcon = $row.find('.expand-icon').first();
                        if (!$expandIcon.hasClass('expanded')) {
                            $expandIcon.addClass('expanded');
                            _expandedStates[nodeId] = true;
                        }
                    }
                } else {
                    $row.hide();
                }
            });
        }
        
        // Check if parent node is expanded
        function isParentExpanded($serviceSection, parentId) {
            var $parentRow = $serviceSection.find('.pricing-row[data-node-id="' + parentId + '"]');
            if ($parentRow.length === 0) return true; // If no parent found, assume visible
            
            var $expandIcon = $parentRow.find('.expand-icon').first();
            return $expandIcon.hasClass('expanded');
        }
        
        // Apply filter to total cost view
        function applyCostFilter(filterText) {
            var $rows = _$totalCostContainer.find('.cost-summary-row');
            
            if (!filterText) {
                // Clear filter - show all rows but respect expansion state
                $rows.each(function() {
                    var $row = $(this);
                    var parentId = $row.data('parent-id');
                    
                    if (!parentId) {
                        // Top level - always show
                        $row.show();
                    } else {
                        // Check if parent is expanded
                        var shouldShow = isParentExpandedInCostView(parentId);
                        if (shouldShow) {
                            $row.show();
                        } else {
                            $row.hide();
                        }
                    }
                });
                return;
            }
            
            // Apply filter
            var matchedNodeIds = new Set();
            var nodesToExpand = new Set();
            
            // First pass - find all matching rows
            $rows.each(function() {
                var $row = $(this);
                var rowText = $row.text().toLowerCase();
                
                if (rowText.indexOf(filterText) > -1) {
                    var nodeId = $row.data('node-id');
                    matchedNodeIds.add(nodeId);
                    
                    // Track all ancestors that need to be visible
                    var parentId = $row.data('parent-id');
                    while (parentId) {
                        matchedNodeIds.add(parentId);
                        nodesToExpand.add(parentId);
                        
                        // Find parent's parent
                        var $parentRow = _$totalCostContainer.find('.cost-summary-row[data-node-id="' + parentId + '"]');
                        if ($parentRow.length > 0) {
                            parentId = $parentRow.data('parent-id');
                        } else {
                            parentId = null;
                        }
                    }
                }
            });
            
            // Second pass - show/hide rows and expand as needed
            $rows.each(function() {
                var $row = $(this);
                var nodeId = $row.data('node-id');
                
                if (matchedNodeIds.has(nodeId)) {
                    $row.show();
                    
                    // Expand if needed to show children
                    if (nodesToExpand.has(nodeId)) {
                        var $expandIcon = $row.find('.expand-icon').first();
                        if (!$expandIcon.hasClass('expanded')) {
                            $expandIcon.addClass('expanded');
                            _expandedStates['cost-' + nodeId] = true;
                        }
                    }
                } else {
                    $row.hide();
                }
            });
        }
        
        // Check if parent node is expanded in cost view
        function isParentExpandedInCostView(parentId) {
            var $parentRow = _$totalCostContainer.find('.cost-summary-row[data-node-id="' + parentId + '"]');
            if ($parentRow.length === 0) return true; // If no parent found, assume visible
            
            var $expandIcon = $parentRow.find('.expand-icon').first();
            return $expandIcon.hasClass('expanded');
        }
        
        // Calculate service statistics for a surpath service
        function calculateServiceStats(serviceNodes) {
            var stats = {
                total: 0,
                enabled: 0,
                disabled: 0
            };
            
            // Count services at each node
            for (var nodeId in serviceNodes) {
                var serviceNode = serviceNodes[nodeId];
                
                if (serviceNode && serviceNode.service) {
                    stats.total++;
                    // Check isEnabled property (case-sensitive)
                    if (serviceNode.service.isPricingOverrideEnabled === true) {
                        stats.enabled++;
                    } else {
                        stats.disabled++;
                    }
                }
            }
            
            return stats;
        }
        
        // Hide children recursively
        function hideChildren(nodeId) {
            // Get all descendants at once using a more efficient approach
            var $directChildren = $('[data-parent-id="' + nodeId + '"]');
            
            if ($directChildren.length === 0) return;
            
            // Collect all descendant IDs first
            var descendantIds = [];
            $directChildren.each(function() {
                descendantIds.push($(this).data('node-id'));
            });
            
            // Hide direct children
            $directChildren.hide();
            $directChildren.find('.expand-icon').removeClass('expanded');
            
            // Process descendants without recursive DOM queries
            var i = 0;
            while (i < descendantIds.length) {
                var currentId = descendantIds[i];
                var $currentDescendants = $('[data-parent-id="' + currentId + '"]');
                
                if ($currentDescendants.length > 0) {
                    $currentDescendants.hide();
                    $currentDescendants.find('.expand-icon').removeClass('expanded');
                    
                    // Add their IDs to process
                    $currentDescendants.each(function() {
                        descendantIds.push($(this).data('node-id'));
                    });
                }
                i++;
            }
        }
        
        // Hide children recursively in cost view
        function hideChildrenInCostView(nodeId) {
            // Get all descendants at once using a more efficient approach
            var $directChildren = _$totalCostContainer.find('[data-parent-id="' + nodeId + '"]');
            
            if ($directChildren.length === 0) return;
            
            // Collect all descendant IDs first
            var descendantIds = [];
            $directChildren.each(function() {
                descendantIds.push($(this).data('node-id'));
            });
            
            // Hide direct children
            $directChildren.hide();
            $directChildren.find('.expand-icon').removeClass('expanded');
            
            // Process descendants iteratively
            var i = 0;
            while (i < descendantIds.length) {
                var currentId = descendantIds[i];
                var $currentDescendants = _$totalCostContainer.find('[data-parent-id="' + currentId + '"]');
                
                if ($currentDescendants.length > 0) {
                    $currentDescendants.hide();
                    $currentDescendants.find('.expand-icon').removeClass('expanded');
                    
                    // Add their IDs to process
                    $currentDescendants.each(function() {
                        descendantIds.push($(this).data('node-id'));
                    });
                }
                
                i++;
            }
        }
        
        // Load hierarchical pricing data using V2 endpoint
        function loadPricingData() {
            console.log('loadPricingData called with tenantId:', _selectedTenantId);
            if (!_selectedTenantId) {
                console.log('No tenant ID, returning');
                return;
            }
            
            _$loadingIndicator.show();
            _$servicesContainer.empty();
            
            console.log('Loading available services...');
            // First load available SurpathServices if not already loaded
            var servicesPromise = _availableSurpathServices 
                ? $.Deferred().resolve(_availableSurpathServices)
                : _tenantSurpathServicesService.getAvailableSurpathServices()
                    .done(function(services) {
                        console.log('Available services loaded:', services);
                        _availableSurpathServices = services;
                    })
                    .fail(function(error) {
                        console.error('Failed to load available services:', error);
                        abp.notify.error('Failed to load available services');
                    });
            
            // Then load hierarchical pricing using V2
            servicesPromise.then(function() {
                console.log('Loading hierarchical pricing V2...');
                return _tenantSurpathServicesService.getHierarchicalPricingV2({
                    tenantId: parseInt(_selectedTenantId),
                    surpathServiceId: null, // Get all services
                    includeDisabled: true // Include disabled services for management UI
                });
            }).done(function(result) {
                _hierarchicalData = result;
                console.log('Hierarchical data V2:', result);
                renderServices(result);
                
                // If we're on the cost tab, render costs too
                if (_currentTab === 'cost') {
                    renderTotalCosts();
                }
            }).fail(function(error) {
                console.error('Failed to load hierarchical pricing:', error);
                abp.notify.error('Failed to load pricing data');
                _$servicesContainer.html('<div class="no-data"><p>Error loading pricing data</p></div>');
            }).always(function() {
                _$loadingIndicator.hide();
            });
        }
        
        // Render all services
        function renderServices(rootNode) {
            if (!rootNode || !_availableSurpathServices) {
                _$servicesContainer.html('<div class="no-data"><p>' + app.localize('NoDataFound') + '</p></div>');
                return;
            }
            
            var html = '';
            
            // Group services by SurpathServiceId
            var serviceGroups = groupServicesByType(rootNode);
            
            // Render each available SurpathService
            _availableSurpathServices.forEach(function(surpathService) {
                var serviceNodes = serviceGroups[surpathService.id] || {};
                html += renderServiceSection(surpathService, serviceNodes, rootNode);
            });
            
            _$servicesContainer.html(html);
            
            // Restore expanded states
            for (var key in _expandedStates) {
                if (_expandedStates[key]) {
                    $('[data-node-id="' + key + '"]').find('.expand-icon').addClass('expanded');
                    $('[data-parent-id="' + key + '"]').show();
                }
            }
        }
        
        // Group services by SurpathServiceId across all nodes
        function groupServicesByType(node, groups) {
            groups = groups || {};
            
            // Process services at this node
            if (node.services) {
                node.services.forEach(function(service) {
                    if (service.surpathServiceId) {
                        if (!groups[service.surpathServiceId]) {
                            groups[service.surpathServiceId] = {};
                        }
                        groups[service.surpathServiceId][node.id] = {
                            node: node,
                            service: service
                        };
                    }
                });
            }
            
            // Process children
            if (node.children) {
                var sortedChildren = sortChildren(node.children);
                sortedChildren.forEach(function(child) {
                    groupServicesByType(child, groups);
                });
            }
            
            return groups;
        }
        
        // Render a service section for a specific SurpathService
        function renderServiceSection(surpathService, serviceNodes, rootNode) {
            var html = '<div class="service-section" data-service-id="' + surpathService.id + '">';
            
            // Calculate service statistics
            var stats = calculateServiceStats(serviceNodes);
            
            // Service header
            html += '<div class="service-header">';
            html += '<h3 class="service-title">' + surpathService.name + '</h3>';
            
            // Service statistics
            if (stats.total > 0) {
                html += '<div class="service-stats">';
                html += '<span class="stat-item">';
                html += '<span class="stat-label">Total:</span> ';
                html += '<span class="stat-value">' + stats.total + '</span>';
                html += '</span>';
                html += '<span class="stat-separator">|</span>';
                html += '<span class="stat-item">';
                html += '<span class="stat-label">Enabled:</span> ';
                html += '<span class="stat-value stat-enabled">' + stats.enabled + '</span>';
                html += '</span>';
                html += '<span class="stat-separator">|</span>';
                html += '<span class="stat-item">';
                html += '<span class="stat-label">Disabled:</span> ';
                html += '<span class="stat-value stat-disabled">' + stats.disabled + '</span>';
                html += '</span>';
                html += '</div>';
            }
            
            html += '<div class="service-actions">';
            
            // Check if service is configured at tenant level
            var tenantService = serviceNodes[rootNode.id];
            if (!tenantService) {
                html += '<button class="btn btn-sm btn-primary add-service-btn" data-node-id="' + rootNode.id + '" data-node-type="tenant" data-service-id="' + surpathService.id + '">';
                html += '<i class="fa fa-plus"></i> Enable Service';
                html += '</button>';
            }
            
            html += '</div>';
            html += '</div>';
            
            // Filter input
            html += '<div class="service-filter-wrapper mt-3 mb-3">';
            html += '<div class="input-group" style="max-width: 400px;">';
            html += '<input type="text" class="form-control service-filter-input" placeholder="Filter departments, cohorts, or users..." data-service-id="' + surpathService.id + '">';
            html += '<button class="btn btn-outline-secondary service-filter-clear" type="button" data-service-id="' + surpathService.id + '">';
            html += '<i class="fa fa-times"></i> Clear';
            html += '</button>';
            html += '</div>';
            html += '</div>';
            
            // Pricing tree
            html += '<div class="pricing-tree">';
            // Always render the hierarchy to allow re-enabling at any level
            html += renderNodeHierarchy(rootNode, surpathService.id, null, 0);
            html += '</div>';
            
            html += '</div>';
            
            return html;
        }
        
        // Render node hierarchy recursively
        function renderNodeHierarchy(node, surpathServiceId, parentId, level) {
            var html = '';
            var nodeService = findServiceForNode(node, surpathServiceId);
            var hasChildren = node.children && node.children.length > 0;
            
            // Determine CSS class based on node type
            var levelClass = node.nodeType + '-level';
            var paddingLeft = level * 20;
            
            html += '<div class="pricing-row ' + levelClass + (hasChildren ? ' collapsible' : '') + '" ';
            html += 'data-node-id="' + node.id + '" ';
            html += 'data-node-type="' + node.nodeType + '" ';
            if (parentId) {
                html += 'data-parent-id="' + parentId + '" ';
                html += 'style="display:none; padding-left:' + paddingLeft + 'px;" ';
            }
            html += '>';
            
            html += '<div class="entity-info">';
            if (hasChildren) {
                html += '<i class="fa fa-chevron-right expand-icon"></i>';
            }
            var displayName = node.nodeType === 'tenant' ? 'Tenant Wide' : (node.fullName || node.name);
            html += '<span class="entity-name">' + escapeHtml(displayName) + '</span>';
            html += '<span class="entity-label label-' + node.nodeType + '">' + node.nodeType.toUpperCase() + '</span>';
            html += '</div>';
            
            html += '<div class="price-input-wrapper">';
            html += renderPriceInput(node, nodeService, surpathServiceId);
            html += '</div>';
            
            html += '</div>';
            
            // Render children
            if (node.children) {
                var sortedChildren = sortChildren(node.children);
                sortedChildren.forEach(function(child) {
                    html += renderNodeHierarchy(child, surpathServiceId, node.id, level + 1);
                });
            }
            
            return html;
        }
        
        // Find service for a specific node
        function findServiceForNode(node, surpathServiceId) {
            if (!node.services) return null;
            return node.services.find(function(s) {
                return s.surpathServiceId === surpathServiceId;
            });
        }
        
        // Render price input for a node
        function renderPriceInput(node, service, surpathServiceId) {
            var inputId = node.nodeType + '-' + node.id + '-service-' + surpathServiceId;
            var html = '';
            
            if (service) {
                // Service exists at this level
                var isEnabled = service.isPricingOverrideEnabled !== undefined ? service.isPricingOverrideEnabled : true;
                
                // Display price based on enabled status
                if (isEnabled) {
                    html += '<span class="price-value price-custom">' + formatPrice(service.price) + '</span>';
                    html += '<i class="fa fa-check-circle service-status-icon service-enabled" title="' + app.localize('ServiceEnabled') + '"></i>';
                } else {
                    // Find inherited price for non-tenant levels
                    var displayPrice = '';
                    
                    if (node.nodeType === 'tenant') {
                        // Tenant level: show $0 ($configured price)
                        displayPrice = '$0.00 (' + formatPrice(service.price) + ')';
                    } else {
                        // Other levels: show $inherited ($configured price)
                        var inheritedPrice = findInheritedPrice(_hierarchicalData, surpathServiceId, node, node.nodeType);
                        displayPrice = formatPrice(inheritedPrice) + ' (' + formatPrice(service.price) + ')';
                    }
                    
                    html += '<span class="price-value price-custom">' + displayPrice + '</span>';
                    html += '<i class="fa fa-times-circle service-status-icon service-disabled" title="' + app.localize('ServiceDisabled') + '"></i>';
                    html += '<span class="badge badge-warning ms-2">Disabled</span>';
                }
                
                // Edit wrapper
                html += '<div class="price-edit-wrapper" style="display:none;">';
                
                // Price input
                html += '<input type="number" step="0.01" min="0" ';
                html += 'id="' + inputId + '" ';
                html += 'class="price-input custom" ';
                html += 'value="' + service.price + '" ';
                html += 'data-node-id="' + node.id + '" ';
                html += 'data-service-id="' + service.id + '" ';
                html += 'data-surpath-service-id="' + surpathServiceId + '" ';
                html += 'data-original-value="' + service.price + '" ';
                html += 'data-original-enabled="' + isEnabled + '" ';
                html += 'data-original-invoiced="' + (service.isInvoiced || false) + '" />';
                
                // Enable/Disable toggle
                html += '<div class="form-check form-switch ms-2">';
                html += '<input class="form-check-input price-enabled-toggle" type="checkbox" ';
                html += 'id="enabled-' + inputId + '" ';
                html += (isEnabled ? 'checked' : '') + '>';
                html += '<label class="form-check-label" for="enabled-' + inputId + '">';
                html += 'Enabled';
                html += '</label>';
                html += '</div>';
                
                // IsInvoiced toggle
                html += '<div class="form-check form-switch ms-2">';
                html += '<input class="form-check-input price-invoiced-toggle" type="checkbox" ';
                html += 'id="invoiced-' + inputId + '" ';
                html += (service.isInvoiced ? 'checked' : '') + '>';
                html += '<label class="form-check-label" for="invoiced-' + inputId + '">';
                html += app.localize('IsInvoiced');
                html += '</label>';
                html += '</div>';
                
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
                
                if (node.nodeType !== 'tenant') {
                    html += '<button class="price-delete-btn" data-service-id="' + service.id + '" title="' + app.localize('RemoveService') + '">';
                    html += '<i class="fa fa-trash"></i> Delete';
                    html += '</button>';
                }
            } else {
                // Service inherited from parent - find and display the inherited price
                var effectiveService = findEffectiveServiceForNode(_hierarchicalData, node, surpathServiceId);
                var inheritedPriceDisplay = 'N/A';
                
                if (effectiveService && effectiveService.service.isPricingOverrideEnabled) {
                    inheritedPriceDisplay = formatPrice(effectiveService.service.price);
                }
                
                html += '<span class="price-value price-inherited">' + inheritedPriceDisplay + '</span>';
                html += '<i class="fa fa-info-circle info-icon" title="' + app.localize('InheritedFromParent') + '"></i>';
                
                // Add button to create service at this level
                html += '<button class="btn btn-sm btn-outline-primary add-service-btn ms-2" ';
                html += 'data-node-id="' + node.id + '" ';
                html += 'data-node-type="' + node.nodeType + '" ';
                html += 'data-service-id="' + surpathServiceId + '">';
                html += '<i class="fa fa-plus"></i> Add Override';
                html += '</button>';
            }
            
            return html;
        }
        
        // Save price change
        function savePriceChange($input) {
            var serviceId = $input.data('service-id');
            var newPrice = $input.val() ? parseFloat($input.val()) : null;
            var nodeId = $input.data('node-id');
            var nodeType = $input.data('node-type');
            var isNew = $input.data('is-new') === true || $input.data('is-new') === 'true';
            
            // Get the enabled status from the toggle
            var $editWrapper = $input.closest('.price-edit-wrapper');
            var $enabledToggle = $editWrapper.find('.price-enabled-toggle');
            var isEnabled = $enabledToggle.length > 0 ? $enabledToggle.is(':checked') : true;
            
            // Get the invoiced status from the toggle
            var $invoicedToggle = $editWrapper.find('.price-invoiced-toggle');
            var isInvoiced = $invoicedToggle.length > 0 ? $invoicedToggle.is(':checked') : false;
            
            // For tenant level, null means 0
            if (nodeType === 'tenant' && newPrice === null) {
                newPrice = 0;
            }
            
            // If this is an existing service, get node type from row
            if (!nodeType) {
                var $row = $input.closest('.pricing-row');
                nodeType = $row.data('node-type') || 'tenant';
            }
            
            // Find the service name from the hierarchical data
            var serviceName = 'Service'; // default
            var surpathServiceId = $input.data('surpath-service-id');
            
            // Find the service in the available services
            if (_availableSurpathServices && surpathServiceId) {
                var surpathService = _availableSurpathServices.find(function(s) {
                    return s.id === surpathServiceId;
                });
                if (surpathService) {
                    serviceName = surpathService.name;
                }
            }
            
            // Prepare the DTO for CreateOrEdit
            var createOrEditDto = {
                id: isNew ? null : serviceId, // null for new overrides
                name: serviceName,
                price: newPrice || 0,
                isPricingOverrideEnabled: isEnabled,
                isInvoiced: isInvoiced,
                surpathServiceId: surpathServiceId,
                tenantId: parseInt(_selectedTenantId)
            };
            
            // Set hierarchy fields based on node type
            if (nodeType === 'department') {
                createOrEditDto.tenantDepartmentId = nodeId;
            } else if (nodeType === 'cohort') {
                createOrEditDto.cohortId = nodeId;
            } else if (nodeType === 'user') {
                createOrEditDto.userId = parseInt(nodeId);
            }
            
            console.log('Saving price change with DTO:', createOrEditDto);
            
            // Use CreateOrEdit method
            _tenantSurpathServicesService.createOrEdit(createOrEditDto)
                .done(function() {
                    // Hide edit wrapper immediately
                    var $editBtn = $editWrapper.siblings('.price-edit-btn');
                    $editWrapper.hide();
                    $editBtn.show();
                    
                    abp.notify.success(app.localize('SuccessfullyUpdated'));
                    
                    // Reload data to reflect changes
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
        
        // Find effective price by traversing up the hierarchy
        function findEffectivePrice(rootNode, surpathServiceId) {
            // First check if the service exists at tenant level
            var tenantService = rootNode.services ? rootNode.services.find(function(s) {
                return s.surpathServiceId === surpathServiceId;
            }) : null;
            
            if (tenantService) {
                return tenantService.price;
            }
            
            // For now, return the base price from available services
            if (_availableSurpathServices) {
                var surpathService = _availableSurpathServices.find(function(s) {
                    return s.id === surpathServiceId;
                });
                if (surpathService) {
                    return surpathService.price;
                }
            }
            
            return 0;
        }
        
        // Find the effective service for a node by looking at the node and its parents
        function findEffectiveServiceForNode(rootNode, targetNode, surpathServiceId) {
            // First check if the node itself has the service
            if (targetNode.services) {
                var nodeService = targetNode.services.find(function(s) {
                    return s.surpathServiceId === surpathServiceId;
                });
                if (nodeService) {
                    return {
                        service: nodeService,
                        nodeId: targetNode.id,
                        nodeType: targetNode.nodeType
                    };
                }
            }
            
            // If not found at this node, find the path from target to root and check each ancestor
            function findPathToRoot(node, targetId, path) {
                if (node.id === targetId) {
                    path.push(node);
                    return true;
                }
                
                if (node.children) {
                    for (var i = 0; i < node.children.length; i++) {
                        if (findPathToRoot(node.children[i], targetId, path)) {
                            path.push(node);
                            return true;
                        }
                    }
                }
                
                return false;
            }
            
            // Find path from target to root
            var path = [];
            findPathToRoot(rootNode, targetNode.id, path);
            
            // Walk up the path (from target to root) to find the first service
            for (var i = 0; i < path.length; i++) {
                var node = path[i];
                if (node.services) {
                    var service = node.services.find(function(s) {
                        return s.surpathServiceId === surpathServiceId;
                    });
                    if (service) {
                        return {
                            service: service,
                            nodeId: node.id,
                            nodeType: node.nodeType
                        };
                    }
                }
            }
            
            return null;
        }
        
        // Find inherited price by traversing up the hierarchy
        function findInheritedPrice(rootNode, surpathServiceId, currentNode, currentNodeType) {
            // Helper function to find parent node and its service
            function findParentService(node, targetId, targetType) {
                // Check if this node has the service and is enabled
                if (node.services) {
                    var service = node.services.find(function(s) {
                        return s.surpathServiceId === surpathServiceId && s.isPricingOverrideEnabled;
                    });
                    if (service && node.id !== targetId) {
                        return service.price;
                    }
                }
                
                // Check children
                if (node.children) {
                    for (var i = 0; i < node.children.length; i++) {
                        var child = node.children[i];
                        if (child.id === targetId) {
                            // Found the target node, return parent's price if it has the service
                            if (node.services) {
                                var parentService = node.services.find(function(s) {
                                    return s.surpathServiceId === surpathServiceId && s.isPricingOverrideEnabled;
                                });
                                if (parentService) {
                                    return parentService.price;
                                }
                            }
                            // If parent doesn't have it, check tenant level
                            if (node.nodeType !== 'tenant' && rootNode.services) {
                                var tenantService = rootNode.services.find(function(s) {
                                    return s.surpathServiceId === surpathServiceId && s.isPricingOverrideEnabled;
                                });
                                if (tenantService) {
                                    return tenantService.price;
                                }
                            }
                            return 0;
                        }
                        // Recursively search in children
                        var result = findParentService(child, targetId, targetType);
                        if (result !== null) {
                            return result;
                        }
                    }
                }
                
                return null;
            }
            
            var inheritedPrice = findParentService(rootNode, currentNode.id, currentNodeType);
            return inheritedPrice !== null ? inheritedPrice : 0;
        }
        
        // Render total costs view
        function renderTotalCosts() {
            if (!_hierarchicalData || !_availableSurpathServices) {
                _$totalCostContainer.html('<div class="no-data"><p>' + app.localize('NoDataFound') + '</p></div>');
                return;
            }
            
            var html = '';
            
            // Filter input for total cost view
            html += '<div class="cost-filter-wrapper mb-3">';
            html += '<div class="input-group" style="max-width: 400px;">';
            html += '<input type="text" class="form-control cost-filter-input" placeholder="Filter departments, cohorts, or users...">';
            html += '<button class="btn btn-outline-secondary cost-filter-clear" type="button">';
            html += '<i class="fa fa-times"></i> Clear';
            html += '</button>';
            html += '</div>';
            html += '</div>';
            
            // Tenant section
            html += '<div class="cost-summary-section">';
            html += '<div class="pricing-tree">';
            
            // Recursively render cost rows for the hierarchy
            html += renderCostRowsRecursive(_hierarchicalData, 0);
            
            html += '</div>';
            html += '</div>';
            
            _$totalCostContainer.html(html);
            
            // Restore expanded states in cost view
            for (var key in _expandedStates) {
                if (key.startsWith('cost-') && _expandedStates[key]) {
                    var nodeId = key.substring(5); // Remove 'cost-' prefix
                    _$totalCostContainer.find('[data-node-id="' + nodeId + '"]').find('.expand-icon').addClass('expanded');
                    _$totalCostContainer.find('[data-parent-id="' + nodeId + '"]').show();
                }
            }
        }
        
        // Recursively render cost rows for hierarchical structure
        function renderCostRowsRecursive(node, level, parentId) {
            var html = '';
            var costs = calculateNodeCosts(node);
            
            // Show nodes that have services (even if disabled)
            if (costs.hasServices) {
                html += renderCostRow(node.nodeType, node, costs, level, parentId);
            }
            
            // Render children
            if (node.children) {
                var sortedChildren = sortChildren(node.children);
                sortedChildren.forEach(function(child) {
                    html += renderCostRowsRecursive(child, level + 1, node.id);
                });
            }
            
            return html;
        }
        
        // Calculate costs for a node
        function calculateNodeCosts(node) {
            var costs = {};
            var total = 0;
            var hasServices = false;
            
            // Check all available services, not just the ones configured at this node
            _availableSurpathServices.forEach(function(surpathService) {
                var effectiveService = findEffectiveServiceForNode(_hierarchicalData, node, surpathService.id);
                
                if (effectiveService) {
                    hasServices = true;
                    var effectivePrice = 0;
                    var displayPrice = '';
                    var isInherited = effectiveService.nodeId !== node.id;
                    
                    if (effectiveService.service.isPricingOverrideEnabled) {
                        // Enabled service - use actual price
                        effectivePrice = effectiveService.service.price;
                        displayPrice = formatPrice(effectivePrice);
                    } else {
                        // Disabled service - price is not applied
                        effectivePrice = 0;
                        displayPrice = 'N/A';
                    }
                    
                    costs[surpathService.id] = {
                        name: surpathService.name,
                        price: effectivePrice,
                        displayPrice: displayPrice,
                        isEnabled: effectiveService.service.isPricingOverrideEnabled,
                        isInvoiced: effectiveService.service.isInvoiced || false,
                        isInherited: isInherited
                    };
                    total += parseFloat(effectivePrice);
                }
            });
            
            return {
                services: costs,
                total: total,
                hasServices: hasServices
            };
        }
        
        // Render cost row
        function renderCostRow(type, node, costs, level, parentId) {
            var html = '';
            var levelClass = type + '-level';
            var hasChildren = node.children && node.children.length > 0;
            var paddingLeft = level * 20;
            
            html += '<div class="cost-summary-row ' + levelClass + (hasChildren ? ' collapsible' : '') + '" ';
            html += 'data-node-id="' + node.id + '" ';
            html += 'data-node-type="' + node.nodeType + '" ';
            if (parentId) {
                html += 'data-parent-id="' + parentId + '" ';
                html += 'style="display:none; padding-left:' + paddingLeft + 'px;" ';
            }
            html += '>';
            
            html += '<div class="cost-entity-info">';
            
            // Add expand icon if has children
            if (hasChildren) {
                html += '<i class="fa fa-chevron-right expand-icon"></i>';
            }
            
            var displayName = type === 'tenant' ? 'Tenant Wide' : (node.fullName || node.name);
            html += '<span class="entity-name">' + escapeHtml(displayName) + '</span>';
            html += '<span class="entity-label label-' + type + '">' + type.toUpperCase() + '</span>';
            html += '</div>';
            
            html += '<div class="cost-breakdown">';
            
            // Show individual service costs
            for (var serviceId in costs.services) {
                var service = costs.services[serviceId];
                html += '<div class="cost-item">';
                html += '<div class="cost-service-name">' + escapeHtml(service.name);
                if (!service.isPricingOverrideEnabled) {
                    html += ' <span class="badge badge-warning">Disabled</span>';
                }
                if (service.isInvoiced) {
                    html += ' <span class="badge badge-info">Invoiced</span>';
                }
                html += '</div>';
                var priceClass = service.isInherited ? 'cost-service-price cost-price-inherited' : 'cost-service-price';
                html += '<div class="' + priceClass + '">' + service.displayPrice + '</div>';
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