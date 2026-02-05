(function () {
    app.modals.AssignTenantSurpathServiceModal = function () {
        var _modalManager;
        var _tenantSurpathServicesService = abp.services.app.tenantSurpathServices;
        var _$form = null;
        
        var _cohortLookupModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Compliance/CohortLookupTableRegModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Compliance/_CohortLookupTableRegModal.js',
            modalClass: 'CohortLookupModal'
        });

        var _CohortLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Compliance/CohortLookupTableRegModal',
            scriptUrl:
                abp.appPath + 'view-resources/Areas/App/Views/Compliance/_CohortLookupTableRegModal.js',
            modalClass: 'CohortLookupTableRegModal',
        });

        var _departmentLookupModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/TenantDepartments/TenantDepartmentLookupModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/TenantDepartments/_TenantDepartmentLookupTableModal.js',
            modalClass: 'TenantDepartmentLookupModal'
        });
        
        var _TenantDepartmentLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/TenantDepartments/TenantDepartmentLookupTableModal',
            scriptUrl:
                abp.appPath +
                'view-resources/Areas/App/Views/TenantDepartments/_TenantDepartmentLookupTableModal.js',
            modalClass: 'TenantDepartmentLookupTableModal',
        });

        // var _cohortUserLookupModal = new app.ModalManager({
        //     viewUrl: abp.appPath + 'App/Common/LookupModal',
        //     scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/_Bundles/common-lookup-modal.min.js',
        //     modalClass: 'CohortUserLookupModal'
        // });

        var _CohortUseruserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CohortUsers/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CohortUsers/_CohortUserUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal',
          });

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _$form = _modalManager.getModal().find('form');

            $('#AssignmentTypeSelect').select2({
                placeholder: app.localize('PleaseSelect')
            });

            $('#AssignmentTypeSelect').on('change', function() {
                var selectedType = $(this).val();
                $('.assignment-field').hide();
                
                switch(selectedType) {
                    case 'tenant':
                        break;
                    case 'cohort':
                        $('#CohortField').show();
                        break;
                    case 'department':
                        $('#DepartmentField').show();
                        break;
                    case 'cohortuser':
                        $('#CohortUserField').show();
                        break;
                }
            });

            // Cohort lookup button
            $('#OpenCohortLookupBtn').click(function() {
                // _cohortLookupModal.open({ 
                //     id: $('#ServiceId').val(),
                //     displayName: $('#CohortDisplayName').val(),
                //     filterText: ''
                // }, function (selectedItem) {
                //     $('#CohortDisplayName').val(selectedItem.displayName);
                //     $('#CohortId').val(selectedItem.id);
                // });

                _CohortLookupTableModal.open(
                    { },
                    function (selectedItem) {
                        $('#CohortDisplayName').val(selectedItem.displayName);
                        $('#CohortId').val(selectedItem.id);
                    }
                );                
            });

            // Department lookup button
            $('#OpenDepartmentLookupBtn').click(function() {
                _TenantDepartmentLookupTableModal.open(
                    { id: '', displayName: '' },
                    function (selectedItem) {
                        $('#DepartmentDisplayName').val(selectedItem.displayName);
                        $('#DepartmentId').val(selectedItem.id);
                        // // set the data attribute
                        // _widget.parent().parent().data('settings_tenantdepartmentid', data.id);
                        // _widget.parent().parent().data('settings_tenantdepartmentname', data.displayName);
                        // _widget.find('#departmentName').text(data.displayName);
                        // //$('#savePageButton').click(function () {
                        // //    console.log('back');
                        // //});
                        // //var f2 = getCohorts();
                        // //$.when(f1).then(f2);
                        // savePageData(getCohorts);
                    }
                );

                // _departmentLookupModal.open({ 
                //     id: $('#ServiceId').val(),
                //     displayName: $('#DepartmentDisplayName').val(),
                //     filterText: ''
                // }, function (selectedItem) {
                //     $('#DepartmentDisplayName').val(selectedItem.displayName);
                //     $('#DepartmentId').val(selectedItem.id);
                // });
            });

            // CohortUser lookup button
            $('#OpenCohortUserLookupBtn').click(function() {
                // _cohortUserLookupModal.open({
                //     title: app.localize('SelectCohortUser'),
                //     serviceMethod: abp.services.app.cohortUsers.getAllUserForLookupTable
                // }, function(selectedItem) {
                //     $('#CohortUserDisplayName').val(selectedItem.displayName);
                //     $('#CohortUserId').val(selectedItem.id);
                // });

                _CohortUseruserLookupTableModal.open(
                    { },
                    function (selectedItem) {
                        $('#CohortUserDisplayName').val(selectedItem.displayName);
                        $('#CohortUserId').val(selectedItem.id);
                    //   _$cohortUserInformationForm.find('input[name=userName]').val(data.displayName);
                    //   _$cohortUserInformationForm.find('input[name=userId]').val(data.id);
                    }
                  );
            });
        };

        this.save = function () {
            if (!_$form.valid()) {
                return;
            }

            var assignmentType = $('#AssignmentTypeSelect').val();
            var serviceId = $('#ServiceId').val();

            _modalManager.setBusy(true);
            
            var promise;
            switch(assignmentType) {
                case 'tenant':
                    promise = _tenantSurpathServicesService.assignToTenant(serviceId);
                    break;
                case 'cohort':
                    var cohortId = $('#CohortId').val();
                    promise = _tenantSurpathServicesService.assignToCohort(serviceId, cohortId);
                    break;
                case 'department':
                    var deptId = $('#DepartmentId').val();
                    promise = _tenantSurpathServicesService.assignToTenantDepartment(serviceId, deptId);
                    break;
                case 'cohortuser':
                    var cohortUserId = $('#CohortUserId').val();
                    promise = _tenantSurpathServicesService.assignToCohortUser(serviceId, cohortUserId);
                    break;
            }

            promise.done(function () {
                _modalManager.close();
                abp.notify.success(app.localize('SavedSuccessfully'));
                abp.event.trigger('app.assignmentSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(); 