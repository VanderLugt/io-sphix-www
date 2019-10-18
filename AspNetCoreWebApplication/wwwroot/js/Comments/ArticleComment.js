function deleteArticleComment(_Id) {
    var result = confirm("Are you sure you want to delete this message?");
    if (result === false) {
        return;
    }
    var dataModel = {
        Id: _Id
    };
    $.ajax({
        type: 'POST',
        data: { model: dataModel },
        url: '/ArticleComments/deleteComment',
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
function editArticleComment(_Id) {
    var dataModel = {
        ArticleId: $('#ArticleId').val(),
        Id: _Id,
        CommentText: $('#txtArticleReply' + _Id).val()
    };
    if (dataModel.CommentText.trim().length === 0) {
        toastr.error("comment box is empty, please share you thoughts", 'Warning', { timeOut: 5000 });
        return;
    }
    $.ajax({
        type: 'POST',
        data: { model: dataModel },
        url: '/ArticleComments/editComment',
        success: function (data) {
            if (data.status) {
                //swal("", data.messsage, "success");
                toastr.success(data.messsage, '', { timeOut: 1000 });
                $('#divArticleEditCommentBox' + _Id).remove();
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
function AddArticleComment() {
    if (formValidation('ArticleComment') === false) {
        return false;
    }
    var dataModel = {
        ArticleId: $('#ArticleId').val(),
        ParentId: 0,
        CommentText: $('#txtArticleComment').val()
    };

    postArticleComment(dataModel);
    $('#txtArticleComment').val('');
    //console.log(JSON.stringify(dataModel));
  
}
function postArticleCommentReply(parentId)
{
   // alert($('#txtArticleReply'+parentId).val());
     var dataModel = {
           ArticleId: $('#ArticleId').val(),
            ParentId: parentId,
            CommentText: $('#txtArticleReply'+parentId).val()
        };
 postArticleComment(dataModel);
}
function postArticleComment(dataModel)
{
    if (dataModel.CommentText.trim().length === 0) {
        toastr.error("comment box is empty, please share you thoughts", 'Warning', { timeOut: 5000 });
        return;
    }
  $.ajax({
        type: 'POST',
        data: { model: dataModel },
        url: '/ArticleComments/addNewComment',
        success: function (data) {
            console.log(JSON.stringify(data));
            if (data.status) {
                //swal("", data.messsage, "success");
                toastr.success(data.messsage, '', { timeOut: 1000 });
                appendArticleComment(data.result);
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
function LoadArticleComments(articleId, parentId) {
    $('#' + parentId).load('/ArticleComments/LoadArticleComments?articleId=' + articleId + '&parentId=' + parentId);
}
var list = "";
function loadArticleCommentsAjax(articleId) {
    list = '';
    $('#divComments').html('loding comments..');
    $.ajax({
        type: 'POST',
        data: { articleId: articleId },
        url: '/ArticleComments/LoadArticleCommentsAsync',
        success: function (data) {
            //console.log(JSON.stringify(data));
            getNestedChildren(data, 0);
            //console.log( getNestedChildren(data, 0));
            //console.log(list);
            $("#divComments").html(list);
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
function getNestedChildren(arr, parent) {
    list += "<ul>";
    for (var i in arr) {
    
        if (arr[i].parentId === parent) {
            list += '<li><div class="comment-profile">' + arr[i].userName + '&nbsp;&nbsp;' + arr[i].commentedDate + '</div><p id="msg_' + arr[i].id + '">' + arr[i].commentText + '</p>';
            if (arr[i].isDeletedMessage === false) {
                if (arr[i].commentedById === arr[i].loggedInUserId) {
                    list += '<div id="div_' + arr[i].id + '"><i class="fa fa-comment-alt fa-sm" ></i ><input type="button" class="replycomment" onclick="addAticleCommentBox(this,' + arr[i].id + ');" value="Reply"><a class="editcomment" onclick="editAticleCommentBox(this,' + arr[i].id + ');">Edit</a><a class="editcomment" onclick="deleteArticleComment(' + arr[i].id + ');">Delete</a></div>';
                }
                else {
                    list += '<div id="div_' + arr[i].id + '"><i class="fa fa-comment-alt fa-sm" ></i > <input type="button" class="replycomment" onclick="addAticleCommentBox(this,' + arr[i].id + ');" value="Reply"></div>';
                }
            }
           
            //bind child comments
            getNestedChildren(arr, arr[i].id);

            list += '</li>';
            list += '<div id="' + arr[i].id + '"><ul></ul></div>';
        }
    }
    list += "</ul>";
}
function appendArticleComment(result) {
    var _replyDiv = '';
    //var _li = ' <li>' + result.commentText + '</li>';
    var _li = '<li><div class="comment-profile">' + result.userName + '&nbsp;&nbsp;' + result.commentedDate + '</div><p id="msg_' + result.id + '">' +result.commentText+'</p>';
    if (result.commentedById === result.loggedInUserId) {
        _replyDiv += '<div id="div_' + result.id + '"><i class="fa fa-comment-alt fa-sm" ></i ><input type="button" class="replycomment" onclick="addAticleCommentBox(this,' + result.id + ');" value="Reply"><a class="editcomment" onclick="editAticleCommentBox(this,' + result.id + ');">Edit</a><a class="editcomment" onclick="deleteArticleComment(' + result.id + ');">Delete</a></div>';
    }
    else {
        _replyDiv = '<div id="div_' + result.id + '"><i class="fa fa-comment-alt fa-sm" ></i > <input type="button" class="replycomment" onclick="addAticleCommentBox(this,' + result.id + ');" value="Reply"></div>';
    }
   

    var _nextDiv = '<div id="' + result.id + '"><ul></ul ></div >';
    if (result.parentId === 0) {
        $('#divComments').children('ul').append( _li + _replyDiv + _nextDiv);
    } else {
        $('#' + result.parentId).children('ul').append(_li + _replyDiv + _nextDiv);
        removeArtcleCommentBox(result.parentId);
    }
    //$('#' + result.parentId).append('<ul>' + _li + _replyDiv + _nextDiv + '</ul>');
    
}
function getNestedChildrenJsonBoject(arr, parent) {
    //var _button = '<div><i class="fa fa-comment-alt" ></i > <input type="button" class="replycomment" onclick="addAticleCommentBox(this,@item.Id);" value="Reply"></div>';
    var out = [];
    for (var i in arr) {

        if (arr[i].parentId === parent) {
            var children = getNestedChildren(arr, arr[i].id);
            if (children.length) {
                arr[i].children = children;
            }
            out.push(arr[i]);
        }
    }
    return out;
}