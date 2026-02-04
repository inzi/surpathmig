import React from 'react';
import { Modal, Alert } from 'antd';
import L from '@/lib/L';

/**
 * ManageCategoriesModal
 *
 * Migrated from Surpath112:
 * - Source: wwwroot/view-resources/Areas/App/Views/RecordRequirements/_CategoryManagementModal.js
 * - View: Areas/App/Views/RecordRequirements/CategoryManagementModal.cshtml
 *
 * TODO: Full implementation required
 * - Display list of categories for the requirement
 * - Checkbox selection for bulk operations
 * - Move categories button (opens MoveCategoriesModal)
 * - Copy categories button (opens CopyCategoriesModal)
 * - Category ordering/drag-drop
 * - Edit individual category
 * - Delete category
 *
 * This is a placeholder implementation to allow the page to compile.
 * The full category management system requires RecordCategories and
 * RecordCategoryRules to be migrated first (WS 3.1.2).
 */

interface Props {
  visible: boolean;
  requirementId: string;
  onCancel: () => void;
  onSave: () => void;
}

const ManageCategoriesModal: React.FC<Props> = ({
  visible,
  requirementId,
  onCancel,
  onSave,
}) => {
  return (
    <Modal
      title={L('ManageCategories')}
      open={visible}
      onCancel={onCancel}
      width={900}
      footer={[
        <button
          key="close"
          type="button"
          className="btn btn-primary fw-bold"
          onClick={onCancel}
        >
          {L('Close')}
        </button>,
      ]}
    >
      <Alert
        message={L('FeatureUnderDevelopment')}
        description={
          <div>
            <p>
              {L('ManageCategoriesFeatureDescription')}
            </p>
            <p className="mb-0">
              <strong>{L('RequirementId')}:</strong> {requirementId}
            </p>
            <p className="text-muted mb-0">
              {L('ManageCategoriesWillBeImplementedInTask')} WS 3.1.2 - RecordCategories
            </p>
          </div>
        }
        type="info"
        showIcon
      />
    </Modal>
  );
};

export default ManageCategoriesModal;
