(function ($) {
    app.modals.TenantDepartmentLookupTableModal = function () {
        var _modalManager;

        var _tenantSurpathServicesService = abp.services.app.tenantSurpathServices;
        var _$tenantDepartmentTable = $('#TenantDepartmentTable');
        var _modalData;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _modalData = _modalManager.getArgs();
            console.log('tenantSurpathServiceTenantDepartmentLookupTableModal', _modalData);
        };

        var dataTable = _$tenantDepartmentTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: abp.services.app.commonLookup.getAllTenantDepartmentForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#TenantDepartmentTableFilter').val(),
                        tenantId: GetTenantIdFilter()
                    };
                },
            },
            columnDefs: [
                {
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent:
                        "<div class=\"text-center\"><input id='selectbtn' class='btn btn-success' type='button' width='25px' value='" +
                        app.localize('Select') +
                        "' /></div>",
                },
                {
                    autoWidth: false,
                    orderable: false,
                    targets: 1,
                    data: 'displayName',
                },
                {
                    targets: 2,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    render: function (data, row, full) {
                        return data.tenantInfoDto.tenancyName;
                    },
                    visible: function () {
                        return abp.session.multiTenancySide == abp.multiTenancy.sides.HOST;
                    }
                }
            ],
        });

        $('#TenantDepartmentTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getTenantDepartment() {
            dataTable.ajax.reload();
        }

        $('#GetTenantDepartmentButton').click(function (e) {
            e.preventDefault();
            getTenantDepartment();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $('#TenantDepartmentTableFilter').keypress(function (e) {
            if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
                getTenantDepartment();
            }
        });

        function GetTenantIdFilter() {
            // First check if tenantId is passed in modal data
            if (_modalData && _modalData.tenantId) {
                console.log('Using tenantId from modal data:', _modalData.tenantId);
                return _modalData.tenantId;
            }
            
            // Fall back to looking in the form
            var tenantId = $('#TenantDepartmentTableFilter').closest('div.form').find('#tenantId').val();
            if (tenantId == undefined || tenantId == null || tenantId == '') {
                console.log('GetTenantIdFilter not found', _modalData);
                return;
            }
            console.log('GetTenantIdFilter from form', tenantId);

            return tenantId;
        }
    };
})(jQuery);
