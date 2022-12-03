(function ($) {

    var counterDate;
    var numOfDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    var borrowed = 0, years = 0, months = 0, days = 0;
    var hours = 0, minutes = 0, seconds = 0;

    var timeout;

    $.fn.counterStart = function () {
        counterDate = new Date();
        updateNumOfDays();
        updateCounter(this);
        return this;
    };

    $.fn.counterStop = function () {
        clearTimeout(timeout);
    }

    function updateNumOfDays() {
        var dateNow = new Date();
        var currYear = dateNow.getFullYear();

        if ((currYear % 4 == 0 && currYear % 100 != 0) || currYear % 400 == 0) {
            numOfDays[1] = 29;
        }

        setTimeout(function () {
            updateNumOfDays();
        }, (new Date((currYear + 1), 1, 2) - dateNow));
    }

    function datePartDiff(then, now, MAX) {
        var diff = now - then - borrowed;
        borrowed = 0;
        if (diff > -1) return diff;
        borrowed = 1;
        return (MAX + diff);
    }

    function calculate() {

        var futureDate = counterDate > new Date() ? counterDate : new Date();
        var pastDate = counterDate == futureDate ? new Date() : counterDate;

        seconds = datePartDiff(pastDate.getSeconds(), futureDate.getSeconds(), 60);
        minutes = datePartDiff(pastDate.getMinutes(), futureDate.getMinutes(), 60);
        hours = datePartDiff(pastDate.getHours(), futureDate.getHours(), 24);
        days = datePartDiff(pastDate.getDate(), futureDate.getDate(), numOfDays[futureDate.getMonth()]);
        months = datePartDiff(pastDate.getMonth(), futureDate.getMonth(), 12);
        years = datePartDiff(pastDate.getFullYear(), futureDate.getFullYear(), 0);
    }

    function addLeadingZero(value) {
        return value < 10 ? ("0" + value) : value;
    }

    function formatTime() {
        seconds = addLeadingZero(seconds);
        minutes = addLeadingZero(minutes);
        hours = addLeadingZero(hours);
    }

    function updateCounter(elem) {
        calculate();
        formatTime();
        elem.html(hours + ":" + minutes + ":" + seconds);
        timeout = setTimeout(function () {
            updateCounter(elem);
        }, 1000);
    }

})(jQuery);