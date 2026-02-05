# Angular Application Structure

## Overview

This document outlines the recommended Angular application structure for the Surpath migration, including module organization, component hierarchy, service architecture, and development patterns that align with ASP.NET Zero Angular edition best practices.

## Project Structure

### Recommended Folder Structure

```
src/
├── app/
│   ├── admin/                          # Admin module (users, roles, tenants)
│   │   ├── users/
│   │   ├── roles/
│   │   ├── tenants/
│   │   └── admin-routing.module.ts
│   ├── main/                           # Main application features
│   │   ├── cohorts/                    # Cohort management
│   │   │   ├── cohorts.component.ts
│   │   │   ├── cohorts.component.html
│   │   │   ├── create-edit-cohort-modal.component.ts
│   │   │   └── cohorts-routing.module.ts
│   │   ├── records/                    # Medical records
│   │   │   ├── records.component.ts
│   │   │   ├── record-detail.component.ts
│   │   │   ├── file-upload.component.ts
│   │   │   └── records-routing.module.ts
│   │   ├── compliance/                 # Compliance tracking
│   │   ├── departments/                # Department management
│   │   ├── dashboard/                  # Main dashboard
│   │   └── main-routing.module.ts
│   ├── shared/                         # Shared components and services
│   │   ├── components/                 # Reusable UI components
│   │   │   ├── data-table/
│   │   │   ├── modal-header/
│   │   │   ├── file-upload/
│   │   │   └── confirmation-dialog/
│   │   ├── services/                   # Business services
│   │   │   ├── cohorts.service.ts
│   │   │   ├── records.service.ts
│   │   │   └── file-upload.service.ts
│   │   ├── pipes/                      # Custom pipes
│   │   ├── directives/                 # Custom directives
│   │   └── utils/                      # Utility functions
│   ├── layout/                         # Application layout
│   │   ├── header/
│   │   ├── sidebar/
│   │   ├── footer/
│   │   └── layout.component.ts
│   ├── auth/                           # Authentication
│   │   ├── login/
│   │   ├── register/
│   │   └── auth-routing.module.ts
│   ├── app-routing.module.ts
│   ├── app.component.ts
│   └── app.module.ts
├── assets/                             # Static assets
├── environments/                       # Environment configurations
└── styles/                            # Global styles
```

## Module Architecture

### 1. Core Module Structure

#### App Module (Root)
```typescript
// app.module.ts
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';

import { SharedModule } from '@shared/shared.module';
import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { RootModule } from './root.module';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    SharedModule.forRoot(),
    ServiceProxyModule,
    RootModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
```

#### Shared Module
```typescript
// shared/shared.module.ts
import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

// PrimeNG Modules
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { DialogModule } from 'primeng/dialog';
import { ProgressBarModule } from 'primeng/progressbar';
import { FileUploadModule } from 'primeng/fileupload';
import { ToastModule } from 'primeng/toast';

// ABP Modules
import { AbpModule } from 'abp-ng2-module';
import { ModalModule } from 'ngx-bootstrap/modal';

// Custom Components
import { DataTableComponent } from './components/data-table/data-table.component';
import { ModalHeaderComponent } from './components/modal-header/modal-header.component';
import { FileUploadComponent } from './components/file-upload/file-upload.component';
import { ConfirmationDialogComponent } from './components/confirmation-dialog/confirmation-dialog.component';

// Services
import { CohortsService } from './services/cohorts.service';
import { RecordsService } from './services/records.service';
import { FileUploadService } from './services/file-upload.service';

// Pipes
import { LocalizePipe } from './pipes/localize.pipe';
import { DateFormatPipe } from './pipes/date-format.pipe';

const PRIMENG_MODULES = [
  TableModule,
  ButtonModule,
  InputTextModule,
  DropdownModule,
  DialogModule,
  ProgressBarModule,
  FileUploadModule,
  ToastModule
];

const SHARED_COMPONENTS = [
  DataTableComponent,
  ModalHeaderComponent,
  FileUploadComponent,
  ConfirmationDialogComponent
];

const SHARED_PIPES = [
  LocalizePipe,
  DateFormatPipe
];

@NgModule({
  declarations: [
    ...SHARED_COMPONENTS,
    ...SHARED_PIPES
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    AbpModule,
    ModalModule.forRoot(),
    ...PRIMENG_MODULES
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    AbpModule,
    ModalModule,
    ...PRIMENG_MODULES,
    ...SHARED_COMPONENTS,
    ...SHARED_PIPES
  ]
})
export class SharedModule {
  static forRoot(): ModuleWithProviders<SharedModule> {
    return {
      ngModule: SharedModule,
      providers: [
        CohortsService,
        RecordsService,
        FileUploadService
      ]
    };
  }
}
```

