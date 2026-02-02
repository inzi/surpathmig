(function ($) {
    app.modals.RecordCategoryConfirmLookupTableModal = function () {
        var _modalManager;
        console.log('RecordCategoryConfirmLookupTableModal compliance');

        var _complianceService = abp.services.app.surpathCompliance;

        var _$recordCategoryTable = $('#RecordCategoryConfirmTable');


        this.init = function (modalManager) {
            _modalManager = modalManager;
        };

        var dataTableConfirmCategory = _$recordCategoryTable.DataTable({
            paging: true,
            lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
            pageLength: 5000,
            serverSide: true,
            processing: true,
            iDisplayLength: 500,
            listAction: {
                ajaxFunction: _complianceService.getAllRecordCategoryForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#RecordCategoryTableFilter').val(),
                        shuffle: $('#confirm').val() == 'true'
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
        })
            .on('xhr.dt', function (e, settings, json, xhr) {
                console.log(e);
                console.log(settings);
                console.log(json);
                console.log(xhr);
                //for (var i = 0, ien = json.aaData.length; i < ien; i++) {
                //    /*json.aaData[i].sum = json.aaData[i].one + json.aaData[i].two;*/
                //}
                // Note no return - manipulate the data directly in the JSON object.
            });

        $('#RecordCategoryConfirmTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTableConfirmCategory.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getRecordCategory() {
            dataTableConfirmCategory.ajax.reload();
        }

        $('#GetRecordCategoryButton').click(function (e) {
            e.preventDefault();
            getRecordCategory();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            
            if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
                getRecordCategory();
            }
        });
    };
})(jQuery);
