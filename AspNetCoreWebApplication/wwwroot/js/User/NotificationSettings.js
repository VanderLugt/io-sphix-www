var model;

$('#chkBlogSubscription').change(function () {
    model = {
        BlogSubscription: $('#chkBlogSubscription').prop('checked'),
        Like: $('#chkNotificationSettingsLike').prop('checked'),
        Comments: $('#chkNotificationSettingsComments').prop('checked'),
        Followis: $('#chkNotificationSettingsFollows').prop('checked')
    };
    SaveNotificationsettings();
    //console.log($('#chkNotificationSettingsLike').prop('checked'));
});

$('#chkNotificationSettingsLike').change(function () {
    model = {
        BlogSubscription: $('#chkBlogSubscription').prop('checked'),
        Like: $('#chkNotificationSettingsLike').prop('checked'),
        Comments: $('#chkNotificationSettingsComments').prop('checked'),
        Followis: $('#chkNotificationSettingsFollows').prop('checked')
    };
    SaveNotificationsettings();
    //console.log($('#chkNotificationSettingsLike').prop('checked'));
});

$('#chkNotificationSettingsComments').change(function () {
    //console.log($('#chkNotificationSettingsComments').prop('checked'));
    model = {
        BlogSubscription: $('#chkBlogSubscription').prop('checked'),
        Like: $('#chkNotificationSettingsLike').prop('checked'),
        Comments: $('#chkNotificationSettingsComments').prop('checked'),
        Followis: $('#chkNotificationSettingsFollows').prop('checked')
    };
    SaveNotificationsettings();
});
$('#chkNotificationSettingsFollows').change(function () {
  //  console.log($('#chkNotificationSettingsFollows').prop('checked'));
    model = {
        BlogSubscription: $('#chkBlogSubscription').prop('checked'),
        Like: $('#chkNotificationSettingsLike').prop('checked'),
        Comments: $('#chkNotificationSettingsComments').prop('checked'),
        Followis: $('#chkNotificationSettingsFollows').prop('checked')
    };
    SaveNotificationsettings();
});
function SaveNotificationsettings() {

    $.ajax({
        type: 'POST',
        data: { model: model },
        url: '/Dashboard/Settings',
        success: function (data) {
            //alert(JSON.stringify(data));
            if (data.status) {
                toastr.success(data.messsage, '', { timeOut: 1000 });
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

