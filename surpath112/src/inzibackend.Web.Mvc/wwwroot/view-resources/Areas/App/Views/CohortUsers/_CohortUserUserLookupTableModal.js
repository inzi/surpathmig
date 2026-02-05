(function ($) {
    // cuultm
    app.modals.UserLookupTableModal = function () {
        var _modalManager;

        var _cohortUsersService = abp.services.app.cohortUsers;
        var _$userTable = $('#UserTable');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.CohortUsers.Create'),
            edit: abp.auth.hasPermission('Pages.CohortUsers.Edit'),
            delete: abp.auth.hasPermission('Pages.CohortUsers.Delete'),
            view: abp.auth.hasPermission('Pages.CohortUsers'),
            isHost: abp.session.multiTenancySide == abp.multiTenancy.sides.HOST,
            "App.Surpath.DrugTest.Download": abp.auth.hasPermission('Surpath.DrugScreenDownload'),
            "App.Surpath.DrugTest.ViewStatus": abp.auth.hasPermission('Surpath.DrugScreenSeeStatus'),
            "App.Surpath.DrugTest.ChangeStatus": abp.auth.hasPermission('Surpath.DrugScreenChangeStatus'),
            "App.Surpath.BackgroundCheck.Download": abp.auth.hasPermission('Surpath.BackgroundCheckDownload'),
            "App.Surpath.BackgroundCheck.ViewStatus": abp.auth.hasPermission('Surpath.BackgroundCheckSeeStatus'),
            "App.Surpath.BackgroundCheck.ChangeStatus": abp.auth.hasPermission('Surpath.BackgroundCheckChangeStatus'),
        };

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };

        var dataTable = _$userTable.DataTable({
            paging: true,
            lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
            pageLength: 5000,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _cohortUsersService.getAllUserForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#UserTableFilter').val(),
                        tenantId: app.session.tenant.id
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
                    orderable: true,
                    targets: 1,
                    data: 'displayName',
                },
                {
                    autoWidth: false,
                    orderable: true,
                    targets: 2,
                    data: 'surname',
                },
                {
                    autoWidth: false,
                    orderable: true,
                    targets: 3,
                    data: 'cohort',
                },
                {
                    visible: _permissions.isHost,
                    autoWidth: false,
                    orderable: true,
                    targets: 4,
                    data: 'tenant',
                    //"visible": function () {
                    //    Console.log("returning: " + _permissions.isHost + " for visible");
                    //    /*return _permissions.isHost;*/
                    //    return false;
                    //}
                },
            ],
        });

        $('#UserTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getUser() {
            dataTable.ajax.reload();
        }

        $('#GetUserButton').click(function (e) {
            e.preventDefault();
            getUser();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {

            if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
                e.preventDefault();
                getUser();
            }
        });
    };
})(jQuery);
