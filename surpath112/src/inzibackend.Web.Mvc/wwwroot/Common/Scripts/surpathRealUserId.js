var app = app || {};
(function () {


    app.realUserId = function () {

        //if (abp.session.impersonatorUserId != null) {
        //    return abp.session.impersonatorUserId;
        //}
        //else {
        //    return abp.session.userId;
        //}
        return abp.session.impersonatorUserId ?? abp.session.userId;
    }

})();
