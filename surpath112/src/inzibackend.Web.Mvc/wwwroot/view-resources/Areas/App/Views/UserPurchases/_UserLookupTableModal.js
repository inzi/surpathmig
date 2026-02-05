(function ($) {
    app.modals.UserLookupTableModal = function () {
        var _modalManager;
        var _userPurchasesService = abp.services.app.userPurchases;
        var _$userTable = $('#UserTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };

        var dataTable = _$userTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _userPurchasesService.getAllUserForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#UserTableFilter').val()
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

        $('#UserTable tbody').on('click', 'tr', function () {
            var data = dataTable.row(this).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getUsers() {
            dataTable.ajax.reload();
        }

        $('#GetUserButton').click(function (e) {
            e.preventDefault();
            getUsers();
        });

        $('#UserTableFilter').keypress(function (e) {
            if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
                getUsers();
            }
        });
    };
})(jQuery);