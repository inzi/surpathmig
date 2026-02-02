import React, { useCallback, useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import {
  AccountServiceProxy,
  PasswordlessAuthenticateModel,
  TokenAuthServiceProxy,
  VerifyPasswordlessLoginCodeInput,
} from "@api/generated/service-proxies";
import { useServiceProxy } from "@/api/service-proxy-factory";
import L from "@/lib/L";

function useQuery() {
  return new URLSearchParams(useLocation().search);
}

const ValidatePasswordlessLoginPage: React.FC = () => {
  const navigate = useNavigate();
  const q = useQuery();

  const accountService = useServiceProxy(AccountServiceProxy, []);
  const tokenAuth = useServiceProxy(TokenAuthServiceProxy, []);

  const [passwordlessCode, setPasswordlessCode] = useState("");
  const [remainingSeconds, setRemainingSeconds] = useState<number>(
    abp?.setting?.getInt("PasswordlessLoginCodeExpireSeconds") || 90,
  );
  const [submitting, setSubmitting] = useState(false);

  const selectedProviderInputValue = q.get("selectedProviderInputValue") || "";
  const selectedProvider = q.get("selectedProvider") || "";

  useEffect(() => {
    const id = setInterval(() => {
      setRemainingSeconds((s) => {
        const next = s - 1;
        if (next === 0) {
          abp?.message?.warn?.(L("TimeoutPleaseTryAgain"));
          navigate("/account/login");
        }
        return next;
      });
    }, 1000);
    return () => clearInterval(id);
  }, [navigate]);

  const processAuthResult = useCallback(
    (result: {
      accessToken?: string;
      expireInSeconds?: number;
      returnUrl?: string;
    }) => {
      const token: string | undefined = result?.accessToken;
      const expireSeconds: number = result?.expireInSeconds || 0;
      if (token) {
        abp?.auth?.setToken?.(
          token,
          new Date(Date.now() + expireSeconds * 1000),
        );
      }
      const returnUrl = result?.returnUrl || "";
      if (returnUrl) {
        window.location.href = returnUrl;
      } else {
        const initialUrl = window.location.href || "";
        if (initialUrl.includes("/account")) {
          const isHost = !abp?.session?.tenantId;
          window.location.href = isHost
            ? "/app/host-dashboard"
            : "/app/tenant-dashboard";
        } else {
          window.location.href = initialUrl;
        }
      }
    },
    [],
  );

  const onSubmit = useCallback(
    async (e: React.FormEvent) => {
      e.preventDefault();
      if (!selectedProviderInputValue || !passwordlessCode) {
        return;
      }
      setSubmitting(true);
      try {
        const verifyModel = new VerifyPasswordlessLoginCodeInput();
        verifyModel.providerValue = selectedProviderInputValue;
        verifyModel.code = passwordlessCode;
        await accountService.verifyPasswordlessLoginCode(verifyModel);

        const singleSignIn =
          new URLSearchParams(window.location.search).get("singleSignIn") ===
          "true";
        const returnUrl =
          new URLSearchParams(window.location.search).get("returnUrl") ||
          undefined;

        const authModel = new PasswordlessAuthenticateModel();
        authModel.providerValue = selectedProviderInputValue;
        authModel.provider = selectedProvider;
        authModel.verificationCode = passwordlessCode;
        authModel.singleSignIn = singleSignIn;
        authModel.returnUrl = returnUrl;

        const result = await tokenAuth.passwordlessAuthenticate(authModel);
        processAuthResult(result);
      } finally {
        setSubmitting(false);
      }
    },
    [
      accountService,
      passwordlessCode,
      processAuthResult,
      selectedProvider,
      selectedProviderInputValue,
      tokenAuth,
    ],
  );

  return (
    <div className="login-form">
      <div className="pb-13 pt-lg-0 pt-5">
        <h3>{L("VerifyPasswordlessCode")}</h3>
      </div>
      <p className="pb-5">
        {L("ConfirmationPasswordlessCodeSentTo")}: {selectedProviderInputValue}
      </p>
      <form className="login-form form" onSubmit={onSubmit}>
        <div className="mb-5">
          <input
            autoFocus
            className="form-control form-control-solid h-auto py-7 px-6 rounded-lg fs-h6"
            type="text"
            autoComplete="one-time-code"
            placeholder={L("Code")}
            name="passwordlessCode"
            required
            maxLength={6}
            value={passwordlessCode}
            onChange={(e) => setPasswordlessCode(e.target.value)}
          />
        </div>

        <div className="pb-lg-0 pb-5">
          <button
            type="submit"
            className="btn btn-primary fw-bolder fs-h6 px-8 py-4 my-3 me-3"
            disabled={!passwordlessCode || submitting}
          >
            {L("Submit")}
          </button>
          {remainingSeconds >= 0 && (
            <span className="remaining-time-counter ml-5">
              {L("RemainingTime")}: {remainingSeconds}
            </span>
          )}
        </div>
      </form>
    </div>
  );
};

export default ValidatePasswordlessLoginPage;
