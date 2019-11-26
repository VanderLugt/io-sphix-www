function DeleteCommunityGroup(id) {
    if (confirm("Are you sure, you want to delete...?")) {
        $.ajax({
            type: 'POST',
            data: { Id: id },
            url: '/ManageCommunityGroups/DeleteCommunityGroup',
            success: function (data) {
                //alert(JSON.stringify(data));
                if (data.status) {
                    // swal("Wait", "Good", "success");
                    toastr.success(data.messsage, '', { timeOut: 1000 });
                    communityGroupsDataTable.ajax.reload();
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
    else {
        return false;
    }
}

function uploadStreamingFile() {
    var data = new FormData();
    $.each($('#fileCommunityGroupDescriptionVideo')[0].files, function (i, file) {
        data.append('file-' + i, file);
    });

    $.ajax({
        url: 'ManageCommunityGroups/UploadStreamingFile',
        data: data,
        cache: false,
        contentType: false,
        processData: false,
        method: 'POST',
        headers: { 'RequestVerificationToken': getAntiForgeryToken({}) },
        success: function (returned) {

        },
        error: function (returned) {

        }
    });
}
function getAntiForgeryToken() {
    token = $('input[name=__RequestVerificationToken]').val();
    //alert(token);
    return token;
}
function ReadyForPublish() {
    if (formValidation('mainForm') === false) {
        return false;
    }
    if ($('#hdnCommunityId').val() === '0') {
        if (checkMaxAttendees() === false) {
            return false;
        }
    }
    $('#btnSaveCommunityGroup').attr('disabled', false);
    $('#divReadyToPublish').modal('toggle');
    //divReadyToPublish
}

    //divReadyToPublish

function SaveCommunity() {
    if (formValidation('mainForm') === false) {
        return false;
    }
   

    //if (checkOpenHoursValidations() === false) {
    //    return;
    //}
    $('#divReadyToPublish').modal('hide');
        // Get the files
    var file1 = $("#fileCommunityGroupDescriptionVideo")[0].files[0];
    var ArticleShareDocument = $("#txtArticleShareDocument")[0].files[0];
    //var fileSize = file1.size; // in bytes
    var formData = new FormData();
    formData.append("file", file1);
    formData.append("articleShareDocument", ArticleShareDocument);

    formData.append("OgranizationsId", _communityId);
    formData.append("CommunityTargetedGroupId", _targetedIds);
    formData.append("AssociationId", _associations);
    //formData.append("Type1Id", $('#drpType1').val());
    //formData.append("Type2Id", $('#drpType2').val());
    formData.append("TargetedInterestIds", _interestIds);
    formData.append("IsPublicGroup", $('#chkIsPublicGroup').prop('checked'));//

    formData.append("ThemesId", _themesIds);

    formData.append("Title", $('#txtCommunityGroupTitle').val());
    formData.append("Description", $('#txtGroupDescription').summernote('code'));
    //OpenOfficeHours data
    var OpenOfficeHours = {
        "OTitle": $('#txtOpenOfficeHoursTitle').val(),
        "OName": $('#txtOpenOfficeHoursName').val(),
        "ODescription": $('#txtOpenOfficeHoursDescription').summernote('code'),
        "OFrequency": $('#drpOpenOfficeHoursFrequency').val(),
        "OFromDate": _openHoursFromDate,
        "OToDate": _openHoursToDate,
        "OTime": _openHoursTime,
        "OTimeDayName": _openHoursDayName,
        "OTimeZone": _openHoursTimeZone,
        "MaxAttendees": $('#drpOpenOfficeHoursMaxAttendees').val(),
        "WhoCanAttend": $('#drpOpenOfficeHoursWhoCanAttend').val(),
        "AddHours": $('#txtOpenOfficeAddhours').is(':checked')
    };
    formData.append("OpenOfficeHours", JSON.stringify(OpenOfficeHours));
    //LiveEvent data
    var LiveEvent = {
        "ETitle": $('#txtLiveEventsTitle').val(),
        "EName": $('#txtLiveEventsName').val(),
        "EDescription": $('#txtLiveEventsDescription').summernote('code'),
        "EFrequency": $('#drpLiveEventsFrequency').val(),
        "EFromDate": _liveEventFromDate,
        "EToDate": _liveEventToDate,
        "ETime": _liveEventTime,
        "ETimeDayName": _liveEventDayName,
        "ETimeZone": _liveEventTimeZone,
        "MaxAttendees": $('#drpLiveEventsMaxAttendees').val(),
        "Observers": $('#drpLiveEventsObservers').val(),
        "WhoCanAttend": $('#drpLiveEventsWhoCanAttend').val(),
      
        "Picture": $('#drpLiveEventsPicture').val(),
        "Participants": $('#drpLiveEventsParticipants').val()
    };
    formData.append("LiveEvent", JSON.stringify(LiveEvent));
    //Article
    var Article = {
        "ArticleTitle": $('#txtArticleTitle').val(),
        "ArticleDescription": $('#txtArticleDescription').summernote('code')
    };
    formData.append("Article", JSON.stringify(Article));
    $('#btnSaveCommunityGroup').attr('disabled', true);
    //$('#btnSaveCommunityGroup').
        // You can abort the upload by calling jqxhr.abort();    
        var jqxhr = $.ajax({
            url: "/ManageCommunityGroups/AddCommunityGroupSteps",
            type: "POST",
            contentType: false,
            data: formData,
            dataType: "json",
            headers: { 'RequestVerificationToken': getAntiForgeryToken({}) },
            cache: false,
            processData: false,
            async: false,
            xhr: function () {
                var xhr = new window.XMLHttpRequest();
                xhr.upload.addEventListener("progress",
                    function (evt) {
                      
                    },
                    false);
                return xhr;
            }
        })
            .done(function (data, textStatus, jqXhr) {
                //profilePicture alert(JSON.stringify(data));
               
                $('#btnSaveCommunityGroup').attr('disabled', false);
                if (data.status) {
                    $('#divCommunitySuccessBox').modal('toggle');
                }
                else {
                    swal("Sorry!", data.messsage, "warning");
                }
                
                //
                // Clear the input

            })
            .fail(function (jqXhr, textStatus, errorThrown) {
                console.log(JSON.stringify(jqXhr));
                $('#btnSaveCommunityGroup').attr('disabled', false);
                if (errorThrown === "abort") {
                    swal("", "Uploading was aborted", "error");

                } else {
                    //alert("Uploading failed");
                  
                   swal("", "Uploading failed", "error");
                }
            })
            .always(function (data, textStatus, jqXhr) { });
    
}
function EditCommunity() {
    if (formValidation('mainForm') === false) {
        return false;
    }

    //if (checkOpenHoursValidations() === false) {
    //    return;
    //}
    $('#divReadyToPublish').modal('hide');
    // Get the files
    var file1 = $("#fileCommunityGroupDescriptionVideo")[0].files[0];
    //var fileSize = file1.size; // in bytes
    var formData = new FormData();
    formData.append("file", file1);

    formData.append("Id", $("#hdnCommunityId").val());
    formData.append("OgranizationsId", _communityId);
    formData.append("CommunityTargetedGroupId", _targetedIds);
    formData.append("AssociationId", _associations);
    formData.append("Type1Id", $('#drpType1').val());
    formData.append("Type2Id", $('#drpType2').val());
    formData.append("TargetedInterestIds", _interestIds);
    formData.append("IsPublicGroup", $('#chkIsPublicGroup').prop('checked'));
    formData.append("DescriptionVideoUrl", _videoUrl);
    formData.append("ThemesId", _themesIds);
    formData.append("Title", $('#txtEditCommunityGroupTitle').val());
    formData.append("Description", $('#txtGroupDescription').summernote('code'));
    //OpenOfficeHours data
    var OpenOfficeHours = {
        "Id": $('#hdnOpenOfficeHoursId').val(),
        "OTitle": $('#txtOpenOfficeHoursTitle').val(),
        "OName": $('#txtOpenOfficeHoursName').val(),
        "ODescription": $('#txtOpenOfficeHoursDescription').summernote('code'),
        "OFrequency": $('#drpOpenOfficeHoursFrequency').val(),
        "OFromDate": _openHoursFromDate,
        "OToDate": _openHoursToDate,
        "OTime": _openHoursTime,
        "OTimeDayName": _openHoursDayName,
        "OTimeZone": _openHoursTimeZone,
        "MaxAttendees": $('#drpOpenOfficeHoursMaxAttendees').val(),
        "WhoCanAttend": $('#drpOpenOfficeHoursWhoCanAttend').val(),
        "AddHours": $('#txtOpenOfficeAddhours').is(':checked')
    };
    formData.append("OpenOfficeHours", JSON.stringify(OpenOfficeHours));

    // You can abort the upload by calling jqxhr.abort();    
    var jqxhr = $.ajax({
        url: "/ManageCommunityGroups/EditCommunityGroupSteps",
        type: "POST",
        contentType: false,
        data: formData,
        dataType: "json",
        headers: { 'RequestVerificationToken': getAntiForgeryToken({}) },
        cache: false,
        processData: false,
        async: false,
        xhr: function () {
            var xhr = new window.XMLHttpRequest();
            xhr.upload.addEventListener("progress",
                function (evt) {

                },
                false);
            return xhr;
        }
    })
        .done(function (data, textStatus, jqXhr) {
            //profilePicture alert(JSON.stringify(data));
            if (data.status) {
                $('#divCommunitySuccessBox').modal('toggle');
            }
            else {
                swal("Sorry!", data.messsage, "warning");
            }
        })
        .fail(function (jqXhr, textStatus, errorThrown) {

            if (errorThrown === "abort") {
                swal("", "Uploading was aborted", "error");

            } else {
                //alert("Uploading failed");

                swal("", "Uploading failed", "error");
            }
        })
        .always(function (data, textStatus, jqXhr) { });

}
function AddUpdateCommunityEvent() {
  
    if (formValidation('EventForm') === false) {
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
        MaxAttendees: $('#drpLiveEventsMaxAttendees').val(),
        Observers: $('#drpLiveEventsObservers').val(),
        WhoCanAttend: $('#drpLiveEventsWhoCanAttend').val(),
        Picture: $('#drpLiveEventsPicture').val(),
        Participants: $('#drpLiveEventsParticipants').val(),
        IsSingleEvent: false
    };
    //var _data=[];
   // console.log(JSON.stringify(LiveEvent));
    $.ajax({
        type: 'POST',
      //  contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: { model: LiveEvent},
        url: 'ManageCommunityGroups/SaveCommunityGroupEvent',
        //data: objectToPass,
        success: function (data) {
            if (data.status === true) {
                toastr.success(data.messsage, '', { timeOut: 1000 });
                eventsDataTable.ajax.reload();
                $('#updateEvent').modal('hide');
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
function AddUpdateArticle() {
    if (formValidation('ArticleForm') === false) {
        return false;
    }
    // Get the files
    var ArticleShareDocument = $("#txtArticleShareDocument")[0].files[0];
    //var fileSize = file1.size; // in bytes
    var formData = new FormData();
    formData.append("articleShareDocument", ArticleShareDocument);

    formData.append("Id", $('#hdnCommunityGroupArticleId').val());
    formData.append("CommunityGroupsId", $('#hdnCommunityGroupId').val());
    formData.append("ArticleTitle", $('#txtArticleTitle').val());
    formData.append("ArticleDescription", $('#txtArticleDescription').summernote('code'));
    
    
    //var _data=[];
    var jqxhr = $.ajax({
        url: "/ManageCommunityGroups/SaveCommunityGroupArticle",
        type: "POST",
        contentType: false,
        data: formData,
        dataType: "json",
        headers: { 'RequestVerificationToken': getAntiForgeryToken({}) },
        cache: false,
        processData: false,
        async: false,
        xhr: function () {
            var xhr = new window.XMLHttpRequest();
            xhr.upload.addEventListener("progress",
                function (evt) {

                },
                false);
            return xhr;
        }
    })
        .done(function (data, textStatus, jqXhr) {
            //profilePicture alert(JSON.stringify(data));
            if (data.status) {
                //swal("Sorry!", data.messsage, "warning");
                toastr.success(data.messsage, '', { timeOut: 1000 });
                articlesDataTable.ajax.reload();
                $('#updateArticle').modal('hide');
            }
            else {
                toastr.success(response, 'Warning', { timeOut: 5000 });
            }

            //
            // Clear the input

        })
        .fail(function (jqXhr, textStatus, errorThrown) {

            if (errorThrown === "abort") {
                swal("", "Uploading was aborted", "error");

            } else {
                //alert("Uploading failed");

                swal("", "Uploading failed", "error");
            }
        })
        .always(function (data, textStatus, jqXhr) { });
   
}
function checkOpenHoursValidations() {
    //open office hours date is not in use
    if (_openHoursFromDate === '') {
        swal("", 'Please select date for open office hours', "error");
        return false;
    }
    if (_openHoursDayName === '' && _openHoursTime === '' && _openHoursTimeZone==='') {
        swal("", 'Please select time for open office hours', "error");
        return false;
    }
}
function checkFirstStepValidations() {
  
    //alert(_targetedIds);
    $.each(selectedCommunity, function (index, value) {
        _communityId = value;
    });
    if (_communityId === 0) {
        swal("Sorry!", "Please select at least one community organizations!", "error");
        return false;
    }
    //alert($('#chkIsPublicGroup').prop('checked'));
       
            setCheckedTargetedtIds();
            setCheckedTargetedtAssociationsIds();
            setCheckedTargetedtInterestsIds();
            setCheckedThemeIds();
    
if ($('#chkIsPublicGroup').prop('checked') === false) {
    if (_targetedIds === '') {
        swal("Sorry!", "Please select at least one Targeted Community Groups!", "error");
        return false;
    }
    if (_associations === '') {
        swal("Sorry!", "Please select at least one associations!", "error");
        return false;
    }
    if (_interestIds === '') {
        swal("Sorry!", "Please select at least one interests!", "error");
        return false;
    }
}
    if (_themesIds === '') {
        swal("Sorry!", "Please select at least one Theme of community group!", "error");
        return false;
    }
    else {
        return true;
    }
}
function checkLiveEventValidations() {
    if (_liveEventFromDate === '') {
        swal("", 'Please select date for live event', "error");
        return false;
    }
    if (_liveEventDayName === '' && _liveEventTime === '' && _liveEventTimeZone === '') {
        swal("", 'Please select time for live event', "error");
        return false;
    }
}
function loadTargetedCommunityGroups() {
    //divCommunityTitle
    //alert(divCommunityTitle);
    $.each(selectedCommunity, function (index, value) {
        //alert(value);
        _communityId = value;
    });
    if (_communityId === 0) {
        swal("Sorry!", "Please select at least one community organizations!", "error");
        return;
    }

    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        data: { CommunityId: _communityId },
        url: '/ManageCommunityGroups/TargetedCommunityGroups',
        //data: objectToPass,
        success: function (data) {
            //alert(data);
            _interestsId = '';
            $('#divTargetedCommunityGroups').html('');
            $('#divTargetedCommunityGroups').html(data);
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

function loadTargetedCommunityThemes() {
    //divCommunityTitle
    //alert(divCommunityTitle);
    $.each(selectedCommunity, function (index, value) {
        //alert(value);
        _communityId = value;
    });
    if (_communityId === 0) {
        swal("Sorry!", "Please select at least one community organizations!", "error");
        return;
    }
    //alert(_communityId);
    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        data: { CommunityId: _communityId },
        url: '/ManageCommunityGroups/TargetedCommunityThemes',
        //data: objectToPass,
        success: function (data) {
            //alert(data);
            _interestsId = '';
            $('#divTargetedCommunityThemes').html('');
            $('#divTargetedCommunityThemes').html(data);
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}
function ShowStep1() {
    $('#formAddCommunityGroupStep1').show();
    $('#formAddCommunityGroupStep2').hide();
    $('#formAddCommunityGroupStep3').hide();
}
function ShowStep2() {
    if (checkFirstStepValidations()) {
        $('#formAddCommunityGroupStep1').hide();
        $('#formAddCommunityGroupStep3').hide();
        $('#formAddCommunityGroupStep2').show();
    }
}
function ShowStep3() {
   // uploadStreamingFile();
    //SaveCommunity();
    if (checkVideoDuration()) {
        $('#formAddCommunityGroupStep1').hide();
        $('#formAddCommunityGroupStep2').hide();
        $('#formAddCommunityGroupStep3').show();
    }
}
