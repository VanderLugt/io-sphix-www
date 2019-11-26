function addEventCommentBox(_this, _Id) {
    if ($('#divEventCommentBox' + _Id).length) {
        removeEventCommentBox(_Id);
        return;
    }
    var _strHtml = '<textarea class="form-control" style="height:80px" placeholder="Start the discussion..." id="txtEventReply' + _Id + '" message="comment text is a required field." required="" spellcheck="true"  onkeyup="eventWordCount(this,' + _Id + ')"></textarea>';
    var _strButtonOne = '<div class="comment-box-btn"><button type="submit" onclick="removeEventCommentBox(' + _Id + ');" id="btnEventCancel' + _Id + '"  class="btn btn-link">CANCEL</button>';
    var _strButtonTwo = '&nbsp;&nbsp;<button type="submit" onclick="postEventCommentReply(' + _Id + ');" id="btnEventReply' + _Id + '" disabled="" class="btn btn-primary">REPLY</button></div>';
    $(_this).closest('div').append('<div class="comment-box" id="divEventCommentBox' + _Id + '">' + _strHtml + _strButtonOne + _strButtonTwo + '</div>');
}
function removeEventCommentBox(_Id) {
    $('#divEventCommentBox' + _Id).remove();
}
function editEventCommentBox(_this, _Id) {
    if ($('#divEventEditCommentBox' + _Id).length) {
        removeEventCommentBox(_Id);
        return;
    }
    var _strHtml = '<textarea class="form-control" style="height:80px"  id="txtEventReply' + _Id + '" message="comment text is a required field." required="" spellcheck="true"  onkeyup="eventWordCount(this,' + _Id + ')">' + $('#msg_' + _Id).html() + '</textarea>';
    var _strButtonOne = '<div class="comment-box-btn"><button type="submit" onclick="removeArticleEditCommentBox(' + _Id + ');" id="btnEventCancel' + _Id + '"  class="btn btn-link">CANCEL</button>';
    var _strButtonTwo = '&nbsp;&nbsp;<button type="submit" onclick="editEventComment(' + _Id + ');" id="btnEventReply' + _Id + '"  class="btn btn-primary">SAVE EDITS</button></div>';
    $('#msg_' + _Id).hide();
    $('#msg_' + _Id).after('<div class="comment-box" id="divEventEditCommentBox' + _Id + '">' + _strHtml + _strButtonOne + _strButtonTwo + '</div>');
}

function removeEventEditCommentBox(_Id) {
    $('#divEventEditCommentBox' + _Id).remove();
    $('#msg_' + _Id).show();
}
function eventWordCount(_this, _id) {
    if (_this.value.length > 0) {
        $("#btnEventReply" + _id).removeAttr("disabled");
    }
    else {
        $("#btnEventReply" + _id).attr("disabled", true);
    }
}

