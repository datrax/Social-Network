window.addEventListener("submit", function (e) {
    var form = e.target;
    if (form.getAttribute("enctype") === "multipart/form-data") {
        if (form.dataset.ajax) {
            e.preventDefault();
            e.stopImmediatePropagation();
            var xhr = new XMLHttpRequest();
            xhr.open(form.method, form.action);
            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4 && xhr.status == 200) {
                    if (form.dataset.ajaxUpdate) {
                        var updateTarget = document.querySelector(form.dataset.ajaxUpdate);
                        if (updateTarget) {
                            updateTarget.innerHTML = xhr.responseText;
                        }
                    }
                }
            };
            xhr.send(new FormData(form));
            document.getElementById("uploadImage").value = "";
            document.getElementById("postField").value = "";
            $('.image-preview-filename').html("");
            return false;
        }
    }
}, true);
$(function () {
    $('#modalWindow').click(function () {

        $('<div/>').appendTo('body').dialog({
            position: { my: "center", at: "top+180", of: window },
            title: "Edit",
            width: 500,
            maxWidth: 500,
            resizable: false,
            closeText: "x",

            close: function (event, ui) {
                dialog.remove();


            },
            open: function (event, ui) {
                //hide close button.
                $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
            },
            modal: true
        }).load(this.href, {});

        return false;
    });
});

$(function () {
    $("#searchField").bind('input', function () {
        $("#SubmitBtn").submit();
    });
});
function DeletePost(postId) {
    $.ajax({
        type: "POST",
        url: '/Home/DeletePost/',
        data: { id: postId },
        success: function (data) {
            var div = document.getElementById("wallResult");
            var oldScrollTop = div.scrollTop;
            div.innerHTML = data;
            div.scrollTop = oldScrollTop;
            return false;
        }

    });
    return false;
};
function LikePost(postId) {
    $.ajax({
        type: "POST",
        url: '/Home/LikePost/',
        data: { id: postId },
        success: function (data) {
            var div = document.getElementById("wallResult");
            var oldScrollTop = div.scrollTop;
            div.innerHTML = data;
            div.scrollTop = oldScrollTop;
            return false;
        }

    });
    return false;
};
function GetLikes(postId) {
    clearInterval(timer);
    $.ajax({
        type: "POST",
        url: '/Home/GetLikeUsers/',
        data: { id: postId },
        success: function (data) {
            console.log(postId + "results");
            var div = document.getElementById(postId + "results");
            var oldScrollTop = div.scrollTop;
            div.innerHTML = data;
            div.scrollTop = oldScrollTop;
            return false;
        }

    });
    return false;
};
var i = 0;


var n = 0;
$(document.body).on("mouseleave", "div.qwerty", function () {
    myTimer();
    clearInterval(timer);
    timer = setInterval(myTimer, 5000);

});

$(document).on('click', '#close-preview', function () {
    $('.image-preview').popover('hide');
    // Hover befor close the preview
    $('.image-preview').hover(
        function () {
            $('.image-preview').popover('show');
        },
         function () {
             $('.image-preview').popover('hide');
         }
    );
});
$(function () {
    // Create the close button
    var closebtn = $('<button/>', {
        type: "button",
        text: 'x',
        id: 'close-preview',
        style: 'font-size: initial;',
    });
    closebtn.attr("class", "close pull-right");
    // Set the popover default content
    $('.image-preview').popover({
        trigger: 'manual',
        html: true,
        title: "<strong>Preview</strong>" + $(closebtn)[0].outerHTML,
        content: "There's no image",
        placement: 'bottom'
    });
    // Clear event
    $('.image-preview-clear').click(function () {
        $('.image-preview').attr("data-content", "").popover('hide');
        $('.image-preview-filename').html("");
        $('.image-preview-input input:file').val("");
        $(".image-preview-input-title").text("Browse");
    });

    $("#uploadImage").change(function () {
        var img = $('<img/>', {
            id: 'dynamic',
            width: 250,
            height: 200
        });
        var file = this.files[0];
        var reader = new FileReader();
        // Set preview image into the popover data-content
        reader.onload = function (e) {
            $(".image-preview-input-title").text("Change");
            $(".image-preview-clear").show();
            $(".image-preview-filename").html(file.name);
            img.attr('src', e.target.result);
            $(".image-preview").attr("data-content", $(img)[0].outerHTML).popover("show");
        }
        reader.readAsDataURL(file);
    });
});
