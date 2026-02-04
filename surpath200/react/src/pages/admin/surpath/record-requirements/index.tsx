import React, { useCallback, useEffect, useState } from 'react';
import { Table, App, Dropdown, Input, Select, Button, Space } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import { SearchOutlined, PlusOutlined, FileExcelOutlined, DownOutlined, UpOutlined } from '@ant-design/icons';
import { usePermissions } from '@/hooks/usePermissions';
import { useDataTable } from '@/hooks/useDataTable';
import PageHeader from '@/pages/admin/components/common/PageHeader';
import { downloadTempFile } from '@/lib/file-download-helper';
import L from '@/lib/L';
import {
  recordRequirementsService,
  type GetRecordRequirementForViewDto,
  type GetAllRecordRequirementsInput,
} from '@/services/surpath/recordRequirements.service';
import CreateOrEditRecordRequirementModal from './components/CreateOrEditRecordRequirementModal';
import ManageCategoriesModal from './components/ManageCategoriesModal';
import { useTheme } from '@/hooks/useTheme';

/**
 * RecordRequirements Index Page
 *
 * Migrated from Surpath112:
 * - Source: wwwroot/view-resources/Areas/App/Views/RecordRequirements/Index.js
 * - View: Areas/App/Views/RecordRequirements/Index.cshtml
 *
 * Key Features:
 * - 8 filter fields (main filter + 7 advanced filters)
 * - Category badge showing count and warning for missing rules
 * - 7 permissions (Base + Create/Edit/Delete + ManageCategories/MoveCategories/CopyCategories)
 * - SurpathServiceId filter (business logic - only shows non-global requirements)
 * - Excel export
 * - Multi-tenancy support
 */

