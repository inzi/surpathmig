(function () {
    $(function () {

        var _userid = $('#profilePicture').data('userid');
        var _$cohortUserRecordStatesTable = $('#CohortUserRecordStatesTable');

        var _cohortUsersService = abp.services.app.cohortUsers;

        var _$cohortUserInformationForm = $('form[name=CohortUserInformationsForm]');
        _$cohortUserInformationForm.validate();

        var _CohortUsercohortLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CohortUsers/CohortLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CohortUsers/_CohortUserCohortLookupTableModal.js',
            modalClass: 'CohortLookupTableModal',
        });
        var _CohortUseruserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CohortUsers/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CohortUsers/_CohortUserUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal',
        });

        $('.date-picker').daterangepicker({
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        });

        $('#OpenCohortLookupTableButton').click(function () {
            // console.log('OpenCohortLookupTableButton createandedit');
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


        // Get Records

        function GetUserRecords() {
            // get the user documents using _userid
            console.log('GetUserRecords');
            dataTable.draw();

        };

        function LoadDocumentDataTable() {

        }
        /// Documents DataTable

        var dataTable = _$cohortUserRecordStatesTable.DataTable({
            paging: true,
            lengthMenu: [5, 10, 25, 50, 100, 250, 500, 5000],
            pageLength: 5000,
            serverSide: true,
            processing: true,
            deferLoading: 0, // here
            listAction: {
                ajaxFunction: abp.services.app.recordStates.getAllForUserId,
                inputFilter: function () {
                    return {
                        userId: _userid,
                        maxResultCount: 500,
                    };
                },
            },
            columnDefs: [
                {
                    targets: 0,
                    data: null,
                    //data: 'recordfilename',
                    //name: 'recordfilename',
                    //defaultContent: '',
                    render: function (data, row, full) {
                        var _statusName = data.recordStatus.statusName;
                        var _ribbonColor = data.recordStatus.htmlColor;

                        var _html = '' +
                            '                                            <div class="surpath-table-ribbon-cell">' +
                            //'                                                <div class="card-header ribbon ribbon-start ribbon-clip">' +
                            '                                                <div class="ribbon ribbon-start ribbon-clip">' +
                            '															<div class="ribbon-label">' + _statusName + ' ' +
                            '															<span class="ribbon-inner" style="background-color: ' + _ribbonColor + '"></span></div>' +
                            '														</div>' +
                            '                                            <div class="symbol symbol-50px me-2 d-none">' +
                            // '                                                <span class="symbol-label bg-light-success" style="bg-color: ' + _ribbonColor + '">' +
                            '                                                <span class="symbol-label" style="bg-color: ' + _ribbonColor + '">' +
                            '                                                    <!--begin::Svg Icon | path: icons/duotune/ecommerce/ecm002.svg-->' +
                            '                                                    <span class="svg-icon svg-icon-2x svg-icon-success">' +
                            '                                                        <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">' +
                            '                                                            <path d="M21 10H13V11C13 11.6 12.6 12 12 12C11.4 12 11 11.6 11 11V10H3C2.4 10 2 10.4 2 11V13H22V11C22 10.4 21.6 10 21 10Z" fill="currentColor"></path>' +
                            '                                                            <path opacity="0.3" d="M12 12C11.4 12 11 11.6 11 11V3C11 2.4 11.4 2 12 2C12.6 2 13 2.4 13 3V11C13 11.6 12.6 12 12 12Z" fill="currentColor"></path>' +
                            '                                                            <path opacity="0.3" d="M18.1 21H5.9C5.4 21 4.9 20.6 4.8 20.1L3 13H21L19.2 20.1C19.1 20.6 18.6 21 18.1 21ZM13 18V15C13 14.4 12.6 14 12 14C11.4 14 11 14.4 11 15V18C11 18.6 11.4 19 12 19C12.6 19 13 18.6 13 18ZM17 18V15C17 14.4 16.6 14 16 14C15.4 14 15 14.4 15 15V18C15 18.6 15.4 19 16 19C16.6 19 17 18.6 17 18ZM9 18V15C9 14.4 8.6 14 8 14C7.4 14 7 14.4 7 15V18C7 18.6 7.4 19 8 19C8.6 19 9 18.6 9 18Z" fill="currentColor"></path>' +
                            '                                                        </svg>' +
                            '                                                    </span>' +
                            '                                                    <!--end::Svg Icon-->' +
                            '                                                </span>' +
                            '                                            </div>' +
                            '';


                        return _html;
                    },
                },
                {
                    targets: 1,
                    data: 'recordCategoryName'
                },
                {
                    targets: 2,
                    data: null,
                    render: function (data, row, full) {

                        var _$link = $('<a/>', {
                            //href: '/File/ViewBinaryFile?id=' + data.recordState.recordDto.binaryObjId,
                            href: '/App/RecordStates/ReviewRecordState?id=' + data.recordState.id,
                            //target: '_blank',
                            text: data.recordfilename
                        });

                        var _$link2 = $('<a/>', {
                            href: '/File/ViewBinaryFile?id=' + data.recordState.recordDto.binaryObjId,
                            target: '_blank',
                            class: 'fas fa-external-link-alt surpath-icon-margin-left',
                            text: '' // app.localize('NewTab')
                        });
                        var _$links = $('<div/>');
                        _$links[0].appendChild(_$link[0]).appendChild(_$link2[0]);
                        //var _html = _$link[0].outerHTML;
                        var _html = _$link[0].outerHTML;
                        _html = _$links[0].outerHTML;
                        return _html;

                    }
                }
            ],
        
        });

    _$cohortUserRecordStatesTable
        .on('xhr.dt', function (e, settings, json, xhr) {
            console.log('data back');
            console.log(settings);
            console.log(json);
            //sortingUser = settings.rawServerResponse.sortingUser;
            dataTable.rawData = settings.rawServerResponse;
            //debugger;
        });


    function showSelection() {
        var _location_hash = location.hash;
        if (_location_hash == '') _location_hash = '#overview';
        _location_hash = _location_hash.substring(1);
        $('#surscan_user_viewarea').addClass('d-none');
        $('.surscan_user_area_menu').removeClass('active');
        $('.surscan_user_area_menu_' + _location_hash).addClass('active');
        $('#surscan_user_' + _location_hash).removeClass('d-none');
        // debugger;
        switch (_location_hash) {
            case 'documents':
                GetUserRecords();
                break;
            default:
                break;
        }
    };
    showSelection();

});
}) ();
