(function ($) {
    app.modals.TenantDepartmentLookupTableRegModal = function () {
        var _modalManager;


        //var _departmentUsersService = abp.services.app.departmentUsers;
        var _complianceService = abp.services.app.surpathCompliance;

        //var _tenantDepartmentsService = abp.services.app.tenantDepartments;
        var _tableName = 'TenantDepartmentRegTable';
        var _$tenantDepartmentTable = $('#' + _tableName);
        var _shuffle = $('#confirm').val();

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
                //ajaxFunction: abp.services.app.commonLookup.getAllTenantDeptForLookupTable, //  _departmentUsersService.getAllTenantDepartmentForLookupTable,
                ajaxFunction: _complianceService.getAllTenantDeptForLookupTable, //  _departmentUsersService.getAllTenantDepartmentForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#TenantDepartmentTableFilter').val(),
                        shuffle: _shuffle
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

        //_$tenantDepartmentTable
        //    .on('xhr.dt', function (e, settings, json, xhr) {
        //        console.log('data back');
        //        console.log(settings);
        //        console.log(json);
        //        //sortingUser = settings.rawServerResponse.sortingUser;
        //        //dataTable.rawData = settings.rawServerResponse;
        //        debugger;
        //    });


        $('#' + _tableName + ' tbody').on('click', '[id*=selectbtn]', function () {
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

        $(document).keypress(function (e) {
            if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
                getTenantDepartment();
            }
        });
    };
})(jQuery);



//(function ($) {
//    app.modals.TenantDepartmentLookupTableRegModal = function () {
//        var _modalManager;
//        console.log('_TenantDepartmentLookupTableRegModal compliance');

//        var _complianceService = abp.services.app.surpathCompliance;

//        var _$TenantDepartmentTable = null;


//        this.init = function (modalManager) {
//            debugger;
//            _modalManager = modalManager;

//            var modal = _modalManager.getModal();

//            modal.find('.date-picker').daterangepicker({
//                singleDatePicker: true,

//                locale: { format: 'MM/DD/YYYY', },
//            });

//            //_SaveButton = _modalManager.getModal().find('button.save-button');
//            //_SaveButton.attr('disabled', 'disabled');
//            _$TenantDepartmentTable = _modalManager.getModal().find('form[name=TenantDepartmentTable]');
//            _$TenantDepartmentTable.validate();

//        };



//        var dataTableTenantDepatment = _$TenantDepartmentTable.DataTable({
//            paging: true,
//            serverSide: true,
//            processing: true,
//            iDisplayLength: 500,
//            listAction: {
//                ajaxFunction: _complianceService.getAllTenantDepartmentForLookupTable,
//                inputFilter: function () {
//                    return {
//                        filter: $('#TenantDepartmentTableFilter').val(),
//                        shuffle: $('#confirm').val() == 'true'
//                        };
//                    },
//                },
//                columnDefs: [
//                    {
//                        autoWidth: false,
//                        orderable: false,
//                        targets: 0,
//                        data: 'displayName',
//                    },
//                ],
//            })
//            .on('xhr.dt', function (e, settings, json, xhr) {
//                console.log(e);
//                console.log(settings);
//                console.log(json);
//                console.log(xhr);
//                //for (var i = 0, ien = json.aaData.length; i < ien; i++) {
//                //    /*json.aaData[i].sum = json.aaData[i].one + json.aaData[i].two;*/
//                //}
//                // Note no return - manipulate the data directly in the JSON object.
//            });

//        $('#TenantDepartmentTable tbody').on('click', '[id*=selectbtn]', function () {
//            var data = dataTableTenantDepatment.row($(this).parents('tr')).data();
//            _modalManager.setResult(data);
//            _modalManager.close();
//        });

//        function getTenantDepartment() {
//            dataTableTenantDepatment.ajax.reload();
//        }

//        $('#GetTenantDepartmentButton').click(function (e) {
//            e.preventDefault();
//            getTenantDepartment();
//        });

//        $('#SelectButton').click(function (e) {
//            e.preventDefault();
//        });

//        $(document).keypress(function (e) {
//            if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
//                e.preventDefault();
//                getTenantDepartment();
//            }
//        });
//    };
//})(jQuery);