### 2. Feature Module Example (Cohorts)

#### Cohorts Module
```typescript
// main/cohorts/cohorts.module.ts
import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';

import { CohortsRoutingModule } from './cohorts-routing.module';
import { CohortsComponent } from './cohorts.component';
import { CreateEditCohortModalComponent } from './create-edit-cohort-modal.component';
import { CohortDetailComponent } from './cohort-detail.component';
import { CohortComplianceComponent } from './cohort-compliance.component';

@NgModule({
  declarations: [
    CohortsComponent,
    CreateEditCohortModalComponent,
    CohortDetailComponent,
    CohortComplianceComponent
  ],
  imports: [
    SharedModule,
    CohortsRoutingModule
  ]
})
export class CohortsModule { }
```

#### Cohorts Routing
```typescript
// main/cohorts/cohorts-routing.module.ts
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';

import { CohortsComponent } from './cohorts.component';
import { CohortDetailComponent } from './cohort-detail.component';

const routes: Routes = [
  {
    path: '',
    component: CohortsComponent,
    canActivate: [AppRouteGuard],
    data: { permission: 'Pages.Cohorts' }
  },
  {
    path: 'detail/:id',
    component: CohortDetailComponent,
    canActivate: [AppRouteGuard],
    data: { permission: 'Pages.Cohorts' }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CohortsRoutingModule { }
```

## Component Architecture

### 1. Base Component Class

```typescript
// shared/components/app-component-base.ts
import { Injector, OnDestroy } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { LocalizationService } from 'abp-ng2-module';
import { PermissionCheckerService } from 'abp-ng2-module';
import { FeatureCheckerService } from 'abp-ng2-module';
import { NotifyService } from 'abp-ng2-module';
import { SettingService } from 'abp-ng2-module';
import { MessageService } from 'primeng/api';
import { ConfirmationService } from 'primeng/api';
import { Subject } from 'rxjs';

export abstract class AppComponentBase implements OnDestroy {
  localizationSourceName = AppConsts.localization.defaultLocalizationSourceName;
  
  localization: LocalizationService;
  permission: PermissionCheckerService;
  feature: FeatureCheckerService;
  notify: NotifyService;
  setting: SettingService;
  message: MessageService;
  confirm: ConfirmationService;

  protected destroy$ = new Subject<void>();

  constructor(injector: Injector) {
    this.localization = injector.get(LocalizationService);
    this.permission = injector.get(PermissionCheckerService);
    this.feature = injector.get(FeatureCheckerService);
    this.notify = injector.get(NotifyService);
    this.setting = injector.get(SettingService);
    this.message = injector.get(MessageService);
    this.confirm = injector.get(ConfirmationService);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  l(key: string, ...args: any[]): string {
    return this.localization.localize(key, this.localizationSourceName, ...args);
  }

  isGranted(permissionName: string): boolean {
    return this.permission.isGranted(permissionName);
  }

  isGrantedAny(...permissions: string[]): boolean {
    if (!permissions) {
      return false;
    }
    for (const permission of permissions) {
      if (this.isGranted(permission)) {
        return true;
      }
    }
    return false;
  }

  showSuccess(message?: string): void {
    this.message.add({
      severity: 'success',
      summary: this.l('Success'),
      detail: message || this.l('SavedSuccessfully')
    });
  }

  showError(message?: string): void {
    this.message.add({
      severity: 'error',
      summary: this.l('Error'),
      detail: message || this.l('AnErrorOccurred')
    });
  }

  confirmDelete(callback: () => void, message?: string): void {
    this.confirm.confirm({
      message: message || this.l('AreYouSureToDeleteThisItem'),
      header: this.l('Confirmation'),
      icon: 'pi pi-exclamation-triangle',
      accept: callback
    });
  }
}
```

### 2. List Component Example (Cohorts)

