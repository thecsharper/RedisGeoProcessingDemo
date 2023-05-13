// Initialize and add the map
let map;

async function initMap() {

    const position = { lat: 51.454514, lng: -2.58 };

    //@ts-ignore
    const { Map } = await google.maps.importLibrary("maps");

    map = new Map(document.getElementById("map"), {
        zoom: 10,
        center: position,
        mapId: "DEMO_MAP_ID",
    });

    new google.maps.Marker({
        position: position,
        map,
        title: "Hello World!",
    });

    $.getJSON("https://localhost:32419/", function (data) {
        var items = [];
        $.each(data, function (key, val) {
            items.push(val);
        });
        $(
            items.forEach(function (number) {
                new google.maps.Marker({
                    position: new google.maps.LatLng(number.position.longitude, number.position.latitude),
                    title: String(number.distance) 
                }).setMap(map);
            })
         )
    });
}

initMap();