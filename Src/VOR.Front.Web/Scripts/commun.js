if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
              ? args[number]
              : match
            ;
        });
    };
}

(function ($) {

    $.fn.detectChanges = function (callback) {
        var _this = this;

        _this.addClass("detectChanges");

        _this.on('change', 'input, checkbox, textarea, radio, select', function () {
            _this.setDirty();
            callback();
        });

        _this.on('input', 'input, textarea', function () {
            _this.setDirty();
            callback();
        });

        return _this;
    };

    $.fn.setDirty = function () {
        if (this.hasClass("detectChanges"))
            this.attr("data-dirty", "1");
        else
            this.closest(".detectChanges").attr("data-dirty", "1");
    };

    $.fn.setClean = function () {
        this.attr("data-dirty", "0");
    };

    $.fn.isDirty = function () {
        if (this.attr("data-dirty") && this.attr("data-dirty") === "1")
            return true;

        if (this.find("[data-dirty='1']").length > 0)
            return true;

        return false;
    };

}(jQuery));
