/**
 * RecordCategoryRules - CreateOrEdit Page
 *
 * Migrated from Surpath112: Areas/App/Views/RecordCategoryRules/CreateOrEdit.cshtml + CreateOrEdit.js
 *
 * Features:
 * - Full-page form (not modal)
 * - Complex validation logic for warning days and status relationships
 * - Conditional field visibility (Expires checkbox controls ExpiredStatus visibility)
 * - Business rule: ExpiredStatus required when Expires=true
 * - Save and Save & New buttons
 */

import React, { useState, useEffect } from 'react';
import { Card, Form, Input, Switch, InputNumber, Button, Space, message, Spin, Row, Col, Select } from 'antd';
import { SaveOutlined, PlusOutlined, ArrowLeftOutlined } from '@ant-design/icons';
import { useNavigate, useParams } from 'react-router-dom';
import { useForm, Controller } from 'react-hook-form';
import { useAppSelector } from '@/store';
import { L } from '@/lib/abpUtility';
import {
  recordCategoryRulesService,
  type CreateOrEditRecordCategoryRuleDto,
  type GetRecordCategoryRuleForEditOutput
} from '@/services/surpath/recordCategoryRules.service';

interface FormValues {
  id?: string;
  name: string;
  description?: string;
  notify: boolean;
  expireInDays: number;
  warnDaysBeforeFirst: number;
  expires: boolean;
  required: boolean;
  isSurpathOnly: boolean;
  warnDaysBeforeSecond: number;
  warnDaysBeforeFinal: number;
  metaData?: string;
  firstWarnStatusId?: string;
  firstWarnStatusName?: string;
  secondWarnStatusId?: string;
  secondWarnStatusName?: string;
  finalWarnStatusId?: string;
  finalWarnStatusName?: string;
  expiredStatusId?: string;
  expiredStatusName?: string;
}

