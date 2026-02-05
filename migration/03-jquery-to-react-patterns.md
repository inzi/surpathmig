# jQuery → React Conversion Patterns

## Overview

This guide covers the core patterns for converting jQuery-based ASPNetZero MVC code to React. Focus on understanding the pattern transformation rather than line-by-line translation.

---

## Core Pattern Transformations

### 1. DOM Manipulation → React State

#### jQuery Pattern
```javascript
// Direct DOM manipulation
$('#userName').text(data.name);
$('#userEmail').val(data.email);
$('#loadingSpinner').show();
$('#loadingSpinner').hide();
$('#errorMessage').addClass('visible');
```

#### React Pattern
```tsx
const [user, setUser] = useState<User | null>(null);
const [loading, setLoading] = useState(false);
const [error, setError] = useState<string | null>(null);

return (
    <>
        {loading && <Spinner />}
        {error && <div className="error visible">{error}</div>}
        {user && (
            <>
                <span id="userName">{user.name}</span>
                <input id="userEmail" value={user.email} />
            </>
        )}
    </>
);
```

---

### 2. AJAX Calls → React Query / Async Functions

#### jQuery Pattern (ASPNetZero)
```javascript
// Using abp.services proxy
abp.services.app.surpathModule.getAll({
    filter: filter,
    skipCount: skipCount,
    maxResultCount: maxResultCount
}).done(function(result) {
    // Handle success
    refreshTable(result.items);
}).fail(function(error) {
    // Handle error
    abp.message.error(error.message);
});

// Or using jQuery AJAX directly
$.ajax({
    url: '/api/services/app/SurpathModule/GetAll',
    type: 'GET',
    data: { filter: filter },
    success: function(result) { ... },
    error: function(xhr) { ... }
});
```

#### React Pattern
```tsx
// Using async/await with service proxy
import { surpathModuleService } from '@services/surpathModule';

const SurpathModuleList: React.FC = () => {
    const [items, setItems] = useState<SurpathItem[]>([]);
    const [loading, setLoading] = useState(false);

    const fetchData = async (filter: string) => {
        setLoading(true);
        try {
            const result = await surpathModuleService.getAll({
                filter,
                skipCount: 0,
                maxResultCount: 10
            });
            setItems(result.items);
        } catch (error) {
            message.error(error.message);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchData('');
    }, []);

    return <Table dataSource={items} loading={loading} />;
};
```

#### React Pattern with React Query (Preferred)
```tsx
import { useQuery, useMutation, useQueryClient } from 'react-query';

const SurpathModuleList: React.FC = () => {
    const [filter, setFilter] = useState('');
    
    const { data, isLoading, error } = useQuery(
        ['surpathModules', filter],
        () => surpathModuleService.getAll({ filter, skipCount: 0, maxResultCount: 10 })
    );

    return (
        <Table
            dataSource={data?.items}
            loading={isLoading}
        />
    );
};
```

---

### 3. Event Handlers → React Events

#### jQuery Pattern
```javascript
// Button click
$('#createButton').click(function() {
    openCreateModal();
});

// Form submit
$('#myForm').submit(function(e) {
    e.preventDefault();
    var formData = $(this).serializeArray();
    saveData(formData);
});

// Input change
$('#filterInput').on('keyup', function() {
    var value = $(this).val();
    filterTable(value);
});

// Delegated events (for dynamic elements)
$('#dataTable').on('click', '.edit-btn', function() {
    var id = $(this).data('id');
    editRecord(id);
});
```

#### React Pattern
```tsx
const SurpathModule: React.FC = () => {
    const [filter, setFilter] = useState('');
    const [formData, setFormData] = useState<FormData>({});

    // Button click
    const handleCreate = () => {
        setModalVisible(true);
    };

    // Form submit
    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        saveData(formData);
    };

    // Input change (with debounce for performance)
    const handleFilterChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setFilter(e.target.value);
    };

    // Edit action (no delegation needed - pass handler to child)
    const handleEdit = (id: number) => {
        editRecord(id);
    };

    return (
        <>
            <Button onClick={handleCreate}>Create</Button>
            <Input onChange={handleFilterChange} />
            <form onSubmit={handleSubmit}>...</form>
            <Table
                dataSource={items}
                columns={[
                    // ...
                    {
                        title: 'Actions',
                        render: (record) => (
                            <Button onClick={() => handleEdit(record.id)}>Edit</Button>
                        )
                    }
                ]}
            />
        </>
    );
};
```

---

### 4. jQuery Plugins → React Components

#### jQuery DataTable → React Table Component
See `04-datatable-conversion-guide.md`

#### jQuery Modal → React Modal Component
See `05-modal-conversion-guide.md`

#### jQuery DatePicker
```javascript
// jQuery
$('#dateInput').datepicker({
    format: 'yyyy-mm-dd',
    autoclose: true
});
```

```tsx
// React with Ant Design
import { DatePicker } from 'antd';

<DatePicker
    format="YYYY-MM-DD"
    onChange={(date) => setSelectedDate(date)}
/>
```

#### jQuery Select2 → React Select
```javascript
// jQuery
$('#userSelect').select2({
    ajax: {
        url: '/api/users/search',
        dataType: 'json',
        processResults: function(data) {
            return { results: data.items };
        }
    }
});
```

