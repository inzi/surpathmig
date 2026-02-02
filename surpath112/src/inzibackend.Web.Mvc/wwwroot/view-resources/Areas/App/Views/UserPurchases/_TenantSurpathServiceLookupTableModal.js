(function ($) {
    app.modals.TenantSurpathServiceLookupTableModal = function () {
        var _modalManager;
        var _userPurchasesService = abp.services.app.userPurchases;
        var _$tenantSurpathServiceTable = $('#TenantSurpathServiceTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };

        var dataTable = _$tenantSurpathServiceTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _userPurchasesService.getAllTenantSurpathServiceForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#TenantSurpathServiceTableFilter').val()
                    };
                }
            },
            columnDefs: [
                {
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: "<div class=\"text-center\"><button class=\"btn btn-success btn-sm\" type=\"button\" title=\"" + app.localize('Select') + "\"><i class=\"fa fa-check\"></i></button></div>"
                },
                {
                    autoWidth: false,
                    orderable: false,
                    targets: 1,
                    data: "displayName"
                }
            ]
        });

        $('#TenantSurpathServiceTable tbody').on('click', 'tr', function () {
            var data = dataTable.row(this).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getTenantSurpathServices() {
            dataTable.ajax.reload();
        }

        $('#GetTenantSurpathServiceButton').click(function (e) {
            e.preventDefault();
            getTenantSurpathServices();
        });

        $('#TenantSurpathServiceTableFilter').keypress(function (e) {
            if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
                getTenantSurpathServices();
            }
        });
    };
})(jQuery);