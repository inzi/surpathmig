(function ($) {
    app.modals.TenantDepartmentLookupTableModal = function () {
        var _modalManager;

        var _cohortsService = abp.services.app.cohorts;
        var _$tenantDepartmentTable = $('#TenantDepartmentTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };

        var dataTable = _$tenantDepartmentTable.DataTable({
            paging: false,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: abp.services.app.commonLookup.getAllTenantDepartmentForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#TenantDepartmentTableFilter').val(),
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
            //debugger;
        });

        $(document).keypress(function (e) {
            if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
                getTenantDepartment();
            }
        });
    };
})(jQuery);