```tsx
// React with Ant Design Select
import { Select } from 'antd';

const [users, setUsers] = useState([]);
const [searching, setSearching] = useState(false);

const handleSearch = async (value: string) => {
    setSearching(true);
    const result = await userService.search(value);
    setUsers(result.items);
    setSearching(false);
};

<Select
    showSearch
    onSearch={handleSearch}
    loading={searching}
    filterOption={false}
>
    {users.map(user => (
        <Select.Option key={user.id} value={user.id}>
            {user.name}
        </Select.Option>
    ))}
</Select>
```

---

### 5. Global State / Shared Data

#### jQuery Pattern (Global Variables)
```javascript
// Often stored in window or global scope
var currentUser = null;
var selectedTenant = null;

// Accessed anywhere
function doSomething() {
    if (currentUser) {
        // ...
    }
}
```

#### React Pattern (Context or State Management)
```tsx
// Using React Context
const AppContext = createContext<AppState | null>(null);

const AppProvider: React.FC = ({ children }) => {
    const [currentUser, setCurrentUser] = useState<User | null>(null);
    const [selectedTenant, setSelectedTenant] = useState<Tenant | null>(null);

    return (
        <AppContext.Provider value={{ currentUser, selectedTenant }}>
            {children}
        </AppContext.Provider>
    );
};

// Using in components
const MyComponent: React.FC = () => {
    const { currentUser } = useContext(AppContext);
    // ...
};
```

---

### 6. Conditional Rendering

#### jQuery Pattern
```javascript
// Show/hide based on condition
if (hasPermission) {
    $('#adminPanel').show();
} else {
    $('#adminPanel').hide();
}

// Add/remove classes
if (isActive) {
    $('#item').addClass('active');
} else {
    $('#item').removeClass('active');
}

// Dynamic content
if (data.items.length > 0) {
    var html = '';
    data.items.forEach(function(item) {
        html += '<li>' + item.name + '</li>';
    });
    $('#list').html(html);
} else {
    $('#list').html('<li>No items found</li>');
}
```

#### React Pattern
```tsx
const MyComponent: React.FC = () => {
    return (
        <>
            {/* Conditional display */}
            {hasPermission && <AdminPanel />}

            {/* Conditional classes */}
            <div className={`item ${isActive ? 'active' : ''}`}>...</div>

            {/* Or with classnames library */}
            <div className={classNames('item', { active: isActive })}>...</div>

            {/* List rendering */}
            <ul>
                {items.length > 0 ? (
                    items.map(item => <li key={item.id}>{item.name}</li>)
                ) : (
                    <li>No items found</li>
                )}
            </ul>
        </>
    );
};
```

---

### 7. Form Handling

#### jQuery Pattern
```javascript
// Get form values
var name = $('#nameInput').val();
var email = $('#emailInput').val();

// Validate
if (!name) {
    abp.message.warn('Name is required');
    return;
}

// Submit
var formData = {
    name: name,
    email: email
};
saveRecord(formData);

// Reset form
$('#myForm')[0].reset();
```

#### React Pattern (with Ant Design Form)
```tsx
const [form] = Form.useForm();

const handleSubmit = async (values: FormValues) => {
    try {
        await saveRecord(values);
        message.success(L('SavedSuccessfully'));
        form.resetFields();
    } catch (error) {
        message.error(error.message);
    }
};

return (
    <Form form={form} onFinish={handleSubmit}>
        <Form.Item
            name="name"
            rules={[{ required: true, message: L('NameRequired') }]}
        >
            <Input />
        </Form.Item>
        <Form.Item
            name="email"
            rules={[{ type: 'email', message: L('InvalidEmail') }]}
        >
            <Input />
        </Form.Item>
        <Button type="primary" htmlType="submit">
            {L('Save')}
        </Button>
    </Form>
);
```

---

## ASPNetZero-Specific Patterns

### ABP Message → Ant Design Message/Modal
```javascript
// jQuery
abp.message.info('Info message');
abp.message.success('Success!');
abp.message.warn('Warning!');
abp.message.error('Error!');
abp.message.confirm('Are you sure?', 'Delete', function(result) {
    if (result) { deleteRecord(); }
});
```

```tsx
// React
import { message, Modal } from 'antd';

message.info('Info message');
message.success('Success!');
message.warning('Warning!');
message.error('Error!');

Modal.confirm({
    title: 'Delete',
    content: 'Are you sure?',
    onOk: () => deleteRecord()
});
```

### ABP Notify → Ant Design Notification
```javascript
// jQuery
abp.notify.info('Info');
abp.notify.success('Saved!');
abp.notify.warn('Warning');
abp.notify.error('Failed');
```

```tsx
// React
import { notification } from 'antd';

notification.info({ message: 'Info' });
notification.success({ message: 'Saved!' });
notification.warning({ message: 'Warning' });
notification.error({ message: 'Failed' });
```

---

## Migration Checklist Per Component

- [ ] Identify all jQuery selectors and DOM manipulations
- [ ] Convert to React state variables
- [ ] Identify all AJAX calls
- [ ] Convert to async functions or React Query
- [ ] Identify all event handlers
- [ ] Convert to React event handlers
- [ ] Identify jQuery plugins used
- [ ] Find React equivalents or create custom components
- [ ] Identify global variables
- [ ] Convert to Context or props
- [ ] Test all functionality