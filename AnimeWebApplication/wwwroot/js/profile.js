$(function (){
    
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
});