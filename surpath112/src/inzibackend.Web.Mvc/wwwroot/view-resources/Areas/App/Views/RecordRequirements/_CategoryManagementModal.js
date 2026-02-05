(function ($) {
    app.modals.CategoryManagementModal = function () {
        var _modalManager;
        var _$categoryManagementModal;
        var _$categoriesTable;
        var _categoryManagementService = abp.services.app.categoryManagement;

        var _permissions = {
            manageCategories: abp.auth.hasPermission('Pages.Administration.RecordRequirements.ManageCategories'),
            moveCategories: abp.auth.hasPermission('Pages.Administration.RecordRequirements.MoveCategories'),
            copyCategories: abp.auth.hasPermission('Pages.Administration.RecordRequirements.CopyCategories')
        };

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _$categoryManagementModal = _modalManager.getModal();
            _$categoriesTable = _$categoryManagementModal.find('#CategoriesTable');
            
            // Initialize event handlers after modal is loaded
            initializeEventHandlers();
            
            // Initialize selection state
            updateSelectionInfo();
        };

        function initializeEventHandlers() {
            // Select all categories checkbox
            _$categoryManagementModal.on('change', '#selectAllCategories', function () {
                var isChecked = $(this).prop('checked');
                _$categoryManagementModal.find('.category-checkbox').prop('checked', isChecked);
                updateSelectionInfo();
            });

            // Individual category checkbox change
            _$categoryManagementModal.on('change', '.category-checkbox', function () {
                updateSelectAllCheckbox();
                updateSelectionInfo();
            });

            // Move categories button
            _$categoryManagementModal.on('click', '#moveSelectedCategories', function () {
                var selectedCategoryIds = getSelectedCategoryIds();
                if (selectedCategoryIds.length === 0) {
                    abp.message.warn(app.localize('PleaseSelectAtLeastOneCategory'));
                    return;
                }
                
                // Close this modal and open move modal
                _modalManager.close();
                
                // Open move categories modal
                openMoveCategoryModal(selectedCategoryIds);
            });

            // Copy categories button
            _$categoryManagementModal.on('click', '#copySelectedCategories', function () {
                var selectedCategoryIds = getSelectedCategoryIds();
                if (selectedCategoryIds.length === 0) {
                    abp.message.warn(app.localize('PleaseSelectAtLeastOneCategory'));
                    return;
                }
                
                // Close this modal and open copy modal
                _modalManager.close();
                
                // TODO: Open copy categories modal
                // This will be implemented in Task 11
                abp.message.info('Copy categories functionality will be implemented in the next task.');
            });
        }

        function updateSelectAllCheckbox() {
            var $categoryCheckboxes = _$categoryManagementModal.find('.category-checkbox');
            var checkedCount = $categoryCheckboxes.filter(':checked').length;
            var totalCount = $categoryCheckboxes.length;
            
            var $selectAllCheckbox = _$categoryManagementModal.find('#selectAllCategories');
            
            if (checkedCount === 0) {
                $selectAllCheckbox.prop('checked', false);
                $selectAllCheckbox.prop('indeterminate', false);
            } else if (checkedCount === totalCount) {
                $selectAllCheckbox.prop('checked', true);
                $selectAllCheckbox.prop('indeterminate', false);
            } else {
                $selectAllCheckbox.prop('checked', false);
                $selectAllCheckbox.prop('indeterminate', true);
            }
        }

        function updateSelectionInfo() {
            var selectedCount = _$categoryManagementModal.find('.category-checkbox:checked').length;
            _$categoryManagementModal.find('#selectedCategoryCount').text(selectedCount);
            
            if (selectedCount > 0) {
                _$categoryManagementModal.find('#categorySelectionInfo').removeClass('d-none');
                _$categoryManagementModal.find('#moveSelectedCategories, #copySelectedCategories').prop('disabled', false);
            } else {
                _$categoryManagementModal.find('#categorySelectionInfo').addClass('d-none');
                _$categoryManagementModal.find('#moveSelectedCategories, #copySelectedCategories').prop('disabled', true);
            }
        }

        function getSelectedCategoryIds() {
            var categoryIds = [];
            _$categoryManagementModal.find('.category-checkbox:checked').each(function () {
                categoryIds.push($(this).val());
            });
            return categoryIds;
        }

        function openMoveCategoryModal(categoryIds) {
            // Create and open move category modal
            var _moveCategoryModal = new app.ModalManager({
                viewUrl: abp.appPath + 'App/RecordRequirements/MoveCategoryModal',
                scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RecordRequirements/_MoveCategoryModal.js',
                modalClass: 'MoveCategoryModal',
            });
            
            _moveCategoryModal.open({ categoryIds: categoryIds.join(',') });
        }

        this.close = function () {
            _modalManager.close();
            // Trigger refresh event when modal is closed
            abp.event.trigger('app.categoryManagementCompleted');
        };
    };
})(jQuery); 