export function getDashboardUrl(tenantId?: number | null): string {
  const effectiveTenantId =
    tenantId !== undefined ? tenantId : abp?.session?.tenantId;
  const isHost = !effectiveTenantId;
  return isHost ? "/app/host-dashboard" : "/app/tenant-dashboard";
}
