/**
 * RecordCategoryRules - Main Index Page
 *
 * Migrated from Surpath112: Areas/App/Views/RecordCategoryRules/Index.cshtml + Index.js
 *
 * Features:
 * - List view with server-side pagination
 * - 11 filters including numeric ranges and boolean filters
 * - Boolean columns with icon rendering
 * - Full-page Edit/View (not modals)
 * - Excel export
 * - Permission-based action visibility
 * - Host-only column (IsSurpathOnly)
 */

import React, { useState, useEffect, useCallback } from 'react';
import { Table, Button, Space, Input, Card, Row, Col, Modal, message, Select, InputNumber, Tag } from 'antd';
import {
  PlusOutlined,
  ReloadOutlined,
  ExportOutlined,
  EditOutlined,
  DeleteOutlined,
  EyeOutlined,
  HistoryOutlined,
  SearchOutlined,
  FilterOutlined,
  CheckCircleOutlined,
  CloseCircleOutlined
} from '@ant-design/icons';
import type { ColumnsType, TablePaginationConfig } from 'antd/es/table';
import { useNavigate } from 'react-router-dom';
import { useAppSelector } from '@/store';
import { L } from '@/lib/abpUtility';
import {
  recordCategoryRulesService,
  type GetRecordCategoryRuleForViewDto,
  type GetAllRecordCategoryRulesInput
} from '@/services/surpath/recordCategoryRules.service';

