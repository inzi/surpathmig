(function ($) {
    app.modals.CohortLookupTableModal = function () {
        var _modalManager;
        var _userPurchasesService = abp.services.app.userPurchases;
        var _$cohortTable = $('#CohortTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };

        var dataTable = _$cohortTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _userPurchasesService.getAllCohortForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#CohortTableFilter').val()
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

        $('#CohortTable tbody').on('click', 'tr', function () {
            var data = dataTable.row(this).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getCohorts() {
            dataTable.ajax.reload();
        }

        $('#GetCohortButton').click(function (e) {
            e.preventDefault();
            getCohorts();
        });

        $('#CohortTableFilter').keypress(function (e) {
            if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
                getCohorts();
            }
        });
    };
})(jQuery);