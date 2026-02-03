/**
 * CreateOrEditRecordCategoryModal Component
 *
 * Migrated from Surpath112: Areas/App/Views/RecordCategories/_CreateOrEditModal.cshtml + _CreateOrEditModal.js
 *
 * Features:
 * - Create new or edit existing RecordCategory
 * - Form validation using React Hook Form
 * - Lookup modals for RecordRequirement and RecordCategoryRule selection
 */

import React, { useState, useEffect } from 'react';
import { Modal, Form, Input, Spin, message, Button, Space } from 'antd';
import { SearchOutlined, CloseOutlined } from '@ant-design/icons';
import { useForm, Controller } from 'react-hook-form';
import { L } from '@/lib/abpUtility';
import {
  recordCategoriesService,
  type CreateOrEditRecordCategoryDto,
  type GetRecordCategoryForEditOutput
} from '@/services/surpath/recordCategories.service';

interface CreateOrEditRecordCategoryModalProps {
  visible: boolean;
  recordCategoryId?: string;
  onCancel: () => void;
  onSuccess: () => void;
}

interface FormValues {
  id?: string;
  name: string;
  instructions?: string;
  recordRequirementId?: string;
  recordRequirementName?: string;
  recordCategoryRuleId?: string;
  recordCategoryRuleName?: string;
}

