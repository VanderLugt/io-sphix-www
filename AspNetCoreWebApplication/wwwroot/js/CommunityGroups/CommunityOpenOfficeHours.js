function AddOpenOfficeHours(OpenHoursId) {
   // alert(OpenHoursId);
    var x;
    x = $('input[name=radioTimeZone]:checked').val();
    if (typeof x === "undefined") {
        //alert('error');
        swal("Sorry!", "Please select atleast one time. ", "warning");
        // ...
        return;
    }
    var dataModel = {
        OTimeZone: $('input[name=radioTimeZone]:checked').val(),
        CommunityGroupId: $('#hdnCommunityGroupId').val(),
        OTitle: $('#txtOpenHoursConversation').val()
    };
    if (formValidation('modelCreateOpenHours') === true) {
        $.ajax({
            type: 'POST',
            data: dataModel,
            url: '/CommunityOpenOfficeHours/AddOpenOfficeHours?OpenHoursId=' + OpenHoursId,
            success: function (data) {
                //console.log(JSON.stringify(data));
                //swal("", data.messsage, "success");
                if (data.status) {
                    toastr.success(data.messsage, '', { timeOut: 1000 });
                    $('#modelCreateOpenHours').modal('hide');
                }
                else {
                    toastr.error(data.messsage, 'Sorry!', { timeOut: 5000 });
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
}