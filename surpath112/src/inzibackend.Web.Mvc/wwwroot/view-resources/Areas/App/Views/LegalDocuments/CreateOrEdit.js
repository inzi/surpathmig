////(function () {
////    $(function () {
////        // Add ModalManager for preview modal
////        var _previewModal = new app.ModalManager({
////            viewUrl: abp.appPath + 'App/LegalDocuments/PreviewModal',
////            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LegalDocuments/_PreviewModal.js',
////            modalClass: 'LegalDocumentPreviewModal'
////        });

////        // Function to preview HTML content
////        function previewHtmlContent(fileToken) {
////            if (!fileToken) {
////                abp.notify.error(L('InvalidFileToken'));
////                return;
////            }

////            abp.ui.setBusy();

////            $.ajax({
////                url: '@Url.Action("PreviewHtml", "LegalDocuments")',
////                type: 'POST',
////                data: { fileToken: fileToken },
////                success: function (response) {
////                    abp.ui.clearBusy();

////                    if (response.success) {
////                        // Show the preview with metadata
////                        showHtmlPreview(response.html);

////                        // Add metadata to the preview modal if available
////                        if (response.fileName) {
////                            var metadataHtml = '<div class="metadata-info alert alert-light mt-3 mb-0">' +
////                                '<strong>File:</strong> ' + response.fileName + '<br>' +
////                                '<strong>Size:</strong> ' + formatBytes(response.fileSize) + '<br>';

////                            if (response.isSanitized) {
////                                metadataHtml += '<strong>Sanitized:</strong> Yes (Original: ' +
////                                    formatBytes(response.originalSize) + ', After: ' +
////                                    formatBytes(response.sanitizedSize) + ')<br>';
////                            } else {
////                                metadataHtml += '<strong>Sanitized:</strong> No changes needed<br>';
////                            }

////                            metadataHtml += '<strong>Last Modified:</strong> ' + response.lastModified +
////                                '</div>';

////                            $('#previewFrame').after(metadataHtml);
////                        }
////                    } else {
////                        abp.notify.error(response.error);
////                    }
////                },
////                error: function () {
////                    abp.ui.clearBusy();
////                    abp.notify.error(L('ErrorPreviewingHtml'));
////                }
////            });
////        }

////        // Helper function to format bytes
////        function formatBytes(bytes, decimals = 2) {
////            if (bytes === 0) return '0 Bytes';

////            const k = 1024;
////            const dm = decimals < 0 ? 0 : decimals;
////            const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];

////            const i = Math.floor(Math.log(bytes) / Math.log(k));

////            return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
////        }

////        $(document).ready(function () {
////            // Update file input label with selected filename
////            $('#FileUpload').on('change', function () {
////                var fileName = $(this).val().split('\\').pop();
////                $(this).next('.custom-file-label').html(fileName || '@L("ChooseFile")');

////                // Validate file type
////                var fileExtension = fileName.split('.').pop().toLowerCase();
////                var allowedExtensions = ['pdf', 'html', 'css'];

////                if (fileName && !allowedExtensions.includes(fileExtension)) {
////                    abp.notify.error(L('InvalidFileType'));
////                    $(this).val('');
////                    $(this).next('.custom-file-label').html('@L("ChooseFile")');
////                    return;
////                }

////                // Upload file when selected
////                if (fileName) {
////                    $('#uploadFeedback').removeClass('d-none');
////                    uploadFile(this.files[0]);
////                }
////            });

////            // Handle form submission
////            $('#LegalDocumentForm').on('submit', function (e) {
////                // Validate form
////                if ($('#LegalDocument_Type').val() === '') {
////                    e.preventDefault();
////                    abp.notify.error(L('PleaseSelectDocumentType'));
////                    return false;
////                }

////                // If external URL is empty and no file is selected/uploaded
////                var hasExternalUrl = $('#ExternalUrl').val().trim() !== '';
////                var hasFileId = '@Model.LegalDocument.FileId' !== '' && '@Model.LegalDocument.FileId' !== 'null';
////                var hasFileToken = $('#FileToken').val() !== '';

////                if (!hasExternalUrl && !hasFileId && !hasFileToken) {
////                    e.preventDefault();
////                    abp.notify.error(L('PleaseProvideFileOrExternalUrl'));
////                    return false;
////                }

////                // Form will be submitted normally with the FileToken
////            });

////            // Function to upload file and get token
////            function uploadFile(file) {
////                var formData = new FormData();
////                formData.append('file', file);

////                abp.ui.setBusy();

////                $.ajax({
////                    url: '@Url.Action("UploadLegalDocumentFile", "LegalDocuments")',
////                    type: 'POST',
////                    data: formData,
////                    processData: false,
////                    contentType: false,
////                    success: function (response) {
////                        abp.ui.clearBusy();
////                        $('#uploadFeedback').addClass('d-none');

////                        if (response.success) {
////                            // Set the file token to the hidden input
////                            $('#FileToken').val(response.result);
////                            abp.notify.success('@L("FileUploadedSuccessfully")')

////                            // If it's an HTML file, enable preview functionality
////                            var fileName = file.name;
////                            if (fileName.toLowerCase().endsWith('.html')) {
////                                // Add a preview button
////                                if (!$('#previewHtmlButton').length) {
////                                    var previewBtn = $('<button id="previewHtmlButton" type="button" class="btn btn-info mt-2"><i class="fa fa-eye"></i> ' + app.localize('PreviewHtml') + '</button>');
////                                    $('#uploadFeedback').after(previewBtn);
////                                    // Use ModalManager to open the modal
////                                    previewBtn.on('click', function () {
////                                        showPreviewModal(response.result);
////                                    });
////                                }
////                            }
////                        } else {
////                            abp.notify.error(response.error.message || L('FileUploadFailed'));
////                        }
////                    },
////                    error: function () {
////                        abp.ui.clearBusy();
////                        $('#uploadFeedback').addClass('d-none');
////                        abp.notify.error(L('FileUploadFailed'));
////                    }
////                });
////            }

////            // Replace preview button logic
////            function showPreviewModal(fileToken) {
////                _previewModal.open({ fileToken: fileToken });
////            }
////        });
////    }
////});