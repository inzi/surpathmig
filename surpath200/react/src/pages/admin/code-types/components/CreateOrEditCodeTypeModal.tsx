import React, { useEffect, useState } from "react";
import { Modal, Form, Input, App } from "antd";
import { useForm } from "antd/es/form/Form";
import {
  CodeTypesServiceProxy,
  CreateOrEditCodeTypeDto,
} from "../codeTypes.service";
import { useServiceProxy } from "@/api/service-proxy-factory";
import L from "@/lib/L";

interface CreateOrEditCodeTypeModalProps {
  visible: boolean;
  onCancel: () => void;
  onSave: () => void;
  codeTypeId?: number;
}

const CreateOrEditCodeTypeModal: React.FC<
  CreateOrEditCodeTypeModalProps
> = ({ visible, onCancel, onSave, codeTypeId }) => {
  const [form] = useForm();
  const { message } = App.useApp();
  const codeTypesService = useServiceProxy(CodeTypesServiceProxy, []);
  const [loading, setLoading] = useState(false);
  const [saving, setSaving] = useState(false);

  const isEditMode = codeTypeId !== undefined;

  useEffect(() => {
    if (visible) {
      if (isEditMode) {
        loadCodeType();
      } else {
        form.resetFields();
      }
    }
  }, [visible, codeTypeId]);

  const loadCodeType = async () => {
    if (!codeTypeId) return;

    setLoading(true);
    try {
      const result = await codeTypesService.getCodeTypeForEdit(codeTypeId);
      form.setFieldsValue({
        name: result.codeType.name,
        id: result.codeType.id,
      });
    } catch (error) {
      console.error("Error loading code type:", error);
      message.error(L("ErrorOccurred"));
    } finally {
      setLoading(false);
    }
  };

  const handleSave = async () => {
    try {
      const values = await form.validateFields();

      setSaving(true);

      const input: CreateOrEditCodeTypeDto = {
        name: values.name,
        id: isEditMode ? codeTypeId : undefined,
      };

      await codeTypesService.createOrEdit(input);

      message.success(L("SavedSuccessfully"));
      form.resetFields();
      onSave();
    } catch (error: any) {
      if (error.errorFields) {
        // Validation errors - don't show error message
        return;
      }
      console.error("Error saving code type:", error);
      message.error(L("ErrorOccurred"));
    } finally {
      setSaving(false);
    }
  };

  const handleCancel = () => {
    form.resetFields();
    onCancel();
  };

  return (
    <Modal
      title={isEditMode ? L("EditCodeType") : L("CreateNewCodeType")}
      open={visible}
      onOk={handleSave}
      onCancel={handleCancel}
      confirmLoading={saving}
      okText={L("Save")}
      cancelText={L("Cancel")}
      width={600}
    >
      <Form
        form={form}
        layout="vertical"
        name="codeTypeForm"
        disabled={loading}
      >
        <Form.Item
          name="name"
          label={L("Name")}
          rules={[
            {
              required: true,
              message: L("ThisFieldIsRequired"),
            },
            {
              min: 1,
              max: 128,
              message: L("NameMustBeBetween1And128Characters"),
            },
          ]}
        >
          <Input
            placeholder={L("Name")}
            maxLength={128}
            autoFocus
          />
        </Form.Item>

        {isEditMode && (
          <Form.Item name="id" hidden>
            <Input type="hidden" />
          </Form.Item>
        )}
      </Form>
    </Modal>
  );
};

export default CreateOrEditCodeTypeModal;
