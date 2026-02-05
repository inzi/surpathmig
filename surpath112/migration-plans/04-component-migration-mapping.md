# Component Migration Mapping: MVC Views to Angular Components

## Overview

This document provides a detailed mapping of existing MVC views to their corresponding Angular components, including conversion strategies, component hierarchies, and implementation patterns specific to the Surpath application.

## Migration Mapping Strategy

### 1. View Type Classification

#### A. List Views (Data Tables)
- **Current**: Razor views with DataTables.js
- **Target**: Angular components with PrimeNG p-table
- **Complexity**: Medium (3-5/10)

#### B. Modal Forms (Create/Edit)
- **Current**: Partial views with jQuery modal management
- **Target**: Angular modal components
- **Complexity**: Medium (4-6/10)

#### C. Detail Views
- **Current**: Full page views with master-detail patterns
- **Target**: Angular components with routing
- **Complexity**: Low-Medium (2-4/10)

#### D. Dashboard/Reports
- **Current**: Views with Chart.js integration
- **Target**: Angular components with ngx-charts
- **Complexity**: Medium-High (5-7/10)

## Detailed Component Mappings

### 1. Cohorts Module

#### Current MVC Structure
```
Areas/App/Views/Cohorts/
├── Index.cshtml                    # Main list view
├── _CreateOrEditModal.cshtml        # Create/Edit modal
├── _ViewCohortModal.cshtml          # View details modal
└── _CohortUserLookupTableModal.cshtml # User lookup modal
```

#### Target Angular Structure
```
main/cohorts/
├── cohorts.component.ts             # Main list component
├── cohorts.component.html
├── create-edit-cohort-modal.component.ts
├── create-edit-cohort-modal.component.html
├── cohort-detail.component.ts       # Detail view (new route)
├── cohort-detail.component.html
├── cohort-user-lookup.component.ts  # User lookup component
└── cohorts-routing.module.ts
```

#### Migration Details

**A. Main List Component (Index.cshtml → cohorts.component.ts)**

*Current MVC Implementation:*
```html
<!-- Areas/App/Views/Cohorts/Index.cshtml -->
<div class="content d-flex flex-column flex-column-fluid">
    <div class="subheader">
        <div class="container-fluid">
            <h3 class="subheader-title">@L("Cohorts")</h3>
        </div>
    </div>
    
    <div class="container-fluid">
        <div class="card">
            <div class="card-header">
                <div class="card-title">
                    <h3>@L("Cohorts")</h3>
                </div>
                <div class="card-toolbar">
                    <button id="CreateNewCohortButton" class="btn btn-primary">
                        <i class="fa fa-plus"></i> @L("CreateNewCohort")
                    </button>
                </div>
            </div>
            <div class="card-body">
                <!-- Filters -->
                <div class="row mb-3">
                    <div class="col-md-4">
                        <input id="CohortsTableFilter" class="form-control" 
                               placeholder="@L("SearchWithThreeDots")" />
                    </div>
                    <div class="col-md-4">
                        <select id="NameFilterId" class="form-control">
                            <option value="">@L("AllNames")</option>
                        </select>
                    </div>
                </div>
                
                <!-- DataTable -->
                <table id="CohortsTable" class="display table table-striped table-bordered">
                    <thead>
                        <tr>
                            <th>@L("Actions")</th>
                            <th>@L("Name")</th>
                            <th>@L("Description")</th>
                            <th>@L("DefaultCohort")</th>
                            <th>@L("TenantDepartment")</th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
    </div>
</div>
```

