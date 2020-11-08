//
// Bootstrap Datepicker
//

'use strict';

var Datepicker = (function () {

    var $datepicker = $('.datepicker');

    function init($this) {
        var options = {
            disableTouchKeyboard: true,
            autoclose: false
        };

        $this.datepicker(options);
    }


    if ($datepicker.length) {
        $datepicker.each(function () {
            init($(this));
        });
    }

})();

//
// Tooltip
//
var Tooltip = (function () {
    var $tooltip = $('[data-toggle="tooltip"]');

    function init() {
        $tooltip.tooltip();
    }

    if ($tooltip.length) {
        init();
    }

})();


var development = true;

function logToConsole(object) {
    if (development)
        console.log(object);
}

//## This variable will be used Gloablly for all Calendar Control. One place - one head, one ache!
window.calendarFormat = "DD MMM YYYY";

$(document).ready(function () {

    $("#loaderbody").addClass('invisible');
    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('invisible');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('invisible');
    });

    //tempus dominus datepicker
    $('.datepicker').each(function (i) {
        $(this).datetimepicker(
            {
                //date: moment($(this).children('.datetimepicker-input').prop('defaultValue'), "DD-MM-YYYY HH:mm:ss" ),
                format: calendarFormat,
                autoclose: true
            })
    }).focusin(function () {
        $(this).datetimepicker('show');
    });

    $('.data-table').DataTable({
    });

});


///** Common JavaScript Methods **/
/* Element Visibility */
function showElement(element) { $(element).removeClass('d-none'); } //## Used in AccessRequest.js
function hideElement(element) { $(element).addClass('d-none'); }

/* String Functions */
var toTitleCase = function (str) {
    return str.replace(/\w\S*/g,
        function (txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
};


/* HTTP Requests */
var httpMethods = {
    GET: 'GET',
    POST: 'POST',
    PUT: 'PUT',
    DELETE: 'DELETE'
}

function httpReq(type, url, successFunc, data) {
    if (data === undefined) data = {};

    $.ajax({
        type: type,
        url: url,
        data: data,
        dataType: "json",
        success: successFunc,
        error: function (xhr, status, error) {
            // handle failure
            console.log('ERROR');
            console.log(xhr);
            console.log(status);
            console.log(error);
        }
    });
}

function IsDateValid(dateValue) {
    return moment(dateValue, calendarFormat).isValid();
}
