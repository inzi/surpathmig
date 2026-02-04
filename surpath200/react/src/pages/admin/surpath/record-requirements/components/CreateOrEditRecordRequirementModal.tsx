import React, { useState, useEffect, useRef } from 'react';
import { Modal, Input, Select, Checkbox, App } from 'antd';
import { useForm, Controller } from 'react-hook-form';
import L from '@/lib/L';
import { useDelayedFocus } from '@/hooks/useDelayedFocus';
import {
  recordRequirementsService,
  type CreateOrEditRecordRequirementDto,
  type GetRecordRequirementForEditOutput,
  type RecordRequirementCohortLookupTableDto,
  type RecordRequirementSurpathServiceLookupTableDto,
  type RecordRequirementTenantSurpathServiceLookupTableDto,
} from '@/services/surpath/recordRequirements.service';

/**
 * CreateOrEditRecordRequirementModal
 *
 * Migrated from Surpath112:
 * - Source: wwwroot/view-resources/Areas/App/Views/Compliance/_CreateEditRequirementModal.js
 * - View: Areas/App/Views/Compliance/CreateEditRequirementModal.cshtml
 *
 * Simplified initial version focusing on core CRUD operations.
 * Category management (complex tree/wizard) handled by separate ManageCategoriesModal.
 *
 * Fields:
 * - Name (required)
 * - Description
 * - Metadata
 * - IsSurpathOnly
 * - TenantDepartmentId (lookup)
 * - CohortId (lookup)
 * - SurpathServiceId (dropdown)
 * - TenantSurpathServiceId (dropdown)
 */

interface Props {
  visible: boolean;
  recordRequirementId?: string;
  onCancel: () => void;
  onSave: () => void;
}

