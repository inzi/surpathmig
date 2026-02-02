# DataTable Conversion Guide: jQuery DataTables → React Tables

## Overview

jQuery DataTables is commonly used in ASPNetZero MVC projects. This guide covers converting to React table components (typically Ant Design Table in ASPNetZero React).

---

## jQuery DataTable Pattern (Surpath112)

### Basic DataTable Setup
```javascript
// In Views/SurpathModule/Index.js or Index.cshtml
var _$table = $('#SurpathModuleTable');

var dataTable = _$table.DataTable({
    paging: true,
    serverSide: true,
    processing: true,
    listAction: {
        ajaxFunction: _surpathModuleService.getAll,
        inputFilter: function() {
            return {
                filter: $('#FilterText').val(),
                statusFilter: $('#StatusFilter').val()
            };
        }
    },
    columnDefs: [
        {
            targets: 0,
            data: 'name',
            title: app.localize('Name')
        },
        {
            targets: 1,
            data: 'description',
            title: app.localize('Description')
        },
        {
            targets: 2,
            data: 'status',
            title: app.localize('Status'),
            render: function(data) {
                return getStatusBadge(data);
            }
        },
        {
            targets: 3,
            data: 'creationTime',
            title: app.localize('CreatedDate'),
            render: function(data) {
                return moment(data).format('L');
            }
        },
        {
            targets: 4,
            data: null,
            title: app.localize('Actions'),
            orderable: false,
            render: function(data, type, row) {
                var html = '<div class="btn-group">';
                if (_permissions.edit) {
                    html += '<button class="btn btn-sm btn-primary edit-btn" data-id="' + row.id + '">' +
                            app.localize('Edit') + '</button>';
                }
                if (_permissions.delete) {
                    html += '<button class="btn btn-sm btn-danger delete-btn" data-id="' + row.id + '">' +
                            app.localize('Delete') + '</button>';
                }
                html += '</div>';
                return html;
            }
        }
    ]
});

// Event handlers for action buttons (delegated)
_$table.on('click', '.edit-btn', function() {
    var id = $(this).data('id');
    editRecord(id);
});

_$table.on('click', '.delete-btn', function() {
    var id = $(this).data('id');
    deleteRecord(id);
});

// Refresh function
function refreshTable() {
    dataTable.ajax.reload();
}
```

---

## React Table Pattern (Surpath200)

### Using Ant Design Table

```tsx
// components/SurpathModule/SurpathModuleTable.tsx
import React, { useState, useEffect } from 'react';
import { Table, Button, Space, Input, Select, Tag, Modal, message } from 'antd';
import { ColumnsType } from 'antd/es/table';
import { usePermission } from '@hooks/usePermission';
import { L } from '@lib/abpUtility';
import { surpathModuleService } from '@services/surpathModule';
import moment from 'moment';

interface SurpathItem {
    id: number;
    name: string;
    description: string;
    status: string;
    creationTime: string;
}

interface TableParams {
    filter: string;
    statusFilter: string;
    skipCount: number;
    maxResultCount: number;
    sorting?: string;
}

const SurpathModuleTable: React.FC = () => {
    // Permissions
    const canEdit = usePermission('Pages.SurpathModule.Edit');
    const canDelete = usePermission('Pages.SurpathModule.Delete');

    // State
    const [data, setData] = useState<SurpathItem[]>([]);
    const [loading, setLoading] = useState(false);
    const [totalCount, setTotalCount] = useState(0);
    const [params, setParams] = useState<TableParams>({
        filter: '',
        statusFilter: '',
        skipCount: 0,
        maxResultCount: 10
    });

    // Fetch data
    const fetchData = async () => {
        setLoading(true);
        try {
            const result = await surpathModuleService.getAll(params);
            setData(result.items);
            setTotalCount(result.totalCount);
        } catch (error) {
            message.error(error.message);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchData();
    }, [params]);

    // Handlers
    const handleEdit = (record: SurpathItem) => {
        // Open edit modal or navigate
        console.log('Edit:', record.id);
    };

    const handleDelete = (record: SurpathItem) => {
        Modal.confirm({
            title: L('DeleteConfirmation'),
            content: L('DeleteWarningMessage'),
            onOk: async () => {
                try {
                    await surpathModuleService.delete({ id: record.id });
                    message.success(L('DeletedSuccessfully'));
                    fetchData(); // Refresh table
                } catch (error) {
                    message.error(error.message);
                }
            }
        });
    };

    const handleTableChange = (pagination: any, filters: any, sorter: any) => {
        setParams(prev => ({
            ...prev,
            skipCount: (pagination.current - 1) * pagination.pageSize,
            maxResultCount: pagination.pageSize,
            sorting: sorter.field ? `${sorter.field} ${sorter.order === 'descend' ? 'DESC' : 'ASC'}` : undefined
        }));
    };

    const handleFilterChange = (value: string) => {
        setParams(prev => ({
            ...prev,
            filter: value,
            skipCount: 0 // Reset to first page
        }));
    };

    // Status badge renderer
    const renderStatus = (status: string) => {
        const colors: Record<string, string> = {
            active: 'green',
            inactive: 'red',
            pending: 'orange'
        };
        return <Tag color={colors[status] || 'default'}>{status}</Tag>;
    };

    // Column definitions
    const columns: ColumnsType<SurpathItem> = [
        {
            title: L('Name'),
            dataIndex: 'name',
            key: 'name',
            sorter: true
        },
        {
            title: L('Description'),
            dataIndex: 'description',
            key: 'description'
        },
        {
            title: L('Status'),
            dataIndex: 'status',
            key: 'status',
            render: renderStatus
        },
        {
            title: L('CreatedDate'),
            dataIndex: 'creationTime',
            key: 'creationTime',
            render: (date: string) => moment(date).format('L'),
            sorter: true
        },
        {
            title: L('Actions'),
            key: 'actions',
            render: (_, record) => (
                <Space>
                    {canEdit && (
                        <Button
                            type="primary"
                            size="small"
                            onClick={() => handleEdit(record)}
                        >
                            {L('Edit')}
                        </Button>
                    )}
                    {canDelete && (
                        <Button
                            danger
                            size="small"
                            onClick={() => handleDelete(record)}
                        >
                            {L('Delete')}
                        </Button>
                    )}
                </Space>
            )
        }
    ];

    return (
        <div>
            {/* Filters */}
            <Space style={{ marginBottom: 16 }}>
                <Input.Search
                    placeholder={L('SearchPlaceholder')}
                    onSearch={handleFilterChange}
                    style={{ width: 250 }}
                />
                <Select
                    placeholder={L('Status')}
                    allowClear
                    style={{ width: 150 }}
                    onChange={(value) => setParams(prev => ({ ...prev, statusFilter: value || '', skipCount: 0 }))}
                >
                    <Select.Option value="active">{L('Active')}</Select.Option>
                    <Select.Option value="inactive">{L('Inactive')}</Select.Option>
                    <Select.Option value="pending">{L('Pending')}</Select.Option>
                </Select>
            </Space>

            {/* Table */}
            <Table
                columns={columns}
                dataSource={data}
                rowKey="id"
                loading={loading}
                onChange={handleTableChange}
                pagination={{
                    current: Math.floor(params.skipCount / params.maxResultCount) + 1,
                    pageSize: params.maxResultCount,
                    total: totalCount,
                    showSizeChanger: true,
                    showTotal: (total) => L('TotalRecords', total)
                }}
            />
        </div>
    );
};

export default SurpathModuleTable;
```

