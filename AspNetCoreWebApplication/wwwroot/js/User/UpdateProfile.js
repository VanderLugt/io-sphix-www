$('#btnUpdateUserProfile').click(function () {
    if (formValidation('UpdateUserProfile') === true) {

        var profileModel = {
           // ProfileLink: $('#txtProfileLink').val(),
            FirstName: $('#txtProfileFirstName').val(),
            LastName: $('#txtProfileLastName').val(),
            EmailAddress: $('#txtProfileEmail').val(),
            PhoneNumber: $('#txtProfilePhone').val(),
            Id: $('#hdnProfileId').val()
        };

        $.ajax({
            type: 'POST',
            data: { model: profileModel },
            url: '/Dashboard/UpdateProfile',
            success: function (data) {
                //alert(JSON.stringify(data));
                if (data.status) {
                    swal("", data.messsage, "success");
                }
                else {
                    swal("Sorry!", data.messsage, "warning");
                }
            },
            failure: function (response) {
                // alert(response.responseText);
                swal("Sorry!", response.responseText, "error");
            },
            error: function (response) {
                swal("Sorry!", response.responseText, "error");
            }
        });
    }
});
function ReSendVerification() {

    $.ajax({
        type: 'POST',
        url: '/Dashboard/ReSendVerification',
        success: function (data) {
            //alert(JSON.stringify(data));
            if (data.status) {
                swal("", data.messsage, "success");
            }
            else {
                swal("Sorry!", data.messsage, "warning");
            }
        },
        failure: function (response) {
            // alert(response.responseText);
            swal("Sorry!", response.responseText, "error");
        },
        error: function (response) {
            swal("Sorry!", response.responseText, "error");
        }
    });
}
