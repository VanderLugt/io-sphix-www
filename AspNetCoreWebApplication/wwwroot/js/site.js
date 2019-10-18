var _sphixWebUrl = '';
var _assocInterests = '';
var _communityId = 0;
// Write your Javascript code.
//var idx = Math.floor((new Date().getHours()));
//var body = document.getElementsByTagName("body")[0];
//body.className = "heaven-" + idx;
$(document).ready(function () {
    //$('#divNavbarDropdownMenuLink').click(function () {
    //    $('#divNavbarDropdownMenuLink').show();
    //    return true;
    //});
   
  //  $("#divProfileMenu").load('/Menu/ProfileMenu');
});
_sphixWebUrl = document.location.origin; 

function LoadSection(path, pageTitle) {
   // window.history.pushState('page2', pageTitle, path);
    $(document).prop('title', pageTitle);
    //$("#divUserContainer").html('<span class="loader">Loading....</span >');
    $("#divContainer").load(path);
   
}
function LoadLeftSection(path, pageTitle) {
    // window.history.pushState('page2', pageTitle, path);
   // $(document).prop('title', pageTitle);
    //$("#divUserContainer").html('<span class="loader">Loading....</span >');
    $("#divLeftSection").html('');
    $("#divLeftSection").load(path); 
}

function checkMaxAttendees() {
    //console.log(_liveEventTime);
    if (_liveEventTime === '') {
        swal("Sorry!", 'Please set the event time.', "error");
        return false;
    }
    var _maxMaxAttendees = parseInt($('#drpLiveEventsMaxAttendees').val());
    var _maxParticipants = parseInt($('#drpLiveEventsParticipants').val());
    var _maxObservers = parseInt($('#drpLiveEventsObservers').val());
   // console.log(_maxMaxAttendees);
    //console.log(_maxParticipants + _maxObservers);
    if (_maxMaxAttendees !== _maxParticipants + _maxObservers) {
        //toastr.success(data.messsage, 'Warning', { timeOut: 5000 });
        swal("Sorry!", 'It looks like something is off.  The maximum number of attendees you think you will have should be equal to combination of the maximum number of participants and the number of observers.  Please adjust your numbers and try publishing again.', "error");
        return false;
    }
    else {
        return true;
    }
}
function manageAssociations(communityId) {
    _communityId = communityId;
    $('#divAssociations').html('Loading...');
    $('#divAssociations').load('/Dashboard/EditAssociations?CommunityId=' + communityId);
    $('#manageAssociations').modal('toggle');
}
function saveAssociations() {
    setCheckedInterestsIds();
 var  model = {
     EditId: $('#hndAssociationId').val(),
     GroupId: $('#drpAssocGroups').val(),
     AssociationId: $('#drpAssocAssociation').val(),
     Type1Id: $('#drpAssocType1').val(),
     Type2Id: $('#drpAssocType2').val(),
     InterestIds: _assocInterests,
     CommunityId: _communityId
    };
    $.ajax({
        type: 'POST',
        data: { model: model },
        url: '/Dashboard/SaveAssociations',
        success: function (data) {
            //alert(JSON.stringify(data));
            if (data.status) {
                $('#manageAssociations').modal('toggle');
                $('.modal-backdrop').remove();
                toastr.success(data.messsage, '', { timeOut: 1000 });
                LoadLeftSection('Dashboard/CommunityAssociations');
            }
            else {
                toastr.success(data.messsage, 'Warning', { timeOut: 5000 });
            }
        },
        failure: function (data) {
            toastr.success(data.messsage, 'Warning', { timeOut: 5000 });
        },
        error: function (data) {
            toastr.success(data.messsage, 'Warning', { timeOut: 5000 });
        }
    });
}
