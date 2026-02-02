(function ($) {
  $(function () {
    function scrollToCurrentMenuElement() {
      if (!$('#kt_aside_menu').length) {
        return;
      }

      var path = location.pathname;
      var menuItem = $("a[href='" + path + "']");
      if (menuItem && menuItem.length) {
        menuItem[0].scrollIntoView({ block: 'center' });
      }
    }
    
    function loadLegalDocumentUrls() {
      console.log('Calling loadLegalDocumentUrls in _Layout.js');
      if (abp && abp.services && abp.services.app && abp.services.app.legalDocuments) {
        abp.services.app.legalDocuments.getLegalDocumentUrls()
          .done(function(response) {
            console.log('Legal document URLs response:', response);
            var hasDocuments = false;
            
            if (response) {
              // Check for Terms of Service
              if (response.TermsOfService) {
                $('#terms-of-service-link')
                  .attr('href', response.TermsOfService)
                  .removeClass('d-none');
                hasDocuments = true;
              }
              
              // Check for Privacy Policy
              if (response.PrivacyPolicy) {
                $('#privacy-policy-link')
                  .attr('href', response.PrivacyPolicy)
                  .removeClass('d-none');
                hasDocuments = true;
              }
              
              if (hasDocuments) {
                const $footer = $('#legal-documents-footer');
                $footer.removeClass('d-none');
                console.log('Legal footer should be visible now');
                
                // Adjust the aside layout
                $('#kt_aside').addClass('has-legal-footer');
              }
            }
          })
          .fail(function(error) {
            console.error('Error fetching legal document URLs:', error);
          });
      } else {
        console.error('Legal documents service not available');
      }
    }
    
    // Initial setup
    scrollToCurrentMenuElement();
    
    // Load legal document URLs after a slight delay to ensure everything is initialized
    setTimeout(loadLegalDocumentUrls, 500);
  });
})(jQuery);