*Target Angular Implementation:*
```html
<!-- main/cohorts/cohorts.component.html -->
<div class="content d-flex flex-column flex-column-fluid">
  <div class="subheader">
    <div class="container-fluid">
      <h3 class="subheader-title">{{l('Cohorts')}}</h3>
    </div>
  </div>
  
  <div class="container-fluid">
    <div class="card">
      <div class="card-header">
        <div class="card-title">
          <h3>{{l('Cohorts')}}</h3>
        </div>
        <div class="card-toolbar">
          <button *ngIf="isGranted('Pages.Cohorts.Create')"
                  (click)="createCohort()" 
                  class="btn btn-primary">
            <i class="fa fa-plus"></i> {{l('CreateNewCohort')}}
          </button>
        </div>
      </div>
      
      <div class="card-body">
        <!-- Filters -->
        <div class="row mb-3">
          <div class="col-md-4">
            <input [(ngModel)]="filters.filter" 
                   (keyup.enter)="getCohorts()"
                   class="form-control" 
                   [placeholder]="l('SearchWithThreeDots')" />
          </div>
          <div class="col-md-4">
            <p-dropdown [(ngModel)]="filters.nameFilter"
                       [options]="nameFilterOptions"
                       optionLabel="label"
                       optionValue="value"
                       [placeholder]="l('AllNames')"
                       (onChange)="getCohorts()">
            </p-dropdown>
          </div>
          <div class="col-md-4">
            <button (click)="getCohorts()" class="btn btn-light me-2">
              <i class="fa fa-search"></i> {{l('Search')}}
            </button>
            <button (click)="clearFilters()" class="btn btn-secondary">
              <i class="fa fa-times"></i> {{l('Clear')}}
            </button>
          </div>
        </div>
        
        <!-- Data Table -->
        <p-table [value]="cohorts" 
                 [paginator]="true" 
                 [rows]="pageSize"
                 [totalRecords]="totalCount"
                 [lazy]="true"
                 [loading]="loading"
                 [rowsPerPageOptions]="[10, 25, 50, 100]"
                 (onLazyLoad)="getCohorts($event)"
                 [trackBy]="trackByCohortId">
          
          <ng-template pTemplate="header">
            <tr>
              <th style="width: 150px">{{l('Actions')}}</th>
              <th pSortableColumn="name">
                {{l('Name')}} <p-sortIcon field="name"></p-sortIcon>
              </th>
              <th pSortableColumn="description">
                {{l('Description')}} <p-sortIcon field="description"></p-sortIcon>
              </th>
              <th>{{l('DefaultCohort')}}</th>
              <th>{{l('TenantDepartment')}}</th>
            </tr>
          </ng-template>
          
          <ng-template pTemplate="body" let-record let-rowIndex="rowIndex">
            <tr>
              <td>
                <div class="btn-group" dropdown>
                  <button class="btn btn-primary btn-sm dropdown-toggle" 
                          dropdownToggle>
                    <i class="fa fa-cog"></i> {{l('Actions')}}
                  </button>
                  <ul class="dropdown-menu" *dropdownMenu>
                    <li>
                      <a class="dropdown-item" 
                         (click)="viewCohort(record.cohort)">
                        <i class="fa fa-eye"></i> {{l('View')}}
                      </a>
                    </li>
                    <li *ngIf="isGranted('Pages.Cohorts.Edit')">
                      <a class="dropdown-item" 
                         (click)="editCohort(record.cohort)">
                        <i class="fa fa-edit"></i> {{l('Edit')}}
                      </a>
                    </li>
                    <li *ngIf="isGranted('Pages.Cohorts.Delete')">
                      <a class="dropdown-item" 
                         (click)="deleteCohort(record.cohort)">
                        <i class="fa fa-trash"></i> {{l('Delete')}}
                      </a>
                    </li>
                  </ul>
                </div>
              </td>
              <td>{{record.cohort.name}}</td>
              <td>{{record.cohort.description}}</td>
              <td>
                <span class="badge" 
                      [ngClass]="record.cohort.defaultCohort ? 'badge-success' : 'badge-secondary'">
                  {{record.cohort.defaultCohort ? l('Yes') : l('No')}}
                </span>
              </td>
              <td>{{record.tenantDepartmentName}}</td>
            </tr>
          </ng-template>
          
          <ng-template pTemplate="emptymessage">
            <tr>
              <td colspan="5" class="text-center">{{l('NoDataAvailable')}}</td>
            </tr>
          </ng-template>
        </p-table>
      </div>
    </div>
  </div>
</div>

<!-- Modals -->
<create-edit-cohort-modal #createOrEditModal 
                          (modalSave)="onModalSaved()">
</create-edit-cohort-modal>
```

