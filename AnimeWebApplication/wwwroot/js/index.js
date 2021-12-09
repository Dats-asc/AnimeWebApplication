$(function (){
    $('#logout-btn').click(function (){
        $.ajax({
            type: 'POST',
            url: '/logout',
            success: function (){
                alert('ok');
            }
        });
    });
});