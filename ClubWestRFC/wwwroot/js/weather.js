



$('btn').click(function () {
    $.getJSON("http://api.openweathermap.org/data/2.5/weather?q=castlebar&units=metric&appid=2fa6123cf4b9294239a28ef16033c218",

        function (data) {

            console.log(data);

            var icon = "http://api.openweathermap.org/img/w/" + data.weather[0].icon + ".png";

            var temp = Math.floor(data.main.temp);
            var weather = data.weather[0].main;


            $(".icon").attr("src", icon);
            $(".weather").append(weather);
            $(".temp").append(temp);


        });


});



