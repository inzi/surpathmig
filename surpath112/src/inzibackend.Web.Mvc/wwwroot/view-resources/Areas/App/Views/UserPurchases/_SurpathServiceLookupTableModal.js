(function ($) {
    app.modals.SurpathServiceLookupTableModal = function () {
        var _modalManager;
        var _userPurchasesService = abp.services.app.userPurchases;
        var _$surpathServiceTable = $('#SurpathServiceTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };

        var dataTable = _$surpathServiceTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _userPurchasesService.getAllSurpathServiceForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#SurpathServiceTableFilter').val()
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

        $('#SurpathServiceTable tbody').on('click', 'tr', function () {
            var data = dataTable.row(this).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getSurpathServices() {
            dataTable.ajax.reload();
        }

        $('#GetSurpathServiceButton').click(function (e) {
            e.preventDefault();
            getSurpathServices();
        });

        $('#SurpathServiceTableFilter').keypress(function (e) {
            if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
                getSurpathServices();
            }
        });
    };
})(jQuery);