```typescript
// main/cohorts/cohorts.component.ts
import { Component, Injector, ViewChild, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LazyLoadEvent } from 'primeng/api';
import { Table } from 'primeng/table';
import { finalize, takeUntil } from 'rxjs/operators';

import { AppComponentBase } from '@shared/components/app-component-base';
import { CohortsService } from '@shared/services/cohorts.service';
import { CreateEditCohortModalComponent } from './create-edit-cohort-modal.component';
import {
  GetCohortForViewDto,
  GetAllCohortsInput,
  CohortDto
} from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-cohorts',
  templateUrl: './cohorts.component.html',
  styleUrls: ['./cohorts.component.less']
})
export class CohortsComponent extends AppComponentBase implements OnInit {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('createOrEditModal', { static: true }) createOrEditModal: CreateEditCohortModalComponent;

  cohorts: GetCohortForViewDto[] = [];
  totalCount = 0;
  loading = false;
  
  // Filters
  filters: GetAllCohortsInput = new GetAllCohortsInput();
  
  // Pagination
  pageSize = 10;
  pageNumber = 1;

  constructor(
    injector: Injector,
    private _cohortsService: CohortsService,
    private _router: Router
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.getCohorts();
  }

  getCohorts(event?: LazyLoadEvent): void {
    if (event) {
      this.pageNumber = Math.floor(event.first / event.rows) + 1;
      this.pageSize = event.rows;
      
      // Handle sorting
      if (event.sortField) {
        this.filters.sorting = `${event.sortField} ${event.sortOrder === 1 ? 'ASC' : 'DESC'}`;
      }
    }

    this.filters.maxResultCount = this.pageSize;
    this.filters.skipCount = (this.pageNumber - 1) * this.pageSize;

    this.loading = true;
    this._cohortsService.getAll(this.filters)
      .pipe(
        finalize(() => this.loading = false),
        takeUntil(this.destroy$)
      )
      .subscribe(result => {
        this.cohorts = result.items;
        this.totalCount = result.totalCount;
      });
  }

  createCohort(): void {
    this.createOrEditModal.show();
  }

  editCohort(cohort: CohortDto): void {
    this.createOrEditModal.show(cohort.id);
  }

  viewCohort(cohort: CohortDto): void {
    this._router.navigate(['/app/main/cohorts/detail', cohort.id]);
  }

  deleteCohort(cohort: CohortDto): void {
    this.confirmDelete(() => {
      this._cohortsService.delete(cohort.id)
        .pipe(takeUntil(this.destroy$))
        .subscribe(() => {
          this.showSuccess(this.l('SuccessfullyDeleted'));
          this.getCohorts();
        });
    });
  }

  exportToExcel(): void {
    this._cohortsService.getCohortsToExcel(this.filters)
      .pipe(takeUntil(this.destroy$))
      .subscribe(result => {
        this._cohortsService.downloadFile(result);
      });
  }

  onModalSaved(): void {
    this.getCohorts();
  }

  clearFilters(): void {
    this.filters = new GetAllCohortsInput();
    this.getCohorts();
  }
}
```

### 3. Modal Component Example

