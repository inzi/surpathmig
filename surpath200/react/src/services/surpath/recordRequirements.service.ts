/**
 * RecordRequirements Service Proxy
 *
 * Temporary manual implementation until backend is running for nswag generation
 * Based on IRecordRequirementsAppService from Surpath112
 */

import type { EntityDto, PagedResultDto, PagedAndSortedResultRequestDto } from '@/shared/service-proxies/service-proxies';
import http from '@/shared/utils/http';

// ============================================================================
// DTOs
// ============================================================================

export interface RecordCategoryDto {
  id: string;
  name: string;
  recordCategoryRuleId?: string;
}

export interface RecordRequirementDto {
  id: string;
  name: string;
  description?: string;
  metadata?: string;
  isSurpathOnly: boolean;
  tenantDepartmentId?: string;
  cohortId?: string;
  surpathServiceId?: string;
  tenantSurpathServiceId?: string;
  categoryDTOs: RecordCategoryDto[];
}

export interface GetRecordRequirementForViewDto {
  recordRequirement: RecordRequirementDto;
  tenantDepartmentName?: string;
  cohortName?: string;
  surpathServiceName?: string;
  tenantSurpathServiceName?: string;
  tenantName?: string;
}

export interface CreateOrEditRecordRequirementDto {
  id?: string;
  name: string;
  description?: string;
  metadata?: string;
  isSurpathOnly: boolean;
  tenantDepartmentId?: string;
  cohortId?: string;
  surpathServiceId?: string;
  tenantSurpathServiceId?: string;
}

export interface GetRecordRequirementForEditOutput {
  recordRequirement: CreateOrEditRecordRequirementDto;
  tenantDepartmentName?: string;
  cohortName?: string;
  surpathServiceName?: string;
  tenantSurpathServiceName?: string;
}

export interface GetAllRecordRequirementsInput extends PagedAndSortedResultRequestDto {
  filter?: string;
  nameFilter?: string;
  descriptionFilter?: string;
  metadataFilter?: string;
  isSurpathOnlyFilter?: number;
  tenantDepartmentNameFilter?: string;
  cohortNameFilter?: string;
  surpathServiceNameFilter?: string;
  tenantSurpathServiceNameFilter?: string;
}

export interface GetAllRecordRequirementsForExcelInput {
  filter?: string;
  nameFilter?: string;
  descriptionFilter?: string;
  metadataFilter?: string;
  isSurpathOnlyFilter?: number;
  tenantDepartmentNameFilter?: string;
  cohortNameFilter?: string;
  surpathServiceNameFilter?: string;
  tenantSurpathServiceNameFilter?: string;
}

export interface RecordRequirementCohortLookupTableDto {
  id: string;
  displayName: string;
}

export interface RecordRequirementSurpathServiceLookupTableDto {
  id: string;
  displayName: string;
}

export interface RecordRequirementTenantSurpathServiceLookupTableDto {
  id: string;
  displayName: string;
}

export interface FileDto {
  fileName: string;
  fileType: string;
  fileToken: string;
}

// ============================================================================
// Service
// ============================================================================

class RecordRequirementsService {
  private readonly baseUrl = '/api/services/app/RecordRequirements';

  /**
   * Get all RecordRequirements with pagination and filters
   */
  async getAll(input: GetAllRecordRequirementsInput): Promise<PagedResultDto<GetRecordRequirementForViewDto>> {
    const response = await http.get(`${this.baseUrl}/GetAll`, { params: input });
    return response.data.result;
  }

  /**
   * Get RecordRequirement for viewing (read-only)
   */
  async getRecordRequirementForView(id: string): Promise<GetRecordRequirementForViewDto> {
    const response = await http.get(`${this.baseUrl}/GetRecordRequirementForView`, {
      params: { id }
    });
    return response.data.result;
  }

  /**
   * Get RecordRequirement for editing
   */
  async getRecordRequirementForEdit(input: EntityDto<string>): Promise<GetRecordRequirementForEditOutput> {
    const response = await http.get(`${this.baseUrl}/GetRecordRequirementForEdit`, {
      params: input
    });
    return response.data.result;
  }

  /**
   * Create or update RecordRequirement
   */
  async createOrEdit(input: CreateOrEditRecordRequirementDto): Promise<void> {
    await http.post(`${this.baseUrl}/CreateOrEdit`, input);
  }

  /**
   * Delete RecordRequirement
   */
  async delete(input: EntityDto<string>): Promise<void> {
    await http.delete(`${this.baseUrl}/Delete`, { params: input });
  }

  /**
   * Export RecordRequirements to Excel
   */
  async getRecordRequirementsToExcel(input: GetAllRecordRequirementsForExcelInput): Promise<FileDto> {
    const response = await http.get(`${this.baseUrl}/GetRecordRequirementsToExcel`, {
      params: input
    });
    return response.data.result;
  }

  /**
   * Get all Cohorts for lookup table dropdown
   */
  async getAllCohortForLookupTable(input: PagedAndSortedResultRequestDto & { filter?: string }): Promise<PagedResultDto<RecordRequirementCohortLookupTableDto>> {
    const response = await http.get(`${this.baseUrl}/GetAllCohortForLookupTable`, {
      params: input
    });
    return response.data.result;
  }

  /**
   * Get all SurpathServices for dropdown
   */
  async getAllSurpathServiceForTableDropdown(): Promise<RecordRequirementSurpathServiceLookupTableDto[]> {
    const response = await http.get(`${this.baseUrl}/GetAllSurpathServiceForTableDropdown`);
    return response.data.result;
  }

  /**
   * Get all TenantSurpathServices for dropdown
   */
  async getAllTenantSurpathServiceForTableDropdown(): Promise<RecordRequirementTenantSurpathServiceLookupTableDto[]> {
    const response = await http.get(`${this.baseUrl}/GetAllTenantSurpathServiceForTableDropdown`);
    return response.data.result;
  }
}

export const recordRequirementsService = new RecordRequirementsService();