//************************Main Comment***************************
function deleteEventComment(_Id) {
    var result = confirm("Are you sure, you want to delete this message?");
    if (result === false) {
        return;
    }
    var dataModel = {
        Id: _Id
    };
    $.ajax({
        type: 'POST',
        data: { model: dataModel },
        url: '/EventComments/deleteComment',
        success: function (data) {
            if (data.status) {
                toastr.success(data.messsage, '', { timeOut: 1000 });
                $('#msg_' + _Id).html('<strike>message is deleted</strike>');
                $('#div_' + _Id).remove();
            }
            else {
                toastr.success(data.messsage, 'Warning', { timeOut: 5000 });
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
function editEventComment(_Id) {
    var dataModel = {
        EventId: $('#hdnEventId').val(),
        Id: _Id,
        CommentText: $('#txtEventReply' + _Id).val()
    };
    if (dataModel.CommentText.trim().length === 0) {
        toastr.error("comment box is empty, please share you thoughts", 'Warning', { timeOut: 5000 });
        return;
    }
    $.ajax({
        type: 'POST',
        data: { model: dataModel },
        url: '/EventComments/editComment',
        success: function (data) {
            if (data.status) {
                //swal("", data.messsage, "success");
                toastr.success(data.messsage, '', { timeOut: 1000 });
                $('#divEventEditCommentBox' + _Id).remove();
                $('#msg_' + _Id).html(dataModel.CommentText);
                $('#msg_' + _Id).show();

            }
            else {
                //swal("Sorry!", data.messsage, "warning");
                toastr.success(data.messsage, 'Warning', { timeOut: 5000 });
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
function AddEventComment() {
    if (formValidation('EventComment') === false) {
        return false;
    }
    var dataModel = {
        EventId: $('#hdnEventId').val(),
        ParentId: 0,
        CommentText: $('#txtEventComment').val()
    };

    postEventComment(dataModel);
    $('#txtEventComment').val('');

}
function postEventCommentReply(parentId) {
    var dataModel = {
        EventId: $('#hdnEventId').val(),
        ParentId: parentId,
        CommentText: $('#txtEventReply' + parentId).val()
    };
    postEventComment(dataModel);
}
function postEventComment(dataModel) {
    if (dataModel.CommentText.trim().length === 0) {
        toastr.error("comment box is empty, please share you thoughts", 'Warning', { timeOut: 5000 });
        return;
    }
    $.ajax({
        type: 'POST',
        data: { model: dataModel },
        url: '/EventComments/addNewComment',
        success: function (data) {
            console.log(JSON.stringify(data));
            if (data.status) {
                toastr.success(data.messsage, '', { timeOut: 1000 });
                appendEventComment(data.result);
            }
            else {
                toastr.success(data.messsage, 'Warning', { timeOut: 5000 });
            }
        },
        failure: function (response) {
            swal("Sorry!", response.responseText, "error");
        },
        error: function (response) {
            swal("Sorry!", response.responseText, "error");
        }

    });
}
function LoadEventComments(EventId, parentId) {
    $('#' + parentId).load('/EventComments/LoadEventComments?EventId=' + EventId + '&parentId=' + parentId);
}
var eventlistComment = "";
function loadEventCommentsAjax(EventId) {
    eventlistComment = '';
    $('#divEventComments').html('loding comments..');
    $.ajax({
        type: 'POST',
        data: { eventId: EventId },
        url: '/EventComments/LoadEventCommentsAsync',
        success: function (data) {
            getNestedChildrenForEventComments(data, 0);
            $("#divEventComments").html(eventlistComment);
        },
        failure: function (response) {
            swal("Sorry!", response.responseText, "error");
        },
        error: function (response) {
            swal("Sorry!", response.responseText, "error");
        }

    });
}
function getNestedChildrenForEventComments(arr, parent) {
    eventlistComment += "<ul>";
    for (var i in arr) {

        if (arr[i].parentId === parent) {
            eventlistComment += '<li><div class="comment-profile">' + arr[i].userName + '&nbsp;&nbsp;' + arr[i].commentedDate + '</div><p id="msg_' + arr[i].id + '">' + arr[i].commentText + '</p>';
            if (arr[i].isDeletedMessage === false) {
                if (arr[i].commentedById === arr[i].loggedInUserId) {
                    eventlistComment += '<div id="div_' + arr[i].id + '"><i class="fa fa-comment-alt fa-sm" ></i ><input type="button" class="replycomment" onclick="addEventCommentBox(this,' + arr[i].id + ');" value="Reply"><a class="editcomment" onclick="editEventCommentBox(this,' + arr[i].id + ');">Edit</a><a class="editcomment" onclick="deleteEventComment(' + arr[i].id + ');">Delete</a></div>';
                }
                else {
                    eventlistComment += '<div id="div_' + arr[i].id + '"><i class="fa fa-comment-alt fa-sm" ></i > <input type="button" class="replycomment" onclick="addEventCommentBox(this,' + arr[i].id + ');" value="Reply"></div>';
                }
            }

            //bind child comments
            getNestedChildrenForEventComments(arr, arr[i].id);

            eventlistComment += '</li>';
            eventlistComment += '<div id="' + arr[i].id + '"><ul></ul></div>';
        }
    }
    eventlistComment += "</ul>";
}
function appendEventComment(result) {
    var _replyDiv = '';
    //var _li = ' <li>' + result.commentText + '</li>';
    var _li = '<li><div class="comment-profile">' + result.userName + '&nbsp;&nbsp;' + result.commentedDate + '</div><p id="msg_' + result.id + '">' + result.commentText + '</p>';
    if (result.commentedById === result.loggedInUserId) {
        _replyDiv += '<div id="div_' + result.id + '"><i class="fa fa-comment-alt fa-sm" ></i ><input type="button" class="replycomment" onclick="addEventCommentBox(this,' + result.id + ');" value="Reply"><a class="editcomment" onclick="editEventCommentBox(this,' + result.id + ');">Edit</a><a class="editcomment" onclick="deleteEventComment(' + result.id + ');">Delete</a></div>';
    }
    else {
        _replyDiv = '<div id="div_' + result.id + '"><i class="fa fa-comment-alt fa-sm" ></i > <input type="button" class="replycomment" onclick="addEventCommentBox(this,' + result.id + ');" value="Reply"></div>';
    }


    var _nextDiv = '<div id="' + result.id + '"><ul></ul ></div >';
    if (result.parentId === 0) {
        $('#divEventComments').children('ul').append(_li + _replyDiv + _nextDiv);
    } else {
        $('#' + result.parentId).children('ul').append(_li + _replyDiv + _nextDiv);
        removeEventCommentBox(result.parentId);
    }
    //$('#' + result.parentId).append('<ul>' + _li + _replyDiv + _nextDiv + '</ul>');

}