```typescript
// main/cohorts/create-edit-cohort-modal.component.ts
import { Component, Injector, ViewChild, Output, EventEmitter } from '@angular/core';
import { NgForm } from '@angular/forms';
import { finalize, takeUntil } from 'rxjs/operators';

import { AppComponentBase } from '@shared/components/app-component-base';
import { CohortsService } from '@shared/services/cohorts.service';
import {
  CreateOrEditCohortDto,
  GetCohortForEditOutput,
  CohortTenantDepartmentLookupTableDto
} from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'create-edit-cohort-modal',
  templateUrl: './create-edit-cohort-modal.component.html'
})
export class CreateEditCohortModalComponent extends AppComponentBase {
  @ViewChild('createOrEditForm', { static: true }) createOrEditForm: NgForm;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

  active = false;
  saving = false;
  
  cohort: CreateOrEditCohortDto = new CreateOrEditCohortDto();
  tenantDepartmentName = '';
  
  allTenantDepartments: CohortTenantDepartmentLookupTableDto[] = [];

  constructor(
    injector: Injector,
    private _cohortsService: CohortsService
  ) {
    super(injector);
  }

  show(cohortId?: string): void {
    this.active = true;
    this.cohort = new CreateOrEditCohortDto();
    this.cohort.defaultCohort = false;

    this._cohortsService.getAllTenantDepartmentForTableDropdown()
      .pipe(takeUntil(this.destroy$))
      .subscribe(result => {
        this.allTenantDepartments = result;
      });

    if (!cohortId) {
      this.tenantDepartmentName = '';
    } else {
      this._cohortsService.getCohortForEdit(cohortId)
        .pipe(takeUntil(this.destroy$))
        .subscribe((result: GetCohortForEditOutput) => {
          this.cohort = result.cohort;
          this.tenantDepartmentName = result.tenantDepartmentName;
        });
    }
  }

  save(): void {
    this.saving = true;
    
    this._cohortsService.createOrEdit(this.cohort)
      .pipe(
        finalize(() => this.saving = false),
        takeUntil(this.destroy$)
      )
      .subscribe(() => {
        this.showSuccess();
        this.close();
        this.modalSave.emit(null);
      });
  }

  setTenantDepartmentIdNull(): void {
    this.cohort.tenantDepartmentId = null;
    this.tenantDepartmentName = '';
  }

  getNewTenantDepartmentId(): string {
    return this.cohort.tenantDepartmentId;
  }

  setNewTenantDepartmentId(id: string): void {
    this.cohort.tenantDepartmentId = id;
  }

  close(): void {
    this.active = false;
    this.createOrEditForm.resetForm();
  }
}
```

## Service Architecture

### 1. Base Service Class

```typescript
// shared/services/service-base.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AppConsts } from '@shared/AppConsts';

@Injectable()
export abstract class ServiceBase {
  protected baseUrl = AppConsts.remoteServiceBaseUrl;

  constructor(protected http: HttpClient) {}

  protected buildParams(params: any): HttpParams {
    let httpParams = new HttpParams();
    
    Object.keys(params).forEach(key => {
      const value = params[key];
      if (value !== null && value !== undefined && value !== '') {
        if (Array.isArray(value)) {
          value.forEach(item => {
            httpParams = httpParams.append(key, item.toString());
          });
        } else {
          httpParams = httpParams.set(key, value.toString());
        }
      }
    });
    
    return httpParams;
  }

  protected downloadFile(data: any, filename?: string): void {
    const blob = new Blob([data], { type: 'application/octet-stream' });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = filename || 'download';
    link.click();
    window.URL.revokeObjectURL(url);
  }
}
```

### 2. Feature Service Example

```typescript
// shared/services/cohorts.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ServiceBase } from './service-base';
import {
  PagedResultDto,
  GetCohortForViewDto,
  GetAllCohortsInput,
  CreateOrEditCohortDto,
  GetCohortForEditOutput,
  CohortTenantDepartmentLookupTableDto,
  GetAllCohortsForExcelInput
} from '@shared/service-proxies/service-proxies';

@Injectable()
export class CohortsService extends ServiceBase {
  private apiUrl = `${this.baseUrl}/api/services/app/Cohorts`;

  constructor(http: HttpClient) {
    super(http);
  }

  getAll(input: GetAllCohortsInput): Observable<PagedResultDto<GetCohortForViewDto>> {
    const params = this.buildParams(input);
    return this.http.get<PagedResultDto<GetCohortForViewDto>>(this.apiUrl, { params });
  }

  getCohortForView(id: string): Observable<GetCohortForViewDto> {
    return this.http.get<GetCohortForViewDto>(`${this.apiUrl}/${id}`);
  }

  getCohortForEdit(id: string): Observable<GetCohortForEditOutput> {
    return this.http.get<GetCohortForEditOutput>(`${this.apiUrl}/GetCohortForEdit`, {
      params: { id }
    });
  }

  createOrEdit(input: CreateOrEditCohortDto): Observable<void> {
    return this.http.post<void>(this.apiUrl, input);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getAllTenantDepartmentForTableDropdown(): Observable<CohortTenantDepartmentLookupTableDto[]> {
    return this.http.get<CohortTenantDepartmentLookupTableDto[]>(
      `${this.apiUrl}/GetAllTenantDepartmentForTableDropdown`
    );
  }

  getCohortsToExcel(input: GetAllCohortsForExcelInput): Observable<Blob> {
    const params = this.buildParams(input);
    return this.http.get(`${this.apiUrl}/GetCohortsToExcel`, {
      params,
      responseType: 'blob'
    });
  }

  downloadFile(blob: Blob, filename: string = 'cohorts.xlsx'): void {
    super.downloadFile(blob, filename);
  }
}
```

