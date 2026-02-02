(function ($) {
    if (!$) {
        return;
    }



    app.emailtousername = function () {
        //console.log('emailtousername activated');

        //$('.surpath-emailtousername').on('change', function (e) {
        //    $('.surpath-usernamefromemail').val($('.surpath-emailtousername').val());
        //});
        //$('.surpath-emailtousername').on('keyup', function (e) {
        //    $('.surpath-usernamefromemail').val($('.surpath-emailtousername').val());
        //});

        //$('.surpath-usernamefromemail').on('change', function (e) {
        //    $('.surpath-emailtousername').val($('.surpath-usernamefromemail').val());
        //});
        //$('.surpath-usernamefromemail').on('keyup', function (e) {
        //    $('.surpath-emailtousername').val($('.surpath-usernamefromemail').val());
        //});
    }

    $.when($.ready).then(function () {
        $('.surpath-emailtousername').on('change', function (e) {
            $('.surpath-usernamefromemail').val($('.surpath-emailtousername').val());
        });
        $('.surpath-emailtousername').on('keyup', function (e) {
            $('.surpath-usernamefromemail').val($('.surpath-emailtousername').val());
        });

    });

    

})(jQuery);






