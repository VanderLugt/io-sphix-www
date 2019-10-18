$('#btnLogin').click(function () {
    if (formValidation('formLogin') === true) {
       
        var loginModel = {
            UserName: $('#txtUserName').val(),
            Password: $('#txtPassword').val()
        };
       
        $.ajax({
            type: 'POST',
            data: { model: loginModel },
            url: '/Home/Login?returnUrl',
            success: function (data) {
                //alert(JSON.stringify(data));
                if (data.status) {
                    var returnUrl = getUrlParameter('returnUrl');
                   // swal("Wait", "Good", "success");
                    if (returnUrl === '') {
                        window.location.href = "/CommunitiesForGood";
                    }
                    else {
                        window.location.href = returnUrl;
                    }
                    
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
});
function getUrlParameter(name) {
    name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
    var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
    var results = regex.exec(location.search);
   // alert(results);
    return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
};
$('#btnForgotLink').click(function () {
    if (formValidation('formForgotPassword') === true) {

        var loginModel = {
            UserName: $('#txtFogotPasswordEmail').val()
        };

        $.ajax({
            type: 'POST',
            data: { model: loginModel },
            url: '/Home/SendForgotPasswordLink',
            success: function (data) {
                //alert(JSON.stringify(data));
                if (data.status) {
                    swal("Thanks", data.messsage, "success");
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
});
$('#btnResetPassword').click(function () {

    if (formValidation('fromRestPasword') === true) {
        if ($('#Password').val() !== $('#ConformPassword').val()) {
            swal("Sorry!", 'Your password and re-confirm password do not match!', "error");
            return;
        }

        var loginModel = {
            UserName: $('#hdnEmailAddress').val(),
            Password: $('#Password').val(),
            Token: $('#hdnToken').val()
        };

        $.ajax({
            type: 'POST',
            data: { model: loginModel },
            url: '/Shx/ChangePassword',
            success: function (data) {
                //alert(JSON.stringify(data));
                if (data.status) {
                    //swal("Thanks", data.messsage, "success");
                    swal("Thanks", data.messsage, "success")
                        .then((value) => {
                            window.location.href = "/";
                        });
                   
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
});