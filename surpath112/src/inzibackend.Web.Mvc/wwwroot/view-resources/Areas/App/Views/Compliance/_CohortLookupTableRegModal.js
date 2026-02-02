(function ($) {
    app.modals.CohortLookupTableRegModal = function () {
        var _modalManager;
        var _complianceService = abp.services.app.surpathCompliance;
        var _tableName = 'CohortRegTable';
        var _$tenantDepartmentTable = $('#' + _tableName);
        var _shuffle = $('#confirm').val();
        var _excludeCohortId = $('#ExcludeCohortId').val();

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };



        var dataTable = _$tenantDepartmentTable.DataTable({
            paging: true,
            lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
            pageLength: 5000,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _complianceService.getAllCohortForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#CohortTableFilter').val(),
                        shuffle: _shuffle,
                        tenantDepartmentId: $('#TenantDepartmentId').val(),
                        tenantId: $('#TenantId').val(),
                        excludeCohortId: _excludeCohortId,
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


        $('#' + _tableName + ' tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getTenantDepartment() {
            dataTable.ajax.reload();
        }

        $('#GetCohortButton').click(function (e) {
            e.preventDefault();
            getTenantDepartment();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
                getTenantDepartment();
            }
        });
    };
})(jQuery);