const CreateOrEditRecordRequirementModal: React.FC<Props> = ({
  visible,
  recordRequirementId,
  onCancel,
  onSave,
}) => {
  const { message } = App.useApp();
  const {
    control,
    handleSubmit,
    reset,
    formState: { errors, isValid },
  } = useForm<CreateOrEditRecordRequirementDto>({ mode: 'onChange' });

  const [saving, setSaving] = useState(false);
  const [loading, setLoading] = useState(false);
  const [editData, setEditData] = useState<GetRecordRequirementForEditOutput | null>(null);

  // Lookup data
  const [cohorts, setCohorts] = useState<RecordRequirementCohortLookupTableDto[]>([]);
  const [surpathServices, setSurpathServices] = useState<RecordRequirementSurpathServiceLookupTableDto[]>([]);
  const [tenantSurpathServices, setTenantSurpathServices] = useState<RecordRequirementTenantSurpathServiceLookupTableDto[]>([]);

  const firstInputRef = useRef<HTMLInputElement | null>(null);
  const delayedFocus = useDelayedFocus();

  // Load data when modal opens
  useEffect(() => {
    if (!visible) {
      return;
    }

    const loadData = async () => {
      setLoading(true);
      try {
        // Load lookup data
        const [cohortsResult, surpathServicesResult, tenantSurpathServicesResult] = await Promise.all([
          recordRequirementsService.getAllCohortForLookupTable({ maxResultCount: 1000, skipCount: 0 }),
          recordRequirementsService.getAllSurpathServiceForTableDropdown(),
          recordRequirementsService.getAllTenantSurpathServiceForTableDropdown(),
        ]);

        setCohorts(cohortsResult.items || []);
        setSurpathServices(surpathServicesResult || []);
        setTenantSurpathServices(tenantSurpathServicesResult || []);

        // Load record if editing
        if (recordRequirementId) {
          const result = await recordRequirementsService.getRecordRequirementForEdit({ id: recordRequirementId });
          setEditData(result);
          reset(result.recordRequirement);
        } else {
          // Initialize with defaults for new record
          reset({
            id: undefined,
            name: '',
            description: '',
            metadata: '',
            isSurpathOnly: false,
            tenantDepartmentId: undefined,
            cohortId: undefined,
            surpathServiceId: undefined,
            tenantSurpathServiceId: undefined,
          });
        }
      } catch (error: any) {
        message.error(error.message || L('AnErrorOccurred'));
      } finally {
        setLoading(false);
      }
    };

    loadData();
  }, [visible, recordRequirementId, reset, message]);

  const onSubmit = async (values: CreateOrEditRecordRequirementDto) => {
    setSaving(true);
    try {
      // Set the ID if editing
      if (recordRequirementId) {
        values.id = recordRequirementId;
      }

      await recordRequirementsService.createOrEdit(values);
      message.success(L('SavedSuccessfully'));
      onSave();
      onCancel();
    } catch (error: any) {
      message.error(error.message || L('AnErrorOccurred'));
    } finally {
      setSaving(false);
    }
  };

  return (
    <Modal
      title={
        recordRequirementId
          ? `${L('Edit')}: ${editData?.recordRequirement.name || ''}`
          : L('CreateNewRecordRequirement')
      }
      open={visible}
      onCancel={onCancel}
      afterOpenChange={(open) => {
        if (open) {
          delayedFocus(firstInputRef);
        }
      }}
      width={800}
      footer={[
        <button
          key="cancel"
          type="button"
          className="btn btn-light-primary fw-bold"
          onClick={onCancel}
          disabled={saving}
        >
          {L('Cancel')}
        </button>,
        <button
          key="save"
          type="submit"
          className="btn btn-primary fw-bold d-inline-flex align-items-center"
          onClick={handleSubmit(onSubmit)}
          disabled={!isValid || saving || loading}
        >
          <i className="fa fa-save align-middle me-2"></i>
          <span className="align-middle">{L('Save')}</span>
        </button>,
      ]}
    >
      <div className="form" aria-disabled={loading}>
        {/* Name - Required */}
        <div className="mb-5">
          <label className="form-label required" htmlFor="Name">
            {L('Name')}
          </label>
          <Controller
            name="name"
            control={control}
            rules={{ required: true, maxLength: 255 }}
            render={({ field }) => (
              <Input
                {...field}
                id="Name"
                className={errors.name ? 'is-invalid' : ''}
                disabled={loading}
                ref={(el) => {
                  field.ref(el);
                  if (el) {
                    firstInputRef.current = el.input;
                  }
                }}
                maxLength={255}
              />
            )}
          />
          {errors.name && (
            <div className="invalid-feedback d-block">{L('ThisFieldIsRequired')}</div>
          )}
        </div>

        {/* Description */}
        <div className="mb-5">
          <label className="form-label" htmlFor="Description">
            {L('Description')}
          </label>
          <Controller
            name="description"
            control={control}
            render={({ field }) => (
              <Input.TextArea
                {...field}
                id="Description"
                disabled={loading}
                rows={3}
                maxLength={1000}
              />
            )}
          />
        </div>

        {/* Metadata */}
        <div className="mb-5">
          <label className="form-label" htmlFor="Metadata">
            {L('Metadata')}
          </label>
          <Controller
            name="metadata"
            control={control}
            render={({ field }) => (
              <Input.TextArea
                {...field}
                id="Metadata"
                disabled={loading}
                rows={2}
                maxLength={500}
              />
            )}
          />
        </div>

        {/* IsSurpathOnly */}
        <div className="mb-5">
          <div className="form-check form-check-custom form-check-solid">
            <Controller
              name="isSurpathOnly"
              control={control}
              render={({ field }) => (
                <Checkbox
                  {...field}
                  id="IsSurpathOnly"
                  disabled={loading}
                  checked={field.value}
                >
                  {L('IsSurpathOnly')}
                </Checkbox>
              )}
            />
          </div>
        </div>

        {/* Cohort Lookup */}
        <div className="mb-5">
          <label className="form-label" htmlFor="CohortId">
            {L('Cohort')}
          </label>
          <Controller
            name="cohortId"
            control={control}
            render={({ field }) => (
              <Select
                {...field}
                id="CohortId"
                disabled={loading}
                placeholder={L('SelectCohort')}
                style={{ width: '100%' }}
                allowClear
                showSearch
                filterOption={(input, option) =>
                  (option?.label ?? '').toLowerCase().includes(input.toLowerCase())
                }
                options={cohorts.map((c) => ({
                  value: c.id,
                  label: c.displayName,
                }))}
              />
            )}
          />
        </div>

        {/* TenantDepartment - Note: This would need a lookup modal in full implementation */}
        {/* For now, we'll skip it or use a simple input for the ID */}

        {/* SurpathService Dropdown */}
        <div className="mb-5">
          <label className="form-label" htmlFor="SurpathServiceId">
            {L('SurpathService')}
          </label>
          <Controller
            name="surpathServiceId"
            control={control}
            render={({ field }) => (
              <Select
                {...field}
                id="SurpathServiceId"
                disabled={loading}
                placeholder={L('SelectSurpathService')}
                style={{ width: '100%' }}
                allowClear
                showSearch
                filterOption={(input, option) =>
                  (option?.label ?? '').toLowerCase().includes(input.toLowerCase())
                }
                options={surpathServices.map((s) => ({
                  value: s.id,
                  label: s.displayName,
                }))}
              />
            )}
          />
        </div>

        {/* TenantSurpathService Dropdown */}
        <div className="mb-5">
          <label className="form-label" htmlFor="TenantSurpathServiceId">
            {L('TenantSurpathService')}
          </label>
          <Controller
            name="tenantSurpathServiceId"
            control={control}
            render={({ field }) => (
              <Select
                {...field}
                id="TenantSurpathServiceId"
                disabled={loading}
                placeholder={L('SelectTenantSurpathService')}
                style={{ width: '100%' }}
                allowClear
                showSearch
                filterOption={(input, option) =>
                  (option?.label ?? '').toLowerCase().includes(input.toLowerCase())
                }
                options={tenantSurpathServices.map((s) => ({
                  value: s.id,
                  label: s.displayName,
                }))}
              />
            )}
          />
        </div>

        {/* Note about category management */}
        {recordRequirementId && (
          <div className="alert alert-info">
            <i className="fa fa-info-circle me-2"></i>
            {L('UseManageCategoriesActionToConfigureCategories')}
          </div>
        )}
      </div>
    </Modal>
  );
};

export default CreateOrEditRecordRequirementModal;
