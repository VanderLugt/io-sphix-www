function AddOpenOfficeHours(OpenHoursId) {
   // alert(OpenHoursId);
    var x;
    x = $('input[name=radioTimeZone]:checked').val()
    if (typeof x === "undefined") {
        //alert('error');
        swal("Sorry!", "Please select atleast one time. ", "warning");
        // ...
        return;
    }
    var dataModel = {
        OTimeZone: $('input[name=radioTimeZone]:checked').val(),
        CommunityGroupId: $('#hdnCommunityGroupId').val()
    };

    $.ajax({
        type: 'POST',
        data: dataModel,
        url: '/CommunityOpenOfficeHours/AddOpenOfficeHours?OpenHoursId=' + OpenHoursId,
        success: function (data) {
            //console.log(JSON.stringify(data));
            swal("", data.messsage, "success");
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