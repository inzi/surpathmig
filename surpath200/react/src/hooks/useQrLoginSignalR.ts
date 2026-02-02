import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from "@microsoft/signalr";
import AppConsts from "../lib/app-consts";
import L from "@/lib/L";

type StartOptions = {
  autoStart?: boolean;
  maxSessionSetCount?: number;
};

export type UseQrLoginSignalR = {
  start: () => Promise<void>;
  stop: () => void;
  isConnected: boolean;
  connection: HubConnection | null;
};

export function useQrLoginSignalR(
  options: StartOptions = {},
): UseQrLoginSignalR {
  const { autoStart = true, maxSessionSetCount = 5 } = options;

  const [isConnected, setIsConnected] = useState<boolean>(false);
  const connectionRef = useRef<HubConnection | null>(null);
  const sessionIntervalRef = useRef<ReturnType<typeof setInterval> | null>(
    null,
  );
  const sessionSetCountRef = useRef<number>(0);

  const hubUrl = useMemo(() => `${AppConsts.appBaseUrl}/signalr-qr-login`, []);

  const clearSessionInterval = useCallback(() => {
    if (sessionIntervalRef.current) {
      clearInterval(sessionIntervalRef.current);
      sessionIntervalRef.current = null;
    }
  }, []);

  const afterStopSessionInterval = useCallback(() => {
    abp?.message?.confirm?.(
      L("QrCodeExpiredMessage"),
      L("QrCodeExpiredTitle"),
      (isConfirmed: boolean) => {
        if (isConfirmed) {
          location.reload();
        }
      },
    );
  }, []);

  const setSessionId = useCallback(async () => {
    const conn = connectionRef.current;
    if (!conn) return;
    try {
      await conn.invoke("setSessionId");
      sessionSetCountRef.current += 1;
    } catch {
      abp?.message?.warn?.("QR login setSessionId failed");
    }
  }, []);

  const start = useCallback(async () => {
    if (connectionRef.current) return;

    const conn = new HubConnectionBuilder()
      .withUrl(hubUrl)
      .configureLogging(LogLevel.Warning)
      .withAutomaticReconnect()
      .build();

    connectionRef.current = conn;

    const onGetAuthData = (message: unknown) => {
      abp?.event?.trigger?.("app.qrlogin.getAuthData", message);
    };
    const onGenerateQrCode = (message: unknown) => {
      abp?.event?.trigger?.("app.qrlogin.generateQrCode", message);
    };

    conn.on("getAuthData", onGetAuthData);
    conn.on("generateQrCode", onGenerateQrCode);

    conn.onclose((e) => {
      if (e) {
        abp?.message?.warn?.(
          "QR login SignalR connection closed with error: " + e,
        );
      } else {
        abp?.message?.warn?.("QR login SignalR disconnected");
      }
      setIsConnected(false);
    });

    conn.onreconnecting(() => setIsConnected(false));
    conn.onreconnected(async () => {
      setIsConnected(true);
      sessionSetCountRef.current = 0;
      clearSessionInterval();
      await setSessionId();
      sessionIntervalRef.current = setInterval(async () => {
        if (sessionSetCountRef.current >= maxSessionSetCount) {
          clearSessionInterval();
          afterStopSessionInterval();
        } else {
          await setSessionId();
        }
      }, 60000);
    });

    try {
      await conn.start();
      setIsConnected(true);
      sessionSetCountRef.current = 0;
      await setSessionId();
      sessionIntervalRef.current = setInterval(async () => {
        if (sessionSetCountRef.current >= maxSessionSetCount) {
          clearSessionInterval();
          afterStopSessionInterval();
        } else {
          await setSessionId();
        }
      }, 60000);
    } catch (error) {
      abp?.message?.error?.("SignalR connection error: ");
      throw error;
    }
  }, [
    afterStopSessionInterval,
    clearSessionInterval,
    hubUrl,
    maxSessionSetCount,
    setSessionId,
  ]);

  const stop = useCallback(() => {
    clearSessionInterval();
    const conn = connectionRef.current;
    if (conn) {
      connectionRef.current = null;
      conn.stop().catch(() => {});
    }
    setIsConnected(false);
  }, [clearSessionInterval]);

  useEffect(() => {
    if (autoStart) {
      start().catch(() => {});
    }
    return () => {
      stop();
    };
  }, [autoStart, start, stop]);

  return {
    start,
    stop,
    isConnected,
    connection: connectionRef.current,
  };
}

export default useQrLoginSignalR;