**B. Modal Component (_CreateOrEditModal.cshtml → create-edit-cohort-modal.component.ts)**

*Current MVC Modal:*
```html
<!-- Areas/App/Views/Cohorts/_CreateOrEditModal.cshtml -->
<div class="modal-header">
    <h4 class="modal-title">
        <span>@L("CreateNewCohort")</span>
        <span>@L("EditCohort")</span>
    </h4>
    <button type="button" class="close" data-dismiss="modal">
        <span>&times;</span>
    </button>
</div>

<div class="modal-body">
    <form name="cohortInformationsForm" role="form" novalidate class="form-validation">
        <div class="form-group">
            <label for="Cohort_Name">@L("Name") *</label>
            <input class="form-control" id="Cohort_Name" type="text" name="Name" required maxlength="@CohortConsts.MaxNameLength">
        </div>
        
        <div class="form-group">
            <label for="Cohort_Description">@L("Description")</label>
            <input class="form-control" id="Cohort_Description" type="text" name="Description" maxlength="@CohortConsts.MaxDescriptionLength">
        </div>
        
        <div class="form-group">
            <div class="checkbox">
                <input id="Cohort_DefaultCohort" type="checkbox" name="DefaultCohort" value="true">
                <label for="Cohort_DefaultCohort">@L("DefaultCohort")</label>
            </div>
        </div>
        
        <div class="form-group">
            <label for="CohortTenantDepartmentDisplayProperty">@L("TenantDepartment")</label>
            <div class="input-group">
                <input class="form-control" id="CohortTenantDepartmentDisplayProperty" type="text" readonly>
                <div class="input-group-append">
                    <button class="btn btn-primary" type="button" id="OpenCohortTenantDepartmentLookupTableButton">
                        <i class="fa fa-search"></i>
                    </button>
                    <button class="btn btn-secondary" type="button" id="ClearCohortTenantDepartmentDisplayPropertyButton">
                        <i class="fa fa-times"></i>
                    </button>
                </div>
            </div>
        </div>
    </form>
</div>

<div class="modal-footer">
    <button type="button" class="btn btn-secondary" data-dismiss="modal">@L("Cancel")</button>
    <button type="button" class="btn btn-primary" id="SaveCohortButton">@L("Save")</button>
</div>
```

