(function () {
    $(function () {
        // console.log('viewcohortuser.js loaded');

        var _userid = $('#profilePicture').data('userid');
        var _$cohortUserRecordStatesTable = $('#CohortUserRecordStatesTable');

        var _cohortUsersService = abp.services.app.cohortUsers;

        var _cohortUserLedgerBtn = $('#CohortUserLedgerBtn');
        var _openLedgerBtn = $('#openLedgerBtn');

        var _complianceCompletionProgressBar = $('#complianceCompletionProgressBar');
        var _complianceCompletionProgressBarPercent = $('#complianceCompletionProgressBarPercent');
        var _complianceCompletionProgressBarWrapper = $('#complianceCompletionProgressBarWrapper');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.CohortUsers.Create'),
            edit: abp.auth.hasPermission('Pages.CohortUsers.Edit'),
            delete: abp.auth.hasPermission('Pages.CohortUsers.Delete'),
            view: abp.auth.hasPermission('Pages.CohortUsers'),
            isHost: abp.session.multiTenancySide == abp.multiTenancy.sides.HOST,
            recordRequirementsAdminAccess: abp.auth.hasPermission('Pages.Administration.RecordRequirements'),
            "App.Surpath.DrugTest.Download": abp.auth.hasPermission('Surpath.DrugScreenDownload'),
            "App.Surpath.DrugTest.ViewStatus": abp.auth.hasPermission('Surpath.DrugScreenSeeStatus'),
            "App.Surpath.DrugTest.ChangeStatus": abp.auth.hasPermission('Surpath.DrugScreenChangeStatus'),
            "App.Surpath.BackgroundCheck.Download": abp.auth.hasPermission('Surpath.BackgroundCheckDownload'),
            "App.Surpath.BackgroundCheck.ViewStatus": abp.auth.hasPermission('Surpath.BackgroundCheckSeeStatus'),
            "App.Surpath.BackgroundCheck.ChangeStatus": abp.auth.hasPermission('Surpath.BackgroundCheckChangeStatus'),
            "Pages.CohortUsers.Edit": abp.auth.hasPermission('Pages.CohortUsers.Edit'),
            "Pages.CohortUsers.Create": abp.auth.hasPermission('Pages.CohortUsers.Create'),
            "Surpath.ReviewRequirementChange": abp.auth.hasPermission('Surpath.ReviewRequirementChange')
        };
        // console.log('viewcohortuser.js','permissions set');
        var _$cohortUserInformationForm = $('form[name=CohortUserInformationsForm]');
        _$cohortUserInformationForm.validate();
        // console.log('viewcohortuser.js', 'setting modals');

        var _CohortUsercohortLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CohortUsers/CohortLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CohortUsers/_CohortUserCohortLookupTableModal.js',
            modalClass: 'CohortLookupTableModal',
        });
        var _modalSize = 'modal-lg';
        if (_permissions.isHost == true) _modalSize = 'modal-xl';
        var getModalSize = function () {
            if (_permissions.isHost == true) return 'modal-xl';
            return 'modal-lg';
        }
        var _CohortUseruserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CohortUsers/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CohortUsers/_CohortUserUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal',
            modalSize: _modalSize
        });

        var mySettingsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Profile/MySettingsModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Profile/_MySettingsModal.js',
            modalClass: 'MySettingsModal',
        });

        $('#ViewUserProfileMySettings').click(function (e) {
            e.preventDefault();
            mySettingsModal.open();
        });

        var _createNewRecordModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Compliance/CreateNewRecordModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Compliance/_CreateNewRecordModal.js',
            modalClass: 'CreateNewRecordModal',
        });

        var _createNewRecordForCategoryModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Compliance/CreateNewRecordForCategoryModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Compliance/_CreateNewRecordForCategoryModal.js',
            modalClass: 'CreateNewRecordForCategoryModal',
        });

        var _viewNotesForRecordModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RecordNotes/ViewNotesForRecord',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RecordNotes/_ViewNotesForRecordModal.js',
            modalClass: 'ViewNotesForRecordModal',
            modalSize: 'modal-xl'
        });

        // console.log('viewcohortuser.js', 'modals set');

        $('.date-picker').daterangepicker({
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        });

        $('#OpenCohortLookupTableButton').click(function () {
            // console.log('OpenCohortLookupTableButton viewcohortuser');
            var cohortUser = _$cohortUserInformationForm.serializeFormToObject();

            _CohortUsercohortLookupTableModal.open(
                { id: cohortUser.cohortId, displayName: cohortUser.cohortDescription },
                function (data) {
                    _$cohortUserInformationForm.find('input[name=cohortDescription]').val(data.displayName);
                    _$cohortUserInformationForm.find('input[name=cohortId]').val(data.id);
                }
            );
        });

        $('#ClearCohortDescriptionButton').click(function () {
            _$cohortUserInformationForm.find('input[name=cohortDescription]').val('');
            _$cohortUserInformationForm.find('input[name=cohortId]').val('');
        });

        $('#OpenUserLookupTableButton').click(function () {
            var cohortUser = _$cohortUserInformationForm.serializeFormToObject();

            _CohortUseruserLookupTableModal.open(
                { id: cohortUser.userId, displayName: cohortUser.userName },
                function (data) {
                    _$cohortUserInformationForm.find('input[name=userName]').val(data.displayName);
                    _$cohortUserInformationForm.find('input[name=userId]').val(data.id);
                }
            );
        });

        $('#ClearUserNameButton').click(function () {
            _$cohortUserInformationForm.find('input[name=userName]').val('');
            _$cohortUserInformationForm.find('input[name=userId]').val('');
        });

        $('#CreateNewRecordStateButton').click(function (e) {
            e.preventDefault();
            var _userId = $('#UserId').val();
            var _cohortUserId = $('#CohortUserId').val();
            // console.log(_userid);
            _createNewRecordModal.open(
                { id: _userId, cohortUserId: _cohortUserId },
                function (data) {
                }
            );
        });

        function handleViewNotesClick(e) {
            e.preventDefault();
            var _$target = $(e.target);
            var _catid = _$target.closest('tr').data('catid');
            var _recordstateid = _$target.closest('tr').data('recordstateid');
            // console.log(_catid);

            _viewNotesForRecordModal.open(
                { id: _recordstateid },
                function (data) {
                }
            );
        };

        function handleCatUploadClick(e) {
            e.preventDefault();
            var _$target = $(e.target);
            var _catid = _$target.closest('tr').data('catid');
            // console.log(_catid);
            var _userId = $('#UserId').val();
            var _cohortUserId = $('#CohortUserId').val();
            _createNewRecordForCategoryModal.open(
                { id: _userId, cohortUserId: _cohortUserId, catid: _catid },
                function (data) {
                }
            );
        };

        function handleServiceUploadClick(e) {
            e.preventDefault();
            var _$target = $(e.target);
            var _catid = _$target.data('catid');
            var _userId = $('#UserId').val();
            var _cohortUserId = $('#CohortUserId').val();
            _createNewRecordForCategoryModal.open(
                { id: _userId, cohortUserId: _cohortUserId, catid: _catid },
                function (data) {
                }
            );
        };
        $('.viewnotesbutton').on('click', handleViewNotesClick);

        //function handleBCDownloadClick(e) {
        //    e.preventDefault();
        //    var _$target = $(e.target);
        //    var _recordid = _$target.data('recordid');
        //    var binobjid = _$target.data('binobjid');
        //    var _url = '/File/DownloadBinaryFile?id=' + binobjid;
        //    window.open(_url, '_blank');
        //}

        function handleServiceDownloadClick(e) {
            e.preventDefault();
            var _$target = $(e.target);
            var _recordid = _$target.data('recordid');
            var binobjid = _$target.data('binobjid');
            var _url = '/File/DownloadBinaryFile?id=' + binobjid;
            window.open(_url, '_blank');
        }
        function handleServiceViewClick(e) {
            e.preventDefault();
            var _$target = $(e.target);
            var _recordid = _$target.data('recordid');
            var binobjid = _$target.data('binobjid');
            var _url = '/File/DownloadBinaryFile?id=' + binobjid;
            window.open(_url, '_blank');
        }
        function handleServiceUploadClick(e) {
            e.preventDefault();
            var _$target = $(e.target);
            var _catid = _$target.data('catid');
            var _userId = $('#UserId').val();
            var _cohortUserId = $('#CohortUserId').val();
            // https://localhost:44302/App/RecordStates/ReviewRecordState?id=08dad3ba-faaf-4fba-8fd3-db76dd7f2dbf
            _createNewRecordForCategoryModal.open(
                { id: _userId, cohortUserId: _cohortUserId, catid: _catid },
                function (data) {
                }
            );
        }

        $('.createNewRecordStateForServiceButton').on('click', handleServiceUploadClick);
        //$('.downloadRecordStateForServiceBackgroundCheckButton').on('click', handleServiceDownloadClick)
        $('.downloadRecordStateForServiceButton').on('click', handleServiceDownloadClick)
        $('.viewRecordStateForServiceButton').on('click', handleServiceViewClick)

        function save(successCallback) {
            if (!_$cohortUserInformationForm.valid()) {
                return;
            }
            if ($('#CohortUser_CohortId').prop('required') && $('#CohortUser_CohortId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Cohort')));
                return;
            }
            if ($('#CohortUser_UserId').prop('required') && $('#CohortUser_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }

            var cohortUser = _$cohortUserInformationForm.serializeFormToObject();

            abp.ui.setBusy();
            _cohortUsersService
                .createOrEdit(cohortUser)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    abp.event.trigger('app.createOrEditCohortUserModalSaved');

                    if (typeof successCallback === 'function') {
                        successCallback();
                    }
                })
                .always(function () {
                    abp.ui.clearBusy();
                });
        }

        function clearForm() {
            _$cohortUserInformationForm[0].reset();
        }

        $('#saveBtn').click(function () {
            save(function () {
                window.location = '/App/CohortUsers';
            });
        });

        $('#saveAndNewBtn').click(function () {
            save(function () {
                if (!$('input[name=id]').val()) {
                    //if it is create page
                    clearForm();
                }
            });
        });

        //var _username = $('#profilePicture').data('username');

        if ((_userid + '').length > 0) {
            abp.services.app.profile
                .getProfilePictureByUser(_userid)
                .done(function (data) {
                    if (data.profilePicture) {
                        $('#profilePicture').attr('src', 'data:image/png;base64, ' + data.profilePicture);
                    } else {
                        $('#profilePicture').attr('src', '/Profile/GetDefaultProfilePicture');
                    }
                })
                .fail(function () {
                    $('#profilePicture').attr('src', '/Profile/GetDefaultProfilePicture');
                });
        }

        // Get Records

        function GetUserRecords() {
            // get the user documents using _userid
            console.log('GetUserRecords called with _userid:', _userid);
            if ((_userid + '').length > 0) {
                console.log('Drawing dataTable for userId:', _userid);
                dataTable.draw();
            } else {
                console.log('GetUserRecords: No valid userId found');
            }
        };

        function LoadDocumentDataTable() {
        }
        /// Documents DataTable

        // Child Row:
        function format(d) {
            // `d` is the original data object for the row
            return (
                '<table class="table align-left gs-0 gy-3"">' +
                '<tr>' +
                '<td>' +
                d.name +
                '</td>' +
                '</tr>' +
                '<tr>' +
                '<td>Extension number:</td>' +
                '<td>' +
                d.extn +
                '</td>' +
                '</tr>' +
                '<tr>' +
                '<td>Extra info:</td>' +
                '<td>And any further details here (images etc)...</td>' +
                '</tr>' +
                '</table>'
            );
        }

        var dataTable = _$cohortUserRecordStatesTable.DataTable({
            paging: true,
            lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
            pageLength: 5000,
            serverSide: true,
            processing: true,
            deferLoading: 0, // here
            listAction: {
                ajaxFunction: abp.services.app.recordStates.getRecordStateComplianceForUserId,
                inputFilter: function () {
                    console.log('DataTable inputFilter called with _userid:', _userid);
                    return {
                        userId: _userid,
                        maxResultCount: 500,
                        skipCount: 0
                    };
                },
            },
            columnDefs: [
                {
                    targets: 0,
                    data: null, // 'recordRequirement.name'
                    render: function (data, row, full) {
                        var thisValue = data.recordRequirement.name;

                        //if (data.recCount == 1) return thisValue;

                        //if (data.isParentRow == true && data.isChildRow==false) {
                        if (data.isChildRow == false && data.isParentRow == true) {
                            return thisValue;
                        }
                        else {
                            return '';
                        }

                        //var retval = '';
                        //if (data.recCount > 1) {
                        //    if (data.isParentRow == true) {
                        //        retval = thisValue;
                        //    }
                        //    else {
                        //        if (data.isChildRow == false) {
                        //            retval = thisValue;

                        //        }
                        //    }
                        //}
                        //else {
                        //    retval = thisValue;

                        //}

                        //return retval;
                    }
                },
                {
                    targets: 1,
                    data: null,
                    //data: 'recordfilename',
                    //name: 'recordfilename',
                    //defaultContent: '',
                    render: function (data, row, full) {
                        //console.log(data);
                        if (data.getRecordStateForViewDto != null && data.recordCategory.recordCategoryRule != null && data.recordCategory.recordCategoryRule.isSurpathOnly == false) {
                            var _classRequired = data.recordCategory.recordCategoryRule.required ? ' compliancerequired' : '';
                            if (data.getRecordStateForViewDto.recordState != null) {
                                var _statusName = data.getRecordStateForViewDto.recordStatus.statusName;
                                var _ribbonColor = data.getRecordStateForViewDto.recordStatus.htmlColor;

                                var _html = '' +
                                    '                                            <div class="surpath-table-ribbon-cell">' +
                                    //'                                                <div class="card-header ribbon ribbon-start ribbon-clip">' +
                                    '                                                <div class="ribbon ribbon-start surpath-status-badge-div">' +
                                    '															<div class="surpath-status-ribbon-label' + _classRequired + ' ribbon-label">' + _statusName + ' ' +
                                    '															<span class="ribbon-inner" style="background-color: ' + _ribbonColor + '"></span></div>' +
                                    '														</div></div></div>' +
                                    //'                                            <div class="symbol symbol-50px me-2 d-none">' +
                                    //// '                                                <span class="symbol-label bg-light-success" style="bg-color: ' + _ribbonColor + '">' +
                                    //'                                                <span class="symbol-label" style="bg-color: ' + _ribbonColor + '">' +
                                    //'                                                    <!--begin::Svg Icon | path: icons/duotune/ecommerce/ecm002.svg-->' +
                                    //'                                                    <span class="svg-icon svg-icon-2x svg-icon-success">' +
                                    //'                                                        <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">' +
                                    //'                                                            <path d="M21 10H13V11C13 11.6 12.6 12 12 12C11.4 12 11 11.6 11 11V10H3C2.4 10 2 10.4 2 11V13H22V11C22 10.4 21.6 10 21 10Z" fill="currentColor"></path>' +
                                    //'                                                            <path opacity="0.3" d="M12 12C11.4 12 11 11.6 11 11V3C11 2.4 11.4 2 12 2C12.6 2 13 2.4 13 3V11C13 11.6 12.6 12 12 12Z" fill="currentColor"></path>' +
                                    //'                                                            <path opacity="0.3" d="M18.1 21H5.9C5.4 21 4.9 20.6 4.8 20.1L3 13H21L19.2 20.1C19.1 20.6 18.6 21 18.1 21ZM13 18V15C13 14.4 12.6 14 12 14C11.4 14 11 14.4 11 15V18C11 18.6 11.4 19 12 19C12.6 19 13 18.6 13 18ZM17 18V15C17 14.4 16.6 14 16 14C15.4 14 15 14.4 15 15V18C15 18.6 15.4 19 16 19C16.6 19 17 18.6 17 18ZM9 18V15C9 14.4 8.6 14 8 14C7.4 14 7 14.4 7 15V18C7 18.6 7.4 19 8 19C8.6 19 9 18.6 9 18Z" fill="currentColor"></path>' +
                                    //'                                                        </svg>' +
                                    //'                                                    </span>' +
                                    //'                                                    <!--end::Svg Icon-->' +
                                    //'                                                </span>' +
                                    //'                                            </div>' +
                                    '';

                                return _html;
                            }
                            else {
                                var _html = '' +
                                    '                                            <div class="surpath-table-ribbon-cell">' +
                                    //'                                                <div class="card-header ribbon ribbon-start ribbon-clip">' +
                                    '                                                <div class="ribbon ribbon-start surpath-status-badge-div">' +
                                    '															<div class="surpath-status-ribbon-label' + _classRequired + ' ribbon-label" style="color: darkblue;">' + app.localize("NoUpload") + ' ' +
                                    '															<span class="ribbon-inner" style="background-color: #eeeeee"></span></div>' +
                                    '														</div></div></div>' +
                                    //'                                            <div class="symbol symbol-50px me-2 d-none">' +
                                    //// '                                                <span class="symbol-label bg-light-success" style="bg-color: ' + _ribbonColor + '">' +
                                    //'                                            </div>' +
                                    '';

                                return _html;
                            }
                        }
                        else {
                            var _html = '';
                            if (_permissions.isHost == true || _permissions.recordRequirementsAdminAccess) {
                                var _tooltiptext = app.localize("RequirementHasNoRuleToolTip"); // data-bs-toggle="tooltip" aria-label="Phone number must be active"
                                _html = '' +
                                    '                                            <div class="surpath-table-ribbon-cell" data-bs-toggle="tooltip" title="' + _tooltiptext + '">' +
                                    '                                                <div class="ribbon ribbon-start surpath-status-badge-div">' +
                                    '															<div class="surpath-status-ribbon-label ribbon-label" style="color: darkblue;">' + app.localize("RequirementHasNoRule") + ' ' +
                                    '															<span class="ribbon-inner" style="background-color: #eeeeee"></span></div>' +
                                    '														</div></div></div>' +
                                    '';
                            }
                            return _html;
                        }
                    },
                },

                {
                    targets: 2,
                    data: null, // 'recordCategory.name',
                    render: function (data, row, full) {
                        if (data.getRecordStateForViewDto != null && data.recordCategory.recordCategoryRule != null && data.recordCategory.recordCategoryRule.isSurpathOnly == false) {
                            if (data.recordCategory.recordCategoryRule != null && data.recordCategory.recordCategoryRule.isSurpathOnly == true) return '';
                            var thisValue = data.recordCategory.name;
                            //if (data.recCount == 1) return thisValue;

                            if (data.isParentRow == false && data.isChildRow == true) {
                                return thisValue;
                            }
                            else {
                                return '';
                            }
                        }
                        else {
                            return '';
                        }
                    }
                },

                {
                    targets: 3,
                    data: null,
                    render: function (data, row, full) {
                        if (data.getRecordStateForViewDto != null && data.recordCategory.recordCategoryRule != null && data.recordCategory.recordCategoryRule.isSurpathOnly == false) {
                            var _$text = $('<span/>').text(' ' + app.localize("Upload")).addClass('cohortUserRecordStatesTableRowButton')
                            var _$ullink = $('<a/>', {
                                href: '#',
                                class: 'badge badge-info fas fa-upload surpath-icon-margin-left createNewRecordStateForCategoryButton',
                                //text: 'upload'
                            });
                            var _$links = $('<div/>');

                            if (data.getRecordStateForViewDto.recordState != null) {
                                var _$notestext = $('<span/>').text(' ' + app.localize("Notes")).addClass('cohortUserRecordStatesTableRowButton')
                                var _$notesLink = $('<a/>', {
                                    href: '#',
                                    class: 'badge badge-info fas fa-note surpath-icon-margin-left viewnotesbutton',
                                });
                                _$notesLink.prepend(_$notestext);
                                $(_$notesLink[0]).data('recordstateid', data.getRecordStateForViewDto.recordState.id);
                                $(_$notesLink).data('recordstateid', data.getRecordStateForViewDto.recordState.id);

                                // make ability to download a feature: DownloadBinaryFile
                                var _$dltext = $('<span/>').text(' ' + app.localize("Download")).addClass('cohortUserRecordStatesTableRowButton')

                                var _$dllink = $('<a/>', {
                                    href: '/File/DownloadBinaryFile?id=' + data.getRecordStateForViewDto.recordState.recordDto.binaryObjId,
                                    target: '_blank',
                                    class: 'badge badge-info fas fa-download surpath-icon-margin-left',
                                });
                                _$dllink.prepend(_$dltext);

                                _$text = $('<span/>').text(' ' + app.localize("UploadNew")).addClass('cohortUserRecordStatesTableRowButton')
                                _$ullink.prepend(_$text);

                                _$links[0].appendChild(_$dllink[0]).appendChild(_$ullink[0]).appendChild(_$notesLink[0]);
                                //var _html = _$link[0].outerHTML;
                                var _html = _$links[0].outerHTML;
                                return _html;
                            }
                            else {
                                //$(_$ullink[0]).click(function (e) {
                                //    alert('clicked');
                                //});
                                //$(_$ullink[0]).data('catid', data.recordCategory.id);
                                //debugger;
                                _$ullink.prepend(_$text);

                                _$links[0].appendChild(_$ullink[0]); //.appendChild(_$notesLink[0]);

                                var _html = _$links[0].outerHTML;
                                return _html;
                                //                                    '                                               <a id="CreateNewRecordStateForCategoryButton" href="" class="btn btn-sm btn-primary me-2 createNewRecordStateForCategoryButton" data-catid="' + 'catid' + '">' + app.localize("UploadThisRecord")+ '</a>' +
                                return '';
                            }
                        }
                        else {
                            return '';
                        }
                    }
                },
                {
                    targets: 4,
                    data: null,
                    render: function (data, row, full) {
                        console.log('Target 4 render');
                        // this needs to check the user permissions, not if it's a surpath service
                        var _hasReviewPermission = _permissions["Surpath.ReviewRequirementChange"];
                        //debugger;
                        //if (data.getRecordStateForViewDto != null && _hasReviewPermission == true) {
                        if (data.getRecordStateForViewDto != null && (_hasReviewPermission==true || data.getRecordStateForViewDto.recordStatus.isSurpathServiceStatus != true)) {
                            if (data.getRecordStateForViewDto.recordState != null) {
                                var _$links = $('<div/>');

                                var _$opentext = $('<span/>').text(' ' + app.localize("Open")).addClass('cohortUserRecordStatesTableRowButton')
                                var _$link2 = $('<a/>', {
                                    href: '/File/ViewBinaryFile?id=' + data.getRecordStateForViewDto.recordState.recordDto.binaryObjId,
                                    target: '_blank',
                                    class: 'badge badge-primary fas fa-external-link-alt surpath-icon-margin-left',
                                    /*text: ' Open' // app.localize('NewTab')*/
                                });
                                _$link2.prepend(_$opentext);

                                _$links[0].appendChild(_$link2[0]);

                                var _$reviewText = $('<span/>').text(' ' + app.localize("Review")).addClass('cohortUserRecordStatesTableRowButton')
                                var _$link = $('<a/>', {
                                    //href: '/File/ViewBinaryFile?id=' + data.recordState.recordDto.binaryObjId,
                                    href: '/App/RecordStates/ReviewRecordState?id=' + data.getRecordStateForViewDto.recordState.id,
                                    class: 'badge badge-primary fas fa-search surpath-icon-margin-left',
                                    //target: '_blank',
                                    //text: ' Review' // + data.getRecordStateForViewDto.recordfilename
                                });
                                _$link.prepend(_$reviewText);

                                if (_permissions.edit) {
                                    _$links[0].appendChild(_$link[0])
                                }
                                //var _html = _$link[0].outerHTML;
                                var _html = _$links[0].outerHTML;
                                return _html;
                            }
                            else {
                                return '';
                            }
                        }
                        else {
                            return '';
                        }
                    }
                },
            ],
            drawCallback: function (data) {
                //$('.createNewRecordStateForCategoryButton,.cohortUserRecordStatesTableRowButton').click(handleCatUploadClick);
                $('.createNewRecordStateForCategoryButton').click(handleCatUploadClick);

                $('.viewnotesbutton').on('click', handleViewNotesClick);
                //console.log('drawCallback');
                let _data = data.aoData;
                //console.log(_data);
                var requiredcount = 0;
                var compliantcount = 0;
                $.each(_data, function (i, v) {
                    //console.log('v:');
                    //console.log(v);
                    if (v._aData.recordCategory.recordCategoryRule != null && v._aData.recordCategory.recordCategoryRule.required === true) {
                        requiredcount++;
                        if (v._aData.getRecordStateForViewDto.recordStatus.complianceImpact === 1) compliantcount++;
                    }
                });
                //console.log(requiredcount);
                //console.log(compliantcount);
                var _percentCompliant = null;
                var _id = location.pathname.substring(location.pathname.lastIndexOf('/') + 1);
                var _key = '_percentCompliant-' + _id;
                var _opacity = '0.2';
                if (requiredcount > 0) {
                    _percentCompliant = Math.round((compliantcount / requiredcount) * 100);
                    localStorage.setItem(_key, _percentCompliant);
                }
                else {
                    _percentCompliant = localStorage.getItem(_key);
                }
                if (_percentCompliant != null) {
                    //_percentString = _percentCompliant.toString() + '%';
                    _complianceCompletionProgressBarWrapper.css('opacity', '1.0');
                    _complianceCompletionProgressBar.css('opacity', '1.0');
                }
                else {
                    _percentCompliant = '0';
                    _complianceCompletionProgressBarWrapper.css('opacity', _opacity);
                    _complianceCompletionProgressBar.css('opacity', _opacity);
                }

                _percentString = _percentCompliant.toString() + '%';
                _complianceCompletionProgressBar.css('width', _percentString);
                _complianceCompletionProgressBarPercent.html(_percentString);
                _complianceCompletionProgressBar.attr('aria-valuenow', _percentCompliant);
            },
            //createdRow: function (row, data, index) {
            //    //
            //    // if the second column cell is blank apply special formatting
            //    //
            //    //if (data[1] == "") {
            //    //    console.dir(row);
            //    //    $('tr', row).addClass('label-warning');
            //    //}
            //    console.log('createdRow')
            //    console.log(data);
            //},
            fnRowCallback: function (row, data, iDisplayIndex, iDisplayIndexFull) {
                //debugger;
                //createdRow: function (row, data, index) {
                //    //
                //    // if the second column cell is blank apply special formatting
                //    //
                //    if (data[1] == "") {
                //        console.dir(row);
                //        $('tr', row).addClass('label-warning');
                //    }
                //}
                //console.log('fnRowCallback')
                //console.log(data);

                if (data.recordCategory.recordCategoryRule != null && data.recordCategory.recordCategoryRule.isSurpathOnly == true) {
                    // hide this created, empty row
                    //debugger;
                    console.log('hide this created, empty row');
                    $(row).addClass("d-none");
                }

                if (data.isParentRow == true) {
                    // set first TD to coslapn 5
                    $('td:eq(0)', row).attr('colspan', 5);
                    $(row).addClass('requirementParentRow');

                    // Hide required number of columns
                    // next to the cell with COLSPAN attribute
                    $('td:eq(1)', row).addClass('d-none');
                    $('td:eq(2)', row).addClass('d-none');
                    $('td:eq(3)', row).addClass('d-none');
                    $('td:eq(4)', row).addClass('d-none');
                }
                else {
                    // a child row
                    $(row).data('catid', data.recordCategory.id);
                    //console.log(data);
                    if (data.getRecordStateForViewDto.recordState != null) {
                        $(row).data('recordstateid', data.getRecordStateForViewDto.recordState.id);
                    }

                    $('td:eq(0)', row).css('display', 'none');
                    $('td:eq(0)', row).addClass('d-none');
                    $('td:eq(1)', row).css('width', '220px');
                    $('td:eq(1)', row).css('height', '1em');
                    $('td:eq(2)', row).css('width', '80%');

                    // Hide required number of columns
                    // next to the cell with COLSPAN attribute
                }

                if (data.getRecordStateForViewDto != null && data.recordCategory.recordCategoryRule == null && _permissions.recordRequirementsAdminAccess == false) {
                    $(row).addClass("d-none");
                }
            },
        });

        _$cohortUserRecordStatesTable
            .on('xhr.dt', function (e, settings, json, xhr) {
                console.log('DataTable xhr.dt event - data back, count:', json.data ? json.data.length : 0);
                dataTable.rawData = settings.rawServerResponse;
                processSurpathServicesFromResults(json.data);
                console.log('xhr.dt complete');
            });

        // Initialize Archives DataTable
        var _$cohortUserArchivedDocumentsTable = $('#CohortUserArchivedDocumentsTable');
        var archivedDocumentsDataTable = null;
        var archivedDocumentsData = [];

        function GetUserArchivedDocuments() {
            abp.services.app.recordStates.getArchivedDocumentsForUser({
                userId: _userid,
                maxResultCount: 1000
            }).done(function (result) {
                console.log('Archived documents result:', result);
                archivedDocumentsData = result;
                renderArchivedDocumentsTable();
            }).fail(function (error) {
                console.error('Error getting archived documents:', error);
                abp.notify.error('Failed to load archived documents');
            });
        }

        function renderArchivedDocumentsTable() {
            var tbody = _$cohortUserArchivedDocumentsTable.find('tbody');
            tbody.empty();

            if (archivedDocumentsData.length === 0) {
                tbody.append('<tr><td colspan="6" class="text-center">' + app.localize('NoArchivedDocumentsFound') + '</td></tr>');
                return;
            }

            $.each(archivedDocumentsData, function (index, category) {
                // Create parent row for category
                var parentRow = $('<tr class="fw-bold bg-light category-row" data-category-index="' + index + '">');
                parentRow.append('<td colspan="6">' +
                    '<span class="expand-icon me-2">▶</span>' +
                    category.categoryName +
                    ' <span class="badge badge-secondary ms-2">' + category.archivedDocuments.length + ' documents</span>' +
                    '</td>');
                tbody.append(parentRow);

                // Create child rows for documents
                $.each(category.archivedDocuments, function (docIndex, doc) {
                    var childRow = $('<tr class="document-row d-none" data-category-index="' + index + '">');
                    
                    // Empty cell for indentation
                    childRow.append('<td style="padding-left: 40px;">');
                    
                    // File name
                    childRow.append('<td>' + (doc.fileName || 'Unknown') + '</td>');
                    
                    // Upload date
                    childRow.append('<td>' + moment(doc.creationTime).format('L LT') + '</td>');
                    
                    // Status
                    var statusHtml = '';
                    if (doc.recordStatus) {
                        var statusClass = doc.recordStatus.cssClass || '';
                        var statusColor = doc.recordStatus.htmlColor || '';
                        statusHtml = '<span class="badge ' + statusClass + '" style="background-color: ' + statusColor + '">' + 
                            doc.recordStatus.statusName + '</span>';
                    }
                    if (!doc.isArchived) {
                        statusHtml += ' <span class="badge badge-info ms-1">Current</span>';
                    }
                    childRow.append('<td>' + statusHtml + '</td>');
                    
                    // Uploaded by
                    childRow.append('<td>' + (doc.createdByUserName || 'System') + '</td>');
                    
                    // Actions
                    var actionsHtml = '<td>';
                    var _$links = $('<div/>');
                    
                    // Download button
                    if (doc.binaryObjId && (_permissions.view || _permissions.edit)) {
                        var _$dltext = $('<span/>').text(' ' + app.localize("Download")).addClass('cohortUserRecordStatesTableRowButton');
                        var _$dllink = $('<a/>', {
                            href: '#',
                            class: 'badge badge-info fas fa-download surpath-icon-margin-left downloadRecordButton',
                            'data-binary-obj-id': doc.binaryObjId,
                            'data-file-name': doc.fileName
                        });
                        _$dllink.prepend(_$dltext);
                        _$links[0].appendChild(_$dllink[0]);
                    }
                    
                    // Open button
                    if (doc.binaryObjId && (_permissions.view || _permissions.edit)) {
                        var _$opentext = $('<span/>').text(' ' + app.localize("Open")).addClass('cohortUserRecordStatesTableRowButton');
                        var _$openlink = $('<a/>', {
                            href: '/File/ViewBinaryFile?id=' + doc.binaryObjId,
                            target: '_blank',
                            class: 'badge badge-primary fas fa-external-link-alt surpath-icon-margin-left'
                        });
                        _$openlink.prepend(_$opentext);
                        _$links[0].appendChild(_$openlink[0]);
                    }
                    
                    // Review button
                    if (_permissions.edit) {
                        var _$reviewText = $('<span/>').text(' ' + app.localize("Review")).addClass('cohortUserRecordStatesTableRowButton');
                        var _$reviewlink = $('<a/>', {
                            href: '/App/RecordStates/ReviewRecordState?id=' + doc.recordStateId,
                            class: 'badge badge-primary fas fa-search surpath-icon-margin-left'
                        });
                        _$reviewlink.prepend(_$reviewText);
                        _$links[0].appendChild(_$reviewlink[0]);
                    }
                    
                    actionsHtml += _$links[0].outerHTML + '</td>';
                    childRow.append(actionsHtml);
                    
                    tbody.append(childRow);
                });
            });

            // Add expand/collapse functionality
            $('.category-row').on('click', function () {
                var categoryIndex = $(this).data('category-index');
                var childRows = $('.document-row[data-category-index="' + categoryIndex + '"]');
                var expandIcon = $(this).find('.expand-icon');
                
                if (childRows.hasClass('d-none')) {
                    childRows.removeClass('d-none');
                    expandIcon.text('▼');
                } else {
                    childRows.addClass('d-none');
                    expandIcon.text('▶');
                }
            });

            // Initialize tooltips
            $('[data-bs-toggle="tooltip"]').tooltip();

            // Bind action buttons
            $('.downloadRecordButton').on('click', function (e) {
                e.preventDefault();
                var binaryObjId = $(this).data('binary-obj-id');
                var fileName = $(this).data('file-name');
                window.open('/File/DownloadBinaryFile?id=' + binaryObjId, '_blank');
            });
        }

        function showSelection() {
            var _location_hash = location.hash;
            if (_location_hash == '') _location_hash = '#documents';
            _location_hash = _location_hash.substring(1);
            console.log('showSelection called, hash:', _location_hash, 'userId:', _userid);
            $('#surscan_user_viewarea').addClass('d-none');
            $('.surscan_user_area_menu').removeClass('active');
            $('.surscan_user_area_menu_' + _location_hash).addClass('active');
            $('#surscan_user_' + _location_hash).removeClass('d-none');
            // debugger;
            switch (_location_hash) {
                case 'documents':
                    GetUserRecords();
                    break;
                case 'archives':
                    GetUserArchivedDocuments();
                    break;
                default:
                    break;
            }
        };
        showSelection();

        abp.event.on('app.CreateNewRecordSaved', function () {
            GetUserRecords();
        });

        var processSurpathServicesFromResults = function (results) {
            //$.each(results.items, function (indexes, value) {
            $.each(results, function (indexes, value) {
                //console.log(value);
                if (value.surpathService != null) {
                    var _featureidentifier = value.surpathService.featureIdentifier;
                    var _catid = value.recordCategory.id;

                    var _canDownload = _permissions[_featureidentifier + '.Download'];
                    var _canSeeStatus = _permissions[_featureidentifier + '.ViewStatus'];
                    var _canChangeStatus = _permissions[_featureidentifier + '.ChangeStatus'];
                    var _isCompleted = false;
                    console.log('permissions for ' + _featureidentifier);
                    console.log(_canDownload);
                    console.log(_canSeeStatus);
                    console.log(_canChangeStatus);
                    // find createNewRecordStateForServiceButton
                    var _div = $("div").find("[data-featureidentifier='" + _featureidentifier + "']");
                    var _btn = $(_div).find('.createNewRecordStateForServiceButton');
                    $(_btn).data('catid', _catid);
                    $(_btn).attr('data-bs-toggle', 'tooltip')
                        .attr('data-bs-placement', 'bottom')
                        .attr('title', 'Upload');
                    //data-bs-toggle="tooltip" data-bs-placement="bottom" title="Tooltip on bottom"

                    var _btnr = $(_div).find('.reviewRecordStateForServiceButton');
                    $(_btnr).data('catid', _catid);
                    $(_btnr).attr('data-bs-toggle', 'tooltip')
                        .attr('data-bs-placement', 'bottom')
                        .attr('title', 'Review');

                    var _btndl = $(_div).find('.downloadRecordStateForServiceButton');

                    if (value.getRecordStateForViewDto.recordState != null) {
                        var _recid = value.getRecordStateForViewDto.recordState.id;

                        _btnr.attr('href', '/App/RecordStates/ReviewRecordState?id=' + _recid);

                        //debugger;
                        // find download button downloadRecordStateForServiceButton
                        // value.getRecordStateForViewDto.recordState.recordId;
                        var _recordid = value.getRecordStateForViewDto.recordState.recordId;
                        var _binObjId = value.getRecordStateForViewDto.recordState.recordDto.binaryObjId;
                        if (abp.session.userId == value.getRecordStateForViewDto.recordState.userId) {
                            _canDownload = true;
                        }
                        if (_canDownload) {
                            $(_btndl).removeClass('d-none');
                            $(_btndl).data('recordid', _recordid);
                            $(_btndl).data('binobjid', _binObjId);
                        }
                        if (_canChangeStatus) {
                            // https://localhost:44302/App/RecordStates/ReviewRecordState?id=08dad3ba-faaf-4fba-8fd3-db76dd7f2dbf
                            $(_btnr).removeClass('d-none');
                        }

                        _isCompleted = value.getRecordStateForViewDto.recordStatus.complianceImpact > 0; // indicates this status defined compliance
                    }

                    if (value.getRecordStateForViewDto.recordStatus != null) {
                        if (value.getRecordStateForViewDto.recordStatus.id != '00000000-0000-0000-0000-000000000000') {
                            var _state = $(_div).find('.cohortUserRecordStatesStatus');
                            if (_canSeeStatus) {
                                $(_state).text(value.getRecordStateForViewDto.recordStatus.statusName);
                                $(_state).css('background-color', value.getRecordStateForViewDto.recordStatus.htmlColor);
                            }
                            else {
                                if (_isCompleted) {
                                    $(_state).text('Completed');
                                    $(_state).addClass('badge-dark');
                                }
                            }
                        }
                    }
                }
                else {
                    // console.log('not surpath service:' + value.recordCategory.name);
                }
            });
            
            // Special handling for drug screen and background check buttons
            // These buttons are hardcoded in the HTML and need category IDs even if marked as IsSurpathOnly
            processDrugScreenAndBackgroundCheckButtons(results);
        }
        
        // New function to handle drug screen and background check category ID assignment
        var processDrugScreenAndBackgroundCheckButtons = function(results) {
            console.log('Processing drug screen and background check buttons...');
            
            // Look for drug screen and background check requirements in the results
            $.each(results, function(index, value) {
                if (value.recordCategory != null) {
                    var categoryId = value.recordCategory.id;
                    var categoryName = value.recordCategory.name || '';
                    var featureIdentifier = value.surpathService ? value.surpathService.featureIdentifier : null;
                    
                    // Handle drug screen button - check both feature identifier and category name
                    if (featureIdentifier === 'App.Surpath.DrugTest' || 
                        (value.recordRequirement && value.recordRequirement.isSurpathOnly && categoryName.toLowerCase().includes('drug'))) {
                        console.log('Found drug screen category:', categoryId, 'Name:', categoryName);
                        $('.createNewRecordStateForServiceButton.recordstateddrugscreen').data('catid', categoryId);
                        $('.reviewRecordStateForServiceButton.recordstateddrugscreen').data('catid', categoryId);
                        $('.downloadRecordStateForServiceButton.recordstateddrugscreen').data('catid', categoryId);
                        $('.viewRecordStateForServiceButton.recordstateddrugscreen').data('catid', categoryId);
                    }
                    
                    // Handle background check button - check both feature identifier and category name
                    if (featureIdentifier === 'App.Surpath.BackgroundCheck' || 
                        (value.recordRequirement && value.recordRequirement.isSurpathOnly && categoryName.toLowerCase().includes('background'))) {
                        console.log('Found background check category:', categoryId, 'Name:', categoryName);
                        $('.createNewRecordStateForServiceButton.recordstatedbackgroundcheck').data('catid', categoryId);
                        $('.reviewRecordStateForServiceButton.recordstatedbackgroundcheck').data('catid', categoryId);
                        $('.downloadRecordStateForServiceButton.recordstatedbackgroundcheck').data('catid', categoryId);
                        $('.viewRecordStateForServiceButton.recordstatedbackgroundcheck').data('catid', categoryId);
                    }
                }
            });
        }

        // need to check permission: AppPermissions.Pages_CohortUsers_Edit
        var pickauser = function () {
            if (!_userid) {
                if (_permissions["Pages.CohortUsers.Edit"] || _permissions["Pages.CohortUsers.Create"]) {
                    _CohortUseruserLookupTableModal.open(
                        {},
                        function (data) {
                            window.location.href = window.location.href + data.cohortUserId;
                        }
                    );
                }
            }
        }
        pickauser();

        var updatePrevNext = function () {
            if (_permissions.view == false) return;
            if (!_userid) {
                return;
            }
            var _filter = {
                cohortIdFilter: $('#CohortId').val(),
                id: $('#CohortUserId').val()
            };
            _cohortUsersService
                .getPrevNext(_filter)
                .done(function (results) {
                    var _prev = results.items[0];
                    var _next = results.items[1];
                    if (_prev.cohortUser == null) {
                        $('#gotoPrevCohortUser').addClass('d-none');
                        $('#prevName').text('');
                    }
                    else {
                        $('#gotoPrevCohortUser').removeClass('d-none');
                        $('#gotoPrevCohortUser').attr('href', '/App/CohortUsers/ViewCohortUser/' + _prev.cohortUser.id);
                        $('#prevName').text(_prev.userEditDto.name + ' ' + _prev.userEditDto.surname);
                    }

                    if (_next.cohortUser == null) {
                        $('#gotoNextCohortUser').addClass('d-none');
                        $('#nextName').text('');
                    }
                    else {
                        $('#gotoNextCohortUser').removeClass('d-none');
                        $('#gotoNextCohortUser').attr('href', '/App/CohortUsers/ViewCohortUser/' + _next.cohortUser.id);
                        $('#nextName').text(_next.userEditDto.name + ' ' + _next.userEditDto.surname);
                    }
                });
        };
        updatePrevNext();

        _openLedgerBtn.on('click', function () {
            window.location = '/App/LedgerEntries/UserLedger?id=' + _userid;
        });
    });
})();