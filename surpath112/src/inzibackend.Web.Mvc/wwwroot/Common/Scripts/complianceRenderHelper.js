var app = app || {};
(function () {
    app.complianceRender = app.complianceRender || {

        render: function (total, complianceSummary) {


            var _statbar = $('<div class="statbar"/>');
            var _statsubs = [];
            var _allWidths = 0
            var tots = 0;
            //debugger;
            $(complianceSummary).each(function (i, r) {



                //var _percentage = parseInt((r.count / total) * 1000)/10;
                //var _percentage = parseInt((r.count / total) * 100);
                //var _percentage = ((r.count / total) * 1000000) / 10000;


                var _percentage = Math.floor((r.count / total)*1000)/10;
                //var _percentage = ((r.count / total) * 100).toFixed();
                var _oldAllWidths = _allWidths;
                _allWidths = (_allWidths + _percentage);
                //_allWidths = Math.floor(10 * (_allWidths)) / 10;
                //console.log(r.count + ' ' + total + ' ' + _percentage + ' ' + _allWidths + ' ' + _oldAllWidths + ' ' + tots);
                if (i == complianceSummary.length - 1) {
                   // if (_allWidths < 100) {
                    //    _percentage = _percentage + 1; // in case we have an odd percentage, we expand the last one
                    //    _percentage = 100 - _oldAllWidths;
                    //}
                    //if (_allWidths > 100) {
                    _percentage = Math.floor(10*(100 - _oldAllWidths))/10;
                    //}
                    _allWidths = _oldAllWidths + _percentage;
                    //console.log('last ' + r.count + ' ' + total + ' ' + _percentage + ' ' + _allWidths + ' ' + _oldAllWidths + ' ' + tots);
                }
                var _statsub = $('<div />');
                _statsub.addClass('statbarsub');
                _statsub.addClass('statbarmid');
                _statsub.css('width', _percentage + '%');
                _statsub.css('background', r.htmlColor);
                _statsubs.push(_statsub);
                tots = tots + _percentage;
            });
            //console.log('tots ' + tots);
            ((_statsubs[0]).addClass('statbarleft')).removeClass('statbarmid');
            _statsubs[_statsubs.length - 1].addClass('statbarright').removeClass('statbarmid');
            _statbar.append(_statsubs);
            var retval = _statbar[0].outerHTML;
            //console.log(retval);
            return retval;
        },
    };
})();
