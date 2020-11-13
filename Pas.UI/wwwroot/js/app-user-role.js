$(document).ready(function () {

    $("#SubmitSelectedRoleButton").click(function () {
        if ($("input[type=radio]:checked").length < 1) {

            //debugger;

            Swal.fire({
                title: 'Error!',
                text: 'You must select a role to continue',
                icon: 'error',
                confirmButtonText: 'Cool'
            })

            return false;
        }
    });

});
