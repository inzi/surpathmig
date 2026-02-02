(function ($) {
    app.modals.TenantDepartmentLookupModal = function () {
        var _modalManager;
        var _$tenantDepartmentTable = $('#TenantDepartmentTable');
        var _tenantSurpathServicesService = abp.services.app.tenantSurpathServices;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            var lookupModal = _modalManager.getModal();
            lookupModal.find('#TenantDepartmentModalSearchButton').click(function (e) {
                e.preventDefault();
                getTenantDepartments();
            });

            var dataTable = _$tenantDepartmentTable.DataTable({
                paging: true,
                serverSide: true,
                processing: true,
                listAction: {
                    ajaxFunction: _tenantSurpathServicesService.getAllTenantDepartmentForLookupTable,
                    inputFilter: function () {
                        return {
                            filter: lookupModal.find('#TenantDepartmentModalFilter').val()
                        };
                    }
                },
                columnDefs: [
                    {
                        targets: 0,
                        data: null,
                        orderable: false,
                        autoWidth: false,
                        defaultContent: '',
                        rowAction: {
                            element: $("<button/>")
                                .addClass("btn btn-primary btn-sm m-btn--icon")
                                .text(app.localize('Select'))
                                .prepend($("<i/>").addClass("la la-check"))
                                .click(function () {
                                    var record = $(this).data();
                                    _modalManager.setResult(record);
                                    _modalManager.close();
                                })
                        }
                    },
                    {
                        targets: 1,
                        data: "name",
                        name: "name"
                    }
                ]
            });

            function getTenantDepartments() {
                dataTable.ajax.reload();
            }

            lookupModal.find('#TenantDepartmentModalFilter').on('keypress', function (e) {
                if (e.which === 13) {
                    getTenantDepartments();
                }
            });
        };
    };
})(jQuery); 