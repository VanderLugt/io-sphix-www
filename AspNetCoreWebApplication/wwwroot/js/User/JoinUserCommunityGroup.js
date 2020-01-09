function JoinCommunityCategory() {
    var dataModel = {
        CommunityGroupId: $('#hdnCommuntyId').val()
    };
    $.ajax({
        type: 'POST',
        data: { model: dataModel },
        url: '/CommunityGroupSubSections/JoinCommunity',
        success: function (data) {
            if (data.status) {
                //swal("", data.messsage, "success");

                swal({
                    title: "Thanks",
                    text: data.messsage,
                    type: "success"
                }).then(function () {
                    window.location = "CommunityGroups/" + $('#hdnCommuntyUrl').val();
                });
               //window.location.href = "CommunityGroups/" + $('#hdnCommuntyUrl').val();
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
function JoinCommunityGroup() {
    var dataModel = {
        CommunityGroupId: $('#hdnCommunityGroupId').val(),
        IsJoin: true
    };
    $.ajax({
        type: 'POST',
        data: { model: dataModel },
        url: '/CommunityGroupSubSections/JoinCommunityGroup',
        success: function (data) {
            if (data.status) {
               // swal("", data.messsage, "success");
                swal({
                    title: "Thanks",
                    text: data.messsage,
                    type: "success"
                }).then(function () {
                    window.location = "../CommunityGroup/" + $('#hdnCommuntyGroupUrl').val();
                });
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
function JoinCommunityOpenHoursMeeting() {
    //alert($('input[name=radioRegisterOpenOfficeHours]:checked').val());
    var x;
    x = $('input[name=radioRegisterOpenOfficeHours]:checked').val();
    if (typeof x === "undefined") {
        //alert('error');
        swal("Sorry!", "Please select atleast one meeting. ", "warning");
        // ...
        return;
    }
    var TimeZone = $('input[name=radioRegisterOpenOfficeHours]:checked').closest('tr').find('td:first').text().split(" ")[2];
    var dataModel = {
        OpenOfficeHoursId: $('input[name=radioRegisterOpenOfficeHours]:checked').val(),
        TimeZone
    };
    $.ajax({
        type: 'POST',
        data: { model: dataModel },
        url: '/CommunityGroupSubSections/JoinCommunityOpenHoursMeeting',
        success: function (data) {
            if (data.status) {
                //swal("", data.messsage, "success");
                $('#modelOpenHoursMeetingMessage').modal('toggle');
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
function JoinCommunityEventMeeting() {
    var dataModel = {
        OpenOfficeHoursId: $('#hdnEventId').val(), 
        TimeZone: $('#hdnTimeZone').val()
    };
    $.ajax({
        type: 'POST',
        data: { model: dataModel },
        url: '/CommunityGroupSubSections/JoinCommunityEventMeeting',
        success: function (data) {
            if (data.status) {
                //swal("", data.messsage, "success");
                $('#modelMeetingMessage').modal('toggle');
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
function AddUpdateSingleEvent() {

   
    if (formValidation('divSingleEvent') === false) {
        return false;
    }

    if (checkLiveEventValidations() === false) {
        return;
    }
    if (checkMaxAttendees() === false) {
        return false;
    }
    //OpenOfficeHours data
    var LiveEvent = {
        Id: $('#hdnCommunityGroupEventId').val(),
        CommunityGroupId: $('#hdnCommunityGroupId').val(),
        ETitle: $('#txtLiveEventsTitle').val(),
        EName: $('#txtLiveEventsName').val(),
        EDescription: $('#txtLiveEventsDescription').summernote('code'),
        EFrequency: $('#drpLiveEventsFrequency').val(),
        EFromDate: _liveEventFromDate,
        EToDate: _liveEventToDate,
        ETime: _liveEventTime,
        ETimeDayName: _liveEventDayName,
        ETimeZone: _liveEventTimeZone,
        Observers: $('#drpLiveEventsObservers').val(),
        MaxAttendees: $('#drpLiveEventsMaxAttendees').val(),
        WhoCanAttend: $('#drpLiveEventsWhoCanAttend').val(),
        Picture: $('#drpLiveEventsPicture').val(),
        Participants: $('#drpLiveEventsParticipants').val(),
        IsSingleEvent: true
    };
    //var _data=[];
    // console.log(JSON.stringify(LiveEvent));
    $.ajax({
        type: 'POST',
        //  contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: { model: LiveEvent },
        url: '../ManageCommunityGroups/SaveCommunityGroupEvent',
        //data: objectToPass,
        success: function (data) {
            if (data.status === true) {
                toastr.success(data.messsage, '', { timeOut: 1000 });
                //eventsDataTable.ajax.reload();
                $('#modelSaveSingleEvent').modal('toggle');
                
            }
            else {
                toastr.success(data.messsage, 'Warning', { timeOut: 1000 });
            }

        },
        failure: function (response) {
            //alert(response.responseText);
            toastr.success(response, 'Warning', { timeOut: 5000 });
        },
        error: function (response) {
            toastr.success(response, 'Warning', { timeOut: 5000 });
        }
    });

}