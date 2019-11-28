var _latitude;
var _longitude;
var _communityId = 0;
var _interestsId = '';
function SignUp() {
   
    if (formValidation('formSinup1') === true) {
      //
        if (_communityId === 0) {
            swal("Sorry!", "Please select at least one community organizations!", "error");
            return;
        }
        if ($('#checkAgree3').prop('checked') === false) {
            $('#checkAgree3').next("span").css("border", "solid 1px red");
            swal("Sorry!", "Please check Terms and conditions checkbox!", "error");
            return;
        }
        else {
            $('#checkAgree3').next("span").css("border", "");
        }

        if ($('#checkAgree1').prop('checked') === false && $('#checkAgree2').prop('checked') === false) {
            //checkAgree1
            $('#checkAgree1').next("span").css("border", "solid 1px red");
            $('#checkAgree2').next("span").css("border", "solid 1px red");
            swal("Sorry!", "Please check at least one checkbox!", "error");
            return;
        }
        else {
            $('#checkAgree1').next("span").css("border", "");
            $('#checkAgree2').next("span").css("border", "");
        }
        

        var modelData = {
            FirstName: $('#FirstName').val(),
            LastName: $('#LastName').val(),
            Email: $('#Email').val(),
            Password: $('#Password').val(),
            PhoneNumber: $('#PhoneNumber').val(),
            CommunityId: _communityId,
            GroupId: $('#drpGroups').val(),
            AssociationId: $('#drpAssociation').val(),
            //Type1ListId: $('#drpType1').val(),
            //Type2ListId: $('#drpType2').val(),
            InterestsId: _interestsId,
            IsKickstartarActive: $('#checkAgree1').prop('checked'),
            IsFinanciallyActive: $('#checkAgree2').prop('checked'),
            IsCertifyTAndC: $('#checkAgree3').prop('checked'),
            Latitude: _latitude,
            Longitude: _longitude
        };
        //console.log(JSON.stringify(modelData));
        $.ajax({
            type: 'POST',
            data: { model: modelData },
            url: '/Home/Register',
            success: function (data) {
                //alert(JSON.stringify(data));
                if (data.status) {
                    swal("Thanks", data.messsage, "success");
                }
                else {
                    swal("Sorry!", data.messsage, "info");
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
$("#SignUpStep1").click(function () {
    var passwordStrength = checkStrength($('#Password').val());
    //alert($('#checkShareMyLocation').prop("checked"));
    var errormessage = '';
    if (formValidation('formSinup1') === true) {

        if ($('#Password').val() !== $('#ConformPassword').val()) {
            errormessage = 'Your password and re-confirm password do not match!' + '\n';
            
        }
        //if ($('#checkShareMyLocation').prop("checked")) {
        //    errormessage = 'Your password and re-confirm password do not match!' + '\n';
        //}
        if (passwordStrength === 'Weak' || passwordStrength === 'Too short') {
            errormessage = errormessage + "Password must contain at least six characters, including uppercase, lowercase letters and numbers. \n";
           
        }
        if (errormessage !== '') {
            swal("Sorry!", errormessage, "error");
            return;
        }
        //formSinup2

        $('html, body').animate({
            scrollTop: $("#formSinup2").offset().top
        }, 1200);
        //$('#formSinup1').hide();
        //$('#formSinup2').show();
        
    }
});

$("#SignUpStep2").click(function () {
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
    $('#divCommunityTitle').html('Customize Your '+ CommunityTitle+' Community Associations');
   

    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        data: { CommunityId: _communityId},
        url: '/Home/SignUpStep3',
        //data: objectToPass,
        success: function (data) {
            //alert(data);
            _interestsId = '';
            $('#divLoadStep3').html(data);
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
    //$('#formSinup1').hide();
    //$('#formSinup2').hide();
    //$('#formSinup3').show();
    $('html, body').animate({
        scrollTop: $("#formSinup3").offset().top
    }, 1200);

});

$("#SignUpStep3").click(function () {
    $("input:checkbox[class=chkInterests]:checked").each(function () {
       // alert("Id: " + $(this).attr("id") + " Value: " + $(this).val());
        _interestsId = _interestsId + $(this).attr("id") + ','; 
    });
    if (_interestsId === '') {
        swal("Sorry!", "Please, select at least one interest!", "error");
        return;
    }
    //$('#formSinup1').hide();
    //$('#formSinup2').hide();
    //$('#formSinup3').hide();
    //$('#formSinup4').show();
    $('html, body').animate({
        scrollTop: $("#formSinup4").offset().top
    }, 1200);
    //if (formValidation('formSinup1') === true) {

    //}
});

$("#BackToStep1").click(function () {
    $('html, body').animate({
        scrollTop: $("#formSinup1").offset().top
    }, 1200);
});
$("#BackToStep2").click(function () {
    $('html, body').animate({
        scrollTop: $("#formSinup2").offset().top
    }, 1200);
});
$("#BackToStep3").click(function () {

   
    //$('#formSinup1').hide();
    //$('#formSinup2').hide();
    //$('#formSinup3').show();
    //$('#formSinup4').hide();
    $('html, body').animate({
        scrollTop: $("#formSinup3").offset().top
    }, 1200);
});

$('#checkShareMyLocation').change(function () {
    if ($(this).prop("checked") === true) {
        navigator.geolocation.getCurrentPosition(function (position, html5Error) {
            geo_loc = processGeolocationResult(position);
            currLatLong = geo_loc.split(",");
            _latitude = currLatLong[0];
            _longitude = currLatLong[1];
            //console.log(currLatLong[0] + ' | ' + currLatLong[1]);
            //initializeCurrent(currLatLong[0], currLatLong[1]);
        });
    }
  
});
function processGeolocationResult(position) {

    html5Lat = position.coords.latitude;
    html5Lon = position.coords.longitude;
   // html5TimeStamp = position.timestamp;
   // html5Accuracy = position.coords.accuracy;
    return (html5Lat).toFixe+d(8) + ", " + (html5Lon).toFixed(8);
}

$('#brnVerificationStep1').click(function () {
    $('#verificationStep1').hide();
    $('#verificationStep2').show();
   
});
$('#brnVerificationStep1Back').click(function () {
    $('#verificationStep1').show();
    $('#verificationStep2').hide();
   
});