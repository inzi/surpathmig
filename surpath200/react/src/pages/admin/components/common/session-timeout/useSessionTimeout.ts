import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import { useAuth } from "../../../../../hooks/useAuth";
import { useNavigate } from "react-router-dom";

type NullableNumber = number | null;

const LAST_ACTIVITY_KEY = "Abp.SessionTimeOut.UserLastActivityTime";
const DEFAULT_TIMEOUT_SECONDS = 15 * 60;
const DEFAULT_NOTIFY_SECONDS = 30;

function getSettingString(settingName: string): string | undefined {
  if (abp?.setting?.get) {
    return abp.setting.get(settingName);
  }
  return undefined;
}

function getSettingInt(settingName: string, defaultValue: number): number {
  const raw = getSettingString(settingName);
  const parsed = raw !== undefined ? parseInt(raw, 10) : NaN;
  return Number.isFinite(parsed) ? parsed : defaultValue;
}

function writeLastActivity(timestampMs: number): void {
  if (typeof localStorage !== "undefined") {
    localStorage.setItem(LAST_ACTIVITY_KEY, String(timestampMs));
    return;
  }
  abp?.utils?.setCookieValue?.(LAST_ACTIVITY_KEY, String(timestampMs));
}

function readLastActivity(): NullableNumber {
  if (typeof localStorage !== "undefined") {
    const value = localStorage.getItem(LAST_ACTIVITY_KEY);
    return value ? parseInt(value, 10) : null;
  }
  const value = abp?.utils?.getCookieValue?.(LAST_ACTIVITY_KEY);
  return value ? parseInt(value, 10) : null;
}

export interface UseSessionTimeoutState {
  isOpen: boolean;
  secondsRemaining: number;
  progressPercent: number;
  close: () => void;
}

export function useSessionTimeout(): UseSessionTimeoutState {
  const { logout } = useAuth();
  const navigate = useNavigate();

  const timeOutSeconds = useMemo(
    () =>
      getSettingInt(
        "App.UserManagement.SessionTimeOut.TimeOutSecond",
        DEFAULT_TIMEOUT_SECONDS,
      ),
    [],
  );
  const notifySeconds = useMemo(
    () =>
      getSettingInt(
        "App.UserManagement.SessionTimeOut.ShowTimeOutNotificationSecond",
        DEFAULT_NOTIFY_SECONDS,
      ),
    [],
  );
  const showLockScreen = useMemo(
    () =>
      abp?.setting.getBoolean?.(
        "App.UserManagement.SessionTimeOut.ShowLockScreenWhenTimedOut",
      ),
    [],
  );

  const [isOpen, setIsOpen] = useState(false);
  const [secondsRemaining, setSecondsRemaining] =
    useState<number>(notifySeconds);
  const [progressPercent, setProgressPercent] = useState<number>(100);

  const countdownIntervalRef = useRef<number | null>(null);
  const activityListenerAttachedRef = useRef(false);

  const close = useCallback(() => {
    setIsOpen(false);
    setSecondsRemaining(notifySeconds);
    setProgressPercent(100);
    if (countdownIntervalRef.current) {
      window.clearInterval(countdownIntervalRef.current);
      countdownIntervalRef.current = null;
    }
  }, [notifySeconds]);

  const done = useCallback(() => {
    close();
    if (showLockScreen) {
      navigate("/lock-screen");
    }
    logout();
  }, [close, logout, showLockScreen, navigate]);

  const startCountdown = useCallback(() => {
    setSecondsRemaining(notifySeconds);
    setProgressPercent(100);

    if (countdownIntervalRef.current) {
      window.clearInterval(countdownIntervalRef.current);
    }
    countdownIntervalRef.current = window.setInterval(() => {
      setSecondsRemaining((prev) => {
        const next = prev - 1;
        if (next <= 0) {
          setProgressPercent(0);
          window.clearInterval(countdownIntervalRef.current!);
          countdownIntervalRef.current = null;
          setTimeout(done, 0);
          return 0;
        }
        setProgressPercent(
          Math.max(0, Math.min(100, (next / notifySeconds) * 100)),
        );
        return next;
      });
    }, 1000);
  }, [done, notifySeconds]);

  useEffect(() => {
    if (activityListenerAttachedRef.current) return;
    activityListenerAttachedRef.current = true;

    const touchActivity = () => {
      writeLastActivity(Date.now());
      if (isOpen) {
        close();
      }
    };

    const events: Array<keyof WindowEventMap> = [
      "mousemove",
      "mousedown",
      "click",
      "scroll",
      "keypress",
      "touchstart",
    ];

    events.forEach((evt) =>
      window.addEventListener(evt, touchActivity, { passive: true }),
    );

    writeLastActivity(Date.now());

    return () => {
      events.forEach((evt) => window.removeEventListener(evt, touchActivity));
    };
  }, [isOpen, close]);

  useEffect(() => {
    const intervalId = window.setInterval(() => {
      const last = readLastActivity();
      if (!last) return;
      const inactiveMs = Date.now() - last;
      const thresholdMs = timeOutSeconds * 1000;

      if (inactiveMs > thresholdMs) {
        if (!isOpen) {
          setIsOpen(true);
          startCountdown();
        }
      } else if (isOpen) {
        close();
      }
    }, 1000);

    return () => window.clearInterval(intervalId);
  }, [close, isOpen, startCountdown, timeOutSeconds]);

  return {
    isOpen,
    secondsRemaining,
    progressPercent,
    close,
  };
}
