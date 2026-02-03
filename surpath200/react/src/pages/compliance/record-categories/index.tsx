/**
 * RecordCategories - Main Index Page
 *
 * Migrated from Surpath112: Areas/App/Views/RecordCategories/Index.cshtml + Index.js
 *
 * Features:
 * - List view with server-side pagination
 * - 4 filters: Name, Instructions, RecordRequirement, RecordCategoryRule
 * - CRUD operations via modals
 * - Excel export
 * - Permission-based action visibility
 */

import React, { useState, useEffect, useCallback } from 'react';
import { Table, Button, Space, Input, Card, Row, Col, Modal, message, Tag } from 'antd';
import {
  PlusOutlined,
  ReloadOutlined,
  ExportOutlined,
  EditOutlined,
  DeleteOutlined,
  EyeOutlined,
  HistoryOutlined,
  SearchOutlined,
  FilterOutlined
} from '@ant-design/icons';
import type { ColumnsType, TablePaginationConfig } from 'antd/es/table';
import { useAppSelector } from '@/store';
import { L } from '@/lib/abpUtility';
import {
  recordCategoriesService,
  type GetRecordCategoryForViewDto,
  type GetAllRecordCategoriesInput
} from '@/services/surpath/recordCategories.service';
import CreateOrEditRecordCategoryModal from './components/CreateOrEditRecordCategoryModal';
import ViewRecordCategoryModal from './components/ViewRecordCategoryModal';

