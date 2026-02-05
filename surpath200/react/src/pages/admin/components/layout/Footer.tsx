import { useEffect, useState } from "react";
import dayjs from "dayjs";

import { ILayout, useLayout } from "metronic/app/partials/layout/core";
import { useSession } from "@/hooks/useSession";

const Footer = () => {
  const { config } = useLayout();
  const [tenantEdition, setTenantEdition] = useState<string>("");
  const [apiVersion, setApiVersion] = useState<string>("");
  const [releaseDate, setReleaseDate] = useState<string>("");
  const [footerContainerClass, setFooterContainerClass] =
    useState<string>("container-xxl");
  const { tenant, application, theme } = useSession();

  useEffect(() => {
    const edition = tenant?.edition?.displayName;
    const version = application?.version;
    const releaseDate = application?.releaseDate;
    setTenantEdition(edition ? String(edition) : "");
    setApiVersion(version ? `v${version}` : "");
    setReleaseDate(releaseDate ? dayjs(releaseDate).format("YYYYMMDD") : "");
  }, [tenant, application]);

  useEffect(() => {
    const footerWidthType = theme?.baseSettings?.footer?.footerWidthType;

    if (footerWidthType === "fluid") {
      setFooterContainerClass("container-fluid");
    } else if (footerWidthType === "fixed") {
      setFooterContainerClass("container-xxl");
    } else {
      setFooterContainerClass("container-xxl");
    }
  }, [theme]);

  if (!config.app?.footer?.display) {
    return null;
  }

  return (
    <div className="footer py-4 d-flex flex-lg-column" id="kt_footer">
      <div
        className={`${footerContainerClass} d-flex flex-column flex-md-row align-items-center justify-content-between`}
      >
        <div className="text-gray-900 order-2 order-md-1">
          <span className="text-muted fw-bold me-1">
            inzibackend {tenantEdition && <span>{tenantEdition}</span>} |
            API: {apiVersion || "-"} | Client: {apiVersion}{" "}
            {releaseDate && `[${releaseDate}]`}
          </span>
        </div>
        <ul className="menu menu-gray-600 menu-hover-primary fw-bold order-1"></ul>
      </div>
    </div>
  );
};

export { Footer };
