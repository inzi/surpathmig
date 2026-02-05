import React from "react";
import { Modal, Progress } from "antd";
import { useSessionTimeout } from "./useSessionTimeout";
import L from "@/lib/L";

const SessionTimeoutModal: React.FC<Record<string, never>> = () => {
  const { isOpen, secondsRemaining, progressPercent, close } =
    useSessionTimeout();

  return (
    <Modal
      open={isOpen}
      title={L("YourSessionIsAboutToExpire")}
      onCancel={close}
      footer={null}
      maskClosable={false}
      closable
      destroyOnHidden
    >
      <p>
        {L("SessionExpireRedirectingInXSecond", {
          0: secondsRemaining,
          seconds: secondsRemaining,
        })}
      </p>

      <div className="bg-light-primary rounded p-2">
        <Progress
          percent={Math.round(progressPercent)}
          showInfo
          format={() => `${secondsRemaining}`}
        />
      </div>
    </Modal>
  );
};

export default SessionTimeoutModal;
