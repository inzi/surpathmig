var app = app || {};
(function () {

    app.daterangefilterhelper = app.daterangefilterhelper || {
        fixfilters: function () {
            $('.datetime-picker[id$=FilterId],.date-picker[id$=FilterId]').each(function (i,obj) {
                app.daterangefilterhelper.setup(obj);
            });
        },
        setup: function (obj) {

            var options = {};


            if ($(obj).hasClass('date-picker'))
            {
                options = {
                    singleDatePicker: true,
                    timePicker: false,
                    locale: {
                        cancelLabel: 'Clear',
                        format: 'MM/DD/YYYY',
                    },
                    autoUpdateInput: false,
                    startDate: moment().startOf('day')
                }
            };

            if ($(obj).hasClass('datetime-picker'))
            {
                options = {
                    singleDatePicker: true,
                    timePicker: true,
                    locale: {
                        cancelLabel: 'Clear',
                        format: 'MM/DD/YYYY hh:mm A',
                    },
                    autoUpdateInput: false,
                    startDate: moment(),
                };
            };


            $(obj).daterangepicker(options)
                .on("apply.daterangepicker", function (e, picker) {
                    picker.element.val(picker.startDate.format(picker.locale.format));
                })
                .on('cancel.daterangepicker', function (e, picker) {
                    $(this).val('');
                })
                .val('');

        }
    };

})();