## Shared Components

### 1. Reusable Data Table Component

```typescript
// shared/components/data-table/data-table.component.ts
import { Component, Input, Output, EventEmitter, TemplateRef } from '@angular/core';
import { LazyLoadEvent } from 'primeng/api';

export interface DataTableColumn {
  field: string;
  header: string;
  sortable?: boolean;
  filterable?: boolean;
  width?: string;
  type?: 'text' | 'date' | 'number' | 'boolean' | 'actions';
}

@Component({
  selector: 'app-data-table',
  templateUrl: './data-table.component.html',
  styleUrls: ['./data-table.component.less']
})
export class DataTableComponent {
  @Input() data: any[] = [];
  @Input() columns: DataTableColumn[] = [];
  @Input() totalRecords = 0;
  @Input() loading = false;
  @Input() paginator = true;
  @Input() rows = 10;
  @Input() rowsPerPageOptions = [10, 25, 50, 100];
  @Input() globalFilterFields: string[] = [];
  @Input() actionsTemplate: TemplateRef<any>;
  @Input() customTemplate: TemplateRef<any>;

  @Output() onLazyLoad = new EventEmitter<LazyLoadEvent>();
  @Output() onRowSelect = new EventEmitter<any>();
  @Output() onRowUnselect = new EventEmitter<any>();

  selectedRows: any[] = [];

  loadData(event: LazyLoadEvent): void {
    this.onLazyLoad.emit(event);
  }

  onRowSelectEvent(event: any): void {
    this.onRowSelect.emit(event);
  }

  onRowUnselectEvent(event: any): void {
    this.onRowUnselect.emit(event);
  }

  getColumnValue(rowData: any, column: DataTableColumn): any {
    return this.getNestedProperty(rowData, column.field);
  }

  private getNestedProperty(obj: any, path: string): any {
    return path.split('.').reduce((o, p) => o && o[p], obj);
  }
}
```

### 2. File Upload Component

```typescript
// shared/components/file-upload/file-upload.component.ts
import { Component, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { FileUpload } from 'primeng/fileupload';
import { MessageService } from 'primeng/api';

import { FileUploadService } from '@shared/services/file-upload.service';

export interface UploadedFile {
  id: string;
  fileName: string;
  fileSize: number;
  contentType: string;
}

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.less']
})
export class FileUploadComponent {
  @ViewChild('fileUpload') fileUpload: FileUpload;
  
  @Input() accept = '.pdf,.doc,.docx,.jpg,.jpeg,.png';
  @Input() maxFileSize = 10485760; // 10MB
  @Input() multiple = false;
  @Input() auto = false;
  @Input() showUploadButton = true;
  @Input() showCancelButton = true;
  @Input() chooseLabel = 'Choose Files';
  @Input() uploadLabel = 'Upload';
  @Input() cancelLabel = 'Cancel';

  @Output() onUpload = new EventEmitter<UploadedFile[]>();
  @Output() onError = new EventEmitter<any>();
  @Output() onProgress = new EventEmitter<number>();

  uploadedFiles: UploadedFile[] = [];
  uploading = false;
  uploadProgress = 0;

  constructor(
    private _fileUploadService: FileUploadService,
    private _messageService: MessageService
  ) {}

  onSelect(event: any): void {
    if (this.auto) {
      this.upload();
    }
  }

  upload(): void {
    if (!this.fileUpload.files || this.fileUpload.files.length === 0) {
      return;
    }

    this.uploading = true;
    this.uploadProgress = 0;

    const files = Array.from(this.fileUpload.files) as File[];
    
    this._fileUploadService.uploadFiles(files)
      .subscribe({
        next: (result) => {
          if (result.type === 'progress') {
            this.uploadProgress = result.progress;
            this.onProgress.emit(this.uploadProgress);
          } else if (result.type === 'response') {
            this.uploadedFiles = result.files;
            this.onUpload.emit(this.uploadedFiles);
            this._messageService.add({
              severity: 'success',
              summary: 'Success',
              detail: 'Files uploaded successfully'
            });
            this.clear();
          }
        },
        error: (error) => {
          this.onError.emit(error);
          this._messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'Upload failed'
          });
        },
        complete: () => {
          this.uploading = false;
          this.uploadProgress = 0;
        }
      });
  }

  clear(): void {
    this.fileUpload.clear();
    this.uploadProgress = 0;
  }

  removeFile(file: UploadedFile): void {
    this._fileUploadService.deleteFile(file.id)
      .subscribe(() => {
        this.uploadedFiles = this.uploadedFiles.filter(f => f.id !== file.id);
        this._messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: 'File removed successfully'
        });
      });
  }
}
```