*Target Angular Modal:*
```html
<!-- main/cohorts/create-edit-cohort-modal.component.html -->
<div class="modal fade" bsModal #modal="bs-modal" 
     [config]="{backdrop: 'static', keyboard: false}"
     (onShown)="onShown()" tabindex="-1">
  <div class="modal-dialog modal-lg">
    <div class="modal-content">
      <form #createOrEditForm="ngForm" novalidate (ngSubmit)="save()">
        
        <div class="modal-header">
          <h4 class="modal-title">
            <span *ngIf="!cohort.id">{{l('CreateNewCohort')}}</span>
            <span *ngIf="cohort.id">{{l('EditCohort')}}</span>
          </h4>
          <button type="button" class="btn-close" (click)="close()"></button>
        </div>
        
        <div class="modal-body">
          <div class="row">
            <div class="col-md-12">
              <div class="form-group">
                <label for="cohort_name">{{l('Name')}} *</label>
                <input #nameInput
                       id="cohort_name" 
                       class="form-control" 
                       type="text" 
                       name="name"
                       [(ngModel)]="cohort.name" 
                       required 
                       [maxlength]="cohortConsts.maxNameLength"
                       [class.is-invalid]="nameInput.invalid && nameInput.touched">
                <div class="invalid-feedback" 
                     *ngIf="nameInput.invalid && nameInput.touched">
                  <span *ngIf="nameInput.errors?.['required']">
                    {{l('ThisFieldIsRequired')}}
                  </span>
                  <span *ngIf="nameInput.errors?.['maxlength']">
                    {{l('MaxLengthExceeded', cohortConsts.maxNameLength)}}
                  </span>
                </div>
              </div>
              
              <div class="form-group">
                <label for="cohort_description">{{l('Description')}}</label>
                <input id="cohort_description" 
                       class="form-control" 
                       type="text" 
                       name="description"
                       [(ngModel)]="cohort.description" 
                       [maxlength]="cohortConsts.maxDescriptionLength">
              </div>
              
              <div class="form-group">
                <div class="form-check">
                  <input id="cohort_defaultCohort" 
                         class="form-check-input" 
                         type="checkbox" 
                         name="defaultCohort"
                         [(ngModel)]="cohort.defaultCohort">
                  <label class="form-check-label" for="cohort_defaultCohort">
                    {{l('DefaultCohort')}}
                  </label>
                </div>
              </div>
              
              <div class="form-group">
                <label>{{l('TenantDepartment')}}</label>
                <div class="input-group">
                  <input class="form-control" 
                         type="text" 
                         [value]="tenantDepartmentName"
                         readonly>
                  <div class="input-group-append">
                    <button class="btn btn-primary" 
                            type="button" 
                            (click)="openSelectTenantDepartmentModal()">
                      <i class="fa fa-search"></i>
                    </button>
                    <button class="btn btn-secondary" 
                            type="button" 
                            (click)="setTenantDepartmentIdNull()">
                      <i class="fa fa-times"></i>
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        
        <div class="modal-footer">
          <button type="button" 
                  class="btn btn-secondary" 
                  (click)="close()">
            {{l('Cancel')}}
          </button>
          <button type="submit" 
                  class="btn btn-primary" 
                  [disabled]="saving || createOrEditForm.invalid">
            <i class="fa fa-save"></i> 
            <span>{{l('Save')}}</span>
          </button>
        </div>
      </form>
    </div>
  </div>
</div>

<!-- Lookup Modal -->
<cohort-tenant-department-lookup-table-modal #cohortTenantDepartmentLookupTableModal
                                              (modalSave)="getNewTenantDepartmentId()">
</cohort-tenant-department-lookup-table-modal>
```

### 2. Records Module (Complex File Upload)

#### Current MVC Structure
```
Areas/App/Views/Records/
├── Index.cshtml                     # Main list with file previews
├── _CreateOrEditModal.cshtml        # Create/Edit with file upload
├── _ViewRecordModal.cshtml          # View with file download
└── _RecordCategoryLookupTableModal.cshtml
```

#### Target Angular Structure
```
main/records/
├── records.component.ts             # Main list component
├── records.component.html
├── create-edit-record-modal.component.ts
├── create-edit-record-modal.component.html
├── record-detail.component.ts       # Detail view with file management
├── record-detail.component.html
├── file-upload.component.ts         # Reusable file upload
├── file-upload.component.html
├── file-preview.component.ts        # File preview component
└── records-routing.module.ts
```

#### Migration Details

**A. File Upload Integration**

*Current MVC Implementation:*
```html
<!-- File upload in _CreateOrEditModal.cshtml -->
<div class="form-group">
    <label>@L("File")</label>
    <div id="RecordFileUploadArea" class="dropzone">
        <div class="dz-message">
            <i class="fa fa-cloud-upload"></i>
            <span>@L("DropFilesHereOrClickToUpload")</span>
        </div>
    </div>
</div>

<script>
$("#RecordFileUploadArea").dropzone({
    url: abp.appPath + "App/Records/UploadFile",
    paramName: "file",
    maxFilesize: 10,
    acceptedFiles: ".pdf,.doc,.docx,.jpg,.jpeg,.png",
    success: function(file, response) {
        $("#Record_BinaryObjectId").val(response.id);
    }
});
</script>
```

