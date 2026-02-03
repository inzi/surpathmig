/**
 * ViewRecordCategoryModal Component
 *
 * Migrated from Surpath112: Areas/App/Views/RecordCategories/_ViewRecordCategoryModal.cshtml
 *
 * Features:
 * - Read-only view of RecordCategory details
 * - Shows all fields including related entity names
 */

import React, { useState, useEffect } from 'react';
import { Modal, Descriptions, Spin, message } from 'antd';
import { L } from '@/lib/abpUtility';
import {
  recordCategoriesService,
  type GetRecordCategoryForViewDto
} from '@/services/surpath/recordCategories.service';

interface ViewRecordCategoryModalProps {
  visible: boolean;
  recordCategoryId: string;
  onClose: () => void;
}

const ViewRecordCategoryModal: React.FC<ViewRecordCategoryModalProps> = ({
  visible,
  recordCategoryId,
  onClose
}) => {
  // ============================================================================
  // STATE
  // ============================================================================
  const [loading, setLoading] = useState(false);
  const [record, setRecord] = useState<GetRecordCategoryForViewDto | null>(null);

  // ============================================================================
  // DATA FETCHING
  // ============================================================================

  useEffect(() => {
    if (visible && recordCategoryId) {
      loadRecord();
    }
  }, [visible, recordCategoryId]);

  const loadRecord = async () => {
    setLoading(true);
    try {
      const result = await recordCategoriesService.getRecordCategoryForView(recordCategoryId);
      setRecord(result);
    } catch (error: any) {
      message.error(error.message || L('AnErrorOccurred'));
      onClose();
    } finally {
      setLoading(false);
    }
  };

  // ============================================================================
  // RENDER
  // ============================================================================

  return (
    <Modal
      title={L('RecordCategoryDetails')}
      open={visible}
      onCancel={onClose}
      width={800}
      footer={null}
    >
      <Spin spinning={loading}>
        {record && (
          <Descriptions bordered column={1} style={{ marginTop: 24 }}>
            <Descriptions.Item label={L('Name')}>
              {record.recordCategory.name}
            </Descriptions.Item>

            <Descriptions.Item label={L('Instructions')}>
              {record.recordCategory.instructions || '-'}
            </Descriptions.Item>

            <Descriptions.Item label={L('RecordRequirement')}>
              {record.recordRequirementName || '-'}
            </Descriptions.Item>

            <Descriptions.Item label={L('RecordCategoryRule')}>
              {record.recordCategoryRuleName || '-'}
            </Descriptions.Item>

            <Descriptions.Item label={L('IsSurpathService')}>
              {record.recordCategory.isSurpathService ? L('Yes') : L('No')}
            </Descriptions.Item>
          </Descriptions>
        )}
      </Spin>
    </Modal>
  );
};

export default ViewRecordCategoryModal;
