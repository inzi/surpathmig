import React from 'react';
import { Modal, Alert } from 'antd';
import L from '@/lib/L';

/**
 * CopyCategoriesModal
 *
 * Migrated from Surpath112:
 * - Feature referenced in _CategoryManagementModal.js but not fully implemented
 *
 * TODO: Full implementation required
 * - Show selected categories
 * - Requirement lookup/selector (to copy categories to different requirement)
 * - Option to copy linked RecordCategoryRules
 * - Confirmation
 * - Copy operation via API
 *
 * This is a placeholder implementation to allow the page to compile.
 * The full implementation will be done after RecordCategories migration (WS 3.1.2).
 */

interface Props {
  visible: boolean;
  categoryIds: string[];
  onCancel: () => void;
  onSave: () => void;
}

const CopyCategoriesModal: React.FC<Props> = ({
  visible,
  categoryIds,
  onCancel,
  onSave,
}) => {
  return (
    <Modal
      title={L('CopyCategories')}
      open={visible}
      onCancel={onCancel}
      width={700}
      footer={[
        <button
          key="cancel"
          type="button"
          className="btn btn-light-primary fw-bold"
          onClick={onCancel}
        >
          {L('Cancel')}
        </button>,
        <button
          key="copy"
          type="button"
          className="btn btn-primary fw-bold"
          disabled
        >
          {L('Copy')}
        </button>,
      ]}
    >
      <Alert
        message={L('FeatureUnderDevelopment')}
        description={
          <div>
            <p>
              {L('CopyCategoriesFeatureDescription')}
            </p>
            <p className="mb-0">
              <strong>{L('SelectedCategories')}:</strong> {categoryIds.length}
            </p>
            <p className="text-muted mb-0">
              {L('CopyCategoriesRequiresPermission')}: Pages.Administration.RecordRequirements.CopyCategories
            </p>
          </div>
        }
        type="info"
        showIcon
      />
    </Modal>
  );
};

export default CopyCategoriesModal;