## Routing and Navigation

### 1. Main Routing Configuration

```typescript
// app-routing.module.ts
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';

const routes: Routes = [
  {
    path: '',
    redirectTo: '/app/main/dashboard',
    pathMatch: 'full'
  },
  {
    path: 'account',
    loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule)
  },
  {
    path: 'app',
    canActivate: [AppRouteGuard],
    canActivateChild: [AppRouteGuard],
    children: [
      {
        path: 'main',
        loadChildren: () => import('./main/main.module').then(m => m.MainModule)
      },
      {
        path: 'admin',
        loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule),
        data: { preload: true }
      }
    ]
  },
  {
    path: '**',
    redirectTo: '/app/main/dashboard'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {
    enableTracing: false,
    preloadingStrategy: PreloadAllModules
  })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
```

### 2. Feature Module Routing

```typescript
// main/main-routing.module.ts
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },
  {
    path: 'dashboard',
    loadChildren: () => import('./dashboard/dashboard.module').then(m => m.DashboardModule)
  },
  {
    path: 'cohorts',
    loadChildren: () => import('./cohorts/cohorts.module').then(m => m.CohortsModule)
  },
  {
    path: 'records',
    loadChildren: () => import('./records/records.module').then(m => m.RecordsModule)
  },
  {
    path: 'compliance',
    loadChildren: () => import('./compliance/compliance.module').then(m => m.ComplianceModule)
  },
  {
    path: 'departments',
    loadChildren: () => import('./departments/departments.module').then(m => m.DepartmentsModule)
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MainRoutingModule { }
```

## State Management

### 1. Simple Component State (Recommended for most cases)

```typescript
// Example: Simple state management in component
export class CohortsComponent {
  // Local component state
  cohorts: CohortDto[] = [];
  loading = false;
  filters = new GetAllCohortsInput();
  selectedCohort: CohortDto | null = null;
  
  // State management methods
  private updateState(updates: Partial<CohortsComponent>): void {
    Object.assign(this, updates);
  }
  
  private resetFilters(): void {
    this.filters = new GetAllCohortsInput();
  }
}
```

### 2. NgRx Store (For complex state scenarios)

```typescript
// state/cohorts/cohorts.state.ts
export interface CohortsState {
  cohorts: CohortDto[];
  loading: boolean;
  error: string | null;
  selectedCohort: CohortDto | null;
  filters: GetAllCohortsInput;
  totalCount: number;
}

export const initialState: CohortsState = {
  cohorts: [],
  loading: false,
  error: null,
  selectedCohort: null,
  filters: new GetAllCohortsInput(),
  totalCount: 0
};

// state/cohorts/cohorts.actions.ts
export const loadCohorts = createAction(
  '[Cohorts] Load Cohorts',
  props<{ filters: GetAllCohortsInput }>()
);

export const loadCohortsSuccess = createAction(
  '[Cohorts] Load Cohorts Success',
  props<{ cohorts: CohortDto[]; totalCount: number }>()
);

export const loadCohortsFailure = createAction(
  '[Cohorts] Load Cohorts Failure',
  props<{ error: string }>()
);

// state/cohorts/cohorts.reducer.ts
const cohortsReducer = createReducer(
  initialState,
  on(loadCohorts, state => ({ ...state, loading: true, error: null })),
  on(loadCohortsSuccess, (state, { cohorts, totalCount }) => ({
    ...state,
    loading: false,
    cohorts,
    totalCount
  })),
  on(loadCohortsFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  }))
);
```

## Development Guidelines

### 1. Naming Conventions

- **Components**: PascalCase with descriptive names (e.g., `CreateEditCohortModalComponent`)
- **Services**: PascalCase ending with "Service" (e.g., `CohortsService`)
- **Files**: kebab-case (e.g., `create-edit-cohort-modal.component.ts`)
- **Variables**: camelCase (e.g., `selectedCohort`)
- **Constants**: UPPER_SNAKE_CASE (e.g., `MAX_FILE_SIZE`)

