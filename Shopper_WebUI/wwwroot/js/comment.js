var productid = -1;
var CommentBodyId = "#comment";


function myFunction(smallImg) {
    var fullImg = document.getElementById("imageBox")
    fullImg.src = smallImg.src;
}
$(document).ready(function () {
    (function () {
        var url = $("#comment").data("url");
        $("#comment").load(url)
        productid = $("#comment").data("product-id");
    })()
});

function doComment(btn, e, commentid, spanid) {
    var button = $(btn)
    var mode = button.data("edit-mode");
    if (e === "edit_clicked") {
        if (!mode) {
            button.data("edit-mode", true);
            button.removeClass("btn-warning");
            button.addClass("btn-success");
            var btnSpan = button.find("span");
            btnSpan.removeClass("fa-edit")
            btnSpan.addClass("fa-check")

            $(spanid).attr("contenteditable", true);
        }
        else {
            button.data("edit-mode", false);
            button.removeClass("btn-success");
            button.addClass("btn-warning");
            var btnSpan = button.find("span");
            btnSpan.removeClass("fa-check")
            btnSpan.addClass("fa-edit")

            $(spanid).attr("contenteditable", false);

            var txt = $(spanid).text();

            $.ajax({
                method: "POST",
                url: "/Comment/Edit/",
                data: { "text": txt, "id": commentid }
            }).done(function (data) {
                if (data.result) {
                    $(CommentBodyId).load("/Comment/ShowProductComments?id=" + productid)
                }
                else {
                    alert("Yorum Güncellenemedi.")
                }
            }).fail(function () {
                alert("Sunucu ile Bağlantı Kurulamadı.")
            })
        }
    }
    else if (e === "delete_clicked") {
        var dialog_res = confirm("Yorum Silinsin mi?")
        if (!dialog_res) return false;

        $.ajax({
            method: "GET",
            url: "/Comment/Delete?id=" + commentid
        }).done(function (data) {
            if (data.result) {
                $(CommentBodyId).load("/Comment/ShowProductComments?id=" + productid)
            }
            else {
                alert("Yorum Silinemedi.")
            }
        }).fail(function () {
            alert("Sunucu ile Bağlantı Kurulamadı.")
        })
    }
    else if (e === "new_clicked") {
        var txt = $("#new_comment_text").val();
      
        $.ajax({
            method: "POST",
            url: "/Comment/Create/",
            data: { "text": txt, "productid": productid }
        }).done(function (data) {
            if (data.result) {
                $(CommentBodyId).load("/Comment/ShowProductComments?id=" + productid)
            }
            else {
                alert("Yorum Eklenemedi.")
            }
        }).fail(function () {
            alert("Sunucu ile Bağlantı Kurulamadı.")
        })
    }
}