(function () {
    app.modals.AddDepartmentModal = function () {
        var _modalManager;

        var _options = {
            serviceMethod: null,
            title: app.localize('SelectAnItem'),
            loadOnStartup: true,
            showFilter: true,
            filterText: '',
            pageSize: app.consts.grid.defaultPageSize
        };

        var _$table;
        var _$filterInput;
        var dataTable;

        function refreshTable() {
            dataTable.ajax.reload();
        }

        function updateSaveButtonState() {
            console.log('updateSaveButtonState updateSaveButtonState updateSaveButtonState');
            var rowData = dataTable.rows({ selected: true }).data().toArray();
            var $saveButton = _modalManager.getModal().find('#btnAddDepartmentsToOrganization');
            if (rowData.length > 0) {
                $saveButton.removeAttr('disabled');
            } else {
                $saveButton.attr('disabled', 'disabled');
            }
        }

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _options = $.extend(_options, _modalManager.getOptions().addDepartmentOptions);

            _$table = _modalManager.getModal().find('#TenantDepartmentTable');
            _$filterInput = _modalManager.getModal().find('.add-member-filter-text');
            _$filterInput.val(_options.filterText);

            dataTable = _$table.DataTable({
                paging: true,
                serverSide: true,
                processing: true,
                deferLoading: 0,
                listAction: {
                    ajaxFunction: _options.serviceMethod,
                    inputFilter: function () {
                        return {
                            filter: _$filterInput.val(),
                            organizationUnitId: _modalManager.getArgs().organizationUnitId
                        };
                    }
                },
                columnDefs: [
                    {
                        targets: 0,
                        data: null,
                        defaultContent: '',
                        render: function (data) {
                            return '<label for="checkbox_' + data.value + '" class="checkbox form-check">' +
                                '<input type="checkbox" id="checkbox_' + data.value +
                                '" class="form-check-input" value="' + data.value + '" />' +
                                '<span></span>' +
                                '</label>';
                        }
                    },
                    {
                        targets: 1,
                        data: "name"
                    }
                ],
                select: {
                    style: 'multi'
                }
            });

            _modalManager.getModal().find('.add-member-filter-button').click(function (e) {
                e.preventDefault();
                refreshTable();
            });

            _modalManager.getModal().find('.modal-body').keydown(function (e) {
                if (e.which === 13) {
                    e.preventDefault();
                    refreshTable();
                }
            });

            _$table.on('change', 'input[type=checkbox]', function () {
                updateSaveButtonState();
            });

            if (_options.loadOnStartup) {
                refreshTable();
            }

            _modalManager
                .getModal()
                .find('#btnAddDepartmentsToOrganization')
                .click(function () {
                    _modalManager.setResult(dataTable.rows({ selected: true }).data().toArray());
                    _modalManager.close();
                });

            //_$table.on('select', function (e, dt, type, indexes) {
            //    updateSaveButtonState();
            //});
            //_$table.on('deselect', function (e, dt, type, indexes) {
            //    updateSaveButtonState();
            //});
        };


    };
})();
