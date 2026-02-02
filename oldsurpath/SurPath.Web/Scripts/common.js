
function IsRequired(ID, errAttr) {
    console.log('IsRequired');
    if (typeof (errAttr) == "undefined") {
        errAttr = "data-errormsg";
    }

    if (ID.length > 0) {
        var status = true;
        $.each(ID.split(','), function (i, v) {
            console.log(v);
            var e = $("#" + v);
            if (e != null && e.length > 0 && ($.trim(e.val()) == "" || $.trim(e.val()) == "___-__-____") && e.is(":visible")) {
                status = false;
                e.css("borderColor", "red");
                e.next().show().html(e.attr(errAttr));
            }
            else {
                e.css("borderColor", "");
                e.next().hide();
            }
        });
        return status;
    }
    
}

function ClearBorder(ID) {
    if (ID.length > 0) {
        var status = true;
        $.each(ID.split(','), function (i, v) {
            var e = $("#" + v);
            e.css("borderColor", "");
            if (e.next().is(".alert-danger")) {
                e.next().hide();
            }
        });
    }
}

function IsEmailValid(ID) {    
    var emailReg = new RegExp(/^(("[\w-\s]+")|([\w-]+(?:\.[\w-]+)*)|("[\w-\s]+")([\w-]+(?:\.[\w-]+)*))(@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][0-9]\.|1[0-9]{2}\.|[0-9]{1,2}\.))((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){2}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\]?$)/i);
    //var emailReg = new RegExp();
    var e = $("#" + ID);
    if (!emailReg.test(e.val())) {
        e.css("borderColor", "red");
        return false;
    }
    e.css("borderColor", "");
    return true;
}

function IsMatch(id1, id2) {
    if ($("#" + id1).val() != $("#" + id2).val()) {
        $("#" + id2).css("borderColor", "red");
        return false;
    }
    return true;
}

//$(window).resize(function () {
//    if ($(this).height() >= 1080) {
//        // change the dataTable pageLength in here
//        $('#searhtable').page.len(50).draw();
//    } else {
//        // default pageLength
//        $('#searchtable').page.len(6).draw();
//    }
//});

       