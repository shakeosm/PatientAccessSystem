
$("#SearchPatientButton").click(function () {

    var getUrl = "/Prescription/Get";

    $.ajax({
        url: getUrl,
        type: "GET",
        processData: false,  // Not to process data
        contentType: false,  // tell jQuery not to set contentType- bcoz its FormData
        data: {
            "id" : "123"
        },
        success: function (result) {
            ServiceFileUploadOnSuccess(result, uploadedFileListTable)
        },
        error: function (xhr, status, error) {
            ServiceFileUploadOnError(xhr, status, error);
        }
    });
});