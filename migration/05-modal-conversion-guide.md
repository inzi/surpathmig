# Modal Conversion Guide: jQuery Modals → React Modals

## Overview

ASPNetZero MVC uses Bootstrap modals with jQuery. The React version typically uses Ant Design Modal components. This guide covers the conversion patterns, especially for forms with business logic.

---

## jQuery Modal Pattern (Surpath112)

### HTML Modal Structure
```html
<!-- Views/SurpathModule/_CreateOrEditModal.cshtml -->
<div class="modal fade" id="CreateOrEditModal" tabindex="-1" role="dialog">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">
                    <span id="ModalTitle"></span>
                </h5>
                <button type="button" class="close" data-dismiss="modal">
                    <span>&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <form id="CreateOrEditForm">
                    <input type="hidden" id="Id" name="Id" />
                    
                    <div class="form-group">
                        <label for="Name">@L("Name")</label>
                        <input type="text" id="Name" name="Name" class="form-control" required />
                    </div>
                    
                    <div class="form-group">
                        <label for="Description">@L("Description")</label>
                        <textarea id="Description" name="Description" class="form-control"></textarea>
                    </div>
                    
                    <div class="form-group">
                        <label for="Status">@L("Status")</label>
                        <select id="Status" name="Status" class="form-control">
                            <option value="active">@L("Active")</option>
                            <option value="inactive">@L("Inactive")</option>
                        </select>
                    </div>
                    
                    <!-- Custom business logic fields -->
                    <div class="form-group" id="AdvancedOptions" style="display: none;">
                        <label for="AdvancedField">@L("AdvancedField")</label>
                        <input type="text" id="AdvancedField" name="AdvancedField" class="form-control" />
                    </div>
                </form>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">
                    @L("Cancel")
                </button>
                <button type="button" class="btn btn-primary" id="SaveButton">
                    @L("Save")
                </button>
            </div>
        </div>
    </div>
</div>
```

### JavaScript Modal Controller
```javascript
// Views/SurpathModule/Index.js
(function() {
    var _$modal = $('#CreateOrEditModal');
    var _$form = _$modal.find('form');
    var _surpathModuleService = abp.services.app.surpathModule;

    // Open modal for create
    function openCreateModal() {
        _$form[0].reset();
        $('#Id').val('');
        $('#ModalTitle').text(app.localize('CreateNewSurpathModule'));
        $('#AdvancedOptions').hide();
        _$modal.modal('show');
    }

    // Open modal for edit
    function openEditModal(id) {
        abp.ui.setBusy(_$modal);
        
        _surpathModuleService.get({ id: id })
            .done(function(result) {
                // Populate form
                $('#Id').val(result.id);
                $('#Name').val(result.name);
                $('#Description').val(result.description);
                $('#Status').val(result.status);
                
                // Business logic: show advanced options based on status
                if (result.status === 'active') {
                    $('#AdvancedOptions').show();
                    $('#AdvancedField').val(result.advancedField);
                }
                
                $('#ModalTitle').text(app.localize('EditSurpathModule'));
                _$modal.modal('show');
            })
            .always(function() {
                abp.ui.clearBusy(_$modal);
            });
    }

    // Save handler
    $('#SaveButton').click(function() {
        if (!_$form.valid()) {
            return;
        }

        var formData = _$form.serializeFormToObject();
        
        // Business logic: validate before save
        if (formData.status === 'active' && !formData.advancedField) {
            abp.message.warn(app.localize('AdvancedFieldRequired'));
            return;
        }

        abp.ui.setBusy(_$modal);

        var saveFunction = formData.id
            ? _surpathModuleService.update
            : _surpathModuleService.create;

        saveFunction(formData)
            .done(function() {
                abp.notify.success(app.localize('SavedSuccessfully'));
                _$modal.modal('hide');
                refreshTable();
            })
            .always(function() {
                abp.ui.clearBusy(_$modal);
            });
    });

    // Business logic: toggle advanced options based on status change
    $('#Status').change(function() {
        var status = $(this).val();
        if (status === 'active') {
            $('#AdvancedOptions').slideDown();
        } else {
            $('#AdvancedOptions').slideUp();
            $('#AdvancedField').val('');
        }
    });

    // Export functions
    window.openCreateModal = openCreateModal;
    window.openEditModal = openEditModal;
})();
```

---

## React Modal Pattern (Surpath200)

