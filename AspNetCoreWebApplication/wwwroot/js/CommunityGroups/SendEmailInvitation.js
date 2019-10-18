function sendInvitationToGroup() {
    if (formValidation('toGroup') === true) {
        var model = {
            ToEmailAddress: $('#txtInviteToGroupEmails').val(),
            CommunityGroup: $('#hdnCommunityGroupId').val()
        };
        $.ajax({
            type: 'POST',
            data: model,
            url: '/EmailInvitation/SendGroupEmailInvitation',
            success: function (data) {
                //console.log(JSON.stringify(data));
                swal("", data.messsage, "success");
            },
            failure: function (response) {
                //alert(response.responseText);
                swal("Sorry!", response.responseText, "error");
            },
            error: function (response) {
                swal("Sorry!", response.responseText, "error");
            }

        });
    }
}
function sendInvitationToSphix() {
    var model = {
        emails: $('#txtInviteToSphixEmails').val()
    };
   // postInvitation(model);
}
