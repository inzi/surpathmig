import React, { useState, useCallback } from "react";
import { Table, Dropdown, App, Input, Button, Space } from "antd";
import type { MenuProps } from "antd";
import { SearchOutlined, FileExcelOutlined, PlusOutlined } from "@ant-design/icons";
import { usePermissions } from "../../../hooks/usePermissions";
import CreateOrEditCodeTypeModal from "./components/CreateOrEditCodeTypeModal";
import {
  CodeTypesServiceProxy,
  GetCodeTypeForViewDto,
  GetAllCodeTypesInput,
} from "./codeTypes.service";
import PageHeader from "../components/common/PageHeader";
import { useServiceProxy } from "@/api/service-proxy-factory";
import L from "@/lib/L";
import { useTheme } from "@/hooks/useTheme";
import type { TablePaginationConfig } from "antd/es/table";
import { downloadTempFile } from "@/lib/file-download-helper";

const CodeTypesPage: React.FC = () => {
  const { modal, message } = App.useApp();
  const { isGranted } = usePermissions();
  const codeTypesService = useServiceProxy(CodeTypesServiceProxy, []);
  const { containerClass } = useTheme();

  const [loading, setLoading] = useState(false);
  const [data, setData] = useState<GetCodeTypeForViewDto[]>([]);
  const [totalCount, setTotalCount] = useState(0);

  const [isCreateOrEditModalVisible, setCreateOrEditModalVisible] =
    useState(false);
  const [editingCodeTypeId, setEditingCodeTypeId] = useState<
    number | undefined
  >();

  // Filters
  const [filter, setFilter] = useState("");
  const [nameFilter, setNameFilter] = useState("");
  const [showAdvancedFilters, setShowAdvancedFilters] = useState(false);

  // Pagination
  const [pagination, setPagination] = useState<TablePaginationConfig>({
    current: 1,
    pageSize: 10,
    total: 0,
  });

  const fetchCodeTypes = useCallback(
    async (
      page: number = pagination.current || 1,
      pageSize: number = pagination.pageSize || 10
    ) => {
      setLoading(true);
      try {
        const input: GetAllCodeTypesInput = {
          filter,
          nameFilter,
          skipCount: (page - 1) * pageSize,
          maxResultCount: pageSize,
        };

        const result = await codeTypesService.getAll(input);
        setData(result.items ?? []);
        setTotalCount(result.totalCount ?? 0);
        setPagination({
          ...pagination,
          current: page,
          pageSize,
          total: result.totalCount ?? 0,
        });
      } catch (error) {
        console.error("Error fetching code types:", error);
        message.error(L("ErrorOccurred"));
      } finally {
        setLoading(false);
      }
    },
    [codeTypesService, filter, nameFilter, pagination, message]
  );

  const handleTableChange = (newPagination: TablePaginationConfig) => {
    fetchCodeTypes(newPagination.current, newPagination.pageSize);
  };

  const handleSearch = () => {
    fetchCodeTypes(1, pagination.pageSize);
  };

  const handleRefresh = () => {
    fetchCodeTypes(pagination.current, pagination.pageSize);
  };

  const deleteCodeType = (record: GetCodeTypeForViewDto) => {
    modal.confirm({
      title: L("AreYouSure"),
      content: L("CodeTypeDeleteWarningMessage", record.codeType.name),
      onOk: async () => {
        try {
          await codeTypesService.delete(record.codeType.id);
          message.success(L("SuccessfullyDeleted"));
          handleRefresh();
        } catch (error) {
          console.error("Error deleting code type:", error);
          message.error(L("ErrorOccurred"));
        }
      },
    });
  };

  const handleExportToExcel = async () => {
    try {
      const result = await codeTypesService.getCodeTypesToExcel({
        filter,
        nameFilter,
      });
      downloadTempFile(result);
    } catch (error) {
      console.error("Error exporting to excel:", error);
      message.error(L("ErrorOccurred"));
    }
  };

  const getMenuItems = (record: GetCodeTypeForViewDto): MenuProps => {
    const items: MenuProps["items"] = [];

    if (isGranted("Pages.Administration.CodeTypes.Edit")) {
      items.push({
        key: "edit",
        label: L("Edit"),
        icon: <i className="far fa-edit" />,
        onClick: () => {
          setEditingCodeTypeId(record.codeType.id);
          setCreateOrEditModalVisible(true);
        },
      });
    }

    if (isGranted("Pages.Administration.CodeTypes.Delete")) {
      items.push({
        key: "delete",
        label: L("Delete"),
        icon: <i className="far fa-trash-alt" />,
        onClick: () => deleteCodeType(record),
        danger: true,
      });
    }

    return { items };
  };

  const columns = [
    {
      title: L("Actions"),
      width: 150,
      align: "center" as const,
      render: (_text: string, record: GetCodeTypeForViewDto) => {
        const menuItems = getMenuItems(record);
        if (!menuItems.items || menuItems.items.length === 0) {
          return null;
        }
        return (
          <Dropdown menu={menuItems} trigger={["click"]}>
            <button
              type="button"
              className="btn btn-primary btn-sm dropdown-toggle d-inline-flex align-items-center"
            >
              <i className="fa fa-cog me-1"></i>
              {L("Actions")}
            </button>
          </Dropdown>
        );
      },
    },
    {
      title: L("Name"),
      dataIndex: ["codeType", "name"],
      key: "name",
    },
  ];

  return (
    <>
      <PageHeader
        title={L("CodeTypes")}
        description={L("CodeTypesHeaderInfo")}
        actions={
          <Space>
            <Button
              icon={<FileExcelOutlined />}
              onClick={handleExportToExcel}
              className="btn-outline-success"
            >
              {L("ExportToExcel")}
            </Button>
            {isGranted("Pages.Administration.CodeTypes.Create") && (
              <Button
                type="primary"
                icon={<PlusOutlined />}
                onClick={() => {
                  setEditingCodeTypeId(undefined);
                  setCreateOrEditModalVisible(true);
                }}
              >
                {L("CreateNewCodeType")}
              </Button>
            )}
          </Space>
        }
      />

      <div className={containerClass}>
        <div className="card card-custom gutter-b">
          <div className="card-body">
            {/* Search Filters */}
            <div className="row align-items-center mb-4">
              <div className="col-xl-12">
                <div className="my-3">
                  <Input.Group compact>
                    <Input
                      placeholder={L("SearchWithThreeDot")}
                      value={filter}
                      onChange={(e) => setFilter(e.target.value)}
                      onPressEnter={handleSearch}
                      style={{ width: "calc(100% - 100px)" }}
                    />
                    <Button
                      type="primary"
                      icon={<SearchOutlined />}
                      onClick={handleSearch}
                    >
                      {L("Search")}
                    </Button>
                  </Input.Group>
                </div>
              </div>
            </div>

            {/* Advanced Filters */}
            {showAdvancedFilters && (
              <div className="row mb-4">
                <div className="col-md-3">
                  <div className="my-3">
                    <label className="form-label">{L("Name")}</label>
                    <Input
                      value={nameFilter}
                      onChange={(e) => setNameFilter(e.target.value)}
                      onPressEnter={handleSearch}
                    />
                  </div>
                </div>
              </div>
            )}

            {/* Advanced Filters Toggle */}
            <div className="row my-4">
              <div className="col-xl-12">
                <span
                  className="text-muted clickable-item"
                  onClick={() => setShowAdvancedFilters(!showAdvancedFilters)}
                  style={{ cursor: "pointer" }}
                >
                  <i
                    className={`fa fa-angle-${showAdvancedFilters ? "up" : "down"}`}
                  ></i>{" "}
                  {showAdvancedFilters
                    ? L("HideAdvancedFilters")
                    : L("ShowAdvancedFilters")}
                </span>
              </div>
            </div>

            {/* Table */}
            <Table
              dataSource={data}
              columns={columns}
              loading={loading}
              rowKey={(record) => record.codeType.id.toString()}
              pagination={pagination}
              onChange={handleTableChange}
            />
          </div>
        </div>
      </div>

      {/* Create/Edit Modal */}
      <CreateOrEditCodeTypeModal
        visible={isCreateOrEditModalVisible}
        onCancel={() => setCreateOrEditModalVisible(false)}
        onSave={() => {
          setCreateOrEditModalVisible(false);
          handleRefresh();
        }}
        codeTypeId={editingCodeTypeId}
      />
    </>
  );
};

export default CodeTypesPage;