### Modal Component
```tsx
// components/SurpathModule/CreateOrEditModal.tsx
import React, { useState, useEffect } from 'react';
import { Modal, Form, Input, Select, message, Spin } from 'antd';
import { L } from '@lib/abpUtility';
import { surpathModuleService } from '@services/surpathModule';

interface SurpathModuleDto {
    id?: number;
    name: string;
    description: string;
    status: string;
    advancedField?: string;
}

interface CreateOrEditModalProps {
    visible: boolean;
    id?: number; // undefined = create, number = edit
    onCancel: () => void;
    onSuccess: () => void;
}

const CreateOrEditModal: React.FC<CreateOrEditModalProps> = ({
    visible,
    id,
    onCancel,
    onSuccess
}) => {
    const [form] = Form.useForm();
    const [loading, setLoading] = useState(false);
    const [saving, setSaving] = useState(false);
    const [showAdvancedOptions, setShowAdvancedOptions] = useState(false);

    const isEditMode = id !== undefined;

    // Load data for edit mode
    useEffect(() => {
        if (visible && isEditMode) {
            loadData();
        } else if (visible && !isEditMode) {
            // Reset form for create mode
            form.resetFields();
            setShowAdvancedOptions(false);
        }
    }, [visible, id]);

    const loadData = async () => {
        setLoading(true);
        try {
            const result = await surpathModuleService.get({ id: id! });
            form.setFieldsValue(result);
            
            // Business logic: show advanced options based on status
            if (result.status === 'active') {
                setShowAdvancedOptions(true);
            }
        } catch (error) {
            message.error(error.message);
        } finally {
            setLoading(false);
        }
    };

    // Business logic: toggle advanced options based on status change
    const handleStatusChange = (status: string) => {
        if (status === 'active') {
            setShowAdvancedOptions(true);
        } else {
            setShowAdvancedOptions(false);
            form.setFieldValue('advancedField', '');
        }
    };

    const handleSave = async () => {
        try {
            const values = await form.validateFields();
            
            // Business logic: validate before save
            if (values.status === 'active' && !values.advancedField) {
                message.warning(L('AdvancedFieldRequired'));
                return;
            }

            setSaving(true);
            
            if (isEditMode) {
                await surpathModuleService.update({ ...values, id });
            } else {
                await surpathModuleService.create(values);
            }

            message.success(L('SavedSuccessfully'));
            onSuccess();
        } catch (error) {
            if (error.errorFields) {
                // Form validation error - already shown
                return;
            }
            message.error(error.message);
        } finally {
            setSaving(false);
        }
    };

    const handleCancel = () => {
        form.resetFields();
        setShowAdvancedOptions(false);
        onCancel();
    };

    return (
        <Modal
            title={isEditMode ? L('EditSurpathModule') : L('CreateNewSurpathModule')}
            open={visible}
            onOk={handleSave}
            onCancel={handleCancel}
            confirmLoading={saving}
            okText={L('Save')}
            cancelText={L('Cancel')}
            width={600}
            destroyOnClose
        >
            <Spin spinning={loading}>
                <Form
                    form={form}
                    layout="vertical"
                    initialValues={{ status: 'inactive' }}
                >
                    <Form.Item
                        name="name"
                        label={L('Name')}
                        rules={[{ required: true, message: L('NameRequired') }]}
                    >
                        <Input />
                    </Form.Item>

                    <Form.Item
                        name="description"
                        label={L('Description')}
                    >
                        <Input.TextArea rows={3} />
                    </Form.Item>

                    <Form.Item
                        name="status"
                        label={L('Status')}
                        rules={[{ required: true }]}
                    >
                        <Select onChange={handleStatusChange}>
                            <Select.Option value="active">{L('Active')}</Select.Option>
                            <Select.Option value="inactive">{L('Inactive')}</Select.Option>
                        </Select>
                    </Form.Item>

                    {/* Business logic: conditional field display */}
                    {showAdvancedOptions && (
                        <Form.Item
                            name="advancedField"
                            label={L('AdvancedField')}
                        >
                            <Input />
                        </Form.Item>
                    )}
                </Form>
            </Spin>
        </Modal>
    );
};

export default CreateOrEditModal;
```

### Parent Component Usage
```tsx
// components/SurpathModule/SurpathModuleIndex.tsx
import React, { useState } from 'react';
import { Button } from 'antd';
import { L } from '@lib/abpUtility';
import { usePermission } from '@hooks/usePermission';
import SurpathModuleTable from './SurpathModuleTable';
import CreateOrEditModal from './CreateOrEditModal';

const SurpathModuleIndex: React.FC = () => {
    const canCreate = usePermission('Pages.SurpathModule.Create');
    
    const [modalVisible, setModalVisible] = useState(false);
    const [editId, setEditId] = useState<number | undefined>(undefined);
    const [refreshKey, setRefreshKey] = useState(0);

    const handleCreate = () => {
        setEditId(undefined);
        setModalVisible(true);
    };

    const handleEdit = (id: number) => {
        setEditId(id);
        setModalVisible(true);
    };

    const handleModalClose = () => {
        setModalVisible(false);
        setEditId(undefined);
    };

    const handleModalSuccess = () => {
        setModalVisible(false);
        setEditId(undefined);
        setRefreshKey(prev => prev + 1); // Trigger table refresh
    };

    return (
        <div>
            <div style={{ marginBottom: 16 }}>
                {canCreate && (
                    <Button type="primary" onClick={handleCreate}>
                        {L('CreateNew')}
                    </Button>
                )}
            </div>

            <SurpathModuleTable
                key={refreshKey}
                onEdit={handleEdit}
            />

            <CreateOrEditModal
                visible={modalVisible}
                id={editId}
                onCancel={handleModalClose}
                onSuccess={handleModalSuccess}
            />
        </div>
    );
};

export default SurpathModuleIndex;
```

