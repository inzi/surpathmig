import { useCallback } from "react";

export function useDelayedFocus(delay = 100) {
  return useCallback(
    (ref: React.RefObject<HTMLElement>) => {
      setTimeout(() => {
        ref?.current?.focus();
      }, delay);
    },
    [delay]
  );
}
