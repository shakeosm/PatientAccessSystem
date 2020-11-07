$(document).ready(function () {

    $("#SearchPatientButton").click(function () {
        var getUrl = "/Patient/Home/Search";
        console.log("getUrl: " + getUrl);

        var firstName = $("#FirstName").val();
        var lastName = $("#LastName").val();
        var mobile = $("#Mobile").val();
        var shortId = $("#ShortId").val();

        $.ajax({
            url: getUrl,
            type: "GET",
            processData: false,
            contentType: false,
            data: {
                'firstName': firstName,
                'lastName': lastName,
                'mobile': mobile,
                'shortId': shortId
            },
            success: function (result) {
                logToConsole(result);
                $("#PatientSearchResultContainer").html(result);
                $("#PatientSearchResultWrapper").removeClass("d-none");
                $("#RegularPatientDiv").slideUp();

                //$('.data-table').DataTable({
                //});
            },
            error: function (err) {
                console.log(err.statusText);
            }
        });

        $("#PatientSearchResultContainer").removeClass("d-none")

       
        /*
                public int Id { get; set; }
        public string Name { get; set; }
        public string BanglaName { get; set; }
        public string Address { get; set; } **----
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string ShortId { get; set; }
 

         */
    });
});

//## Following 2 lines are required for Ajax call to include AntiForgeryToken; to be used in all ProviderClaim Forms (Create/Edit)
var form = $('#StartNewPrescriptionForm');
var token = $('input[name="__RequestVerificationToken"]', form).val();

$(document).on("click", "#PatientSearchTable .create-new-prescription-button", function (e) {
    //## Create FormData object
    var formData = new FormData();

    formData.append('id', 123);
    formData.append('__RequestVerificationToken', token);  //# AntiForgeryToken

    var postUrl = "";

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


/*
 debugger;
        $('#PatientSearchTable').DataTable({
            destroy: true,
            searching: false
        });

        $("#PatientSearchTable").DataTable({
            "processing": true,
            "serverSide": true,
            "filter": true,
            "ajax": {
                "url": formAction,
                "type": "GET",
                "datatype": "json"
            },
            "columnDefs": [{
                "targets": [0],
                "visible": false,
                "searchable": false
            }],
            "columns": [
                { "data": "id", "name": "Id", "autoWidth": true },
                { "data": "shortId", "name": "ShortId", "autoWidth": true },
                { "data": "name", "name": "Full Name", "autoWidth": true },
                { "data": "banglaName", "name": "Bangla Name", "autoWidth": true },
                { "data": "gender", "name": "Gender", "autoWidth": true },
                { "data": "dateOfBirth", "name": "Date of Birth", "autoWidth": true },
                { "data": "age", "name": "Age", "autoWidth": true },
                {
                    "render": function (data, row) {
                        return "<a href='/Doctor/Prescription/Create/" + row.id + "' class='btn btn-general btn-primary'); >Delete</a>";
                    }
                },
            ]
        });
 */