const RecordRequirementsPage: React.FC = () => {
  const { isGranted } = usePermissions();
  const { containerClass } = useTheme();
  const { modal, message } = App.useApp();

  // Main filter state
  const [filterText, setFilterText] = useState('');

  // Advanced filter state
  const [advancedFiltersVisible, setAdvancedFiltersVisible] = useState(false);
  const [nameFilter, setNameFilter] = useState('');
  const [descriptionFilter, setDescriptionFilter] = useState('');
  const [metadataFilter, setMetadataFilter] = useState('');
  const [isSurpathOnlyFilter, setIsSurpathOnlyFilter] = useState<number | undefined>();
  const [tenantDepartmentNameFilter, setTenantDepartmentNameFilter] = useState('');
  const [cohortNameFilter, setCohortNameFilter] = useState('');
  const [surpathServiceNameFilter, setSurpathServiceNameFilter] = useState('');
  const [tenantSurpathServiceNameFilter, setTenantSurpathServiceNameFilter] = useState('');

  // Modal state
  const [createEditVisible, setCreateEditVisible] = useState(false);
  const [editingId, setEditingId] = useState<string | undefined>();
  const [manageCategoriesVisible, setManageCategoriesVisible] = useState(false);
  const [managingRequirementId, setManagingRequirementId] = useState<string | undefined>();

  // Permission checks
  const permissions = {
    base: isGranted('Pages.Administration.RecordRequirements'),
    create: isGranted('Pages.Administration.RecordRequirements.Create'),
    edit: isGranted('Pages.Administration.RecordRequirements.Edit'),
    delete: isGranted('Pages.Administration.RecordRequirements.Delete'),
    manageCategories: isGranted('Pages.Administration.RecordRequirements.ManageCategories'),
    moveCategories: isGranted('Pages.Administration.RecordRequirements.MoveCategories'),
    copyCategories: isGranted('Pages.Administration.RecordRequirements.CopyCategories'),
  };

  // Data fetching
  const fetchFn = useCallback(
    (skipCount: number, maxResultCount: number, sorting: string) => {
      const input: GetAllRecordRequirementsInput = {
        filter: filterText,
        nameFilter,
        descriptionFilter,
        metadataFilter,
        isSurpathOnlyFilter,
        tenantDepartmentNameFilter,
        cohortNameFilter,
        surpathServiceNameFilter,
        tenantSurpathServiceNameFilter,
        sorting,
        maxResultCount,
        skipCount,
      };
      return recordRequirementsService.getAll(input);
    },
    [
      filterText,
      nameFilter,
      descriptionFilter,
      metadataFilter,
      isSurpathOnlyFilter,
      tenantDepartmentNameFilter,
      cohortNameFilter,
      surpathServiceNameFilter,
      tenantSurpathServiceNameFilter,
    ]
  );

  const { records, loading, pagination, handleTableChange, fetchData } =
    useDataTable<GetRecordRequirementForViewDto>(fetchFn);

  useEffect(() => {
    fetchData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  // Action handlers
  const handleCreate = () => {
    setEditingId(undefined);
    setCreateEditVisible(true);
  };

  const handleEdit = (id: string) => {
    setEditingId(id);
    setCreateEditVisible(true);
  };

  const handleManageCategories = (id: string) => {
    setManagingRequirementId(id);
    setManageCategoriesVisible(true);
  };

  const handleDelete = (record: GetRecordRequirementForViewDto) => {
    modal.confirm({
      title: L('AreYouSure'),
      content: L('RecordRequirementDeleteWarningMessage', record.recordRequirement.name),
      onOk: async () => {
        try {
          await recordRequirementsService.delete({ id: record.recordRequirement.id });
          message.success(L('SuccessfullyDeleted'));
          fetchData();
        } catch (error: any) {
          message.error(error.message || L('AnErrorOccurred'));
        }
      },
    });
  };

  const handleExportToExcel = async () => {
    try {
      const file = await recordRequirementsService.getRecordRequirementsToExcel({
        filter: filterText,
        nameFilter,
        descriptionFilter,
        metadataFilter,
        isSurpathOnlyFilter,
        tenantDepartmentNameFilter,
        cohortNameFilter,
        surpathServiceNameFilter,
        tenantSurpathServiceNameFilter,
      });
      downloadTempFile(file);
    } catch (error: any) {
      message.error(error.message || L('ExportToExcelFailed'));
    }
  };

  const handleResetFilters = () => {
    setNameFilter('');
    setDescriptionFilter('');
    setMetadataFilter('');
    setIsSurpathOnlyFilter(undefined);
    setTenantDepartmentNameFilter('');
    setCohortNameFilter('');
    setSurpathServiceNameFilter('');
    setTenantSurpathServiceNameFilter('');
  };

  // Render category badge with count and warning
  const renderCategoryBadge = (record: GetRecordRequirementForViewDto) => {
    const categories = record.recordRequirement.categoryDTOs || [];
    const categoryCount = categories.length;
    const noRuleCount = categories.filter(c => !c.recordCategoryRuleId).length;

    return (
      <>
        <span>{record.recordRequirement.name}</span>
        {categoryCount > 1 && (
          <span
            className="badge badge-sm badge-warning ms-2"
            title={L('StepsRequiredForCompliance', categoryCount.toString())}
          >
            {categoryCount}
          </span>
        )}
        {noRuleCount > 0 && (
          <span
            className="badge badge-sm badge-danger ms-1"
            title={L('RequirementHasNoRuleWarning')}
          >
            !
          </span>
        )}
      </>
    );
  };

  // Build action menu items
  const getMenuItems = (record: GetRecordRequirementForViewDto) => {
    const items: Array<{
      key: string;
      label: string;
      icon?: React.ReactNode;
      onClick: () => void;
      danger?: boolean;
    }> = [];

    if (permissions.edit) {
      items.push({
        key: 'edit',
        label: L('Edit')!,
        icon: <i className="far fa-edit" />,
        onClick: () => handleEdit(record.recordRequirement.id),
      });
    }

    if (permissions.manageCategories) {
      items.push({
        key: 'manageCategories',
        label: L('ManageCategories')!,
        icon: <i className="fa fa-folder" />,
        onClick: () => handleManageCategories(record.recordRequirement.id),
      });
    }

    if (permissions.delete) {
      items.push({
        key: 'delete',
        label: L('Delete')!,
        icon: <i className="far fa-trash-alt" />,
        onClick: () => handleDelete(record),
        danger: true,
      });
    }

    return items;
  };

  // Table columns
  const columns: ColumnsType<GetRecordRequirementForViewDto> = [
    {
      title: L('Actions'),
      key: 'actions',
      width: 130,
      render: (_, record) => {
        const items = getMenuItems(record);
        if (items.length === 0) return null;

        return (
          <Dropdown menu={{ items }} trigger={['click']} placement="bottomLeft">
            <button
              type="button"
              className="btn btn-primary btn-sm dropdown-toggle d-flex align-items-center"
            >
              <i className="fa fa-cog"></i>
              <span className="ms-2">{L('Actions')}</span>
              <span className="caret ms-1"></span>
            </button>
          </Dropdown>
        );
      },
    },
    {
      title: L('Name'),
      dataIndex: ['recordRequirement', 'name'],
      key: 'name',
      sorter: true,
      render: (_, record) => renderCategoryBadge(record),
    },
    {
      title: L('Description'),
      dataIndex: ['recordRequirement', 'description'],
      key: 'description',
      sorter: true,
    },
    // Show Tenant column only for Host users
    ...(abp.session.multiTenancySide === abp.multiTenancy.sides.HOST
      ? [
          {
            title: L('TenantName'),
            dataIndex: 'tenantName',
            key: 'tenantName',
            sorter: true,
          },
        ]
      : []),
    {
      title: L('TenantDepartmentName'),
      dataIndex: 'tenantDepartmentName',
      key: 'tenantDepartmentName',
      sorter: true,
    },
    {
      title: L('CohortName'),
      dataIndex: 'cohortName',
      key: 'cohortName',
      sorter: true,
    },
  ];

  return (
    <div className="content d-flex flex-column flex-column-fluid">
      <PageHeader
        title={L('RecordRequirements')}
        description={L('RecordRequirementsHeaderInfo')}
      >
        <Button
          icon={<FileExcelOutlined />}
          onClick={handleExportToExcel}
          className="me-2"
        >
          {L('ExportToExcel')}
        </Button>
        {permissions.create && (
          <Button
            type="primary"
            icon={<PlusOutlined />}
            onClick={handleCreate}
          >
            {L('CreateNewRecordRequirement')}
          </Button>
        )}
      </PageHeader>

      <div className={containerClass}>
        <div className="card card-custom gutter-b">
          <div className="card-body">
            {/* Main filter */}
            <div className="row align-items-center mb-4">
              <div className="col-xl-12">
                <Space.Compact style={{ width: '100%' }}>
                  <Input
                    placeholder={L('SearchWithThreeDot')}
                    value={filterText}
                    onChange={(e) => setFilterText(e.target.value)}
                    onPressEnter={() => fetchData()}
                    prefix={<SearchOutlined />}
                  />
                  <Button type="primary" icon={<SearchOutlined />} onClick={() => fetchData()}>
                    {L('Search')}
                  </Button>
                </Space.Compact>
              </div>
            </div>

            {/* Advanced filters toggle */}
            <div className="row my-4">
              <div className="col-xl-12">
                <span
                  className="text-muted clickable-item"
                  onClick={() => setAdvancedFiltersVisible(!advancedFiltersVisible)}
                  style={{ cursor: 'pointer' }}
                >
                  {advancedFiltersVisible ? <UpOutlined /> : <DownOutlined />}
                  {' '}
                  {advancedFiltersVisible ? L('HideAdvancedFilters') : L('ShowAdvancedFilters')}
                </span>
              </div>
            </div>

            {/* Advanced filters */}
            {advancedFiltersVisible && (
              <div className="row mb-4">
                <div className="col-md-3">
                  <div className="my-3">
                    <label className="form-label">{L('Name')}</label>
                    <Input
                      value={nameFilter}
                      onChange={(e) => setNameFilter(e.target.value)}
                      onPressEnter={() => fetchData()}
                    />
                  </div>
                </div>
                <div className="col-md-3">
                  <div className="my-3">
                    <label className="form-label">{L('Description')}</label>
                    <Input
                      value={descriptionFilter}
                      onChange={(e) => setDescriptionFilter(e.target.value)}
                      onPressEnter={() => fetchData()}
                    />
                  </div>
                </div>
                <div className="col-md-3">
                  <div className="my-3">
                    <label className="form-label">{L('Metadata')}</label>
                    <Input
                      value={metadataFilter}
                      onChange={(e) => setMetadataFilter(e.target.value)}
                      onPressEnter={() => fetchData()}
                    />
                  </div>
                </div>
                <div className="col-md-3">
                  <div className="my-3">
                    <label className="form-label">{L('TenantDepartmentName')}</label>
                    <Input
                      value={tenantDepartmentNameFilter}
                      onChange={(e) => setTenantDepartmentNameFilter(e.target.value)}
                      onPressEnter={() => fetchData()}
                    />
                  </div>
                </div>
                <div className="col-md-3">
                  <div className="my-3">
                    <label className="form-label">{L('CohortName')}</label>
                    <Input
                      value={cohortNameFilter}
                      onChange={(e) => setCohortNameFilter(e.target.value)}
                      onPressEnter={() => fetchData()}
                    />
                  </div>
                </div>
                <div className="col-md-12 mt-3">
                  <Button onClick={handleResetFilters}>
                    {L('ResetFilters')}
                  </Button>
                </div>
              </div>
            )}

            {/* Data table */}
            <Table
              rowKey={(record) => record.recordRequirement.id}
              columns={columns}
              dataSource={records}
              loading={loading}
              pagination={pagination}
              onChange={handleTableChange}
              scroll={{ x: true }}
            />
          </div>
        </div>
      </div>

      {/* Modals */}
      {createEditVisible && (
        <CreateOrEditRecordRequirementModal
          visible={createEditVisible}
          recordRequirementId={editingId}
          onCancel={() => setCreateEditVisible(false)}
          onSave={() => {
            setCreateEditVisible(false);
            fetchData();
          }}
        />
      )}

      {manageCategoriesVisible && managingRequirementId && (
        <ManageCategoriesModal
          visible={manageCategoriesVisible}
          requirementId={managingRequirementId}
          onCancel={() => setManageCategoriesVisible(false)}
          onSave={() => {
            setManageCategoriesVisible(false);
            fetchData();
          }}
        />
      )}
    </div>
  );
};

export default RecordRequirementsPage;