const CreateOrEditRecordCategoryModal: React.FC<CreateOrEditRecordCategoryModalProps> = ({
  visible,
  recordCategoryId,
  onCancel,
  onSuccess
}) => {
  // ============================================================================
  // STATE
  // ============================================================================
  const [loading, setLoading] = useState(false);
  const [saving, setSaving] = useState(false);

  const {
    control,
    handleSubmit,
    setValue,
    watch,
    formState: { errors },
    reset
  } = useForm<FormValues>({
    defaultValues: {
      name: '',
      instructions: '',
      recordRequirementId: undefined,
      recordRequirementName: '',
      recordCategoryRuleId: undefined,
      recordCategoryRuleName: ''
    }
  });

  const isEditMode = !!recordCategoryId;

  // ============================================================================
  // DATA FETCHING
  // ============================================================================

  useEffect(() => {
    if (visible) {
      if (recordCategoryId) {
        loadRecordCategory();
      } else {
        reset();
      }
    }
  }, [visible, recordCategoryId]);

  const loadRecordCategory = async () => {
    if (!recordCategoryId) return;

    setLoading(true);
    try {
      const result: GetRecordCategoryForEditOutput = await recordCategoriesService.getRecordCategoryForEdit({
        id: recordCategoryId
      });

      const recordCategory = result.recordCategory;
      setValue('id', recordCategory.id);
      setValue('name', recordCategory.name);
      setValue('instructions', recordCategory.instructions || '');
      setValue('recordRequirementId', recordCategory.recordRequirementId);
      setValue('recordRequirementName', result.recordRequirementName || '');
      setValue('recordCategoryRuleId', recordCategory.recordCategoryRuleId);
      setValue('recordCategoryRuleName', result.recordCategoryRuleName || '');
    } catch (error: any) {
      message.error(error.message || L('AnErrorOccurred'));
      onCancel();
    } finally {
      setLoading(false);
    }
  };

  // ============================================================================
  // FORM SUBMISSION
  // ============================================================================

  const onSubmit = async (values: FormValues) => {
    setSaving(true);
    try {
      const input: CreateOrEditRecordCategoryDto = {
        id: values.id,
        name: values.name,
        instructions: values.instructions,
        recordRequirementId: values.recordRequirementId,
        recordCategoryRuleId: values.recordCategoryRuleId
      };

      await recordCategoriesService.createOrEdit(input);
      message.success(L('SavedSuccessfully'));
      onSuccess();
    } catch (error: any) {
      message.error(error.message || L('AnErrorOccurred'));
    } finally {
      setSaving(false);
    }
  };

  // ============================================================================
  // LOOKUP HANDLERS
  // ============================================================================

  const handleRecordRequirementLookup = () => {
    // TODO: Implement RecordRequirement lookup modal when available
    message.info('RecordRequirement lookup modal will be implemented when RecordRequirements module is available');
  };

  const handleClearRecordRequirement = () => {
    setValue('recordRequirementId', undefined);
    setValue('recordRequirementName', '');
  };

  const handleRecordCategoryRuleLookup = () => {
    // TODO: Implement RecordCategoryRule lookup modal
    message.info('RecordCategoryRule lookup modal will be implemented with RecordCategoryRules module');
  };

  const handleClearRecordCategoryRule = () => {
    setValue('recordCategoryRuleId', undefined);
    setValue('recordCategoryRuleName', '');
  };

  // ============================================================================
  // RENDER
  // ============================================================================

  return (
    <Modal
      title={isEditMode ? L('EditRecordCategory') : L('CreateNewRecordCategory')}
      open={visible}
      onCancel={onCancel}
      width={800}
      footer={[
        <Button key="cancel" onClick={onCancel}>
          {L('Cancel')}
        </Button>,
        <Button
          key="save"
          type="primary"
          loading={saving}
          onClick={handleSubmit(onSubmit)}
        >
          {L('Save')}
        </Button>
      ]}
    >
      <Spin spinning={loading}>
        <Form layout="vertical" style={{ marginTop: 24 }}>
          {/* Name */}
          <Form.Item
            label={L('Name')}
            required
            validateStatus={errors.name ? 'error' : ''}
            help={errors.name?.message}
          >
            <Controller
              name="name"
              control={control}
              rules={{
                required: L('ThisFieldIsRequired'),
                maxLength: {
                  value: 255,
                  message: L('MaxLengthExceeded', 255)
                }
              }}
              render={({ field }) => (
                <Input {...field} placeholder={L('Name')} maxLength={255} />
              )}
            />
          </Form.Item>

          {/* Instructions */}
          <Form.Item
            label={L('Instructions')}
            validateStatus={errors.instructions ? 'error' : ''}
            help={errors.instructions?.message}
          >
            <Controller
              name="instructions"
              control={control}
              rules={{
                maxLength: {
                  value: 2000,
                  message: L('MaxLengthExceeded', 2000)
                }
              }}
              render={({ field }) => (
                <Input.TextArea
                  {...field}
                  placeholder={L('Instructions')}
                  rows={4}
                  maxLength={2000}
                />
              )}
            />
          </Form.Item>

          {/* RecordRequirement Lookup */}
          <Form.Item label={L('RecordRequirement')}>
            <Space.Compact style={{ width: '100%' }}>
              <Controller
                name="recordRequirementName"
                control={control}
                render={({ field }) => (
                  <Input {...field} placeholder={L('SelectRecordRequirement')} readOnly />
                )}
              />
              <Button
                icon={<SearchOutlined />}
                onClick={handleRecordRequirementLookup}
                title={L('Select')}
              />
              <Button
                icon={<CloseOutlined />}
                onClick={handleClearRecordRequirement}
                title={L('Clear')}
              />
            </Space.Compact>
          </Form.Item>

          {/* RecordCategoryRule Lookup */}
          <Form.Item label={L('RecordCategoryRule')}>
            <Space.Compact style={{ width: '100%' }}>
              <Controller
                name="recordCategoryRuleName"
                control={control}
                render={({ field }) => (
                  <Input {...field} placeholder={L('SelectRecordCategoryRule')} readOnly />
                )}
              />
              <Button
                icon={<SearchOutlined />}
                onClick={handleRecordCategoryRuleLookup}
                title={L('Select')}
              />
              <Button
                icon={<CloseOutlined />}
                onClick={handleClearRecordCategoryRule}
                title={L('Clear')}
              />
            </Space.Compact>
          </Form.Item>
        </Form>
      </Spin>
    </Modal>
  );
};

export default CreateOrEditRecordCategoryModal;
