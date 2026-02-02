// Temporary service file for CodeTypes
// TODO: Replace with auto-generated proxies once .NET SDK 10.0 is available

import { AxiosInstance } from "axios";
import { PagedResultDto } from "@api/generated/service-proxies";

// DTOs based on backend implementation
export interface CodeTypeDto {
  name: string;
  id: number;
}

export interface CreateOrEditCodeTypeDto {
  name: string;
  id?: number;
}

export interface GetCodeTypeForViewDto {
  codeType: CodeTypeDto;
}

export interface GetCodeTypeForEditOutput {
  codeType: CreateOrEditCodeTypeDto;
}

export interface GetAllCodeTypesInput {
  filter?: string;
  nameFilter?: string;
  sorting?: string;
  skipCount?: number;
  maxResultCount?: number;
}

export interface GetAllCodeTypesForExcelInput {
  filter?: string;
  nameFilter?: string;
}

export interface FileDto {
  fileName: string;
  fileType?: string;
  fileToken: string;
}

// Temporary service proxy
export class CodeTypesServiceProxy {
  protected instance: AxiosInstance;
  protected baseUrl: string;

  constructor(baseUrl?: string, instance?: AxiosInstance) {
    this.instance = instance!;
    this.baseUrl = baseUrl ?? "";
  }

  async getAll(
    input: GetAllCodeTypesInput
  ): Promise<PagedResultDto<GetCodeTypeForViewDto>> {
    const url = this.baseUrl + "/api/services/app/CodeTypes/GetAll";
    const response = await this.instance.post(url, input);
    return response.data.result;
  }

  async getCodeTypeForView(id: number): Promise<GetCodeTypeForViewDto> {
    const url = this.baseUrl + "/api/services/app/CodeTypes/GetCodeTypeForView";
    const response = await this.instance.get(url, { params: { id } });
    return response.data.result;
  }

  async getCodeTypeForEdit(
    id: number | undefined
  ): Promise<GetCodeTypeForEditOutput> {
    const url =
      this.baseUrl + "/api/services/app/CodeTypes/GetCodeTypeForEdit";
    const response = await this.instance.get(url, {
      params: id ? { id } : {},
    });
    return response.data.result;
  }

  async createOrEdit(input: CreateOrEditCodeTypeDto): Promise<void> {
    const url = this.baseUrl + "/api/services/app/CodeTypes/CreateOrEdit";
    await this.instance.post(url, input);
  }

  async delete(id: number): Promise<void> {
    const url = this.baseUrl + "/api/services/app/CodeTypes/Delete";
    await this.instance.delete(url, { params: { id } });
  }

  async getCodeTypesToExcel(
    input: GetAllCodeTypesForExcelInput
  ): Promise<FileDto> {
    const url = this.baseUrl + "/api/services/app/CodeTypes/GetCodeTypesToExcel";
    const response = await this.instance.post(url, input);
    return response.data.result;
  }
}
