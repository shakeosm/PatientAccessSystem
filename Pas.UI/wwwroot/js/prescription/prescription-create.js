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

var prescriptionId = -1;

$(document).ready(function () {
    prescriptionId = $("[name='PrescriptionIdInput']").val();

    //## Following 2 lines are required for Ajax call to include AntiForgeryToken; to be used in all ProviderClaim Forms (Create/Edit)
    var form = $('#PrescriptionCreateForm');
    var token = $('input[name="__RequestVerificationToken"]', form).val();

    //### LoaderBody- Spinning image
    $("#PasLoaderbody").addClass('invisible');
    $(document).bind('ajaxStart', function () {
        $("#PasLoaderbody").removeClass('invisible');
    }).bind('ajaxStop', function () {
        $("#PasLoaderbody").addClass('invisible');
    });

    //## Plugins- Initiate the Select2 Plugins
    //$('#DrugListSelect').select2({        placeholder: "Select a Generic drug"    });

    $('#IndicationSelectList').select2();

    $('#DiagnosisListSelect').select2({
        placeholder: "Select a Diagnosis"
    });

    $('#ChiefComplaintList').select2({
        placeholder: "Select CC"
    });


    //$('.data-table').DataTable({
    //});

    //## Change of 'Chief Complaint' Dropdown box
    $('#ChiefComplaintList').on('select2:select', function (e) {

        //debugger;
        //var selectedVal = $(this).find(':selected').val();
        //alert("selectedVal : " + selectedVal);

        //var data = e.params.data;
        //console.log(data);
    });


    //## Change of 'Diagnosis/Indications' Dropdown box
    $('#DiagnosisListSelect').on('select2:select', function (e) {

        var selectedVal = $(this).find(':selected').val();
        var selectedText = $(this).find(':selected').text();
        //alert("selectedVal : " + selectedVal + " - " + selectedText);

        //## Create a new RadioButton and add to the RadioButtonGroup
        var newDiagnosis = "<label class='btn btn-outline-danger waves-effect waves-themed diagnosis-radio-option-label'>" +
            "<input type='radio' class='diagnosis-radio-option' name='DiagnosisOptions' id='DiagnoisisOption_" + selectedVal + "' value='" + selectedVal +"' /> " + selectedText +
            //"<span class='delete-selected-diagnosis p-2'><i class='fal fa-minus-circle fa-lg ml-2' ></i></span>" +
            "</label >";
        $("#SelectedDiagnosisOptions").append(newDiagnosis);
    });

    //## Remove a Diagnosis from selection
    $("#RemoveDiagnosisLinkButton").click(function (e) {
        //var selectedVal = $('#DiagnosisListSelect').find(':selected').val();

        if ($('#SelectedDiagnosisOptions').find(".active").length > 0) {
            $('#SelectedDiagnosisOptions').find(".active").remove();    //## Remove the selected Diagnosis Radio Item
            $("#BrandListSelect").empty()                               //## Empty the DrugBrand list box
        }

        e.preventDefault();
    });


    //### Show Drug Brands per Diagnosis. When selecting a Diagnosis- show the possible list of Drug Brands.. make the life easier
    $(document).on("click", ".diagnosis-radio-option", function (e) {        

        var diagnosisId = $(this).val();
        var URL = `/Doctor/Prescription/ListAllBrandsForDiagnosis/${diagnosisId}`;
        
        var brandList = $("#BrandListSelect");
        brandList.empty();
        
        axios.get(URL)
            .then(function (response) {
                for (i = 0; i < response.data.length; i++) {
                    $('#BrandListSelect').append("<option value=" + response.data[i].id + ">" + response.data[i].name + "</option>");
                }
            });

    });

    function OnSuccessGetDrugsByDiagnosis(data) {
        debugger;
        console.log("Inside: OnSuccessGetDrugsByDiagnosis()");

        console.log(data);
        alert("response.data: " + data.name + ", Id: " + data.id);

        $("#SelectedDiagnosisOptions").append(data.name);

        for (i = 0; i < data.length; i++) {
            alert("inside loop.... response.data: " + data.name + ", Id: " + data.id);
            $('#BrandListSelect').append("<option value=" + data[i].id + ">" + data[i].name + "</option>");
        }
    }

    //## Drug Brand Selection Change-> Load all DrugDoseTemplates
    $("#BrandListSelect").click(function() {

        var drugBrandId = $(this).val();
        if (drugBrandId === undefined || drugBrandId < 1)
            return;

        var URL = `/Doctor/Prescription/ListAllBrandsDoseTemplates/${drugBrandId}`;

        var brandDoseTemplateList = $("#DrugDoseTemplateListSelect");
        brandDoseTemplateList.empty();

        axios.get(URL)
            .then(function (response) {
                debugger;
                if (response.data !== "error") {
                    for (i = 0; i < response.data.length; i++) {
                        $('#DrugDoseTemplateListSelect').append("<option value=" + response.data[i].templateId + ">" + response.data[i].templateName + "</option>");
                    }
                }
            });

        //## Update the Preview/sample area- showing the selected drug name

    });

    //## In DrugDose Template Modal Popup- selecting Tablet/Capsule/Syrup and show/hide Drug strength input box
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


    //## Add- New Drug Brand - opens Popup Modal Windows
    $("#AddNewDrugBrandButton").click(function () {
        //## If any Diagnosis is selected and a DiagnosisRadioOption is selected- then show Modal to create a new DrugBrand for that Diagnosis
        if ($('#SelectedDiagnosisOptions').find(".active").length > 0) {
            $("#CreateNewBrandEntryDiv").slideUp();    //## Hide the 'CreateNew' Area
            $("#SelectExistingBrandDiv").slideDown();    //## Show the Create-New-BrandEntry
            $("#NewDrugBrandEntryPopupModal").modal("show");
            return;
        } else {
            swal({
                title: "Diagnosis missing",
                text: "You must select a Diagnosis first, to create a Drug template!",
                icon: "warning",
            });
        }        
    });

    //## POST Ajax- Insert a new DrugBrand for the Selected Diagnosis
    $("#AddNewDiagnosisDrugBrandSubmitButton").click(function () {

        var drugId = $("#DrugListSelect").val();
        //var drugBrandId = $("#DiagnosisListSelect").val();
        var newBrandName = $("#DrugNewBrandNameInput").val();
        var indicationTypeId = $('input[type=radio][name=DiagnosisOptions]:checked').val()
        var manufacturerId = $('#ManufacturerListSelect').val()

        //## Post for a New Brand create and Assign to the Diognosis/indicationTypeId
        Post_NewBrand(drugId, 0, newBrandName, indicationTypeId, manufacturerId);

    });

    //## POST Ajax- Assign a DrugBrand to the Selected Diagnosis
    $("#AssignDrugBrandToDiagnosisButton").click(function () {

        var drugBrandId = $("#DrugBrandListSelect").val();   //## TODO: Brand ID
        var indicationTypeId = $('input[type=radio][name=DiagnosisOptions]:checked').val()

        //## Post this existing Brand - and Assign to the Diognosis/indicationTypeId
        Post_NewBrand(0, drugBrandId, '', indicationTypeId, 0);

    });


    function Post_NewBrand(drugId, drugBrandId, newBrandName, indicationTypeId, manufacturerId) {

        var postUrl = "/Doctor/Prescription/Insert_DrugBrandForDiagnosis";
        console.log("token: " + token);
        var selectOptionText = newBrandName === "" ? $("#DrugBrandListSelect option:selected").text() : newBrandName;

        $.ajax({
            url: postUrl,
            type: "POST",
            dataType: 'json',
            data: {
                __RequestVerificationToken: token,  //# AntiForgeryToken
                'drugId': drugId,
                'id': drugBrandId,
                'name': newBrandName,
                'IndicationsTypeId': indicationTypeId,
                'ManufacturerId': manufacturerId
            },
            success: function (result) {
                debugger;
                if (result >= 1) {
                    //$("#DiagnosisListSelect").append();                    
                    $("#BrandListSelect").append(new Option(selectOptionText, result))
                    $("#BrandListSelect").val(result);  //## Make it Selected

                    $("#DrugDoseTemplateListSelect").empty();
                } else {
                    swal({
                        title: "Update Failed!",
                        text: "System has failed to save this new Brand Details. Please reload the page and try again!",
                        icon: "warning",
                    });
                }

                $("#NewDrugBrandEntryPopupModal").modal("hide");

            },
            error: function (err) {
                console.log(err.statusText);
                return false;
            }
        });
    }

    //## In the Assign Brand to Diagnosis Popup-> Show 'Create Brand ' Div
    $("#ShowCreateNewBrandButton").click(function () {
        //$("#CreateNewBrandEntryDiv").removeClass("d-none");
        $("#CreateNewBrandEntryDiv").slideDown();
        $("#SelectExistingBrandDiv").slideUp();
    });

    //## Create New Brand Dose Template
    $("#CreateBrandDoseTemplateButton").click(function () {

        var postUrl = "/Doctor/Prescription/Insert_BrandDoseTemplate";        
        
        var modeOfDelivery = $("#ModeOfDeliverySelectList").val();
        var drugBrandId = $("#BrandListSelect").val();
        var doseStrength = $("input[name='DrugStrengthInputRadio']:checked").val();
        var strengthTypeText = $("#DrugStrengthInput").val() + " " + doseStrength;
        var drugDoseQty = $("input[name='DrugDoseQtyRadioOption']:checked").val();
        var doseFrequency = $("input[name='DoseFrequencyRadioOption']:checked").val();
        var durationDays = $("#DurationDays").val();
        var intakePatternId = $("#IntakePatternSourceSelect").val();

        var brandDoseTemplateCreateVM = {
            __RequestVerificationToken: token,  //# AntiForgeryToken__Validate : token,
            "DrugBrandId": drugBrandId,
            "ModeOfDeliveryId": modeOfDelivery,
            "StrengthTypeText": strengthTypeText,
            "Dose": drugDoseQty,
            "Frequency": doseFrequency,
            "Duration": durationDays,
            "IntakePatternId": intakePatternId,
        };

        $.ajax({
            type: "POST",
            url: postUrl,
            data: brandDoseTemplateCreateVM,
            dataType: "json",
            success: function (response) {
                debugger;
                if (response !== "error") {
                    var resultList = response.split(";");
                    var newPatternId = resultList[0];
                    var pattern = resultList[1];

                    console.log(response);
                    $("#NewDrugDoseTemplatePopupModal").modal("hide")
                    alert("response -" + newPatternId + "-" + newPatternId);
                    $("#DrugDoseTemplateListSelect").append($('<option></option>').attr('value', newPatternId).text(pattern));
                    $("#DrugDoseTemplateListSelect").val(newPatternId);
                } else {
                    OnFail_DrugDoseTemplate_Create();
                }
            },
            error: function (xhr, status, error) {
                // handle failure
                console.log('ERROR');
                OnFail_DrugDoseTemplate_Create();
            }
        });

        return;

        axios({
            method: 'post',
            headers: { "RequestVerificationToken": token },
            url: postUrl,
            data: {
                drugBrandId: "1",
                modeOfDeliveryId: "2",
                strengthTypeText: "2",
                dose: "3",
                frequency: "4",
                duration: "5",
                intakePatternId: "6",
            }
        }).then(function (response) {
            debugger;
            if (response.data !== "error") {
                var resultList = string.split(response.data, ";");
                var newPatternId = resultList[0];
                var pattern = resultList[1];

                console.log(response.data);
                $("#NewDrugDoseTemplatePopupModal").modal("hide")
                alert("response -" + response.data[0] + "-" + newPatternId);    
                $("#DrugDoseTemplateListSelect").append($('<option></option>').attr('value', newPatternId).text(pattern));
                $("#DrugDoseTemplateListSelect").val(newPatternId);
            } else {
                $("#NewDrugDoseTemplatePopupModal").modal("hide")
                OnFail_DrugDoseTemplate_Create();
            }
            
        })
            .catch(function (error) {
                OnFail_DrugDoseTemplate_Create();
            });
    });

    function OnFail_DrugDoseTemplate_Create() {
        swal({
            title: "Update Failed!",
            text: "System has failed to save this new Drug Dose template. Please reload the page and try again!",
            icon: "warning",
        });
    }


    //## Add New Drug item to Prescription
    $("#AddNewPescriptionItemButton").click(function (e) {
        debugger;

        if (PrescriptionHeaderInfo_Invalid()) {
            e.preventDefault();
            return false;
        }
       
        
        var drugBrandId = $("#BrandListSelect").val();
        var doseTemplateId = $("#DrugDoseTemplateListSelect").val();
        var instructionId = $("#DrugInstructionSelectList").val();

        var postUrl = "/Doctor/Prescription/Insert_PescriptionItem";        

        $.ajax({
            type: "POST",
            url: postUrl,
            data: {
                __RequestVerificationToken: token,  //# AntiForgeryToken__Validate : token,
                "PrescriptionId": prescriptionId,
                "DrugBrandId": drugBrandId,
                "BrandDoseTemplateId": doseTemplateId,
                "AdviseInstructionId": instructionId
            },
            dataType: "json",
            success: function (response) {
                debugger;
                if (response !== "error") {
                    OnSuccess_PrescriptionDrugItemAdd(response);
                } else {
                    console.log("failed - AddNewPescriptionItemButton() ");
                }
            },
            error: function (xhr, status, error) {
                // handle failure
                console.log('ERROR');
                console.log("failed - AddNewPescriptionItemButton() ");
            }
        });
               
    });

    function PrescriptionHeaderInfo_Invalid() {
        //## Validate Prescription Header info- ie: CC/Diagnosis/DrugBrand/Template
        var ccSelected = $("#ChiefComplaintList option:selected").length > 0;
        var templateSelected = $("#DrugDoseTemplateListSelect option:selected").length > 0;

        if (ccSelected == false || templateSelected == false) {
            swal({
                title: "Data missing",
                text: "You must select: Complaint, Diagnosis, Drug Brand and a Drug Template- to add an item to the prescription.",
                icon: "warning",
            });

            return true;    //## Prescription header is incomplete
        }

        return false;;
    }

    function OnSuccess_PrescriptionDrugItemAdd(data) {
        //debugger;

        var result = data.split(";");
        var newItemId = result[0];
        var intakePattern = result[1];

        var brandName = $("#BrandListSelect option:selected").text();
        var doseTemplate = $("#DrugDoseTemplateListSelect option:selected").text();
        var doseInstruction = $("#DrugInstructionSelectList option:selected").text();
        var doseArray = doseTemplate.split("-");


        var newItem = $("#PrescriptionItemRowTemplate").clone();
        $(newItem).attr("id", "prescriptionItem_" + newItemId);
        $(newItem).addClass("prescription-item-row");

        $("#PrescriptionItemsPreviewContainer").append(newItem);
        $(newItem).removeClass("d-none");

        //## Feed the PrescriptionItemValues in the Row        
        $(newItem).find(".mode-of-delivery-type").text(doseArray[1]);    //## Tablet/capsule
        $(newItem).find(".prescription-item-name").text(brandName);    //## Nurofen, Solpadine Max
        $(newItem).find(".prescription-item-strength").text(doseArray[0]);    //## ie: '200 mg'
        $(newItem).find(".prescription-item-pattern").text(intakePattern);    //## (1+0+1)

        if ($("#DrugInstructionSelectList").val() > 0) {
            $(newItem).find(".prescription-item-instruction").text(doseInstruction);    //## 'One before meals'
        }

        $(newItem).find(".prescription-item-duration").text(doseArray[3]);    //## ie: '7 Days'

        $(newItem).find(".prescription-item-remove").data("itemId", newItemId);    //### Delete Button will have the Newly Created Item's ID


        //## Add this atribute- so we cna count how many DrugItems are in the Prescription currently and show the serial number
        var itemsCount = $("#PrescriptionItemsPreviewContainer .prescription-item-row").length;
        $(newItem).find(".prescription-item-number").text(itemsCount);

        //## Now reset the Dropdown boxes values
        $("#DrugInstructionSelectList").val(0);
        $("#DrugDoseTemplateListSelect").empty()
        $("#DrugDoseTemplateListSelect").append(new Option("Select a Diagnosis", null));


    }


    //## WHen removing an item from the Current Prescription
    $(document).on("click", ".prescription-item-remove", function (e)
    {
        var drugItem = $(this).parent().find(".prescription-item-name").text();
        var drugItemId = $(this).data("itemId");

        debugger;

        swal({
            title: "Confirm remove?",
            text: "Are you sre to remove '" + drugItem + "' from this Prescription?",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        })
            .then((willDelete) => {
                if (willDelete) {
                    var itemDivToRemove = $(this).parent().parent().attr("id");
                    RemoveDrugItemFromPrescription(drugItemId, itemDivToRemove);
                    //swal("Poof! Your imaginary file has been deleted!", {
                    //    icon: "success",
                    //});
                }
            });

        e.preventDefault();
    });

    function RemoveDrugItemFromPrescription(drugItemId, itemDivToRemove) {
        var postUrl = "/Doctor/Prescription/Delete_PescriptionItem";        
        debugger;

        $.ajax({
            type: "POST",
            url: postUrl,
            data: {
                __RequestVerificationToken: token,  //# AntiForgeryToken__Validate : token,
                "PrescriptionItemId": drugItemId,
            },
            dataType: "json",
            success: function (response) {
                debugger;
                if (response === "success") {
                    $("#" + itemDivToRemove).remove();
                } else {
                    console.log("failed - RemoveDrugItemFromPrescription() ");
                }
            },
            error: function (xhr, status, error) {
                console.log("failed - RemoveDrugItemFromPrescription() ");
            }
        });

        
    }

    //## Add new Instruction- 'Before Meals', 'After Meals'
    $("#AddNewDrugInstructionButton").click(function () {
        $("#InstructionModalTitle").text("Instructions");
        $("#NewDrugAdviseInstructionPopupDiv").modal("show");
    });

    //## Add new Drug Dose Template, ie (1 + 1 + 1)
    $("#AddNewDrugDosageTemplateButton").click(function () {
        if ($("#BrandListSelect").val() != null) {
            var selectedDrugBrand = $("#BrandListSelect option:selected").text();
            $("#NewDrugDoseTemplateTitleSpan").text(selectedDrugBrand); //## Title of the Modal Popup
            $("#NewDrugDoseTemplateExampleSpan").text(selectedDrugBrand);   //## Example Text- to show how it will look like on the Prescription

            $("#NewDrugDoseTemplatePopupModal").modal("show");
            return;
        } else {
            swal({
                title: "Drug not selected",
                text: "You must select a Drug brand first, to create a Dose template!",
                icon: "warning",
            });
        }

        
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

    $("#ReferredPatientCheckBox").click(function () {
        $("#ReferredDoctorNameInput").prop("disabled", $(this).is(":checked") ? "" : "disabled");
        $("#ReferredDoctorNameInput").val("");  //## clear the existing value

    });


    //## Last Action in the Presction-> "Finish And Print"
    $("#FinishAndPrintButton").click(function(e) {

        //## Do Validation. All fields are filled?

        //## Now POST
        var postUrl = "/Doctor/Prescription/FinishAndPreview";
        debugger;

        $.ajax({
            type: "POST",
            url: postUrl,
            data: {
                __RequestVerificationToken: token,  //# AntiForgeryToken__Validate : token,
                "Id": prescriptionId,
            },
            dataType: "json",
            success: function (response) {
                $("#PrescriptionMainContainer").html(response);
                debugger;

                console.log("success")
            },
            error: function (xhr, status, error) {
                console.log("failed - FinishAndPrintButton.CLick() ");
            }
        });

    });

    $("#PrintPreviewButton").click(function (e) {
        debugger;

        //## Patient Details
        var personalDetails = $("#PersonalDetails .patient-name").text();
        $("#PrescriptionPreviewPatientDetails .patient-name").text(personalDetails);
        var address = $("#PersonalDetails .patient-address").text();
        $("#PrescriptionPreviewPatientDetails .patient-address").text(address);
        //## CC

        //## Examination
        var temp = $("#PatientVitalsInfoDiv .vital-temperature").text();
        var pulse = $("#PatientVitalsInfoDiv .vital-pulse").text();
        var pressure = $("#PatientVitalsInfoDiv .vital-pressure").text();
        var weight = $("#PatientVitalsInfoDiv .vital-weight").text();
        //var height = $("#PatientVitalsInfoDiv .vital-height").text();
                
        $("#PrescriptionVitalsDiv .vital-temperature").text(temp);
        $("#PrescriptionVitalsDiv .vital-pulse").text(pulse);
        $("#PrescriptionVitalsDiv .vital-pressure").text(pressure);
        $("#PrescriptionVitalsDiv .vital-weight").text(weight);
        //$("#PrescriptionVitalsDiv .vital-height").text(height);

        //## Notes
        $("#PrescriptionPreviewNotesDiv").text($("#PrescriptionNotesInput").val());

        //## Planning
        $("#PrescriptionPreviewPlanDiv").text($("#PlanInputBox").val());
        
        //## Advise
        $("#PrescritionPreviewAdvise").text($("#AdviseInput").val());

        //## Lab Test Request

        //## Finally all the DrugItem names will be copied to the Preview Panel
        $("#PrintPreviewContainer").html($("#PrescriptionItemsPreviewContainer").html());
        $("#PrintPreviewContainer .prescription-action-column").remove();
        $("#PrintPreviewContainerModalPopup").modal("show");

    });


    $("#ConfirmAndPrintPrescriptionButton").click(function () {

        //## Now POST
        var postUrl = "/Doctor/Prescription/Create_Prescription_HTML";
        var htmlContents = $("#PreviewContentsDiv").html();
        
        $.ajax({
            type: "POST",
            url: postUrl,
            data: {
                __RequestVerificationToken: token,  //# AntiForgeryToken__Validate : token,
                "id": prescriptionId,
                "contents": htmlContents,
            },
            dataType: "json",
            success: function (response) {
                if (response === "success") {
                    window.print();
                    console.log("success")
                } else {
                    console.log("failed to create output HTML file")
                }
            },
            error: function (xhr, status, error) {
                console.log("failed - ConfirmAndPrintPrescriptionButton.Click() ");
            }
        });
        
    });

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

});

/*
 $.each(data,function(index,itemData){
    $('#dropListBuilding').append($("<option></option>")
        .attr("value",key)
        .text(value));
});

 * */