const RecordCategoryRulesPage: React.FC = () => {
  const navigate = useNavigate();

  // ============================================================================
  // STATE
  // ============================================================================
  const [data, setData] = useState<GetRecordCategoryRuleForViewDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [totalCount, setTotalCount] = useState(0);
  const [showAdvancedFilters, setShowAdvancedFilters] = useState(false);

  // Filters
  const [filters, setFilters] = useState<GetAllRecordCategoryRulesInput>({
    filter: '',
    nameFilter: '',
    descriptionFilter: '',
    notifyFilter: undefined,
    minExpireInDaysFilter: undefined,
    maxExpireInDaysFilter: undefined,
    minWarnDaysBeforeFirstFilter: undefined,
    maxWarnDaysBeforeFirstFilter: undefined,
    expiresFilter: undefined,
    requiredFilter: undefined,
    isSurpathOnlyFilter: undefined,
    minWarnDaysBeforeSecondFilter: undefined,
    maxWarnDaysBeforeSecondFilter: undefined,
    minWarnDaysBeforeFinalFilter: undefined,
    maxWarnDaysBeforeFinalFilter: undefined,
    metaDataFilter: '',
    skipCount: 0,
    maxResultCount: 10,
    sorting: ''
  });

  // Permissions
  const permissions = useAppSelector(state => state.session.auth.grantedPermissions);
  const sessionInfo = useAppSelector(state => state.session);
  const canCreate = permissions?.includes('Pages.RecordCategoryRules.Create');
  const canEdit = permissions?.includes('Pages.RecordCategoryRules.Edit');
  const canDelete = permissions?.includes('Pages.RecordCategoryRules.Delete');
  const canViewHistory = permissions?.includes('Pages.Administration.AuditLogs');
  const isHost = sessionInfo.tenant?.id === undefined; // Host user if no tenant

  // ============================================================================
  // DATA FETCHING
  // ============================================================================

  const fetchData = useCallback(async () => {
    setLoading(true);
    try {
      const result = await recordCategoryRulesService.getAll(filters);
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
    navigate('/app/compliance/record-category-rules/create');
  };

  const handleEdit = (id: string) => {
    navigate(`/app/compliance/record-category-rules/edit/${id}`);
  };

  const handleView = (id: string) => {
    navigate(`/app/compliance/record-category-rules/view/${id}`);
  };

  const handleDelete = (record: GetRecordCategoryRuleForViewDto) => {
    Modal.confirm({
      title: L('AreYouSure'),
      content: L('RecordCategoryRuleDeleteWarningMessage', record.recordCategoryRule.name),
      okText: L('Yes'),
      cancelText: L('No'),
      okType: 'danger',
      onOk: async () => {
        try {
          await recordCategoryRulesService.delete({ id: record.recordCategoryRule.id });
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
      const fileDto = await recordCategoryRulesService.getRecordCategoryRulesToExcel({
        filter: filters.filter,
        nameFilter: filters.nameFilter,
        descriptionFilter: filters.descriptionFilter,
        notifyFilter: filters.notifyFilter,
        minExpireInDaysFilter: filters.minExpireInDaysFilter,
        maxExpireInDaysFilter: filters.maxExpireInDaysFilter,
        minWarnDaysBeforeFirstFilter: filters.minWarnDaysBeforeFirstFilter,
        maxWarnDaysBeforeFirstFilter: filters.maxWarnDaysBeforeFirstFilter,
        expiresFilter: filters.expiresFilter,
        requiredFilter: filters.requiredFilter,
        isSurpathOnlyFilter: filters.isSurpathOnlyFilter,
        minWarnDaysBeforeSecondFilter: filters.minWarnDaysBeforeSecondFilter,
        maxWarnDaysBeforeSecondFilter: filters.maxWarnDaysBeforeSecondFilter,
        minWarnDaysBeforeFinalFilter: filters.minWarnDaysBeforeFinalFilter,
        maxWarnDaysBeforeFinalFilter: filters.maxWarnDaysBeforeFinalFilter,
        metaDataFilter: filters.metaDataFilter
      });

      // Trigger download
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
      descriptionFilter: '',
      notifyFilter: undefined,
      minExpireInDaysFilter: undefined,
      maxExpireInDaysFilter: undefined,
      minWarnDaysBeforeFirstFilter: undefined,
      maxWarnDaysBeforeFirstFilter: undefined,
      expiresFilter: undefined,
      requiredFilter: undefined,
      isSurpathOnlyFilter: undefined,
      minWarnDaysBeforeSecondFilter: undefined,
      maxWarnDaysBeforeSecondFilter: undefined,
      minWarnDaysBeforeFinalFilter: undefined,
      maxWarnDaysBeforeFinalFilter: undefined,
      metaDataFilter: '',
      skipCount: 0,
      maxResultCount: filters.maxResultCount,
      sorting: filters.sorting
    });
  };

  // ============================================================================
  // BOOLEAN FILTER OPTIONS
  // ============================================================================
  const booleanFilterOptions = [
    { value: undefined, label: L('All') },
    { value: 1, label: L('Yes') },
    { value: 0, label: L('No') }
  ];

  // ============================================================================
  // TABLE CONFIGURATION
  // ============================================================================

  const columns: ColumnsType<GetRecordCategoryRuleForViewDto> = [
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
            onClick={() => handleView(record.recordCategoryRule.id)}
            title={L('View')}
          />
          {canEdit && (
            <Button
              type="link"
              icon={<EditOutlined />}
              onClick={() => handleEdit(record.recordCategoryRule.id)}
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
      dataIndex: ['recordCategoryRule', 'name'],
      key: 'name',
      sorter: true,
      width: 200
    },
    {
      title: L('Description'),
      dataIndex: ['recordCategoryRule', 'description'],
      key: 'description',
      width: 250,
      ellipsis: true
    },
    {
      title: L('Notify'),
      dataIndex: ['recordCategoryRule', 'notify'],
      key: 'notify',
      width: 100,
      align: 'center',
      render: (value: boolean) =>
        value ? (
          <CheckCircleOutlined style={{ color: '#52c41a', fontSize: 16 }} title={L('True')} />
        ) : (
          <CloseCircleOutlined style={{ color: '#ff4d4f', fontSize: 16 }} title={L('False')} />
        )
    },
    {
      title: L('ExpireInDays'),
      dataIndex: ['recordCategoryRule', 'expireInDays'],
      key: 'expireInDays',
      width: 120,
      align: 'center'
    },
    {
      title: L('WarnDaysBeforeFirst'),
      dataIndex: ['recordCategoryRule', 'warnDaysBeforeFirst'],
      key: 'warnDaysBeforeFirst',
      width: 150,
      align: 'center'
    },
    {
      title: L('Expires'),
      dataIndex: ['recordCategoryRule', 'expires'],
      key: 'expires',
      width: 100,
      align: 'center',
      render: (value: boolean) =>
        value ? (
          <CheckCircleOutlined style={{ color: '#52c41a', fontSize: 16 }} title={L('True')} />
        ) : (
          <CloseCircleOutlined style={{ color: '#ff4d4f', fontSize: 16 }} title={L('False')} />
        )
    },
    {
      title: L('Required'),
      dataIndex: ['recordCategoryRule', 'required'],
      key: 'required',
      width: 100,
      align: 'center',
      render: (value: boolean) =>
        value ? (
          <CheckCircleOutlined style={{ color: '#52c41a', fontSize: 16 }} title={L('True')} />
        ) : (
          <CloseCircleOutlined style={{ color: '#ff4d4f', fontSize: 16 }} title={L('False')} />
        )
    },
    ...(isHost ? [{
      title: L('IsSurpathOnly'),
      dataIndex: ['recordCategoryRule', 'isSurpathOnly'],
      key: 'isSurpathOnly',
      width: 130,
      align: 'center' as const,
      render: (value: boolean) =>
        value ? (
          <CheckCircleOutlined style={{ color: '#52c41a', fontSize: 16 }} title={L('True')} />
        ) : (
          <CloseCircleOutlined style={{ color: '#ff4d4f', fontSize: 16 }} title={L('False')} />
        )
    }] : []),
    {
      title: L('WarnDaysBeforeSecond'),
      dataIndex: ['recordCategoryRule', 'warnDaysBeforeSecond'],
      key: 'warnDaysBeforeSecond',
      width: 150,
      align: 'center'
    },
    {
      title: L('WarnDaysBeforeFinal'),
      dataIndex: ['recordCategoryRule', 'warnDaysBeforeFinal'],
      key: 'warnDaysBeforeFinal',
      width: 150,
      align: 'center'
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
          <h2>{L('RecordCategoryRules')}</h2>
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
                placeholder={L('Description')}
                value={filters.descriptionFilter}
                onChange={(e) => setFilters({ ...filters, descriptionFilter: e.target.value, skipCount: 0 })}
                onPressEnter={fetchData}
                allowClear
              />
            </Col>
            <Col span={8}>
              <Select
                placeholder={L('Notify')}
                value={filters.notifyFilter}
                onChange={(value) => setFilters({ ...filters, notifyFilter: value, skipCount: 0 })}
                style={{ width: '100%' }}
                options={booleanFilterOptions}
                allowClear
              />
            </Col>
            <Col span={8}>
              <Select
                placeholder={L('Expires')}
                value={filters.expiresFilter}
                onChange={(value) => setFilters({ ...filters, expiresFilter: value, skipCount: 0 })}
                style={{ width: '100%' }}
                options={booleanFilterOptions}
                allowClear
              />
            </Col>
            <Col span={8}>
              <Select
                placeholder={L('Required')}
                value={filters.requiredFilter}
                onChange={(value) => setFilters({ ...filters, requiredFilter: value, skipCount: 0 })}
                style={{ width: '100%' }}
                options={booleanFilterOptions}
                allowClear
              />
            </Col>
            {isHost && (
              <Col span={8}>
                <Select
                  placeholder={L('IsSurpathOnly')}
                  value={filters.isSurpathOnlyFilter}
                  onChange={(value) => setFilters({ ...filters, isSurpathOnlyFilter: value, skipCount: 0 })}
                  style={{ width: '100%' }}
                  options={booleanFilterOptions}
                  allowClear
                />
              </Col>
            )}
            <Col span={12}>
              <Space.Compact style={{ width: '100%' }}>
                <InputNumber
                  placeholder={L('MinExpireInDays')}
                  value={filters.minExpireInDaysFilter}
                  onChange={(value) => setFilters({ ...filters, minExpireInDaysFilter: value || undefined, skipCount: 0 })}
                  style={{ width: '50%' }}
                />
                <InputNumber
                  placeholder={L('MaxExpireInDays')}
                  value={filters.maxExpireInDaysFilter}
                  onChange={(value) => setFilters({ ...filters, maxExpireInDaysFilter: value || undefined, skipCount: 0 })}
                  style={{ width: '50%' }}
                />
              </Space.Compact>
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
        rowKey={(record) => record.recordCategoryRule.id}
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
          pageSizeOptions: ['5', '10', '25', '50', '100', '250', '500']
        }}
        scroll={{ x: 1600 }}
      />
    </Card>
  );
};

export default RecordCategoryRulesPage;
