(function ($) {
    // This script is loaded by ModalManager when the modal is opened
    abp.event.on('modal.shown', function (e) {
        var $modal = $(e.target);
        if (!$modal.hasClass('LegalDocumentPreviewModal')) {
            return;
        }
        // If the server passes the fileToken in the model, you can use it here if needed
        var fileToken = $modal.find('[name="FileToken"]').val();
        // Optionally, you can fetch and display the preview content here if not already rendered
        // For now, assume content is already rendered in the modal

        // Download button logic
        $modal.find('#downloadPreviewBtn').off('click').on('click', function () {
            var htmlContent = $modal.find('#previewFrame').html();
            var blob = new Blob([htmlContent], { type: 'text/html' });
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = 'document_preview.html';
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        });
    });
})(jQuery); 