---

## Feature Mapping

| jQuery DataTable Feature | React Ant Design Table Equivalent |
|--------------------------|----------------------------------|
| `serverSide: true` | Manual pagination with API calls |
| `listAction.ajaxFunction` | `useEffect` + service call |
| `listAction.inputFilter` | State-based filter params |
| `columnDefs[].render` | Column `render` function |
| `paging: true` | `pagination` prop |
| `processing: true` | `loading` prop |
| Delegated click events | Inline `onClick` handlers |
| `dataTable.ajax.reload()` | Call `fetchData()` again |
| Column sorting | `sorter: true` + `onChange` handler |

---

## Advanced Patterns

### Row Selection
```javascript
// jQuery
_$table.DataTable({
    select: {
        style: 'multi'
    }
});

var selectedRows = dataTable.rows({ selected: true }).data();
```

```tsx
// React
const [selectedRowKeys, setSelectedRowKeys] = useState<number[]>([]);

const rowSelection = {
    selectedRowKeys,
    onChange: (keys: number[]) => setSelectedRowKeys(keys)
};

<Table rowSelection={rowSelection} ... />

// Access selected data
const selectedData = data.filter(item => selectedRowKeys.includes(item.id));
```

### Custom Row Styling
```javascript
// jQuery
createdRow: function(row, data) {
    if (data.status === 'inactive') {
        $(row).addClass('inactive-row');
    }
}
```

```tsx
// React
<Table
    rowClassName={(record) => record.status === 'inactive' ? 'inactive-row' : ''}
    ...
/>
```

### Expandable Rows
```javascript
// jQuery (usually custom implementation)
```

```tsx
// React
<Table
    expandable={{
        expandedRowRender: (record) => (
            <p>{record.details}</p>
        )
    }}
    ...
/>
```

### Export to Excel/CSV
```javascript
// jQuery with DataTables Buttons
buttons: ['copy', 'excel', 'csv', 'pdf']
```

```tsx
// React - typically custom implementation or library
import { exportToExcel } from '@utils/export';

<Button onClick={() => exportToExcel(data, 'surpath-module')}>
    {L('ExportToExcel')}
</Button>
```

---

## Extracting Business Logic

When migrating, identify custom logic in DataTables:

### 1. Custom Renderers
Look for `render` functions with business logic:
```javascript
render: function(data, type, row) {
    // Extract this logic to a separate function
    if (row.someCondition) {
        return calculateSomething(data);
    }
    return formatData(data);
}
```

### 2. Row Actions with Complex Logic
```javascript
_$table.on('click', '.action-btn', function() {
    var row = dataTable.row($(this).closest('tr')).data();
    // Complex logic here - extract to handler function
    if (row.status === 'pending') {
        handlePendingAction(row);
    } else {
        handleDefaultAction(row);
    }
});
```

### 3. Filter Logic
```javascript
inputFilter: function() {
    // Complex filter building - extract to separate function
    var filter = {};
    if ($('#dateFrom').val()) {
        filter.startDate = $('#dateFrom').val();
    }
    // ... more complex filter logic
    return filter;
}
```

---

## Migration Checklist

- [ ] Identify all DataTables in Surpath112
- [ ] Document column definitions for each
- [ ] Document custom render functions
- [ ] Document row click handlers
- [ ] Document filter inputs and logic
- [ ] Document export functionality
- [ ] Create React table component for each
- [ ] Implement server-side pagination
- [ ] Implement sorting
- [ ] Implement filtering
- [ ] Implement action buttons with permissions
- [ ] Test pagination with large datasets
- [ ] Test all action buttons
- [ ] Verify data display matches original