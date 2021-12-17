$(function (){
    
    
    // редактирование
    $('#edit-btn').click(function (){
        $.ajax({
            type: "POST",
            url: "/profile?handler=ProfileChanged",
            headers: {
                "XSRF-TOKEN": $('input:hidden[name="__RequestVerificationToken"]').val()
            },
            data: $('#profile-edit-form').serialize(),
            //data: {birthday: "${birthday}", sex: "fem", city: "${city}", description: "${description}"},
            success: function() {
                alert("ok")
            }
        });
    });
    
    
    // выход из аккаунта
    $('#logout-btn').click(function (){
        $.ajax({
            type: "GET",
            url: "/logout?handler=Logout",
            headers: {
                "XSRF-TOKEN": $('input:hidden[name="__RequestVerificationToken"]').val()
            },
            success: function() {
                window.location.replace("/index");
            }
        });
    });


    // данные профиля
    $("#get-profile-btn").click(function (){
        $.ajax({
            type: "GET",
            url: "/profile?handler=UserProfile",
            headers: {
                "XSRF-TOKEN": $('input:hidden[name="__RequestVerificationToken"]').val()
            },
            success: function(data) {
                var profileData = JSON.parse(data);
                alert(profileData.City);
            }
        });
    });

    // Загрузка фотографии
    $('#file').on('change' ,function (){
        var files = $('#file').prop("files");
        var url = "/profile?handler=UploadPhoto";
        formData = new FormData();
        formData.append("uploadedFile", files[0]);

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
    });
});