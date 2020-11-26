
$("#SearchPatientButton").click(function () {

    $("#PrescriptionView").removeClass("invisible");
    return;

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
            //ServiceFileUploadOnSuccess(result, uploadedFileListTable)
        },
        error: function (xhr, status, error) {
            //ServiceFileUploadOnError(xhr, status, error);
        }
    });
});

//### LoaderBody- Spinning image
$("#PasLoaderbody").addClass('invisible');
$(document).bind('ajaxStart', function () {
    $("#PasLoaderbody").removeClass('invisible');
}).bind('ajaxStop', function () {
    $("#PasLoaderbody").addClass('invisible');
});


$("#LoadHtmlButton").click(function () {
    debugger;
    var prescriptionId = 1003;

    var URL = `/Home/GetPrescription_HTML/${prescriptionId}`;

    axios.get(URL)
        .then(function (response) {
            $("#LoadPrescriptionPreviewDiv").html(response.data);
        });


});