---

## Business Logic Extraction

When migrating modals, identify and preserve these patterns:

### 1. Conditional Field Display
```javascript
// jQuery
if (result.status === 'active') {
    $('#AdvancedOptions').show();
}

// React
const [showAdvancedOptions, setShowAdvancedOptions] = useState(false);
// ... set based on data or status change
{showAdvancedOptions && <Form.Item>...</Form.Item>}
```

### 2. Field Dependencies
```javascript
// jQuery
$('#ParentField').change(function() {
    var value = $(this).val();
    loadChildOptions(value);
});

// React
const handleParentChange = async (value: string) => {
    const childOptions = await loadChildOptions(value);
    setChildOptions(childOptions);
    form.setFieldValue('childField', undefined); // Reset child
};
```

### 3. Custom Validation
```javascript
// jQuery
if (formData.status === 'active' && !formData.advancedField) {
    abp.message.warn('AdvancedFieldRequired');
    return;
}

// React - can use form rules or manual validation
// Option 1: Form rules with dependencies
<Form.Item
    name="advancedField"
    rules={[
        {
            validator: (_, value) => {
                const status = form.getFieldValue('status');
                if (status === 'active' && !value) {
                    return Promise.reject(L('AdvancedFieldRequired'));
                }
                return Promise.resolve();
            }
        }
    ]}
    dependencies={['status']}
>

// Option 2: Manual validation before save
const handleSave = async () => {
    const values = await form.validateFields();
    if (values.status === 'active' && !values.advancedField) {
        message.warning(L('AdvancedFieldRequired'));
        return;
    }
    // ... save
};
```

### 4. Pre-populate from Related Data
```javascript
// jQuery
$('#CustomerSelect').change(function() {
    var customerId = $(this).val();
    loadCustomerDefaults(customerId, function(defaults) {
        $('#Address').val(defaults.address);
        $('#Phone').val(defaults.phone);
    });
});

// React
const handleCustomerChange = async (customerId: number) => {
    const defaults = await customerService.getDefaults(customerId);
    form.setFieldsValue({
        address: defaults.address,
        phone: defaults.phone
    });
};
```

---

## Common Modal Patterns

### View-Only Modal
```tsx
// For displaying read-only details
<Modal
    title={L('ViewDetails')}
    open={visible}
    onCancel={onCancel}
    footer={[
        <Button key="close" onClick={onCancel}>
            {L('Close')}
        </Button>
    ]}
>
    <Descriptions column={1}>
        <Descriptions.Item label={L('Name')}>{data.name}</Descriptions.Item>
        <Descriptions.Item label={L('Status')}>{data.status}</Descriptions.Item>
    </Descriptions>
</Modal>
```

### Confirmation Modal
```javascript
// jQuery
abp.message.confirm(
    app.localize('DeleteConfirmation'),
    app.localize('AreYouSure'),
    function(confirmed) {
        if (confirmed) {
            deleteRecord(id);
        }
    }
);

// React
Modal.confirm({
    title: L('AreYouSure'),
    content: L('DeleteConfirmation'),
    okText: L('Yes'),
    cancelText: L('No'),
    onOk: () => deleteRecord(id)
});
```

### Multi-Step Modal (Wizard)
```tsx
// React with Steps component
const [currentStep, setCurrentStep] = useState(0);

<Modal open={visible}>
    <Steps current={currentStep}>
        <Steps.Step title={L('Step1')} />
        <Steps.Step title={L('Step2')} />
        <Steps.Step title={L('Step3')} />
    </Steps>
    
    {currentStep === 0 && <Step1Form />}
    {currentStep === 1 && <Step2Form />}
    {currentStep === 2 && <Step3Form />}
    
    <div>
        {currentStep > 0 && <Button onClick={() => setCurrentStep(c => c - 1)}>{L('Previous')}</Button>}
        {currentStep < 2 && <Button onClick={() => setCurrentStep(c => c + 1)}>{L('Next')}</Button>}
        {currentStep === 2 && <Button type="primary" onClick={handleSubmit}>{L('Submit')}</Button>}
    </div>
</Modal>
```

---

## Migration Checklist

- [ ] Identify all modals in Surpath112
- [ ] Document form fields for each modal
- [ ] Document validation rules
- [ ] Document conditional field display logic
- [ ] Document field dependencies
- [ ] Document custom validation logic
- [ ] Create React modal components
- [ ] Implement form validation
- [ ] Implement conditional rendering
- [ ] Implement field dependencies
- [ ] Implement save/cancel handlers
- [ ] Test create flow
- [ ] Test edit flow (data population)
- [ ] Test validation (required, custom)
- [ ] Verify business logic parity