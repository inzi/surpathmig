/// <reference types="vite/client" />

import type { AbpUserConfigurationResult } from "./api/abp-user-configuration";
import "../node_modules/abp-web-resources/Abp/Framework/scripts/abp.d.ts";
import "../node_modules/abp-web-resources/Abp/Framework/scripts/libs/abp.jquery.d.ts";
import "../node_modules/abp-web-resources/Abp/Framework/scripts/libs/abp.signalr.d.ts";

declare global {
  interface Window {
    __applyAbpUserConfiguration?: (config: AbpUserConfigurationResult) => void;
    signalR?: typeof signalR;
  }
}

interface ImportMetaEnv {
  readonly VITE_API_URL?: string;
  readonly VITE_API_BASE_URL?: string;
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}
