

$('#add-item-btn').click(function (){
    $.ajax({
        type: "POST",
        url: "/addanimeitem?handler=AddItem",
        headers: {
            "XSRF-TOKEN": $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        data: $('#add-item-form').serialize(),
        success: function(data) {
            uloadImage(data)
        }
    });
});


function uloadImage(itemId){
    var files = $('#file').prop("files");
    var url = "/addanimeitem?handler=UploadPhoto";
    formData = new FormData();
    formData.append("uploadedFile", files[0]);
    formData.append('itemId', itemId.toString())

    $.ajax({
        type: 'POST',
        url: url,
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (data){
            //setTimeout(function (){},5000);
            $('#userPhoto').attr("src", data)
        }
    });
}