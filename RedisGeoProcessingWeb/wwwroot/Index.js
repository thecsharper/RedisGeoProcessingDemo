// Initialize and add the map
let map;

async function initMap() {
    // The location of Uluru
    const position = { lat: -25.344, lng: 131.031 };
    // Request needed libraries.
    //@ts-ignore
    const { Map } = await google.maps.importLibrary("maps");
    const { AdvancedMarkerView } = await google.maps.importLibrary("marker");

    // The map, centered at Uluru
    map = new Map(document.getElementById("map"), {
        zoom: 4,
        center: position,
        mapId: "DEMO_MAP_ID",
    });


    new google.maps.Marker({
        position: position,
        map,
        title: "Hello World!",
    });

    // The marker, positioned at Uluru
    //const marker = new AdvancedMarkerView({
    //    map: map,
    //    position: position,
    //    title: "Uluru",
    //});


    $(document).ready(function () {
        $.ajax({
            url: "https://localhost:32419/"
        }).then(function (data) {
            $('.greeting-id').append(data.id);
            $('.greeting-content').append(data.content);
        });
    });

    var myLatlng = new google.maps.LatLng(51.454514, -2.58);
    //var mapOptions = {
    //    zoom: 4,
    //    center: myLatlng
    //}
    //var map = new google.maps.Map(document.getElementById("map"), mapOptions);

    var marker = new google.maps.Marker({
        position: myLatlng,
        title: "Hello Worldxx!"
    });

    // To add the marker to the map, call setMap();
    marker.setMap(map);
}

initMap();

