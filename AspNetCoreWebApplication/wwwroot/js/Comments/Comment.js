function addAticleCommentBox(_this, _Id) {
    if ($('#divArticleCommentBox' + _Id).length)        
    {
        removeArtcleCommentBox(_Id);
        return;
    }
    var _strHtml = '<textarea class="form-control" style="height:80px" placeholder="Start the discussion..." id="txtArticleReply' + _Id + '" message="comment text is a required field." required="" spellcheck="true"  onkeyup="articleWordCount(this,' + _Id + ')"></textarea>';
    var _strButtonOne = '<div class="comment-box-btn"><button type="submit" onclick="removeArtcleCommentBox(' + _Id + ');" id="btnArticleCancel' + _Id + '"  class="btn btn-link">CANCEL</button>';
    var _strButtonTwo = '&nbsp;&nbsp;<button type="submit" onclick="postArticleCommentReply(' + _Id + ');" id="btnArticleReply' + _Id + '" disabled="" class="btn btn-primary">REPLY</button></div>';
    $(_this).closest('div').append('<div class="comment-box" id="divArticleCommentBox' + _Id + '">' + _strHtml + _strButtonOne + _strButtonTwo + '</div>');
}
function removeArtcleCommentBox(_Id) {
    $('#divArticleCommentBox' + _Id).remove();
}
function editAticleCommentBox(_this, _Id) {
    if ($('#divArticleEditCommentBox' + _Id).length) {
        removeArtcleCommentBox(_Id);
        return;
    }
    var _strHtml = '<textarea class="form-control" style="height:80px"  id="txtArticleReply' + _Id + '" message="comment text is a required field." required="" spellcheck="true"  onkeyup="articleWordCount(this,' + _Id + ')">' + $('#msg_'+_Id).html() + '</textarea>';
    var _strButtonOne = '<div class="comment-box-btn"><button type="submit" onclick="removeArticleEditCommentBox(' + _Id + ');" id="btnArticleCancel' + _Id + '"  class="btn btn-link">CANCEL</button>';
    var _strButtonTwo = '&nbsp;&nbsp;<button type="submit" onclick="editArticleComment(' + _Id + ');" id="btnArticleReply' + _Id + '"  class="btn btn-primary">SAVE EDITS</button></div>';
    $('#msg_' + _Id).hide();
    $('#msg_' + _Id).after('<div class="comment-box" id="divArticleEditCommentBox' + _Id + '">' + _strHtml + _strButtonOne + _strButtonTwo + '</div>');
}

function removeArticleEditCommentBox(_Id) {
    $('#divArticleEditCommentBox' + _Id).remove();
    $('#msg_' + _Id).show();
}
function articleWordCount(_this,_id) {
    if (_this.value.length > 0) {
        $("#btnArticleReply" + _id).removeAttr("disabled");
    }
    else {
        $("#btnArticleReply" + _id).attr("disabled", true);
    }
}