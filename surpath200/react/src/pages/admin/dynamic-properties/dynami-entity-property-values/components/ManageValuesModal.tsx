import React, { useRef } from "react";
import { Modal } from "antd";
import type { DynamicPropertyValueManagerRef } from "./DynamicPropertyValueManager";
import DynamicPropertyValueManager from "./DynamicPropertyValueManager";
import L from "@/lib/L";

interface Props {
  isVisible: boolean;
  onClose: () => void;
  entityFullName?: string;
  entityId?: string;
}

const ManageValuesModal: React.FC<Props> = ({
  isVisible,
  onClose,
  entityFullName,
  entityId,
}) => {
  const managerRef = useRef<DynamicPropertyValueManagerRef>(null);

  const handleSave = () => {
    managerRef.current?.saveAll();
    onClose();
  };

  return (
    <Modal
      title={`${L("EntityFullName")}: ${entityFullName} - ${L(
        "EntityId",
      )}: ${entityId}`}
      open={isVisible}
      onCancel={onClose}
      width={1000}
      footer={[
        <button
          key="cancel"
          type="button"
          className="btn btn-light-primary fw-bold"
          onClick={onClose}
        >
          {L("Cancel")}
        </button>,
        <button
          key="save"
          type="submit"
          className="btn btn-primary fw-bold d-inline-flex align-items-center"
          onClick={handleSave}
        >
          <i className="fa fa-save align-middle me-2"></i>
          <span className="align-middle">{L("Save")}</span>
        </button>,
      ]}
    >
      {entityFullName && entityId && (
        <DynamicPropertyValueManager
          ref={managerRef}
          entityFullName={entityFullName}
          entityId={entityId}
        />
      )}
    </Modal>
  );
};

export default ManageValuesModal;
