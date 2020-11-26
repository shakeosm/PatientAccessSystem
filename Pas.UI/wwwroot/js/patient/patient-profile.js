moment.locale('en-GB'); //.tz('Europe/London');

//## Following 2 lines are required for Ajax call to include AntiForgeryToken; to be used in all ProviderClaim Forms (Create/Edit)
var form = $('#PatientProfileForm');
var token = $('input[name="__RequestVerificationToken"]', form).val();
var _dateOfBirth = "";
var _dateOfBirth_YMD  = "";
var _patientId = GetValue("UserId");
$(document).ready(function () {


});

$("#ShowPersonalHistoryModalButton").click(function () {
    $("#PatientLifeHistoryModal").modal("show");
    $(this).parent().find(".last-updated-info").text("Last updated: Today");
});


$("#PersonalHistotyModalSubmitButton").click(function (e) {
    debugger;

    if(!IsDataValid_PersonalHistory()) {
        e.preventDefault();
        return false;
    }

    //## Create FormData object
    var formData = new FormData();

    formData.append('__RequestVerificationToken', token);  //# AntiForgeryToken
    formData.append('PatientId', _patientId);
    formData.append('BloodGroup', GetValue("BloodGroupInputSelect"));
    formData.append('DateOfBirth', _dateOfBirth_YMD);
    formData.append('SmokePerDay', GetValue("SmokePerDayInput"));
    formData.append('Drinker', GetValue("DrinkHabitInputSelect"));
    formData.append('Excercise', IsChecked("ExcerciseSelectionCheckbox"));
    formData.append('Sports', GetValue("SportsInputSelect"));
    //formData.append('Weight', GetValue());

    var postUrl = "/Patient/Home/UpdatePersonalHistory";

    $.ajax({
        url: postUrl,
        type: "POST",
        processData: false,  // Not to process data
        contentType: false,  // tell jQuery not to set contentType- bcoz its FormData
        data: formData,
        success: function (result) {
            if (result === "success") {
                OnSuccess_UpdatePersonalHistory();
            }
            else {
                OnFail_UpdatePersonalHistory("Error updating",
                    "There was an error updating Patient's Personal Details. Please reload this page and try again.");
            }
            console.log("PatientSearchTable=> Success");
        },
        error: function (xhr, status, error) {
            OnFail_UpdatePersonalHistory("Error updating",
                "There was an error updating Patient's Personal Details. Please reload this page and try again.<br/>Error: " + error);
        }
    });
});

function OnSuccess_UpdatePersonalHistory() {
    $("#PersonalHistoryDetailsDiv .blood-group").text($("#BloodGroupInputSelect option:selected").text());
    $("#PersonalHistoryDetailsDiv .date-of-birth").text(_dateOfBirth);
    $("#PersonalHistoryDetailsDiv .smoke-unit-per-day").text(GetValue("SmokePerDayInput"));    
    $("#PersonalHistoryDetailsDiv .drink-habit").text($("#DrinkHabitInputSelect option:selected").text());

    var excercise = IsChecked("ExcerciseSelectionCheckbox") ? "Yes" : "No";
    $("#PersonalHistoryDetailsDiv .excercise").text(excercise);

    $("#PersonalHistoryDetailsDiv .sports").text($("#SportsInputSelect option:selected").text());

    $("#PatientLifeHistoryModal").modal("hide");
    
    $("#PersonalHistoryLastUpdatedDiv").text("Last updated: now");
}

function OnFail_UpdatePersonalHistory(failTitle, failText) {
    swal({
        title: failTitle,
        text: failText,
        icon: "warning",
    });
}


function IsDataValid_PersonalHistory() {
    
    _dateOfBirth = GetValue("AgeInput_DD") + "-" + GetValue("AgeInput_MM") + "-" + GetValue("AgeInput_YYYY");
    _dateOfBirth_YMD = moment(_dateOfBirth, "DD-MM-YYYY", true).format("YYYY-MM-DD");

    var patientAge = moment().diff(_dateOfBirth_YMD, 'years');
    var patientAgeInHours = moment().diff(_dateOfBirth_YMD, 'hours');
    
    if (!IsDateValid(_dateOfBirth)) {
        swal({
            title: "Invalid date",
            text: "You must enter a valida date- for Date of Birth!",
            icon: "warning",
        });
        return false;
    }
    if (patientAge > 110 || patientAgeInHours < 0) {
        swal({
            title: _dateOfBirth + " is Invalid",
            text: "Patient age must be less than 110 years and Date of birth cannot be a future date.",
            icon: "warning",
        });
        return false;
    }

    var smokesPerDay = GetValue("SmokePerDayInput");
    if (smokesPerDay > 50) {
        swal({
            title: smokesPerDay + " is Excessive amount",
            text: "Smoking: Please keep below 50 units per day",
            icon: "warning",
        });
        return false;
    }

    return true;
}

$("#ContactDetailsEditButton").click(function () {
    $("#ContantDetailsCardModal").modal("show");
});

$("#ContactDetailsUpdateForm").validate({
    rules: {
        AddressLine1: {
            required: true,
            minlength: 3
        },
        LocalArea: {
            required: true,            
        },
        CityId: {
            required: true,
            min: 1
        },
    }
});

$("#PersonalHistoryModalSubmitButton").click(function () {
    debugger;

    //$("#ContactDetailsUpdateForm").validate();
    var validator = $("#ContactDetailsUpdateForm").validate();
    validator.form();

    // If the form isn't valid, prevent the post
    if (!$("#ContactDetailsUpdateForm").valid()) {
        return false;
    }

    e.preventDefault();

});

$("#EditAllergryListButton").click(function () {


    //const { value: allergyList } = Swal.fire({
    //    title: 'Enter allergy items, separate by comma \',\' ',
    //    input: 'text',
    //    inputLabel: 'Allergy info',
    //    inputValue: 'some value, more value',
    //    showCancelButton: true,
    //    inputValidator: (value) => {
    //        if (!value) {
    //            return 'You need to write something!'
    //        }
    //    }
    //})

    //if (allergyList) {
    //    Swal.fire(`Your list is: ${allergyList}`)
    //}
});

function IsDateValid(dateValue) {
    return moment(dateValue, "DD-MM-YYYY", true).isValid();
}

function GetValue(elementId) {
    return $("#" + elementId).val();
}

function IsChecked(elementId) {
    return $("#" + elementId).is(":checked");
}