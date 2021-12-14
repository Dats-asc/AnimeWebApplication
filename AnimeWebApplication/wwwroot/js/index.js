$('#getUsers').click(function (){
    $.ajax({
        type: "GET",
        url: "/index?handler=Users",
        success: function(data) {
            var parsedData = JSON.parse(data);
            parsedData.users.forEach(user => printUser(user));
        }
    });

    function printUser(user) {
        // var newElement = document.createElement("p");
        // newElement.innerHTML = user.Id.toString();
        // document.getElementById("test").appendChild(newElement);
        
        var card = document.createElement("div");
        card.setAttribute('class', 'card')
        card.setAttribute('style', 'width: 18rem;')
        var cardBody = document.createElement("div");
        cardBody.setAttribute('class', 'card-body')
        var cardTitle = document.createElement("h5");
        cardTitle.setAttribute('class', 'card-title')
        cardTitle.innerHTML = user.Username;
        var cardText = document.createElement("p");
        cardText.setAttribute('class', 'card-text')
        cardText.innerHTML = user.Password;
        cardText.setAttribute('style', 'background-color = red;')
        
        var cardImgTop = document.createElement("img");
        cardImgTop.setAttribute('class', 'card-img-top')
        cardImgTop.setAttribute('src', 'https://martialartsplusinc.com/wp-content/uploads/2017/04/default-image-620x600.jpg')
        
        cardBody.appendChild(cardTitle);
        cardBody.appendChild(cardText);
        
        card.appendChild(cardImgTop)
        card.appendChild(cardBody);
        
        document.getElementById("content").appendChild(card);
    };
});
