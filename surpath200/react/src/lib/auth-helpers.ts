import axios from "axios";
import AppConsts from "./app-consts";
import { store } from "@/app/store";
import {
  setToken as setAuthToken,
  logout as authLogout,
} from "@/app/slices/authSlice";
import { clearSession } from "@/app/slices/sessionSlice";
import { loadSessionData } from "@/app/bootstrap/AppBootstrap";
import L from "./L";

export function hardLogout(): void {
  try {
    abp?.auth?.setToken?.("");
    abp?.utils?.deleteCookie?.("Abp.AuthToken", abp?.appPath);
    localStorage.removeItem(AppConsts.authorization.encrptedAuthTokenName);
    localStorage.removeItem("TwoFactorRememberClientToken");
    delete axios.defaults.headers.common["Authorization"];
    store.dispatch(clearSession());
    store.dispatch(authLogout());
    loadSessionData().catch(() => {});
  } catch {
    abp.message?.error?.(L("AnErrorOccurred"));
  }
}

export function setAuthTokens(
  accessToken: string,
  encryptedAccessToken: string,
  callback?: () => void,
): void {
  try {
    abp.auth.setToken(accessToken);
    axios.defaults.headers.common["Authorization"] = `Bearer ${accessToken}`;
    store.dispatch(setAuthToken(accessToken));
    setEncryptedTokenCookie(encryptedAccessToken, callback);
  } catch {
    abp.message?.error?.(L("AnErrorOccurred"));
  }
}

export function setEncryptedTokenCookie(
  encryptedAccessToken: string,
  callback?: () => void,
): void {
  localStorage.setItem(
    AppConsts.authorization.encrptedAuthTokenName,
    JSON.stringify({ token: encryptedAccessToken }),
  );
  if (callback) {
    callback();
  }
}
