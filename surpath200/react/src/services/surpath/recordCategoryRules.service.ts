/**
 * RecordCategoryRules Service Proxy
 *
 * Manual implementation following RecordRequirements pattern
 * Based on IRecordCategoryRulesAppService from Surpath112
 */

import type { EntityDto, PagedResultDto, PagedAndSortedResultRequestDto } from '@/shared/service-proxies/service-proxies';
import http from '@/shared/utils/http';

// ============================================================================
// DTOs
// ============================================================================

export interface RecordCategoryRuleDto {
  id: string;
  name: string;
  description?: string;
  notify: boolean;
  expireInDays: number;
  warnDaysBeforeFirst: number;
  expires: boolean;
  required: boolean;
  isSurpathOnly: boolean;
  warnDaysBeforeSecond: number;
  warnDaysBeforeFinal: number;
  metaData?: string;
  firstWarnStatusId?: string;
  firstWarnStatusName?: string;
  secondWarnStatusId?: string;
  secondWarnStatusName?: string;
  finalWarnStatusId?: string;
  finalWarnStatusName?: string;
  expiredStatusId?: string;
  expiredStatusName?: string;
}

export interface GetRecordCategoryRuleForViewDto {
  recordCategoryRule: RecordCategoryRuleDto;
}

export interface CreateOrEditRecordCategoryRuleDto {
  id?: string;
  name: string;
  description?: string;
  notify: boolean;
  expireInDays: number;
  warnDaysBeforeFirst: number;
  expires: boolean;
  required: boolean;
  isSurpathOnly: boolean;
  warnDaysBeforeSecond: number;
  warnDaysBeforeFinal: number;
  metaData?: string;
  firstWarnStatusId?: string;
  secondWarnStatusId?: string;
  finalWarnStatusId?: string;
  expiredStatusId?: string;
}

export interface GetRecordCategoryRuleForEditOutput {
  recordCategoryRule: CreateOrEditRecordCategoryRuleDto;
  firstWarnStatusName?: string;
  secondWarnStatusName?: string;
  finalWarnStatusName?: string;
  expiredStatusName?: string;
}

export interface GetAllRecordCategoryRulesInput extends PagedAndSortedResultRequestDto {
  filter?: string;
  nameFilter?: string;
  descriptionFilter?: string;
  notifyFilter?: number;
  minExpireInDaysFilter?: number;
  maxExpireInDaysFilter?: number;
  minWarnDaysBeforeFirstFilter?: number;
  maxWarnDaysBeforeFirstFilter?: number;
  expiresFilter?: number;
  requiredFilter?: number;
  isSurpathOnlyFilter?: number;
  minWarnDaysBeforeSecondFilter?: number;
  maxWarnDaysBeforeSecondFilter?: number;
  minWarnDaysBeforeFinalFilter?: number;
  maxWarnDaysBeforeFinalFilter?: number;
  metaDataFilter?: string;
}

export interface GetAllRecordCategoryRulesForExcelInput {
  filter?: string;
  nameFilter?: string;
  descriptionFilter?: string;
  notifyFilter?: number;
  minExpireInDaysFilter?: number;
  maxExpireInDaysFilter?: number;
  minWarnDaysBeforeFirstFilter?: number;
  maxWarnDaysBeforeFirstFilter?: number;
  expiresFilter?: number;
  requiredFilter?: number;
  isSurpathOnlyFilter?: number;
  minWarnDaysBeforeSecondFilter?: number;
  maxWarnDaysBeforeSecondFilter?: number;
  minWarnDaysBeforeFinalFilter?: number;
  maxWarnDaysBeforeFinalFilter?: number;
  metaDataFilter?: string;
}

export interface FileDto {
  fileName: string;
  fileType: string;
  fileToken: string;
}

// ============================================================================
// Service
// ============================================================================

class RecordCategoryRulesService {
  private readonly baseUrl = '/api/services/app/RecordCategoryRules';

  /**
   * Get all RecordCategoryRules with pagination and filters
   */
  async getAll(input: GetAllRecordCategoryRulesInput): Promise<PagedResultDto<GetRecordCategoryRuleForViewDto>> {
    const response = await http.get(`${this.baseUrl}/GetAll`, { params: input });
    return response.data.result;
  }

  /**
   * Get RecordCategoryRule for viewing (read-only)
   */
  async getRecordCategoryRuleForView(id: string): Promise<GetRecordCategoryRuleForViewDto> {
    const response = await http.get(`${this.baseUrl}/GetRecordCategoryRuleForView`, {
      params: { id }
    });
    return response.data.result;
  }

  /**
   * Get RecordCategoryRule for editing
   */
  async getRecordCategoryRuleForEdit(input: EntityDto<string>): Promise<GetRecordCategoryRuleForEditOutput> {
    const response = await http.get(`${this.baseUrl}/GetRecordCategoryRuleForEdit`, {
      params: input
    });
    return response.data.result;
  }

  /**
   * Create or update RecordCategoryRule
   */
  async createOrEdit(input: CreateOrEditRecordCategoryRuleDto): Promise<void> {
    await http.post(`${this.baseUrl}/CreateOrEdit`, input);
  }

  /**
   * Delete RecordCategoryRule
   */
  async delete(input: EntityDto<string>): Promise<void> {
    await http.delete(`${this.baseUrl}/Delete`, { params: input });
  }

  /**
   * Export RecordCategoryRules to Excel
   */
  async getRecordCategoryRulesToExcel(input: GetAllRecordCategoryRulesForExcelInput): Promise<FileDto> {
    const response = await http.get(`${this.baseUrl}/GetRecordCategoryRulesToExcel`, {
      params: input
    });
    return response.data.result;
  }
}

export const recordCategoryRulesService = new RecordCategoryRulesService();
