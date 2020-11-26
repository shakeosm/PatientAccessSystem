//## Following 2 lines are required for Ajax call to include AntiForgeryToken; to be used in all ProviderClaim Forms (Create/Edit)
var form = $('#DoctorProfileForm');
var token = $('input[name="__RequestVerificationToken"]', form).val();

$(document).ready(function () {
    $("#PasLoaderbody").hide();

});


//## Edit Header: English
$(".edit-header-text-button").click(function () {
    //## For Edit- enable the Input boxes
    $(this).parent().find(".header-input-boxes").prop("disabled", false);
    $(this).parent().find(".update-header-text-button").removeClass("d-none");    //## Show the Update Button

    $(this).addClass("d-none"); //## Hide this Edit button    
});

//## Save Header: English
$(".update-header-text-button").click(function () {
    debugger;
    var registrationNumber = GetValue("RegistrationNumber");;
    var headerEnglish = GetValue("HeaderEnglish");
    var headerBangla = GetValue("HeaderBangla");

    //## Create FormData object
    var formData = new FormData();

    formData.append('__RequestVerificationToken', token);  //# AntiForgeryToken
    formData.append('RegistrationNumber', registrationNumber);
    formData.append('HeaderEnglish', headerEnglish);
    formData.append('HeaderBangla', headerBangla);

    var postUrl = "/Doctor/Profile/UpdatePrescriptionHeader";    


    //## After saving- Disable the Editable Input boxes
    $(this).parent().find(".header-input-boxes").prop("disabled", "disabled");

    $(this).parent().find(".edit-header-text-button").removeClass("d-none");    //## Show the Edit Button again
    $(this).addClass("d-none"); //## Hide this Update button                

    return;

    $.ajax({
        url: postUrl,
        type: "POST",
        processData: false,  // Not to process data
        contentType: false,  // tell jQuery not to set contentType- bcoz its FormData
        data: formData,
        success: function (result) {
            if (result === "success") {
                debugger;
                //## After saving- Disable the Editable Input boxes
                $(this).parent().find(".header-input-boxes").prop("disabled", "disabled");

                $(this).parent().find(".edit-header-text-button").removeClass("d-none");    //## Show the Edit Button again
                $(this).addClass("d-none"); //## Hide this Update button                
            }
            else {
                OnFail_AjaxPostCall("Error updating",
                    "There was an error updating Prescription Header Details. Please reload this page and try again.");
            }
            console.log("PatientSearchTable=> Success");
        },
        error: function (xhr, status, error) {
            OnFail_AjaxPostCall("Error updating",
                "There was an error updating Prescription Header Details. Please reload this page and try again.<br/>Error: " + error);
        }
    });


});


function OnFail_AjaxPostCall(failTitle, failText) {
    swal({
        title: failTitle,
        text: failText,
        icon: "warning",
    });
}

function Show(elementId) {
    $(elementId).removeClass("d-none");
}

function Hide(elementId) {
    $(elementId).addClass("d-none");
}

function OnFail_UpdateData(failTitle, failText) {
    swal({
        title: failTitle,
        text: failText,
        icon: "warning",
    });
}

function GetValue(elementId) {
    return $("#" + elementId).val();
}

function IsChecked(elementId) {
    return $("#" + elementId).is(":checked");
}