const RecordCategoriesPage: React.FC = () => {
  // ============================================================================
  // STATE
  // ============================================================================
  const [data, setData] = useState<GetRecordCategoryForViewDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [totalCount, setTotalCount] = useState(0);
  const [showAdvancedFilters, setShowAdvancedFilters] = useState(false);

  // Modals
  const [createOrEditModalVisible, setCreateOrEditModalVisible] = useState(false);
  const [viewModalVisible, setViewModalVisible] = useState(false);
  const [selectedId, setSelectedId] = useState<string | undefined>();

  // Filters
  const [filters, setFilters] = useState<GetAllRecordCategoriesInput>({
    filter: '',
    nameFilter: '',
    instructionsFilter: '',
    recordRequirementNameFilter: '',
    recordCategoryRuleNameFilter: '',
    skipCount: 0,
    maxResultCount: 10,
    sorting: ''
  });

  // Permissions
  const permissions = useAppSelector(state => state.session.auth.grantedPermissions);
  const canCreate = permissions?.includes('Pages.RecordCategories.Create');
  const canEdit = permissions?.includes('Pages.RecordCategories.Edit');
  const canDelete = permissions?.includes('Pages.RecordCategories.Delete');
  const canViewHistory = permissions?.includes('Pages.Administration.AuditLogs');

  // ============================================================================
  // DATA FETCHING
  // ============================================================================

  const fetchData = useCallback(async () => {
    setLoading(true);
    try {
      const result = await recordCategoriesService.getAll(filters);
      setData(result.items || []);
      setTotalCount(result.totalCount || 0);
    } catch (error: any) {
      message.error(error.message || L('AnErrorOccurred'));
    } finally {
      setLoading(false);
    }
  }, [filters]);

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  // ============================================================================
  // ACTIONS
  // ============================================================================

  const handleCreate = () => {
    setSelectedId(undefined);
    setCreateOrEditModalVisible(true);
  };

  const handleEdit = (id: string) => {
    setSelectedId(id);
    setCreateOrEditModalVisible(true);
  };

  const handleView = (id: string) => {
    setSelectedId(id);
    setViewModalVisible(true);
  };

  const handleDelete = (record: GetRecordCategoryForViewDto) => {
    Modal.confirm({
      title: L('AreYouSure'),
      content: L('RecordCategoryDeleteWarningMessage', record.recordCategory.name),
      okText: L('Yes'),
      cancelText: L('No'),
      okType: 'danger',
      onOk: async () => {
        try {
          await recordCategoriesService.delete({ id: record.recordCategory.id });
          message.success(L('SuccessfullyDeleted'));
          fetchData();
        } catch (error: any) {
          message.error(error.message || L('AnErrorOccurred'));
        }
      }
    });
  };

  const handleExportToExcel = async () => {
    try {
      const fileDto = await recordCategoriesService.getRecordCategoriesToExcel({
        filter: filters.filter,
        nameFilter: filters.nameFilter,
        instructionsFilter: filters.instructionsFilter,
        recordRequirementNameFilter: filters.recordRequirementNameFilter,
        recordCategoryRuleNameFilter: filters.recordCategoryRuleNameFilter
      });

      // Trigger download using ABP's downloadTempFile utility
      const downloadUrl = `/File/DownloadTempFile?fileToken=${fileDto.fileToken}&fileName=${fileDto.fileName}`;
      window.location.href = downloadUrl;
    } catch (error: any) {
      message.error(error.message || L('AnErrorOccurred'));
    }
  };

  const handleRefresh = () => {
    fetchData();
  };

  const handleResetFilters = () => {
    setFilters({
      filter: '',
      nameFilter: '',
      instructionsFilter: '',
      recordRequirementNameFilter: '',
      recordCategoryRuleNameFilter: '',
      skipCount: 0,
      maxResultCount: filters.maxResultCount,
      sorting: filters.sorting
    });
  };

  const handleModalClose = () => {
    setCreateOrEditModalVisible(false);
    setViewModalVisible(false);
    setSelectedId(undefined);
    fetchData();
  };

  // ============================================================================
  // TABLE CONFIGURATION
  // ============================================================================

  const columns: ColumnsType<GetRecordCategoryForViewDto> = [
    {
      title: L('Actions'),
      key: 'actions',
      width: 150,
      fixed: 'left',
      render: (_, record) => (
        <Space size="small">
          <Button
            type="link"
            icon={<EyeOutlined />}
            onClick={() => handleView(record.recordCategory.id)}
            title={L('View')}
          />
          {canEdit && (
            <Button
              type="link"
              icon={<EditOutlined />}
              onClick={() => handleEdit(record.recordCategory.id)}
              title={L('Edit')}
            />
          )}
          {canDelete && (
            <Button
              type="link"
              danger
              icon={<DeleteOutlined />}
              onClick={() => handleDelete(record)}
              title={L('Delete')}
            />
          )}
          {canViewHistory && (
            <Button
              type="link"
              icon={<HistoryOutlined />}
              title={L('History')}
              onClick={() => {
                message.info('Entity history feature coming soon');
              }}
            />
          )}
        </Space>
      )
    },
    {
      title: L('Name'),
      dataIndex: ['recordCategory', 'name'],
      key: 'name',
      sorter: true,
      width: 200
    },
    {
      title: L('Instructions'),
      dataIndex: ['recordCategory', 'instructions'],
      key: 'instructions',
      width: 300,
      ellipsis: true
    },
    {
      title: L('RecordRequirement'),
      dataIndex: 'recordRequirementName',
      key: 'recordRequirementName',
      width: 200
    },
    {
      title: L('RecordCategoryRule'),
      dataIndex: 'recordCategoryRuleName',
      key: 'recordCategoryRuleName',
      width: 200
    }
  ];

  const handleTableChange = (
    pagination: TablePaginationConfig,
    _filters: any,
    sorter: any
  ) => {
    const newFilters = {
      ...filters,
      skipCount: ((pagination.current || 1) - 1) * (pagination.pageSize || 10),
      maxResultCount: pagination.pageSize || 10,
      sorting: sorter.field ? `${sorter.field} ${sorter.order === 'ascend' ? 'ASC' : 'DESC'}` : ''
    };
    setFilters(newFilters);
  };

  // ============================================================================
  // RENDER
  // ============================================================================

  return (
    <Card>
      {/* Header */}
      <Row gutter={[16, 16]} style={{ marginBottom: 16 }}>
        <Col flex="auto">
          <h2>{L('RecordCategories')}</h2>
        </Col>
        <Col>
          <Space>
            {canCreate && (
              <Button
                type="primary"
                icon={<PlusOutlined />}
                onClick={handleCreate}
              >
                {L('Create')}
              </Button>
            )}
            <Button
              icon={<ExportOutlined />}
              onClick={handleExportToExcel}
            >
              {L('ExportToExcel')}
            </Button>
            <Button
              icon={<ReloadOutlined />}
              onClick={handleRefresh}
            >
              {L('Refresh')}
            </Button>
          </Space>
        </Col>
      </Row>

      {/* Basic Filter */}
      <Row gutter={[16, 16]} style={{ marginBottom: 16 }}>
        <Col span={18}>
          <Input
            placeholder={L('SearchWithThreeDot')}
            value={filters.filter}
            onChange={(e) => setFilters({ ...filters, filter: e.target.value, skipCount: 0 })}
            onPressEnter={fetchData}
            prefix={<SearchOutlined />}
            allowClear
          />
        </Col>
        <Col span={6}>
          <Button
            block
            icon={<FilterOutlined />}
            onClick={() => setShowAdvancedFilters(!showAdvancedFilters)}
          >
            {showAdvancedFilters ? L('HideAdvancedFilters') : L('ShowAdvancedFilters')}
          </Button>
        </Col>
      </Row>

      {/* Advanced Filters */}
      {showAdvancedFilters && (
        <Card size="small" style={{ marginBottom: 16, backgroundColor: '#fafafa' }}>
          <Row gutter={[16, 16]}>
            <Col span={12}>
              <Input
                placeholder={L('Name')}
                value={filters.nameFilter}
                onChange={(e) => setFilters({ ...filters, nameFilter: e.target.value, skipCount: 0 })}
                onPressEnter={fetchData}
                allowClear
              />
            </Col>
            <Col span={12}>
              <Input
                placeholder={L('Instructions')}
                value={filters.instructionsFilter}
                onChange={(e) => setFilters({ ...filters, instructionsFilter: e.target.value, skipCount: 0 })}
                onPressEnter={fetchData}
                allowClear
              />
            </Col>
            <Col span={12}>
              <Input
                placeholder={L('RecordRequirement')}
                value={filters.recordRequirementNameFilter}
                onChange={(e) => setFilters({ ...filters, recordRequirementNameFilter: e.target.value, skipCount: 0 })}
                onPressEnter={fetchData}
                allowClear
              />
            </Col>
            <Col span={12}>
              <Input
                placeholder={L('RecordCategoryRule')}
                value={filters.recordCategoryRuleNameFilter}
                onChange={(e) => setFilters({ ...filters, recordCategoryRuleNameFilter: e.target.value, skipCount: 0 })}
                onPressEnter={fetchData}
                allowClear
              />
            </Col>
            <Col span={24}>
              <Space>
                <Button onClick={fetchData} type="primary">
                  {L('Search')}
                </Button>
                <Button onClick={handleResetFilters}>
                  {L('Reset')}
                </Button>
              </Space>
            </Col>
          </Row>
        </Card>
      )}

      {/* Table */}
      <Table
        rowKey={(record) => record.recordCategory.id}
        columns={columns}
        dataSource={data}
        loading={loading}
        onChange={handleTableChange}
        pagination={{
          current: (filters.skipCount || 0) / (filters.maxResultCount || 10) + 1,
          pageSize: filters.maxResultCount,
          total: totalCount,
          showSizeChanger: true,
          showTotal: (total) => L('TotalRecordsCount', total),
          pageSizeOptions: ['5', '10', '25', '50', '100', '250', '500', '5000']
        }}
        scroll={{ x: 1000 }}
      />

      {/* Modals */}
      {createOrEditModalVisible && (
        <CreateOrEditRecordCategoryModal
          visible={createOrEditModalVisible}
          recordCategoryId={selectedId}
          onCancel={handleModalClose}
          onSuccess={handleModalClose}
        />
      )}

      {viewModalVisible && selectedId && (
        <ViewRecordCategoryModal
          visible={viewModalVisible}
          recordCategoryId={selectedId}
          onClose={handleModalClose}
        />
      )}
    </Card>
  );
};

export default RecordCategoriesPage;
