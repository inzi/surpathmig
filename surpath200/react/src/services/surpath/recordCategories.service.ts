/**
 * RecordCategories Service Proxy
 *
 * Manual implementation following RecordRequirements pattern
 * Based on IRecordCategoriesAppService from Surpath112
 */

import type { EntityDto, PagedResultDto, PagedAndSortedResultRequestDto } from '@/shared/service-proxies/service-proxies';
import http from '@/shared/utils/http';

// ============================================================================
// DTOs
// ============================================================================

export interface RecordCategoryRuleDto {
  id: string;
  name: string;
}

export interface RecordCategoryDto {
  id: string;
  name: string;
  instructions?: string;
  recordRequirementId?: string;
  recordCategoryRuleId?: string;
  isSurpathService: boolean;
  tenantId?: number;
  recordCategoryRule?: RecordCategoryRuleDto;
}

export interface GetRecordCategoryForViewDto {
  recordCategory: RecordCategoryDto;
  recordRequirementName?: string;
  recordCategoryRuleName?: string;
}

export interface CreateOrEditRecordCategoryDto {
  id?: string;
  name: string;
  instructions?: string;
  recordRequirementId?: string;
  recordCategoryRuleId?: string;
}

export interface GetRecordCategoryForEditOutput {
  recordCategory: CreateOrEditRecordCategoryDto;
  recordRequirementName?: string;
  recordCategoryRuleName?: string;
}

export interface GetAllRecordCategoriesInput extends PagedAndSortedResultRequestDto {
  filter?: string;
  nameFilter?: string;
  instructionsFilter?: string;
  recordRequirementNameFilter?: string;
  recordCategoryRuleNameFilter?: string;
}

export interface GetAllRecordCategoriesForExcelInput {
  filter?: string;
  nameFilter?: string;
  instructionsFilter?: string;
  recordRequirementNameFilter?: string;
  recordCategoryRuleNameFilter?: string;
}

export interface RecordCategoryRecordRequirementLookupTableDto {
  id: string;
  displayName: string;
}

export interface RecordCategoryRecordCategoryRuleLookupTableDto {
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

class RecordCategoriesService {
  private readonly baseUrl = '/api/services/app/RecordCategories';

  /**
   * Get all RecordCategories with pagination and filters
   */
  async getAll(input: GetAllRecordCategoriesInput): Promise<PagedResultDto<GetRecordCategoryForViewDto>> {
    const response = await http.get(`${this.baseUrl}/GetAll`, { params: input });
    return response.data.result;
  }

  /**
   * Get RecordCategory for viewing (read-only)
   */
  async getRecordCategoryForView(id: string): Promise<GetRecordCategoryForViewDto> {
    const response = await http.get(`${this.baseUrl}/GetRecordCategoryForView`, {
      params: { id }
    });
    return response.data.result;
  }

  /**
   * Get RecordCategory for editing
   */
  async getRecordCategoryForEdit(input: EntityDto<string>): Promise<GetRecordCategoryForEditOutput> {
    const response = await http.get(`${this.baseUrl}/GetRecordCategoryForEdit`, {
      params: input
    });
    return response.data.result;
  }

  /**
   * Create or update RecordCategory
   */
  async createOrEdit(input: CreateOrEditRecordCategoryDto): Promise<void> {
    await http.post(`${this.baseUrl}/CreateOrEdit`, input);
  }

  /**
   * Delete RecordCategory
   */
  async delete(input: EntityDto<string>): Promise<void> {
    await http.delete(`${this.baseUrl}/Delete`, { params: input });
  }

  /**
   * Export RecordCategories to Excel
   */
  async getRecordCategoriesToExcel(input: GetAllRecordCategoriesForExcelInput): Promise<FileDto> {
    const response = await http.get(`${this.baseUrl}/GetRecordCategoriesToExcel`, {
      params: input
    });
    return response.data.result;
  }

  /**
   * Get all RecordRequirements for lookup table dropdown
   */
  async getAllRecordRequirementForLookupTable(input: PagedAndSortedResultRequestDto & { filter?: string }): Promise<PagedResultDto<RecordCategoryRecordRequirementLookupTableDto>> {
    const response = await http.get(`${this.baseUrl}/GetAllRecordRequirementForLookupTable`, {
      params: input
    });
    return response.data.result;
  }

  /**
   * Get all RecordCategoryRules for dropdown
   */
  async getAllRecordCategoryRuleForTableDropdown(): Promise<RecordCategoryRecordCategoryRuleLookupTableDto[]> {
    const response = await http.get(`${this.baseUrl}/GetAllRecordCategoryRuleForTableDropdown`);
    return response.data.result;
  }

  /**
   * Get RecordCategoryDto by ID (custom endpoint)
   */
  async getRecordCategoryDto(catid: string): Promise<RecordCategoryDto> {
    const response = await http.get(`${this.baseUrl}/GetRecordCategoryDto`, {
      params: { catid }
    });
    return response.data.result;
  }
}

export const recordCategoriesService = new RecordCategoriesService();
