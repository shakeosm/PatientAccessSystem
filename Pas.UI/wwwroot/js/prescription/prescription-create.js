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

    //## Plugins- Initiatethe Select2 Plugins
    $('#DrugListSelect').select2();
    $('#DiagnosisListSelect').select2();
    $('#ChiefComplaintList').select2({
        placeholder: "Select CC"
    });

    //### LoaderBody- Spinning image
    $("#loaderbody").addClass('invisible');
    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('invisible');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('invisible');
    });

    //$('.data-table').DataTable({
    //});


    $('#ChiefComplaintList').on('select2:select', function (e) {

        debugger;
        var selectedVal = $(this).find(':selected').val();
        alert("selectedVal : " + selectedVal);

        //var data = e.params.data;
        //console.log(data);
    });

    $('#DiagnosisListSelect').on('select2:select', function (e) {

        debugger;
        var selectedVal = $(this).find(':selected').val();
        var selectedText = $(this).find(':selected').text();
        alert("selectedVal : " + selectedVal + " - " + selectedText);
    });

    $(".drug-intake-radio-option").click(function () {
        if ($(this).hasClass("drug-intake-solid")) {
            if ($("#DrugStrengthRowDiv").is("visible") == false) {
                $("#DrugStrengthRowDiv").slideDown();
                $("#DoseSelectionDiv").slideDown();
                //$("#DoseSolidSelect").prop("disabled", false); //## For- Table/Capsule- show the Dosage Quantity
            }
        } else {
            $("#DrugStrengthRowDiv").slideUp();
            $("#DoseSelectionDiv").slideUp();
            //$("#DoseSolidSelect").prop("disabled", true); //## For- Liquid, Gel- disable the Dosage Quantity
        }

        //$("#DoseSolidSelect").val("1");
    });


    //Popup Modal Windows
    $("#AddNewDrugBrandButton").click(function() {
        $("#NewDrugBrandEntryPopupModal").modal("show");
    });

    $("#AddNewDrugInstructionButton").click(function() {
        $("#NewDrugAdviseInstructionPopupDiv").modal("show");
    });

    $("#AddNewDrugDosageTemplateButton").click(function() {
        $("#NewDrugDoseTemplatePopupModal").modal("show");
    });

    $("#PatientVitalEntryModalButton").click(function() {
        $("#PatientVitalsModalPopup").modal("show");
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