*Target Angular Implementation:*
```html
<!-- file-upload.component.html -->
<div class="form-group">
  <label>{{l('File')}}</label>
  <p-fileUpload #fileUpload
                [customUpload]="true"
                (uploadHandler)="onUpload($event)"
                [accept]="acceptedFileTypes"
                [maxFileSize]="maxFileSize"
                [showUploadButton]="false"
                [showCancelButton]="false"
                [multiple]="false">
    
    <ng-template pTemplate="header" let-files let-chooseCallback let-clearCallback>
      <div class="d-flex justify-content-between align-items-center">
        <div>
          <button type="button" 
                  class="btn btn-primary" 
                  (click)="chooseCallback()">
            <i class="fa fa-plus"></i> {{l('ChooseFile')}}
          </button>
          <button type="button" 
                  class="btn btn-secondary ms-2" 
                  (click)="clearCallback()" 
                  [disabled]="!files || files.length === 0">
            <i class="fa fa-times"></i> {{l('Clear')}}
          </button>
        </div>
        <div *ngIf="uploadProgress > 0" class="ms-3">
          <p-progressBar [value]="uploadProgress" 
                         [showValue]="true">
          </p-progressBar>
        </div>
      </div>
    </ng-template>
    
    <ng-template pTemplate="content" let-files>
      <div *ngIf="files && files.length > 0">
        <div class="file-list">
          <div *ngFor="let file of files" class="file-item d-flex align-items-center mb-2">
            <div class="file-info flex-grow-1">
              <div class="file-name">{{file.name}}</div>
              <div class="file-size text-muted">{{formatFileSize(file.size)}}</div>
            </div>
            <div class="file-actions">
              <button type="button" 
                      class="btn btn-success btn-sm" 
                      (click)="uploadFile(file)"
                      [disabled]="uploading">
                <i class="fa fa-upload"></i> {{l('Upload')}}
              </button>
            </div>
          </div>
        </div>
      </div>
      
      <div *ngIf="uploadedFile" class="uploaded-file mt-3">
        <div class="alert alert-success">
          <i class="fa fa-check"></i> 
          {{l('FileUploadedSuccessfully')}}: {{uploadedFile.fileName}}
        </div>
      </div>
    </ng-template>
  </p-fileUpload>
</div>
```

```typescript
// file-upload.component.ts
export class FileUploadComponent {
  @Input() acceptedFileTypes = '.pdf,.doc,.docx,.jpg,.jpeg,.png';
  @Input() maxFileSize = 10485760; // 10MB
  @Output() fileUploaded = new EventEmitter<UploadedFile>();
  
  uploadProgress = 0;
  uploading = false;
  uploadedFile: UploadedFile | null = null;

  onUpload(event: any): void {
    // Handle custom upload logic
  }

  uploadFile(file: File): void {
    this.uploading = true;
    this.uploadProgress = 0;

    this._fileUploadService.uploadFile(file)
      .subscribe({
        next: (event) => {
          if (event.type === HttpEventType.UploadProgress) {
            this.uploadProgress = Math.round(100 * event.loaded / event.total);
          } else if (event.type === HttpEventType.Response) {
            this.uploadedFile = event.body;
            this.fileUploaded.emit(this.uploadedFile);
            this.uploading = false;
            this.uploadProgress = 0;
          }
        },
        error: (error) => {
          this.uploading = false;
          this.uploadProgress = 0;
          // Handle error
        }
      });
  }
}
```

### 3. Dashboard Module (Charts and Widgets)

#### Current MVC Structure
```
Areas/App/Views/Dashboard/
├── Index.cshtml                     # Main dashboard
├── _ComplianceWidget.cshtml         # Compliance chart widget
├── _RecentRecordsWidget.cshtml      # Recent records widget
└── _StatisticsWidget.cshtml         # Statistics cards
```

