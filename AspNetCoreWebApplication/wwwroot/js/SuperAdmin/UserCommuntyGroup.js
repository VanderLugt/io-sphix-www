function PublishCommuntyGroup(IsPublish) {
    if (confirm("Are you sure, you want to edit this ...?")) {
        $.ajax({
            type: 'POST',
            data: { Id: $('#hdnCommunityGrooupId').val(), IsPublish: IsPublish },
            url: '/SuperAdmin/UsersCommunityGroups/PublishCommunityGroup',
            success: function (data) {
                //alert(JSON.stringify(data));
                if (data.status) {
                    // swal("Wait", "Good", "success");
                    toastr.success(data.messsage, '', { timeOut: 1000 });
                    communityGroupsDataTable.ajax.reload();
                    $('#divReviewCommunityGroupModel').modal('hide');

                    //DashBoard
                }
                else {
                    swal("Sorry!", data.messsage, "warning");
                }
                //$('#divLoadStep3').html(data);
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
}