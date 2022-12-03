function toggle(source) {
    var isvisible = $(source).attr('checked');
    var checkboxes = $("input[id^='chkMedia_']");
    checkboxes.attr('checked', false);

    $("input[id^='chkMedia_']:visible").attr('checked', isvisible);
    checkboxes.each(function (idx, elem) {
        showSocietes(elem);
    });

}
function showSocietes(source) {

    var chk_societeId = source.value;

    var tdToColor = $("td[id^='Pup'][value='" + chk_societeId + "']");
    $(tdToColor).css('background-color', source.checked ? '#ff0000' : '#ff9900');
    //if (($(tdToColor).attr('bgcolor') != '#f00') | ($(tdToColor).attr('bgcolor') != '#ff9900'))
    //    $(tdToColor).fadeOut().fadeIn();
}

function checkEnter(e) {
    e = e || event;
    var txtArea = /textarea/i.test((e.target || e.srcElement).tagName);
    return txtArea || (e.keyCode || e.which || e.charCode || 0) !== 13;
}

(function ($) {


    // custom css expression for a case-insensitive contains()
    jQuery.expr[':'].Contains = function (a, i, m) {
        return (a.textContent || a.innerText || "").toUpperCase().indexOf(m[3].toUpperCase()) >= 0;
    };
   

}(jQuery));


