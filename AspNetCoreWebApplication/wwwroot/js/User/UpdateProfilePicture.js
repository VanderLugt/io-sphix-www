function updateProfilePicture() {
    
    if (formValidation('updateProfilePicture') === true) {
            $('#btnUpdateProfilePicture').html('Uploading..');
            $('#btnUpdateProfilePicture').prop('disabled', true);
            // Get the files
            var file1 = $("#txtPrpfilePicture")[0].files[0];
        //var fileSize = file1.size; // in bytes
        //alert(fileSize);
            var formData = new FormData();
            formData.append("file", file1);

            // You can abort the upload by calling jqxhr.abort();    
            var jqxhr = $.ajax({
                url: "Dashboard/UpdateProfilePicture",
                type: "POST",
                contentType: false,
                data: formData,
                dataType: "json",
                cache: false,
                processData: false,
                async: false,
                xhr: function () {
                    var xhr = new window.XMLHttpRequest();
                    xhr.upload.addEventListener("progress",
                        function (evt) {
                            $('#btnUpdateProfilePicture').html('Upload');
                            $('#btnUpdateProfilePicture').prop('disabled', false);
                            if (evt.lengthComputable) {
                                var progress = Math.round((evt.loaded / evt.total) * 100);
                                console.log(progress);

                                // Do something with the progress
                            }
                        },
                        false);
                    return xhr;
                }
            })
                .done(function (data, textStatus, jqXhr) {
                    $("#txtPrpfilePicture").val('');
                    $('#btnUpdateProfilePicture').html('Upload');
                    $('#btnUpdateProfilePicture').prop('disabled', false);
                    //profilePicture alert(JSON.stringify(data));
                    $('#imgUserProfileSmall').attr('src', data.profilePicture);
                    $('#imgUserProfileBig').attr('src', data.profilePicture);
                    swal("", "Your profile picture has been successfully updated", "success");
                    // Clear the input

                })
                .fail(function (jqXhr, textStatus, errorThrown) {
                    $('#btnUpdateProfilePicture').html('Upload');
                    $('#btnUpdateProfilePicture').prop('disabled', false);

                    if (errorThrown === "abort") {
                        $("#txtPrpfilePicture").val('');
                        swal("", "Uploading was aborted", "success");

                    } else {
                        $("#txtPrpfilePicture").val('');
                        //alert("Uploading failed");
                        swal("", "Uploading failed", "success");
                    }
                })
                .always(function (data, textStatus, jqXhr) { });
        }
}