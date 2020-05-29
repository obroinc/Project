//js for google map on home page
function init() {
    var mapOptions =
    {
        centre: new google.maps.LatLng(53.8550, 9.2879),
        mapTypeId: google.maps.MapTypeid.ROADMAP,
        zoom: 13
    };

    var venueMap;
    venueMap = new google.maps.Map(document.getElementById('map'), mapOptions);
}

function loadScript() {
    var script = document.createElement('script');
    script.src = 'https://maps.googleapis.com/maps/api/js?key=AIzaSyCbygjBKHskvkMNNt78FeDjyiuU6opvweI&callback=initMap';

    document.body.appendChild(script);

}

window.onload = loadScript;