const CreateOrEditRecordCategoryRulePage: React.FC = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEditMode = !!id;

  // ============================================================================
  // STATE
  // ============================================================================
  const [loading, setLoading] = useState(false);
  const [saving, setSaving] = useState(false);
  const [recordStatuses, setRecordStatuses] = useState<Array<{ value: string; label: string }>>([]);

  const {
    control,
    handleSubmit,
    setValue,
    watch,
    formState: { errors },
    reset,
    trigger
  } = useForm<FormValues>({
    defaultValues: {
      name: '',
      description: '',
      notify: false,
      expireInDays: 0,
      warnDaysBeforeFirst: 0,
      expires: false,
      required: false,
      isSurpathOnly: false,
      warnDaysBeforeSecond: 0,
      warnDaysBeforeFinal: 0,
      metaData: '',
      firstWarnStatusId: undefined,
      secondWarnStatusId: undefined,
      finalWarnStatusId: undefined,
      expiredStatusId: undefined
    }
  });

  // Watch form values for conditional logic
  const expires = watch('expires');
  const warnDaysBeforeFirst = watch('warnDaysBeforeFirst');
  const warnDaysBeforeSecond = watch('warnDaysBeforeSecond');
  const warnDaysBeforeFinal = watch('warnDaysBeforeFinal');
  const firstWarnStatusId = watch('firstWarnStatusId');
  const secondWarnStatusId = watch('secondWarnStatusId');
  const finalWarnStatusId = watch('finalWarnStatusId');
  const expiredStatusId = watch('expiredStatusId');

  // Permissions
  const sessionInfo = useAppSelector(state => state.session);
  const isHost = sessionInfo.tenant?.id === undefined; // Host user if no tenant

  // ============================================================================
  // DATA FETCHING
  // ============================================================================

  useEffect(() => {
    loadRecordStatuses();
    if (id) {
      loadRecordCategoryRule();
    }
  }, [id]);

  const loadRecordStatuses = async () => {
    // TODO: Implement RecordStatus lookup service when available
    // For now, use placeholder data
    setRecordStatuses([
      { value: '1', label: 'Pending' },
      { value: '2', label: 'Approved' },
      { value: '3', label: 'Rejected' },
      { value: '4', label: 'Expired' }
    ]);
  };

  const loadRecordCategoryRule = async () => {
    if (!id) return;

    setLoading(true);
    try {
      const result: GetRecordCategoryRuleForEditOutput = await recordCategoryRulesService.getRecordCategoryRuleForEdit({
        id
      });

      const rule = result.recordCategoryRule;
      setValue('id', rule.id);
      setValue('name', rule.name);
      setValue('description', rule.description || '');
      setValue('notify', rule.notify);
      setValue('expireInDays', rule.expireInDays);
      setValue('warnDaysBeforeFirst', rule.warnDaysBeforeFirst);
      setValue('expires', rule.expires);
      setValue('required', rule.required);
      setValue('isSurpathOnly', rule.isSurpathOnly);
      setValue('warnDaysBeforeSecond', rule.warnDaysBeforeSecond);
      setValue('warnDaysBeforeFinal', rule.warnDaysBeforeFinal);
      setValue('metaData', rule.metaData || '');
      setValue('firstWarnStatusId', rule.firstWarnStatusId);
      setValue('firstWarnStatusName', result.firstWarnStatusName);
      setValue('secondWarnStatusId', rule.secondWarnStatusId);
      setValue('secondWarnStatusName', result.secondWarnStatusName);
      setValue('finalWarnStatusId', rule.finalWarnStatusId);
      setValue('finalWarnStatusName', result.finalWarnStatusName);
      setValue('expiredStatusId', rule.expiredStatusId);
      setValue('expiredStatusName', result.expiredStatusName);
    } catch (error: any) {
      message.error(error.message || L('AnErrorOccurred'));
      navigate('/app/compliance/record-category-rules');
    } finally {
      setLoading(false);
    }
  };

  // ============================================================================
  // VALIDATION LOGIC (from Surpath112 CreateOrEdit.js)
  // ============================================================================

  const validateWarningDaysAndStatus = (): boolean => {
    let isValid = true;

    // Only ExpiredStatus is required when Expires is checked
    // Warning statuses are optional but recommended
    if (expires && !expiredStatusId) {
      message.error(L('ExpiredStatusRequired'));
      isValid = false;
    }

    // Info messages for warning statuses (not errors)
    if (warnDaysBeforeFirst > 0 && !firstWarnStatusId) {
      message.info(L('ConsiderSelectingAStatusForFirstWarning'));
    }

    if (warnDaysBeforeSecond > 0 && !secondWarnStatusId) {
      message.info(L('ConsiderSelectingAStatusForSecondWarning'));
    }

    if (warnDaysBeforeFinal > 0 && !finalWarnStatusId) {
      message.info(L('ConsiderSelectingAStatusForFinalWarning'));
    }

    return isValid;
  };

  // ============================================================================
  // FORM SUBMISSION
  // ============================================================================

  const onSubmit = async (values: FormValues, saveAndNew: boolean = false) => {
    if (!validateWarningDaysAndStatus()) {
      return;
    }

    setSaving(true);
    try {
      const input: CreateOrEditRecordCategoryRuleDto = {
        id: values.id,
        name: values.name,
        description: values.description,
        notify: values.notify,
        expireInDays: values.expireInDays,
        warnDaysBeforeFirst: values.warnDaysBeforeFirst,
        expires: values.expires,
        required: values.required,
        isSurpathOnly: values.isSurpathOnly,
        warnDaysBeforeSecond: values.warnDaysBeforeSecond,
        warnDaysBeforeFinal: values.warnDaysBeforeFinal,
        metaData: values.metaData,
        // Convert empty strings to null for status IDs
        firstWarnStatusId: values.firstWarnStatusId || undefined,
        secondWarnStatusId: values.secondWarnStatusId || undefined,
        finalWarnStatusId: values.finalWarnStatusId || undefined,
        expiredStatusId: values.expires ? (values.expiredStatusId || undefined) : undefined
      };

      await recordCategoryRulesService.createOrEdit(input);
      message.success(L('SavedSuccessfully'));

      if (saveAndNew && !id) {
        // Clear form for new entry
        reset();
      } else {
        // Navigate back to list
        navigate('/app/compliance/record-category-rules');
      }
    } catch (error: any) {
      message.error(error.message || L('AnErrorOccurred'));
    } finally {
      setSaving(false);
    }
  };

  // ============================================================================
  // EVENT HANDLERS (from Surpath112 CreateOrEdit.js)
  // ============================================================================

  const handleExpiresChange = (checked: boolean) => {
    setValue('expires', checked);

    if (!checked) {
      setValue('expiredStatusId', undefined);
      setValue('expiredStatusName', '');
    } else if (!expiredStatusId) {
      message.warning(L('ExpiredStatusRequired'));
    }
  };

  // ============================================================================
  // RENDER
  // ============================================================================

  return (
    <div style={{ padding: '24px' }}>
      <Card
        title={isEditMode ? L('EditRecordCategoryRule') : L('CreateNewRecordCategoryRule')}
        extra={
          <Button icon={<ArrowLeftOutlined />} onClick={() => navigate('/app/compliance/record-category-rules')}>
            {L('Back')}
          </Button>
        }
      >
        <Spin spinning={loading}>
          <Form layout="vertical" style={{ maxWidth: 1200 }}>
            <Row gutter={24}>
              {/* Left Column */}
              <Col span={12}>
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
                        value: 100,
                        message: L('MaxLengthExceeded', 100)
                      }
                    }}
                    render={({ field }) => (
                      <Input {...field} placeholder={L('Name')} maxLength={100} />
                    )}
                  />
                </Form.Item>

                {/* Description */}
                <Form.Item
                  label={L('Description')}
                  validateStatus={errors.description ? 'error' : ''}
                  help={errors.description?.message}
                >
                  <Controller
                    name="description"
                    control={control}
                    rules={{
                      maxLength: {
                        value: 500,
                        message: L('MaxLengthExceeded', 500)
                      }
                    }}
                    render={({ field }) => (
                      <Input.TextArea
                        {...field}
                        placeholder={L('Description')}
                        rows={4}
                        maxLength={500}
                      />
                    )}
                  />
                </Form.Item>

                {/* Notify */}
                <Form.Item label={L('Notify')}>
                  <Controller
                    name="notify"
                    control={control}
                    render={({ field }) => (
                      <Switch
                        checked={field.value}
                        onChange={field.onChange}
                        checkedChildren={L('Yes')}
                        unCheckedChildren={L('No')}
                      />
                    )}
                  />
                </Form.Item>

                {/* Expire In Days */}
                <Form.Item label={L('ExpireInDays')}>
                  <Controller
                    name="expireInDays"
                    control={control}
                    render={({ field }) => (
                      <InputNumber
                        {...field}
                        style={{ width: '100%' }}
                        min={0}
                        placeholder={L('ExpireInDays')}
                      />
                    )}
                  />
                </Form.Item>

                {/* Expires */}
                <Form.Item label={L('Expires')}>
                  <Controller
                    name="expires"
                    control={control}
                    render={({ field }) => (
                      <Switch
                        checked={field.value}
                        onChange={(checked) => {
                          field.onChange(checked);
                          handleExpiresChange(checked);
                        }}
                        checkedChildren={L('Yes')}
                        unCheckedChildren={L('No')}
                      />
                    )}
                  />
                </Form.Item>

                {/* Expired Status - Only visible when Expires is true */}
                {expires && (
                  <Form.Item
                    label={L('ExpiredStatus')}
                    required
                    validateStatus={expires && !expiredStatusId ? 'error' : ''}
                    help={expires && !expiredStatusId ? L('ExpiredStatusRequired') : ''}
                  >
                    <Controller
                      name="expiredStatusId"
                      control={control}
                      render={({ field }) => (
                        <Select
                          {...field}
                          placeholder={L('SelectExpiredStatus')}
                          options={recordStatuses}
                          allowClear
                        />
                      )}
                    />
                  </Form.Item>
                )}

                {/* Required */}
                <Form.Item label={L('Required')}>
                  <Controller
                    name="required"
                    control={control}
                    render={({ field }) => (
                      <Switch
                        checked={field.value}
                        onChange={field.onChange}
                        checkedChildren={L('Yes')}
                        unCheckedChildren={L('No')}
                      />
                    )}
                  />
                </Form.Item>
              </Col>

              {/* Right Column */}
              <Col span={12}>
                {/* Warn Days Before First */}
                <Form.Item label={L('WarnDaysBeforeFirst')}>
                  <Controller
                    name="warnDaysBeforeFirst"
                    control={control}
                    render={({ field }) => (
                      <InputNumber
                        {...field}
                        style={{ width: '100%' }}
                        min={0}
                        placeholder={L('WarnDaysBeforeFirst')}
                      />
                    )}
                  />
                </Form.Item>

                {/* First Warn Status */}
                <Form.Item label={L('FirstWarnStatus')}>
                  <Controller
                    name="firstWarnStatusId"
                    control={control}
                    render={({ field }) => (
                      <Select
                        {...field}
                        placeholder={L('SelectFirstWarnStatus')}
                        options={recordStatuses}
                        allowClear
                      />
                    )}
                  />
                </Form.Item>

                {/* Warn Days Before Second */}
                <Form.Item label={L('WarnDaysBeforeSecond')}>
                  <Controller
                    name="warnDaysBeforeSecond"
                    control={control}
                    render={({ field }) => (
                      <InputNumber
                        {...field}
                        style={{ width: '100%' }}
                        min={0}
                        placeholder={L('WarnDaysBeforeSecond')}
                      />
                    )}
                  />
                </Form.Item>

                {/* Second Warn Status */}
                <Form.Item label={L('SecondWarnStatus')}>
                  <Controller
                    name="secondWarnStatusId"
                    control={control}
                    render={({ field }) => (
                      <Select
                        {...field}
                        placeholder={L('SelectSecondWarnStatus')}
                        options={recordStatuses}
                        allowClear
                      />
                    )}
                  />
                </Form.Item>

                {/* Warn Days Before Final */}
                <Form.Item label={L('WarnDaysBeforeFinal')}>
                  <Controller
                    name="warnDaysBeforeFinal"
                    control={control}
                    render={({ field }) => (
                      <InputNumber
                        {...field}
                        style={{ width: '100%' }}
                        min={0}
                        placeholder={L('WarnDaysBeforeFinal')}
                      />
                    )}
                  />
                </Form.Item>

                {/* Final Warn Status */}
                <Form.Item label={L('FinalWarnStatus')}>
                  <Controller
                    name="finalWarnStatusId"
                    control={control}
                    render={({ field }) => (
                      <Select
                        {...field}
                        placeholder={L('SelectFinalWarnStatus')}
                        options={recordStatuses}
                        allowClear
                      />
                    )}
                  />
                </Form.Item>

                {/* IsSurpathOnly - Only visible to Host users */}
                {isHost && (
                  <Form.Item label={L('IsSurpathOnly')}>
                    <Controller
                      name="isSurpathOnly"
                      control={control}
                      render={({ field }) => (
                        <Switch
                          checked={field.value}
                          onChange={field.onChange}
                          checkedChildren={L('Yes')}
                          unCheckedChildren={L('No')}
                        />
                      )}
                    />
                  </Form.Item>
                )}

                {/* MetaData */}
                <Form.Item label={L('MetaData')}>
                  <Controller
                    name="metaData"
                    control={control}
                    render={({ field }) => (
                      <Input.TextArea
                        {...field}
                        placeholder={L('MetaData')}
                        rows={4}
                      />
                    )}
                  />
                </Form.Item>
              </Col>
            </Row>

            {/* Action Buttons */}
            <Form.Item style={{ marginTop: 24 }}>
              <Space>
                <Button
                  type="primary"
                  icon={<SaveOutlined />}
                  loading={saving}
                  onClick={handleSubmit((data) => onSubmit(data, false))}
                  size="large"
                >
                  {L('Save')}
                </Button>
                {!isEditMode && (
                  <Button
                    icon={<PlusOutlined />}
                    loading={saving}
                    onClick={handleSubmit((data) => onSubmit(data, true))}
                    size="large"
                  >
                    {L('SaveAndNew')}
                  </Button>
                )}
                <Button
                  onClick={() => navigate('/app/compliance/record-category-rules')}
                  size="large"
                >
                  {L('Cancel')}
                </Button>
              </Space>
            </Form.Item>
          </Form>
        </Spin>
      </Card>
    </div>
  );
};

export default CreateOrEditRecordCategoryRulePage;
