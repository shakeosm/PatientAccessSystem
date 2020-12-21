// Tooltip //
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

var _prescriptionId = -1;
var _patientId = -1;

$(document).ready(function () {
    _prescriptionId = $("[name='PrescriptionIdInput']").val();
    _patientId = $("[name='PatientId']").val();

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


    $('#ExaminationItemAddedAlert').fadeOut();

    $("#ExaminationItemAddButton").click(function () {
        debugger;

        var examFindings = $("#ExaminationFindingInput").val();
        var category = $("#ExminationCategorySelect option:selected").text();
        var genericOption = $("#ExminationGenericItemsSelect option:selected").text();
        var nervousOption = $("#ExminationNervousSystemItemsSelect option:selected").text();

        var examItem = genericOption === "" ? nervousOption : genericOption;
        var examPointId = genericOption === "" ? $("#ExminationNervousSystemItemsSelect").val() : $("#ExminationGenericItemsSelect").val();

        if (examFindings === "" || category === "" || examItem === "") {
            $("#ExaminationFindingInput").parent().find(".error-feedback").removeClass("invisible");
            return;
        }
                
        var examinationItem = category.concat("- ", examItem, ": ", examFindings);

        $(this).prop("disabled", true); //## to stop from clicking multiple times

        //## Post via Ajax to Controller
        Post_NewExaminationItem($("#ExminationCategorySelect").val(), examPointId, examFindings, examinationItem);                

    });

    function Post_NewExaminationItem(categoryId, pointId, findings, examinationItem) {

        var postUrl = "/Doctor/Prescription/Insert_PescriptionExaminationItem";
        console.log("token: " + token);        

        $.ajax({
            url: postUrl,
            type: "POST",
            dataType: 'json',
            data: {
                __RequestVerificationToken: token,  //# AntiForgeryToken
                'PrescriptionId': _prescriptionId,
                'CategoryId': categoryId,
                'PointId': pointId,
                'Findings': findings
            },
            success: function (result) {
                debugger;

                if (result >= 1) {
                    $("#DefaultExamintionText").slideUp();
                    var deleteButton = " <i role='button' class='fal fa-minus-circle p-1 text-danger delete-examination-item' " +
                        "data-examination-item-id='"+ result +"' data-toggle='tooltip' data-placement='right' title='Remove Examination item'></i> ";
                    
                    console.log(examinationItem);

                    $("#ExaminationSelectedItemsUL").append("<li>" + examinationItem + deleteButton + "</li>");

                    //## Reset the Form
                    $("#ExaminationFindingInput").val("");
                    $("#ExaminationFindingInput").parent().find(".error-feedback").addClass("invisible");
                    $(".exam-option-list").val(''); //## remove all previous selection

                    //## show the alert for a second.. and then hide
                    $('#ExaminationItemAddedAlert').fadeIn();
                    $('#ExaminationItemAddedAlert').delay(1000).fadeOut();

                    $("#ExaminationItemAddButton").prop("disabled", false); 

                    $('[data-toggle="tooltip"]').tooltip();
                    


                } else {
                    $("#ExaminationModalPopup").modal("hide");
                    ShowAlert('error', 'Adding Failed', 'System has failed to save this new Examination item. Please reload the page and try again!');

                    $(this).prop("disabled", false); 
                }
            },
            error: function (err) {
                console.log(err.statusText);
                return false;
            }
        });
    }

    $(document).on("click", ".delete-examination-item", function (e) {
        var examinationItemId = $(this).data("examinationItemId");
        var listItem = $(this).parent();    //## if confirmed- we will deelte this <li>
        
        Swal.fire({
            title: 'Confirm delete',
            text: "Do you want to delete this examination item?",
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Delete!'
        }).then((result) => {
            if (result.isConfirmed) {
                Post_DeleteExaminationItem(examinationItemId, listItem);
                
            }
        })

    });

    function Post_DeleteExaminationItem(examinationItemId, listItem) {

        var postUrl = "/Doctor/Prescription/Delete_PescriptionExaminationItem";
        $("#PasLoaderbody").removeClass('invisible');

        $.ajax({
            url: postUrl,
            type: "POST",
            dataType: 'json',
            data: {
                __RequestVerificationToken: token,  //# AntiForgeryToken
                'id': examinationItemId,
            },
            success: function (result) {
                $("#PasLoaderbody").addClass('invisible');

                if (result === "success") {
                    $(listItem).remove();   //## remove the list item

                    //## if all Examination items are removed- show the default text- ""
                    if ($("#ExaminationSelectedItemsUL .delete-examination-item").length < 1) {
                        $("#DefaultExamintionText").slideDown();
                    }

                } else {
                    ShowAlert('error', 'Delete Failed!', 'System has failed to delete this Examination item. Please try again later!');
                }

            },
            error: function (err) {
                console.log(err.statusText);
                return false;
            }
        });
    }


    $("#ExminationCategorySelect").click(function() {
        var category = $("#ExminationCategorySelect").val();
        if (category === "5") { //## ExminationCategory.Nervous = 5
            $("#ExminationNervousSystemItemsSelect").removeClass("d-none");
            $("#ExminationGenericItemsSelect").addClass("d-none");
        } else {
            if ($("#ExminationNervousSystemItemsSelect").is(":visible")) {
                $("#ExminationNervousSystemItemsSelect").addClass("d-none");
                $("#ExminationGenericItemsSelect").removeClass("d-none");
            }
        }
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


    //####      Investigation Add/ Remove
    $("#InvestigationItemAddedAlert").fadeOut();

    $("#InvestigationCategorySelect").click(function () {
        debugger;
        var parentId = $(this).val();

        AddInvestigationChildItems(parentId, "#InvestigationSubCategorySelect")

        $("#InvestigationSubCategoryItemSelect").empty();
        
    });

    //## InvestigationSubCategorySelect
    $("#InvestigationSubCategorySelect").click(function () {
        debugger;
        var parentId = $(this).val();
        AddInvestigationChildItems(parentId, "#InvestigationSubCategoryItemSelect")

    });

    //### This will get list of Child Items for a Specific Parent and then add those to a specific SelectList Input box
    function AddInvestigationChildItems(parentId, targetSelectListId) {
        if (parentId < 1 || parentId === "undefined")
            return;

        ShowPreloader();
        
        var targetSelectListId = $(targetSelectListId);
        targetSelectListId.empty();

        var URL = `/Doctor/Prescription/ListAllInvestigationSubCategory/${parentId}`;
        axios.get(URL)
            .then(function (response) {
                debugger;
                if (response.data !== "error") {
                    if (response.data.length > 0) { //## if any result is returned- then add to the target SelectList control
                        $(targetSelectListId).prop("disabled", false);

                        for (i = 0; i < response.data.length; i++) {
                            $(targetSelectListId).append("<option value=" + response.data[i].id + ">" + response.data[i].description + "</option>");
                        }

                    } else {    //## if no Child Items found- then disable the target SelectList input
                        $(targetSelectListId).prop("disabled", true);
                    }
                }

                HidePreloader();
            });
    }

    $("#InvestigationItemAddButton").click(function(e) {

        var itemSelected = true;

        debugger;

        if ($("#InvestigationSubCategoryItemSelect").prop("disabled") === false) {
            if ($("#InvestigationSubCategoryItemSelect").val() < 1) {
                itemSelected = false;
            }
        } else {
            if ($("#InvestigationSubCategorySelect").val() < 1) {
                itemSelected = false;
            }
        }

        if (itemSelected === false) {
            //ShowAlert('warning', 'Investigation missing!', 'You must select an Investigation first, to add to the Prescription!');
            $(this).parent().find(".error-feedback").removeClass("invisible");
            return;
        }

        $(this).parent().find(".error-feedback").addClass("invisible");

        var investigationItemId = -1;
        var selectedcategoryId = $("#InvestigationCategorySelect").val();

        var categoryText = "";

        if (selectedcategoryId == 12 || selectedcategoryId == 13) {
            categoryText = GetSelectListText("#InvestigationCategorySelect") + "- ";
        }


        var subCategoryItem = $("#InvestigationSubCategoryItemSelect").val();

        var displayText = categoryText + GetSelectListText("#InvestigationSubCategorySelect");

        if (subCategoryItem > 0) {
            investigationItemId = subCategoryItem;
            //var subItem = GetSelectListText("#InvestigationSubCategoryItemSelect");
            displayText = displayText + " - " + GetSelectListText("#InvestigationSubCategoryItemSelect");

        } else {
            investigationItemId = $("#InvestigationSubCategorySelect").val();
        }
        
        
        Post_NewInvestigationItem(investigationItemId, displayText);

    });

    function Post_NewInvestigationItem(investigationId, displayText) {
        var postUrl = "/Doctor/Prescription/Insert_InvestigationItem";

        ShowPreloader();

        SetDisabled("#InvestigationItemAddButton", true);

        $.ajax({
            url: postUrl,
            type: "POST",
            dataType: 'json',
            data: {
                __RequestVerificationToken: token,  //# AntiForgeryToken
                'PrescriptionId': _prescriptionId,
                'InvestigationId': investigationId,
                //'Notes': newBrandName,
            },
            success: function (result) {
                debugger;                

                SetDisabled("#InvestigationItemAddButton", false);

                if (result >= 1) {
                    $("#DefaultInvestigationText").fadeOut();   //## for the firs time- once an item is added- hide the default text

                    var deleteButton = " <i role='button' class='fal fa-minus-circle p-1 text-danger delete-investigation-item' " +
                        "data-investigation-item-id='" + result + "' data-toggle='tooltip' data-placement='right' title='Remove investigation item'></i> ";

                    $("#InvestigationSelectedItemsUL").append("<li>" + displayText.trim() + deleteButton + "</li>")
                    
                    $('[data-toggle="tooltip"]').tooltip();

                    HidePreloader();                    

                    $('#InvestigationItemAddedAlert').fadeIn();
                    $('#InvestigationItemAddedAlert').delay(1000).fadeOut();

                } else {                    
                    $("#InvestigationModalModalPopup").modal("hide");
                    ShowAlert('warning', 'Update Failed!', 'System has failed to add this new Investigation Item. Please reload the page and try again!');
                }
            },
            error: function (err) {
                console.log(err.statusText);
                return false;
            }
        });
    }


    $(document).on("click", ".delete-investigation-item", function (e) {
        var investigationItemId = $(this).data("investigationItemId");
        var listItem = $(this).parent();    //## if confirmed- we will delete this <li>

        Swal.fire({
            title: 'Confirm delete',
            text: "Do you want to delete this investigation item?",
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Delete!'
        }).then((result) => {
            if (result.isConfirmed) {
                Post_DeleteInvestigationItem(investigationItemId, listItem);

            }
        })

    });

    function Post_DeleteInvestigationItem(investigationItemId, listItem) {

        var postUrl = "/Doctor/Prescription/Delete_InvestigationItem";
        ShowPreloader();

        $.ajax({
            url: postUrl,
            type: "POST",
            dataType: 'json',
            data: {
                __RequestVerificationToken: token,  //# AntiForgeryToken
                'id': investigationItemId,
            },
            success: function (result) {
                HidePreloader();

                if (result === "success") {
                    $(listItem).remove();   //## remove the list item

                    //## if all investigation items are removed- show the default text-
                    if ($("#InvestigationSelectedItemsUL .delete-investigation-item").length < 1) {
                        $("#DefaultInvestigationText").fadeIn();
                    }

                } else {
                    ShowAlert('error', 'Delete Failed!', 'System has failed to delete this Investigation item. Please try again later!');
                }

            },
            error: function (err) {
                console.log(err.statusText);
                return false;
            }
        });
    }



    //## End of Investigation

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
            ShowAlert('warning', 'Diagnosis missing!', 'You must select a Diagnosis first, to create a Drug template!');
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
                    $("#NewDrugBrandEntryPopupModal").modal("hide");
                    ShowAlert('warning', 'Update Failed!', 'System has failed to save this new Brand Details. Please reload the page and try again!');
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

        debugger;

        var postUrl = "/Doctor/Prescription/Insert_BrandDoseTemplate";        
        
        var modeOfDelivery = $("#ModeOfDeliverySelectList").val();
        var drugBrandId = $("#BrandListSelect").val();
        var doseStrength = $("input[name='DrugStrengthInputRadio']:checked").val();
        var strengthTypeText = $("#DrugStrengthInput").val() + " " + doseStrength;
        var drugDoseQty = $("input[name='DrugDoseQtyRadioOption']:checked").val();
        var doseFrequency = $("input[name='DoseFrequencyRadioOption']:checked").val();
        var durationDays = $("#DurationDays").val();
        var intakePatternId = $("#IntakePatternSourceSelect").val();

        //## Validation
        if (modeOfDelivery === "" || doseStrength == undefined || strengthTypeText == undefined || durationDays < 1 || intakePatternId === "") {
            showElement("#DoseTemplateAddError");
            return false;
        }

        hideElement("#DoseTemplateAddError");

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
                    //alert("response -" + newPatternId + "-" + newPatternId);
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
                //alert("response -" + response.data[0] + "-" + newPatternId);    
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
        $("#NewDrugDoseTemplatePopupModal").modal("hide");
        ShowAlert('error', 'Update Failed!', 'System has failed to save this new Drug Dose template. Please reload the page and try again!');
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
                "PrescriptionId": _prescriptionId,
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
            ShowAlert('warning', 'Data missing!', 'You must select: Complaint, Diagnosis, Drug Brand and a Drug Template- to add an item to the prescription.');

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

        if ($("#BrandListSelect").val() >= 1) {
            var selectedDrugBrand = $("#BrandListSelect option:selected").text();
            $("#NewDrugDoseTemplateTitleSpan").text(selectedDrugBrand); //## Title of the Modal Popup
            $("#NewDrugDoseTemplateExampleSpan").text(selectedDrugBrand);   //## Example Text- to show how it will look like on the Prescription

            $("#NewDrugDoseTemplatePopupModal").modal("show");
            return;
        } else {
            ShowAlert('warning', 'Drug not selected!', 'You must select a Drug brand first, to create a Dose template!');
        }

        
    });

    $("#PatientVitalEntryModalButton").click(function() {
        $("#PatientVitalsModalPopup").modal("show");
    });

    //## UPdate the Prescription Area with the Vital Info from the Popup Modal
    $("#VitalModalSubmitButton").click(function () {
        var vitalsHistoryId = GetVal("#VitalsHistoryId");
        debugger;

        var temperature = GetVal("#TemperatureInput");
        var pulseInput = GetVal("#PulseInput");
        var diastolic = GetVal("#BoodPressureDiastolicInput");
        var systolic = GetVal("#BoodPressureSystolicInput");
        var weight = GetVal("#WeightInput");
        //var height = GetVal("#HeightInput");

        //## Now POST
        var postUrl = "/Doctor/Prescription/Update_Vitals";

        $.ajax({
            type: "POST",
            url: postUrl,
            data: {
                __RequestVerificationToken: token,  //# AntiForgeryToken__Validate : token,
                "Id": vitalsHistoryId,
                "PatientId": _patientId,
                "PrescriptionId": _prescriptionId,
                "Temperature": temperature,
                "BloodPulse": pulseInput,
                "Diastolic": diastolic,
                "Systolic": systolic,
                "Weight": weight,
                //"Weight": weight,
            },
            dataType: "json",
            success: function (response) {
                debugger;
                if (response !== 0) {
                    $("#VitalsHistoryId").val(response);    //## Update the returned RecordId

                    $(".vital-temperature").text(temperature);
                    $(".vital-pulse").text(pulseInput);
                    $(".vital-pressure").text(diastolic + "/" + systolic);
                    $(".vital-weight").text(weight);
                    //$("#PatientVitalsInfoDiv .vital-height").text(height);

                    //## Update the value in PrintPreview, too.. save time
                    //$("#PrescriptionVitalsDiv .vital-temperature").text(temp);
                    //$("#PrescriptionVitalsDiv .vital-pulse").text(pulse);
                    //$("#PrescriptionVitalsDiv .vital-pressure").text(pressure);
                    //$("#PrescriptionVitalsDiv .vital-weight").text(weight);

                    //## Once success- then hide the Modal
                    $("#PatientVitalsModalPopup").modal("hide");
                    
                } else {
                    OnError_VitalsUpdateFailed()
                }
            },
            error: function (xhr, status, error) {
                OnError_VitalsUpdateFailed()
            }
        });

        
    });

    function OnError_VitalsUpdateFailed() {
        $("#PatientVitalsModalPopup").modal("hide");

        ShowAlert("warning", "Failed to update", "There was an error and the Patient Vitals was not updated. Please reload the page and try again.");
    }
        

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

    $("#PrescriptionPrintButton").click(function () {
        window.print();
    });
    //## Last Action in the Presction-> "Finish And Print"
    //$("#FinishAndPrintButton").click(function(e) {

    //    //## Do Validation. All fields are filled?

    //    //## Now POST
    //    var postUrl = "/Doctor/Prescription/FinishAndPreview";
    //    debugger;

    //    $.ajax({
    //        type: "POST",
    //        url: postUrl,
    //        data: {
    //            __RequestVerificationToken: token,  //# AntiForgeryToken__Validate : token,
    //            "Id": _prescriptionId,
    //        },
    //        dataType: "json",
    //        success: function (response) {
    //            $("#PrescriptionMainContainer").html(response);
    //            debugger;

    //            console.log("success")
    //        },
    //        error: function (xhr, status, error) {
    //            console.log("failed - FinishAndPrintButton.CLick() ");
    //        }
    //    });

    //});

    $("#PrintPreviewButton").click(function (e) {
        debugger;

        var address = $("#PersonalDetails .patient-address").text();
        $("#PrescriptionPreviewPatientDetails .patient-address").text(address);

        //## CC
        $("#PrescriptionCCListUL").empty();
        $("#ChiefComplaintList option:selected").each(function (index) {
            //$("#PrescriptionCCListUL").append("<li class='list-group-item'>" + $(this).text() + "</li>");
            $("#PrescriptionCCListUL").append("<li>" + $(this).text() + "</li>");
            //console.log($(this).text());
        });

        //## Examination- values are already updated- when Modal Form SubmitButton Clicked.
        $("#PrescriptionVitalsDivMobileUL").append($("#ExaminationSelectedItemsUL").html());
        $("#PrescriptionVitalsDivMobileUL").find(".delete-examination-item").remove();
        $("#PrescriptionPreviewExaminationList").html($("#ExaminationSelectedItemsUL").html());
        $("#PrescriptionPreviewExaminationList").find(".delete-examination-item").remove();
        

        //## Notes
        var notesList = $('#PrescriptionNotesInput').val().trim().split("\n");
        $("#PrescritionPreviewNotesSelectList").empty();

        $.each(notesList, function (item) {
            $("#PrescritionPreviewNotesSelectList").append("<li>" + notesList[item] + "</li>");
        });


        //## Planning
        var planList = $('#PlanInputBox').val().trim().split("\n");
        $("#PrescritionPreviewPlanSelectList").empty();

        $.each(planList, function (item) {
            $("#PrescritionPreviewPlanSelectList").append("<li>" + planList[item] + "</li>");
        });

        //## Advise
        var adviseList = $('#AdviseInput').val().trim().split("\n");
        $("#PrescritionPreviewAdviseSelectList").empty();

        $.each(adviseList, function (item) {
            $("#PrescritionPreviewAdviseSelectList").append("<li>" + adviseList[item] + "</li>");
        });

        //## Lab Test Request
        $("#PreviewInvestigationListUL").empty();
        $("#PreviewInvestigationListUL").append($("#InvestigationSelectedItemsUL").html())
        $("#PreviewInvestigationListUL").find(".delete-investigation-item").remove();


        //## Finally all the DrugItem names will be copied to the Preview Panel
        $("#PrintPreviewContainer").html($("#PrescriptionItemsPreviewContainer").html());
        $("#PrintPreviewContainer .prescription-action-column").remove();   //## we don't need the action buttons in the Preview
        $("#PrintPreviewContainer .prescription-item-details").removeClass("col-6").addClass("col-7");   //## Increase the width of all Drug item columns

        //## If no DrugItem is added- then don't show the "Finish" Button in the Preview Modal
        if ($("#PrescriptionItemsPreviewContainer .prescription-item-row").length >= 1) {
            $("#PrescriptionFinishButton, #DisclaimerCheckBoxDiv").removeClass("d-none");
        }

        $("#PrintPreviewContainerModalPopup").modal("show");

    });

    $("#ConfirmFinishCheckbox").click(function () {

            $("#PrescriptionFinishButton").prop("disabled", !$(this).is(":checked"));
        
    });

    $("#PrescriptionFinishButton").click(function () {
        debugger;

        //## Now POST
        var postUrl = "/Doctor/Prescription/Create_Prescription_HTML";
        var htmlContents = $("#PreviewContentsDiv").html();

        //## arrange the Data- that are not saved yet
        var ccList = $("#ChiefComplaintList").val(); //## This will return an Array of selected item values.

        var diagnosisList = ""; //## We have selected Diagnosis in the radioButtons. All of them- send with Prescription
        $("[name=DiagnosisOptions]").each(function () {
            diagnosisList = diagnosisList + $(this).val() + ",";
        });

        var notes = GetVal("#PrescriptionNotesInput");
        var plans = GetVal("#PlanInputBox");
        var advise = GetVal("#AdviseInput");
        var referredDoctor = GetVal("#ReferredDoctorNameInput");
        var isFollowUp = IsChecked("#FollowUpVisitCheckbox");

        $.ajax({
            type: "POST",
            url: postUrl,
            data: {
                __RequestVerificationToken: token,  //# AntiForgeryToken__Validate : token,
                "PrescriptionId": _prescriptionId,
                "PatientId": _patientId,
                "HtmlContents": htmlContents,
                "ccList": ccList,
                "Diagnosis": diagnosisList,
                "LabTestRequestList": '1,2,3',
                "Notes": notes,
                "Plans": plans,
                "Advise": advise,
                "ReferralDoctor": referredDoctor,
                "IsFollowUpVisit": isFollowUp,
            },
            dataType: "json",
            success: function (response) {
                debugger;

                if (response === "success") {
                    $("#DisclaimerCheckBoxDiv").hide();
                    $("#PrescriptionFinishButton").addClass("d-none");
                    $("#PrescriptionPrintButton").removeClass("d-none");

                    $("#BackToPatientSearchButton").removeClass("d-none");
                    $("#ClosePreviewDialogButton").addClass("d-none");

                    $("#SaveSuccessConfirm").removeClass("d-none");

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

    //## Description: This will get the Value in the Element and return it
    function GetVal(elementId) {
        return $(elementId).val();
    }

    function GetText(elementId) {
        return $(elementId).text();
    }

    function GetSelectListText(selectListId) {
        var selectedText = $(selectListId + " option:selected").text();
        return selectedText;
    }

    function IsChecked(elementId) {
        return $(elementId).is(":checked");
    }

    function ShowPreloader() {
        $("#PasLoaderbody").removeClass('invisible');
    }

    function HidePreloader() {
        $("#PasLoaderbody").addClass('invisible');

    }

    function SetDisabled(controlName, disableOption) {
        $(controlName).prop("disabled", disableOption);
    }


    function ShowAlert(swalIcon, messageTitle, messageText) {
        HidePreloader(); 

        Swal.fire({
            icon: swalIcon,
            title: messageTitle,
            text: messageText,
        })
    }

    function showElement(element) { $(element).removeClass('d-none'); } //## Used in AccessRequest.js
    function hideElement(element) { $(element).addClass('d-none'); }

    //## Prepare PrescriptionPreviwe Modal- with Doctor and Hospital Details

});

/*
 $.each(data,function(index,itemData){
    $('#dropListBuilding').append($("<option></option>")
        .attr("value",key)
        .text(value));
});

 * */