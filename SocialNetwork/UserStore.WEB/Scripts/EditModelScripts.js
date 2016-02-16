$('input[id=base-input]').change(function () {
    $('#fake-input').val($(this).val().replace("C:\\fakepath\\", ""));

});
$('input[id=base-input2]').change(function () {
    $('#fake-input2').val($(this).val().replace("C:\\fakepath\\", ""));

});


$(document).on('click', '#close-preview2', function () {
    $('.image-preview2').popover('hide');
    // Hover befor close the preview
    $('.image-preview2').hover(
        function () {
            $('.image-preview2').popover('show');
        },
         function () {
             $('.image-preview2').popover('hide');
         }
    );
});
$(function () {
    // Create the close button
    var closebtn = $('<button/>', {
        type: "button",
        text: 'x',
        id: 'close-preview2',
        style: 'font-size: initial;',
    });
    closebtn.attr("class", "close pull-right");
    // Set the popover default content
    $('.image-preview2').popover({
        trigger: 'manual',
        html: true,
        title: "<strong>Preview</strong>" + $(closebtn)[0].outerHTML,
        content: "There's no image",
        placement: 'bottom'
    });
    // Clear event
    $('.image-preview2-clear2').click(function () {
        $('.image-preview2').attr("data-content", "").popover('hide');
        $('.image-preview2-filename2').html("");
        $('.image-preview2-input input:file').val("");
        $(".image-preview2-input-title2").text("Browse");
    });

    $("#base-input").change(function () {
        var img = $('<img/>', {
            id: 'dynamic',
            width: 250,
            height: 200
        });
        var file = this.files[0];
        var reader = new FileReader();
        // Set preview image into the popover data-content
        reader.onload = function (e) {
            $(".image-preview2-input-title2").text("Change");
            $(".image-preview2-clear2").show();
            $(".image-preview2-filename2").html(file.name);
            img.attr('src', e.target.result);
            $(".image-preview2").attr("data-content", $(img)[0].outerHTML).popover("show");
        }
        reader.readAsDataURL(file);
    });
});


$(document).on('click', '#close-preview3', function () {
    $('.image-preview3').popover('hide');
    // Hover befor close the preview
    $('.image-preview3').hover(
        function () {
            $('.image-preview3').popover('show');
        },
         function () {
             $('.image-preview3').popover('hide');
         }
    );
});
$(function () {
    // Create the close button
    var closebtn = $('<button/>', {
        type: "button",
        text: 'x',
        id: 'close-preview3',
        style: 'font-size: initial;',
    });
    closebtn.attr("class", "close pull-right");
    // Set the popover default content
    $('.image-preview3').popover({
        trigger: 'manual',
        html: true,
        title: "<strong>Preview</strong>" + $(closebtn)[0].outerHTML,
        content: "There's no image",
        placement: 'bottom'
    });
    // Clear event
    $('.image-preview3-clear3').click(function () {
        $('.image-preview3').attr("data-content", "").popover('hide');
        $('.image-preview3-filename3').html("");
        $('.image-preview3-input input:file').val("");
        $(".image-preview3-input-title3").text("Browse");
    });

    $("#base-input2").change(function () {
        var img = $('<img/>', {
            id: 'dynamic',
            width: 250,
            height: 200
        });
        var file = this.files[0];
        var reader = new FileReader();
        // Set preview image into the popover data-content
        reader.onload = function (e) {
            $(".image-preview3-input-title3").text("Change");
            $(".image-preview3-clear3").show();
            $(".image-preview3-filename3").html(file.name);
            img.attr('src', e.target.result);
            $(".image-preview3").attr("data-content", $(img)[0].outerHTML).popover("show");
        }
        reader.readAsDataURL(file);
    });
});


function addUserInfo() {
    var form = $('#myForm');
    var formData = new FormData(form[0]);
    $.ajax({
        url: '/Home/UploadImages',
        type: 'Post',
        data: formData,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.result == 'Redirect')
                window.location = data.url;
            else {
                sweetAlert(data.responseText);
                // $("#showErrorMessage").append(data.responseText);
            }
        }
    });
}
function Close() {
    $("#divparent1").hide();
}