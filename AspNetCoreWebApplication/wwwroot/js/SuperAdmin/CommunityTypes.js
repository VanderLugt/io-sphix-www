function getAntiForgeryToken() {
    token = $('input[name=__RequestVerificationToken]').val();
    //alert(token);
    return token;
}
function AddUpdate() {
    if (formValidation('CommunityTypeForm') === false) {
        return false;
    }
    // Get the files
    var _file = $("#txtFile")[0].files[0];
    //var fileSize = file1.size; // in bytes
    var formData = new FormData();
    formData.append("File", _file);

    formData.append("Id", $('#hdnId').val());
    formData.append("Name", $('#txtName').val());
    formData.append("FooterLinkText", $('#txtFooterLinkText').val());
    formData.append("DisplayIndex", $('#txtDisplayIndex').val());
    formData.append("IsActive", $('#chkCheckBox').prop("checked"));
    formData.append("Description", $('#txtDescription').val());
    formData.append("Color", $('#hdnBackgroundColor').val());
    //formData.append("CommunityUrl", $('#txtCommunityUrl').val());

    //var _data=[];
    var jqxhr = $.ajax({
        url: "/SuperAdmin/CommunityTypes/SaveCommunityType",
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
                communitytypesDataTable.ajax.reload();
                $('#divReviewCommunityTypesModel').modal('hide');
            }
            else {
                toastr.error(response, 'Warning', { timeOut: 5000 });
            }

            //
            // Clear the input

        })
        .fail(function (jqXhr, textStatus, errorThrown) {
           // console.log(jqXhr);
            if (errorThrown === "abort") {
                swal("", "Uploading was aborted", "error");

            } else {
                //alert("Uploading failed");

                swal("", "Uploading failed", "error");
            }
        })
        .always(function (data, textStatus, jqXhr) { });

}