#### Target Angular Structure
```
main/dashboard/
├── dashboard.component.ts           # Main dashboard container
├── dashboard.component.html
├── widgets/
│   ├── compliance-chart.component.ts
│   ├── compliance-chart.component.html
│   ├── recent-records.component.ts
│   ├── recent-records.component.html
│   ├── statistics-cards.component.ts
│   └── statistics-cards.component.html
└── dashboard-routing.module.ts
```

#### Migration Details

**A. Chart Component Migration**

*Current MVC Chart Implementation:*
```html
<!-- _ComplianceWidget.cshtml -->
<div class="card">
    <div class="card-header">
        <h3 class="card-title">@L("ComplianceOverview")</h3>
    </div>
    <div class="card-body">
        <canvas id="complianceChart" width="400" height="200"></canvas>
    </div>
</div>

<script>
var ctx = document.getElementById('complianceChart').getContext('2d');
var complianceChart = new Chart(ctx, {
    type: 'doughnut',
    data: {
        labels: ['Compliant', 'Non-Compliant', 'Pending'],
        datasets: [{
            data: [65, 25, 10],
            backgroundColor: ['#28a745', '#dc3545', '#ffc107']
        }]
    }
});
</script>
```

*Target Angular Chart Implementation:*
```html
<!-- widgets/compliance-chart.component.html -->
<div class="card">
  <div class="card-header">
    <h3 class="card-title">{{l('ComplianceOverview')}}</h3>
    <div class="card-toolbar">
      <p-dropdown [(ngModel)]="selectedPeriod"
                 [options]="periodOptions"
                 optionLabel="label"
                 optionValue="value"
                 (onChange)="onPeriodChange()">
      </p-dropdown>
    </div>
  </div>
  <div class="card-body">
    <div *ngIf="loading" class="text-center">
      <i class="fa fa-spinner fa-spin"></i> {{l('Loading')}}
    </div>
    
    <ngx-charts-pie-chart *ngIf="!loading && chartData.length > 0"
                          [results]="chartData"
                          [view]="[400, 200]"
                          [labels]="true"
                          [doughnut]="true"
                          [arcWidth]="0.5"
                          [gradient]="false"
                          [legend]="true"
                          [legendPosition]="'below'"
                          [tooltipDisabled]="false"
                          (select)="onChartSelect($event)">
    </ngx-charts-pie-chart>
    
    <div *ngIf="!loading && chartData.length === 0" class="text-center text-muted">
      {{l('NoDataAvailable')}}
    </div>
  </div>
</div>
```

```typescript
// widgets/compliance-chart.component.ts
@Component({
  selector: 'app-compliance-chart',
  templateUrl: './compliance-chart.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ComplianceChartComponent extends AppComponentBase implements OnInit {
  chartData: any[] = [];
  loading = false;
  selectedPeriod = 'month';
  
  periodOptions = [
    { label: this.l('ThisMonth'), value: 'month' },
    { label: this.l('ThisQuarter'), value: 'quarter' },
    { label: this.l('ThisYear'), value: 'year' }
  ];

  constructor(
    injector: Injector,
    private _dashboardService: DashboardService,
    private _cdr: ChangeDetectorRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.loadChartData();
  }

  loadChartData(): void {
    this.loading = true;
    this._dashboardService.getComplianceData(this.selectedPeriod)
      .pipe(
        finalize(() => {
          this.loading = false;
          this._cdr.detectChanges();
        }),
        takeUntil(this.destroy$)
      )
      .subscribe(result => {
        this.chartData = [
          { name: this.l('Compliant'), value: result.compliant },
          { name: this.l('NonCompliant'), value: result.nonCompliant },
          { name: this.l('Pending'), value: result.pending }
        ];
      });
  }

  onPeriodChange(): void {
    this.loadChartData();
  }

  onChartSelect(event: any): void {
    // Handle chart selection
    console.log('Chart selection:', event);
  }
}
```

## Complex Migration Scenarios

### 1. Master-Detail Relationships