### 2. Code Organization

- **One component per file**
- **Group related files in feature folders**
- **Use barrel exports** (index.ts files) for clean imports
- **Separate concerns** (component, template, styles, tests)

### 3. Performance Best Practices

```typescript
// Use OnPush change detection for better performance
@Component({
  selector: 'app-cohorts',
  templateUrl: './cohorts.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CohortsComponent {
  // Use trackBy functions for *ngFor
  trackByCohortId(index: number, cohort: CohortDto): string {
    return cohort.id;
  }
}
```

### 4. Error Handling

```typescript
// Centralized error handling in services
@Injectable()
export class CohortsService extends ServiceBase {
  getAll(input: GetAllCohortsInput): Observable<PagedResultDto<GetCohortForViewDto>> {
    return this.http.get<PagedResultDto<GetCohortForViewDto>>(this.apiUrl, { params })
      .pipe(
        catchError(this.handleError),
        retry(1)
      );
  }
  
  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An unknown error occurred';
    
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = error.error.message;
    } else {
      // Server-side error
      errorMessage = error.error?.message || `Error Code: ${error.status}`;
    }
    
    return throwError(() => new Error(errorMessage));
  }
}
```

## Testing Strategy

### 1. Unit Testing Components

```typescript
// cohorts.component.spec.ts
describe('CohortsComponent', () => {
  let component: CohortsComponent;
  let fixture: ComponentFixture<CohortsComponent>;
  let cohortsService: jasmine.SpyObj<CohortsService>;

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('CohortsService', ['getAll', 'delete']);

    await TestBed.configureTestingModule({
      declarations: [CohortsComponent],
      providers: [
        { provide: CohortsService, useValue: spy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(CohortsComponent);
    component = fixture.componentInstance;
    cohortsService = TestBed.inject(CohortsService) as jasmine.SpyObj<CohortsService>;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load cohorts on init', () => {
    const mockCohorts = [{ id: '1', name: 'Test Cohort' }];
    cohortsService.getAll.and.returnValue(of({ items: mockCohorts, totalCount: 1 }));

    component.ngOnInit();

    expect(cohortsService.getAll).toHaveBeenCalled();
    expect(component.cohorts).toEqual(mockCohorts);
  });
});
```

### 2. Service Testing

```typescript
// cohorts.service.spec.ts
describe('CohortsService', () => {
  let service: CohortsService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [CohortsService]
    });
    service = TestBed.inject(CohortsService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should retrieve cohorts', () => {
    const mockResponse = { items: [], totalCount: 0 };

    service.getAll(new GetAllCohortsInput()).subscribe(response => {
      expect(response).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(`${service.apiUrl}`);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });
});
```

## Build and Deployment

### 1. Environment Configuration

```typescript
// environments/environment.ts
export const environment = {
  production: false,
  appConfig: 'appconfig.json',
  remoteServiceBaseUrl: 'http://localhost:21021',
  allowedHosts: ['localhost:4200']
};

// environments/environment.prod.ts
export const environment = {
  production: true,
  appConfig: 'appconfig.production.json',
  remoteServiceBaseUrl: 'https://api.surpath.com',
  allowedHosts: ['surpath.com', 'www.surpath.com']
};
```

### 2. Build Scripts

```json
// package.json scripts
{
  "scripts": {
    "ng": "ng",
    "start": "ng serve --host 0.0.0.0 --port 4200",
    "build": "ng build",
    "build:prod": "ng build --configuration production",
    "test": "ng test",
    "lint": "ng lint",
    "e2e": "ng e2e",
    "analyze": "ng build --stats-json && npx webpack-bundle-analyzer dist/stats.json"
  }
}
```

## Next Steps

1. **Set up the Angular project** using the structure outlined above
2. **Create the shared module** with common components and services
3. **Implement the base component class** for consistent functionality
4. **Build feature modules** starting with the simplest ones
5. **Set up routing and navigation** with proper guards
6. **Implement authentication** and authorization patterns
7. **Create reusable components** for data tables, modals, and file uploads

---

**Next**: [Component Migration Mapping](./04-component-migration-mapping.md) - Detailed mapping of MVC views to Angular components.
