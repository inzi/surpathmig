$(function () {
    // Surscan Working Filter
    // Note the event it's firing
    // the widget should listen for that.

    var _$container = $(".filter-surpath-department-container");
    var _$btn = _$container.find("button[name='btn-surpath-department']");
    var _$input = _$container.find("input[name='input-surpath-department']");

    _$btn.on('click', function () {
        abp.event.trigger('app.dashboardFilters.surpathDeptFilter.onNameChange', _$input.val());

    })

    _$btn.click(function () {
        console.log('clicked');
    });
});