#### Current MVC Pattern
```html
<!-- Expandable DataTable rows with nested content -->
<table id="CohortsTable">
    <!-- Main row -->
    <tr>
        <td><button class="btn-expand" data-id="@cohort.Id">+</button></td>
        <td>@cohort.Name</td>
    </tr>
    <!-- Detail row (hidden by default) -->
    <tr class="detail-row" id="detail-@cohort.Id" style="display:none;">
        <td colspan="5">
            <!-- Nested table or content -->
            <div class="nested-content">
                <!-- Users in cohort -->
            </div>
        </td>
    </tr>
</table>
```

#### Target Angular Pattern
```html
<!-- Using PrimeNG expandable rows -->
<p-table [value]="cohorts" dataKey="id">
  <ng-template pTemplate="header">
    <tr>
      <th style="width: 3rem"></th>
      <th>{{l('Name')}}</th>
      <th>{{l('Description')}}</th>
    </tr>
  </ng-template>
  
  <ng-template pTemplate="body" let-cohort let-expanded="expanded">
    <tr>
      <td>
        <button type="button" 
                pButton 
                pRipple 
                [pRowToggler]="cohort" 
                class="p-button-text p-button-rounded p-button-plain" 
                [icon]="expanded ? 'pi pi-chevron-down' : 'pi pi-chevron-right'">
        </button>
      </td>
      <td>{{cohort.name}}</td>
      <td>{{cohort.description}}</td>
    </tr>
  </ng-template>
  
  <ng-template pTemplate="rowexpansion" let-cohort>
    <tr>
      <td colspan="3">
        <div class="p-3">
          <h5>{{l('UsersInCohort')}}</h5>
          <p-table [value]="cohort.users" 
                   [paginator]="true" 
                   [rows]="5">
            <ng-template pTemplate="header">
              <tr>
                <th>{{l('UserName')}}</th>
                <th>{{l('Email')}}</th>
                <th>{{l('Status')}}</th>
              </tr>
            </ng-template>
            <ng-template pTemplate="body" let-user>
              <tr>
                <td>{{user.userName}}</td>
                <td>{{user.emailAddress}}</td>
                <td>
                  <span class="badge" 
                        [ngClass]="user.isActive ? 'badge-success' : 'badge-secondary'">
                    {{user.isActive ? l('Active') : l('Inactive')}}
                  </span>
                </td>
              </tr>
            </ng-template>
          </p-table>
        </div>
      </td>
    </tr>
  </ng-template>
</p-table>
```

### 2. Multi-Step Wizards

#### Current MVC Pattern
```html
<!-- Multi-step form with jQuery steps -->
<div id="wizard">
    <h3>Step 1</h3>
    <section>
        <!-- Step 1 content -->
    </section>
    <h3>Step 2</h3>
    <section>
        <!-- Step 2 content -->
    </section>
</div>

<script>
$("#wizard").steps({
    headerTag: "h3",
    bodyTag: "section",
    transitionEffect: "slideLeft"
});
</script>
```

#### Target Angular Pattern
```html
<!-- Using PrimeNG Steps component -->
<p-steps [model]="wizardSteps" 
         [(activeIndex)]="activeStepIndex"
         [readonly]="false">
</p-steps>

<div class="wizard-content mt-4">
  <div *ngIf="activeStepIndex === 0">
    <!-- Step 1: Basic Information -->
    <app-basic-info-step [(data)]="wizardData.basicInfo"
                         (valid)="onStepValidation(0, $event)">
    </app-basic-info-step>
  </div>
  
  <div *ngIf="activeStepIndex === 1">
    <!-- Step 2: File Upload -->
    <app-file-upload-step [(data)]="wizardData.files"
                          (valid)="onStepValidation(1, $event)">
    </app-file-upload-step>
  </div>
  
  <div *ngIf="activeStepIndex === 2">
    <!-- Step 3: Review -->
    <app-review-step [data]="wizardData"
                     (valid)="onStepValidation(2, $event)">
    </app-review-step>
  </div>
</div>

<div class="wizard-actions mt-4">
  <button type="button" 
          class="btn btn-secondary" 
          (click)="previousStep()"
          [disabled]="activeStepIndex === 0">
    {{l('Previous')}}
  </button>
  
  <button type="button" 
          class="btn btn-primary ms-2" 
          (click)="nextStep()"
          [disabled]="!isCurrentStepValid() || activeStepIndex === wizardSteps.length - 1">
    {{l('Next')}}
  </button>
  
  <button type="button" 
          class="btn btn-success ms-2" 
          (click)="completeWizard()"
          [disabled]="!isWizardComplete()"
          *ngIf="activeStepIndex === wizardSteps.length - 1">
    {{l('Complete')}}
  </button>
</div>
```

