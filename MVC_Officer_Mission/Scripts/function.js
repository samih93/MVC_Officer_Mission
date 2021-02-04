
var DataTablesLang = {
    "lengthMenu": "عرض _MENU_ سجل في كل صفحة",
    "zeroRecords": "عذراً لا يوجد نتائج",
    "info": "عرض الصفحة رقم _PAGE_ من أصل _PAGES_",
    "infoEmpty": "لا يوجد نتائج",
    "infoFiltered": "(مصفاة من _MAX_ سجل)",
    "paginate": {
        "first": "الأول",
        "previous": "السابق",
        "next": "التالي",
        "last": "الأخير"
    },
    "sSearch": "",
    "sSearchPlaceholder": " بحث "
};

$(document).ready(function () {
    //hide by default
    var tournamentContent = jQuery(".tournamentContent");
    var missionOfficersWrapper = jQuery(".missionOfficersWrapper");
    tournamentContent.hide();
    if ($("#Mission_Istournament").is(":checked") == true) {
        tournamentContent.fadeIn();
        missionOfficersWrapper.fadeOut();
    }
    $('#Mission_Istournament').change(function () {
        if (this.checked) {
            tournamentContent.fadeIn();
            missionOfficersWrapper.fadeOut();
        } else {
            tournamentContent.fadeOut();
            missionOfficersWrapper.fadeIn();
        }
    });
});


$(document).ready(function () {
    if ($(".datepicker").length > 0) {
        $(".datepicker").each(function () {
            var id = $(this).attr("id");

            $("#" + id).datepicker({
                dateFormat: "yy-mm-dd",
                changeYear: true,
                changeMonth: true,
                stepMonths: 12,
                yearRange: "1950:2060",
                showAnim: "fadeIn",
                showButtonPanel: true,
                currentText: 'اليوم',
                nextText: '<<',
                prevText: '>>',
                closeText: 'إغلاق',
                autoSize: false,
                buttonImage: "/images/datepicker.gif",
                monthNames: ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"],
                monthNamesShort: ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"],
                dayNamesMin: [" الأحد ", "إثنين", "ثلاثاء", "أربعاء", "خميس", "جمعة", "سبت"],
                firstDay: 1,
                beforeShow: function (input, inst) {
                    $(".ui-datepicker-month").addClass('attet');
                    $(".ui-datepicker-year").addClass('attet2');
                }
            });
        })
    }

    //if ($(".datepicker").length > 0) {
    //    $('.datepicker').datetimepicker({
    //        language: 'pt-br'
    //    });

    //}

})

$(document).ready(function () {
    if ($('.selectpicker').length > 0) {
        $('.selectpicker').selectpicker({
            liveSearch: true,
            showSubtext: true
        });
    }
});

//checks if strDate is in between strFrom and strTo
function dateBetween(date, from, to) {
    return from <= date && date <= to;
}
function treatAsUTC(date) {
    var result = date;
    result.setMinutes(result.getMinutes() - result.getTimezoneOffset());
    result.setHours(0, 0, 0, 0);
    return result;
}
//returns int days between 2 dates
function daysBetween(startDate, endDate) {
    var result;
    var millisecondsPerDay = 24 * 60 * 60 * 1000;
    if (parseInt(((treatAsUTC(endDate) - treatAsUTC(startDate)) / millisecondsPerDay)) < ((treatAsUTC(endDate) - treatAsUTC(startDate)) / millisecondsPerDay)) {
        result = parseInt(((treatAsUTC(endDate) - treatAsUTC(startDate)) / millisecondsPerDay)) + 2;
    }
    else {
        result = parseInt(((treatAsUTC(endDate) - treatAsUTC(startDate)) / millisecondsPerDay)) + 1;
    }
    return result;
}

$(document).ready(function () {
    $(".btn-pref .btn").click(function () {
        $(".btn-pref .btn").removeClass("btn-primary").addClass("btn-default");
        // $(".tab").addClass("active"); // instead of this do the below 
        $(this).removeClass("btn-default").addClass("btn-primary");
    });
});

