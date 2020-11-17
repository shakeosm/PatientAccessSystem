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


//## This variable will be used Gloablly for all Calendar Control. One place - one head, one ache!
window.calendarFormat = "DD MMM YYYY";

$(document).ready(function () {

    $('#DrugListSelect').select2();

    $("#loaderbody").addClass('invisible');
    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('invisible');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('invisible');
    });

    //$('.data-table').DataTable({
    //});


    $(".drug-intake-radio-option").click(function () {
        if ($(this).hasClass("drug-intake-solid")) {
            if ($("#DrugStrengthRowDiv").is("visible") == false) {
                $("#DrugStrengthRowDiv").slideDown();
                //$("#DoseSolidSelect").prop("disabled", false); //## For- Table/Capsule- show the Dosage Quantity
            }
        } else {
            $("#DrugStrengthRowDiv").slideUp();
            //$("#DoseSolidSelect").prop("disabled", true); //## For- Liquid, Gel- disable the Dosage Quantity
        }

        //$("#DoseSolidSelect").val("1");
    });

    $("#RelatedToMealCheckBox").click(function () {
        if ($(this).is(":checked")) {
            $("#MealsBeforeAfterOptionsDiv").removeClass("invisible");
            $(this).siblings('label').html('Yes');
        } else {
            $("#MealsBeforeAfterOptionsDiv").addClass("invisible");
            $(this).siblings('label').html('No');
        }
    });

    $("#NextStepSummaryButton").click(function () {
        $("#PrescriptionSummaryNotesDiv").removeClass("d-none");
        $("#PrescriptionItemEntryTopDiv").slideUp();
    });

    $("#PreviousStepButton").click(function () {
        $("#PrescriptionItemEntryTopDiv").slideDown();
        $("#PrescriptionSummaryNotesDiv").addClass("d-none");
    });

});