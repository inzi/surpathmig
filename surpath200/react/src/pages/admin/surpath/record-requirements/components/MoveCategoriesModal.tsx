import React from 'react';
import { Modal, Alert } from 'antd';
import L from '@/lib/L';

/**
 * MoveCategoriesModal
 *
 * Migrated from Surpath112:
 * - Source: wwwroot/view-resources/Areas/App/Views/RecordRequirements/_MoveCategoryModal.js
 * - View: Areas/App/Views/RecordRequirements/MoveCategoryModal.cshtml
 *
 * TODO: Full implementation required
 * - Show selected categories
 * - Requirement lookup/selector (to move categories to different requirement)
 * - Confirmation
 * - Move operation via API
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

const MoveCategoriesModal: React.FC<Props> = ({
  visible,
  categoryIds,
  onCancel,
  onSave,
}) => {
  return (
    <Modal
      title={L('MoveCategories')}
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
          key="move"
          type="button"
          className="btn btn-primary fw-bold"
          disabled
        >
          {L('Move')}
        </button>,
      ]}
    >
      <Alert
        message={L('FeatureUnderDevelopment')}
        description={
          <div>
            <p>
              {L('MoveCategoriesFeatureDescription')}
            </p>
            <p className="mb-0">
              <strong>{L('SelectedCategories')}:</strong> {categoryIds.length}
            </p>
            <p className="text-muted mb-0">
              {L('MoveCategoriesRequiresPermission')}: Pages.Administration.RecordRequirements.MoveCategories
            </p>
          </div>
        }
        type="info"
        showIcon
      />
    </Modal>
  );
};

export default MoveCategoriesModal;
