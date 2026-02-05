import React, { useMemo, useState } from "react";
import { Outlet, useLocation, Link } from "react-router-dom";
import { useLogo } from "../admin/components/common/logo/useLogo";
import LanguageSwitch from "./components/LanguageSwitch";
import TenantChangeModal from "./components/TenantChangeModal";
import { useSession } from "@/hooks/useSession";
import L from "@/lib/L";
import AppConsts from "@/lib/app-consts";

const AccountIndex: React.FC = () => {
  const location = useLocation();
  const { getLogoUrl, getDefaultLogoUrl, skin } = useLogo();
  const { tenant } = useSession();
  const backgroundImageName = skin === "dark" ? "login-dark" : "login";
  const isSelectEditionPage = useMemo(
    () => location.pathname.includes("/account/select-edition"),
    [location.pathname],
  );

  const [useTenantLogo, setUseTenantLogo] = useState(true);
  const [tenantModalOpen, setTenantModalOpen] = useState(false);

  const displayedTenancyName = useMemo(() => {
    const name = tenant?.tenancyName?.trim();
    return name || "";
  }, [tenant?.tenancyName]);

  const tenantLogoUrl = useMemo(
    () => getLogoUrl(skin, false),
    [getLogoUrl, skin],
  );
  const defaultLogo = useMemo(
    () => getDefaultLogoUrl(skin, false),
    [getDefaultLogoUrl, skin],
  );

  const showTenantChange = useMemo(() => {
    try {
      const path = (location?.pathname || "").toLowerCase();
      if (!path) return false;

      const tenantChangeDisabledRoutes = [
        "register-tenant",
        "gateway-selection",
        "select-edition",
        "register-tenant-result",
        "email-activation",
        "confirm-email",
        "change-email",
        "verify-passwordless-login",
        "passwordless-login",
        "send-code",
        "verify-code",
        "stripe-pre-payment",
        "stripe-post-payment",
        "paypal-pre-payment",
        "paypal-post-payment",
        "stripe-cancel-payment",
      ];

      const isDisabledRoute = tenantChangeDisabledRoutes.some(
        (route) => path.indexOf(`/account/${route}`) >= 0,
      );
      if (isDisabledRoute) return false;

      const supportsTenancyNameInUrl = () => {
        try {
          const format = AppConsts.appBaseUrl;
          return !!format && format.indexOf("{TENANCY_NAME}") >= 0;
        } catch {
          return false;
        }
      };

      return !!abp?.multiTenancy?.isEnabled && !supportsTenancyNameInUrl();
    } catch {
      return false;
    }
  }, [location?.pathname]);

  return (
    <div className="d-flex flex-column flex-root" style={{ minHeight: '100vh' }}>
      <div
        className="d-flex flex-column flex-column-fluid bgi-position-y-bottom position-x-center bgi-no-repeat bgi-size-contain bgi-attachment-fixed"
        style={{
          backgroundImage: `url(/metronic/assets/media/svg/illustrations/${backgroundImageName}.png)`,
          minHeight: '100vh',
        }}
      >
        <div
          className={`d-flex flex-center flex-column flex-column-fluid ${isSelectEditionPage ? "pb-0" : "pb-lg-20"}`}
        >
          <Link to="/" className={"d-flex justify-content-center mt-10 mb-12"}>
            <img
              src={useTenantLogo ? tenantLogoUrl : defaultLogo}
              alt="logo"
              className="h-40px"
              onError={() => setUseTenantLogo(false)}
            />
          </Link>

          {isSelectEditionPage ? (
            <div className="container-xxl mx-auto">
              <div className="d-flex justify-content-start mb-5">
                {showTenantChange && (
                  <span className="text-muted">
                    <span>{L("CurrentTenant")}:</span>{" "}
                    {displayedTenancyName ? (
                      <span title={tenant?.name || ""}>
                        <strong>{displayedTenancyName}</strong>
                      </span>
                    ) : (
                      <span>{L("NotSelected")}</span>
                    )}{" "}
                    (
                    <a
                      href="#"
                      onClick={(e) => {
                        e.preventDefault();
                        setTenantModalOpen(true);
                      }}
                    >
                      {L("Change")}
                    </a>
                    )
                  </span>
                )}
              </div>
              <Outlet />
            </div>
          ) : (
            <div className="w-lg-500px bg-body rounded shadow-sm p-10 p-lg-15 mx-auto">
              <div className="d-flex justify-content-start mb-5">
                {showTenantChange && (
                  <span className="text-muted">
                    <span>{L("CurrentTenant")}:</span>{" "}
                    {displayedTenancyName ? (
                      <span title={tenant?.name || ""}>
                        <strong>{displayedTenancyName}</strong>
                      </span>
                    ) : (
                      <span>{L("NotSelected")}</span>
                    )}{" "}
                    (
                    <a
                      href="#"
                      onClick={(e) => {
                        e.preventDefault();
                        setTenantModalOpen(true);
                      }}
                    >
                      {L("Change")}
                    </a>
                    )
                  </span>
                )}
              </div>
              <Outlet />
            </div>
          )}
        </div>

        <div
          className={`d-flex flex-center flex-column-auto p-10 ${isSelectEditionPage ? "pt-0 mt-5" : ""}`}
        >
          <div className="d-flex align-items-center fw-bold fs-6">
            <LanguageSwitch />
          </div>
        </div>
      </div>
      <TenantChangeModal
        isOpen={tenantModalOpen}
        onClose={() => setTenantModalOpen(false)}
      />
    </div>
  );
};

export default AccountIndex;
