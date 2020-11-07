//## Following 2 lines are required for Ajax call to include AntiForgeryToken; to be used in all ProviderClaim Forms (Create/Edit)
var form = $('#StartNewPrescriptionForm');
var token = $('input[name="__RequestVerificationToken"]', form).val();

$("#SearchPatientButton").click(function () {
    //## Create FormData object
    var formData = new FormData();

    formData.append('id', 123);
    formData.append('__RequestVerificationToken', token);  //# AntiForgeryToken

    var postUrl = "/Doctor/Home/StartNewPrescription";

    $.ajax({
        url: postUrl,
        type: "POST",
        processData: false,  // Not to process data
        contentType: false,  // tell jQuery not to set contentType- bcoz its FormData
        data: formData,
        success: function (result) {
            ServiceFileUploadOnSuccess(result, uploadedFileListTable)
        },
        error: function (xhr, status, error) {
            ServiceFileUploadOnError(xhr, status, error);
        }
    });
});