## Migration Checklist

### Pre-Migration Preparation

- [ ] **Audit Current Views**: Catalog all existing MVC views and their complexity
- [ ] **Identify Reusable Components**: Find common patterns that can be shared
- [ ] **Plan Component Hierarchy**: Design the Angular component structure
- [ ] **Create Migration Priority**: Order components by complexity and dependencies

### Component Migration Steps

1. **Create Angular Component Structure**
   - [ ] Set up component files (.ts, .html, .less)
   - [ ] Implement base component inheritance
   - [ ] Add routing configuration

2. **Convert Templates**
   - [ ] Replace Razor syntax with Angular template syntax
   - [ ] Convert server-side localization to client-side
   - [ ] Update form validation patterns
   - [ ] Replace jQuery event handlers with Angular event binding

3. **Implement Component Logic**
   - [ ] Convert controller logic to component methods
   - [ ] Implement service calls using Angular HttpClient
   - [ ] Add proper error handling and loading states
   - [ ] Implement permission checks

4. **Style Migration**
   - [ ] Convert CSS/LESS to component-scoped styles
   - [ ] Update Bootstrap classes for newer version
   - [ ] Ensure responsive design compatibility

5. **Testing**
   - [ ] Unit test component logic
   - [ ] Integration test with services
   - [ ] E2E test user workflows
   - [ ] Cross-browser compatibility testing

### Quality Assurance

#### Functional Testing
- [ ] All CRUD operations work correctly
- [ ] File upload/download functionality
- [ ] Search and filtering capabilities
- [ ] Pagination and sorting
- [ ] Modal dialogs and navigation
- [ ] Permission-based access control

#### Performance Testing
- [ ] Initial load time optimization
- [ ] Large dataset handling
- [ ] Memory leak prevention
- [ ] Mobile responsiveness

#### User Experience
- [ ] Consistent UI/UX patterns
- [ ] Proper loading indicators
- [ ] Error message clarity
- [ ] Accessibility compliance

## Migration Timeline Estimate

### Phase 1: Foundation (2-3 weeks)
- Set up Angular project structure
- Create shared components and services
- Implement authentication and routing

### Phase 2: Core Components (4-6 weeks)
- Migrate main list components (Cohorts, Records, Users)
- Implement basic CRUD operations
- Create modal components

### Phase 3: Advanced Features (3-4 weeks)
- File upload/download functionality
- Dashboard and reporting components
- Complex forms and wizards

### Phase 4: Polish and Testing (2-3 weeks)
- Performance optimization
- Comprehensive testing
- Bug fixes and refinements

## Best Practices for Migration

### 1. Incremental Migration
- Start with simplest components first
- Test each component thoroughly before moving to the next
- Maintain parallel MVC and Angular versions during transition

### 2. Code Reusability
- Create shared components for common patterns
- Use Angular services for business logic
- Implement consistent error handling

### 3. Performance Optimization
- Use OnPush change detection strategy
- Implement lazy loading for routes
- Optimize bundle sizes with tree shaking

### 4. Maintainability
- Follow Angular style guide conventions
- Document component APIs and usage
- Implement comprehensive testing coverage

---

**Next**: [Data Flow and State Management](./05-data-flow-state-management.md) - Learn how to manage application state and data flow in the Angular architecture.
